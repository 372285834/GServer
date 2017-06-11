using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Wuxia;

namespace ServerCommon.Data.Player
{
    public class PlanesPlayerManager
    {
        CSCommon.Data.PlanesData mPlanesData = null;
        public CSCommon.Data.PlanesData PlanesData
        {
            get { return mPlanesData; }
            set { mPlanesData = value; }
        }
        Dictionary<ulong, PlayerDataEx> mGuidDict = new Dictionary<ulong, PlayerDataEx>();//根据角色Guid查找角色信息
        public Dictionary<ulong, PlayerDataEx> GuidDict
        {
            get { return mGuidDict; }
        }
        Dictionary<string, PlayerDataEx> mNameDict = new Dictionary<string, PlayerDataEx>();//根据角色名字查找角色信息
        public Dictionary<string, PlayerDataEx> NameDict
        {
            get { return mNameDict; }
        }

        public void RegPlayer(CSCommon.Data.PlayerDataEx pd)
        {
            GuidDict[pd.RoleDetail.RoleId] = pd;
            NameDict[pd.RoleDetail.RoleName] = pd;
            pd.CurPlanes = this;
        }

        public void UnRegPlayer(CSCommon.Data.PlayerDataEx pd)
        {
            GuidDict.Remove(pd.RoleDetail.RoleId);
            NameDict.Remove(pd.RoleDetail.RoleName);
            pd.CurPlanes = null;
        }
    }

//     public class MapContain
//     {
//         Dictionary<ulong, PlayerDataEx> mGuidDict = new Dictionary<ulong, PlayerDataEx>();
//         Dictionary<string, PlayerDataEx> mNameDict = new Dictionary<string, PlayerDataEx>();
// 
//         public void AddPlayer(PlayerDataEx pd)
//         {
//             mGuidDict[pd.RoleDetail.RoleId] = pd;
//             mNameDict[pd.RoleDetail.RoleName] = pd;
//         }
// 
//         public bool RemovePlayer(ulong roleId)
//         {
//             PlayerDataEx pd;
//             if (false == mGuidDict.TryGetValue(roleId, out pd))
//             {
//                 return false;
//             }
//             mGuidDict.Remove(roleId);
//             mNameDict.Remove(pd.RoleDetail.RoleName);
//             return true;
//         }
// 
//         public PlayerDataEx FindPlayerData(ulong roleId)
//         {
//             PlayerDataEx pd;
//             if (false == mGuidDict.TryGetValue(roleId, out pd))
//             {
//                 return null;
//             }
//             return pd;
//         }
// 
//         public PlayerDataEx FindPlayerData(string roleName)
//         {
//             PlayerDataEx pd;
//             if (false == mNameDict.TryGetValue(roleName, out pd))
//             {
//                 return null;
//             }
//             return pd;
//         }
//     }
// 
//     public class PlanesContain
//     {
//         Dictionary<int, MapContain> mMapDict = new Dictionary<int, MapContain>();
//         
//         public MapContain FindMapContain(int mapName)
//         {
//             MapContain map;
//             if (false == mMapDict.TryGetValue(mapName, out map))
//             {
//                 return null;
//             }
//             return map;
//         }
// 
//         public void AddPlayerToGlobalMap(int mapName, PlayerDataEx pd)
//         {
//             MapContain map;
//             if (false==mMapDict.TryGetValue(mapName, out map))
//             {
//                 map = new MapContain();
//                 mMapDict.Add(mapName,map);
//             }
//             map.AddPlayer(pd);
//         }
// 
//         public bool RemovePlayerFromGlobalMap(int mapName, ulong roleId)
//         {
//             MapContain map;
//             if (false == mMapDict.TryGetValue(mapName, out map))
//             {
//                 return false;
//             }
//             map.RemovePlayer(roleId);
//             return true;
//         }
//     }

    [RPC.RPCClassAttribute(typeof(PlayerManager))]
    public class PlayerManager : RPC.RPCObject
    {
        #region RPC必须的基础定义
        public static RPC.RPCClassInfo smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
        {
            return smRpcClassInfo;
        }
        #endregion

        static PlayerManager smInstance = new PlayerManager();
        public static PlayerManager Instance
        {
            get { return smInstance; }
        }

        public void Tick()
        {
            
        }

        #region 容器数据
        //账号信息
        Dictionary<ulong, AccountInfo> mAccountDict = new Dictionary<ulong, AccountInfo>();//根据账号Guid查找账号信息
        public Dictionary<ulong, AccountInfo> AccountDict
        {
            get { return mAccountDict; }
        }
        Dictionary<string, AccountInfo> mAccountNameDict = new Dictionary<string, AccountInfo>();//根据账号Name查找账号信息
        public Dictionary<string, AccountInfo> AccountNameDict
        {
            get { return mAccountNameDict; }
        }

        //角色信息
        Dictionary<ulong, CSCommon.Data.PlayerDataEx> mAllRoles = new Dictionary<ulong, CSCommon.Data.PlayerDataEx>();
        public Dictionary<ulong, CSCommon.Data.PlayerDataEx> AllRoles
        {
            get { return mAllRoles; }
        }

        //位面信息
        Dictionary<ushort, PlanesPlayerManager> mPlanesPlayerManager = new Dictionary<ushort, PlanesPlayerManager>();
        public Dictionary<ushort, PlanesPlayerManager> PlanesPlayerManager
        {
            get { return mPlanesPlayerManager; }
        }

        public int GetOnlyLoginAccountNumber()
        {
            int ret = 0;
            foreach (var i in mAccountDict)
            {
                if (i.Value.CurPlayer == null)
                    ret++;
            }
            return ret;
        }
        public int GetLoginAccountNumber()
        {
            return mAccountDict.Count;
        }
//         Dictionary<ulong, PlanesContain> mPlanesContains = new Dictionary<ulong, PlanesContain>();
// 
//         Dictionary<ulong, AccountInfo> mAccountDict = new Dictionary<ulong, AccountInfo>();//根据账号Guid查找账号信息
//         Dictionary<string, AccountInfo> mAccountNameDict = new Dictionary<string, AccountInfo>();//根据账号Name查找账号信息
// 
//         Dictionary<ulong, PlayerDataEx> mGuidDict = new Dictionary<ulong, PlayerDataEx>();//根据角色Guid查找角色信息
//         Dictionary<string, PlayerDataEx> mNameDict = new Dictionary<string, PlayerDataEx>();//根据角色名字查找角色信息
// 
//         public int GetOnlyLoginAccountNumber()
//         {
//             int ret = 0;
//             foreach (var i in mGuidDict)
//             {
//                 if (i.Value.mLandStep == PlayerLandStep.SelectRole)
//                     ret++;
//             }
//             return ret;
//         }
//         public int GetLoginAccountNumber()
//         {
//             return mGuidDict.Count;
//         }
        #endregion

        public AccountInfo FindAccountInfo(ulong accountId)
        {
            AccountInfo pd;
            if (mAccountDict.TryGetValue(accountId, out pd) == false)
                return null;
            return pd;
        }

        public AccountInfo FindAccountInfo(string accountName)
        {
            AccountInfo pd;
            if (mAccountNameDict.TryGetValue(accountName, out pd) == false)
                return null;
            return pd;
        }

        public PlayerDataEx FindPlayerData(ulong roleId)
        {
            PlayerDataEx pd;
            if (mAllRoles.TryGetValue(roleId, out pd) == false)
                return null;
            return pd;
        }
        public PlayerDataEx FindPlayerData(ushort planesId, string roleName)
        {
            PlanesPlayerManager ppm;
            if (mPlanesPlayerManager.TryGetValue(planesId, out ppm) == false)
                return null;
            PlayerDataEx pd;
            if (ppm.NameDict.TryGetValue(roleName, out pd) == false)
                return null;
            return pd;
        }

