
using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Generic;
//using System.Collections.Concurrent;

namespace Iocp
{
    public delegate void NETSERVER_EVENT(TcpConnect pConnect, TcpServer pServer, byte[] pData, int nLength);

    /// <summary>
    /// <remarks>
    /// 参考MSDN代码： http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socketasynceventargs(v=vs.110).aspx
    /// </remarks>
    /// </summary>
    public class TcpServer
    {
        /// <summary>
        /// 事件回调，在Update()的调用线程中触发
        /// </summary>
        #region NETSERVER_EVENTs
        public event NETSERVER_EVENT ReceiveData;
        public event NETSERVER_EVENT NewConnect;
        public event NETSERVER_EVENT CloseConnect;

        struct RecvEventArg
        {
            public TcpConnect connect;
            public byte[] msg;
            public long ReceiveTime;

            public RecvEventArg(TcpConnect con, byte[] m)
            {   
                connect = con;
                msg = new byte[m.Length];
                System.Buffer.BlockCopy(m, 0, msg, 0, m.Length);
                ReceiveTime = 0;
            }
        }

        ThreadSafeQueue<RecvEventArg> recvQueue = new ThreadSafeQueue<RecvEventArg>();
        ThreadSafeQueue<TcpConnect> newConnectQueue = new ThreadSafeQueue<TcpConnect>();
        //HashSet<TcpConnect> closeConnectQueue = new HashSet<TcpConnect>();
        Dictionary<TcpConnect, TcpConnect> closeConnectQueue = new Dictionary<TcpConnect, TcpConnect>();

        bool TestLag = false;
        #endregion

        #region 成员变量
        BufferManager bufferManager;
        Socket listenSocket;
        //HashSet<TcpConnect> clientSockets = new HashSet<TcpConnect>();
        Dictionary<TcpConnect, TcpConnect> clientSockets = new Dictionary<TcpConnect, TcpConnect>();
        public Semaphore maxConnectionsEnforcer;
        public TcpOption socketSettings;
        ThreadSafeStack<SocketAsyncEventArgs> acceptEventArgsPool;
        ThreadSafeStack<SocketAsyncEventArgs> ioEventArgsPool;
        private bool _isStart;

        public ThreadSafeStack<SocketAsyncEventArgs> GetAcceptArgPool()
        {
            return acceptEventArgsPool;
        }
        public ThreadSafeStack<SocketAsyncEventArgs> GetIOArgPool()
        {
            return ioEventArgsPool;
        }
        public int GetConnectingCount()
        {
            return clientSockets.Count;
        }
        #endregion

        #region Public接口
        public bool Open(TcpOption socketSettings,int nPort)
        {
            return Open(socketSettings, nPort, null);
        }

        public bool Open(TcpOption socketSettings,int nPort, string strBindIp)
        {
            this.socketSettings = socketSettings;
            this.ioEventArgsPool = new ThreadSafeStack<SocketAsyncEventArgs>();
            this.acceptEventArgsPool = new ThreadSafeStack<SocketAsyncEventArgs>();
            this.maxConnectionsEnforcer = new Semaphore(this.socketSettings.MaxConnections, this.socketSettings.MaxConnections);

            if (this.bufferManager == null)
            {
                this.bufferManager = new BufferManager(this.socketSettings.BufferSize * this.socketSettings.NumOfSaeaForRecSend, this.socketSettings.BufferSize);
                Initialize();
            }
            
            //-- 解析IP地址
            IPEndPoint localEndPoint;
            if (strBindIp != null)
                localEndPoint = new IPEndPoint(IPAddress.Parse(strBindIp), nPort);
            else
                localEndPoint = new IPEndPoint(IPAddress.Any, nPort);

            //-- 创建Socket
            listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            listenSocket.Bind(localEndPoint);
            listenSocket.Listen(nPort);
            _isStart = true;

            //-- 开始Accept请求
            PostAccept();

            return true;
        }

