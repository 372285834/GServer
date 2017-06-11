using CSCommon;
using CSCommon.Data;
using ServerFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace ServerCommon.Planes
{
    [RPC.RPCClassAttribute(typeof(PlayerImage))]
    public class PlayerImage : NPCInstance
    {
        public ulong RoleId
        {
            get { return RoleData.RoleId; }
        }

        public override int RoleLevel
        {
            get { return RoleData.Level; }
        }

        public override string RoleName
        {
            get { return RoleData.Name; }
        }

        RoleCom mRoleData;
        public RoleCom RoleData { get { return mRoleData; } }

        OffPlayerData mPlayerData;
        public OffPlayerData PlayerData { get { return mPlayerData; } }

        public static void CreatePlayerImage(CSCommon.MapInfo_Npc nd, MapInstance map, ulong playerId)
        {
            PlayerImage ret = new PlayerImage();
            ret.mId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.PlayerImage);

            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_GetOffPlayerData(pkg, playerId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                CSCommon.Data.RoleCom rc = new CSCommon.Data.RoleCom();
                _io.Read(rc);
                OffPlayerData pd = new OffPlayerData();
                pd.Deserizle(rc);

                ret.InitPlayerData(pd, rc);

                var init = new NpcInit();
                init.GameType = eActorGameType.PlayerImage;
                init.Data = nd;
                init.OwnerMapData = map.MapInfo;
                if (!ret.Initialize(init))
                    return;

                ret.mAttackTarget = playerId;
                ret.Reborn();
                ret.OnEnterMap(map);
            };
        }

        protected override bool InitTplData(IActorInitBase initBase)
        {
            var init = initBase as NpcInit;
            if (null == init) return false;

            mNpcData.TemplateId = init.Data.tid;
            BaseGameLogic<INPCLogic> bgLogic = (BaseGameLogic<INPCLogic>)GameLogicManager.Instance.GetGameLogic(eGameLogicType.NPCLogic, (short)eNPCLogicType.PlayerImage);
            if (null == bgLogic) return false;

            mLogic = bgLogic.Logic;
            mNpcData.RoleMaxHp = mPlayerData.value.MaxHP;
            mState = eNPCState.None;

            return true;
        }

        protected bool InitPlayerData(OffPlayerData pd, RoleCom rc)
        {
            mPlayerData = pd;
            mRoleData = rc;
            SkillMgr.InitBag(this, pd.skills);

            return true;
        }

        //public override void CalcInitValue()
        //{
        //    mInitRoleValue.MaxHP = mPlayerData.value.MaxHP;
        //    mInitRoleValue.Atk = mPlayerData.value.Atk;
        //    mInitRoleValue.Def = mPlayerData.value.Def;
        //    mInitRoleValue.Hit = mPlayerData.value.Hit;
        //    mInitRoleValue.Dodge = mPlayerData.value.Dodge;
        //    mInitRoleValue.Crit = mPlayerData.value.Crit;
        //    mInitRoleValue.CritDef = mPlayerData.value.CritDef;
        //    mInitRoleValue.HPRecover = mPlayerData.value.HPRecover;
        //}

        int mCurrSpellNode; //当前技能节点
        List<int> mSpellSNodes = new List<int> { 3, 2, 1, 4, 3, 2, 1, 3, 2, 1 }; //技能释放顺序表
        public override bool SelectSpell()
        {
            if (mCurrSpellNode >= mSpellSNodes.Count())
                mCurrSpellNode = 0;

            int spellId = SkillMgr.getElementByIndex(mSpellSNodes[mCurrSpellNode] - 1).ID;
            ASkillObject skbase = mSkillMgr.GetSkill(spellId);
            if (skbase == null) return false;

            var sk = skbase as SkillActive;
            if (sk == null) return false;

            if (!sk.SkillCD.IsCoolDown() || !SkillCD.IsCoolDown()) return false;

            mCurrentSpellID = spellId;
            CSTable.SkillActiveData skillTplData = sk.Data as CSTable.SkillActiveData;
            mCastSpellDistance = skillTplData.range;

            return true;
        }

        public override void OnCastSpell()
        {
            if (!mIsCanAttack) return;
            if (IsMoving) return;

            RoleActor target = GetTarget(mAttackTarget);
            if (null == target || !target.CanBeAttacked())
            {
                ChangeState(eNPCState.Idle);
                return;
            }
            //if (Spell(mCurrentSpellID, target) == eSkillResult.OK)
            eSkillResult ret = eSkillResult.OK;
            if (CastSpell(mCurrentSpellID, mAttackTarget, ref ret))
            {
                mCurrSpellNode++;
                ChangeState(eNPCState.WaitCoolDown);
            }
        }

        public override void OnWaitCoolDown()
        {
            if (!SelectSpell()) return;

            RoleActor target = GetTarget(mAttackTarget);
            if (target == null) return;

            //够攻击距离
            if (CanCastSpell())
            {
                ChangeState(eNPCState.CastSpell);
            }
            else if (!FollowTarget(target))
            {
                ChangeState(eNPCState.Idle);
            }
        }

        public override void OnDead() //躺尸
        {
            if (!IsDie) return;

            if (mStateLastTime >= 5000)
            {
                mState = eNPCState.None;
                OnLeaveMap();
            }
        }

    }
}
