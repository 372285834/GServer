using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using CSCommon;

namespace ServerCommon
{
    public enum PlanesServerState
    {
        None,
        WaitRegServer,
        WaitDataServer,
        Working,
    }
    [ServerCommon.Editor.CDataEditorAttribute(".planesrv")]
    [Serializable]
    public class IPlanesServerParameter
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
        UInt16 mListenPort = 24000;
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

    [RPC.RPCClassAttribute(typeof(IPlanesServer))]
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public partial class IPlanesServer : RPC.RPCObject
    {
        #region RPC必须的基础定义
        public static RPC.RPCClassInfo smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
        {
            return smRpcClassInfo;
        }
        #endregion     
   
        #region 核心数据
        public static IPlanesServer Instance;
        protected Iocp.TcpServer mTcpSrv = new Iocp.TcpServer();
        protected Iocp.TcpClient mRegisterConnect = new Iocp.TcpClient();
        protected Iocp.TcpClient mDataConnect = new Iocp.TcpClient();
        protected Iocp.TcpClient mComConnect = new Iocp.TcpClient();

        public Iocp.TcpClient DataConnect
        {
            get { return mDataConnect; }
        }

        public Iocp.TcpClient ComConnect
        {
            get { return mComConnect; }
        }

        //protected Iocp.TcpClient mPathFindConnect = new Iocp.TcpClient();
        //public Iocp.TcpClient PathFindConnect
        //{
        //    get { return mPathFindConnect; }
        //}

        PlanesServerState mLinkState = PlanesServerState.None;
        public PlanesServerState LinkState
        {
            get { return mLinkState; }
        }

        IPlanesServerParameter mParameter;

        public IPlanesServerParameter Parameter
        {
            get { return mParameter; }
        }
        #endregion

        #region 统计属性

        public Planes.PlanesServerDataManager PlanesDataManager
        {
            get
            {
                return Planes.PlanesServerDataManager.Instance;
            }
        }

        public int AllPlayerCount
        {
            get { return Planes.PlanesServerDataManager.Instance.Players.Count; }
        }

        public int PlanesCount
        {
            get { return MapManager.PlanesManager.AllPlanesInstance.Count; }
        }

        public Planes.MapInstanceManager MapManager
        {
            get
            {
                return Planes.MapInstanceManager.Instance;
            }
        }

        public Dictionary<System.UInt16, RPC.RPCWaitHandle> WaitHandles
        {
            get { return RPC.RPCNetworkMgr.Instance.WaitHandles; }
        }

        #endregion

        #region 支持函数 
        public Planes.PlayerInstance GetPlayerByForwordInfo(RPC.RPCForwardInfo fwd)
        {
            var mapInstance = Planes.MapInstanceManager.Instance.GetMapInstance(fwd.MapIndexInServer);
            if (mapInstance == null)
                return null;
            return mapInstance.GetPlayer(fwd.PlayerIndexInMap);
        }
        #endregion

        #region 总操作
        public void Start(IPlanesServerParameter parameter)
        {
            //ServerFrame.Support.ClassInfoManager.Instance.Load(false);
            Log.FileLog.Instance.Begin("PlanesServer.log", false);
            Log.Log.Server.Print("PlanesServer 0!");

            RPCPlanesServerSpecialRootImpl impl = new RPCPlanesServerSpecialRootImpl();
            impl.mServer = this;
            RPC.RPCNetworkMgr.Instance.mPlanesServerSpecialRoot = impl;

            Log.Log.Server.Print("Register AICode Builder ===== ok!");
            IServer.LoadAllTemplateData(ServerCommon.ServerConfig.Instance.TemplatePath);
            CSCommon.CSLog.LogFun = Log.Log.Common.Warning;
            ServerCommon.TemplateTableLoader.LoadTable(ServerCommon.ServerConfig.Instance.TablePath);

            Log.Log.Server.Print("PlanesServer 1!");

            mParameter = parameter;
            mTcpSrv.ReceiveData += RPC.RPCNetworkMgr.Instance.ServerReceiveData;
            mTcpSrv.CloseConnect += this.GateServerDisConnected;

            mRegisterConnect.ReceiveData += RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mRegisterConnect.NewConnect += this.OnRegisterConnected;

            mDataConnect.ReceiveData += RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mDataConnect.NewConnect += this.OnDataServerConnected;

            mComConnect.ReceiveData += RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mComConnect.NewConnect += this.OnComServerConnected;

            //mPathFindConnect.ReceiveData += RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            //mPathFindConnect.NewConnect += this.OnFindPathServerConnected;

            mRegisterConnect.Connect(parameter.RegServerIP, parameter.RegServerPort);
            mLinkState = PlanesServerState.WaitRegServer;

            Planes.GameLogicManager.Instance.Init();

            //Log.FileLog.Instance.Begin("PlanesServer.log", false);
            Log.Log.Server.Print("PlanesServer Start ===== ok!");

            Planes.LogicProcessorManager.Instance.StartProcessors(2);

            Log.Log.Server.Print("StartProcessors ===== ok!");
            Log.FileLog.Instance.Flush();


            ServerFrame.TimerManager.doLoop(5, EventDispacthAutoRemove);
        }

        int mLastPlayerCount = 0;
        int mLastPlaneCount = 0;
        public void EventDispacthAutoRemove(ServerFrame.TimerEvent ev)
        {
            ServerCommon.Planes.EventDispatcher.AutoRemoveNoRefEventListener();

            if (mLastPlayerCount != AllPlayerCount)
            {
                RPC.PackageWriter pkg0 = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_DataServer(pkg0).UpdatePlanesServerPlayerNumber(pkg0, AllPlayerCount);
                pkg0.DoCommand(mDataConnect, RPC.CommandTargetType.DefaultType);

                mLastPlayerCount = AllPlayerCount;
            }

            if (mLastPlaneCount != PlanesCount)
            {
                RPC.PackageWriter pkg0 = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_DataServer(pkg0).UpdatePlanesServerPlanesNumber(pkg0, PlanesCount);
                pkg0.DoCommand(mDataConnect, RPC.CommandTargetType.DefaultType);

                mLastPlaneCount = PlanesCount;
            }

        }

        public void Stop()
        {
            Planes.LogicProcessorManager.Instance.StopProcessors();
            //Navigation.INavigation.Instance.CleanupPathFinder();

            mTcpSrv.ReceiveData -= RPC.RPCNetworkMgr.Instance.ServerReceiveData;
            mTcpSrv.CloseConnect -= this.GateServerDisConnected;

            mRegisterConnect.ReceiveData -= RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mRegisterConnect.NewConnect -= this.OnRegisterConnected;

            mDataConnect.ReceiveData -= RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mDataConnect.NewConnect -= this.OnDataServerConnected;

            mComConnect.ReceiveData -= RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mComConnect.NewConnect -= this.OnComServerConnected;

            //mPathFindConnect.ReceiveData -= RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            //mPathFindConnect.NewConnect -= this.OnFindPathServerConnected;

            mDataConnect.Close();
            mComConnect.Close();
            mRegisterConnect.Close();
            mTcpSrv.Close();
            System.Diagnostics.Debug.WriteLine("位面服务器关闭");
            mLinkState = PlanesServerState.None;

            Log.FileLog.Instance.End();
        }

        Int64 mTryRegServerReconnectTime;
        Int64 mTryDataServerReconnectTime;
        public void Tick()
        {
            IServer.Instance.Tick();

            #region 处理服务器连接
            var time = IServer.timeGetTime();
            if (mRegisterConnect.State == Iocp.NetState.Disconnect || mRegisterConnect.State == Iocp.NetState.Invalid)
            {
                if (time - mTryRegServerReconnectTime > 3000)
                {
                    mTryRegServerReconnectTime = time;
                    mRegisterConnect.Reconnect();
                }
            }
            
            if (mDataConnect.State == Iocp.NetState.Disconnect || mDataConnect.State == Iocp.NetState.Invalid)
            {
                if (time - mTryDataServerReconnectTime > 3000)
                {
                    mTryDataServerReconnectTime = time;
                    if (mRegisterConnect.State == Iocp.NetState.Connect)
                    {
                        ConnectDataServer();
                    }
                }
            }

            if (mComConnect.State == Iocp.NetState.Disconnect || mComConnect.State == Iocp.NetState.Invalid)
            {
                if (time - mTryDataServerReconnectTime > 3000)
                {
                    mTryDataServerReconnectTime = time;
                    if (mRegisterConnect.State == Iocp.NetState.Connect)
                    {
                        ConnectComServer();
                    }
                }
            }

            //if (mPathFindConnect.State != Iocp.NetState.Connect)
            //{
            //    if (time - mTryDataServerReconnectTime > 3000)
            //    {
            //        mTryDataServerReconnectTime = time;
            //        if (mRegisterConnect.State == Iocp.NetState.Connect)
            //        {
            //            ConnectPathFindServer();
            //        }
            //    }
            //}
            #endregion

            mRegisterConnect.Update();
            mDataConnect.Update();
            mComConnect.Update();
            //mPathFindConnect.Update();
            mTcpSrv.Update();

            RPC.RPCNetworkMgr.Instance.Tick(IServer.Instance.GetElapseMilliSecondTime());

            //Planes.PlanesManager.Instance.TickPlanesInstance();
           
        }

        Dictionary<Guid, Guid> mRefreshFSMTemplates = new Dictionary<Guid, Guid>();
        public void RefreshFSMTemplate(Guid id)
        {
            lock (this)
            {
                mRefreshFSMTemplates[id] = id;
            }
        }

        Dictionary<Guid, Guid> mRefreshEventCallBacks = new Dictionary<Guid, Guid>();
        public void RefreshEventCallBack(Guid id)
        {
            lock (this)
            {
                mRefreshEventCallBacks[id] = id;
            }
        }

        Dictionary<string, string> mRefreshActionNotifys = new Dictionary<string, string>();
        public void RefreshActionNotify(string name)
        {
            lock (this)
            {
                mRefreshActionNotifys[name] = name;
            }
        }
        #endregion

        #region 服务器各种注册流程
        string SelectDataServerIP(string[] ips)
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
            //mParameter.ListenIP = SelectDataServerIP(ips);

            H_RPCRoot.smInstance.HGet_RegServer(pkg).RegPlanesServer(pkg, mParameter.ServerId);
            pkg.WaitDoCommand(mRegisterConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                UInt16 port;
                _io.Read(out port);
                mParameter.ListenPort = port;

                System.Diagnostics.Debug.WriteLine("位面服务器({0})启动并且注册成功，可以等待连接服务器接入了", mParameter.ServerId);

                mLinkState = PlanesServerState.WaitDataServer;

                //ConnectDataServer();

                //ConnectPathFindServer();
            };
        }