        public void Update()
        {
            //-- 派发新建链接事件
            while (!this.newConnectQueue.IsEmpty)
            {
                TcpConnect con;
                this.newConnectQueue.TryDequeue(out con);
                con.mLimitLevel = LimitLevel;
                con.State = NetState.Connect;
                if (NewConnect!=null)
                    NewConnect(con, this, null, 1);
            }

            //-- 派发接收数据事件
            while (!this.recvQueue.IsEmpty)
            {
                RecvEventArg evtArg;
                this.recvQueue.TryDequeue(out evtArg);
                try
                {
                    ReceiveData(evtArg.connect, this, evtArg.msg, evtArg.msg.Length);
                    if (TestLag)
                    {
                        var nt = System.DateTime.Now.Ticks;// IDllImportAPI.HighPrecision_GetTickCount();
                        if (nt - evtArg.ReceiveTime > 10000)
                        {
                            Log.Log.Net.Info("RecvMsg Time = {0}", nt - evtArg.ReceiveTime);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Log.Net.Error("ReceiveData error:{0}", ex);
                }
            }

            //-- 派发链接关闭事件
            lock (this)
            {
                foreach(var i in closeConnectQueue)
                {
                    try
                    {
                        i.Value.State = NetState.Disconnect;
                        if (CloseConnect != null)
                            CloseConnect(i.Value, this, null, 0);

                        i.Value.Close();
                        maxConnectionsEnforcer.Release();
                    }
                    catch (System.Exception ex)
                    {
                        Log.FileLog.WriteLine(ex.ToString());
                        Log.FileLog.WriteLine(ex.StackTrace.ToString());
                    }
                }
                closeConnectQueue.Clear();
            }
        }

        public void Raise_CloseConnect(TcpConnect pConnect)
        {
            pConnect.State = NetState.Disconnect;
            if (CloseConnect != null)
                CloseConnect(pConnect, this, null, 0);
        }

        public void Close()
        {
            _isStart = false;
            lock (this)
            {
                foreach (var socket in clientSockets)
                {
                    if (socket.Value.WorkSocket!=null)
                        socket.Value.WorkSocket.Shutdown(SocketShutdown.Both);
                }
            }

            while (clientSockets.Count != 0)
                Thread.Sleep(10);

            DisposeAllSaeaObjects();
            if (listenSocket!=null)
                listenSocket.Close();
        }

        public TcpServer()
        {
            //this.socketSettings = socketSettings;

            //this.bufferManager = new BufferManager(this.socketSettings.BufferSize * this.socketSettings.NumOfSaeaForRecSend, this.socketSettings.BufferSize);

        }

        //public TcpServer():this(TcpOption.ForServer){}

        #endregion

        private void Initialize()
        {
            //-- 初始化BufferManager
            this.bufferManager.InitBuffer();

            //-- 创建Accept池
            for (int i = 0; i < this.socketSettings.MaxAcceptOps; i++)
            {
                this.acceptEventArgsPool.Push(CreateAcceptEventArgs());
            }

            //-- 创建IO池
            SocketAsyncEventArgs ioEventArgs;
            for (int i = 0; i < this.socketSettings.NumOfSaeaForRecSend; i++)
            {
                ioEventArgs = new SocketAsyncEventArgs();
                this.bufferManager.SetBuffer(ioEventArgs);
                ioEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                AsyncUserToken dataToken = new AsyncUserToken();
                dataToken.bufferOffset = ioEventArgs.Offset;
                ioEventArgs.UserToken = dataToken;
                this.ioEventArgsPool.Push(ioEventArgs);
            }
        } 
        
        private SocketAsyncEventArgs CreateAcceptEventArgs()
        {
            SocketAsyncEventArgs acceptEventArg = new SocketAsyncEventArgs();
            acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Completed);
            return acceptEventArg;
        }

        private void PostAccept()
        {
            SocketAsyncEventArgs acceptEventArgs;
            if (this.acceptEventArgsPool.Count > 1)
            {
                try
                {
                    this.acceptEventArgsPool.TryPop(out acceptEventArgs);
                }
                catch
                {
                    acceptEventArgs = CreateAcceptEventArgs();
                }
            }
            else
            {
                acceptEventArgs = CreateAcceptEventArgs();
            }

            this.maxConnectionsEnforcer.WaitOne();
            if (!_isStart)
            {
                return;
            }
            bool willRaiseEvent = listenSocket.AcceptAsync(acceptEventArgs);
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArgs);
            }
        }

