using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon
{
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class ServerInfo
    {
        public ulong Id;
        public string Ip;
        public UInt16 Port;
        
        public Iocp.NetConnection Connect;

        public int LinkNumber;
    }
    public enum RegServerState
    {
        None,
        Working,
    }
    
    [ServerCommon.Editor.CDataEditorAttribute(".regsrv")]
    [Serializable]
    public class IRegisterServerParameter
    {
        string mLocalNetIp = "127.0.0.1";
        [ServerFrame.Config.DataValueAttribute("LocalNetIp")]
        public string LocalNetIp
        {
            get { return mLocalNetIp; }
            set { mLocalNetIp = value; }
        }
        UInt16 mListenPort = 8888;
        [ServerFrame.Config.DataValueAttribute("ListenPort")]
        public UInt16 ListenPort
        {
            get { return mListenPort; }
            set { mListenPort = value; }
        }

        string mGlobalNetIp = "127.0.0.1";
        [ServerFrame.Config.DataValueAttribute("GlobalNetIp")]
        public string GlobalNetIp
        {
            get { return mGlobalNetIp; }
            set { mGlobalNetIp = value; }
        }
        UInt16 mClientListenPort = 9998;
        [ServerFrame.Config.DataValueAttribute("ClientListenPort")]
        public UInt16 ClientListenPort
        {
            get { return mClientListenPort; }
            set { mClientListenPort = value; }
        }

        UInt16 mServerPortAllocStart = 32000;
        [ServerFrame.Config.DataValueAttribute("ServerPortAllocStart")]
        public UInt16 ServerPortAllocStart
        {
            get { return mServerPortAllocStart; }
            set { mServerPortAllocStart = value; }
        }

        UInt16 mServerPortAllocEnd = 33000;
        [ServerFrame.Config.DataValueAttribute("ServerPortAllocEnd")]
        public UInt16 ServerPortAllocEnd
        {
            get { return mServerPortAllocEnd; }
            set { mServerPortAllocEnd = value; }
        }
    }

    class ClientLinkedInfo
    {
        public uint ConnectedTime = IServer.timeGetTime();
        public int LandStep = 0;
    }

    [RPC.RPCClassAttribute(typeof(IRegisterServer))]
    public class IRegisterServer : RPC.RPCObject
    {
        #region RPC必须的基础定义
        public static RPC.RPCClassInfo smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
        {
            return smRpcClassInfo;
        }
        #endregion        

        public IRegisterServer()
        {
            //InitPorts();
        }

        public Dictionary<System.UInt16, RPC.RPCWaitHandle> WaitHandles
        {
            get { return RPC.RPCNetworkMgr.Instance.WaitHandles; }
        }

        #region 核心数据
        protected Iocp.TcpServer mTcpSrv = new Iocp.TcpServer();
        RegServerState mLinkState = RegServerState.None;
        public RegServerState LinkState
        {
            get { return mLinkState; }
        }
        protected Iocp.TcpServer mClientLoginSrv = new Iocp.TcpServer();

        IRegisterServerParameter mParameter;
        #endregion

        #region 总操作
        public void Start(IRegisterServerParameter parameter)
        {
            if (mLinkState != RegServerState.None)
                return;

            mParameter = parameter;
            InitPorts();

            mTcpSrv.ReceiveData += RPC.RPCNetworkMgr.Instance.ServerReceiveData;
            mTcpSrv.NewConnect += this.ServerConnected;
            mTcpSrv.CloseConnect += this.ServerDisConnected;

            mClientLoginSrv.LimitLevel = (int)RPC.RPCExecuteLimitLevel.Player;
            mClientLoginSrv.ReceiveData += RPC.RPCNetworkMgr.Instance.ServerReceiveData;
            mClientLoginSrv.NewConnect += this.ClientConnected;
            mClientLoginSrv.CloseConnect += this.ClientDisConnected;

            if (false == mTcpSrv.Open(Iocp.TcpOption.ForComServer,mParameter.ListenPort))//, mParameter.LocalNetIp))
                return;
            System.Diagnostics.Debug.WriteLine("注册服务器启动，可以接客了");

            if (false == mClientLoginSrv.Open(Iocp.TcpOption.ForRegServer,parameter.ClientListenPort))//, mParameter.GlobalNetIp))//在9999端口收听客户端登陆请求
                return;
            System.Diagnostics.Debug.WriteLine("登陆服务器启动，可以接客了");
            mLinkState = RegServerState.Working;

            Log.FileLog.Instance.Begin("RegisterServer.log",false);
            Log.Log.Server.Print("RegisterServer Start ===== ok!");

            //CSCommon.Net.Tcp.TcpServer testServer = new CSCommon.Net.Tcp.TcpServer();
            //testServer.Opt = CSCommon.Net.Tcp.TcpOption.ForServer;
            //testServer.Open(6666);
        }

        public void Stop()
        {
            mTcpSrv.ReceiveData -= RPC.RPCNetworkMgr.Instance.ServerReceiveData;
            mTcpSrv.NewConnect -= this.ServerConnected;
            mTcpSrv.CloseConnect -= this.ServerDisConnected;

            mClientLoginSrv.ReceiveData -= RPC.RPCNetworkMgr.Instance.ServerReceiveData;

            mClientLoginSrv.Close();
            mTcpSrv.Close();
            mDataServer = null;
            mGateServers.Clear();
            mPlanesServers.Clear();
            mAllConnect.Clear();

            System.Diagnostics.Debug.WriteLine("注册服务器关闭");
            mLinkState = RegServerState.None;

            Log.FileLog.Instance.End();
        }

        public void Tick()
        {
            IServer.Instance.Tick();

            mTcpSrv.Update();
            mClientLoginSrv.Update();
            RPC.RPCNetworkMgr.Instance.Tick(IServer.Instance.GetElapseMilliSecondTime());

            KickInvalidLinker();
        }
        #endregion

        #region 服务器各种注册流程
        List<Iocp.TcpConnect> mAllConnect = new List<Iocp.TcpConnect>();
        List<Iocp.TcpConnect> mAllClientShortConnect = new List<Iocp.TcpConnect>();
        ServerFrame.NetEndPoint mDataServer;
        public ServerFrame.NetEndPoint DataServer
        {
            get { return mDataServer; }
        }
        ServerFrame.NetEndPoint mComServer;
        public ServerFrame.NetEndPoint ComServer
        {
            get { return mComServer; }
        }
        ServerFrame.NetEndPoint mLogServer;
        public ServerFrame.NetEndPoint LogServer
        {
            get { return mLogServer; }
        }
        Dictionary<Iocp.NetConnection, ServerInfo> mGateServers = new Dictionary<Iocp.NetConnection, ServerInfo>();
        public Dictionary<Iocp.NetConnection, ServerInfo> GateServers
        {
            get { return mGateServers; }
        }
        Dictionary<Iocp.NetConnection, ServerInfo> mPlanesServers = new Dictionary<Iocp.NetConnection, ServerInfo>();
        public Dictionary<Iocp.NetConnection, ServerInfo> PlanesServers
        {
            get { return mPlanesServers; }
        }
        Dictionary<Iocp.NetConnection, ServerInfo> mPathFindServers = new Dictionary<Iocp.NetConnection, ServerInfo>();
        public Dictionary<Iocp.NetConnection, ServerInfo> PathFindServers
        {
            get { return mPathFindServers; }
        }

        public void ServerConnected(Iocp.TcpConnect pConnect, Iocp.TcpServer pServer, byte[] pData, int nLength)
        {
            lock (this)
            {
                if (pServer == this.mTcpSrv)
                {
                    ServerInfo si = AllocServerPort(pConnect.IpAddress, pConnect);
                    pConnect.mLimitLevel = this.mTcpSrv.LimitLevel;
                    pConnect.m_BindData = si;
                    Log.Log.Server.Print("有服务器{0}接入，分配端口{1}", si.Id, si.Port);
                    foreach (Iocp.TcpConnect conn in mAllConnect)
                    {
                        if (conn == pConnect)
                            return;
                    }
                    mAllConnect.Add(pConnect);
                }
                else
                {
                    pConnect.Disconnect();
                }
            }            
        }

        public void ServerDisConnected(Iocp.TcpConnect pConnect, Iocp.TcpServer pServer, byte[] pData, int nLength)
        {
            lock (this)
            {
                mAllConnect.Remove(pConnect);
                if (mDataServer != null && pConnect == mDataServer.Connect)
                {
                    mDataServer = null;
                }
                if (mLogServer != null && pConnect == mLogServer.Connect)
                {
                    mLogServer = null;
                }
                if (mComServer != null && pConnect == mComServer.Connect)
                {
                    mComServer = null;
                }
                mGateServers.Remove(pConnect);
                mPlanesServers.Remove(pConnect);
                PathFindServers.Remove(pConnect);

                ServerInfo si = pConnect.m_BindData as ServerInfo;
                Log.Log.Server.Print("有服务器{0}断开连接，分配端口{1}", si.Id, si.Port);

                FreeServerPort(si);
            }            
        }

        public void ClientConnected(Iocp.TcpConnect pConnect, Iocp.TcpServer pServer, byte[] pData, int nLength)
        {
            lock (mAllClientShortConnect)
            {
                if (pServer == this.mClientLoginSrv)
                {
                    pConnect.mLimitLevel = (int)RPC.RPCExecuteLimitLevel.Player;
                    foreach (Iocp.TcpConnect conn in mAllClientShortConnect)
                    {
                        if (conn == pConnect)
                            return;
                    }
                    var linkedInfo = new ClientLinkedInfo();
                    pConnect.m_BindData = linkedInfo;

                    mAllClientShortConnect.Add(pConnect);
                }
                else
                {
                    pConnect.Disconnect();
                }
            }
        }

        public void ClientDisConnected(Iocp.TcpConnect pConnect, Iocp.TcpServer pServer, byte[] pData, int nLength)
        {
            lock (mAllClientShortConnect)
            {
                pConnect.m_BindData = null;
                mAllClientShortConnect.Remove(pConnect);
            }
        }

        uint mPrevKickInvalidLinker = 0;
        void KickInvalidLinker()
        {
            uint nowTime = IServer.timeGetTime();
            if (nowTime - mPrevKickInvalidLinker < 10000)
                return;
            mPrevKickInvalidLinker = nowTime;

            try
            {
                System.Threading.Monitor.Enter(this);
                foreach (var i in mAllClientShortConnect)
                {
                    if (i == null)
                        continue;
                    var linkedInfo = i.m_BindData as ClientLinkedInfo;
                    if (linkedInfo == null)
                        continue;

                    if (nowTime - linkedInfo.ConnectedTime > 15000)
                    {
                        i.Disconnect();
                    }
                }
            }
            finally
            {
                System.Threading.Monitor.Exit(this);
            }
        }

        #endregion

        #region 服务器端口分配
        ServerInfo[] mServerPorts;

        void InitPorts()
        {
            UInt16 portNum = (UInt16)(mParameter.ServerPortAllocEnd - mParameter.ServerPortAllocStart);
            mServerPorts = new ServerInfo[portNum];
            for (UInt16 i = 0; i < portNum; i++)
            {
                mServerPorts[i] = new ServerInfo();
                mServerPorts[i].Port = (UInt16)(mParameter.ServerPortAllocEnd + i);
                mServerPorts[i].Ip = "";
                mServerPorts[i].Id = 0;
            }
        }

        ServerInfo AllocServerPort(string ip,Iocp.NetConnection connect)
        {
            lock (this)
            {
                foreach (var i in mServerPorts)
                {
                    if (i.Connect == connect)
                    {
                        return i;
                    }
                }

                foreach (var i in mServerPorts)
                {
                    if (i.Connect==null)
                    {
                        i.Ip = ip;
                        i.Connect = connect;
                        i.LinkNumber = 0;
                        return i;
                    }
                }
                return null;
            }
        }
        void FreeServerPort(ServerInfo server)
        {
            lock(this)
            {
                server.Ip = "";
                server.Id = 0;
                server.Connect = null;
                server.LinkNumber = 0;
            }
        }
        #endregion

        #region RPC method
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public UInt16 RegGateServer(string ipAddress,ulong id, Iocp.NetConnection connect)
        {
            ServerInfo si = connect.m_BindData as ServerInfo;
            si.Id = id;
            si.Ip = ipAddress;
            si.LinkNumber = 0;
            mGateServers[connect] = si;

            Log.Log.Server.Print("Gate服务器{0}注册，{1}:{2}", si.Id, si.Ip, si.Port);
            return si.Port;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public RPC.DataWriter GetGateServers()
        {
            RPC.DataWriter d = new RPC.DataWriter();
            System.Byte nCount = (Byte)mGateServers.Count;
            d.Write(nCount);
            foreach (var l in mGateServers)
            {
                d.Write(l.Value.Ip);
                d.Write(l.Value.Port);
            }
            return d;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public UInt16 RegPlanesServer(ulong id, Iocp.NetConnection connect)
        {
            if (id == 0)
            {
                Log.Log.Server.Print("RegPlanesServer id is Empty");
            }

            ServerInfo si = connect.m_BindData as ServerInfo;
            si.Id = id;
            Iocp.TcpConnect tcpConnect = connect as Iocp.TcpConnect;
            si.Ip = tcpConnect.IpAddress;
            si.LinkNumber = 0;
            mPlanesServers[connect] = si;

            Log.Log.Server.Print("Planes服务器{0}注册，{1}:{2}", si.Id, si.Ip , si.Port);

            foreach (var i in mGateServers)
            {
                //i.Value.Connect
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_GateServer(pkg).NewPlanesServerStarted(pkg);
                pkg.DoCommand(i.Value.Connect, RPC.CommandTargetType.DefaultType);
            }
            return si.Port;
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public SByte RegDataServer(string ListenIp, UInt16 ListenPort, ulong id, Iocp.NetConnection connect)
        {
            if (mDataServer != null )
            {
                Iocp.TcpConnect conn = mDataServer.Connect as Iocp.TcpConnect;
                if (conn != null)
                {//数据服务器是唯一的，起来一个就要把另外一个踢下线
                    conn.Disconnect();
                }
            }
            mDataServer = new ServerFrame.NetEndPoint(ListenIp, ListenPort);
            mDataServer.Connect = connect;
            mDataServer.Id = id;

            Log.Log.Server.Print("Data服务器{0}注册，{1}:{2}", id, ListenIp, ListenPort);
            return 1;
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public UInt16 RegPathFindServer(string ListenIp, ulong id, Iocp.NetConnection connect)
        {
            ServerInfo si = connect.m_BindData as ServerInfo;
            si.Id = id;
            si.Ip = ListenIp;
            si.LinkNumber = 0;

            PathFindServers[connect] = si;
            return si.Port;
        }
        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        //public void GetPathFindServers(Iocp.NetConnection connect,RPC.RPCForwardInfo fwd)
        //{
        //    RPC.PackageWriter retPkg = new RPC.PackageWriter();
        //    retPkg.Write((Byte)PathFindServers.Count);
        //    foreach (var i in PathFindServers)
        //    {
        //        retPkg.Write(i.Value.Ip);
        //        retPkg.Write(i.Value.Port);
        //    }
        //    retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        //}
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public UInt16 RegComServer(string ListenIp, ulong id, Iocp.NetConnection connect)
        {
            ServerInfo si = connect.m_BindData as ServerInfo;
            si.Id = id;
            si.Ip = ListenIp;
            si.LinkNumber = 0;

            mComServer = new ServerFrame.NetEndPoint(ListenIp, si.Port);
            mComServer.Connect = connect;
            mComServer.Id = id;

            Log.Log.Server.Print("Com服务器{0}注册，{1}:{2}", id, ListenIp, si.Port);
            return si.Port;
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public SByte RegLogServer(string ListenIp, UInt16 ListenPort, ulong id, Iocp.NetConnection connect)
        {
            if (mLogServer != null)
            {
                Iocp.TcpConnect conn = mLogServer.Connect as Iocp.TcpConnect;
                if (conn != null)
                {//Log服务器是唯一的，起来一个就要把另外一个踢下线
                    conn.Disconnect();
                }
            }
            mLogServer = new ServerFrame.NetEndPoint(ListenIp, ListenPort);
            mLogServer.Connect = connect;
            mLogServer.Id = id;
            return 1;
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void SetGateLinkNumber(Iocp.NetConnection connect, int num)
        {
            ServerInfo serverInfo;
            if (mGateServers.TryGetValue(connect,out serverInfo) == true)
            {
                serverInfo.LinkNumber = num;
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_QueryAllActivePlanesInfo(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).QueryAllActivePlanesInfo(pkg, fwd.Handle);
            pkg.WaitDoCommand(mDataServer.Connect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.DataReader dr;
                _io.Read(out dr);

                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                UInt16 count = 0;
                dr.Read(out count);
                retPkg.Write(count);
                for (UInt16 i = 0; i < count; i++)
                {
                    ushort id = dr.ReadUInt16();
                    string planesName = dr.ReadString();
                    retPkg.Write(id);
                    retPkg.Write(planesName);
                }
                retPkg.DoReturnGate2Client(fwd);
            };
        }


        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public RPC.DataWriter GetLowGateServer(Iocp.NetConnection connect)
        {
            RPC.DataWriter d = new RPC.DataWriter();
            if (mGateServers.Count==0)
            {
                return d;
            }

            var linkedInfo = connect.m_BindData as ClientLinkedInfo;
            if (linkedInfo == null)
            {
                return d;
            }

            int minValue = Int32.MaxValue;
            ServerInfo serverInfo = null;
            foreach (var l in mGateServers)
            {
                if (l.Value.LinkNumber < minValue)
                {
                    minValue = l.Value.LinkNumber;
                    serverInfo = l.Value;
                }
            }

            if (serverInfo!=null)
            {
                d.Write(serverInfo.Ip);
                d.Write(serverInfo.Port);
            }

            linkedInfo.LandStep = 1;

            return d;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public RPC.DataWriter GetDataServer()
        {
            RPC.DataWriter d = new RPC.DataWriter();
            if (mDataServer == null)
            {
                return d;
            }
            d.Write(mDataServer.IpAddress);
            d.Write(mDataServer.Port);

            return d;
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public RPC.DataWriter GetComServer()
        {
            RPC.DataWriter d = new RPC.DataWriter();
            if (mComServer == null)
            {
                return d;
            }
            d.Write(mComServer.IpAddress);
            d.Write(mComServer.Port);

            return d;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public RPC.DataWriter GetLogServer()
        {
            RPC.DataWriter d = new RPC.DataWriter();
            if (mLogServer == null)
            {
                d.Write((sbyte)(-1));
                return d;
            }
            d.Write((sbyte)(1));
            d.Write(mLogServer.IpAddress);
            d.Write(mLogServer.Port);

            return d;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public RPC.DataWriter GetPlanesServers()
        {
            RPC.DataWriter d = new RPC.DataWriter();

            Byte count = (Byte)mPlanesServers.Count;
            d.Write(count);
            foreach( var s in mPlanesServers )
            {
                if (s.Value.Id == 0)
                {
                    Log.Log.Server.Print("GetPlanesServers时有的PlanesSever的Id不合法");
                    RPC.PackageWriter pkg = new RPC.PackageWriter();
                    H_RPCRoot.smInstance.HGet_PlanesServer(pkg).GetPlanesServerId(pkg);
                    pkg.WaitDoCommand(s.Value.Connect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
                    {
                        _io.Read(out s.Value.Id);
                    };
                }
                d.Write(s.Value.Id);
                d.Write(s.Value.Ip);
                d.Write(s.Value.Port);
            }

            return d;
        }
        #endregion
    }
}