        void ConnectDataServer()
        {
            Instance = this;
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_RegServer(pkg).GetDataServer(pkg);
            pkg.WaitDoCommand(mRegisterConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.DataReader dr;
                _io.Read(out dr);
                string gsIpAddress = "";
                dr.Read(out gsIpAddress);
                UInt16 gsPort = 0;
                dr.Read(out gsPort);
                if (string.IsNullOrEmpty(gsIpAddress))
                    return;
                mDataConnect.Connect(gsIpAddress, gsPort);
            };
        }

        void ConnectComServer()
        {
            Instance = this;
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_RegServer(pkg).GetComServer(pkg);
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
                    if (mLinkState == PlanesServerState.WaitRegServer)
                    {
                        mComConnect.Connect(gsIpAddress, gsPort);
                        System.Diagnostics.Debug.WriteLine("PlanesServer成功连接RegServer，尝试连接DataServer:" + gsIpAddress + ":" + gsPort);
                    }
                    else
                    {
                        mComConnect.Connect(gsIpAddress, gsPort);
                        System.Diagnostics.Debug.WriteLine("PlanesServer断线，重新从RegServer获得地址，尝试连接DataServer:" + gsIpAddress + ":" + gsPort);
                    }
                }
            };
        }

        void OnComServerConnected(Iocp.TcpClient pClient, byte[] pData, int nLength)
        {

        }


        //yzb
        void OnDataServerConnected(Iocp.TcpClient pClient, byte[] pData, int nLength)
        {
            if (nLength == 0)
                return;
            if (mLinkState != PlanesServerState.Working)
            {
                if (false == mTcpSrv.Open(Iocp.TcpOption.ForPlanesServer,mParameter.ListenPort))
                    return;
                System.Diagnostics.Debug.WriteLine("DateServer连接成功，PlanesServer开始接受GateServer接入");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("PlanesServer与DataServer断线后重连接成功");
            }
            mLinkState = PlanesServerState.Working;

            //注册到data服务器
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).RegPlanesServer(pkg, mParameter.ListenIP, mParameter.ListenPort, mParameter.ServerId);

            pkg.WaitDoCommand(mDataConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.DataReader dr;
                _io.Read(out dr);

                //读取区服务器信息
                int liCount = 0;
                dr.Read(out liCount);
                for (int i = 0; i < liCount; i++)
                {
                    ushort lAreaId = 0;
                    string lAreaName = "";
                    dr.Read(out lAreaId);
                    dr.Read(out lAreaName);

                    CSCommon.Data.PlanesData planesData = new CSCommon.Data.PlanesData();
                    planesData.PlanesId = lAreaId;      //区ID
                    planesData.PlanesName = lAreaName;  //区名
                    Planes.MapInstanceManager.Instance.PlanesManager.GetPlanesInstance(planesData);
                }

                //地图信息
                liCount = 0;
                dr.Read(out liCount);
                for (int i = 0; i < liCount; i++)
                {
                    int liAreaId = 0;
                    dr.Read(out liAreaId);
                    int liMapId = 0;
                    dr.Read(out liMapId);

                    //启动地图
                    Planes.PlanesInstance planes;
                    if (Planes.MapInstanceManager.Instance.PlanesManager.AllPlanesInstance.TryGetValue((ulong)liAreaId, out planes))
                    {
                        Planes.MapInstance map = null;
                        map = planes.GetGlobalMap((ushort)liMapId);
                        if (map == null)
                        {
                            map = Planes.MapInstanceManager.Instance.CreateMapInstance(planes, 0, (ushort)liMapId, null);
                            if (map == null)
                            {
                                map = Planes.MapInstanceManager.Instance.GetDefaultMapInstance(planes);
                            }
                            planes.AddGlobalMap((ushort)liMapId, map);
                        }
                    }
                }
            };

            //同步国战信息
            PlanesCountryWar.CCountryWarMgr.Instance.Start();

        }

        void OnFindPathServerConnected(Iocp.TcpClient pClient, byte[] pData, int nLength)
        {
            if (nLength == 0)
                return;
        }

        Dictionary<Iocp.NetConnection, ServerFrame.NetEndPoint> mGateServers = new Dictionary<Iocp.NetConnection, ServerFrame.NetEndPoint>();
        public Dictionary<Iocp.NetConnection, ServerFrame.NetEndPoint> GateServers
        {
            get { return mGateServers; }
        }

        #endregion

        #region GateServer连接和断开处理
        void GateServerDisConnected(Iocp.TcpConnect pConnect, Iocp.TcpServer pServer, byte[] pData, int nLength)
        {
            mGateServers.Remove(pConnect);
        }
        #endregion

        #region RPC method
        Planes.GameMaster mGameMaster;
        [RPC.RPCChildObjectAttribute(0, (int)RPC.RPCExecuteLimitLevel.GM, false)]
        public Planes.GameMaster GameMaster
        {
            get 
            {
                if (mGameMaster == null)
                {
                    mGameMaster = new Planes.GameMaster();
                    //mGameMaster.mPlanesSever = this;
                }
                return mGameMaster; 
            }
        }