        private void Accept_Completed(object sender, SocketAsyncEventArgs acceptEventArgs)
        {
            try
            {
                ProcessAccept(acceptEventArgs);
            }
            catch (Exception ex)
            {
                Trace.TraceError("AcceptCompleted method error: {0}", ex);

                if (acceptEventArgs.AcceptSocket != null)
                {
                    acceptEventArgs.AcceptSocket.Close();
                    acceptEventArgs.AcceptSocket = null;
                }
                acceptEventArgsPool.Push(acceptEventArgs);
                maxConnectionsEnforcer.Release();
            }
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs ioEventArgs)
        {
            try
            {
                AsyncUserToken ioDataToken = (AsyncUserToken)ioEventArgs.UserToken;
                switch (ioEventArgs.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        ProcessReceive(ioEventArgs);
                        break;
                    case SocketAsyncOperation.Send:
                        ProcessSend(ioEventArgs);
                        break;
                    case SocketAsyncOperation.Disconnect:
                        HandleCloseSocket(ioEventArgs);
                        break;
                    default:
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
            catch (ObjectDisposedException error)
            {
                Log.Log.Net.Info("IO_Completed error: {0}", error);
                //ReleaseIOEventArgs(ioEventArgs);
                HandleCloseSocket(ioEventArgs);
            }
            catch (Exception ex)
            {
                Log.Log.Net.Info("IO_Completed unkown error: {0}", ex);
                HandleCloseSocket(ioEventArgs);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            PostAccept();

            if (acceptEventArgs.SocketError != SocketError.Success)
            {
                HandleBadAccept(acceptEventArgs);
                return;
            }
            
            var newConn = new TcpConnect(this, acceptEventArgs);
            acceptEventArgs.AcceptSocket = null;
            this.acceptEventArgsPool.Push(acceptEventArgs);

            lock (this)
            {
                if (clientSockets.ContainsKey(newConn) == false)
                {
                    clientSockets.Add(newConn, newConn);
                }
                else
                {
                    Log.Log.Net.Info("怎么可能？clientSockets.ContainsKey(newConn) == false");
                }
                newConnectQueue.Enqueue(newConn);
            }

            PostReceive(newConn.ReceiveEventArgs);
        }
        
        /// <summary>
        /// 投递接收数据请求
        /// </summary>
        private void PostReceive(SocketAsyncEventArgs ioEventArgs)
        {
            bool willRaiseEvent = ioEventArgs.AcceptSocket.ReceiveAsync(ioEventArgs);

            if (!willRaiseEvent)
            {
                ProcessReceive(ioEventArgs);
            }
        }

        /// <summary>
        /// 处理数据接收回调
        /// </summary>
        private void ProcessReceive(SocketAsyncEventArgs ioEventArgs)
        {
            AsyncUserToken dataToken = (AsyncUserToken)ioEventArgs.UserToken;
            if (ioEventArgs.SocketError != SocketError.Success)
            {
                //Socket错误
                //Trace.TraceError("ProcessReceive:{0}", ioEventArgs.SocketError);
                
                HandleCloseSocket(ioEventArgs);
                return;
            }

            if (ioEventArgs.BytesTransferred == 0)
            {
                HandleCloseSocket(ioEventArgs);
                return;
            }

            var tcpConn = dataToken.tcpConn;

            #region 消息包解析
            int remainingBytesToProcess = ioEventArgs.BytesTransferred;
            //bool needPostAnother = true;
            do
            {
                if (dataToken.prefixBytesDone < NetPacketParser.PREFIX_SIZE)
                {
                    remainingBytesToProcess = NetPacketParser.HandlePrefix(ioEventArgs, dataToken, remainingBytesToProcess);
                    if (dataToken.IsPrefixReady && (dataToken.messageLength > 65535 || dataToken.messageLength <= 0))
                    {
                        //消息头已接收完毕，并且接收到的消息长度已经超长，socket传输的数据已紊乱，关闭掉
                        Log.FileLog.WriteLine("{0} Receive Ip {2} message length error:{1}",DateTime.Now.ToString("HH:mm:ss"), dataToken.messageLength, ioEventArgs.RemoteEndPoint);
                        //needPostAnother = false;
                        HandleCloseSocket(ioEventArgs);
                        return;
                    }
                    if (remainingBytesToProcess == 0) 
                        break;
                }

                remainingBytesToProcess = NetPacketParser.HandleMessage(ioEventArgs, dataToken, remainingBytesToProcess);

                if (dataToken.IsMessageReady)
                {
                    RecvEventArg evtArg = new RecvEventArg(dataToken.tcpConn, dataToken.messageBytes);
                    if(TestLag)
                        evtArg.ReceiveTime = System.DateTime.Now.Ticks; //IDllImportAPI.HighPrecision_GetTickCount();

                    recvQueue.Enqueue(evtArg);
                    if (remainingBytesToProcess != 0)
                    {
                        dataToken.Reset(false);
                    }
                }
                else
                {
                    //if (logger.IsDebugEnabled) logger.Debug("不完整封包 长度[{0}],总传输[{1}],已接收[{2}]", dataToken.messageLength, ioEventArgs.BytesTransferred, dataToken.messageBytesDone);
                }
            } while (remainingBytesToProcess != 0);
            #endregion
                        
            //if (needPostAnother)
            {
                //继续处理下个请求包
                if (dataToken.IsPrefixReady && dataToken.IsMessageReady)
                {
                    dataToken.Reset(true);
                }
                dataToken.bufferSkip = 0;
                PostReceive(ioEventArgs);
            }
        }

        private void TryDequeueAndPostSend(TcpConnect tcpConn)
        {
            byte[] data;
            if (tcpConn.TryDequeue(out data))
            {
                AsyncUserToken dataToken = (AsyncUserToken)tcpConn.SendEventArgs.UserToken;
                dataToken.messageBytes = data;
                dataToken.messageLength = data.Length;
                if (TestLag)
                    dataToken.SendTime = System.DateTime.Now.Ticks; // IDllImportAPI.HighPrecision_GetTickCount();
                try
                {
                    PostSend(tcpConn.SendEventArgs);
                }
                catch(System.Exception ex)
                {
                    Log.Log.Net.Info(ex.ToString());
                    Log.Log.Net.Info(ex.StackTrace.ToString());
                    //发送发生了异常，那么这个连接都关闭算了。
                    HandleCloseSocket(tcpConn.ReceiveEventArgs);
                }
            }
            else
            {
                tcpConn.ResetSendFlag();
            }
        }
        /// <summary>
        /// Posts the send.
        /// </summary>
        public void PostSend(TcpConnect tcpConn, byte[] data, int offset, int count)
        {
            int P = NetPacketParser.PREFIX_SIZE;
            byte[] buffer = new byte[count + P];
            Buffer.BlockCopy(BitConverter.GetBytes((UInt16)count), 0, buffer, 0, P);
            Buffer.BlockCopy(data, offset, buffer, P, count);
            tcpConn.Enqueue(buffer);
            if (tcpConn.TrySetSendFlag())
            {
                try
                {
                    AsyncUserToken dataToken = (AsyncUserToken)tcpConn.SendEventArgs.UserToken;
                    dataToken.Reset(true);

                    TryDequeueAndPostSend(tcpConn);
                }
                catch
                {
                    tcpConn.ResetSendFlag();
                    throw;
                }
            }
        }

        private void PostSend(SocketAsyncEventArgs ioEventArgs)
        {
            AsyncUserToken dataToken = (AsyncUserToken)ioEventArgs.UserToken;
            if (dataToken.messageLength - dataToken.messageBytesDone <= this.socketSettings.BufferSize)
            {
                ioEventArgs.SetBuffer(dataToken.bufferOffset, dataToken.messageLength - dataToken.messageBytesDone);
                Buffer.BlockCopy(dataToken.messageBytes, dataToken.messageBytesDone, ioEventArgs.Buffer, dataToken.bufferOffset, dataToken.messageLength - dataToken.messageBytesDone);
            }
            else
            {
                ioEventArgs.SetBuffer(dataToken.bufferOffset, this.socketSettings.BufferSize);
                Buffer.BlockCopy(dataToken.messageBytes, dataToken.messageBytesDone, ioEventArgs.Buffer, dataToken.bufferOffset, this.socketSettings.BufferSize);
            }

            var willRaiseEvent = ioEventArgs.AcceptSocket.SendAsync(ioEventArgs);
            if (!willRaiseEvent)
            {
                ProcessSend(ioEventArgs); 
            }
        }

        private void ProcessSend(SocketAsyncEventArgs ioEventArgs)
        {
            AsyncUserToken dataToken = (AsyncUserToken)ioEventArgs.UserToken;
            if (dataToken == null)
            {
                Log.Log.Net.Info("ProcessSend dataToken==null");
                return;
            }
            else
            {
                if (dataToken.tcpConn == null)
                {
                    Log.Log.Net.Info("ProcessSend dataToken.tcpConn==null");
                    return;
                }
                else
                {
                    if (dataToken.tcpConn.SendEventArgs != ioEventArgs)
                    {
                        Log.Log.Net.Info("ProcessSend dataToken.tcpConn.SendEventArgs != ioEventArgs");
                        return;
                    }
                }
            }
            if (ioEventArgs.SocketError == SocketError.Success)
            {
                dataToken.messageBytesDone += ioEventArgs.BytesTransferred;
                if (dataToken.messageBytesDone != dataToken.messageLength)
                {
                    PostSend(ioEventArgs);
                }
                else
                {
                    if (TestLag)
                    {
                        var nt = System.DateTime.Now.Ticks; //IDllImportAPI.HighPrecision_GetTickCount();
                        if ((nt - dataToken.SendTime) > 5000)
                        {
                            Log.Log.Net.Info("Send waste {0}", nt - dataToken.SendTime);
                        }
                    }

                    dataToken.Reset(true);
                    try
                    {
                        TryDequeueAndPostSend(dataToken.tcpConn);
                    }
                    catch
                    {
                        dataToken.tcpConn.ResetSendFlag();
                        throw;
                    }
                }
            }
            else
            {
                //Log.FileLog.WriteLine("ProcessSend ioEventArgs.SocketError");
                dataToken.tcpConn.ResetSendFlag();
                HandleCloseSocket(ioEventArgs);
            }
        }

        public void CloseSocket(TcpConnect tcpConn)
        {
            HandleCloseSocket(tcpConn.ReceiveEventArgs);
            //tcpConn.Close();
        }

        private void HandleCloseSocket(SocketAsyncEventArgs ioEventArgs)
        {
            lock (this)
            {
                if (ioEventArgs == null)
                {
                    Log.Log.Net.Info("HandleCloseSocket ioEventArgs==null");
                    return;
                }
                TcpConnect connect = null;
                var token = ioEventArgs.UserToken as AsyncUserToken;
                if(token==null)
                {
                    Log.Log.Net.Info("HandleCloseSocket ioEventArgs.UserToken==null");
                }
                else
                {
                    if(token.tcpConn == null)
                    {
                        return;
                        //Log.FileLog.WriteLine("HandleCloseSocket token.tcpConn==null");
                    }
                    else
                    {
                        connect = token.tcpConn;
                    }
                }
                if (connect == null)
                {
                    Log.Log.Net.Info("HandleCloseSocket connect==null");
                    try
                    {
                        if (ioEventArgs.AcceptSocket != null)
                        {
                            ioEventArgs.AcceptSocket.Shutdown(SocketShutdown.Both);
                            ioEventArgs.AcceptSocket.Close();
                        }
                        else
                        {
                            //TcpConnect已经Close了
                            Log.Log.Net.Info("HandleCloseSocket ioEventArgs.AcceptSocket==null");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Log.Log.Net.Info(ex.ToString());
                        Log.Log.Net.Info(ex.StackTrace.ToString());
                    }
                    finally
                    {
                        ioEventArgs.AcceptSocket = null;
                    }
                }
                else
                {
                    try
                    {
                        if (closeConnectQueue.ContainsKey(connect))
                        {
                            clientSockets.Remove(connect);
                            return;
                            //Log.FileLog.WriteLine("HandleCloseSocket closeConnectQueue.ContainsKey(connect)");
                        }

                        TcpConnect conn;
                        if (clientSockets.TryGetValue(connect, out conn))
                        {
                            clientSockets.Remove(connect);
                        }
                        else
                        {
                            Log.Log.Net.Info("Error! HandleCloseSocket clientSockets找不到connect");
                        }

                        closeConnectQueue[connect] = connect;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Log.Net.Info(ex.ToString());
                        Log.Log.Net.Info(ex.StackTrace.ToString());
                    }
                }
            }
        }

        private void HandleBadAccept(SocketAsyncEventArgs acceptEventArgs)
        {//Accept的时候网络出错，做一下清理工作
            ResetSAEAObject(acceptEventArgs);
            acceptEventArgs.AcceptSocket = null;
            acceptEventArgsPool.Push(acceptEventArgs);
            maxConnectionsEnforcer.Release();
        }
        
        private void DisposeAllSaeaObjects()
        {
            if (this.acceptEventArgsPool == null)
            {
                return;
            }

            SocketAsyncEventArgs eventArgs;
            while (this.acceptEventArgsPool.Count > 0)
            {
                acceptEventArgsPool.TryPop(out eventArgs);
                ResetSAEAObject(eventArgs);
            }
            while (this.ioEventArgsPool.Count > 0)
            {
                ioEventArgsPool.TryPop(out eventArgs);
                ResetSAEAObject(eventArgs);
            }
        }

        private static void ResetSAEAObject(SocketAsyncEventArgs eventArgs)
        {
            try
            {
                if (eventArgs.AcceptSocket != null)
                {
                    eventArgs.AcceptSocket.Shutdown(SocketShutdown.Both);
                    eventArgs.AcceptSocket.Close();
                }
            }
            catch(Exception ex)
            {
                Log.Log.Net.Info(ex.ToString());
                Log.Log.Net.Info(ex.StackTrace.ToString());
            }
            finally
            {
                eventArgs.AcceptSocket = null;
            }
        }

        //add by gosky
        public int LimitLevel = (int)RPC.RPCExecuteLimitLevel.Developer;
        public TcpOption Opt
        {
            get { return socketSettings; }
            //set { socketSettings = value; }
        }
        //end add
    }
}