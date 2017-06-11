
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Generic;
//using System.Collections.Concurrent;

namespace Iocp
{
    public class ThreadSafeQueue<T>
    {
        Queue<T> mQueueData = new Queue<T>();

        public int Count
        {
            get 
            { 
                lock(this)
                    return mQueueData.Count; 
            }
        }
        public bool IsEmpty
        {
            get 
            {
                lock (this) 
                    return mQueueData.Count == 0;
            } 
        }

        public void Clear()
        {
            lock (this)
            {
                mQueueData.Clear();
            }
        }

        public T Dequeue()
        {
            lock (this)
            {
                return mQueueData.Dequeue();
            }
        }

        public void Enqueue(T item)
        {
            lock (this)
            {
                mQueueData.Enqueue(item);
            }
        }

        public bool TryDequeue(out T result)
        {
            lock (this)
            {
                if (mQueueData.Count == 0)
                {
                    result = default(T);
                    return false;
                }
                result = mQueueData.Dequeue();
                return true;
            }
        }
    }

    public class ThreadSafeStack<T>
    {
        Stack<T> mStackData = new Stack<T>();

        public int Count 
        {
            get
            {
                lock (this)
                    return mStackData.Count;
            }
        }

        public void Push(T item)
        {
            lock (this)
            {
                mStackData.Push(item);
            }
        }

        public bool TryPop(out T result)
        {
            lock (this)
            {
                if (mStackData.Count == 0)
                {
                    result = default(T);
                    return false;
                }
                result = mStackData.Pop();
                return true;
            }
        }
    }

    public delegate void NETCLIENT_EVENT(TcpClient pClient, byte[] pData, int nLength);

    public class TcpClient : NetConnection
    {
        #region NETCLIENT_EVENTs
        public event NETCLIENT_EVENT NewConnect;    //TODO:还要这个吗？
        public event NETCLIENT_EVENT CloseConnect;
        public event NETCLIENT_EVENT ReceiveData;
        #endregion

        Socket socket;
        int bufferSize;
        SocketAsyncEventArgs sendEventArg;
        AsyncUserToken sendDataToken;
        SocketAsyncEventArgs receiveEventArg;
        AsyncUserToken receiveDataToken;
        ThreadSafeQueue<byte[]> sendQueue = new ThreadSafeQueue<byte[]>();
        ThreadSafeQueue<byte[]> recvQueue = new ThreadSafeQueue<byte[]>();
        int isInSending;

        public bool Connected { get { return mState == NetState.Connect; } }
        
        public TcpClient(int _bufferSize = 65535)
        {
            this.bufferSize = _bufferSize;
        }

        string mHostIp;
        int mPort;

        public void Connect(string strHostIp, int nPort)
        {
            if (string.IsNullOrEmpty(strHostIp))
                return;

            mHostIp = strHostIp;
            mPort = nPort;

            try
            {
                IPEndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Parse(strHostIp), nPort);
                socket = new Socket(RemoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                mState = NetState.Accept;
                socket.Connect(RemoteEndPoint);
            }
            catch (System.Exception)
            {
                mState = NetState.Invalid;
                if (NewConnect!=null)
                    NewConnect(this, null, 0);
                return;            	
            }
            mState = NetState.Connect;

            var buffer = new byte[this.bufferSize * 2];
            this.sendEventArg = new SocketAsyncEventArgs();
            this.sendEventArg.SetBuffer(buffer, 0, this.bufferSize);
            this.sendDataToken = new AsyncUserToken();
            this.sendDataToken.bufferOffset = this.sendEventArg.Offset;
            this.sendEventArg.UserToken = this.sendDataToken;
            this.sendEventArg.AcceptSocket = socket;
            this.sendEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);

            this.receiveEventArg = new SocketAsyncEventArgs();
            this.receiveEventArg.SetBuffer(buffer, this.bufferSize, this.bufferSize);
            this.receiveDataToken = new AsyncUserToken();
            this.receiveDataToken.bufferOffset = this.receiveEventArg.Offset;
            this.receiveEventArg.UserToken = this.receiveDataToken;
            this.receiveEventArg.AcceptSocket = socket;
            this.receiveEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            if (NewConnect!=null)
                NewConnect(this, null, 1);

            PostReceive();
        }

        public override void SendBuffer(byte[] data, int offset, int count)
        {
            if (!Connected)
            {
                return;
                //throw new ObjectDisposedException("socket");
            }

            int P = NetPacketParser.PREFIX_SIZE;
            byte[] buffer = new byte[count + P];
            Buffer.BlockCopy(BitConverter.GetBytes((UInt16)count), 0, buffer, 0, P);
            Buffer.BlockCopy(data, offset, buffer, P, count);
            Queue<byte> aa = new Queue<byte>();
            sendQueue.Enqueue(buffer);
            if (TrySetSendFlag())
            {
                try
                {
                    TryDequeueAndPostSend();
                }
                catch
                {
                    ResetSendFlag();
                    throw;
                }
            }
        }

        public void Update()
        {            
            while (!this.recvQueue.IsEmpty)
            {
                byte[] msg;
                this.recvQueue.TryDequeue(out msg);
                try
                {
                    ReceiveData(this, msg, msg.Length);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("ReceiveData: {0}", ex);
                }
            }

            if (socket != null)
            {
                if (socket.Connected == false)
                {
                    if (CloseConnect != null && mState!=NetState.Disconnect)
                    {
                        mState = NetState.Disconnect;
                        CloseConnect(this, null, 0);
                    }
                }
            }
        }

        public override void Disconnect()
        {
            HandleCloseSocket();
            mState = NetState.Disconnect;
        }

