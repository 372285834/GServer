using CSCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon
{
    public enum GateServerState
    {
        None,
        WaitRegServer,
        WaitDataServer,
        Working,
    }

    public enum EServerError
    {
        RaiseExp,
    }

    [ServerCommon.Editor.CDataEditorAttribute(".gatesrv")]
    [Serializable]
    public class IGateServerParameter
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
        UInt16 mListenPort = 21000;
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
    [RPC.RPCClassAttribute(typeof(IGateServer))]
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class IGateServer : RPC.RPCObject
    {
        public IGateServer()
        {
            InitClientLinkerPool();
        }
        #region RPC必须的基础定义
        public static RPC.RPCClassInfo smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
        {
            return smRpcClassInfo;
        }
        #endregion        

        #region 核心数据
        protected Iocp.TcpServer mTcpSrv = new Iocp.TcpServer();

        protected Iocp.TcpClient mRegisterConnect = new Iocp.TcpClient();
        protected Iocp.TcpClient mDataConnect = new Iocp.TcpClient();
        
        GateServerState mLinkState = GateServerState.None;
        public GateServerState LinkState
        {
            get { return mLinkState; }
        }

        IGateServerParameter mParameter;
        #endregion        

        #region 统计属性

        public int MaxLinkedClientNumber
        {
            get
            {
                return mMaxClientLinker;
            }
        }

        public int FreeLinkedClientNumber
        {
            get
            {
                return mFreeClientLinkers.Count;
            }
        }

        public Dictionary<System.UInt16, RPC.RPCWaitHandle> WaitHandles
        {
            get { return RPC.RPCNetworkMgr.Instance.WaitHandles; }
        }

        #endregion

        #region 总操作
        public void Start(IGateServerParameter parameter)
        {
            //ServerFrame.Support.ClassInfoManager.Instance.Load(false);

            RPCGateServerForwardImpl impl = new RPCGateServerForwardImpl();
            impl.mServer = this;
            RPC.RPCNetworkMgr.Instance.mGateServerForward = impl;

            mParameter = parameter;
            mTcpSrv.ReceiveData += RPC.RPCNetworkMgr.Instance.ServerReceiveData;
            mTcpSrv.NewConnect += this.ClientConnected;
            mTcpSrv.CloseConnect += this.ClientDisConnected;

            mRegisterConnect.ReceiveData += RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mRegisterConnect.NewConnect += this.OnRegisterConnected; 
            mDataConnect.ReceiveData += RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mDataConnect.NewConnect += this.OnDataServerConnected;

            mRegisterConnect.Connect(parameter.RegServerIP, parameter.RegServerPort);
            mLinkState = GateServerState.WaitRegServer;

            
            Log.FileLog.Instance.Begin("GateServer.log",false);
            Log.Log.Server.Print("GateServer Start ===== ok!");
        }

        public void Stop()
        {
            mTcpSrv.ReceiveData -= RPC.RPCNetworkMgr.Instance.ServerReceiveData;
            mTcpSrv.NewConnect -= this.ClientConnected;
            mTcpSrv.CloseConnect -= this.ClientDisConnected;

            mRegisterConnect.ReceiveData -= RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mRegisterConnect.NewConnect -= this.OnRegisterConnected;
            mDataConnect.ReceiveData -= RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mDataConnect.NewConnect -= this.OnDataServerConnected;

            mRegisterConnect.Close();
            mDataConnect.Close();
            mDataConnect.Close();

            System.Diagnostics.Debug.WriteLine("连接服务器关闭");
            mLinkState = GateServerState.None;

            Log.FileLog.Instance.End();
        }
        UInt64 mTryRegServerReconnectTime;
        UInt64 mTryDataServerReconnectTime;

        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        extern static UInt32 timeGetTime();

        public void Tick()
        {
            UInt64 time = timeGetTime();

            #region 维护服务器之间的链接
            switch (mLinkState)
            {
                case GateServerState.None:
                case GateServerState.WaitRegServer:
                    {
                        //每过一段时间尝试连接一次
                        if (mRegisterConnect.State == Iocp.NetState.Disconnect || mRegisterConnect.State == Iocp.NetState.Invalid)
                        {
                            if (time - mTryRegServerReconnectTime > 3000)
                            {
                                mTryRegServerReconnectTime = time;
                                mRegisterConnect.Reconnect();
                            }
                        }
                    }
                    break;
                case GateServerState.WaitDataServer:
                    {
                        if (mDataConnect.State == Iocp.NetState.Disconnect || mDataConnect.State == Iocp.NetState.Invalid)
                        {
                            //每过一段时间尝试连接一次
                            if (time - mTryDataServerReconnectTime > 3000)
                            {
                                mTryDataServerReconnectTime = time;
                                //mDataConnect.Reconnect();
                                ConnectDataServer();
                            }
                        }
                    }
                    break;
                case GateServerState.Working:
                    {
                        if (mRegisterConnect.State == Iocp.NetState.Disconnect || mRegisterConnect.State == Iocp.NetState.Invalid)
                        {
                            //每过一段时间尝试连接一次
                            if (time - mTryRegServerReconnectTime > 3000)
                            {
                                mTryRegServerReconnectTime = time;
                                mRegisterConnect.Reconnect();
                            }
                        }
                        if (mDataConnect.State == Iocp.NetState.Disconnect || mRegisterConnect.State == Iocp.NetState.Invalid)
                        {
                            //每过一段时间尝试连接一次
                            if (time - mTryDataServerReconnectTime > 3000)
                            {
                                mTryDataServerReconnectTime = time;
                                //mDataConnect.Reconnect();
                                ConnectDataServer();
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            #endregion

            IServer.Instance.Tick();

            mRegisterConnect.Update();
            mDataConnect.Update();
            mTcpSrv.Update();

            foreach (KeyValuePair<ulong, Iocp.TcpClient> kv in mPlanesConnects)
            {
                if (kv.Value != null)
                {
                    if (kv.Value.State == Iocp.NetState.Disconnect || kv.Value.State == Iocp.NetState.Invalid)
                    {
                        kv.Value.Update();
                        mPlanesConnects.Remove(kv.Key);
                        break;
                    }
                    kv.Value.Update();
                }
            }

            RPC.RPCNetworkMgr.Instance.Tick(IServer.Instance.GetElapseMilliSecondTime());

            //KickInvalidLinker();
        }
        #endregion

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
                foreach (var i in mClientLinkers)
                {
                    if (i == null)
                        continue;
                    if (i.mLandStep != Gate.LandStep.TryLogin)
                        continue;

                    if (nowTime - i.ConnectedTime > 15000)
                    {
                        i.mForwardInfo.Gate2ClientConnect.Disconnect();
                    }
                }
            }
            finally
            {
                System.Threading.Monitor.Exit(this);
            }
        }

        #region 服务器各种注册流程
        string GetInternetServerIP(string[] ips)
        {
            return mParameter.ListenIP;
        }

        void OnRegisterConnected(Iocp.TcpClient pClient, byte[] pData, int nLength)
        {
            if (nLength == 0)
                return;
            RPC.PackageWriter pkg = new RPC.PackageWriter();

            //string[] ips = Iocp.TcpServer.GetHostIpAddress();
            //mParameter.ListenIP = GetInternetServerIP(ips);

            H_RPCRoot.smInstance.HGet_RegServer(pkg).RegGateServer(pkg, mParameter.ListenIP, mParameter.ServerId);
            pkg.WaitDoCommand(mRegisterConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                UInt16 port;
                _io.Read(out port);
                mParameter.ListenPort = port;
                
                if (mLinkState == GateServerState.WaitRegServer)
                {
                    Log.Log.Server.Print("连接服务器({0})启动并且注册成功", mParameter.ServerId);
                    mLinkState = GateServerState.WaitDataServer;
                    ConnectDataServer();
                }
                else
                {
                    Log.Log.Server.Print("GateServer断线重连接RegServer");
                }
            };
        }

        void ConnectDataServer()
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_RegServer(pkg).GetDataServer(pkg);
            pkg.WaitDoCommand(mRegisterConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.DataReader dr;
                _io.Read(out dr);
                string gsIpAddress = "";
                UInt16 gsPort = 0;
                if (dr.Length > 0)
                {
                    dr.Read(out gsIpAddress);
                    dr.Read(out gsPort);
                }
                if (gsIpAddress != "" && gsPort != 0)
                {
                    if (mLinkState == GateServerState.WaitRegServer)
                    {
                        mDataConnect.Connect(gsIpAddress, gsPort);
                        System.Diagnostics.Debug.WriteLine("GateServer成功连接RegServer，尝试连接DataServer:" + gsIpAddress + ":" + gsPort);
                    }
                    else
                    {
                        mDataConnect.Connect(gsIpAddress, gsPort);
                        System.Diagnostics.Debug.WriteLine("DataServer断线，重新从RegServer获得地址，尝试连接DataServer:" + gsIpAddress + ":" + gsPort);
                    }
                }
            };
        }

        void OnDataServerConnected(Iocp.TcpClient pClient, byte[] pData, int nLength)
        {
            if (nLength == 0)
                return;
            if (mLinkState != GateServerState.Working)
            {
                if (false == mTcpSrv.Open(Iocp.TcpOption.ForGateServer,mParameter.ListenPort))
                    return;
                System.Diagnostics.Debug.WriteLine("DateServer连接成功，GateServer开始接受客户端接入");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("GateServer与DateServer断线后重连接成功");
            }
            mLinkState = GateServerState.Working;

            RPC.PackageWriter pkg = new RPC.PackageWriter();

            H_RPCRoot.smInstance.HGet_DataServer(pkg).RegGateServer(pkg, mParameter.ListenIP, mParameter.ListenPort, mParameter.ServerId);
            pkg.DoCommand(mDataConnect, RPC.CommandTargetType.DefaultType);

            RefreshPlanesConnect();
        }
        #endregion

        #region 客户端接入断开管理
        UInt16 mMaxClientLinker = 8192;
        Gate.ClientLinker[] mClientLinkers;
        Stack<UInt16> mFreeClientLinkers = new Stack<UInt16>();
        void InitClientLinkerPool()
        {
            mClientLinkers = new Gate.ClientLinker[mMaxClientLinker];
            mFreeClientLinkers.Clear();
            for (int i = mMaxClientLinker-1; i >=0 ; i--)
            {
                mFreeClientLinkers.Push((UInt16)i);
            }
        }
        bool AllocLinker(Gate.ClientLinker lnk)
        {
            lock (this)
            {
                if (mFreeClientLinkers.Count == 0)
                    return false;
                lnk.mForwardInfo.Handle = mFreeClientLinkers.Pop();
                mClientLinkers[lnk.mForwardInfo.Handle] = lnk;
                lnk.ConnectedTime = IServer.timeGetTime();
                return true;
            }
        }
        bool FreeLinker(Gate.ClientLinker lnk)
        {
            lock (this)
            {
                if (lnk.mForwardInfo.Handle > mMaxClientLinker)
                {
                    Log.Log.Server.Print("Error!FreeLinker : lnk.mForwardInfo.Handle > mMaxClientLinker");
                    return false;
                }
                mFreeClientLinkers.Push(lnk.mForwardInfo.Handle);
                mClientLinkers[lnk.mForwardInfo.Handle] = null;
                lnk.mForwardInfo.Handle = UInt16.MaxValue;
                return true;
            }
        }
        void CheckFreeLinkers()
        {
            try
            {
                for (int i = 0; i < mClientLinkers.Length; i++ )
                {
                    var lnk = mClientLinkers[i];
                    if (lnk == null)
                        continue;
                    var realLinker = lnk.mForwardInfo.Gate2ClientConnect.m_BindData as Gate.ClientLinker;
                    if (realLinker == lnk)
                        continue;

                    Log.Log.Server.Print("Error!CheckFreeLinkers check failed");
                    FreeLinker(lnk);
                    break;
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.Server.Print(ex.ToString());
                Log.Log.Server.Print(ex.StackTrace.ToString());
            }
        }

        public Gate.ClientLinker[] ClientLinkers
        {
            get { return mClientLinkers; }
        }
        public Gate.ClientLinker FindClientLinker(UInt16 index)
        {
            if (index >= mMaxClientLinker)
                return null;

            return mClientLinkers[index];
        }
        public Gate.ClientLinker FindClientLinkerByAccountId(ulong accountId)
        {
            foreach (var i in mClientLinkers)
            {
                if(i==null)
                    continue;
                if (i.AccountInfo.Id == accountId)
                    return i;
            }
            return null;
        }

        int mClientConnectCount = 0;
        public int ClientConnectCount
        {
            get { return mClientConnectCount; }
        }
        public void ClientConnected(Iocp.TcpConnect pConnect, Iocp.TcpServer pServer, byte[] pData, int nLength)
        {
            mClientConnectCount++;
            Gate.ClientLinker cltLinker = new Gate.ClientLinker();
            if (AllocLinker(cltLinker))
            {
                cltLinker.mForwardInfo.Gate2ClientConnect = pConnect;
                cltLinker.mForwardInfo.LimitLevel = (int)RPC.RPCExecuteLimitLevel.Player;
                pConnect.mLimitLevel = (int)RPC.RPCExecuteLimitLevel.Player;

                pConnect.m_BindData = cltLinker;

                int linkNumber = mMaxClientLinker - mFreeClientLinkers.Count;
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_RegServer(pkg).SetGateLinkNumber(pkg, linkNumber);
                pkg.DoCommand(mRegisterConnect, RPC.CommandTargetType.DefaultType);
            }
            else
            {//接入满了
                Log.Log.Server.Print("ClientConnected AllocLinker Failed");
                pConnect.Disconnect();

                CheckFreeLinkers();
            }
        }

        public void ClientDisConnected(Iocp.TcpConnect pConnect, Iocp.TcpServer pServer, byte[] pData, int nLength)
        {
            mClientConnectCount--;
            Gate.ClientLinker cltLinker = pConnect.m_BindData as Gate.ClientLinker;
            if (cltLinker == null)
            {
                Log.Log.Server.Print("Error!ClientDisConnected : cltLinker==null");
                return;
            }
            FreeLinker(cltLinker);

            NotifyOtherServers_ClientDisconnect(cltLinker);

            pConnect.m_BindData = null;
        }

        public void NotifyOtherServers_ClientDisconnect(Gate.ClientLinker cltLinker)
        {
            {//刷新注册服务器上接入服务器的用户负载数
                int linkNumber = mMaxClientLinker - mFreeClientLinkers.Count;
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_RegServer(pkg).SetGateLinkNumber(pkg, linkNumber);
                pkg.DoCommand(mRegisterConnect, RPC.CommandTargetType.DefaultType);
            }

            if (cltLinker.mForwardInfo.Gate2PlanesConnect != null)
            {
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_PlanesServer(pkg).ClientDisConnect(pkg, cltLinker.PlayerData.RoleDetail.RoleId);
                pkg.DoCommand(cltLinker.mForwardInfo.Gate2PlanesConnect, RPC.CommandTargetType.DefaultType);

                cltLinker.mPlayerData = null;
                cltLinker.mForwardInfo.Gate2PlanesConnect = null;
            }
            else
            {//没连位面就下线了
                //Log.Log.Server.Print("GateServer ClientDisConnected RPC:LogoutAccount,username={0}",cltLinker.AccountInfo.UserName);
                RPC.PackageWriter pkg0 = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_DataServer(pkg0).HGet_PlayerManager(pkg0).LogoutAccount(pkg0, cltLinker.AccountInfo.Id, (sbyte)eServerType.Gate);
                pkg0.DoCommand(mDataConnect, RPC.CommandTargetType.DefaultType);
                cltLinker.mPlayerData = null;
                cltLinker.mForwardInfo.Gate2PlanesConnect = null;
            }
        }

        #endregion

        #region 与位面服务器之间建立连接
        protected Dictionary<ulong, Iocp.TcpClient> mPlanesConnects = new Dictionary<ulong, Iocp.TcpClient>();
        Iocp.TcpClient FindPlanesConnect(ulong serverId)
        {
            Iocp.TcpClient result;
            if (mPlanesConnects.TryGetValue(serverId, out result))
                return result;
            return null;
        }
        void RefreshPlanesConnect()
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_RegServer(pkg).GetPlanesServers(pkg);
            pkg.WaitDoCommand(mRegisterConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _pkg, bool bTimeOut)
            {
                RPC.DataReader dr;
                _pkg.Read(out dr);
                Byte count;
                dr.Read(out count);
                for (Byte i = 0; i < count; i++)
                {
                    ulong svrId;
                    dr.Read(out svrId);
                    string ip;
                    dr.Read(out ip);
                    UInt16 port;
                    dr.Read(out port);

                    if (null == FindPlanesConnect(svrId))
                    {
                        Iocp.TcpClient conn = new Iocp.TcpClient();
                        conn.NewConnect += this.PlanesConnected;
                        conn.CloseConnect += this.PlanesDisConnected;
                        conn.ReceiveData += RPC.RPCNetworkMgr.Instance.ClientReceiveData;
                        conn.Connect(ip, port);
                        mPlanesConnects.Add(svrId, conn);
                    }
                }
            };
        }
        public void PlanesConnected(Iocp.TcpClient pClient, byte[] pData, int nLength)
        {
            if (nLength == 0)
                return;
            System.Diagnostics.Debug.WriteLine("一个位面服务器与连接服务器建立连接");
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_PlanesServer(pkg).RegGateServer(pkg, mParameter.ListenIP, mParameter.ListenPort, mParameter.ServerId);
            pkg.DoCommand(pClient, RPC.CommandTargetType.DefaultType);
        }
        public void PlanesDisConnected(Iocp.TcpClient pClient, byte[] pData, int nLength)
        {
            //断开所有进入此位面的玩家接入服务器连接
            foreach (Gate.ClientLinker lnk in mClientLinkers)
            {
                if (lnk!=null && lnk.mForwardInfo.Gate2PlanesConnect == pClient)
                {
                    lnk.mForwardInfo.Gate2PlanesConnect = null;
                    lnk.mForwardInfo.Gate2ClientConnect.Disconnect();
                }
            }
        }
        #endregion

        bool IsSameClientLinker(Gate.ClientLinker linker, Iocp.NetConnection connect)
        {
            Gate.ClientLinker cltLinker = connect.m_BindData as Gate.ClientLinker;
            if (cltLinker == null)
            {
                return false;
            }
            if (linker != cltLinker)
            {
                return false;
            }
            if (linker.LinkerSerialId != cltLinker.LinkerSerialId)
            {
                return false;
            }
            return true;
        }

        #region RPC method

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void NewPlanesServerStarted()
        {
            this.RefreshPlanesConnect();
        }
        
        #region Login&Land
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GuestLogin(Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            Gate.ClientLinker cltLinker = connect.m_BindData as Gate.ClientLinker;
            if (cltLinker == null)
            {
                return;
            }

            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).TryRegGuest(pkg, cltLinker.mForwardInfo.Handle);
            pkg.WaitDoCommand(mDataConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.DataReader dr;
                _io.Read(out dr);
                sbyte success = -1;
                dr.Read(out success);
                string userName = "";
                dr.Read(out userName);

                //ClientTryLogin(userName, "", connect, fwd);

                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                switch (success)
                {
                    case -1:
                        retPkg.Write((sbyte)-1);
                        break;

                    case -2:
                        retPkg.Write((sbyte)-2);
                        break;

                    case 1:
                        retPkg.Write((sbyte)1);
                        retPkg.Write(userName);
                        break;
                }
                retPkg.DoReturnGate2Client(fwd);
            };
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void ClientTryLogin(string usr, string psw, ushort planesid,Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            Gate.ClientLinker cltLinker = connect.m_BindData as Gate.ClientLinker;
            if (cltLinker == null)
                return;
            cltLinker.AccountInfo.UserName = usr;
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).HGet_PlayerManager(pkg).LoginAccount(pkg, cltLinker.mForwardInfo.Handle, usr, psw, planesid,cltLinker.LinkerSerialId);
            pkg.WaitDoCommand(mDataConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.SetSinglePkg();

                sbyte successed = -1;
                _io.Read(out successed);
                if (successed == (sbyte)CSCommon.eRet_LoginAccount.AccountHasLogin)                
                {
                    ulong accountId;
                    _io.Read(out accountId);
                    cltLinker.AccountInfo.Id = accountId;
                    connect.Disconnect();

                    Gate.ClientLinker cltLinker1 = FindClientLinkerByAccountId(accountId);
                    if (cltLinker != null && cltLinker1.mForwardInfo.Gate2ClientConnect != connect)
                    {
                        cltLinker1.mForwardInfo.Gate2ClientConnect.Disconnect();
                    }
                }
                else if (successed != (sbyte)CSCommon.eRet_LoginAccount.Succeed)
                {
                    cltLinker.mLandStep = Gate.LandStep.TryLogin;

                    retPkg.Write(successed);
                    retPkg.DoReturnGate2Client(fwd);
                }
                else
                {
                    if (cltLinker.mLandStep != Gate.LandStep.TryLogin)
                    {
                        if (IsSameClientLinker(cltLinker, connect) == false)
                        {
                            Log.Log.Server.Print("ClientLinker Error");
                            Log.Log.Server.Print(new System.Diagnostics.StackTrace(0, true).ToString());
                            connect.Disconnect();
                            return;
                        }
                    }

                    _io.Read(cltLinker.AccountInfo);
                    cltLinker.mLandStep = Gate.LandStep.SelectRole;

                    Byte count;
                    _io.Read(out count);

                    retPkg.Write((sbyte)1);
                    retPkg.Write(count);

                    cltLinker.AccountInfo.Roles.Clear();
                    for (Byte i = 0; i < count; i++)
                    {
                        CSCommon.Data.RoleInfo ri = new CSCommon.Data.RoleInfo();
                        _io.Read(ri);

                        cltLinker.AccountInfo.Roles.Add(ri);
                        retPkg.Write(ri);
                    }
                    retPkg.DoReturnGate2Client(fwd);
                }                           
            };
        }

        bool HasRole(Gate.ClientLinker cltLinker, ulong roleId)
        {
            if (cltLinker.AccountInfo == null)
                return false;

            foreach (var i in cltLinker.AccountInfo.Roles)
            {
                if (i.RoleId == roleId)
                {
                    return true;
                }
            }
            return false;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RoleTryEnterGame(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            Gate.ClientLinker cltLinker = connect.m_BindData as Gate.ClientLinker;
            if (cltLinker == null)
            {
                Log.Log.Server.Print("cltLinker == null");
                connect.Disconnect();
                return;
            }

            if (HasRole(cltLinker,roleId) == false)
            {
                Log.Log.Server.Print("HasRole error roleId{0}", roleId);
                connect.Disconnect();
                return;
            }
            //从数据服务器获取玩家角色信息
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).HGet_PlayerManager(pkg).LoginRole(pkg, cltLinker.LinkerSerialId, cltLinker.mForwardInfo.Handle, roleId, cltLinker.AccountInfo.Id);
            pkg.WaitDoCommand(mDataConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                #region 各种位面信息失败处理
                sbyte succsess = -1;
                _io.Read(out succsess);
                if(succsess<0)//这里返回错误的话是-1到-3
                {//-1~-3
                    RPC.PackageWriter retPkg = new RPC.PackageWriter();
                    retPkg.Write(succsess);
                    retPkg.DoReturnGate2Client(fwd);

                    if (succsess != -1)
                    {
                        Log.Log.Server.Print("LoginRole succsess != -1");
                        connect.Disconnect();
                    }
                    return;
                }

                if (IsSameClientLinker(cltLinker, connect) == false)
                {
                    Log.Log.Server.Print("ClientLinker Error");
                    Log.Log.Server.Print(new System.Diagnostics.StackTrace(0, true).ToString());
                    connect.Disconnect();
                    return;
                }
                #endregion                

                if (IsSameClientLinker(cltLinker, connect) == false)
                {
                    Log.Log.Server.Info("ClientLinker Error");
                    Log.Log.Server.Info(new System.Diagnostics.StackTrace(0, true).ToString());
                    connect.Disconnect();
                    return;
                }

                cltLinker.mPlayerData = new CSCommon.Data.PlayerDataEx();
                CSCommon.Data.PlayerData pd = cltLinker.mPlayerData;
                CSCommon.Data.PlanesData planesData = null;
                ulong planesServerId = 0;
                ulong mapInstanceId = 0;
                switch (succsess)
                {
                    case 1:
                        {
                            _io.Read(out planesServerId);
                            _io.Read(pd);
                            planesData = new CSCommon.Data.PlanesData();
                            _io.Read(planesData);
                            _io.Read(out mapInstanceId);

                            if (HasRole(cltLinker, pd.RoleDetail.RoleId) == false)
                            {
                                connect.Disconnect();
                                return;
                            }
                        }
                        break;
                    default:
                        //应该返回失败了
                        break;
                }
                
                #region 错误登陆数据
                if (cltLinker.AccountInfo.Id != pd.RoleDetail.AccountId)
                {//走到这里，说明这个玩家已经下线了，甚至下线后，新的玩家上来占用了这个linker，所以这里需要通知数据服务器Logout该角色
                    RPC.PackageWriter retPkg = new RPC.PackageWriter();
                    retPkg.Write((sbyte)-8);
                    retPkg.DoReturnGate2Client(fwd);
                    return;
                }
                #endregion

                cltLinker.mForwardInfo.LimitLevel = pd.RoleDetail.LimitLevel;
                cltLinker.mForwardInfo.Gate2PlanesConnect = FindPlanesConnect(planesServerId);
                if (cltLinker.mForwardInfo.Gate2PlanesConnect == null)
                {
                    RefreshPlanesConnect();

                    RPC.PackageWriter retPkg = new RPC.PackageWriter();
                    retPkg.Write((sbyte)-9);//服务器忙，稍后再尝试
                    retPkg.DoReturnGate2Client(fwd);
                    return;
                }
                else
                {//位面服务器和本接入服务器绑定成功了!
                    if (HasRole(cltLinker, roleId) == false)
                    {
                        connect.Disconnect();
                        return;
                    }
                    cltLinker.mForwardInfo.RoleId = roleId;

                    RPC.PackageWriter pkg0 = new RPC.PackageWriter();
                    //告诉位面服务器玩家进入
                    if (planesData == null)
                        planesData = new CSCommon.Data.PlanesData();
                    var pos = new SlimDX.Vector3(pd.RoleDetail.LocationX, pd.RoleDetail.LocationY, pd.RoleDetail.LocationZ);
                    H_RPCRoot.smInstance.HGet_PlanesServer(pkg0).EnterMap(pkg0, pd, planesData, pd.RoleDetail.MapSourceId, mapInstanceId, pos, cltLinker.mForwardInfo.Handle);
                    pkg0.WaitDoCommand(cltLinker.mForwardInfo.Gate2PlanesConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _pkg, bool bTimeOut1)
                    {
                        if (IsSameClientLinker(cltLinker, connect) == false)
                        {
                            Log.Log.Server.Print("ClientLinker Error");
                            Log.Log.Server.Print(new System.Diagnostics.StackTrace(0, true).ToString());
                            connect.Disconnect();
                            return;
                        }

                        //#region 玩家进入位面
                        sbyte result = -1;
                        _pkg.Read(out result);

                        switch (result)
                        {
                            case (sbyte)eRet_GotoMap.EnterMap:
                                {
                                    float locX, locY, locZ;
                                    _pkg.Read(out locX);
                                    _pkg.Read(out locY);
                                    _pkg.Read(out locZ);
                                    pd.RoleDetail.LocationX = locX;
                                    pd.RoleDetail.LocationY = locY;
                                    pd.RoleDetail.LocationZ = locZ;
                                    pd.RoleDetail.RoleHp = _pkg.ReadInt32();
                                    pd.RoleDetail.RoleMaxHp = _pkg.ReadInt32();
                                    pd.RoleDetail.RoleSpeed = _pkg.ReadSingle();
                                    cltLinker.mForwardInfo.PlayerIndexInMap = _pkg.ReadUInt16();
                                    cltLinker.mForwardInfo.MapIndexInServer = _pkg.ReadUInt16();
                                    
                                    RPC.PackageWriter retPkg0 = new RPC.PackageWriter();
                                    retPkg0.SetSinglePkg();
                                    retPkg0.Write((sbyte)1);//玩家进入位面终于成功了！
                                    retPkg0.Write(pd);
                                    
                                    retPkg0.DoReturnGate2Client(fwd);
                                    cltLinker.mLandStep = Gate.LandStep.EnterGame;
                                }
                                break;
                            default:
                                {
                                    DisconnectPlayer(pd.RoleDetail.AccountId, (sbyte)eServerType.Gate);
                                    RPC.PackageWriter retPkg = new RPC.PackageWriter();
                                    retPkg.Write((sbyte)-11);//玩家进入位面失败
                                    retPkg.DoReturnGate2Client(fwd);
                                }
                                break;
                        }
                        //#endregion
                    };
                }
            };
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void ReturnToRoleSelect(Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            Gate.ClientLinker cltLinker = connect.m_BindData as Gate.ClientLinker;
            if (cltLinker == null)
                return;

            UInt16 SavedReturnSerialId = fwd.ReturnSerialId;
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            if (cltLinker.mForwardInfo.Gate2PlanesConnect != null && cltLinker.PlayerData!=null)
            {
                H_RPCRoot.smInstance.HGet_PlanesServer(pkg).ReturnToRoleSelect(pkg,cltLinker.PlayerData.RoleDetail.PlanesId,
                                    cltLinker.mForwardInfo.RoleId, cltLinker.AccountInfo.Id);
                pkg.WaitDoCommand(cltLinker.mForwardInfo.Gate2PlanesConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
                {
                    sbyte successed = -1;
                    _io.Read(out successed);

                    RPC.PackageWriter retPkg = new RPC.PackageWriter();
                    switch (successed)
                    {
                        case -1:
                            retPkg.Write((sbyte)-1);//位面ID不正确
                            retPkg.DoReturnCommand2(connect, SavedReturnSerialId);
                            break;
                        case -2:
                            retPkg.Write((sbyte)-2);//数据服务器LogoutPlayer失败
                            retPkg.DoReturnCommand2(connect, SavedReturnSerialId);
                            break;
                        case 1:
                            retPkg.Write((sbyte)1);
                            retPkg.DoReturnCommand2(connect, SavedReturnSerialId);
                            break;
                        default:
                            retPkg.Write((sbyte)2);
                            retPkg.DoReturnCommand2(connect, SavedReturnSerialId);
                            break;
                    }
                    cltLinker.InvalidLinkerWhenRoleExit();
                };
            }
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void PlayerEnterMapInPlanes(ulong roleId, UInt16 lnk, UInt16 indexInMap, UInt16 indexInServer)
        {
            Gate.ClientLinker cltLinker = FindClientLinker((UInt16)lnk);
            if (cltLinker == null)
                return;
            if (cltLinker.mForwardInfo.RoleId == roleId)
            {
                cltLinker.mForwardInfo.MapIndexInServer = indexInServer;
                cltLinker.mForwardInfo.PlayerIndexInMap = indexInMap;
            }
            else
            {
                Log.Log.Server.Print("草泥馬，出大問題了，兩個有以上的Role爭搶同一個鏈接啦！");
            }
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void PlayerLeaveMapInPlanes(UInt16 lnk)
        {
            Gate.ClientLinker cltLinker = FindClientLinker((UInt16)lnk);
            if (cltLinker == null)
                return;
            cltLinker.mForwardInfo.MapIndexInServer = UInt16.MaxValue;
            cltLinker.mForwardInfo.PlayerIndexInMap = UInt16.MaxValue;
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void OtherPlane_EnterMap(ulong planesSeverId, UInt16 cltHandle, CSCommon.Data.PlayerData pd, CSCommon.Data.PlanesData planesData,ushort mapSourceId,ulong mapInstanceId, SlimDX.Vector3 pos, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            Gate.ClientLinker cltLinker = FindClientLinker(cltHandle);
            if (cltLinker == null)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)(CSCommon.eRet_GotoMap.NoCltLinker));
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            if (cltLinker.AccountInfo.Id!=pd.RoleDetail.AccountId)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)(CSCommon.eRet_GotoMap.NoAccountId));
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            Iocp.TcpClient planesConnect = this.FindPlanesConnect(planesSeverId);
            if (planesConnect == null)
            {
                this.RefreshPlanesConnect();

                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)(CSCommon.eRet_GotoMap.NoPlaneConnect));
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            UInt16 returnSerialId = fwd.ReturnSerialId;
            cltLinker.mForwardInfo.Gate2PlanesConnect = planesConnect;
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_PlanesServer(pkg).EnterMap(pkg, pd, planesData, mapSourceId, mapInstanceId,pos, cltLinker.mForwardInfo.Handle);
            pkg.WaitDoCommand(planesConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _pkg, bool bTimeOut)
            {
                if (IsSameClientLinker(cltLinker, connect) == false)
                {
                    Log.Log.Server.Print("ClientLinker Error");
                    Log.Log.Server.Print(new System.Diagnostics.StackTrace(0, true).ToString());
                    connect.Disconnect();
                    return;
                }

                //#region 玩家进入位面
                sbyte result = -1;
                _pkg.Read(out result);

                switch (result)
                {
                    case (sbyte)eRet_GotoMap.EnterMap:
                        {
                            float locX, locY, locZ;
                            _pkg.Read(out locX);
                            _pkg.Read(out locY);
                            _pkg.Read(out locZ);
                            pd.RoleDetail.LocationX = locX;
                            pd.RoleDetail.LocationY = locY;
                            pd.RoleDetail.LocationZ = locZ;
                            pd.RoleDetail.RoleHp = _pkg.ReadInt32();
                            pd.RoleDetail.RoleMaxHp = _pkg.ReadInt32();
                            cltLinker.mForwardInfo.PlayerIndexInMap = _pkg.ReadUInt16();
                            cltLinker.mForwardInfo.MapIndexInServer = _pkg.ReadUInt16();

                            cltLinker.mLandStep = Gate.LandStep.EnterGame;
                        }
                        break;
                    default:
                        {
                            DisconnectPlayer(pd.RoleDetail.AccountId, (sbyte)eServerType.Gate);
                            RPC.PackageWriter retPkg = new RPC.PackageWriter();
                            retPkg.Write(result);
                            retPkg.DoReturnCommand2(connect, returnSerialId);
                        }
                        break;
                }


//                 sbyte successed = -1;
//                 _io.Read(out successed);
//                 switch (successed)
//                 {
//                     case (sbyte)eRet_GotoMap.EnterMap:
//                         {
//                             RPC.PackageWriter retPkg = new RPC.PackageWriter();
//                             retPkg.Write(successed);
//                             retPkg.DoReturnCommand2(connect, returnSerialId);
//                         }
//                         break;
//                     default:
//                         {
//                             RPC.PackageWriter retPkg = new RPC.PackageWriter();
//                             retPkg.Write(successed);
//                             retPkg.DoReturnCommand2(connect, returnSerialId);
// 
//                             if (cltLinker.mForwardInfo.Gate2ClientConnect != null)
//                             {
//                                 cltLinker.mForwardInfo.Gate2ClientConnect.Disconnect();
//                             }
//                         }
//                         break;
//                 }
            };
        }
        #endregion
        
        #region 角色账号管理
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void TryRegAccount(string usr, string psw, string mobileNum, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            Gate.ClientLinker cltLinker = connect.m_BindData as Gate.ClientLinker;
            if (cltLinker == null)
                return;
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).TryRegAccount(pkg, cltLinker.mForwardInfo.Handle, usr, psw, mobileNum);
            pkg.WaitDoCommand(mDataConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                sbyte successed = -1;
                _io.Read(out successed);

                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                switch(successed)
                {
                    case -1:
                        retPkg.Write( (sbyte)-1 );
                        break;
                    case -2:
                        retPkg.Write((sbyte)-2);
                        break;
                    case 1:
                        retPkg.Write((sbyte)1);
                        break;
                }
                retPkg.DoReturnGate2Client(fwd);
            };
        }
//         [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
//         public void QueryAllActivePlanesInfo(RPC.RPCForwardInfo fwd)
//         {
//             RPC.PackageWriter pkg = new RPC.PackageWriter();
//             H_RPCRoot.smInstance.HGet_DataServer(pkg).QueryAllActivePlanesInfo(pkg, fwd.Handle);
//             pkg.WaitDoCommand(mDataConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
//             {
//                 RPC.DataReader dr;
//                 _io.Read(out dr);
// 
//                 RPC.PackageWriter retPkg = new RPC.PackageWriter();
//                 UInt16 count = 0;
//                 dr.Read(out count);
//                 retPkg.Write(count);
//                 for (UInt16 i = 0; i < count; i++)
//                 {
//                     ushort id = dr.ReadUInt16();
//                     string planesName = dr.ReadString();
//                     retPkg.Write(id);
//                     retPkg.Write(planesName);
//                 }
//                 retPkg.DoReturnGate2Client(fwd);
//             };
//         }

        

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_RandRoleName(Byte sex, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).RPC_RandRoleName(pkg, sex);
            pkg.WaitDoCommand(mDataConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                string name = _io.ReadString();
                retPkg.Write(name);
                retPkg.DoReturnGate2Client(fwd);
            };
        }


        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void TryCreatePlayer(string planesName,string playerName, Byte pro, Byte sex, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            Gate.ClientLinker cltLinker = connect.m_BindData as Gate.ClientLinker;
            if (cltLinker == null)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)CSCommon.eRet_TryCreatePlayer.NetError);
                retPkg.DoReturnGate2Client(fwd);
                return;
            }
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).TryCreatePlayer(pkg, cltLinker.mForwardInfo.Handle, cltLinker.AccountInfo.Id, planesName, playerName, pro, sex);
            pkg.WaitDoCommand(mDataConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                sbyte success = -1;
                _io.Read(out success);

                if (success == 1)
                {
                    CSCommon.Data.RoleDetail ri = new CSCommon.Data.RoleDetail();
                    _io.Read(ri);
                    bool bFindRole = false;
                    foreach (var r in cltLinker.AccountInfo.Roles)
                    {
                        if (r.RoleId == ri.RoleId)
                        {
                            bFindRole = true;
                            break;
                        }
                    }
                    CSCommon.Data.RoleInfo rie = new CSCommon.Data.RoleInfo();
                    if (bFindRole == false)
                    {
                        rie.AccountId = ri.AccountId;
                        rie.RoleId = ri.RoleId;
                        rie.PlanesId = ri.PlanesId;
                        rie.PlanesName = ri.PlanesName;
                        rie.RoleName = ri.RoleName;
                        //rie.MapName = ri.MapName;
                        rie.LimitLevel = ri.LimitLevel;
                        cltLinker.AccountInfo.Roles.Add(rie);
                    }
                    

                    RPC.PackageWriter retPkg = new RPC.PackageWriter();
                    retPkg.SetSinglePkg();
                    retPkg.Write((sbyte)CSCommon.eRet_TryCreatePlayer.Succeed);
                    retPkg.Write(rie);
                    retPkg.DoReturnGate2Client(fwd);
                }
                else if (success == -1)
                {
                    RPC.PackageWriter retPkg = new RPC.PackageWriter();
                    retPkg.Write((sbyte)CSCommon.eRet_TryCreatePlayer.SameName);
                    retPkg.DoReturnGate2Client(fwd);
                }
            };
        }
        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        //public string RPC_RandomName(Byte sex, RPC.RPCForwardInfo fwd)
        //{
        //    string playername = CSCommon.Data.RoleTemplateManager.Instance.RandomName(CSCommon.Data.ItemData.Rand, (CSCommon.eRoleSex)sex);

        //    return playername;
        //}
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public sbyte DeleteRole(ushort planesId, string roleName, Iocp.NetConnection connect)
        {
            Gate.ClientLinker cltLinker = connect.m_BindData as Gate.ClientLinker;
            if (cltLinker == null)
                return -1;

            foreach (var m in cltLinker.AccountInfo.Roles)
            {
                if (m.RoleName == roleName && m.PlanesId == planesId)
                {
                    RPC.PackageWriter pkg = new RPC.PackageWriter();
                    H_RPCRoot.smInstance.HGet_DataServer(pkg).DeleteRole(pkg, planesId, roleName);                    
                    pkg.DoCommand(mDataConnect, RPC.CommandTargetType.DefaultType);
                    cltLinker.AccountInfo.Roles.Remove(m);
                    return 1;
                }
            }
            return -2;
        }
        #endregion

        #region GM请求
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadItemTemplate(UInt16 item , Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).SYS_ReloadItemTemplate(pkg, item);
            pkg.DoCommand(mDataConnect, RPC.CommandTargetType.DefaultType);
        }       
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadTaskTemplate(UInt16 item, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).SYS_ReloadTaskTemplate(pkg, item);
            pkg.DoCommand(mDataConnect, RPC.CommandTargetType.DefaultType);
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadDropTemplate(UInt16 item, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).SYS_ReloadDropTemplate(pkg, item);
            pkg.DoCommand(mDataConnect, RPC.CommandTargetType.DefaultType);
        }       
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadSellerTemplate(UInt16 item, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).SYS_ReloadSellerTemplate(pkg, item);
            pkg.DoCommand(mDataConnect, RPC.CommandTargetType.DefaultType);
        }
        #endregion

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public sbyte DisconnectPlayer(ulong accountId, sbyte serverType)
        {
            Gate.ClientLinker cltLinker = FindClientLinkerByAccountId(accountId);
            if (cltLinker == null)
                return -1;

            if (cltLinker.mForwardInfo.Gate2ClientConnect != null)
            {
                cltLinker.mForwardInfo.Gate2ClientConnect.Disconnect();
                Log.Log.Server.Print("Server force disconnect player!");
                return 1;
            }
            return -2;
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void DisconnectPlayerByConnectHandle(UInt16 lnk)
        {
            Gate.ClientLinker cltLinker = FindClientLinker(lnk);
            if (cltLinker == null)
                return;
            if (cltLinker.mForwardInfo.Gate2ClientConnect != null)
                cltLinker.mForwardInfo.Gate2ClientConnect.Disconnect();
        }

        ///// <summary>
        ///// 连接之前客户端断开，而服务器未断开的连接
        ///// </summary>
        ///// <param name="linkSerialId"></param>
        ///// <param name="connect"></param>
        ///// <param name="fwd"></param>
        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        //public void ClientLoginBySerialId(ulong linkSerialId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        //{
        //    Gate.ClientLinker cltLinker = connect.m_BindData as Gate.ClientLinker;
        //    if (cltLinker == null)
        //    {
        //        connect.Disconnect();
        //        return;
        //    }

        //    var linker = Gate.KeepLinkerManager.Instance.PeekLinker(linkSerialId);
        //    if (linker == null)
        //    {
        //        connect.Disconnect();
        //        return;
        //    }

        //    cltLinker.LinkerSerialId = linker.LinkerSerialId;
        //    cltLinker.mLandStep = linker.mLandStep;
        //    cltLinker.mAccountInfo = linker.mAccountInfo;
        //    cltLinker.mPlayerData = linker.mPlayerData;

        //    //cltLinker.mForwardInfo.Handle这个不能赋值，否则连接下标就不正确
        //    cltLinker.mForwardInfo.LimitLevel = linker.mForwardInfo.LimitLevel;
        //    cltLinker.mForwardInfo.MapIndexInServer = linker.mForwardInfo.MapIndexInServer;
        //    cltLinker.mForwardInfo.PlayerIndexInMap = linker.mForwardInfo.PlayerIndexInMap;
        //    cltLinker.mForwardInfo.ReturnSerialId = linker.mForwardInfo.ReturnSerialId;
        //    cltLinker.mForwardInfo.RoleId = linker.mForwardInfo.RoleId;

        //    //cltLinker.mForwardInfo.Planes2GateConnect = linker.mForwardInfo.Planes2GateConnect;这个不用赋值，是位面服务器用的
        //    //cltLinker.mForwardInfo.Gate2ClientConnect这个不能赋值，这个是对客户端连接的维护，不能改变
        //    cltLinker.mForwardInfo.Gate2PlanesConnect = linker.mForwardInfo.Gate2PlanesConnect;
        //}

        ///// <summary>
        ///// 离线，但是希望保持连接
        ///// </summary>
        ///// <param name="connect"></param>
        ///// <param name="fwd"></param>
        ///// <returns></returns>
        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        //public Guid OffLineKeepLinker(Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        //{
        //    Gate.ClientLinker cltLinker = connect.m_BindData as Gate.ClientLinker;
        //    if (cltLinker == null)
        //    {
        //        connect.Disconnect();
        //        return Guid.Empty;
        //    }
        //    Gate.KeepLinkerManager.Instance.KeepLinker(cltLinker);

        //    return cltLinker.LinkerSerialId;
        //}

        #endregion
    }

    public class RPCGateServerForwardImpl : RPC.RPCGateServerForward
	{
        public IGateServer mServer;
        public override RPC.RPCForwardInfo GetForwardInfo(Iocp.NetConnection sender)
		{
            Iocp.TcpConnect connect = sender as Iocp.TcpConnect;
            if (connect == null)
                return null;
            Gate.ClientLinker cltLinker = connect.m_BindData as Gate.ClientLinker;
            if (cltLinker == null)
                return null;
            return cltLinker.mForwardInfo;
		}
        public override Iocp.NetConnection GetConnectByHandle(System.UInt16 handle)
        {
            Gate.ClientLinker cltLinker = mServer.FindClientLinker(handle);
            if (cltLinker == null)
                return null;
            return cltLinker.mForwardInfo.Gate2ClientConnect;
        }
	};
}
