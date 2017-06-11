
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using System.Collections.Concurrent;


namespace Iocp
{
    public class TcpConnect : NetConnection
    {
        private TcpServer server;
        private SocketAsyncEventArgs mIOReceiveEventArgs;
        private SocketAsyncEventArgs mIOSendEventArgs;
        private ThreadSafeQueue<byte[]> mSendQueue;
        private int isInSending;

        #region 对外接口
        public bool IsValidTcpConnect()
        {
            return WorkSocket != null;
        }

        Iocp.NetState mState = Iocp.NetState.Invalid;
        public NetState State
        {
            get
            {
                return mState;
            }
            set
            {
                mState = value;
            }
        }

        public int Port
        {
            get { return RemoteEndPoint.Port; }
        }

        public string IpAddress
        {
            get { return RemoteEndPoint.Address.ToString(); }
        }

        public override void SendBuffer(byte[] data, int offset, int count)
        {
            if (State == NetState.Connect )
                this.server.PostSend(this, data, offset, count);
        }

        public override void Disconnect()
        {
            if (this.server == null)
            {//说明已经关过了
                Log.Log.Net.Info("Disconnect if (this.server == null)");
                return;
            }

            this.server.CloseSocket(this);
            //this.server.Raise_CloseConnect(this);
        }

        #endregion


        public TcpConnect(TcpServer server, SocketAsyncEventArgs acceptEventArgs)
        {
            HashCode = Guid.NewGuid();
            this.server = server;

            server.GetIOArgPool().TryPop(out mIOReceiveEventArgs);
            mIOReceiveEventArgs.AcceptSocket = acceptEventArgs.AcceptSocket;
            var dataToken1 = (AsyncUserToken)mIOReceiveEventArgs.UserToken;
            mIOReceiveEventArgs.SetBuffer(dataToken1.bufferOffset, server.socketSettings.BufferSize);
            dataToken1.tcpConn = this;
            
            server.GetIOArgPool().TryPop(out mIOSendEventArgs);
            mIOSendEventArgs.AcceptSocket = acceptEventArgs.AcceptSocket;
            var dataToken2 = (AsyncUserToken)mIOSendEventArgs.UserToken;
            mIOSendEventArgs.SetBuffer(dataToken2.bufferOffset, server.socketSettings.BufferSize);
            dataToken2.tcpConn = this;

            mSendQueue = new ThreadSafeQueue<byte[]>();

            mState = NetState.Invalid;
        }

        ~TcpConnect()
        {
            Close();
        }

        public Guid HashCode { get; private set; }

        public SocketAsyncEventArgs ReceiveEventArgs { get { return mIOReceiveEventArgs; } }
        public SocketAsyncEventArgs SendEventArgs { get { return mIOSendEventArgs; } }

        public bool Connected 
        { 
            get 
            {
                if (mIOReceiveEventArgs.AcceptSocket!=null)
                    return mIOReceiveEventArgs.AcceptSocket.Connected;
                return false;
            } 
        }

        internal Socket WorkSocket 
        { 
            get 
            {
                if (mIOReceiveEventArgs == null)
                    return null;
                return mIOReceiveEventArgs.AcceptSocket; 
            } 
        }

        public IPEndPoint RemoteEndPoint 
        {
            get 
            {
                if (mIOReceiveEventArgs == null)
                    return null;
                if (mIOReceiveEventArgs.AcceptSocket == null)
                    return null;
                return (IPEndPoint)mIOReceiveEventArgs.AcceptSocket.RemoteEndPoint;
            } 
        }
        
        public int QueueLength 
        {
            get { return mSendQueue.Count; } 
        }

        internal void Close()
        {
            if (mIOSendEventArgs == null || mIOSendEventArgs == null)
                return;

            try
            {
                var socket = WorkSocket;
                if (socket != null)
                {
                    WorkSocket.Shutdown(SocketShutdown.Both);
                    WorkSocket.Close();
                }

                var dataToken0 = (AsyncUserToken)mIOSendEventArgs.UserToken;
                if (dataToken0 != null)
                {
                    dataToken0.Reset(true);
                    dataToken0.tcpConn = null;
                }
                var dataToken1 = (AsyncUserToken)mIOReceiveEventArgs.UserToken;
                if (dataToken1 != null)
                {
                    dataToken1.Reset(true);
                    dataToken1.tcpConn = null;
                }
            }
            catch(Exception)
            {

            }
            finally 
            {
                mIOSendEventArgs.AcceptSocket = null;
                server.GetIOArgPool().Push(mIOSendEventArgs);
                mIOSendEventArgs = null;

                mIOReceiveEventArgs.AcceptSocket = null;
                server.GetIOArgPool().Push(mIOReceiveEventArgs);
                mIOReceiveEventArgs = null;

                this.server = null;
                mState = NetState.Disconnect;
            }
        }

        internal void Enqueue(byte[] data)
        {
            mSendQueue.Enqueue(data);
        }
        internal bool TryDequeue(out byte[] result)
        {
            return mSendQueue.TryDequeue(out result);
        }
        internal bool TrySetSendFlag()
        {
            return Interlocked.CompareExchange(ref isInSending, 1, 0) == 0;
        }
        internal void ResetSendFlag()
        {
            Interlocked.Exchange(ref isInSending, 0);
        }
    }
}