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
    public class NpcInit : IActorInitBase
    {
        public CSCommon.MapInfo_Npc Data;
        public TableWrap.MapInfoData OwnerMapData;
    }

    [RPC.RPCClassAttribute(typeof(NPCInstance))]
    public class NPCInstance : RoleActor, RPC.RPCObject
    {
        #region RPC必须的基础定义
        public static RPC.RPCClassInfo smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
        {
            return smRpcClassInfo;
        }
        #endregion    

        protected ulong mId;
        public override ulong Id
        {
            get { return mNpcData.RoleId; }
        }

        public override int RoleLevel
        {
            get { return InMapData.level; }
            set { }
        }

        public override string RoleName
        {
            get { return InMapData.name; }
        }

        public override int CurHP
        {
            set { mNpcData.RoleHp = value; }
            get { return mNpcData.RoleHp; }
        }

        public override int CurMP
        {
            set { }
            get { return 0; }
        }

        public override eCamp Camp
        {
            get { return (eCamp)mNpcData.Camp; }
        }

        public override eElemType ElemType
        {
            get { return (eElemType)mNpcData.Template.monsterAtt; }
        }

        public eNpcType Type
        {
            get { return (eNpcType)mNpcData.Template.type; }
        }

        public INPCLogic mLogic;
        public ulong mAttackTarget; //攻击目标，也是最大仇恨目标
        protected bool mIsCanAttack = true;
        protected int mCurrentSpellID; //当前技能
	    public float mCastSpellDistance; //攻击距离
	    protected long mSpellCD; //cd
        protected ushort mSpellType; //0 给玩家  1 给自己  2 给队友

        RoleValue mFinalRoleValue = new RoleValue();
        public override RoleValue FinalRoleValue { get { return mFinalRoleValue; } }    //最终属性

        public eNPCState mState;
        public uint mStateLastTime;
        public uint mScanLastTime; //搜索玩家时间
        public uint mDeadRefreshTime; //死亡刷新时间
        public uint mNextPatrolTime; //下次巡逻时间
        public bool mIsBattle;
        public bool mDelAfterDie; //是否死亡永久删除
        public Vector3 mSpawnPoint = Vector3.Zero; //出生点

        protected CSCommon.Data.NPCData mNpcData;
        public CSCommon.Data.NPCData NPCData { get { return mNpcData; } }
        public CSCommon.MapInfo_Npc InMapData { get; set; }
        public TableWrap.MapInfoData OwnerMapData { get; set; }
        public CSTable.NPC_AttriData AttriData { get; set; }
        public MoveNodes<int> mPathNodes = new MoveNodes<int>(); //镖车行走路线

        public static NPCInstance CreateNPCInstance(CSCommon.MapInfo_Npc nd, MapInstance map)
        {
            NPCInstance ret = new NPCInstance();
            ret.mId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.NPC);

            var init = new NpcInit();
            init.GameType = eActorGameType.Npc;
            init.Data = nd;
            init.OwnerMapData = map.MapInfo;
            if (!ret.Initialize(init))
                return null;

            ret.Reborn();
            ret.OnEnterMap(map);

            return ret;
        }

        public override bool Initialize(IActorInitBase initBase)
        {
            if (!base.Initialize(initBase))
                return false;

            if (!InitNpcData())
                return false;

            if (!InitTplData(initBase))
                return false;

            if (!InitMapData(initBase))
                return false;

            return true;
        }

        protected virtual bool InitNpcData()
        {
            mNpcData = new CSCommon.Data.NPCData();
            mNpcData._SetHostNpc(this);

            return true;
        }

        protected virtual bool InitMapData(IActorInitBase initBase)
        {
            var init = initBase as NpcInit;
            if (null == init) return false;

            InMapData = init.Data;
            OwnerMapData = init.OwnerMapData;
            mSpawnPoint = new Vector3(InMapData.posX, 0, InMapData.posZ);
            mNpcData.ActionId = InMapData.actionId;
            mNpcData.Camp = (byte)InMapData.camp;
            InMapData.level = InMapData.level > 0 ? InMapData.level : 1;
            if (InMapData.followRange < 0)
                InMapData.followRange = float.MaxValue;
            if (InMapData.followRange < InMapData.patrolRange)
                InMapData.patrolRange = InMapData.followRange;
            AttriData = CSTable.StaticDataManager.NPC_Attri[mNpcData.Template.type, InMapData.level];
            if (AttriData == null) return false;

            return true;
        }

        protected virtual bool InitTplData(IActorInitBase initBase)
        {
            var init = initBase as NpcInit;
            if (null == init) return false;

            mNpcData.TemplateId = init.Data.tid;
            if (null == NPCData.Template) return false;

            mNpcData.RoleName = NPCData.Template.name;
            mDeadBodyTime = mNpcData.Template.deadBodyTime;
            SkillMgr.Init(this, mNpcData);

            mCurrentSpellID = SkillMgr.getElementByIndex(0).ID;
            mCastSpellDistance = NPCData.Template.distance;
            mSpellType = (ushort)eSkillTargetType.Enemy;

            BaseGameLogic<INPCLogic> bgLogic = (BaseGameLogic<INPCLogic>)GameLogicManager.Instance.GetGameLogic(eGameLogicType.NPCLogic, (short)mNpcData.Template.logic);
            if (null != bgLogic)
                mLogic = bgLogic.Logic;

            mState = eNPCState.Idle;

            return true;
        }

        #region 数值计算
        void InitAttr()
        {
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Atk, AttriData.atk);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.MaxHP, AttriData.hp);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Def_Gold, AttriData.goldDef);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Def_Wood, AttriData.woodDef);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Def_Water, AttriData.waterDef);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Def_Fire, AttriData.fireDef);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Def_Earth, AttriData.soilDef);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Hit, AttriData.hitVal);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Dodge, AttriData.dodgeVal);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.MoveSpeed, (int)AttriData.speed);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Crit, AttriData.critVal);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.CritDef, AttriData.defCritVal);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.HPRecover, AttriData.hpRecovery);
        }
        #endregion

        public void Reborn()
        {
            //mId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.NPC);
            mNpcData.RoleId = mId;
            this.Placement.SetLocation(ref mSpawnPoint);
            this.Placement.SetDirection(InMapData.dir);

            mNpcData.Scale = InMapData.scale;
            InitAttr();
            CurHP = FinalRoleValue.MaxHP;
            mNpcData.RoleMaxHp = FinalRoleValue.MaxHP;
            mNpcData.Speed = FinalRoleValue.Speed;
        }

        protected override void OnPlacementUpdatePosition(ref SlimDX.Vector3 pos)
        {
            base.OnPlacementUpdatePosition(ref pos);
            mNpcData.Position = pos;
        }

        protected override void OnPlacementUpdateDirectionY(float dir)
        {
            base.OnPlacementUpdateDirectionY(dir);
            mNpcData.Direction = dir;
        }

        protected override void OnMaxEnmityObjChanged(ulong targetId)
        {
            mAttackTarget = targetId;
        }

        public void SetBattleStatus(bool status)
        {
            if (mIsBattle == status)
                return;

            mIsBattle = status;
            //广播战斗状态
        }

        public override void Tick(long elapsedMillisecond)
        {
            base.Tick(elapsedMillisecond);
            if (null == mNpcData) return;
            
            mStateLastTime += LogicTime;
            switch(mState)
	        {
	            case eNPCState.Idle:
			        OnIdle();
		            break;
	            case eNPCState.Patrol:
			        OnPatrol();
		            break;
	            case eNPCState.FollowTarget:
			        OnFollowTarget();
		            break;
	            case eNPCState.CastSpell:
			        OnCastSpell();
		            break;
	            case eNPCState.WaitCoolDown:
			        OnWaitCoolDown();
		            break;
	            case eNPCState.ReturnSpawnPoint:
			        OnReturnSpawnPoint();
                    break;
                case eNPCState.Transport:
                    OnTransport();
                    break;
                case eNPCState.WaitJumpMap:
                    OnWaitJumpMap();
                    break;
	            case eNPCState.FixedBody:
			        OnFixedBody();
			        break;
	            case eNPCState.Pause:
			        OnPause();
                    break;
                case eNPCState.Dead:
                    OnDead();
                    break;
                case eNPCState.None:
                    var arena = mHostMap as ArenaInstance;
                    if (arena != null && arena.IsReady)
                        ChangeState(eNPCState.Idle);
                    return;
                    //if (mStateLastTime < mDeadRefreshTime)
                    //    return;
                    //if (mDelAfterDie)
                    //    return;
                    //Revive();
                    //break;
	            default:
		            break;
	        }
            if(mState == eNPCState.Patrol || mState == eNPCState.FollowTarget || mState == eNPCState.ReturnSpawnPoint || mState == eNPCState.Transport)
	        {
		        base.MoveLogic(LogicTime);
	        }

            if (mScanLastTime > GameSet.Instance.ScanPlayerTime)
	        {
		        //主动怪
                if (mState == eNPCState.Idle || mState == eNPCState.Patrol)
                {
                    SelectTarget();
                }
	        }
	        else
	        {
		        mScanLastTime += LogicTime;
	        }
        }

        //怪物ai逻辑部分
	    public bool OnKilled()
        {
            if (null == mLogic)
                return false;
            return mLogic.OnKilled(this);
        }

	    public void Revive()
        {
            if (null == mLogic)
                return;
            mLogic.Revive(this);
        }

	    public void OnIdle()
        {
            if (null == mLogic)
                return;
            mLogic.OnIdle(this);
        }

	    public void OnPatrol() //巡逻
        {
            if (null == mLogic)
                return;
            mLogic.OnPatrol(this);
        }

	    public void StartPatrol() //巡逻
        {
            if (null == mLogic)
                return;
            mLogic.StartPatrol(this);
        }

	    public void OnFollowTarget() //追击
        {
            if (null == mLogic)
                return;
            mLogic.OnFollowTarget(this);
        }

	    public bool FollowTarget(RoleActor player) //追击
        {
            if (null == mLogic)
                return false;
            return mLogic.FollowTarget(this, player);
        }

        public override bool CanAttack(RoleActor target)
        {
            if (!base.CanAttack(target))
                return false;

            if (HostMap.CanAttack(this, target) == eMapAttackRule.AtkOK)
                return true;

            if (NPCData.Template.type == (byte)eNpcType.Npc)
                return false;

            if (target is NPCInstance)
            {
                var npc = target as NPCInstance;
                if (npc.NPCData.Template.type == (byte)eNpcType.Npc)
                    return false;

                if (target.Camp == eCamp.None)
                    return false;
            }
            else if (target is PlayerInstance)
            {
                if (target.Camp != this.Camp)
                    return true;

                if (this.Camp == eCamp.None)
                    return true;
            }

            if (target.Camp == this.Camp)
                return false;


            return true;
        }

        /// <summary>
        /// 目标是否可以被攻击
        /// </summary>
        public override bool CanBeAttacked()
        {
            if (!base.CanBeAttacked())
                return false;

            if (NPCData.Template.type == (byte)eNpcType.Npc)
                return false;

            return true;
        }

        public virtual void OnCastSpell()
        {
            if (!mIsCanAttack) return;
            if (IsMoving) return;

            //RoleActor target = GetTarget(mAttackTarget);
            //Spell(mCurrentSpellID, target);
            eSkillResult ret = eSkillResult.OK;
            if (CastSpell(mCurrentSpellID, mAttackTarget, ref ret))
            {
                mSpellCD = mNowTime + (long)NPCData.Template.interval * 1000;
                ChangeState(eNPCState.WaitCoolDown);
            }
        }

        public virtual void OnWaitCoolDown()
        {
            if (mSpellType == (ushort)eSkillTargetType.Enemy)
	        {
		        RoleActor target = GetTarget(mAttackTarget);
		        if (null == target || !target.CanBeAttacked())
		        {
                    if (!SelectTarget())
                    {
                        mAttackTarget = 0;
                        ReturnSpawnPoint();
                        return;
                    }
                    target = GetTarget(mAttackTarget);
		        }

		        //够攻击距离
		        if (CanCastSpell())
		        {
                    if (mNowTime > mSpellCD)
                    {
                        ChangeState(eNPCState.CastSpell);
                    }
		        }
		        else if (!FollowTarget(target))
		        {
			        ReturnSpawnPoint();
		        }
	        }
	        else
	        {
                if (mNowTime > mSpellCD)
                {
                    ChangeState(eNPCState.CastSpell);
                }
	        }
        }

        public void OnReturnSpawnPoint()
        {
            if (IsMoving)
		        return;

            //清除buff
            BuffMgr.ClearUp();

            //yzb  根据不同怪的类型，回血速度
            if ( Type == CSCommon.eNpcType.Point)
            {
            }
            else
            {
                //回复生命
                CurHP = FinalRoleValue.MaxHP;
            }
                    
	        if (!SelectTarget())
	        {
		        ChangeState(eNPCState.Idle);
	        }
        }

        public bool OnTransport()
        {
            if (null == mLogic)
                return false;

            return mLogic.OnTransport(this);
        }

        public bool OnWaitJumpMap()
        {
            if (null == mLogic)
                return false;

            return mLogic.OnWaitJumpMap(this);
        }

	    public bool SelectTarget() //选中攻击目标
        {
            if (null == mLogic)
                return false;

            return mLogic.SelectTarget(this);
        }

        public override void OnDie() //死亡
        {
            base.OnDie();

            if (null == mLogic)
            {
                ChangeState(eNPCState.Dead);
                return;
            }

            mLogic.OnKilled(this);
        }

        public virtual void OnDead() //躺尸
        {
            if (!IsDie) return;

            if (mStateLastTime >= mNpcData.Template.deadBodyTime * 1000)
            {
                mState = eNPCState.None;
                if (InMapData.rebornTime == 0)
                    OnTimeReborn(new TimerEvent(HostMap));
                else if (InMapData.rebornTime > 0)
                    TimerManager.doOnce(InMapData.rebornTime, OnTimeReborn, HostMap);
                OnLeaveMap();
            }
        }

	    public void OnFixedBody() //定身
        {
            if (null == mLogic)
                return;
            mLogic.OnFixedBody(this);
        }

	    public bool OnPause() //暂停
        {
            if (null == mLogic)
                return false;
            return mLogic.OnPause(this);
        }

        public void ChangeState(eNPCState state)
        {
            if (IsDie && mState != eNPCState.None && state != eNPCState.Dead)
		        return;

	        if (mState == state)
		        return;

	        if (state == eNPCState.ReturnSpawnPoint)
	        {
		        mAttackTarget = 0;
		        PlayerStatus.AddStatusType((int)eBuffStatusType.无敌); //开启无敌模式
	        }
	        else if (mState == eNPCState.ReturnSpawnPoint)
	        {
                PlayerStatus.DelStatusType((int)eBuffStatusType.无敌); //关闭无敌模式
	        }
	        mState = state;
	        mStateLastTime = 0;

	        switch(mState)
	        {
	            case eNPCState.Idle:
	            case eNPCState.Patrol:
	            case eNPCState.ReturnSpawnPoint:
			        SetBattleStatus(false);
			        break;
	            case eNPCState.FollowTarget:
	            case eNPCState.CastSpell:
	            case eNPCState.WaitCoolDown:
	            case eNPCState.Dead:
	            case eNPCState.FixedBody:
			        SetBattleStatus(true);
			        break;
	        }

	        //不是巡逻，追击，回出生点，移动停下来
	        if ( mState != eNPCState.Patrol && mState != eNPCState.FollowTarget && mState != eNPCState.ReturnSpawnPoint && mState != eNPCState.Transport && mState != eNPCState.Pause )
	        {
		        CleanMoveNodes();
	        }
        }

        public void ReturnSpawnPoint()
        {
	        if (MoveTo(mSpawnPoint))
	        {
		        ChangeState(eNPCState.ReturnSpawnPoint);
	        }
        }

        public bool MoveTo(Vector3 target)
        {
	        if ( Util.DistanceH(target, GetPosition()) < GameSet.Instance.PosSyncDistRange )
	        {
		        return true;
	        }
	        return base.MoveTo(target);
        }

        void OnTimeReborn(TimerEvent evt)
        {
            var map = evt.param as MapInstance;
            if (map == null || map.Planes == null)
                return;
            
            NPCInstance.CreateNPCInstance(InMapData, map);
        }

        public override void OnEnterMap(MapInstance map)
        {
            base.OnEnterMap(map);
            map.AddNPC(this);
        }

        public override void OnLeaveMap()
        {
            if (!HostMap.IsNullMap)
            {
                HostMap.RemoveNPC(this);
            }
            base.OnLeaveMap();
        }

        public bool CanCastSpell()
        {
	        RoleActor target = GetTarget(mAttackTarget);
	        if (target == null)
		        return false;

	        if (Util.DistanceH(target.GetPosition(), GetPosition()) > mCastSpellDistance)
		        return false;

	        return true;
        }

        public virtual bool SelectSpell()
        {
            return true;
        }

        public RoleActor FindHatredTarget()
        {
            if (InMapData.guardRange > 0)
            {
                List<RoleActor> roleList = new List<RoleActor>();
                UInt32 actorTypes = (1 << (Int32)eActorGameType.Player) | (1 << (Int32)eActorGameType.Npc) | (1 << (Int32)eActorGameType.PlayerImage);
                var pos = GetPosition();
                if (!HostMap.TourRoles(ref pos, InMapData.guardRange, actorTypes, roleList))
                    return null;

                foreach (var tar in roleList)
                {
                    if (CanAttack(tar))
                        return tar;
                }
            }
            return null;
        }

    }
}