        //---
        private void SendBuffer()
        {
            var dataToken = this.sendDataToken;
            var ioEventArg = this.sendEventArg;

            if (dataToken.messageLength - dataToken.messageBytesDone <= this.bufferSize)
            {
                ioEventArg.SetBuffer(dataToken.bufferOffset, dataToken.messageLength - dataToken.messageBytesDone);
                Buffer.BlockCopy(dataToken.messageBytes, dataToken.messageBytesDone, ioEventArg.Buffer, dataToken.bufferOffset, dataToken.messageLength - dataToken.messageBytesDone);
            }
            else
            {
                this.sendEventArg.SetBuffer(dataToken.bufferOffset, this.bufferSize);
                Buffer.BlockCopy(dataToken.messageBytes, dataToken.messageBytesDone, ioEventArg.Buffer, dataToken.bufferOffset, this.bufferSize);
            }

            bool willRaiseEvent = this.socket.SendAsync(this.sendEventArg);
            if (!willRaiseEvent)
            {
                ProcessSend();
            }
        }


        private void PostReceive()
        {
            bool willRaiseEvent = socket.ReceiveAsync(receiveEventArg);

            if (!willRaiseEvent)
            {
                ProcessReceive();
            }
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                AsyncUserToken ioDataToken = (AsyncUserToken)e.UserToken;

                switch (e.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        ProcessReceive();
                        break;
                    case SocketAsyncOperation.Send:
                        ProcessSend();
                        break;

                    default:
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                Trace.TraceError("IO_Completed: {0}", ex);
            }
        }

        private void ProcessReceive()
        {
            var dataToken = this.receiveDataToken;
            var ioEventArg = this.receiveEventArg;
            if (ioEventArg.SocketError != SocketError.Success)
            {
                //Socket错误
                HandleCloseSocket();
                return;
            }

            if (ioEventArg.BytesTransferred == 0)
            {
                //远端主动关闭socket
                HandleCloseSocket();
                return;
            }

            #region 消息包解析
            int remainingBytesToProcess = ioEventArg.BytesTransferred;
            bool needPostAnother = true;
            do
            {
                if (dataToken.prefixBytesDone < NetPacketParser.PREFIX_SIZE)
                {
                    remainingBytesToProcess = NetPacketParser.HandlePrefix(ioEventArg, dataToken, remainingBytesToProcess);
                    if (dataToken.IsPrefixReady && (dataToken.messageLength > 65535 || dataToken.messageLength <= 0))
                    {
                        //消息头已接收完毕，并且接收到的消息长度大于64K，socket传输的数据已紊乱，关闭掉
                        Trace.TraceWarning("接收到的消息长度错误:{0}", dataToken.messageLength);
                        needPostAnother = false;
                        HandleCloseSocket();
                        break;
                    }
                    if (remainingBytesToProcess == 0)
                        break;
                }

                remainingBytesToProcess = NetPacketParser.HandleMessage(ioEventArg, dataToken, remainingBytesToProcess);

                if (dataToken.IsMessageReady)
                {
                    recvQueue.Enqueue(dataToken.messageBytes);
                    if (remainingBytesToProcess != 0)
                    {
                        dataToken.Reset(false);
                    }
                }
            } while (remainingBytesToProcess != 0);
            #endregion

            if (needPostAnother)
            {
                if (dataToken.IsPrefixReady && dataToken.IsMessageReady)
                {
                    dataToken.Reset(true);
                }
                dataToken.bufferSkip = 0;
                PostReceive();
            }
        }

        private bool TrySetSendFlag()
        {
            return Interlocked.CompareExchange(ref isInSending, 1, 0) == 0;
        }
        private void ResetSendFlag()
        {
            Interlocked.Exchange(ref isInSending, 0);
        }

        private void TryDequeueAndPostSend()
        {
            byte[] data;
            if (sendQueue.TryDequeue(out data))
            {
                AsyncUserToken dataToken = sendDataToken;
                dataToken.messageBytes = data;
                dataToken.messageLength = data.Length;
                SendBuffer();
            }
            else
            {
                ResetSendFlag();
            }
        }
        
        private void ProcessSend()
        {
            var dataToken = this.sendDataToken;
            var ioEventArg = this.sendEventArg;

            if (ioEventArg.SocketError == SocketError.Success)
            {
                dataToken.messageBytesDone += ioEventArg.BytesTransferred;
                if (dataToken.messageBytesDone != dataToken.messageLength)
                {
                    SendBuffer();
                }
                else
                {
                    dataToken.Reset(true);
                    try
                    {
                        TryDequeueAndPostSend();
                    }
                    catch
                    {
                        ResetSendFlag();
                        throw;
                    }
                }
            }
            else
            {
                ResetSendFlag();
                HandleCloseSocket();
            }
        }

        private object syncRoot = new object();

        //add by gosky
        public void Close()
        {
            Disconnect();
        }

        public void Reconnect()
        {
            Connect(mHostIp, mPort);
        }

        Iocp.NetState mState = Iocp.NetState.Invalid;
        public Iocp.NetState State
        {
            get { return mState; }
        }           

        //add end
        

        private void HandleCloseSocket()
        {
            if (Connected)
            {
                lock (syncRoot)
                {
                    if (Connected)
                    {
                        mState = NetState.Disconnect;
                        try
                        {
                            socket.Shutdown(SocketShutdown.Both);
                            try
                            {
                                if (CloseConnect!=null)
                                    CloseConnect(this, null, 0);
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceError("OnDisconnected: {0}", ex);
                            }
                        }
                        catch { }

                        socket.Close();
                    }
                }
            }
        }
    }
}