//         [RPC.RPCIndexObjectAttribute(0, typeof(ulong), (int)RPC.RPCExecuteLimitLevel.All, false)]
//         public Planes.PlanesInstance this[ulong i]
//         {
//             get
//             {
//                 return Planes.PlanesManager.Instance.FindPlanesByPlanesId(i);
//             }
//         }
//         public Planes.PlanesInstance GetPlanes(Byte index)
//         {
//             return Planes.PlanesManager.Instance.GetPlanes(index);
//         }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RegGateServer(string ip, UInt16 port, ulong id, Iocp.NetConnection connect)
        {
            ServerFrame.NetEndPoint nep = new ServerFrame.NetEndPoint(ip, port);
            nep.Id = id;
            nep.Connect = connect;

            ServerFrame.NetEndPoint oldnep;
            if (mGateServers.TryGetValue(connect, out oldnep) == true)
            {
                mGateServers[connect] = nep;
            }
            else
            {
                mGateServers.Add(connect, nep);
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public ulong GetPlanesServerId()
        {
            return mParameter.ServerId;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void ClientDisConnect(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            //清理干净角色后，返回给DS玩家的PlayerData
            Planes.PlayerInstance player = Planes.PlanesServerDataManager.Instance.FindPlayerInstance(roleId);
            if (player == null)
            {
                Log.Log.Server.Print("ClientDisConnect:找不到角色");
                return;
            }
            Planes.MapInstance map = player.HostMap;
            Planes.PlanesInstance planes = player.PlanesInstance;

            if (map != null)
            {
                planes = player.PlanesInstance;
                map.PlayerLeaveMap(player, true);//退出地图，并且存盘
            }
            else
            {
                Log.Log.Server.Print("ClientDisConnect:找不到地图");
                return;
            }

            if (planes != null)
            {
                planes.LeavePlanes(roleId);//退出位面
            }
            else
            {
                Log.Log.Server.Print("ClientDisConnect:位面ID不正确");
                return;
            }

            Planes.PlanesServerDataManager.Instance.RemovePlayerInstance(player);//退出服务器

            //客户端连接断开，需要告诉数据服务器，登出账号
            RPC.PackageWriter pkg0 = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg0).HGet_PlayerManager(pkg0).LogoutAccount(pkg0, player.AccountId, (sbyte)eServerType.Planes);
            pkg0.DoCommand(mDataConnect, RPC.CommandTargetType.DefaultType);

            return;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RemoveInstanceMap(ulong mapInstanceId)
        {
            ServerCommon.Planes.MapInstanceManager.Instance.RemoveInstanceMap(mapInstanceId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void ReturnToRoleSelect(ulong planesId, ulong roleId, ulong accountId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {//这里的fwd只能用ReturnSerialId
            Planes.PlayerInstance player = Planes.PlanesServerDataManager.Instance.FindPlayerInstance(roleId);
            if (player == null)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)-1);//找不到角色
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            Planes.MapInstance map = player.HostMap;
            Planes.PlanesInstance planes = null;
            
            if (map != null)
            {
                planes = map.Planes;
                map.PlayerLeaveMap(player,true);//退出地图
            }
            else
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)-2);//找不到地图
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            if (planes != null)
            {
                planes.LeavePlanes(roleId);//退出位面
            }
            else
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)-3);//位面ID不正确
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            Planes.PlanesServerDataManager.Instance.RemovePlayerInstance(player);//退出服务器

            UInt16 SavedReturnSerialId = fwd.ReturnSerialId;
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).HGet_PlayerManager(pkg).LogoutRole(pkg, roleId,player.PlayerData);
            pkg.WaitDoCommand(mDataConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                sbyte successed = -1;
                _io.Read(out successed);

                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                switch (successed)
                {
                    case -1:
                        retPkg.Write((sbyte)-2);//数据服务器LogoutPlayer失败
                        retPkg.DoReturnCommand2(connect, SavedReturnSerialId);
                        break;
                    case 1:
                    case 2:
                        retPkg.Write((sbyte)1);
                        retPkg.DoReturnCommand2(connect, SavedReturnSerialId);
                        break;
                    default:
                        retPkg.Write((sbyte)2);
                        retPkg.DoReturnCommand2(connect, SavedReturnSerialId);
                        break;
                }
            };
        }

        #region GM请求
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadItemTemplate(UInt16 item, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
           // CSCommon.ItemTemplateManager.Instance.LoadItem(item);
        }        
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadRoleTemplate(UInt16 item, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadTaskTemplate(UInt16 item, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            //CSCommon.Data.Task.TaskTemplateManager.Instance.GetTask(item, true);
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadDropTemplate(int item, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            CSCommon.DropTemplateManager.Instance.GetTemplate(item);
        }       
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadSellerTemplate(UInt16 item, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
           // CSCommon.Data.Item.ItemSellerTemplateManager.Instance.LoadItem(item);
        }
        #endregion


        #region 角色进出地图
        //这个函数必须是GateServer调用的
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void EnterMap(CSCommon.Data.PlayerData pd, CSCommon.Data.PlanesData planesData, ushort mapSourceId, ulong instanceId,SlimDX.Vector3 pos, UInt16 cltHandle, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
//             var map = Planes.PlanesManager.Instance.GetGlobalMap(planesData, pd.RoleDetail.MapSourceId);
//             if (map == null)
//                 return;
            
            Planes.PlayerInstance player = Planes.PlanesServerDataManager.Instance.FindPlayerInstance(pd.RoleDetail.RoleId);
            if (player != null)
            {//玩家就在这个服务器
                Planes.MapInstanceManager.Instance.PlayerLeaveMap(player, false);//离开地图
                player.PlanesInstance.LeavePlanes(player.Id);//离开位面
                var planes = Planes.MapInstanceManager.Instance.PlanesManager.GetPlanesInstance(planesData);
                planes.EnterPlanes(player);//进位面

                Planes.MapInstanceManager.Instance.PlayerEnterMap(player, mapSourceId, instanceId, pos, true);//进地图
            }
            else
            {
                #region CreatePlayerInstance
                try
                {
                    player = Planes.PlayerInstance.CreatePlayerInstance(pd, connect as Iocp.TcpConnect, cltHandle);
                    if (player==null)//创建角色
                    {
                        RPC.PackageWriter pkg = new RPC.PackageWriter();
                        H_RPCRoot.smInstance.HGet_GateServer(pkg).DisconnectPlayer(pkg, pd.RoleDetail.AccountId, (sbyte)eServerType.Planes);
                        pkg.DoCommand(connect, RPC.CommandTargetType.DefaultType);

                        RPC.PackageWriter retPkg = new RPC.PackageWriter();
                        retPkg.Write((sbyte)eRet_GotoMap.CreatePlayerFailed);
                        retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);

                        Log.Log.Server.Print("DataServer1 force disconnect player!");
                        Log.Log.Server.Print(new System.Diagnostics.StackTrace(0, true).ToString());
                        return;
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Log.Server.Print(ex.ToString());
                    Log.Log.Server.Print(ex.StackTrace.ToString());
                    RPC.PackageWriter pkg = new RPC.PackageWriter();
                    H_RPCRoot.smInstance.HGet_GateServer(pkg).DisconnectPlayer(pkg, pd.RoleDetail.AccountId, (sbyte)eServerType.Planes);
                    pkg.DoCommand(connect, RPC.CommandTargetType.DefaultType);
                    RPC.PackageWriter retPkg = new RPC.PackageWriter();
                    retPkg.Write((sbyte)eRet_GotoMap.CreatePlayerFailed);
                    retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);

                    Log.Log.Server.Print("PlanesServer2 force disconnect player!");
                    Log.Log.Server.Print(new System.Diagnostics.StackTrace(0, true).ToString());
                    return;
                }
                #endregion
                Planes.PlanesServerDataManager.Instance.AddPlayerInstance(player);//进服务器
                var planes = Planes.MapInstanceManager.Instance.PlanesManager.GetPlanesInstance(planesData);
                planes.EnterPlanes(player);//进位面

                Planes.MapInstanceManager.Instance.PlayerEnterMap(player, mapSourceId, instanceId, pos, true);//进地图

                var overPkg = new RPC.PackageWriter();
                Wuxia.H_RpcRoot.smInstance.RPC_OnJumpMapOver(overPkg, (int)mapSourceId, pos.X, pos.Z);
                overPkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);

            }

            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)eRet_GotoMap.EnterMap);
                retPkg.Write(pd.RoleDetail.LocationX);
                retPkg.Write(pd.RoleDetail.LocationY);
                retPkg.Write(pd.RoleDetail.LocationZ);
                //retPkg.Write(player.Id);
                retPkg.Write(player.PlayerData.RoleDetail.RoleHp);
                retPkg.Write(player.PlayerData.RoleDetail.RoleMaxHp);
                retPkg.Write(player.PlayerData.RoleDetail.RoleSpeed);
                retPkg.Write(player.IndexInMap);
                retPkg.Write(player.HostMap.IndexInServer);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
            }

            return;
        }

        #endregion

        #region 关于消息

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, true)]
        public void RPC_DSTalkMsg(ulong planesId, string sender, sbyte channel, ulong targetId, string msg, RPC.DataReader hyperlink)
        {
            RPC.DataWriter data = new RPC.DataWriter();
            var link = hyperlink.ReadDataReader();
            data.Write(link.mHandle);
            var player = Planes.PlanesServerDataManager.Instance.FindPlayerInstance(targetId);
            if (null != player)
            {
                Planes.PlayerInstance.SendTalkMsg2Client(player, channel, sender, msg, data);
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_AddGiftCount(ulong id, int index, int count)
        {
            var player = Planes.PlanesServerDataManager.Instance.FindPlayerInstance(id);
            if (null != player)
            {
                Planes.PlayerInstance.SendGift2Client(player, index, count);
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_SendPlayerMsg(ulong id, CSCommon.Data.Message msg)
        {
            var player = Planes.PlanesServerDataManager.Instance.FindPlayerInstance(id);
            if (player == null)
            {
                Log.Log.Guild.Print("RPC_SendPlayerMsg player is null");
                return;
            }
            Planes.PlayerInstance.SendMsg2Client(player, msg);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_SendPlayerMail(ulong id)
        {
            var player = Planes.PlanesServerDataManager.Instance.FindPlayerInstance(id);
            if (player == null)
            {
                Log.Log.Guild.Print("RPC_SendPlayerMail player is null");
                return;
            }
            Planes.PlayerInstance.NoticeNewMail2Client(player);
        }

        //yzb
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_SendPlayerTeamInfo(ulong id, RPC.DataReader dr)
        {
            var player = Planes.PlanesServerDataManager.Instance.FindPlayerInstance(id);
            if (player == null)
            {
                Log.Log.Common.Print("RPC_SendPlayerTeamInfo player is null");
                return;
            }
            List<CSCommon.Data.RoleCom> infos = new List<CSCommon.Data.RoleCom>();
            RPC.IAutoSaveAndLoad.DaraReadList<CSCommon.Data.RoleCom>(infos, dr, false);

            RPC.DataWriter dw = new RPC.DataWriter();
            RPC.IAutoSaveAndLoad.DaraWriteList<CSCommon.Data.RoleCom>(infos, dw, true);
            Planes.PlayerInstance.SendTeam2Client(player, dw);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_SendToLeaveTeamPlayer(ulong id)
        {
            var player = Planes.PlanesServerDataManager.Instance.FindPlayerInstance(id);
            if (player == null)
            {
                Log.Log.Common.Print("RPC_SendPlayerTeamInfo player is null");
                return;
            }
            Planes.PlayerInstance.SendToLeaveTeamPlayerToClient(player);
        }
        #endregion

        #endregion 
    }


    public class RPCPlanesServerSpecialRootImpl : RPC.RPCPlanesServerSpecialRoot
	{
        public IPlanesServer mServer;
        public override void Push2Processor(RPC.RPCSpecialHolder holder,int ptype)
		{
            if (ptype == (int)RPC.PackageType.PKGT_C2P_Player_Send || ptype == (int)RPC.PackageType.PKGT_C2P_Player_SendAndWait)
            {
                Planes.MapInstance mapInstance = Planes.MapInstanceManager.Instance.GetMapInstance(holder.mForward.MapIndexInServer);
                if (mapInstance == null)
                {
                    holder.DestroyBuffer();
                    Log.Log.Server.Info("玩家地图索引非法");
                    return;
                }

                RPC.RPCSpecialHolderProcessor RpcProcessor = mapInstance.RpcProcessor;
                if (RpcProcessor == null)
                {
                    holder.DestroyBuffer();
                    Log.Log.Server.Info("地图的RPC处理器不合法");
                    //RPC.PackageWriter pkg = new RPC.PackageWriter();
                    //H_RPCRoot.smInstance.HGet_GateServer(pkg).DisconnectPlayerByConnectHandle(pkg, holder.mForward.Handle);
                    //pkg.DoCommand(holder.mForward.Planes2GateConnect, RPC.CommandTargetType.DefaultType);
                    return;
                }
                Planes.PlayerInstance player = mapInstance.GetPlayer(holder.mForward.PlayerIndexInMap);
                if (player == null)
                {//切换地图后还有数据包发来该位面
                    holder.DestroyBuffer();
                    
                    //Log.FileLog.WriteLine("找不到RPC处理的玩家");
                    //RPC.PackageWriter pkg = new RPC.PackageWriter();
                    //H_RPCRoot.smInstance.HGet_GateServer(pkg).DisconnectPlayerByConnectHandle(pkg, holder.mForward.Handle);
                    //pkg.DoCommand(holder.mForward.Planes2GateConnect, RPC.CommandTargetType.DefaultType);
                    return;
                }
                else
                {
                    if (player.Id != holder.mForward.RoleId)
                    {
                        holder.DestroyBuffer();
                        Log.FileLog.WriteLine("RPC玩家Id不符合");
                        return;
                    }
                    holder.mRoot = player;
                    //RPC.RPCSpecialHolderProcessor.Process(holder);
                    RpcProcessor.PushHolder(holder);
                    return;
                }
            }

            holder.DestroyBuffer();
            return;
		}

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RemoveInstanceMap(ulong mapInstanceId)
        {
            ServerCommon.Planes.MapInstanceManager.Instance.RemoveInstanceMap(mapInstanceId);
        }

        public override string GetPlayerInfoString(RPC.RPCForwardInfo fwd)
		{
//             Planes.PlanesInstance planes = mServer.GetPlanes(fwd.PlanesIndexInServer);
//             if (planes == null)
//                 return "Unknown Planes";
            return "Unknown Planes";
        }
	};
}