        public PlanesPlayerManager FindPlanesPlayerManager(ushort planesId)
        {
            PlanesPlayerManager planes;
            if (false == mPlanesPlayerManager.TryGetValue(planesId, out planes))
            {
                planes = new PlanesPlayerManager();
                CSCommon.Data.PlanesData planesData;
                if (IDataServer.Instance.PlanesMgr.Planes.TryGetValue(planesId, out planesData))
                {
                    planes.PlanesData = planesData;
                    mPlanesPlayerManager.Add(planesId, planes);
                    return planes;
                }
                else
                {
                    return null;
                }
            }
            return planes;
        }

        public void LoginCurPlayer(CSCommon.Data.AccountInfo account, MapConnectInfo map)
        {
            var pd = account.CurPlayer;
            mAllRoles[pd.RoleDetail.RoleId] = pd;

            if (map.PlanesPlayerManager != null)
            {
                pd.CurPlanes = map.PlanesPlayerManager;
                map.PlanesPlayerManager.RegPlayer(pd);
            }

            if (map != null)
                map.AddPlayer(pd);
        }

        public bool LogoutCurPlayer(CSCommon.Data.AccountInfo account, CSCommon.Data.PlayerData newPd)
        {
            lock (this)
            {
                if (account.CurPlayer == null)
                {
                    Log.Log.Server.Info("LogoutCurPlayer {0}: account.CurPlayer == null", account.Id);
                    return false;
                }
                var roleId = account.CurPlayer.RoleDetail.RoleId;
                if (newPd != null && roleId != newPd.RoleDetail.RoleId)
                {
                    Log.Log.Server.Info("LogoutCurPlayer {0}: account.CurPlayer.RoleDetail.RoleId != newPd.RoleDetail.RoleId", account.Id);
                    return false;
                }

                var pd = FindPlayerData(roleId);

                if (pd == null)
                {
                    Log.Log.Server.Info("LogoutCurPlayer {0}: mGuidDict.TryGetValue failed", roleId);
                    return false;
                }
                else
                {
                    mAllRoles.Remove(roleId);

                    //角色退出游戏状态，强制全部存盘
                    if (newPd != null)
                    {
                        SavePlayerData(newPd);
                    }
                    else
                    {
                        ForceSavePlayerData(pd);
                    }

                    var map = pd.CurMap as MapConnectInfo;
                    if (map != null)
                    {
                        map.RemovePlayer(pd.RoleDetail.RoleId);
                    }

                    var ppm = pd.CurPlanes as PlanesPlayerManager;
                    if (ppm != null)
                    {
                        ppm.UnRegPlayer(pd);
                    }

                    Thread.DBConnectManager.Instance.RemovePlayer(pd);
                    pd.mPlanesConnect = null;//位面连接断开
                    pd.BagItems.Clear();
                    pd.StoreItems.Clear();
                    account.CurPlayer = null;

                    return true;
                }
            }
        }
//         public PlayerDataEx FindPlayerData(string roleName)
//         {
//             PlayerDataEx pd;
//             if (mNameDict.TryGetValue(roleName, out pd) == false)
//                 return null;
//             return pd;
//         }

//         public PlanesContain FindPlanesContain(ulong planesId)
//         {
//             PlanesContain planes;
//             if (false == mPlanesContains.TryGetValue(planesId, out planes))
//             {
//                 return null;
//             }
//             return planes;
//         }
// 
//         public void AddPlayer2PlanesGlobalMap(ulong planesId, int mapName, PlayerDataEx pd)
//         {
//             lock (this)
//             {
//                 PlanesContain planes;
//                 if (false == mPlanesContains.TryGetValue(planesId, out planes))
//                 {
//                     planes = new PlanesContain();
//                     mPlanesContains.Add(planesId, planes);
//                 }
//                 planes.AddPlayerToGlobalMap(mapName, pd);
//             }
//         }
// 
//         public bool RemovePlayerFromPlanesGlobalMap(ulong planesId, int mapName, ulong roleId)
//         {
//             lock (this)
//             {
//                 PlanesContain planes;
//                 if (false == mPlanesContains.TryGetValue(planesId, out planes))
//                 {
//                     return false;
//                 }
//                 
//                 return planes.RemovePlayerFromGlobalMap(mapName, roleId);
//             }
//         }
// 
//         public bool RemovePlayer(ulong roleId, bool forceSave)
//         {
//             lock (this)
//             {
//                 PlayerDataEx pd;
//                 if (false == mGuidDict.TryGetValue(roleId, out pd))
//                 {
//                     //DB.DBConnectManager.Instance.RemovePlayer(roleId);//从数据库存储线程队列取掉该角色
//                     Log.Log.Common.Print(string.Format("RemovePlayer {0}: mGuidDict.TryGetValue failed",roleId));
//                     return false;
//                 }
//                 else
//                 {
//                     //角色退出游戏状态，强制全部存盘
//                     if (forceSave)
//                     {
//                         ForceSavePlayerData(pd);
//                     }
// 
//                     pd.mLandStep = PlayerLandStep.SelectRole;
//                     mAccountDict.Remove(pd.mAccountInfo.Id);//账号字典取掉该角色
//                     mAccountNameDict.Remove(pd.mAccountInfo.UserName);
//                     mGuidDict.Remove(roleId);//Guid字典取掉该角色
//                     mNameDict.Remove(pd.RoleDetail.RoleName);//Name字典取掉改角色
//                     bool ret = RemovePlayerFromPlanesGlobalMap(pd.RoleDetail.PlanesId, pd.RoleDetail.MapSourceId, roleId);//从位面地图取掉该角色
// 
//                     Thread.DBConnectManager.Instance.RemovePlayer(pd);
//                     pd.mPlanesConnect = null;//位面连接断开
//                     pd.BagItems.Clear();
//                     pd.StoreItems.Clear();
// 
//                     return ret;
//                 }
//             }
//         }


        #region DataSet操作

        DataSet mDataSet = null;
        public DataSet DataSet
        {
            get
            {
                if (mDataSet != null)
                {
                    return mDataSet;
                }
                return new DataSet();
            }
        }


        Dictionary<ulong, RoleDetail> mOffPlayerDict = new Dictionary<ulong, RoleDetail>();//根据角色id查找角色信息

        public void DownloadPlayerData(ServerFrame.DB.DBConnect dbConnect)
        {
            string strSQL = "SELECT * FROM roleinfo";

            MySqlCommand command = new MySqlCommand(strSQL, dbConnect.mConnection);
            MySqlDataAdapter adap = new MySqlDataAdapter(command);
            MySqlCommandBuilder msb = new MySqlCommandBuilder(adap);
            mDataSet = new DataSet();
            adap.Fill(mDataSet, "roleinfo");

            foreach (DataRow i in mDataSet.Tables["roleinfo"].Rows)
            {
                RoleDetail rd = new RoleDetail();
                ServerFrame.DB.DBConnect.FillObject(rd, i);
                mOffPlayerDict[rd.RoleId] = rd;
            }

        }

