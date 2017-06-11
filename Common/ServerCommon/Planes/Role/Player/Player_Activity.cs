using System;
using System.Collections.Generic;

namespace ServerCommon.Planes
{
    public class DayRewardInfo
    {
        public int id;
        public int num;
        public int neednum;
    }

    public class DayRewardInfos
    {
        public List<DayRewardInfo> infos = new List<DayRewardInfo>();

        public DayRewardInfos(string info)
        {
            if (string.IsNullOrEmpty(info))
            {
                Log.Log.Common.Info("DayRewardInfo is null");
                return;
            }
            var temp = info.Split('|');
            foreach(var i in temp)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    var tt = i.Split(',');
                    DayRewardInfo dri = new DayRewardInfo();
                    dri.id = Convert.ToInt32(tt[0]);
                    dri.num = Convert.ToInt32(tt[1]);
                    dri.neednum = Convert.ToInt32(tt[2]);
                    infos.Add(dri);
                }
            }
        }
    }

    public partial class PlayerInstance : RPC.RPCObject
    {
        #region 成就
        RecordManager mRecordMgr = new RecordManager();

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetAchieve(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var list = mRecordMgr.GetAchieveList(CSCommon.eAchieveType.Achieve);
            int count = list.Count;

            pkg.Write(count);
            foreach (var i in list)
            {
                pkg.Write(i.data.id);
                pkg.Write(i.data.targetNum);
                pkg.Write(i.data.getReward);
            }
            pkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetAchieveName(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var list = mRecordMgr.GetAchieveList(CSCommon.eAchieveType.AchieveName);
            int count = list.Count;

            pkg.Write(count);
            foreach (var i in list)
            {
                pkg.Write(i.data.id);
                pkg.Write(i.data.targetNum);
                pkg.Write(i.data.getReward);
            }
            pkg.DoReturnPlanes2Client(fwd);
        }

        //领取奖励
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetAchieveReward(int id, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var achieve = mRecordMgr.GetAchieve(id);
            if (!achieve.IsFinished())
            {
                //未完成
                pkg.Write((sbyte)CSCommon.eRet_GetAchieveReward.NoFinish);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var tpl = CSTable.ItemUtil.GetAchieve(id);
            if (tpl == null)
            {
                //id错误
                pkg.Write((sbyte)CSCommon.eRet_GetAchieveReward.IdError);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }

            if (tpl.atype == (int)CSCommon.eAchieveType.Achieve)
            {
                if (achieve.data.getReward == (byte)CSCommon.eBoolState.True)
                {
                    //已领取
                    pkg.Write((sbyte)CSCommon.eRet_GetAchieveReward.YetGet);
                    pkg.DoReturnPlanes2Client(fwd);
                    return;
                }

                var achievetpl = tpl as CSTable.AchieveTplData;
                GetReward(achievetpl.currencyReward, achievetpl.itemsReward);
            }
            else if (tpl.atype == (int)CSCommon.eAchieveType.AchieveName)
            {
                //启用称号
                mRecordMgr.UseAchieveName(id);
                var achieveNametpl = tpl as CSTable.AchieveNameData;
                this.PlayerData.RoleDetail.AchieveName = achieveNametpl.name;
            }

            achieve.data.getReward = (byte)CSCommon.eBoolState.True;
            pkg.Write((sbyte)CSCommon.eRet_GetAchieveReward.Succeed);
            pkg.DoReturnPlanes2Client(fwd);
        }

        #endregion

        #region 每日
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetMyRank(byte type, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_GetMyRank(pkg, this.Id, type);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result = _io.ReadSByte();
                retPkg.Write(result);
                if (result == 1)
                {
                    int nowrank = _io.ReadInt32();//今日排名
                    byte count = _io.ReadByte();//前10名数据
                    retPkg.Write(nowrank);
                    retPkg.Write(count);
                    for (int i = 0; i < count; i++)
                    {
                        string name = _io.ReadString();
                        ushort level = _io.ReadUInt16();
                        int value = _io.ReadInt32();
                        retPkg.Write(name);
                        retPkg.Write(level);
                        retPkg.Write(value);
                    }
                    int lastrank = 0;
                    int nowValue = 0;
                    byte rewarddata = 0;
                    _GetRankByType(type, ref lastrank, ref nowValue, ref rewarddata);
                    retPkg.Write(lastrank);//昨日排名
                    retPkg.Write(nowValue);//今日数据
                    retPkg.Write(rewarddata);//箱子数量
                }
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            };
        }

        //开箱子
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_OpenExploitBox(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            if (this.PlayerData.RankData.ExploitBox <= 0)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }

            var boxId = CSCommon.RoleCommon.Instance.ExploitBoxId;
            var tpl = CSTable.ItemUtil.GetItem(boxId) as CSTable.ItemPackageData;
            if (tpl == null)
            {
                return;
            }
            var dropId = tpl.DropId;
            List<IdNum> items = new List<IdNum>();
            int mul = GetDropItems(dropId, items);
            this.PlayerData.RankData.ExploitBox -= 1;
            pkg.Write((sbyte)1);
            pkg.Write(mul);
            pkg.Write(items.Count);
            foreach(var i in items)
            {
                pkg.Write(i.templateId);
                pkg.Write(i.stackNum);
            }
            pkg.DoReturnPlanes2Client(fwd);
            return;
        }

        //获取每日成就奖励
//         [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
//         public void RPC_GetDayDataReward(byte type, byte index, RPC.RPCForwardInfo fwd)
//         {
//             RPC.PackageWriter pkg = new RPC.PackageWriter();
//             sbyte result = -1;
//             switch ((CSCommon.eRankType)type)
//             {
//                 case CSCommon.eRankType.KillEnemy:
//                     {
//                         int id = this.PlayerData.RankData.KillEnemyRewardId;//奖励ID
//                         byte reward = this.PlayerData.RankData.KillEnemyReward;//奖励领取情况
//                         int data = this.PlayerData.RankData.KillEnemy;//已经完成数据
//                         result = GetDayDataReward(index, id, ref reward, data);
//                         if (result == (sbyte)CSCommon.eRet_GetDayDataReward.Succeed)
//                         {
//                             this.PlayerData.RankData.KillEnemyReward = reward;
//                         }
// 
//                     }
//                     break;
//                 case CSCommon.eRankType.BurnFood:
//                     {
//                         int id = this.PlayerData.RankData.BurnFoodRewardId;//奖励ID
//                         byte reward = this.PlayerData.RankData.BurnFoodReward;//奖励领取情况
//                         int data = this.PlayerData.RankData.BurnFood;//已经完成数据
//                         result = GetDayDataReward(index, id, ref reward, data);
//                         if (result == (sbyte)CSCommon.eRet_GetDayDataReward.Succeed)
//                         {
//                             this.PlayerData.RankData.BurnFoodReward = reward;
//                         }
//                     }
//                     break;
//                 case CSCommon.eRankType.Robbery:
//                     {
//                         int id = this.PlayerData.RankData.RobberyRewardId;//奖励ID
//                         byte reward = this.PlayerData.RankData.RobberyReward;//奖励领取情况
//                         int data = this.PlayerData.RankData.Robbery;//已经完成数据
//                         result = GetDayDataReward(index, id, ref reward, data);
//                         if (result == (sbyte)CSCommon.eRet_GetDayDataReward.Succeed)
//                         {
//                             this.PlayerData.RankData.RobberyReward = reward;
//                         }
//                     }
//                     break;
//                 case CSCommon.eRankType.KillArmy:
//                     {
//                         int id = this.PlayerData.RankData.KillArmyRewardId;//奖励ID
//                         byte reward = this.PlayerData.RankData.KillArmyReward;//奖励领取情况
//                         int data = this.PlayerData.RankData.KillArmy;//已经完成数据
//                         result = GetDayDataReward(index, id, ref reward, data);
//                         if (result == (sbyte)CSCommon.eRet_GetDayDataReward.Succeed)
//                         {
//                             this.PlayerData.RankData.KillArmyReward = reward;
//                         }
//                     }
//                     break;
//                 case CSCommon.eRankType.Challenge:
//                     {
//                         int id = this.PlayerData.RankData.ChallengeRewardId;//奖励ID
//                         byte reward = this.PlayerData.RankData.ChallengeReward;//奖励领取情况
//                         int data = this.PlayerData.RankData.Challenge;//已经完成数据
//                         result = GetDayDataReward(index, id, ref reward, data);
//                         if (result == (sbyte)CSCommon.eRet_GetDayDataReward.Succeed)
//                         {
//                             this.PlayerData.RankData.ChallengeReward = reward;
//                         }
//                     }
//                     break;
//             }
//             pkg.Write(result);
//             pkg.DoReturnPlanes2Client(fwd);
//             return;
//         }

        //获取每日排名奖励

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetDayRankDataReward(byte type, RPC.RPCForwardInfo fwd)
        {

        }

        //private sbyte GetDayDataReward(byte index, int id, ref byte reward, int data)
        //{
        //    int pindex = (int)System.Math.Pow(2, index);
        //    if (CheckEverGet(reward, pindex))
        //    {
        //        return (sbyte)CSCommon.eRet_GetDayDataReward.EverGet;
        //    }
        //    var info = GetRewardInfo(id, index);
        //    if (info == null)
        //    {
        //        return (sbyte)CSCommon.eRet_GetDayDataReward.TplError;
        //    }
        //    int need = info.neednum;
        //    if (need > data)//3
        //    {
        //        //条件不满足
        //        return (sbyte)CSCommon.eRet_GetDayDataReward.CanotGet;
        //    }
        //    //领取物品
        //    if (this.Bag.GetEmptyCount() <= 0)
        //    {
        //        return (sbyte)CSCommon.eRet_GetDayDataReward.BagFull;
        //    }
        //    if (false == CreateItemToBag(info.id, info.num))
        //    {
        //        return (sbyte)CSCommon.eRet_GetDayDataReward.AddBagError;
        //    }
        //    reward |= (byte)pindex;
        //    return (sbyte)CSCommon.eRet_GetDayDataReward.Succeed;
        //}

        public void _GetRankByType(byte type, ref int lastrank, ref int nowValue, ref byte rewarddata)
        {
            switch ((CSCommon.eRankType)type)
            {
                case CSCommon.eRankType.Exploit:
                    {
                        lastrank = this.PlayerData.RankData.ExploitRank;
                        nowValue = this.PlayerData.RankData.Exploit;
                        rewarddata = this.PlayerData.RankData.ExploitBox;
                    }
                    break;
                case CSCommon.eRankType.Prestige:
                    {
                        lastrank = this.PlayerData.RankData.PrestigeRank;
                        nowValue = this.PlayerData.RankData.Prestige;
                        rewarddata = this.PlayerData.RankData.PrestigeBox;
                    }
                    break;
            }
        }

        public void AddRankData(CSCommon.eRankType type, int addvalue)
        {
            switch (type)
            {
                case CSCommon.eRankType.Exploit:
                    {
                        CheckGetExploitBox(this.PlayerData.RankData.Exploit, addvalue);
                        this.PlayerData.RankData.Exploit += addvalue;
                    }
                    break;
                case CSCommon.eRankType.Prestige:
                    break;
            }
        }

        //获取箱子
        public void CheckGetExploitBox(int oldExploit, int addvalue)
        {
            var list = CSCommon.RoleCommon.Instance.ExploitReward;
            for (int i = 0; i < list.Count; i++)
            {
                if (oldExploit < list[i].NeedNum)
                {
                    if (oldExploit + addvalue >= list[i].NeedNum)
                    {
                        this.PlayerData.RankData.ExploitBox += list[i].GetNum;
                    }
                }
            }
        }

//         public bool CheckEverGet(byte reward, int pindex)
//         {
//             int isget = this.PlayerData.RankData.KillEnemyReward & pindex;//2
//             if (isget > 0)
//             {
//                 //已领取
//                 return true;
//             }
//             return false;
//         }

//         public DayRewardInfo GetRewardInfo(int id, byte index)
//         {
//             string reward = CSTable.StaticDataManager.DayRank.GetReward(id);
//             DayRewardInfos RewardInfo = new DayRewardInfos(reward);
//             if (index >= RewardInfo.infos.Count)
//             {
//                 //index错误
//                 return null;
//             }
//             var info = RewardInfo.infos[index];
//             return info;
//         }

        public void OnRankValueChanged(string name, RPC.DataWriter dw)//变了之后,告诉rankdata
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_UpdateRankDataValue(pkg, this.Id, name, dw);
            pkg.DoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType);
        }
        #endregion

        #region 副本
        public int mInstanceId;
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_EnterCopy(int id , RPC.RPCForwardInfo fwd)
        {
            var tpl = CSTable.StaticDataManager.CopyTpl[id];
            if (tpl == null)
            {
                return;
            }
            if (_IsLevelEnough((ushort)tpl.playerlevel) == false)
            {
                return;
            }
            mInstanceId = id;
            ushort mapid = (ushort)tpl.mapid;
            var x = CSTable.StaticDataManager.Maps[mapid].startX;
            var z = CSTable.StaticDataManager.Maps[mapid].startY;
            JumpToMap(mapid, x, 0, z, fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_LeaveCopy(RPC.RPCForwardInfo fwd)
        {
            ReturnToHostMap(fwd);
        }

        public int ComplateNum = 0;
        public void CheckCopy(string condition = "")
        {
            //if (mCopyTplData == null)
            //{
            //    return;
            //}
            //CSCommon.eCopyType type = (CSCommon.eCopyType)mCopyTplData.type;
            //switch (type)
            //{
            //    case CSCommon.eCopyType.KillAll:
            //        {
            //            ComplateNum++;
            //            var challenge = this.mHostMap as ChallengeInstance;
            //            if (ComplateNum >= challenge.mTotalNpcNum)
            //            {
            //                //完成
            //                ComplateNum = 0;
            //                challenge.mWave++;
            //                if (challenge.mWave < challenge.mTotalWave )
            //                {
            //                    challenge.RebornNpc(challenge.mWave);
            //                }
            //                else
            //                {
            //                    TellToClientCopyEnd(true);
            //                }
            //            }
            //        }
            //        break;
            //    case CSCommon.eCopyType.Collect:
            //        {

            //        }
            //        break;
            //    case CSCommon.eCopyType.KillTarget:
            //        {
            //            if (mCopyTplData.targetId == Convert.ToInt32(condition))
            //            {
            //                //完成
            //                var map = mHostMap as ChallengeInstance;
            //                map.ClearNpc();
            //                TellToClientCopyEnd(true);
            //            }
            //        }
            //        break;
            //    case CSCommon.eCopyType.ProtectTarget:
            //        {
            //            if (mCopyTplData.targetId == Convert.ToInt32(condition))
            //            {
            //                //失败
            //                var map = mHostMap as ChallengeInstance;
            //                map.ClearNpc();
            //                TellToClientCopyEnd(false);
            //            }
            //        }
            //        break;
            //}
        }
        public void TellToClientCopyEnd(bool rlt)
        {
            byte result = (byte)CSCommon.eBoolState.False;
            if (rlt)
            {
                if (!mRecordMgr.CopyList.Contains(mInstanceId))
                {
                    mRecordMgr.CopyList.Add(mInstanceId);
                }
                result = (byte)CSCommon.eBoolState.True;
            }
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.RPC_OnCopyEnd(pkg, result);
            pkg.DoCommandPlanes2Client(this.Planes2GateConnect, this.ClientLinkId);
        }

        public void TellToClientCopyCountDown(float second)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.RPC_OnCopyCountDown(pkg, second);
            pkg.DoCommandPlanes2Client(this.Planes2GateConnect, this.ClientLinkId);
        }


        #endregion
    }
}