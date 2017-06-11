using CSCommon;
using CSCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public partial class PlayerInstance : RPC.RPCObject
    {
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_UpSkillLv(int skillId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var sk = SkillMgr.GetSkill(skillId);
            var sktpl = CSTable.SkillUtil.GetSkillTpl(skillId);
            if (sktpl == null)
            {
                pkg.Write((sbyte)eRet_UpSkillLv.NoSkill);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (sk == null)
            {
                //学习
                var sklvtpl = CSTable.SkillUtil.GetSkillLevelTpl(skillId, 1);
                if (sklvtpl == null)
                {
                    pkg.Write((sbyte)eRet_UpSkillLv.NoSkillLvTpl);
                    pkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                sbyte result = (sbyte)eRet_UpSkillLv.Failure;
                switch ((CSCommon.eSkillType)sktpl.type)
                {
                    case eSkillType.Active:
                        result = UpActiveSkill(sklvtpl);
                        break;
                    case eSkillType.Cheats://秘籍通过副本开启
                        break;
                    case eSkillType.BodyChannel:
                        result = UpBodyChannel(sklvtpl);
                        break;
                }
                if (result != (sbyte)eRet_UpSkillLv.Succeed)
                {
                    pkg.Write(result);
                    pkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                SkillMgr.CreateSkillToBag(sklvtpl.id, (byte)sklvtpl.level);
            }
            else
            {
                //升级
                if (sk.Level >= PlayerData.RoleDetail.RoleLevel)
                {
                    pkg.Write((sbyte)eRet_UpSkillLv.LessRoleLv);
                    pkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                if (sk.Level >= sk.Data.maxLv)
                {
                    pkg.Write((sbyte)eRet_UpSkillLv.MaxLv);
                    pkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                //下级模板
                var sklvtpl = CSTable.SkillUtil.GetSkillLevelTpl(skillId, sk.Level + 1);
                if (sklvtpl == null)
                {
                    pkg.Write((sbyte)eRet_UpSkillLv.NoSkillLvTpl);
                    pkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                sbyte result = (sbyte)eRet_UpSkillLv.Failure;
                switch ((CSCommon.eSkillType)sktpl.type)
                {
                    case eSkillType.Active:
                    case eSkillType.Hide:
                        result = UpActiveSkill(sklvtpl);
                        break;
                    case eSkillType.Cheats://秘籍通过其他方法
                        break;
                    case eSkillType.BodyChannel:
                        result = UpBodyChannel(sklvtpl);
                        break;
                }
                if (result != (sbyte)eRet_UpSkillLv.Succeed)
                {
                    pkg.Write(result);
                    pkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                sk.Init(sklvtpl.id, (byte)sklvtpl.level);
                sk.SetOwner(this);

            }
            pkg.Write((sbyte)eRet_UpSkillLv.Succeed);
            pkg.DoReturnPlanes2Client(fwd);
        }

        public sbyte UpActiveSkill(CSTable.ISkillLevelTable tpl)
        {
            var dict = ServerFrame.Util.ParsingStr(tpl.cost);
            foreach (var i in dict)
            {
                if (!_IsMoneyEnough((CSCommon.eCurrenceType)i.Key, i.Value))
                {
                    return (sbyte)eRet_UpSkillLv.LessMoney;
                }
            }
            foreach (var cost in dict)
            {
                _ChangeMoney((CSCommon.eCurrenceType)cost.Key, eMoneyChangeType.LearnSkill, -cost.Value);
            }
            return (sbyte)eRet_UpSkillLv.Succeed;
        }

        public sbyte UpBodyChannel(CSTable.ISkillLevelTable tpl)
        {
            var btemp = CSCommon.RoleCommon.Instance;
            if (btemp == null)
            {
                return (sbyte)eRet_UpSkillLv.NoSkill;
            }
            if (PlayerData.RoleDetail.FreeUpMuscleTimes >= btemp.FreeCount)//免费次数用完
            {
                if (PlayerData.RoleDetail.UpMuscleTimes >= btemp.PayCount)//花钱次数用完
                {
                    return (sbyte)eRet_UpSkillLv.UpNum;
                }

                if (!_IsMoneyEnough(CSCommon.eCurrenceType.Rmb, btemp.PayMoneyNum))
                {
                    return (sbyte)eRet_UpSkillLv.LessMoney;
                }
                _ChangeMoney(CSCommon.eCurrenceType.Rmb, eMoneyChangeType.UpSkill, -btemp.PayMoneyNum);
                PlayerData.RoleDetail.UpMuscleTimes++;
                PlayerData.RoleDetail.LastUpMuscleTime = System.DateTime.Now;
            }
            else
            {
                PlayerData.RoleDetail.FreeUpMuscleTimes++;
                PlayerData.RoleDetail.LastUpMuscleTime = System.DateTime.Now;
            }
            return (sbyte)eRet_UpSkillLv.Succeed;
        }

        public sbyte UpCheats(CSTable.ISkillLevelTable tpl, byte costType)
        {
            var ctpl = tpl as CSTable.CheatsData;
            int cost = 0;
            float rate = 0;
            eCurrenceType costtype = eCurrenceType.Gold;
            if (costType == (byte)CSCommon.eUpCheatsType.Normal)
            {
                cost = ctpl.goldcost;
                rate = ctpl.goldcostrate;
                costtype = eCurrenceType.Gold;
            }
            else if (costType == (byte)CSCommon.eUpCheatsType.Normal)
            {
                cost = ctpl.rmbcost;
                rate = ctpl.rmbcostrate;
                costtype = eCurrenceType.Rmb;
            }
            if (!_IsMoneyEnough(costtype, cost))
            {
                return (sbyte)eRet_UpSkillLv.LessMoney;
            }
            _ChangeMoney(costtype, eMoneyChangeType.UpSkill, -cost);
            float rand = (float)mRandom.NextDouble();
            if (rand > rate)
            {
                return (sbyte)eRet_UpSkillLv.Failure;
            }
            return (sbyte)eRet_UpSkillLv.Succeed;
        }

        public void InitSkillTime()
        {
            int ftime = (int)CSCommon.RoleCommon.Instance.Reset;//刷新时间
            if (ServerFrame.Time.InitTimes(ftime, PlayerData.RoleDetail.LastUpMuscleTime))
            {
                PlayerData.RoleDetail.UpMuscleTimes = 0;
                PlayerData.RoleDetail.FreeUpMuscleTimes = 0;
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_UpCheats(int skillId, byte costType, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var sk = SkillMgr.GetSkill(skillId);
            if (sk == null)
            {
                pkg.Write((sbyte)eRet_UpSkillLv.NotLearn);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var sktpl = CSTable.SkillUtil.GetSkillTpl(skillId);
            if (sktpl == null)
            {
                pkg.Write((sbyte)eRet_UpSkillLv.NoSkill);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            //升级
            if (sk.Level >= PlayerData.RoleDetail.RoleLevel)
            {
                pkg.Write((sbyte)eRet_UpSkillLv.LessRoleLv);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (sk.Level >= sk.Data.maxLv)
            {
                pkg.Write((sbyte)eRet_UpSkillLv.MaxLv);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            //下级模板
            var sklvtpl = CSTable.SkillUtil.GetSkillLevelTpl(skillId, sk.Level + 1);
            if (sklvtpl == null)
            {
                pkg.Write((sbyte)eRet_UpSkillLv.NoSkillLvTpl);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            sbyte result = UpCheats(sklvtpl, costType);
            if (result != (sbyte)eRet_UpSkillLv.Succeed)
            {
                pkg.Write(result);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            sk.Init(sklvtpl.id, (byte)sklvtpl.level);
            sk.SetOwner(this);
            pkg.Write((sbyte)eRet_UpSkillLv.Succeed);
            pkg.DoReturnPlanes2Client(fwd);
        }
    }
}