        public void UPdateOffPlayer(RoleDetail rd)
        {

            mOffPlayerDict[rd.RoleId] = rd;

            DataTable dt = DataSet.Tables["roleinfo"];
            string sql = string.Format("RoleId = \'{0}\'", rd.RoleId);
            DataRow[] rows = dt.Select(sql);
            int count = rows.Count();
            if (count == 0)
            {
                DataRow row = DataSet.Tables["roleinfo"].NewRow();
                ServerFrame.DB.DBConnect.FillDataRow(row, rd);
                DataSet.Tables["roleinfo"].Rows.Add(row);
            }
            else
            {
                DataRow row = DataSet.Tables["roleinfo"].NewRow();
                ServerFrame.DB.DBConnect.FillDataRow(row, rd);
                rows[0] = row;
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_GetOffPlayer(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            RoleDetail rd = new RoleDetail();
            if (mOffPlayerDict.ContainsKey(roleId))
            {
                rd = mOffPlayerDict[roleId];
                retPkg.Write((sbyte)1);
                retPkg.SetSinglePkg();
                retPkg.Write(rd);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
            }
            else
            {
                retPkg.Write((sbyte)-1);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
            }
        }

        //yzb
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_GetOffPlayerList(RPC.DataReader dr, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {

            List<CSCommon.Data.RoleDetail> mOffSocialInfoList = new List<CSCommon.Data.RoleDetail>();
            int count;
            dr.Read(out count);
            ulong roleId;
            for (int i = 0; i < count; i++)
            {
                dr.Read(out roleId);
                RoleDetail rd = new RoleDetail();
                if (mOffPlayerDict.ContainsKey(roleId))
                {
                    rd = mOffPlayerDict[roleId];
                    mOffSocialInfoList.Add(rd);
                }

            }
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            retPkg.SetSinglePkg();
            retPkg.Write(mOffSocialInfoList.Count);
            foreach (var i in mOffSocialInfoList)
            {
                retPkg.Write(i);
            }
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);

        }


        #endregion

        #region 数据库操作
        public bool DB_UpdataPlayerGuildData(ulong roleId, Guid GuildId, string GuildName, UInt32 GuildContribution)
        {
            //ServerFrame.DB.DBConnect dbConnect = Thread.AccountLoginThread.Instance.DBConnect;
            //CSCommon.Data.RoleInfo ri = new CSCommon.Data.RoleInfo();
            //string sqlCode = "RoleId = \'" + roleId + "\'";
            //ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(sqlCode, ri, null);
            //var tab = dbConnect._ExecuteSelect(dbOp, "RoleInfo");
            //if (tab == null || tab.Rows.Count != 1)
            //    return false;

            //if (false == ServerFrame.DB.DBConnect.FillObject(ri, tab.Rows[0]))
            //    return false;

            //ri.RoleGuild = GuildId;
            //ri.GuildName = GuildName;
            //ri.RoleGuildContribution = GuildContribution;

            //dbOp = ServerFrame.DB.DBConnect.UpdateData(sqlCode, ri, null);
            //dbConnect._ExecuteUpdate(dbOp);
            return true;
        }

        public static AccountInfo DB_LoadAccountInfo(string name)
        {
            name = ServerFrame.DB.DBConnect.SqlSafeString(name);
            ServerFrame.DB.DBConnect dbConnect = Thread.AccountLoginThread.Instance.DBConnect;
            //这里应该有一个根据name从数据库读取的动作
            CSCommon.Data.AccountInfo cu;
            string condition;
            System.Data.DataTable tab = null;

            cu = new CSCommon.Data.AccountInfo();
            condition = "UserName=\'" + name + "\'";
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, cu, "");
            tab = null;
            tab = dbConnect._ExecuteSelect(dbOp, "account");
            if (tab == null || tab.Rows.Count != 1)
            {
                //return null;
                //注册一个
                cu.Id = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Account);
                cu.UserName = name;
                cu.Password = "";
                condition = "Id=" + cu.Id;
                dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, cu, true);
                dbConnect._ExecuteInsert(dbOp);
            }
            else
            {
                if (false == ServerFrame.DB.DBConnect.FillObject(cu, tab.Rows[0]))
                    return null;
            }           
            condition = "AccountId=" + cu.Id;
            dbOp = ServerFrame.DB.DBConnect.SelectData(condition, new RoleInfo(), "");
            tab = null;
            tab = dbConnect._ExecuteSelect(dbOp, "RoleInfo");
            if (tab == null)
                return null;

            cu.Roles.Clear();
            foreach (System.Data.DataRow r in tab.Rows)
            {
                RoleInfo ri = new RoleInfo();
                if (false == ServerFrame.DB.DBConnect.FillObject(ri, r))
                    continue;

                cu.Roles.Add(ri);
            }

            return cu;
        }
        public static PlayerDataEx DB_LoadPlayerData(ulong roleId,Iocp.NetConnection gateConnect)
        {
            PlayerDataEx pd = new PlayerDataEx();
            pd.mGateConnect = gateConnect;

            var dbConnect = IDataServer.Instance.DBLoaderConnect;
            #region 初始化RoleDetail
            string condition = "RoleId=" + roleId;
            RoleDetail rd = new RoleDetail();
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, rd, "");
            System.Data.DataTable tab = dbConnect._ExecuteSelect(dbOp, "RoleInfo");
            if (tab == null || tab.Rows.Count != 1)
            {
                System.Diagnostics.Debug.WriteLine("角色进入游戏失败，因为数据库执行失败:" + dbOp.SqlCode);
                return null;
            }

            if (false == ServerFrame.DB.DBConnect.FillObject(rd, tab.Rows[0]))
                return null;

            pd.RoleDetail = rd;
            #endregion

