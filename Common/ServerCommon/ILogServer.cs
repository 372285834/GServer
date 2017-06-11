using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon
{

    public enum LogServerState
    {
        None,
        WaitRegServer,
        Working,
    }

    [ServerCommon.Editor.CDataEditorAttribute(".logsrv")]
    public class ILogServerParameter
    {
        string mDateBaseIP = "127.0.0.1";
        [ServerFrame.Config.DataValueAttribute("DateBaseIP")]
        public string DateBaseIP
        {
            get { return mDateBaseIP; }
            set { mDateBaseIP = value; }
        }
        string mRegServerIP = "127.0.0.1";
        [ServerFrame.Config.DataValueAttribute("RegServerIP")]
        public string RegServerIP
        {
            get { return mRegServerIP; }
            set { mRegServerIP = value; }
        }
        UInt16 mRegServerPort = 8888;
        [ServerFrame.Config.DataValueAttribute("RegServerPort")]
        public UInt16 RegServerPort
        {
            get { return mRegServerPort; }
            set { mRegServerPort = value; }
        }

        string mListenIP = "127.0.0.1";
        [ServerFrame.Config.DataValueAttribute("ListenIP")]
        public string ListenIP
        {
            get { return mListenIP; }
            set { mListenIP = value; }
        }
        UInt16 mListenPort = 23000;
        [ServerFrame.Config.DataValueAttribute("ListenPort")]
        public UInt16 ListenPort
        {
            get { return mListenPort; }
            set { mListenPort = value; }
        }
        ulong mServerId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Server);
        [ServerFrame.Config.DataValueAttribute("ServerId")]
        public ulong ServerId
        {
            get { return mServerId; }
            set { mServerId = value; }
        }
    }

    [RPC.RPCClassAttribute(typeof(ILogServer))]
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    class ILogServer : RPC.RPCObject
    {
        #region RPC必须的基础定义
        public static RPC.RPCClassInfo smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
        {
            return smRpcClassInfo;
        }
        #endregion

        #region 核心数据
        protected Iocp.TcpServer mTcpSrv = new Iocp.TcpServer();
        protected Iocp.TcpClient mRegisterConnect = new Iocp.TcpClient(65535);

        LogServerState mLinkState = LogServerState.None;
        public LogServerState LinkState
        {
            get { return mLinkState; }
        }

        ILogServerParameter mParameter;

        ServerFrame.DB.DBConnect mDBConnect;
        public ServerFrame.DB.DBConnect DBConnect
        {
            get { return mDBConnect; }
        }

        #endregion

        #region 总操作
        public void Start(ILogServerParameter parameter)
        {
            Stop();

            Log.FileLog.Instance.Begin("LogServer.log", false);

            Log.Log.Common.Print("LogServer Start!");
            Log.FileLog.Instance.Flush();
            
            mDBConnect = new ServerFrame.DB.DBConnect();

            mDBConnect.OpenConnect();

            Log.Log.Common.Print("DBConnect OK!");

            mParameter = parameter;
            mTcpSrv.ReceiveData += RPC.RPCNetworkMgr.Instance.ServerReceiveData;
            mTcpSrv.CloseConnect += this.ServerDisConnected;
            mRegisterConnect.ReceiveData += RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mRegisterConnect.NewConnect += this.OnRegisterConnected;

            mRegisterConnect.Connect(parameter.RegServerIP, parameter.RegServerPort);
            mLinkState = LogServerState.WaitRegServer;
        }

        public void Stop()
        {
            if (mDBConnect != null)
            {
                System.Windows.Forms.MessageBox.Show("数据服务器关闭！");
                mDBConnect.CloseConnect();
                mDBConnect = null;
            }

            mTcpSrv.ReceiveData -= RPC.RPCNetworkMgr.Instance.ServerReceiveData;
            mTcpSrv.CloseConnect -= this.ServerDisConnected;
            mRegisterConnect.ReceiveData -= RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mRegisterConnect.NewConnect -= this.OnRegisterConnected;

            mRegisterConnect.Close();
            mTcpSrv.Close();
            System.Diagnostics.Debug.WriteLine("数据服务器关闭");
            mLinkState = LogServerState.None;

            Log.FileLog.Instance.End();
        }

        Int64 mTryRegServerReconnectTime;
        public void Tick()
        {
            IServer.Instance.Tick();
            var time = IServer.timeGetTime();
            if (mDBConnect != null && mDBConnect.IsValidConnect() == false)
            {
                //这里发生数据断开的情况了！
                Log.Log.Common.Print("要命，数据库断开了！");
                //System.Diagnostics.Debug.Assert(false);                
                mDBConnect.ReOpen();
                return;
            }

            if (mRegisterConnect.State != Iocp.NetState.Connect)
            {
                if (time - mTryRegServerReconnectTime > 3000)
                {
                    mTryRegServerReconnectTime = time;
                    mRegisterConnect.Reconnect();
                }
            }
            mRegisterConnect.Update();
            mTcpSrv.Update();
            RPC.RPCNetworkMgr.Instance.Tick(IServer.Instance.GetElapseMilliSecondTime());
        }
        #endregion

        #region 服务器各种注册流程
        string SelectLogServerIP(string[] ips)
        {
            string result = ips[0];
            foreach (string s in ips)
            {
                if (s.IndexOf("192.168.") == 0)
                {//选择局域网内部的
                    return s;
                }
            }
            return result;
        }

        void OnRegisterConnected(Iocp.TcpClient pClient, byte[] pData, int nLength)
        {
            if (nLength == 0)
                return;
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            //System.String[] ips = Iocp.TcpServer.GetHostIpAddress();
            //mParameter.ListenIP = SelectLogServerIP(ips);

            H_RPCRoot.smInstance.HGet_RegServer(pkg).RegLogServer(pkg, mParameter.ListenIP, mParameter.ListenPort, mParameter.ServerId);
            pkg.WaitDoCommand(mRegisterConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                System.Diagnostics.Debug.WriteLine("数据服务器({0})启动并且注册成功，可以等待连接服务器接入了", mParameter.ServerId);

                if (mLinkState != LogServerState.Working)
                {
                    if (false == mTcpSrv.Open(Iocp.TcpOption.ForComServer,mParameter.ListenPort))
                        return;
                }

                mLinkState = LogServerState.Working;
            };
        }

        public void ServerDisConnected(Iocp.TcpConnect pConnect, Iocp.TcpServer pServer, byte[] pData, int nLength)
        {
            
        }
        #endregion

        #region RPC method
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer,true)]
        public void WriteDBLog(ServerFrame.DB.DBLogData data)
        {
            LogSaverThread.Instance.Push(data);
        }
        #endregion
    }

    public class LogSaverThread
    {
        static LogSaverThread smInstance = new LogSaverThread();
        public static LogSaverThread Instance
        {
            get { return smInstance; }
        }
        
        public void Push(ServerFrame.DB.DBLogData holder)
        {
            lock (this)
            {
                mLogQueue.Enqueue(holder);
            }
        }
        Queue<ServerFrame.DB.DBLogData> mLogQueue = new Queue<ServerFrame.DB.DBLogData>();
        public int GetNumber()
        {
            return mLogQueue.Count;
        }
        void Tick()
        {
            if (mLogQueue.Count > 0)
            {
                ServerFrame.DB.DBLogData atom = null;
                lock (this)
                {
                    atom = mLogQueue.Dequeue();
                }
                try
                {
                    string condition = "LogId = " + ServerFrame.DB.DBConnect.Guid2SqlString(atom.LogId);
                    ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, atom, false);
                    mDBConnect._ExecuteInsert(dbOp);
                }
                catch (System.Exception ex)
                {
                    Log.Log.Common.Print(ex.ToString());
                    Log.Log.Common.Print(ex.StackTrace.ToString());
                }
            }
        }
        bool mRunning;
        private ServerFrame.DB.DBConnect mDBConnect;
        private System.Threading.Thread mThread;
        public void ThreadLoop()
        {
            while (mRunning)
            {
                Tick();
                System.Threading.Thread.Sleep(5);
            }
            mThread = null;
            Log.Log.Common.Print("LogThread Thread exit!");
        }
        public void StartThread()
        {
            mDBConnect = new ServerFrame.DB.DBConnect();
            mDBConnect.OpenConnect();
            mRunning = true;
            mThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ThreadLoop));
            mThread.Start();
        }
        public void StopThread()
        {
            mRunning = false;
        }
    }
}
