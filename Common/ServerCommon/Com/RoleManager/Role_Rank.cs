using CSCommon.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ServerCommon.Com
{
    public partial class UserRoleManager : RPC.RPCObject
    {
        #region 数据
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

        Random mRandom = new Random();

        Dictionary<ulong, RankData> mRankDatas = new Dictionary<ulong, RankData>();
        public Dictionary<ulong, RankData> RankDatas
        {
            get { return mRankDatas; }
        }

        MySqlDataAdapter mAdap = null;

        public void RankInit()
        {
            ServerFrame.TimerManager.RegTodayTimerEvent(13.75f, UpdateToSql);
        }

        //从数据库获取所有rank数据
        public void DownloadRankData()
        {
            string strSQL = "SELECT * FROM RankData";
            MySqlCommand command = new MySqlCommand(strSQL, mDBConnect.mConnection);
            mAdap = new MySqlDataAdapter(command);
            MySqlCommandBuilder msb = new MySqlCommandBuilder(mAdap);
            mAdap.InsertCommand = msb.GetInsertCommand();
            mAdap.UpdateCommand = msb.GetUpdateCommand();
            mAdap.DeleteCommand = msb.GetDeleteCommand();
            mDataSet = new DataSet();
            mAdap.Fill(mDataSet, "rankdata");
            foreach (DataRow i in mDataSet.Tables["rankdata"].Rows)
            {
                RankData rd = new RankData();
                ServerFrame.DB.DBConnect.FillObject(rd, i);
                mRankDatas[rd.RoleId] = rd;
            }
        }

        //更新到数据库
        public void UpdateToSql(ServerFrame.TimerEvent timerEvent)
        {
            if (mAdap == null || mDataSet == null)
            {
                return;
            }
            TimeToRank();
            try
            {
                Log.Log.Common.Info("更新rankdata");
                mAdap.Update(mDataSet, "rankdata");
            }
            catch(System.Exception ex)
            {
                Log.Log.Common.Info(ex.ToString());
                Log.Log.Common.Info(ex.StackTrace.ToString());
            }
            
        }

        public RankData GetRank(ulong id)
        {
            RankData rd = new RankData();
            lock (this)
            {
                if (mRankDatas.TryGetValue(id, out rd) == true)
                    return rd;
            }

            UserRole role = GetRole(id);
            if (role == null)
                return null;

            rd = DB_LoadRankData(role);
            if (rd == null)
                return null;

            UPdateRankData(rd);
            return rd;
        }

        public RankData DB_LoadRankData(UserRole role)
        {
            RankData rd = new RankData();
            rd.RoleId = role.RoleData.RoleId;
            rd.PlanesId = role.RoleData.PlanesId;
            rd.RoleName = role.RoleData.Name;
            rd.Level = role.RoleData.Level;
            rd.Fighting = 0;
            rd.Exploit = 0;
            rd.ExploitRank = 0;

            string condition = "RoleId=" + rd.RoleId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, rd, false);
            if (false == mDBConnect._ExecuteInsert(dbOp))
            {
                Log.Log.Common.Print("create RankData failed");
                return null;
            }
            
            return rd;
        }
        #endregion

        #region 更新数据
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_UpdateRankDataValue(ulong roleId, string name, RPC.DataReader value)
        {
            RankData rank = GetRank(roleId);
            if (rank == null)
            {
                return;
            }
            switch (name)
            {
                case "RoleLevel":
                    rank.Level = value.ReadUInt16();
                    break;
                case "Fighting":
                    rank.Fighting = value.ReadInt32();
                    break;
                case "Exploit":
                    rank.Exploit = value.ReadInt32();
                    break;
                case "ExploitBox":
                    rank.ExploitBox = value.ReadByte();
                    break;
                case "KillEnemy":
                    rank.KillEnemy = value.ReadInt32();
                    break;
                case "Challenge":
                    rank.Challenge = value.ReadInt32();
                    break;
            }
            UPdateRankData(rank);
        }

        //更新一条
        public void UPdateRankData(RankData rd)
        {
            mRankDatas[rd.RoleId] = rd;
            DataTable dt = DataSet.Tables["RankData"];
            string sql = string.Format("RoleId = \'{0}\'", rd.RoleId);
            DataRow[] rows = dt.Select(sql);
            int count = rows.Count();
            if (count == 0)
            {
                DataRow row = DataSet.Tables["RankData"].NewRow();
                ServerFrame.DB.DBConnect.FillDataRow(row, rd);  
                DataSet.Tables["RankData"].Rows.Add(row);
            }
            else
            {
                DataRow row = DataSet.Tables["RankData"].NewRow();
                ServerFrame.DB.DBConnect.FillDataRow(row, rd);
                rows[0] = row;
            }
        }
        #endregion
        
        #region 武馆排行榜
        public void RPC_GetFightTopPlayer(ushort planesId)
        {
            List<RankData> rds = _GetFightTopPlayer(planesId);

        }

        public List<RankData> _GetFightTopPlayer(ushort planesId)
        {
            string planesCondition = "";
            if (planesId != 0)
            {
                planesCondition = "PlanesId = \'" + planesId + "\'";
            }
            int count = CSCommon.MartialClubCommon.Instance.RankMaxCount;
            List<RankData> rds = new List<RankData>();
            var temp = mDataSet.Tables["RankData"].Select(planesCondition, "Fighting Desc");
            for (int i = 0; i < count; i++)
            {
                RankData rd = new RankData();
                ServerFrame.DB.DBConnect.FillObject(rd, temp[i]);
                rds.Add(rd);
            }
            return rds;
        }

        public void RPC_GetFightNearRandomPlayer(ushort planesId, ulong roleId)
        {
            var rank = GetRank(roleId);
            if (rank == null)
            {
                return;
            }
            int count = CSCommon.MartialClubCommon.Instance.RankMaxCount;
            if (mDataSet.Tables["RankData"].Rows.Count < count)
            {
                return;
            }
            string planesCondition = "";
            if (planesId != 0)
            {
                planesCondition = " and " + "PlanesId = \'" + planesId + "\'";
            }
            int updown = 100;
            List<RankData> rds = new List<RankData>();
            int upft = rank.Fighting + updown;
            int downft = rank.Fighting - updown;
            DataRow[] temp = null;
            bool selected = true;
            while (selected)
            {
                string sql = "Fighting > \'" + downft + "\'" + " and " + "Fighting < \'" + upft + "\'" + planesCondition;
                temp = mDataSet.Tables["RankData"].Select(sql);
                if (temp.Count() >= count)
                {
                    selected = false;
                }
                else
                {
                    updown += 100;
                    upft = rank.Fighting + updown;
                    downft = rank.Fighting - updown;
                }
            }
            List<int> list = new List<int>();
            int rMaxNum = temp.Count();
            int getCount = 0;
            while (getCount < count)
            {
                int rand = mRandom.Next(rMaxNum);
                if (!list.Contains(rand))
                {
                    list.Add(rand);
                    getCount++;
                }
            }
            foreach (var i in list)
            {
                RankData rd = new RankData();
                if (ServerFrame.DB.DBConnect.FillObject(rd, temp[i]))
                {
                    rds.Add(rd);
                }
            }
        }
        #endregion

        #region 历练排行榜

        //每日固定时间排行100名
        public void TimeToRank()
        {
            int ranknum = 100;
            foreach(var p in Planes)
            {
                var planesCondition = "PlanesId = \'" + p.Value.PlanesId + "\'";
                for (int type = (int)CSCommon.eRankType.Exploit; type < (int)CSCommon.eRankType.Max; type++)
                {
                    _UpdateRank(ranknum, planesCondition, (CSCommon.eRankType)type);
                }
            }
        }

        //历练各种排行榜
        private void _UpdateRank(int ranknum, string planesCondition, CSCommon.eRankType type)
        {
            string sort = "";
            string rankname = "";
            string rankatt = "";
            GetRankName(type, ref sort, ref rankname, ref rankatt);
            var temp = mDataSet.Tables["RankData"].Select(planesCondition, sort);
            for (int i = 0; i < temp.Count(); i++)
            {
                if (i < ranknum)
                {
                    temp[i][rankname] = i + 1;
                }
                else
                {
                    temp[i][rankname] = 0;
                }
                temp[i][rankatt] = 0;
                temp[i]["Challenge"] = 0;
                temp[i]["KillEnemy"] = 0;
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_GetMyRank(ulong id, byte type, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var rank = GetRank(id);
            if (rank == null)
            {
                Log.Log.Common.Info("RPC_GetMyRank GetRank null{0}", id);
                pkg.Write((sbyte)-1);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (type >= (byte)CSCommon.eRankType.Max)
            {
                Log.Log.Common.Info("RPC_GetMyRank type error{0}", type);
                pkg.Write((sbyte)-2);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            pkg.Write((sbyte)1);
            string sort = "";
            _GetSort(type, ref sort);
            int nowrank = 0;//现在排名
            List<RankData> toplist = new List<RankData>(); 
            string condition = "PlanesId = \'" + rank.PlanesId + "\'";
            nowrank = _GetMyRank(id, toplist, condition, sort, 10);
            pkg.Write(nowrank);
            pkg.Write((byte)toplist.Count);
            foreach(var i in toplist)
            {
                pkg.Write(i.RoleName);
                pkg.Write(i.Level);
                int value = _GetReturnRankValue(type, i);
                pkg.Write(value);
            }
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        private int _GetMyRank(ulong id, List<RankData> toplist, string condition, string sort, int getcount)
        {
            int maxnum = 100;//最大排名
            int nowrank = 0;
            var temp = mDataSet.Tables["RankData"].Select(condition, sort);
            int count = temp.Count();
            for (int i = 0; i < count; i++)
            {
                if (i >= maxnum)
                {
                    break;
                }
                if (Convert.ToUInt64(temp[i]["RoleId"]) == id)
                {
                    nowrank = i;
                }
                if (i < getcount)
                {
                    RankData rd = new RankData();
                    ServerFrame.DB.DBConnect.FillObject(rd, temp[i]);
                    toplist.Add(rd);
                }
            }
            return nowrank;
        }

        private int _GetReturnRankValue(byte type, RankData data)
        {
            switch ((CSCommon.eRankType)type)
            {
                case CSCommon.eRankType.Exploit:
                    return data.Exploit;
                case CSCommon.eRankType.Prestige:
                    return data.Prestige;
            }
            return 0;
        }

        private void _GetSort(byte type, ref string sort)
        {
            switch ((CSCommon.eRankType)type)
            {
                case CSCommon.eRankType.Exploit:
                    {
                        sort = "Exploit Desc";
                    }
                    break;
                case CSCommon.eRankType.Prestige:
                    {
                        sort = "Prestige Desc";
                    }
                    break;
            }
        }

        public void GetRankName(CSCommon.eRankType type, ref string sort, ref string rankname, ref string rankatt)
        {
            switch (type)
            {
                case CSCommon.eRankType.Exploit:
                    {
                        sort = "Exploit Desc";
                        rankname = "ExploitRank";
                        rankatt = "Exploit";
                    }
                    break;
                case CSCommon.eRankType.Prestige:
                    {
                        sort = "Prestige Desc";
                        rankname = "PrestigeRank";
                        rankatt = "Prestige";
                    }
                    break;
            }
        }
        #endregion

        #region 竞技场排行榜

        //前20名，相邻10名,自己当前排名
        public void RPC_GetArenaRank(ulong id)
        {
            var rank = GetRank(id);
            int nowrank = 0;//现在排名
            List<RankData> toplist = new List<RankData>();
            string sort = "ArenaPoint Desc";
            string condition = "PlanesId = \'" + rank.PlanesId + "\'";
            nowrank = _GetMyRank(id, toplist, condition, sort, 20);//获取前20名玩家
            

        }

        #endregion
    }
}