            //这里还要初始化玩家的物品，弟子等数据
            _InitMartial(roleId, dbConnect, pd);
            _InitItem(roleId, dbConnect, pd);
            _InitTask(roleId, dbConnect, pd);
            _InitSkill(roleId, dbConnect, pd);
            _InitAchieve(roleId, dbConnect, pd);
            if (pd == null || pd.RoleDetail == null)
            {
                Log.Log.Server.Info("pd error");
            }
            Thread.DBConnectManager.Instance.AddPlayer(pd);
            return pd;
        }

        #region 各种辅助DB函数
        private static void _InitMartial(ulong roleId, ServerFrame.DB.DBConnect dbConnect, PlayerDataEx pd)
        {
            CSCommon.Data.MartialData md = new CSCommon.Data.MartialData();
            string condition = "RoleId=" + roleId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, md, "");
            System.Data.DataTable tab = dbConnect._ExecuteSelect(dbOp, "MartialData");
            if (tab != null)
            {
                if (tab.Rows.Count == 1)
                {
                    if (ServerFrame.DB.DBConnect.FillObject(md, tab.Rows[0]))
                    {
                        pd.MartialData = md;
                        return;
                    }

                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("角色获取武馆数据库执行失败:" + dbOp.SqlCode);
            }
        }

        private static void _InitTask(ulong roleId, ServerFrame.DB.DBConnect dbConnect, PlayerDataEx pd)
        {
            CSCommon.Data.TaskData td = new CSCommon.Data.TaskData();
            string condition = "OwnerId=" + roleId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, td, "");
            System.Data.DataTable tab = dbConnect._ExecuteSelect(dbOp, "TaskData");
            if (tab != null)
            {
                if (tab.Rows.Count == 1)
                {
                    if (ServerFrame.DB.DBConnect.FillObject(td, tab.Rows[0]))
                    {
                        pd.TaskData = td;
                        return;
                    }

                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("角色获取任务数据库执行失败:" + dbOp.SqlCode);
            }
        }

        private static void _InitAchieve(ulong roleId, ServerFrame.DB.DBConnect dbConnect, PlayerDataEx pd)
        {
            CSCommon.Data.AchieveData ad = new CSCommon.Data.AchieveData();
            string condition = "RoleId=" + roleId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, ad, "");
            System.Data.DataTable tab = dbConnect._ExecuteSelect(dbOp, "AchieveData");
            if (tab != null)
            {
                if (tab.Rows.Count == 1)
                {
                    if (ServerFrame.DB.DBConnect.FillObject(ad, tab.Rows[0]))
                    {
                        pd.AchieveData = ad;
                        return;
                    }

                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("角色获取任务数据库执行失败:" + dbOp.SqlCode);
            }
        }
        private static void _InitItem(ulong roleId, ServerFrame.DB.DBConnect dbConnect, PlayerDataEx pd)
        {
            string condition = "OwnerId=" + roleId + " and (Inventory=\'" + (int)(CSCommon.eItemInventory.ItemBag) + "\' or Inventory=\'" +
                                                                            (int)(CSCommon.eItemInventory.EquipBag) + "\' or Inventory=\'" +
                                                                            (int)(CSCommon.eItemInventory.EquipGemBag) + "\' or Inventory=\'" +
                                                                            (int)(CSCommon.eItemInventory.GemBag) + "\' or Inventory=\'" +
                                                                            (int)(CSCommon.eItemInventory.FashionBag) + "\')";
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, new CSCommon.Data.ItemData(), "");
            System.Data.DataTable tab = dbConnect._ExecuteSelect(dbOp, "ItemData");
            if (tab != null)
            {
                List<CSCommon.Data.ItemData> items = new List<CSCommon.Data.ItemData>();

                foreach (System.Data.DataRow r in tab.Rows)
                {
                    CSCommon.Data.ItemData itemData = new CSCommon.Data.ItemData();
                    if (false == ServerFrame.DB.DBConnect.FillObject(itemData, r))
                        continue;
                    items.Add(itemData);
                }

                pd.BagItems.Clear();
                pd.EquipedItems.Clear();
                pd.FashionItems.Clear();
                pd.EquipGemItems.Clear();
                pd.GemItems.Clear();
                for (int i = 0; i < items.Count; i++)
                {
                    switch ((CSCommon.eItemInventory)items[i].Inventory)
                    {
                        case CSCommon.eItemInventory.ItemBag:
                            {
                                pd.BagItems.Add(items[i]);
                                items.RemoveAt(i);
                                i--;
                            }
                            break;
                        case CSCommon.eItemInventory.EquipBag:
                            {
                                pd.EquipedItems.Add(items[i]);
                                items.RemoveAt(i);
                                i--;
                            }
                            break;
                        case CSCommon.eItemInventory.FashionBag:
                            {
                                pd.FashionItems.Add(items[i]);
                                items.RemoveAt(i);
                                i--;
                            }
                            break;
                        case CSCommon.eItemInventory.EquipGemBag:
                            {
                                pd.EquipGemItems.Add(items[i]);
                                items.RemoveAt(i);
                                i--;
                            }
                            break;
                        case CSCommon.eItemInventory.GemBag:
                            {
                                pd.GemItems.Add(items[i]);
                                items.RemoveAt(i);
                                i--;
                            }
                            break;
                    }

                }

                if (items.Count != 0)
                {
                    System.Diagnostics.Debug.WriteLine("还有物品在数据库读取出来后没有分类进入列表");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("角色获取物品数据库执行失败:" + dbOp.SqlCode);
            }
        }

        private static void _InitSkill(ulong roleId, ServerFrame.DB.DBConnect dbConnect, PlayerDataEx pd)
        {
            string condition = "OwnerId=" + roleId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, new CSCommon.Data.SkillData(), "");
            System.Data.DataTable tab = dbConnect._ExecuteSelect(dbOp, "SkillData");
            if (tab != null)
            {
                List<CSCommon.Data.SkillData> items = new List<CSCommon.Data.SkillData>();

                foreach (System.Data.DataRow r in tab.Rows)
                {
                    CSCommon.Data.SkillData itemData = new CSCommon.Data.SkillData();
                    if (false == ServerFrame.DB.DBConnect.FillObject(itemData, r))
                        continue;
                    items.Add(itemData);
                }

                pd.SkillDatas.Clear();
                pd.SkillDatas.AddRange(items);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("角色获取技能数据库执行失败:" + dbOp.SqlCode);
            }
        }
        #endregion

        #endregion        

        #region 数据库分项存储
        public bool SaveRoleDetail(bool bSaveAll, ServerFrame.DB.DBConnect dbConnect, RoleDetail rd, PlayerDataEx role)
        {
            if (role == null)
                return false;

            string condition = "RoleId=" + rd.RoleId;
            ServerFrame.DB.DBOperator dbOp = null;
            if (bSaveAll)
                dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, rd, null);
            else
                dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, rd, role.RoleDetail);
            //System.Diagnostics.Debug.WriteLine(code);
            Thread.DBConnectManager.Instance.PushSave(role, dbOp);
            role.RoleDetail = rd;
            return true;
        }

        public bool SaveMartial(bool bSaveAll, ServerFrame.DB.DBConnect dbConnect, MartialData md, PlayerDataEx role)
        {
            if (role == null)
                return false;

            string condition = "RoleId=" + md.RoleId;
            ServerFrame.DB.DBOperator dbOp = null;
            if (bSaveAll)
                dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, md, null);
            else
                dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, md, true);

            Thread.DBConnectManager.Instance.PushSave(role, dbOp);
            role.MartialData = md;
            return true;
        }

        public bool SaveItems(bool bSaveAll, ServerFrame.DB.DBConnect dbConnect, CSCommon.eItemInventory type, List<CSCommon.Data.ItemData> items, PlayerDataEx role)
        {
            if (bSaveAll)
            {
                foreach (var i in items)
                {
                    // 产生insert语句
                    string condition = "ItemId = " + i.ItemId;
                    ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, i, true);
                    Thread.DBConnectManager.Instance.PushSave(role, dbOp);
                }
            }
            else
            {
                List<CSCommon.Data.ItemData> bag = null;
                switch (type)
                {
                    case CSCommon.eItemInventory.ItemBag:
                        bag = role.BagItems;
                        break;
                    case CSCommon.eItemInventory.EquipBag:
                        bag = role.EquipedItems;
                        break;
                    case CSCommon.eItemInventory.FashionBag:
                        bag = role.FashionItems;
                        break;
                    case CSCommon.eItemInventory.EquipGemBag:
                        bag = role.EquipGemItems;
                        break;
                    case CSCommon.eItemInventory.GemBag:
                        bag = role.GemItems;
                        break;
                }

                foreach (var i in items)
                {
                    CSCommon.Data.ItemData outItem = null;
                    foreach (var j in bag)
                    {
                        if (j.ItemId == i.ItemId)
                        {
                            outItem = j;
                            break;
                        }
                    }
                    if (outItem != null)
                    {
                        // 产生update语句
                        string condition = "ItemId = " + i.ItemId;
                        ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, i, outItem);
                        Thread.DBConnectManager.Instance.PushSave(role, dbOp);
                    }
                    else
                    {
                        // 产生insert语句
                        string condition = "ItemId = " + i.ItemId;
                        ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, i, true);
                        Thread.DBConnectManager.Instance.PushSave(role, dbOp);
                    }
                }
            }

            switch (type)
            {
                case CSCommon.eItemInventory.ItemBag:
                    role.BagItems = items;
                    break;
                case CSCommon.eItemInventory.EquipBag:
                    role.EquipedItems = items;
                    break;
                case CSCommon.eItemInventory.FashionBag:
                    role.FashionItems = items;
                    break;
                case CSCommon.eItemInventory.EquipGemBag:
                    role.EquipGemItems = items;
                    break;
                case CSCommon.eItemInventory.GemBag:
                    role.GemItems = items;
                    break;
            }
            return true;
        }

        public bool SaveTask(bool bSaveAll, ServerFrame.DB.DBConnect dbConnect, TaskData td, PlayerDataEx role)
        {
            if (role == null)
                return false;

            string condition = "OwnerId=" + td.OwnerId;
            ServerFrame.DB.DBOperator dbOp = null;
            if (bSaveAll)
                dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, td, null);
            else
            {
                if (td.OwnerId == role.RoleDetail.RoleId)
                {
                    dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, td, role.TaskData);
                }
                else
                {
                    td.OwnerId = role.RoleDetail.RoleId;
                    dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, td, true);
                }

            }

            Thread.DBConnectManager.Instance.PushSave(role, dbOp);
            role.TaskData = td;
            return true;
        }

        public bool SaveAchieve(bool bSaveAll, ServerFrame.DB.DBConnect dbConnect, AchieveData ad, PlayerDataEx role)
        {
            if (role == null)
                return false;

            string condition = "RoleId=" + ad.RoleId;
            ServerFrame.DB.DBOperator dbOp = null;
            if (bSaveAll)
                dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, ad, null);
            else
            {
                if (role.AchieveData.RoleId == role.RoleDetail.RoleId)//是否存在
                {
                    dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, ad, role.AchieveData);
                }
                else
                {
                    dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, ad, true);
                }

            }

            Thread.DBConnectManager.Instance.PushSave(role, dbOp);
            role.AchieveData = ad;
            return true;
        }

        public bool SaveSkills(bool bSaveAll, ServerFrame.DB.DBConnect dbConnect, List<CSCommon.Data.SkillData> items, PlayerDataEx role)
        {
            if (bSaveAll)
            {
                foreach (var i in items)
                {
                    // 产生insert语句
                    string condition = "OwnerID = " + i.OwnerId + " and " + "TemplateID = " + i.TemplateId;
                    ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, i, true);
                    Thread.DBConnectManager.Instance.PushSave(role, dbOp);
                }
            }
            else
            {
                foreach (var i in items)
                {
                    CSCommon.Data.SkillData outItem = null;
                    foreach (var j in role.SkillDatas)
                    {
                        if (j.TemplateId == i.TemplateId)
                        {
                            outItem = j;
                            break;
                        }
                    }
                    if (outItem != null)
                    {
                        // 产生update语句
                        string condition = "OwnerID = " + i.OwnerId + " and " + "TemplateID = " + i.TemplateId;
                        ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, i, outItem);
                        Thread.DBConnectManager.Instance.PushSave(role, dbOp);
                    }
                    else
                    {
                        // 产生insert语句
                        string condition = "OwnerID = " + i.OwnerId + " and " + "TemplateID = " + i.TemplateId;
                        ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, i, true);
                        Thread.DBConnectManager.Instance.PushSave(role, dbOp);
                    }
                }
            }
            role.SkillDatas = items;
            return true;
        }
        #endregion


        #region 给多线程调用的登陆函数
        public void Do_AccountLogin(Thread.AccountLoginHolder atom)
        {
            #region 账号密码校验
            var ainfo = FindAccountInfo(atom.Name);
            if (ainfo == null)
            {//需要从数据库读取
                //TODO：可以考虑数据缓存，不要频繁查询数据库
                ainfo = PlayerManager.DB_LoadAccountInfo(atom.Name);
                if (ainfo == null)
                {
                    RPC.PackageWriter pkg0 = new RPC.PackageWriter();
                    Log.Log.Server.Info("{0}账号不存在", atom.Name);
                    pkg0.Write((sbyte)CSCommon.eRet_LoginAccount.NoAccount);//账号不存在
                    pkg0.DoReturnCommand2(atom.Connect, atom.ReturnSerialId);
                    return;
                }
                else if (ainfo.Password != atom.Password)
                {//密码错误
                    RPC.PackageWriter pkg0 = new RPC.PackageWriter();
                    pkg0.Write((sbyte)CSCommon.eRet_LoginAccount.PasswordError);//密码错误
                    pkg0.DoReturnCommand2(atom.Connect, atom.ReturnSerialId);
                    return;
                }

                //账号登陆进来了，把账号信息数据准备好
                mAccountNameDict[atom.Name] = ainfo;
                mAccountDict[ainfo.Id] = ainfo;
            }
            else
            {
                if (ainfo.Password != atom.Password)
                {//密码错误
                    RPC.PackageWriter pkg0 = new RPC.PackageWriter();
                    pkg0.Write((sbyte)CSCommon.eRet_LoginAccount.PasswordError);//密码错误
                    pkg0.Write(ainfo.Id);
                    pkg0.DoReturnCommand2(atom.Connect, atom.ReturnSerialId);
                    return;
                }
                else
                {
                    RPC.PackageWriter pkg = new RPC.PackageWriter();
                    Log.Log.Server.Info("{0}顶人下来，这次登陆你也进不来，再尝试吧", atom.Name);
                    pkg.Write((sbyte)CSCommon.eRet_LoginAccount.AccountHasLogin);//顶人下来，这次登陆你也进不来，再尝试吧
                    pkg.Write(ainfo.Id);
                    pkg.DoReturnCommand2(atom.Connect, atom.ReturnSerialId);

                    if(ainfo.Data2GateConnect != atom.Connect)  //不是同一个gate接入
                    {
                        pkg.Reset();
                        H_RPCRoot.smInstance.HGet_GateServer(pkg).DisconnectPlayer(pkg, ainfo.Id, (sbyte)eServerType.Data);
                        pkg.DoCommand(ainfo.Data2GateConnect, RPC.CommandTargetType.DefaultType);
                    }
                    return;
                }
            }
            
            #endregion
            _ReturnLoginOK(ainfo, atom);
        }

        //返回登陆成功信息
        private void _ReturnLoginOK(AccountInfo ainfo, Thread.AccountLoginHolder atom)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            pkg.Write((sbyte)CSCommon.eRet_LoginAccount.Succeed);
            pkg.Write(ainfo);

            var roles = ainfo.Roles.FindAll(role => role.PlanesId == atom.PlanesId);

            Byte count = (Byte)roles.Count;
            pkg.Write(count);
            foreach (CSCommon.Data.RoleInfo ri in roles)
            {
                pkg.Write(ri);
            }

            pkg.DoReturnCommand2(atom.Connect, atom.ReturnSerialId);
            return;
        }
        public void Do_RoleLogin(Thread.RoleEnterHolder atom)
        {
            ulong linkSerialId = atom.linkSerialId;
            UInt16 cltIndex = atom.cltIndex;
            ulong roleId = atom.roleId;
            ulong accountId = atom.accountId;
            Iocp.NetConnection connect = atom.connect;

            #region 从数据库或者内存加载角色信息
            var account = this.FindAccountInfo(accountId);
            if (account == null)
            {//没有账号登陆，就有角色要进来
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)-1);
                retPkg.DoReturnCommand2(connect, atom.returnSerialId);
                return;
            }

            var pd = this.FindPlayerData(roleId);
            if (pd == null)
            {
                var saver = Thread.DBConnectManager.Instance.FindPlayerSaverById(roleId);//存盘还没有结束
                if (saver != null)
                {
                    //告诉Gate，该角色还在存盘，暂时不允许登陆
                    Log.Log.Server.Info("该角色还在存盘");
                    RPC.PackageWriter retPkg = new RPC.PackageWriter();
                    retPkg.Write((sbyte)-2);
                    retPkg.DoReturnCommand2(connect, atom.returnSerialId);

                    //先把角色从存盘队列清除
                    Thread.DBConnectManager.Instance.RemovePlayer(saver.mPlayerDataEx);
                    return;
                }
                pd = PlayerManager.DB_LoadPlayerData(roleId,atom.connect);                
                pd.mLinkSerialId = linkSerialId;
            }
            else
            {
                if (pd.mLinkSerialId != linkSerialId)
                {
                    RPC.PackageWriter retPkg = new RPC.PackageWriter();
                    retPkg.Write((sbyte)-3);
                    retPkg.DoReturnCommand2(connect, atom.returnSerialId);
                    return;
                }
            }            
            #endregion

            if (account.CurPlayer != null)
            {
                Log.Log.Server.Info("当前账号有角色已经登录游戏了，怎么还有人要进来!");
            }
            account.CurPlayer = pd;

            ulong mapInstanceId = 0;
            MapConnectInfo mapInfo;
            if (AllMapManager.Instance.InstanceMaps.TryGetValue(pd.RoleDetail.DungeonID, out mapInfo))
            {//上次进的副本还没有关闭，还存在
                var mapdata = Planes.MapInstanceManager.GetMapInitBySourceId(mapInfo.MapSourceId);
                
                if (mapdata != null && mapdata.MapData.mapType > (int)CSCommon.eMapType.InstanceStart)
                {
                    //副本
                }
                else
                {
                    //世界地图
                    pd.RoleDetail.DungeonID = 0;
                    mapInfo = AllMapManager.Instance.GetMapConnectInfo(pd, pd.RoleDetail.PlanesId, pd.RoleDetail.MapSourceId, mapInstanceId);
                }
                
            }
            else
            {//上次没进过副本,或者副本已经关闭
                pd.RoleDetail.DungeonID = 0;
                if (AllMapManager.IsInstanceMap(pd.RoleDetail.MapSourceId))
                {
                    mapInstanceId = AllMapManager.Instance.GetInstanceMapId(pd, pd.RoleDetail.MapSourceId);
                }
                mapInfo = AllMapManager.Instance.GetMapConnectInfo(pd, pd.RoleDetail.PlanesId, pd.RoleDetail.MapSourceId, mapInstanceId);
            }

            this.LoginCurPlayer(account, mapInfo);

            ServerFrame.NetEndPoint nep = mapInfo.mConnect;
            if (nep == null)
            {//找不到地图所在位面服务器
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)-3);
                retPkg.DoReturnCommand2(connect, atom.returnSerialId);
                return;
            }
            #region 正确绑定角色的各种连接信息和状态
            pd.mLinkSerialId = linkSerialId;
            pd.mGateConnectIndex = cltIndex;
            pd.mAccountInfo = account;
            pd.mGateConnect = connect;
            pd.mPlanesConnect = nep.Connect as Iocp.TcpConnect;
            #endregion

            #region 玩家数据获取正确，返回信息
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.Write((sbyte)1);
                retPkg.Write(nep.Id);
                retPkg.Write(pd);
                if (mapInfo.PlanesPlayerManager != null)
                {
                    retPkg.Write(mapInfo.PlanesPlayerManager.PlanesData);
                }
                else
                {
                    retPkg.Write(new CSCommon.Data.PlanesData());
                }
                retPkg.Write(mapInstanceId);
                retPkg.DoReturnCommand2(connect, atom.returnSerialId);
            }
            #endregion
        }
        #endregion

        #region 注册账号，创建角色
        public SByte RegAccount(ServerFrame.DB.DBConnect dbConnect, string usr, string psw, string mobileNum)
        {
            usr = ServerFrame.DB.DBConnect.SqlSafeString(usr);
            psw = ServerFrame.DB.DBConnect.SqlSafeString(psw);
            CSCommon.Data.AccountInfo ai = new CSCommon.Data.AccountInfo();
            ai.Id = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Account);
            ai.UserName = usr;
            ai.Password = psw;
            string condition = "UserName=\'" + ai.UserName + "\'";
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, ai, false);
            if (false == dbConnect._ExecuteInsert(dbOp))
            {
                return -1;
            }

