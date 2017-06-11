using CSCommon;
using ServerFrame;
using ServerFrame.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon
{
    public enum DataServerState
    {
        None,
        WaitRegServer,
        Working,
    }

    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class PlanesServerInfo
    {
        ServerFrame.NetEndPoint mEndPoint;
        public ServerFrame.NetEndPoint EndPoint
        {
            get { return mEndPoint; }
            set { mEndPoint = value; }
        }
        
        int mPlanesNumber = 0;
        public int PlanesNumber
        {
            get { return mPlanesNumber; }
            set { mPlanesNumber = value; }
        }

        int mGlobalMapNumber = 0;
        public int GlobalMapNumber
        {
            get { return mGlobalMapNumber; }
            set { mGlobalMapNumber = value; }
        }

        int mPlayerNumber = 0;
        public int PlayerNumber
        {
            get { return mPlayerNumber; }
            set { mPlayerNumber = value; }
        }
    }

    [ServerCommon.Editor.CDataEditorAttribute(".datasrv")]
    [Serializable]
    public class IDataServerParameter
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


    [RPC.RPCClassAttribute(typeof(IDataServer))]
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public partial class IDataServer : RPC.RPCObject
    {
        #region RPC必须的基础定义
        public static RPC.RPCClassInfo smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
        {
            return smRpcClassInfo;
        }
        #endregion

        #region 构造
        static IDataServer mInstance = null;
        public static IDataServer Instance
        {
            get { return mInstance; }
            set { mInstance = value; }
        }

        public IDataServer()
        {
            mPlayerManager = new Data.Player.PlayerManager();
        }
        #endregion

        #region 核心数据
        protected Iocp.TcpServer mTcpSrv = new Iocp.TcpServer();
        protected Iocp.TcpClient mRegisterConnect = new Iocp.TcpClient();

        DataServerState mLinkState = DataServerState.None;
        public DataServerState LinkState
        {
            get { return mLinkState; }
        }

        IDataServerParameter mParameter;
        public IDataServerParameter Parameter
        {
            get { return mParameter; }
        }

        ServerFrame.DB.DBConnect mDBLoaderConnect;
        public ServerFrame.DB.DBConnect DBLoaderConnect
        {
            get { return mDBLoaderConnect; }
        }
        
        Data.PlanesMgr mPlanesMgr;
        public Data.PlanesMgr PlanesMgr
        {
            get { return mPlanesMgr; }
        }

        #endregion

        #region 统计属性

        public int OnlyLoginAccountNumber
        {
            get
            {
                return mPlayerManager.GetOnlyLoginAccountNumber();
            }
        }

        public int PlayerNumber
        {
            get
            {
                return mPlayerManager.GetLoginAccountNumber();
            }
        }

        public int LoginPipeNumber
        {
            get
            {
                return Thread.AccountLoginThread.Instance.GetNumber();
            }
        }

        public int PlayerSaverNumber
        {
            get
            {
                return Thread.DBConnectManager.Instance.GetPlayerSaverNumber();
            }
        }

        public int RoleLoadPipeNumber
        {
            get
            {
                return Thread.PlayerEnterThread.Instance.GetNumber();
            }
        }

        public Dictionary<System.UInt16, RPC.RPCWaitHandle> WaitHandles
        {
            get { return RPC.RPCNetworkMgr.Instance.WaitHandles; }
        }

        public AllMapManager AllMapManager
        {
            get { return AllMapManager.Instance; }
        }

        #endregion

        #region 总操作
        public void Start(IDataServerParameter parameter)
        {
            Stop();
            
            mParameter = parameter;
            mDBLoaderConnect = new ServerFrame.DB.DBConnect();
            mDBLoaderConnect.OpenConnect();

            //ServerFrame.Support.ClassInfoManager.Instance.Load(false);

            Log.FileLog.Instance.Begin("DataServer.log", false);

            Log.Log.Server.Print("DataServer Start!");
            Log.FileLog.Instance.Flush();

            IServer.LoadAllTemplateData(ServerCommon.ServerConfig.Instance.TemplatePath);
            CSCommon.CSLog.LogFun = Log.Log.Common.Warning;
            ServerCommon.TemplateTableLoader.LoadTable(ServerCommon.ServerConfig.Instance.TablePath);

            //加载数据库中的静态配置信息
            CSCommon.Data.CDbConfig.LoadDbConfig(mDBLoaderConnect);

            //国战服务初始化
            CountryWar.CCountryWarMgr.Instance.Start(mDBLoaderConnect);


            Thread.AccountLoginThread.Instance.StartThread();
            Thread.PlayerEnterThread.Instance.StartThread();
            Thread.DBConnectManager.Instance.StartThreadPool(2);

            AsyncExecuteThreadManager.Instance.InitManager(1);
            AsyncExecuteThreadManager.Instance.StartThread();

            //mChargeManger = new Data.ChargeManager();
            
            mPlanesMgr = new Data.PlanesMgr(this);

            Log.Log.Server.Print("DBConnect OK!");

            mTcpSrv.ReceiveData += RPC.RPCNetworkMgr.Instance.ServerReceiveData;
            mTcpSrv.CloseConnect += this.ServerDisConnected;
            mRegisterConnect.ReceiveData += RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mRegisterConnect.NewConnect += this.OnRegisterConnected;
                       
            mRegisterConnect.Connect(parameter.RegServerIP, parameter.RegServerPort);
            mLinkState = DataServerState.WaitRegServer;

            InitNamePool();

            mPlayerManager.DownloadPlayerData(mDBLoaderConnect);


            /*int avgSize = 0;
            Support.PerfCounter perf = new Support.PerfCounter();
            perf.Begin();
            for (int i = 0; i < 1000; i++)
            {
                RPC.DataWriter dw0 = new RPC.DataWriter();
                CSCommon.Data.ItemData item0 = new CSCommon.Data.ItemData();
                item0.ItemTemlateId = 2;
                //item0.DangrousReInitData();
                RPC.DataWriter dw1 = new RPC.DataWriter();
                CSCommon.Data.ItemData item1 = new CSCommon.Data.ItemData();
                item1.ItemTemlateId = 2;
               // item1.DangrousReInitData();
                dw0.Write(item0);
                dw1.Write(item1);

                int compressSize = dw1.CompressWithTemplate(dw0);
                dw1.UnCompressWithTemplate(dw0);

                avgSize += compressSize;

                RPC.DataWriter dw2 = new RPC.DataWriter();
                dw2.Write(item1);
                bool isSame = dw1.IsSame(dw2);
            }
            Int64 time = perf.End();
            System.Diagnostics.Debug.WriteLine( "{0}:{1}" , avgSize / 1000, time);*/

            //test
            /*
            var start = ServerCommon.IServer.timeGetTime();

            CSCommon.Data.RoleDetail rd = new CSCommon.Data.RoleDetail();
            CSCommon.Data.RoleDetail rd2 = new CSCommon.Data.RoleDetail();
            //var r = TestAutoCode(rd);
            rd.BagSize = 155;
            rd.BindRmb = 155;
            rd.CreateTime = DateTime.Now;
            rd.LocationX = 15211;
            rd.LocationY = 15211;
            rd.LocationZ = 15211;
            rd.Rmb = 123;
            rd.RoleExp = 333;
            for (int i = 0; i < 10000; i++)
            {
                //var r = rd.GetSql(rd2);
                var r = ServerFrame.DB.DBConnect.TestUpdateData("", rd, rd2);
            }
            var end = ServerCommon.IServer.timeGetTime() - start;
            var t = end;*/

        }


        /*
        public string TestAutoCode(CSCommon.Data.RoleDetail obj)
        {
            Type objType = obj.GetType();
            object[] propsTab = objType.GetCustomAttributes(typeof(DBBindTable), false);
            if (propsTab == null)
                return null;
            DBBindTable bindTab = propsTab[0] as DBBindTable;
            System.Reflection.PropertyInfo[] props = objType.GetProperties();
            bool first = true;
            string result = "";
            string temp = "            if (this.{name} != cmp.{name}) \r\n fields.Add(new KeyValuePair<string,object>(\"{name}\",{name})); \r\n";
            foreach (System.Reflection.PropertyInfo p in props)
            {

                object[] attrs = p.GetCustomAttributes(typeof(DBBindField), true);
                if (attrs == null || attrs.Length == 0)
                    continue;
                DBBindField dbBind = attrs[0] as DBBindField;
                if (dbBind == null)
                    continue;
                result += temp.Replace("{name}", p.Name);
            }
            return result;
        }*/


        public void Stop()
        {
            AsyncExecuteThreadManager.Instance.StopThread();

            Thread.AccountLoginThread.Instance.StopThread();
            Thread.PlayerEnterThread.Instance.StopThread();
            Thread.DBConnectManager.Instance.StopThreadPool();

            while (Thread.DBConnectManager.Instance.IsEmptyPool() == false)
            {
                System.Threading.Thread.Sleep(5);
            }

            mTcpSrv.ReceiveData -= RPC.RPCNetworkMgr.Instance.ServerReceiveData;
            mTcpSrv.CloseConnect -= this.ServerDisConnected;
            mRegisterConnect.ReceiveData -= RPC.RPCNetworkMgr.Instance.ClientReceiveData;
            mRegisterConnect.NewConnect -= this.OnRegisterConnected;

            mRegisterConnect.Close();
            mTcpSrv.Close();
            mGateServers.Clear();
            System.Diagnostics.Debug.WriteLine("数据服务器关闭");
            mLinkState = DataServerState.None;

            Log.FileLog.Instance.End();
        }

        Int64 mTryRegServerReconnectTime;
        Int64 mTryRemoveLogoutPlayerTime;
        public void Tick()
        {
            IServer.Instance.Tick();
            var time = IServer.timeGetTime();
            mDBLoaderConnect.Tick();
            if (time - mTryRemoveLogoutPlayerTime > 3000)
            {//把已经完成数据存盘的玩家清理了
                mTryRemoveLogoutPlayerTime = time;
                mPlayerManager.Tick();
            }

            if (mLinkState == DataServerState.Working)
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
            
            mRegisterConnect.Update();
            mTcpSrv.Update();
            RPC.RPCNetworkMgr.Instance.Tick(IServer.Instance.GetElapseMilliSecondTime());


            AllMapManager.Instance.Tick();
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
            //mParameter.ListenIP = mParameter.//SelectDataServerIP(ips);

            H_RPCRoot.smInstance.HGet_RegServer(pkg).RegDataServer(pkg, mParameter.ListenIP, mParameter.ListenPort, mParameter.ServerId);
            pkg.WaitDoCommand(mRegisterConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                System.Diagnostics.Debug.WriteLine("数据服务器({0})启动并且注册成功，可以等待连接服务器接入了", mParameter.ServerId);

                if (mLinkState != DataServerState.Working)
                {
                    if (false == mTcpSrv.Open(Iocp.TcpOption.ForDataServer,mParameter.ListenPort))
                        return;
                }

                mLinkState = DataServerState.Working;
            };
        }

        List<CSCommon.Data.AccountInfo> accountList = new List<CSCommon.Data.AccountInfo>();
        public void ServerDisConnected(Iocp.TcpConnect pConnect, Iocp.TcpServer pServer, byte[] pData, int nLength)
        {
            accountList.Clear();
            if (mGateServers.ContainsKey(pConnect))
            {
                mGateServers.Remove(pConnect);
                foreach (var i in this.PlayerManager.AccountDict)
                {
                    if (i.Value.Data2GateConnect == pConnect && (i.Value.CurPlayer == null || i.Value.CurPlayer.mPlanesConnect == null))
                    {
                        accountList.Add(i.Value);
                    }
                }

                foreach (var account in accountList)
                {
                    PlayerManager.ClearAccount(account);
                }
            }

            mPlanesMgr.OnPlanesSeverDisconnect(pConnect);
            mPlanesServers.Remove(pConnect);

            if (null == CloseThread)
            {
                CloseThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ThreadCloses));
                CloseThread.Start();
            }
            
        }
        System.Threading.Thread CloseThread;
        void ThreadCloses()
        {
            while (true)
            {
                if (0 == mGateServers.Count && mPlanesServers.Count == 0 && this.PlayerManager.AccountDict.Count == 0)
                {
                    if (AllMapManager.Instance.GlobalMaps.Count == 0 && AllMapManager.Instance.InstanceMaps.Count == 0)
                    {
                        if (ServerCommon.Thread.DBConnectManager.Instance.IsEmptyPool())
                            System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                }
            }
        }

        Dictionary<Iocp.NetConnection, ServerFrame.NetEndPoint> mGateServers = new Dictionary<Iocp.NetConnection, ServerFrame.NetEndPoint>();
        public Dictionary<Iocp.NetConnection, ServerFrame.NetEndPoint> GateServers
        {
            get { return mGateServers; }
        }
        Dictionary<Iocp.NetConnection, PlanesServerInfo> mPlanesServers = new Dictionary<Iocp.NetConnection, PlanesServerInfo>();
        public Dictionary<Iocp.NetConnection, PlanesServerInfo> PlanesServers
        {
            get { return mPlanesServers; }
        }
        public ServerFrame.NetEndPoint FindPlanesServer(ulong serverId)
        {
            foreach (var i in mPlanesServers)
            {
                if(i.Value.EndPoint.Id ==serverId )
                    return i.Value.EndPoint;
            }
            return null;
        }
        #endregion

        #region RPC method

        Data.Player.PlayerManager mPlayerManager;
        [RPC.RPCChildObjectAttribute(0, (int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public Data.Player.PlayerManager PlayerManager
        {
            get { return mPlayerManager; }
        }

        #region Server Connect
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RegGateServer(string ip, UInt16 port, ulong id, Iocp.NetConnection connect)
        {
            ServerFrame.NetEndPoint nep = new ServerFrame.NetEndPoint(ip, port);
            nep.Id = id;
            nep.Connect = connect;

            ServerFrame.NetEndPoint oldnep;
            if ( mGateServers.TryGetValue(connect, out oldnep)==true )
            {
                mGateServers[connect] = nep;
            }
            else
            {
                mGateServers.Add(connect, nep);
            }
        }

        /// <summary>
        /// 注册位面服务器
        /// </summary>
        /// 
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public RPC.DataWriter RegPlanesServer(string ip, UInt16 port, ulong id, Iocp.NetConnection connect)
        {
            ServerFrame.NetEndPoint nep = new ServerFrame.NetEndPoint(ip, port);
            nep.Id = id;
            nep.Connect = connect;

            PlanesServerInfo oldnep;
            if (mPlanesServers.TryGetValue(connect, out oldnep) == true)
            {
                mPlanesServers[connect].EndPoint = nep;
            }
            else
            {
                oldnep = new PlanesServerInfo();
                oldnep.EndPoint = nep;
                mPlanesServers.Add(connect, oldnep);
            }

            Log.Log.Server.Print("yzb id = {0}", id);
            

            RPC.DataWriter lret = new RPC.DataWriter();

            //启动该planesserver对应的世界地图和国战地图
            AllMapManager.Instance.StartupWorldMap((int)id);

            //位面信息
            int count = this.PlanesMgr.Planes.Count;
            lret.Write(count);
            Log.Log.Server.Print("count = {0}",count);
            foreach (var i in this.PlanesMgr.Planes)
            {
                lret.Write(i.Value.PlanesId);
                lret.Write(i.Value.PlanesName);
            }

            count = 0;
            //将要启动的地图id返回给PlanesServer
            if (CSCommon.Data.CDbConfig.m_PlanesConfig.ContainsKey((int)id))
            {
                count = CSCommon.Data.CDbConfig.m_PlanesConfig[(int)id].ListMap.Count;
                lret.Write(count);
                Log.Log.Server.Print("map count = {0}", count);
                foreach (CSCommon.Data.CAreaMap lAreaMap in CSCommon.Data.CDbConfig.m_PlanesConfig[(int)id].ListMap)
                {
                    lret.Write(lAreaMap.Area);
                    lret.Write(lAreaMap.Map);
                }
            }
            else
            {
                lret.Write(count);
                Log.Log.Server.Print("map count 0 = {0}", count);
            }
            return lret;
        }
        #endregion

        #region Reg , Create , Delete Role
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.All, true)]
        public sbyte TryRegAccount(UInt16 cltLinker, string usr, string psw, string mobileNum, Iocp.NetConnection connect)
        {
            return mPlayerManager.RegAccount(Thread.AccountLoginThread.Instance.DBConnect, usr, psw, mobileNum);
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.All, true)]
        public RPC.DataWriter TryRegGuest(UInt16 cltLinker, Iocp.NetConnection connect)
        {
            return mPlayerManager.RegGuest(Thread.AccountLoginThread.Instance.DBConnect);
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public RPC.DataWriter QueryAllActivePlanesInfo(UInt16 lnk, Iocp.NetConnection connect)
        {
            return mPlanesMgr.GetAllActivePlanesInfo();
        }

        #region 随机名字
        string[] surnames = null;
        string[] malenames = null;
        string[] femalenames = null;
        void InitNamePool()
        {
            string surnamepath = ServerConfig.Instance.NamePath + "/surname.txt";
            string malenamepath = ServerConfig.Instance.NamePath + "/name.txt";
            string femalenamepath = ServerConfig.Instance.NamePath + "/femalename.txt";

            string context = ReadText(surnamepath);
            surnames = context.Split(' ');

            context = ReadText(malenamepath);
            malenames = context.Split(' ');

            context = ReadText(femalenamepath);
            femalenames = context.Split(' ');
        }

        public string ReadText(string path)
        {
            System.IO.StreamReader read = new System.IO.StreamReader(path);
            string context = read.ReadToEnd();
            return context;
        }

        Random mRandom = new Random();

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_RandRoleName(Byte sex, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            string name = _RandRoleName(sex);
            pkg.Write(name);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);

        }
        public bool _CheckName(string name)
        {
            if (name.CompareTo("") == 0)
            {
                return false;
            }
            name = ServerFrame.DB.DBConnect.SqlSafeString(name);
            string sql = "select RoleName from roleinfo where rolename = \'" + name + "\'";
            var table = Thread.PlayerEnterThread.Instance.DBConnect._ExecuteSql(sql, "roleinfo");
            if (table != null && table.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        public string _RandRoleName(Byte sex)
        {
            string name = "";
            int surIndex = mRandom.Next(surnames.Length);
            if (sex == (byte)CSCommon.eRoleSex.Men)
            {
                int maleIndex = mRandom.Next(malenames.Length);
                name = surnames[surIndex] + malenames[maleIndex];
            }
            else
            {
                int femaleIndex = mRandom.Next(femalenames.Length);
                name = surnames[surIndex] + femalenames[femaleIndex];
            }

            if (_CheckName(name))
            {
                return name;
            }

            return _RandRoleName(sex);
        }
        #endregion
        
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void TryCreatePlayer(UInt16 lnk, ulong accountId, string planeName, string plyName, Byte pro, Byte sex, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            //planesName = ServerFrame.DB.DBConnect.SqlSafeString(planesName);
            plyName = ServerFrame.DB.DBConnect.SqlSafeString(plyName);
            ulong roleId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Player);
            AsyncExecuter exe = AsyncExecuteThreadManager.Instance.AsyncExe(false);
            UInt16 returnSerialId = fwd.ReturnSerialId;
            exe.Exec = delegate()
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                CSCommon.Data.PlayerData result = mPlayerManager.TryCreatePlayer(Thread.PlayerEnterThread.Instance.DBConnect, mPlanesMgr, accountId, roleId, planeName, plyName, pro, sex);

                //告诉GateServer，创建是否成功
                if (result != null)
                {
                    retPkg.Write((sbyte)1);
                    retPkg.Write(result.RoleDetail);
                    retPkg.DoReturnCommand2(connect, returnSerialId);
                }
                else
                {
                    retPkg.Write((sbyte)-1);
                    retPkg.DoReturnCommand2(connect, returnSerialId);
                }
            };
            //这里要放到一个专门的队列创建
            Thread.PlayerEnterThread.Instance.PushRoleCreator(exe);
        }
        
        //ZY_2013-02-27
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public sbyte DeleteRole(ushort planesId, string roleName)
        {
            roleName = ServerFrame.DB.DBConnect.SqlSafeString(roleName);
            //Thread.PlayerEnterThread.Instance.DBConnect.CallSqlSP("DestoryRoleInfo", roleName, 1);
            string condition = "RoleName=" + "\'" + roleName + "\'" + " and " + "PlanesId=" + "\'" + planesId.ToString() + "\'";
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.DelData(condition, new CSCommon.Data.RoleDetail());
            if (false == Thread.PlayerEnterThread.Instance.DBConnect._ExecuteDelete(dbOp))
            {
                Log.Log.Login.Print("delete name is exist");
                return -1;
            }
            return 1;
        }
        
        #endregion

        #region Item
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public sbyte UpdateItem(ulong roleId, CSCommon.Data.ItemData item)
        {
            var pd = this.PlayerManager.FindPlayerData(roleId);
            if (pd == null)
                return -1;
            string condition = "ItemId = " + item.ItemId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, item, true);
            Thread.DBConnectManager.Instance.PushSave(pd, dbOp);
            return 1;
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void DelItem(ulong roleId, ulong itemId, sbyte bDestroy)
        {
            var pd = this.PlayerManager.FindPlayerData(roleId);
            if (pd == null)
                return;
            if (bDestroy > 0)
            {
                string condition = "ItemId = " +itemId;
                ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.DestroyData(condition, new CSCommon.Data.ItemData());
                Thread.DBConnectManager.Instance.PushSave(pd, dbOp);
            }
            else
            {
                string condition = "ItemId = " +itemId;
                ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.DelData(condition, new CSCommon.Data.ItemData());
                Thread.DBConnectManager.Instance.PushSave(pd, dbOp);
            }
        }
        #endregion


        #region Mail
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void DelMail(ulong roleId, ulong mailId)
        {
            var pd = mPlayerManager.FindPlayerData(roleId);
            if (pd == null)
            {
                return;
            }
            string condition = "MailId = " + mailId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.DestroyData(condition, new CSCommon.Data.MailData());
            Thread.DBConnectManager.Instance.PushSave(pd, dbOp);
        }
        #endregion
        
        #region Account & Role
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, true)]
        public RPC.DataWriter GetAccountInfoData(ulong accountID)
        {
            RPC.DataWriter dw = new RPC.DataWriter();

            var account = mPlayerManager.FindAccountInfo(accountID);

            if (account != null)
            {
                dw.Write((SByte)1);
                dw.Write(account,false);
            }
            else
            {
                dw.Write((SByte)(-1));
            }

            return dw;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, true)]
        public void GetRoleDetailByName(ushort planesId,string roleName, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            roleName = ServerFrame.DB.DBConnect.SqlSafeString(roleName);
            UInt16 returnSerialId = fwd.ReturnSerialId;
            AsyncExecuteThreadManager.Instance.AsyncExe(true).Exec = delegate()
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                var acInfo = mPlayerManager.FindPlayerData(planesId,roleName);

                if (acInfo != null)
                {
                    retPkg.Write((SByte)1);
                    retPkg.Write(acInfo.RoleDetail);
                    retPkg.DoReturnCommand2(connect, returnSerialId);
                }
                else
                {
                    CSCommon.Data.RoleDetail rd = new CSCommon.Data.RoleDetail();
                    string condition = "RoleName = \'" + roleName + "\'";
                    ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, rd, null);
                    System.Data.DataTable tab = DBLoaderConnect._ExecuteSelect(dbOp, "RoleInfo");
                    if (tab == null || tab.Rows.Count != 1)
                    {
                        retPkg.Write((SByte)(-1));
                        retPkg.DoReturnCommand2(connect, returnSerialId);
                    }
                    else
                    {
                        ServerFrame.DB.DBConnect.FillObject(rd, tab.Rows[0]);
                        retPkg.Write((SByte)1);
                        retPkg.Write(rd);
                        retPkg.DoReturnCommand2(connect, returnSerialId);
                    }
                }   
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, true)]
        public RPC.DataWriter GetRoleDetailData(ulong accountID)
        {
            RPC.DataWriter dw = new RPC.DataWriter();

            var acInfo = mPlayerManager.FindPlayerData(accountID);

            if (acInfo != null)
            {
                dw.Write((SByte)1);
                dw.Write(acInfo.RoleDetail,false);
            }
            else
            {
                dw.Write((SByte)(-1));
            }

            return dw;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public ulong GetPlayerGuidByName(ushort planesId, string name)
        {

            return mPlayerManager.GetPlayerGuidByName(planesId,name);
        }

        #endregion

        #region Map Manager
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void GotoMap(ulong roleId, ushort planesId, ushort mapSourceId, SlimDX.Vector3 pos, UInt16 cltHandle, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            UInt16 returnSerialId = fwd.ReturnSerialId;

            var mapInit = Planes.MapInstanceManager.GetMapInitBySourceId(mapSourceId);
            if (mapInit == null)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)(eRet_GotoMap.GetMapInitFailed));
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            CSCommon.Data.PlayerDataEx pd = IDataServer.Instance.PlayerManager.FindPlayerData(roleId);
            if (pd == null)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)(eRet_GotoMap.NoPlayerData));
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            switch ((CSCommon.eMapType)mapInit.MapData.mapType)
            {
                case CSCommon.eMapType.Master:
                    break;
//                case CSCommon.eMapType.TeamInstance:
//                     if (pd.RoleDetail.TeamId == Guid.Empty)
//                     {
//                         RPC.PackageWriter retPkg = new RPC.PackageWriter();
//                         retPkg.Write((sbyte)(-5));//没有组队不能进组队副本
//                         retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
//                         return;
//                     }
                    break;
            }

            var mapInstanceId = AllMapManager.Instance.GetInstanceMapId(pd, mapSourceId);
            var account = pd.mAccountInfo;
            PlayerManager.LogoutCurPlayer(account, null);

            account.CurPlayer = pd;
            var mapInfo = AllMapManager.Instance.GetMapConnectInfo(pd, planesId, mapSourceId, mapInstanceId);
            PlayerManager.LoginCurPlayer(account, mapInfo);

            var nep = mapInfo.mConnect;
            if (mapInfo == null || nep == null)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)(eRet_GotoMap.NoConnectInfo));
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            if (connect == nep.Connect)
            {//目的地就在原来服务器
                pd.mPlanesConnect = nep.Connect as Iocp.TcpConnect;

                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)eRet_GotoMap.SamePlane);
                if (mapInfo.PlanesPlayerManager != null)
                    retPkg.Write(mapInfo.PlanesPlayerManager.PlanesData);
                else
                    retPkg.Write(new CSCommon.Data.PlanesData());
                retPkg.Write(mapInstanceId);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            else
            {
                RPC.PackageWriter pkg1 = new RPC.PackageWriter();

                H_RPCRoot.smInstance.HGet_GateServer(pkg1).OtherPlane_EnterMap(pkg1, nep.Id, cltHandle, pd, mapInfo.PlanesPlayerManager.PlanesData, mapSourceId, mapInstanceId,pos);
                pkg1.WaitDoCommand(pd.mGateConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io1, bool bTimeOut)
                {
                    sbyte successed = -1;
                    _io1.Read(out successed);
                    switch (successed)
                    {
                        case (sbyte)eRet_GotoMap.EnterMap:
                            {
                                pd.mPlanesConnect = nep.Connect as Iocp.TcpConnect;
                                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                                retPkg.Write((sbyte)((sbyte)eRet_GotoMap.OtherPlane));//需要跳转物理服务器
                                retPkg.DoReturnCommand2(connect, returnSerialId);
                            }
                            break;
                        default:
                            {
                                pd.mPlanesConnect = nep.Connect as Iocp.TcpConnect;
                                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                                retPkg.Write((sbyte)(eRet_GotoMap.FailedEnterMap));
                                retPkg.DoReturnCommand2(connect, returnSerialId);
                                Log.Log.Server.Warning("跨plane,地图跳转失败!");
                            }
                            break;
                    }
                };
            }
        } 

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void GetPlanesInfo(ushort planesId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            CSCommon.Data.PlanesData data;
            if (false == this.PlanesMgr.Planes.TryGetValue(planesId, out data))
            {
                retPkg.Write((SByte)(-1));
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            retPkg.Write((SByte)(1));
            retPkg.Write(data);
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
            return;
        }



        #endregion



        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void UpdatePlanesServerPlanesNumber(int num, Iocp.NetConnection connect)
        {
            PlanesServerInfo info;
            if( PlanesServers.TryGetValue(connect,out info) )
            {
                info.PlanesNumber = num;
            }
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void UpdatePlanesServerGlobalMapNumber(int num, Iocp.NetConnection connect)
        {
            PlanesServerInfo info;
            if (PlanesServers.TryGetValue(connect, out info))
            {
                info.GlobalMapNumber = num;
            }
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void UpdatePlanesServerPlayerNumber(int num, Iocp.NetConnection connect)
        {
            PlanesServerInfo info;
            if (PlanesServers.TryGetValue(connect, out info))
            {
                info.PlayerNumber = num;
            }
        }

        #region GM请求
        //         [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
//         public void CreatePlanes(byte level, string name, byte state, RPC.RPCForwardInfo fwd)
//         {
//             this.PlanesMgr.CreatePlanesData(level, name, state);
//         }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadItemTemplate(UInt16 item, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
//             CSCommon.ItemTemplateManager.Instance.LoadItem(item);
//             
//             RPC.PackageWriter pkg = new RPC.PackageWriter();
//             H_RPCRoot.smInstance.HGet_PlanesServer(pkg).SYS_ReloadItemTemplate(pkg, item);
//             foreach (var i in PlanesServers)
//             {
//                 pkg.DoCommand(i.Value.EndPoint.Connect,RPC.CommandTargetType.DefaultType);
//             }
        }       
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadTaskTemplate(UInt16 item, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
//             CSCommon.Data.Task.TaskTemplateManager.Instance.GetTask(item,true);
// 
//             RPC.PackageWriter pkg = new RPC.PackageWriter();
//             H_RPCRoot.smInstance.HGet_PlanesServer(pkg).SYS_ReloadTaskTemplate(pkg, item);
//             foreach (var i in PlanesServers)
//             {
//                 pkg.DoCommand(i.Value.EndPoint.Connect, RPC.CommandTargetType.DefaultType);
//             }
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadDropTemplate(UInt16 item, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
//             CSCommon.Data.Item.DropTemplateManager.Instance.LoadDropper(item);
// 
//             RPC.PackageWriter pkg = new RPC.PackageWriter();
//             H_RPCRoot.smInstance.HGet_PlanesServer(pkg).SYS_ReloadDropTemplate(pkg, item);
//             foreach (var i in PlanesServers)
//             {
//                 pkg.DoCommand(i.Value.EndPoint.Connect, RPC.CommandTargetType.DefaultType);
//             }
        }       
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SYS_ReloadSellerTemplate(UInt16 item, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
//             CSCommon.Data.Item.ItemSellerTemplateManager.Instance.LoadItem(item);
// 
//             RPC.PackageWriter pkg = new RPC.PackageWriter();
//             H_RPCRoot.smInstance.HGet_PlanesServer(pkg).SYS_ReloadSellerTemplate(pkg, item);
//             foreach (var i in PlanesServers)
//             {
//                 pkg.DoCommand(i.Value.EndPoint.Connect, RPC.CommandTargetType.DefaultType);
//             }
        }
        #endregion

        #endregion
    }
}
