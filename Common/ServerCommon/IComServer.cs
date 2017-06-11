using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon
{
    public enum ComServerState
    {
        None,
        WaitRegServer,
        Working,
    }

    [ServerCommon.Editor.CDataEditorAttribute(".comsrv")]
    [Serializable]
    public class IComServerParameter
    {
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

        UInt16 mListenPort;
        [ServerFrame.Config.DataValueAttribute("ListenPort")]
        public UInt16 ListenPort
        {
            get { return mListenPort; }
            set { mListenPort = value; }
        }

        string mDateBaseIP = "127.0.0.1";
        [ServerFrame.Config.DataValueAttribute("DateBaseIP")]
        public string DateBaseIP
        {
            get { return mDateBaseIP; }
            set { mDateBaseIP = value; }
        }

        ulong mServerId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Server);
        [ServerFrame.Config.DataValueAttribute("ServerId")]
        public ulong ServerId
        {
            get { return mServerId; }
            set { mServerId = value; }
        }
    }

    [RPC.RPCClassAttribute(typeof(IComServer))]
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class IComServer : RPC.RPCObject
    {
        #region RPC必须的基础定义
        public static RPC.RPCClassInfo smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
        {
            return smRpcClassInfo;
        }
        #endregion

        #region 核心数据
        static IComServer mInstance = null;
        public static IComServer Instance
        {
            get { return mInstance; }
            set { mInstance = value; }
        }

        protected Iocp.TcpClient mRegisterConnect = new Iocp.TcpClient();

        ComServerState mLinkState = ComServerState.None;
        public ComServerState LinkState
        {
            get { return mLinkState; }
        }

        protected Iocp.TcpServer mTcpSrv = new Iocp.TcpServer();

        IComServerParameter mParameter;

        public ulong ServerId
        {
            get { return mParameter.ServerId; }
        }
        #endregion

        #region 总操作
        public void Start(IComServerParameter parameter)
        {
            Stop();

            mParameter = parameter;
            mTcpSrv.ReceiveData += RPC.RPCNetworkMgr.Instance.ServerReceiveData;

            mRegisterConnect.ReceiveData += RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mRegisterConnect.NewConnect += this.OnRegisterConnected;

            ServerFrame.DB.AsyncExecuteThreadManager.Instance.InitManager(2);
            ServerFrame.DB.AsyncExecuteThreadManager.Instance.StartThread();

            mRegisterConnect.Connect(parameter.RegServerIP, parameter.RegServerPort);
            mLinkState = ComServerState.WaitRegServer;

            this.UserRoleManager.DBConnect.OpenConnect();

            Log.FileLog.Instance.Begin("ComServer.log", false);
            Log.Log.Server.Print("ComServer Start!");
            Log.FileLog.Instance.Flush();
            IServer.LoadAllTemplateData(ServerCommon.ServerConfig.Instance.TemplatePath);
            ServerCommon.TemplateTableLoader.LoadTable(ServerCommon.ServerConfig.Instance.TablePath);

            this.UserRoleManager.DownloadPlanesData();
            this.UserRoleManager.DownloadRankData();
            this.UserRoleManager.RankInit();

            Com.GuildManager.Instance.Init();
            this.WolrdManager.Init(UserRoleManager.Planes.Values.ToArray());
        }

        public void Stop()
        {
            ServerFrame.DB.AsyncExecuteThreadManager.Instance.StopThread();
            mTcpSrv.ReceiveData -= RPC.RPCNetworkMgr.Instance.ServerReceiveData;
            mTcpSrv.Close();
            Log.FileLog.Instance.End();
        }

        Int64 mTryRegServerReconnectTime;
        public void Tick()
        {
            IServer.Instance.Tick();
            try
            {
                UserRoleManager.Tick();
            }
            catch (System.Exception ex)
            {
                Log.Log.Common.Print(ex.ToString());
                Log.Log.Common.Print(ex.StackTrace.ToString());
            }
            

            var time = IServer.timeGetTime();
            if (mLinkState == ComServerState.WaitRegServer)
            {
                //每过一段时间尝试连接一次
                if (mRegisterConnect.State != Iocp.NetState.Connect)
                {
                    if (time - mTryRegServerReconnectTime > 3000)
                    {
                        mTryRegServerReconnectTime = time;
                        mRegisterConnect.Reconnect();
                    }
                }
            }
            else if (mLinkState == ComServerState.Working)
            {
                if (mRegisterConnect.State == Iocp.NetState.Disconnect || mRegisterConnect.State == Iocp.NetState.Invalid)
                {
                    if (time - mTryRegServerReconnectTime > 3000)
                    {
                        mTryRegServerReconnectTime = time;
                        mRegisterConnect.Reconnect();
                    }
                }
            }
            this.UserRoleManager.DBConnect.Tick();
            mRegisterConnect.Update();
            mTcpSrv.Update();
            RPC.RPCNetworkMgr.Instance.Tick(IServer.Instance.GetElapseMilliSecondTime());
        }
        #endregion

        #region 服务器各种注册流程

        void OnRegisterConnected(Iocp.TcpClient pClient, byte[] pData, int nLength)
        {
            if (nLength == 0)
                return;
            RPC.PackageWriter pkg = new RPC.PackageWriter();

            H_RPCRoot.smInstance.HGet_RegServer(pkg).RegComServer(pkg, mParameter.ListenIP, mParameter.ServerId);
            pkg.WaitDoCommand(mRegisterConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                System.Diagnostics.Debug.WriteLine("公共信息通讯服务器({0})启动并且注册成功，可以等待位面服务器接入了", mParameter.ServerId);
                UInt16 listenPort = 0;
                _io.Read(out listenPort);
                mParameter.ListenPort = listenPort;

                if (mLinkState != ComServerState.Working)
                {
                    if (false == mTcpSrv.Open(Iocp.TcpOption.ForComServer, mParameter.ListenPort))
                        return;
                }

                mLinkState = ComServerState.Working;
            };
        }

        #endregion

        #region RPC method

        [RPC.RPCChildObjectAttribute(0, (int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public Com.UserRoleManager UserRoleManager
        {
            get { return Com.UserRoleManager.Instance; }
        }


        [RPC.RPCChildObjectAttribute(1, (int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public Com.WorldManager WolrdManager
        {

            get { return Com.WorldManager.Instance; }
        }
        #endregion

        
    }
}