//             CSCommon.Data.AccountBinder ab = new CSCommon.Data.AccountBinder();
//             ab.AccountId = ai.Id;
//             ab.MobileNumber = mobileNum;
//             condition = "AccountId=\'" + ab.AccountId + "\'";
//             dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, ab, false);
//             if (false == dbConnect._ExecuteInsert(dbOp))
//             {
//                 return -2;
//             }

            return 1;
        }

        UInt64 playerNum = 0;
        public RPC.DataWriter RegGuest(ServerFrame.DB.DBConnect dbConnect)
        {
            RPC.DataWriter dw = new RPC.DataWriter();
            SByte retValue = 1;

            if (playerNum == 0)
            {
                playerNum = dbConnect.GetDataCount("Account");
            }

            CSCommon.Data.AccountInfo ai = new CSCommon.Data.AccountInfo();
            ai.Id = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Account);
            ai.UserName = "Guest" + playerNum++;
            ai.Password = "";

            string condition = "UserName=\'" + ai.UserName + "\'";
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, ai, false);
            if (false == dbConnect._ExecuteInsert(dbOp))
                retValue = -1;
            else
            {
//                 CSCommon.Data.AccountBinder ab = new CSCommon.Data.AccountBinder();
//                 ab.AccountId = ai.Id;
//                 condition = "AccountId=\'" + ab.AccountId + "\'";
//                 dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, ab, false);
//                 if (false == dbConnect._ExecuteInsert(dbOp))
//                     retValue = -2;
            }

            dw.Write(retValue);
            dw.Write(ai.UserName);

            return dw;
        }

        public PlayerData TryCreatePlayer(ServerFrame.DB.DBConnect dbConnect, PlanesMgr planesMgr,
            ulong accountId, ulong roleId, string planesName, string plyName, Byte pro, Byte sex)
        {
            planesName = ServerFrame.DB.DBConnect.SqlSafeString(planesName);
            plyName = ServerFrame.DB.DBConnect.SqlSafeString(plyName);
            var account = this.FindAccountInfo(accountId);
            if (account == null)
            {
                return null;
            }
            CSCommon.Data.PlanesData planesData = planesMgr.FindPlanesByName(planesName);
            if (planesData == null)
                planesData = planesMgr.RandomPlanes();
            PlayerData result = new PlayerData();
            RoleDetail rd = new RoleDetail();
            rd.CreateTime = System.DateTime.Now;//.ToBinary();
            rd.LastLoginDate = System.DateTime.Now;
            rd.AccountId = accountId;
            rd.RoleId = roleId;
            rd.BagSize = CSCommon.BagCommon.Instance.BagStartSize;
            rd.LimitLevel = account.LimitLevel;
            rd.PlanesId = planesData.PlanesId;
            rd.PlanesName = planesData.PlanesName;
            rd.RoleName = plyName;
            rd.Profession = pro;
            rd.RoleLevel = 1;
            rd.Sex = sex;
            rd.MapSourceId = GameSet.Instance.DefaultMapId; //(short)ServerFrame.Support.IFileConfig.Instance.DefaultMapId;
            //rd.MapName = "test";//ServerCommon.Planes.PlanesManager.Instance.DefaultMapName;//"浮世德";
            //rd.MapType = (byte)CSCommon.Data.MapData.enMapType.DongHaiYuCun;
            rd.LocationX = GameSet.Instance.BornPoint.X;// 31;//MapConfig.Instance.GetDefaultMapData().mBornPoint.PositionX; //360.6068F;
            rd.LocationY = GameSet.Instance.BornPoint.Y; //4;// MapConfig.Instance.GetDefaultMapData().mBornPoint.PositionY; //219.3933F;
            rd.LocationZ = GameSet.Instance.BornPoint.Z; //-7;
            //rd.RolePower = 10;// (byte)CSCommon.Data.Fight.FightCommonData.Instance.GetUpgradeDataWithPlayerLevel(1).PowerLimit;

            string condition = "RoleId=" + roleId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, rd, false);
            if (false == dbConnect._ExecuteInsert(dbOp))
            {
                Log.Log.Login.Print("create name is exist");
                return null;
            }
            MartialData md = new MartialData();
            md.RoleId = roleId;
            dbOp = ServerFrame.DB.DBConnect.InsertData(condition, md, false);
            if (false == dbConnect._ExecuteInsert(dbOp))
            {
                Log.Log.Login.Print("create MartialData is fail");
                return null;
            }
            //try
            //{
            //    RoleCom roleComData = new RoleCom();
            //    roleComData.RoleId = rd.RoleId;
            //    roleComData.Name = rd.RoleName;
            //    roleComData.PlanesId = rd.PlanesId;
            //    roleComData.TemplateId = rd.TemplateId;
            //    roleComData.Profession = rd.Profession;            
            //    roleComData.Level = rd.RoleLevel;
            //    roleComData.MapName = rd.MapName;

            //    ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, roleComData, false);
            //    if (false == dbConnect._ExecuteInsert(dbOp))
            //        return null;
            //}
            //catch (System.Exception ex)
            //{
            //    Log.Log.Common.Print(ex.ToString());
            //    return null;//数据库插入失败，检查重名什么的
            //}

            result.RoleDetail = rd;
            result.TaskData = CreateTask(dbConnect, rd.RoleId);
            InitPlayerData(dbConnect, rd.RoleId, rd.Sex, rd.Profession, result.EquipedItems, result.SkillDatas, result.FashionItems);
            RoleInfo ri = new RoleInfo();
            ri.AccountId = result.RoleDetail.AccountId;
            ri.LimitLevel = result.RoleDetail.LimitLevel;
            ri.MapName = result.RoleDetail.MapName;
            ri.PlanesId = result.RoleDetail.PlanesId;
            ri.PlanesName = result.RoleDetail.PlanesName;
            ri.RoleId = result.RoleDetail.RoleId;
            ri.RoleName = result.RoleDetail.RoleName;
            account.Roles.Add(ri);
            return result;
        }

        public TaskData CreateTask(ServerFrame.DB.DBConnect dbConnect, ulong ownerId)
        {
            //任务初始化
            TaskData data = new TaskData();
            data.OwnerId = ownerId;
            data.TemplateId = 100001;
            data.State = (byte)CSCommon.eTaskState.Finished;
            data.Process = 0;
            string condition = "OwnerId=" + data.OwnerId + " and " + "TemplateId=" + data.TemplateId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, data, false);
            if (false == dbConnect._ExecuteInsert(dbOp))
            {
                Log.Log.Login.Print("CreateTask failed!");
                return null;
            }
            return data;
        }

        public void InitPlayerData(ServerFrame.DB.DBConnect dbConnect, ulong ownerId, byte sex, byte pro, List<ItemData> items, List<SkillData> skills, List<ItemData> fashions)
        {
            int roletempid = CSCommon.CommonUtil.GetTemplateIDBySexAndPro(sex, pro);
            var temp = CSTable.StaticDataManager.PlayerTpl[roletempid];
            if (temp != null)
            {
                //装备初始化
                var item = CreateItem(dbConnect, ownerId, (UInt16)CSCommon.eEquipType.Weapon, temp.weapon, CSCommon.eItemInventory.EquipBag);           //武器
                if (item != null)
                    items.Add(item);
                item = CreateItem(dbConnect, ownerId, (UInt16)CSCommon.eEquipType.Necklace, temp.necklace, CSCommon.eItemInventory.EquipBag);       //项链
                if (item != null)
                    items.Add(item);
                item = CreateItem(dbConnect, ownerId, (UInt16)CSCommon.eEquipType.Ring1, temp.ring1, CSCommon.eItemInventory.EquipBag);             //戒指1
                if (item != null)
                    items.Add(item);
                item = CreateItem(dbConnect, ownerId, (UInt16)CSCommon.eEquipType.Ring2, temp.ring2, CSCommon.eItemInventory.EquipBag);             //戒指2
                if (item != null)
                    items.Add(item);
                item = CreateItem(dbConnect, ownerId, (UInt16)CSCommon.eEquipType.JadePendant, temp.jadependant, CSCommon.eItemInventory.EquipBag); //玉佩
                if (item != null)
                    items.Add(item);
                item = CreateItem(dbConnect, ownerId, (UInt16)CSCommon.eEquipType.Head, temp.head, CSCommon.eItemInventory.EquipBag);              //头盔
                if (item != null)
                    items.Add(item);
                item = CreateItem(dbConnect, ownerId, (UInt16)CSCommon.eEquipType.Chest, temp.chest, CSCommon.eItemInventory.EquipBag);             //衣服
                if (item != null)
                    items.Add(item);
                item = CreateItem(dbConnect, ownerId, (UInt16)CSCommon.eEquipType.Belt, temp.belt, CSCommon.eItemInventory.EquipBag);               //腰带
                if (item != null)
                    items.Add(item);
                item = CreateItem(dbConnect, ownerId, (UInt16)CSCommon.eEquipType.Cuff, temp.cuff, CSCommon.eItemInventory.EquipBag);               //护手
                if (item != null)
                    items.Add(item);
                item = CreateItem(dbConnect, ownerId, (UInt16)CSCommon.eEquipType.Shoes, temp.shoes, CSCommon.eItemInventory.EquipBag);             //鞋子
                if (item != null)
                    items.Add(item);

                //时装
                item = CreateItem(dbConnect, ownerId, 0, temp.chestfashion, CSCommon.eItemInventory.FashionBag);  //衣服时装
                if (item != null)
                    fashions.Add(item);

                //技能初始化
                var skill = CreateSkill(dbConnect, ownerId, temp.skill1);
                if (skill != null)
                    skills.Add(skill);

                skill = CreateSkill(dbConnect, ownerId, temp.skill2);
                if (skill != null)
                    skills.Add(skill);

                skill = CreateSkill(dbConnect, ownerId, temp.skill3);
                if (skill != null)
                    skills.Add(skill);

                skill = CreateSkill(dbConnect, ownerId, temp.skill4);
                if (skill != null)
                    skills.Add(skill);

                skill = CreateSkill(dbConnect, ownerId, temp.skill5);
                if (skill != null)
                    skills.Add(skill);
            }
        }

        public SkillData CreateSkill(ServerFrame.DB.DBConnect dbConnect, ulong ownerId, int templateId)
        {
            if (templateId == 0)
            {
                Log.Log.Login.Print("CreateSkill templateId == 0");
                return null;
            }
            SkillData data = new SkillData();
            data.OwnerId = ownerId;
            data.TemplateId = templateId;
            data.Level = 1;
            string condition = "OwnerId=" + data.OwnerId + " and " + "TemplateId=" + data.TemplateId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, data, false);
            if (false == dbConnect._ExecuteInsert(dbOp))
            {
                Log.Log.Login.Print("CreateSkill failed");
                return null;
            }
            return data;
        }

        public ItemData CreateItem(ServerFrame.DB.DBConnect dbConnect, ulong ownerId, ushort pos, int templateId, CSCommon.eItemInventory bag)
        {
            if (templateId == 0)
            {
                Log.Log.Login.Print("CreateItem templateId == 0");
                return null;
            }
            ItemData data = new ItemData();
            data.ItemTemlateId = templateId;
            data.OwnerId = ownerId;
            data.ItemId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Item);
            data.CreateTime = System.DateTime.Now;
            data.Inventory = (byte)bag;
            data.Position = pos;
            data.WearState = (byte)CSCommon.eBoolState.True;
            string condition = "ItemId=" + data.ItemId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, data, false);
            if (false == dbConnect._ExecuteInsert(dbOp))
            {
                Log.Log.Login.Print("CreateItem failed");
                return null;
            }
            return data;
        }

        public ulong GetPlayerGuidByName(ushort planesId,string name)
        {
            name = ServerFrame.DB.DBConnect.SqlSafeString(name);
            var pd = FindPlayerData(planesId,name);
            if (pd != null)
            {
                return pd.RoleDetail.RoleId;
            }

            CSCommon.Data.RoleDetail cu = new CSCommon.Data.RoleDetail();
            string condition = "RoleName=\'" + name + "\'";
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, cu, "");            
            System.Data.DataTable tab = IDataServer.Instance.DBLoaderConnect._ExecuteSelect(dbOp, "RoleInfo");
            if (tab == null || tab.Rows.Count != 1)
                return 0;

            if (false == ServerFrame.DB.DBConnect.FillObject(cu, tab.Rows[0]))
                return 0;

            return cu.RoleId;
        }
        #endregion

        #region Save&Load
        public void ForceSavePlayerData(PlayerData pd)
        {
            PlayerDataEx outpd = FindPlayerData(pd.RoleDetail.RoleId);
            if (outpd == null)
            {
                return;
            }
            var saverThread = outpd.mSaverThread as Thread.PlayerSaverThread;
            if(saverThread==null)
            {
                Log.Log.Common.Print("ForceSavePlayerData saverThread is null");
                return;
            }
            ServerFrame.DB.DBConnect dbConnect = saverThread.DBConnect;
            SaveRoleDetail(true, dbConnect, pd.RoleDetail, outpd);
            SaveMartial(true, dbConnect, pd.MartialData, outpd);
            SaveTask(true, dbConnect, pd.TaskData, outpd);
            SaveItems(true, dbConnect, CSCommon.eItemInventory.ItemBag, pd.BagItems, outpd);
            SaveItems(true, dbConnect, CSCommon.eItemInventory.EquipGemBag, pd.BagItems, outpd);
            SaveItems(true, dbConnect, CSCommon.eItemInventory.EquipBag, pd.BagItems, outpd);
            SaveItems(true, dbConnect, CSCommon.eItemInventory.FashionBag, pd.BagItems, outpd);
            SaveItems(true, dbConnect, CSCommon.eItemInventory.GemBag, pd.BagItems, outpd);
            SaveSkills(true, dbConnect, pd.SkillDatas, outpd);
            SaveAchieve(true, dbConnect, pd.AchieveData, outpd);

        }
        public void SavePlayerData(PlayerData pd)
        {
            PlayerDataEx outpd = FindPlayerData(pd.RoleDetail.RoleId);
            if (outpd == null)
                return;
            var saverThread = outpd.mSaverThread as Thread.PlayerSaverThread;
            if (saverThread == null)
            {
                Log.Log.Common.Print("SavePlayerData saverThread is null");
                return;
            }
            ServerFrame.DB.DBConnect dbConnect = saverThread.DBConnect;
            SaveRoleDetail(false, dbConnect, pd.RoleDetail, outpd);
            SaveMartial(false, dbConnect, pd.MartialData, outpd);
            SaveTask(false, dbConnect, pd.TaskData, outpd);
            SaveItems(false, dbConnect, CSCommon.eItemInventory.ItemBag, pd.BagItems, outpd);
            SaveItems(false, dbConnect, CSCommon.eItemInventory.EquipGemBag, pd.EquipGemItems, outpd);
            SaveItems(false, dbConnect, CSCommon.eItemInventory.EquipBag, pd.EquipedItems, outpd);
            SaveItems(false, dbConnect, CSCommon.eItemInventory.FashionBag, pd.FashionItems, outpd);
            SaveItems(false, dbConnect, CSCommon.eItemInventory.GemBag, pd.GemItems, outpd);
            SaveSkills(false, dbConnect, pd.SkillDatas, outpd);
            SaveAchieve(false, dbConnect, pd.AchieveData, outpd);
        }
        #endregion

        #region RPC Method
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void LoginAccount(UInt16 cltIndex, string name, string psw,ushort planesId, ulong LinkSerialId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            Thread.AccountLoginHolder loginHolder = new Thread.AccountLoginHolder();
            loginHolder.LinkHandle = cltIndex;
            loginHolder.Name = name.ToLower();
            loginHolder.Password = psw;
            loginHolder.PlanesId = planesId;
            loginHolder.Connect = connect;
            loginHolder.ReturnSerialId = fwd.ReturnSerialId;
            loginHolder.LinkSerialId = LinkSerialId;
            Thread.AccountLoginThread.Instance.PushLogin(loginHolder);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void LogoutAccount(ulong accountId, sbyte serverType)
        {
            var account = this.FindAccountInfo(accountId);
            if (account != null)//如果正在登陆排队，取消登陆
            {
                ServerCommon.Thread.AccountLoginThread.Instance.CancelLogin(account.UserName);
                mAccountDict.Remove(account.Id);
                mAccountNameDict.Remove(account.UserName);

                if (account.CurPlayer != null)
                {
                    ServerCommon.Thread.DBConnectManager.Instance.RemovePlayer(account.CurPlayer);
                    this.LogoutCurPlayer(account, null);
                }
            }
            else
            {
                //System.Diagnostics.Debugger.Break();
                Log.Log.Server.Info("LogoutAccount FindAccountInfo NULL");
            }
        }

        public void ClearAccount(AccountInfo account)
        {
            if (account != null)//如果正在登陆排队，取消登陆
            {
                ServerCommon.Thread.AccountLoginThread.Instance.CancelLogin(account.UserName);
                mAccountDict.Remove(account.Id);
                mAccountNameDict.Remove(account.UserName);

                if (account.CurPlayer != null)
                {
                    ServerCommon.Thread.DBConnectManager.Instance.RemovePlayer(account.CurPlayer);
                    this.LogoutCurPlayer(account, null);
                }
            }
            else
            {
                //System.Diagnostics.Debugger.Break();
                Log.Log.Server.Info("ClearAccount FindAccountInfo NULL");
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void LoginRole(ulong linkSerialId, UInt16 cltIndex, ulong roleId, ulong accountId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            Thread.RoleEnterHolder holder = new Thread.RoleEnterHolder();
            holder.linkSerialId = linkSerialId;
            holder.cltIndex = cltIndex;
            holder.roleId = roleId;
            holder.accountId = accountId;
            holder.connect = connect;
            holder.returnSerialId = fwd.ReturnSerialId;
            Thread.PlayerEnterThread.Instance.Push(holder,true);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public sbyte LogoutRole(ulong accountId, PlayerData pd)
        {
            var account = this.FindAccountInfo(accountId);

            if (this.LogoutCurPlayer(account, pd))
                return 1;
            return -1;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void SaveRole(ulong roleId, CSCommon.Data.PlayerData pd)
        {
            SavePlayerData(pd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RoleEnterPlanesSuccessed(ulong roleId,Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            var pd = FindPlayerData(roleId);
            if (pd != null)
            {
                //pd.mLandStep = PlayerLandStep.EnterGame;

                //RPC.PackageWriter retPkg = new RPC.PackageWriter();
                //retPkg.Write((sbyte)1);
                //retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
            }
            else
            {
                //RPC.PackageWriter retPkg = new RPC.PackageWriter();
                //retPkg.Write((sbyte)-1);
                //retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void UpdataPlayerGuildData(ulong roleId, Guid GuildId, string GuildName, UInt32 GuildContribution, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            if (DB_UpdataPlayerGuildData(roleId, GuildId, GuildName, GuildContribution))
            {
                retPkg.Write((sbyte)1);
            }
            else
            {
                retPkg.Write((sbyte)-1);
            }
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        #endregion
    }
}
