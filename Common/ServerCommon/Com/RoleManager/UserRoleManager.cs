using CSCommon.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ServerCommon.Com
{
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class UserRole
    {
        RoleCom mRoleData = new RoleCom();
        public RoleCom RoleData
        {
            get { return mRoleData; }
        }

        Iocp.TcpConnect mPlanesConnect = null;
        public Iocp.TcpConnect PlanesConnect
        {
            get { return mPlanesConnect; }
            set 
            { 
                mPlanesConnect = value;
                if (mPlanesConnect == null)
                {
                    mTeamHeaderId = 0;
                }
            }
        } 

        public GuildInstance GuildInstance;

        SocialManager mSocialManager = new SocialManager();
        public SocialManager SocialManager
        {
            get { return mSocialManager; }
        }

        List<CSCommon.Data.SocialData> mSocials = new List<CSCommon.Data.SocialData>();
        public List<CSCommon.Data.SocialData> Socials
        {
            get { return mSocials; }
            set { mSocials = value; }
        }

        public Dictionary<ulong, CSCommon.Data.SocialRoleInfo> mSocialInfoList = new Dictionary<ulong, CSCommon.Data.SocialRoleInfo>();

        CSCommon.Data.GiftData mGiftData = new CSCommon.Data.GiftData();
        public CSCommon.Data.GiftData GiftData
        {
            get { return mGiftData; }
            set { mGiftData = value; }
        }

        ulong mTeamHeaderId = 0;
        public ulong TeamHeaderId
        {
            get { return mTeamHeaderId; }
        }

        public void SetTeamHeaderId(ulong id)
        {
            mTeamHeaderId = id;
        }

        public List<ulong> VisitTopList = new List<ulong>();
        public List<ulong> VisitFriendList = new List<ulong>();

        //初始化数据库数据
        public void VisitDataUnSerialize()
        {
            byte[] buffer = RoleData.VisitPlayers;
            VisitTopList.Clear();
            VisitFriendList.Clear();
            if (buffer == null || buffer.Length <= 0)
            {
                return;
            }
            RPC.DataReader dr = new RPC.DataReader(buffer, 0, buffer.Length, buffer.Length);
            byte count = dr.ReadByte();
            for (byte i = 0; i < count; i++)
            {
                ulong id = dr.ReadUInt64();
                VisitTopList.Add(id);
            }
            count = dr.ReadByte();
            for (byte i = 0; i < count; i++)
            {
                ulong id = dr.ReadUInt64();
                VisitFriendList.Add(id);
            }
        }

        //保存数据库调用
        public void VisitDataSerialize()
        {
            RPC.DataWriter dw = new RPC.DataWriter();
            byte count = (byte)VisitTopList.Count;
            dw.Write(count);
            foreach (var i in VisitTopList)
            {
                dw.Write(i);
            }
            count = (byte)VisitFriendList.Count;
            dw.Write(count);
            foreach (var i in VisitFriendList)
            {
                dw.Write(i);
            }
            RoleData.VisitPlayers = dw.Trim();
        }
    }

    [RPC.RPCClassAttribute(typeof(UserRoleManager))]
    public partial class UserRoleManager : RPC.RPCObject
    {
        #region RPC必须的基础定义
        public static RPC.RPCClassInfo smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
        {
            return smRpcClassInfo;
        }
        #endregion

        static UserRoleManager smInstance = new UserRoleManager();
        public static UserRoleManager Instance
        {
            get { return smInstance; }
        }

        Dictionary<ulong, UserRole> mRoles = new Dictionary<ulong, UserRole>();
        public Dictionary<ulong, UserRole> Roles
        {
            get { return mRoles; }
        }

        #region 角色相关
        
        public void Tick()
        {
//             foreach (var role in Roles)
//             {
//                 if (role.Value.TeamHeader != null)
//                 {
//                     role.Value.AskTeam.Clear();
//                 }
// 
//                 if (role.Value.Teams.Count > 4)
//                 {
//                     role.Value.InviteTeam.Clear();
//                 }
// 
//                 foreach (var invite in role.Value.InviteTeam)
//                 {
//                     var second = System.DateTime.Now - invite.time;
//                     if (second.TotalSeconds >= 30)
//                     {
//                         role.Value.InviteTeam.Remove(invite);
//                     }
//                 }
//                 foreach (var ask in role.Value.AskTeam)
//                 {
//                     var second = System.DateTime.Now - ask.time;
//                     if (second.TotalSeconds >= 30)
//                     {
//                         role.Value.InviteTeam.Remove(ask);
//                     }
//                 }
//             }
        }

        public UserRole GetRoleRand()
        {
            UserRole role = new UserRole();
            lock (this)
            {
                if (mRoles.Count > 0)
                {
                    int index = mRandom.Next(mRoles.Count);
                    role = mRoles.ToList()[index].Value;
                }
                return role;
            }
        }

        public UserRole GetRole(ulong roleId)
        {
            UserRole role = new UserRole();
            lock (this)
            {
                if (mRoles.TryGetValue(roleId, out role) == true)
                    return role;
            }

            role = DB_LoadRoleData(roleId);
            if (role == null)
                return null;

            if (!mRoles.ContainsKey(roleId))
                mRoles.Add(roleId, role);

            //这里还可以把角色按照位面放入管理器，方便位面说话
            DB_InitRole(role);
            return role;
        }

        public UserRole GetRole(string roleName)
        {
            foreach (var i in mRoles)
            {
                if (i.Value.RoleData.Name == roleName)
                {
                    return i.Value;
                }
            }

            var role = DB_LoadRoleData(roleName);
            if (role == null)
                return null;

            if (!mRoles.ContainsKey(role.RoleData.RoleId))
                mRoles.Add(role.RoleData.RoleId, role);

            DB_InitRole(role);
            return role;
        }

        ServerFrame.DB.DBConnect mDBConnect = new ServerFrame.DB.DBConnect();
        public ServerFrame.DB.DBConnect DBConnect
        {
            get { return mDBConnect; }
        }

        private UserRole DB_LoadRoleData(ulong roleId)
        {
            UserRole role = new UserRole();

            var condition = "RoleId=" + roleId;
            var dbOp = ServerFrame.DB.DBConnect.SelectData(condition, role.RoleData, "");
            var tab = mDBConnect._ExecuteSelect(dbOp, "RoleCom");
            if (tab == null || tab.Rows.Count != 1)
                return null;

            if (false == ServerFrame.DB.DBConnect.FillObject(role.RoleData, tab.Rows[0]))
                return null;

            
            return role;
        }

        private UserRole DB_LoadRoleData(string roleName)
        {
            UserRole role = new UserRole();

            var condition = "Name=\'" + roleName + "\'";
            var dbOp = ServerFrame.DB.DBConnect.SelectData(condition, role.RoleData, "");
            var tab = mDBConnect._ExecuteSelect(dbOp, "RoleCom");
            if (tab == null || tab.Rows.Count != 1)
                return null;

            if (false == ServerFrame.DB.DBConnect.FillObject(role.RoleData, tab.Rows[0]))
                return null;

            return role;
        }

        private void DB_InitRole(UserRole role)
        {
            role.VisitDataUnSerialize();
            if (role.RoleData.GuildId != 0)
            {
                role.GuildInstance = GuildManager.Instance.GetGuild(role.RoleData.GuildId);
                
            }

            DB_InitSocial(role);
            DB_InitGiftData(role);
        }

        private void DB_SaveRole(UserRole role)
        {
            role.VisitDataSerialize();
            DB_SaveRoleData(role);
            if (role.GuildInstance != null)
            {
                DB_SaveGuildCom(role.GuildInstance.GuildData);
            }
            DB_SaveSocials(role);
            DB_SaveGiftData(role);
        }

        #endregion

        #region 消息相关

        private void DB_RoleEnteInitMessages(UserRole role)//只有登录的时候调用
        {
            string condition = "OwnerId=" + role.RoleData.RoleId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, new CSCommon.Data.Message(), "");
            System.Data.DataTable tab = mDBConnect._ExecuteSelect(dbOp, "Message");
            if (tab != null)
            {
                foreach (System.Data.DataRow r in tab.Rows)
                {
                    CSCommon.Data.Message msg = new CSCommon.Data.Message();
                    if (false == ServerFrame.DB.DBConnect.FillObject(msg, r))
                        continue;
                    DB_DelMessage(msg);
                    //发送消息
                    RPC.PackageWriter pkg = new RPC.PackageWriter();
                    H_RPCRoot.smInstance.HGet_PlanesServer(pkg).RPC_SendPlayerMsg(pkg, role.RoleData.RoleId, msg);
                    pkg.DoCommand(role.PlanesConnect, RPC.CommandTargetType.DefaultType);

                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("角色获取信息数据库执行失败:" + dbOp.SqlCode);
            }
        }

        private bool DB_CreateMessage(CSCommon.Data.Message message)
        {
            string condition = "MessageId = " + message.MessageId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, message, true);
            return mDBConnect._ExecuteInsert(dbOp);
        }

        private void DB_DelMessage(CSCommon.Data.Message message)
        {
            string condition = "MessageId =" + message.MessageId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.DestroyData(condition, message);
            mDBConnect._ExecuteDestroy(dbOp);
            return;
        }


        #endregion

        #region 最近联系人相关

//         private void DB_CreatStranger(CSCommon.Data.StrangerInfo stranger)
//         {
//             // 产生insert语句
//             string condition = "EnemyerId = " + stranger.StrangerId;
//             ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, stranger, true);
//             mDBConnect._ExecuteInsert(dbOp);
//             return;
//         }
// 
//         private void DB_SaveStranger(CSCommon.Data.StrangerInfo stranger)
//         {
//             var condition = "EnemyerId=\'" + (stranger.StrangerId);
//             var dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, stranger, null);
//             mDBConnect._ExecuteUpdate(dbOp);
//             return;
//         }
// 
//         private void DB_DelStranger(CSCommon.Data.StrangerInfo stranger)
//         {
//             //产生Destroy语句
//             string condition = "EnemyerId =" + stranger.StrangerId;
//             ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.DestroyData(condition, stranger);
//             mDBConnect._ExecuteDestroy(dbOp);
//             return;
//         }

        #endregion

        #region 创建加载角色

        public UserRole TryCreatePlaye(CSCommon.Data.RoleDetail pd)
        {
            UserRole role = new UserRole();
            role.RoleData.RoleId = pd.RoleId;
            role.RoleData.Name = pd.RoleName;
            role.RoleData.Sex = pd.Sex;
            role.RoleData.Level = pd.RoleLevel;//change
            role.RoleData.Camp = pd.Camp;//change
            role.RoleData.Profession = pd.Profession;//change
            role.RoleData.PlanesId = pd.PlanesId;//change
            role.RoleData.MapName = pd.MapName;

            string condition = "RoleId=" + pd.RoleId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, role.RoleData, false);
            if (false == mDBConnect._ExecuteInsert(dbOp))
            {
                Log.Log.Common.Print("create rolecom failed");
                return null;
            }

            mRoles.Add(pd.RoleId, role);

            RankData rd = new RankData();
            rd.RoleId = pd.RoleId;
            rd.PlanesId = pd.PlanesId;
            rd.RoleName = pd.RoleName;
            rd.Level = pd.RoleLevel;
            rd.Fighting = 0;
            rd.Exploit = 0;
            rd.ExploitRank = 0;
            UPdateRankData(rd);
            return role;
        }

        #endregion

        #region RPC method

        #region 玩家进出游戏
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_RoleEnterPlanes(CSCommon.Data.RoleDetail roledata, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roledata.RoleId);
            if (role == null)//创建角色时调用
            {
                role = TryCreatePlaye(roledata);
                if (role == null)
                    return;
            }
            else
            {
                role.RoleData.Camp = roledata.Camp;
                role.RoleData.Name = roledata.RoleName;
                role.RoleData.Level = roledata.RoleLevel;
                role.RoleData.Profession = roledata.Profession;
                role.RoleData.PlanesId = roledata.PlanesId;
                role.RoleData.MapName = roledata.MapName;
            }

            role.PlanesConnect = connect as Iocp.TcpConnect;
            var rank = GetRank(roledata.RoleId);
            if (rank == null)
            {
                Log.Log.Common.Info("GetRank is null{0}", roledata.RoleId);
                pkg.Write(new RankData());
            }
            pkg.Write(rank);
            DB_RoleEnteInitMessages(role);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
            return;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_RoleLogout(ulong roleId, byte[] offvalue)
        {
            var role = GetRole(roleId);
            if (role == null)
            {
                return;
            }
            role.RoleData.FinalValue = offvalue;
            role.PlanesConnect = null;

            //存盘，这里后面要改成异步执行
            DB_SaveRole(role);

        }
        #endregion   

        #region 玩家共同数据操作

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_UpdateRoleComValue(ulong roleId, string name, RPC.DataReader value)
        {
            var role = GetRole(roleId);
            if (role == null)
            {
                return;
            }
            switch (name)
            {
                case "RoleLevel":
                    {
                        role.RoleData.Level = value.ReadUInt16();
                    }
                    break;
                case "Camp":
                    {
                        role.RoleData.Camp = value.ReadByte();
                    }
                    break;
                case "Profession":
                    {
                        role.RoleData.Profession = value.ReadByte();
                    }
                    break;
                case "PlanesId":
                    {
                        role.RoleData.PlanesId = value.ReadUInt16();
                    }
                    break;
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_SearchPlayerByName(string roleName, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var role = GetRole(roleName);
            if (role == null)
            {
                retPkg.Write((sbyte)-2);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            retPkg.Write((sbyte)1);
            retPkg.Write(role.RoleData);
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
            return;
        }
        #endregion

        #endregion

        #region Plane集合

        Dictionary<ushort, PlanesData> mPlanes = new Dictionary<ushort, PlanesData>();
        public Dictionary<ushort, PlanesData> Planes 
        {
            get { return mPlanes; }
        }

        public void DownloadPlanesData()
        {
            var dbOp = ServerFrame.DB.DBConnect.SelectData("", new PlanesData(), "");
            var tab = mDBConnect._ExecuteSelect(dbOp, "PlanesData");
            if (tab == null || tab.Rows.Count == 0)
                return;
            foreach (DataRow i in tab.Rows)
            {
                PlanesData pd = new PlanesData();
                if (ServerFrame.DB.DBConnect.FillObject(pd, i))
                {
                    mPlanes[pd.PlanesId] = pd;
                }
            }
        }
#endregion

        #region 拜访

        public void InitVisitTimes(UserRole role)
        {
            if (ServerFrame.Time.InitTimes((int)CSCommon.MartialClubCommon.Instance.FlushHour, role.RoleData.LastVisitTime))
            {
                role.RoleData.WorldVisitCount = 0;
                role.RoleData.FriendVisitCount = 0;
                role.RoleData.BuyVisitCount = 0;
                role.VisitFriendList.Clear();
                role.VisitTopList.Clear();
            }

        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_GetTopAndFriend(ulong roleId, ushort planesId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            TopList.Clear();
            FriendList.Clear();
            var role = this.GetRole(roleId);
            if (role == null)
            {
                Log.Log.Server.Print("RPC_GetTopAndFriend role is null , {0}", roleId);
                pkg.Write((sbyte)-1);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            pkg.Write((sbyte)1);
            InitVisitTimes(role);
            pkg.Write(role.RoleData.WorldVisitCount);
            pkg.Write(role.RoleData.FriendVisitCount);
            pkg.Write(role.RoleData.BuyVisitCount);
            pkg.Write(role.RoleData.ByVisitCount);
            List<RankData> rds = _GetFightTopPlayer(planesId);
            pkg.Write((byte)rds.Count);
            foreach(var i in rds)
            {
                pkg.Write(i.RoleId);
                pkg.Write(i.RoleName);
                if (role.VisitTopList.Contains(i.RoleId))
                {
                    pkg.Write((byte)CSCommon.eBoolState.True);
                }
                else
                {
                    pkg.Write((byte)CSCommon.eBoolState.False);
                }
                TopList.Add(i.RoleId);
            }
            UpdateSocialInfoList(CSCommon.eSocialType.Friend, role);
            pkg.Write((byte)role.mSocialInfoList.Count);
            foreach(var j in role.mSocialInfoList.Values)
            {
                pkg.Write(j.id);
                pkg.Write(j.name);
                if (role.VisitFriendList.Contains(j.id))
                {
                    pkg.Write((byte)CSCommon.eBoolState.True);
                }
                else
                {
                    pkg.Write((byte)CSCommon.eBoolState.False);
                }
                FriendList.Add(j.id);
            }
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        List<ulong> TopList = new List<ulong>();
        List<ulong> FriendList = new List<ulong>(); 

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_Visit(byte type, ulong roleId, ulong otherId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = this.GetRole(roleId);
            if (role == null)
            {
                Log.Log.Server.Print("RPC_Visit role is null , {0}", roleId);
                pkg.Write((sbyte)CSCommon.eRet_Visit.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var other = this.GetRole(otherId);
            if (other == null)
            {
                Log.Log.Server.Print("RPC_Visit other is null , {0}", otherId);
                pkg.Write((sbyte)CSCommon.eRet_Visit.NoOther);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (type == (byte)CSCommon.eVisitType.WorldVisit)
            {
                if (!TopList.Contains(roleId))
                {
                    pkg.Write((sbyte)CSCommon.eRet_Visit.IdError);
                    pkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                if (role.VisitTopList.Contains(otherId))
                {
                    pkg.Write((sbyte)CSCommon.eRet_Visit.Visited);
                    pkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                role.VisitTopList.Add(otherId);
            }
            else if (type == (byte)CSCommon.eVisitType.FriendVisit)
            {
                if (!FriendList.Contains(roleId))
                {
                    pkg.Write((sbyte)CSCommon.eRet_Visit.IdError);
                    pkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                if (role.VisitFriendList.Contains(otherId))
                {
                    pkg.Write((sbyte)CSCommon.eRet_Visit.Visited);
                    pkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                role.VisitFriendList.Add(otherId);
            }
            else
            {
                pkg.Write((sbyte)CSCommon.eRet_Visit.TypeError);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            other.RoleData.ByVisitCount++;
            if (other.RoleData.ByVisitCount >= CSCommon.MartialClubCommon.Instance.ByVisitMaxCount)
            {
                other.RoleData.ByVisitCount = (ushort)CSCommon.MartialClubCommon.Instance.ByVisitMaxCount;
            }
            role.RoleData.LastVisitTime = System.DateTime.Now;
            pkg.Write((sbyte)CSCommon.eRet_Visit.Succeed);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);



        }
        #endregion

        #region 匹配玩家

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_GetOffPlayerData(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var getRole = GetRoleRand();
            pkg.Write(getRole.RoleData);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);

        }

        #endregion
    }
}
