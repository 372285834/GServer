using CSCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using ServerFrame;
using CSCommon.Data;

namespace ServerCommon.Planes
{
    public partial class RoleActor
    {
        eRoleFightState mFightState = eRoleFightState.Normal;
        public eRoleFightState FightState
        {
            get { return mFightState; }
            set
            {
                mFightState = value;
            }
        }

        protected SkillManager mSkillMgr = new SkillManager();
        public SkillManager SkillMgr { get { return mSkillMgr; } }

        protected BuffManager mBuffMgr = new BuffManager();
        public BuffManager BuffMgr { get { return mBuffMgr; } }

        SkillCD mSkillCD = new SkillCD();
        public SkillCD SkillCD
        {
            get { return mSkillCD; }
        }

        PlayerStatus mPlayerStatus = new PlayerStatus();
        public PlayerStatus PlayerStatus
        {
            get { return mPlayerStatus; }
        }

        SkillActive mCurrSkill;
        public SkillActive CurrSkill //当前释放技能
        {
            get { return mCurrSkill; }
        }

        Impact mCurrImpact;
        public Impact CurrImpact //当前收到的技能影响
        {
            get { return mCurrImpact; }
        }

        #region 仇恨列表
        public struct EnmityInfo
        {
            public ulong playerID;
            public int mEnmityValue; //仇恨值
            public uint mUpdateEnmityTime; //更新仇恨值时间
            public EnmityInfo(ulong id, int enmity, uint time)
            {
                playerID = id;
                mEnmityValue = enmity;
                mUpdateEnmityTime = time;
            }
        };

        List<EnmityInfo> mEnmityList = new List<EnmityInfo>(); //仇恨列表
        ulong mMaxEnmityObjID; //最大仇恨目标
        int mMaxEnmityValue; //最大仇恨值

        /// <summary>
        /// 受攻击时更新仇恨列表
        /// </summary>
        public void UpdateEnmityList(ulong ObjID, int value)
        {
            if(value < 0)
		        return;

	        if ( !IsInEnmityList(ObjID) )
	        {
		        mEnmityList.Add( new EnmityInfo( ObjID, value, IServer.timeGetTime() ) );
		        if (value > mMaxEnmityValue)
		        {
			        mMaxEnmityObjID = ObjID;
			        mMaxEnmityValue = value;
		        }
	        }
	        else
	        {
                for (var i = 0; i < mEnmityList.Count(); i++)
                {
                    EnmityInfo ite = mEnmityList[i];
                    if (ite.playerID == ObjID)
                    {
                        ite.mEnmityValue += value;
                        ite.mUpdateEnmityTime = IServer.timeGetTime();
                        if (ite.mEnmityValue > mMaxEnmityValue)
                        {
                            mMaxEnmityObjID = ObjID;
                            mMaxEnmityValue = ite.mEnmityValue;
                        }
                        break;
                    }
                }
	        }

            OnMaxEnmityObjChanged(mMaxEnmityObjID);
        }

        /// <summary>
        /// 仇恨目标第一位发生变化
        /// </summary>
        protected virtual void OnMaxEnmityObjChanged(ulong targetId)
        {

        }

        /// <summary>
        /// //清仇恨列表
        /// </summary>
        public void ClearEnmityList()
        {
            mEnmityList.Clear();
            mMaxEnmityObjID = 0;
            mMaxEnmityValue = -1;
        }

        /// <summary>
        /// //从仇恨列表里清除一个
        /// </summary>
        public void KickOneFromEnmityList(ulong ObjID)
        {
            if ( mEnmityList.Count() == 0 )
		        return;

            for (var i = 0; i < mEnmityList.Count(); i++)
            {
                EnmityInfo ite = mEnmityList[i];
                if (ite.playerID == ObjID)
                {
                    mEnmityList.RemoveAt(i);
                    break;
                }
            }

	        if (mMaxEnmityObjID == ObjID) //如果把仇恨列表里第一的删了，通知属主换个目标
	        {
                mMaxEnmityObjID = 0;
                mMaxEnmityValue = 0;
		        for (var i = 0; i < mEnmityList.Count(); i++)
		        {
			        EnmityInfo ite = mEnmityList[i];
                    if (ite.mEnmityValue > mMaxEnmityValue)
                    {
                        mMaxEnmityObjID = ite.playerID;
                        mMaxEnmityValue = ite.mEnmityValue;
                    }
		        }
		        OnMaxEnmityObjChanged(mMaxEnmityObjID);
	        }
        }

        /// <summary>
        /// 是否在仇恨列表中
        /// </summary>
        public bool IsInEnmityList(ulong playerId)
        {
            for (var i = 0; i < mEnmityList.Count(); i++)
            {
                EnmityInfo ite = mEnmityList[i];
                if (ite.playerID == playerId)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 得到仇恨列表里仇恨值最大的可攻击对象
        /// </summary>
        public ulong GetFirstEnmityCanHit()
        {
            RoleActor target = GetTarget(mMaxEnmityObjID);
            if (null != target && target.CanBeAttacked())
                return mMaxEnmityObjID;

            int tmp = 0;
            target = null;
            for (var i = 0; i < mEnmityList.Count(); i++)
            {
                EnmityInfo ite = mEnmityList[i];
                if (ite.mEnmityValue > tmp)
                {
                    RoleActor r = GetTarget(ite.playerID);
                    if (null != r && r.CanBeAttacked())
                        target = r;
                }
            }
            if (null == target)
                return 0;

            return target.Id;
        }
        #endregion

        /// <summary>
        /// 使用技能
        /// </summary>
        public bool CastSpell(int skillId, ulong targetId, ref eSkillResult result)
        {
            ASkillObject skbase = mSkillMgr.GetSkill(skillId);
            if (null == skbase)
                return false;

            var skill = skbase as SkillActive;
            if (null == skill)
                return false;

            bool isCastSuccess = CastSpell(skill, targetId);
            result = skill.mResult;
            return isCastSuccess;
        }

        public bool CastSpell(SkillActive skill, ulong targetId)
        {
            if (null == skill || null == skill.Data)
                return false;

            skill.mResult = eSkillResult.CastFailed;

            if (mPlayerStatus.IsHasBuffStatus(eBuffStatusType.眩晕))
                return false;

            if (skill.mStatus != eSkillStatus.Valid)
                return false;

            if (skill.mStep != eSkillStep.Init)
                return false;

            //检查条件
            RoleActor target = mHostMap.GetRole(targetId);
            if (null != skill.Checker)
            {
                skill.mStep = eSkillStep.Check;
                //预选择目标检查
                if (!skill.Checker.PreSelectTarget(this, skill, target))
                {
                    skill.Reset();
                    return false;
                }
                if (null != target)
                {
                    skill.mDistance = Util.DistanceH(target.GetPosition(), GetPosition());
                }
                if (!skill.Checker.CheckSkillCondition(this, skill))
                {
                    skill.Reset();
                    return false;
                }
            }

            //检查消耗
            if (null != skill.Consumer)
            {
                skill.mStep = eSkillStep.Consume;
                if (!skill.Consumer.CheckSkillConsume(this, skill))
                {
                    skill.Reset();
                    return false;
                }
                //实际消耗
                //if (!skill.Consumer.SkillConsume(this, skill))
                //{
                //    skill.Reset();
                //    return false;
                //}
            }

            if (skill.SkillData.cdTime > 0)
            {
                //技能CD
                float cdTime = skill.SkillData.cdTime;
                cdTime -= GameSet.Instance.SkillCDTimeSync;
                skill.SkillCD.AddCoolDown(cdTime, Time.time + cdTime);
                float protectTime = new float[]{skill.SkillData.protectTime, CSCommon.SCDefine.cCommonCD}.Max();
                protectTime -= GameSet.Instance.SkillCDTimeSync;
                SkillCD.AddCoolDown(protectTime, Time.time + protectTime);
            }

            //广播技能效果
            var pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, Id).RPC_SpellSkill(pkg, skill.ID, skill.mTargetId);
            HostMap.SendPkg2Clients(null, this.GetPosition(), pkg);

            mCurrSkill = skill;

            //实际释放技能
            return DoCastSkill(skill);
        }

        /// <summary>
        /// 取消释放技能
        /// </summary>
        public bool CancelSpell()
        {
            if (null == mCurrSkill)
                return false;

            mCurrSkill.Reset();

            return true;
        }

        /// <summary>
        /// 实际释放技能
        /// </summary>
        private bool DoCastSkill(SkillActive skill)
        {
            //是否分段计算效果？
            float hitTime = skill.mHitIndex >= 0 && skill.LvData.hitTime.Length > 0 ? skill.LvData.hitTime[skill.mHitIndex] : 0;
            skill.mHitIndex++;
            if (hitTime > 0)
            {
                DelayDoSkillEffect(skill, hitTime);
            }
            else
            {
                DoSkillEffect(skill);
            }

            skill.mResult = eSkillResult.OK;

            return true;
        }

        /// <summary>
        /// 发送技能分段计算延迟事件
        /// </summary>
        private void DelayDoSkillEffect(SkillActive skill, float hitTime)
        {
            skill.mStep = eSkillStep.Delay;
            TimerManager.doOnce(hitTime, OnSkillEffectEvent, skill);
        }

        private void OnSkillEffectEvent(TimerEvent timerEvent)
        {
            SkillActive skill = timerEvent.param as SkillActive;
            DoSkillEffect(skill);
            if (skill.mHitIndex < skill.LvData.hitTime.Length && skill.mStep != eSkillStep.Init)
            {
                DoCastSkill(skill);
            }
        }

        /// <summary>
        /// 实施技能效果
        /// </summary>
        private void DoSkillEffect(SkillActive skill)
        {
            //是否有飞行时长投射?
            float flyTime = 0f;
            float speed = skill.SkillData.throwEffectSpeed;

            if (speed > 0)
            {
                flyTime = skill.mDistance / speed;
            }

            //按实际飞行时长计算
            if (flyTime > 0)
            {
                DelayDoSkillRealEffect(skill, flyTime);
            }
            else
            {
                DoSkillRealEffect(skill);
                if (skill.mHitIndex >= skill.LvData.hitTime.Length)
                {
                    skill.Reset();
                }
            }
        }

        /// <summary>
        /// 发送技能投射物飞行延迟事件
        /// </summary>
        private void DelayDoSkillRealEffect(SkillActive skill, float flyTime)
        {
            TimerManager.doOnce(flyTime, OnSkillRealEffectEvent, skill);
        }

        private void OnSkillRealEffectEvent(TimerEvent timerEvent)
        {
            SkillActive skill = timerEvent.param as SkillActive;
            DoSkillRealEffect(skill);
            if (skill.mHitIndex >= skill.LvData.hitTime.Length)
            {
                skill.Reset();
            }
        }

        /// <summary>
        /// 实施技能真实效果
        /// </summary>
        private bool DoSkillRealEffect(SkillActive skill)
        {
            if (null == skill.Selector)
                return false;

            if (!CanAttack())
                return false;

            skill.mStep = eSkillStep.Target;

            List<RoleActor> targets = new List<RoleActor>();
            skill.Selector.SelectTarget(this, skill, ref targets);

            if (targets.Count() == 0)
                return false;

            foreach (var tar in targets)
            {
                _DoSkillRealEffect(skill, tar);
            }

            return true;
        }

        private bool _DoSkillRealEffect(SkillActive skill, RoleActor target)
        {
            if (skill.Selector.TargetFilter(this, skill, target, eRelationType.Enemy))
            {
                if (!CanAttack(target))
                    return false;

                if (skill.ActionGroupData.limit >= skill.LvData.areaCount)
                    return false;

                ActionGroup tempGroup = skill.ActionGroupData;
                tempGroup.limit += 1;
                skill.ActionGroupData = tempGroup;
                bool isEffect = true; //是否命中
                foreach (var action in skill.ActionGroupData.actions)
                {
                    skill.mAction = action;
                    if (action.actionLogic.OnInit(target, skill))
                    {
                        isEffect = action.actionLogic.Cast(target, skill);
                        action.actionLogic.OnEnd(target, skill);
                    }
                    skill.mAction = default(Action);
                    if (!isEffect)
                        return false;
                }
            }

            return true;
        }

        public void SendFlutterInfo(int type, int param1, int param2, int value, RoleActor _caster)
        {
            //只把技能结果数据同步给发技能的玩家，或者是收到技能攻击的玩家
            var target = this as PlayerInstance;
            var caster = _caster as PlayerInstance;
            var pkg = new RPC.PackageWriter();
            if (null != target)
            {
                Wuxia.H_RpcRoot.smInstance.HIndex(pkg, this.Id).RPC_FlutterInfo(pkg, (byte)type, param1, param2, value);
                pkg.DoCommandPlanes2Client(target.Planes2GateConnect, target.ClientLinkId);
            }
            
            if (caster != null && caster != target)
            {
                pkg = new RPC.PackageWriter();
                Wuxia.H_RpcRoot.smInstance.HIndex(pkg, this.Id).RPC_FlutterInfo(pkg, (byte)type, param1, param2, value);
                pkg.DoCommandPlanes2Client(caster.Planes2GateConnect, caster.ClientLinkId);
            }
        }

        public virtual bool OnAttack()
        {
            return true;
        }

        public virtual bool OnImpactBegin(Impact imp)
        {
            mCurrImpact = imp;
            return true;
        }

        public virtual bool OnImpactEnd()
        {
            return true;
        }

        public bool OnImpacted(bool isBroadCast)
        {
            if (null == mCurrImpact)
                return false;
            if (null == mCurrImpact.Owner)
                return false;
            if (null == mCurrImpact.Skill)
                return false;

            ChangeHP(mCurrImpact.DamageHP, mCurrImpact.Owner);
            if (mCurrImpact.DamageHP < 0)
            {
                if (IsDie)
                    mCurrImpact.mKilled = true;
                if (mCurrImpact.HitType == eHitType.Crit)
                {
                    //击退
                }
            }

            ChangeMP(mCurrImpact.DamageMP);

            if (mCurrImpact.DamageHP == 0 && (mCurrImpact.HitType == eHitType.Hit || mCurrImpact.HitType == eHitType.Crit))
            {

            }
            else
            {
                //发送飘字效果
                SendFlutterInfo((int)mCurrImpact.HitType, mCurrImpact.Skill.ID, mCurrImpact.Skill.Level, -1 * mCurrImpact.DamageHP, mCurrImpact.Owner);
            }

            if (isBroadCast)
            {
                //广播结果
            }

            mCurrImpact = null;

            return true;
        }

        /// <summary>
        /// 是否免疫DEBUFF
        /// </summary>
        public virtual bool IsImmunityDebuff()
        {
            return false;
        }

        /// <summary>
        /// 能否攻击
        /// </summary>
        public bool CanAttack()
        {
            if (IsDie)
                return false;

            if (mFightState == eRoleFightState.PKProtect)
                return false;

            return true;
        }

        /// <summary>
        /// 目标是否可以被攻击
        /// </summary>
        public virtual bool CanBeAttacked()
        {
            if (IsDie)
                return false;

            if (mFightState == eRoleFightState.PKProtect)
                return false;

            if (PlayerStatus.IsHasBuffStatus(eBuffStatusType.无敌))
                return false;

            return true;
        }

        //监测阵营关系导致的是否可以攻击
        public virtual bool CanAttack(RoleActor target)
        {
            if (!CanAttack())
                return false;

            if (HostMap.CanAttack(this, target) == eMapAttackRule.AtkNO)
                return false;

            return target.CanBeAttacked();
        }

        public virtual void OnDie()
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            //Wuxia.H_RpcRoot.smInstance.HIndex(pkg, Id).RPC_ChangeState(pkg, (byte)eFsmState.Die, mDeadBodyTime);
            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, Id).RPC_Killed(pkg, CurrImpact.Skill.ID, CurrImpact.Skill.Level, CurrImpact.Owner.Id);
            HostMap.SendPkg2Clients(null, this.GetPosition(), pkg);  
        }

        public bool IsDie { get { return CurHP <= 0; } }

        public virtual void OnKillActor(RoleActor actor)
        {
            //RPC.PackageWriter pkg = new RPC.PackageWriter();
            //Wuxia.H_RpcRoot.smInstance.HIndex(pkg, actor.Id).RPC_Killed(pkg, CurrSkill.ID, Id);
            //HostMap.SendPkg2Clients(null, actor.GetPosition(), pkg);  
        }

        #region 角色自动站位，尽量均匀分布在攻击目标周围
        const int MaxModSurround = 7; //围点的最大距离
        const int SurroundEdge = 5; //每个方向点数
        const int MaxDir = 8; //方向数
        ulong[,] mModPositionIndex = new ulong[MaxDir, SurroundEdge];
        public struct ModDirPosition
        {
            public int dir;
            public Vector2[] point;
            public ModDirPosition(int _dir, Vector2 _a, Vector2 _b, Vector2 _c, Vector2 _d, Vector2 _e)
            {
                point = new Vector2[SurroundEdge];
                dir = _dir;
                point[0] = _a;
                point[1] = _b;
                point[2] = _c;
                point[3] = _d;
                point[4] = _e;
            }
        };

        //public ModDirPosition[] ModDirToPos = new ModDirPosition[MaxDir]
        //{
        //    new ModDirPosition(1,new Vector2(-2,7),new Vector2(-1,7),new Vector2(0,7),new Vector2(1,7),new Vector2(2,7)),
        //    new ModDirPosition(2,new Vector2(-3,6),new Vector2(-4,6),new Vector2(-5,5),new Vector2(-6,4),new Vector2(-6,3)),
        //    new ModDirPosition(3,new Vector2(-7,2),new Vector2(-7,1),new Vector2(-7,0),new Vector2(-7,-1),new Vector2(-7,-2)),
        //    new ModDirPosition(4,new Vector2(-6,-3),new Vector2(-6,-4),new Vector2(-5,-5),new Vector2(-4,-6),new Vector2(-3,-6)),
        //    new ModDirPosition(5,new Vector2(-2,-7),new Vector2(-1,-7),new Vector2(0,-7),new Vector2(1,-7),new Vector2(2,-7)),
        //    new ModDirPosition(6,new Vector2(3,-6),new Vector2(4,-6),new Vector2(5,-5),new Vector2(6,-4),new Vector2(6,-3)),
        //    new ModDirPosition(7,new Vector2(7,-2),new Vector2(7,-1),new Vector2(7,0),new Vector2(7,1),new Vector2(7,2)),
        //    new ModDirPosition(8,new Vector2(6,3),new Vector2(6,4),new Vector2(5,5),new Vector2(4,6),new Vector2(3,6))
        //};
        //public int[,] ModSurroundDir = new int[,]
        //{
        //    {1,2,8,3,7,4,6,5},
        //    {2,1,3,8,4,7,5,6},
        //    {3,2,4,1,5,8,6,7},
        //    {4,3,5,2,6,1,7,8},
        //    {5,6,4,7,3,8,2,1},
        //    {6,5,7,4,8,3,1,2},
        //    {7,6,8,5,1,4,2,3},
        //    {8,7,1,6,2,5,3,4}
        //};
        public ModDirPosition[] ModDirToPos = new ModDirPosition[MaxDir]
        {
	        new ModDirPosition(1,new Vector2(0,10),new Vector2(1,9),new Vector2(-1,9),new Vector2(3,9),new Vector2(-3,9)),
	        new ModDirPosition(2,new Vector2(7,7),new Vector2(8,5),new Vector2(5,8),new Vector2(8,4),new Vector2(4,8)),
	        new ModDirPosition(3,new Vector2(10,0),new Vector2(9,-1),new Vector2(9,1),new Vector2(9,-3),new Vector2(9,3)),
	        new ModDirPosition(4,new Vector2(7,-7),new Vector2(5,-8),new Vector2(8,-5),new Vector2(4,-8),new Vector2(8,-4)),
	        new ModDirPosition(5,new Vector2(0,-10),new Vector2(-1,-9),new Vector2(1,-9),new Vector2(-3,-9),new Vector2(3,-9)),
	        new ModDirPosition(6,new Vector2(-7,-7),new Vector2(-8,-5),new Vector2(-5,-8),new Vector2(-8,-4),new Vector2(-4,-8)),
	        new ModDirPosition(7,new Vector2(-10,0),new Vector2(-9,1),new Vector2(-9,-1),new Vector2(-9,3),new Vector2(-9,-3)),
	        new ModDirPosition(8,new Vector2(-7,7),new Vector2(-5,8),new Vector2(-8,5),new Vector2(-4,8),new Vector2(-8,4))
        };
        public int[,] ModSurroundDir = new int[,]
        {
            {1,3,7,5,2,8,4,6},
            {2,4,8,6,1,3,5,7},
            {3,1,5,7,2,4,6,8},
            {4,2,6,8,3,5,7,1},
            {5,3,7,1,4,6,8,2},
            {6,4,8,2,5,7,1,3},
            {7,5,1,3,6,8,2,4},
            {8,6,2,4,7,1,3,5}
        };

        public void ResetModPositionIndex()
        {
            for (int i = 0; i < MaxDir; i++)
            {
                for (int j = 0; j < SurroundEdge; j++)
                {
                    mModPositionIndex[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// 分散怪物站立位置，不要挤在一个地方
        /// </summary>
        public Vector3 GetSurroundPosition(Vector3 modpoint, float atkRange, ulong id)
        {
            Vector3 point = Vector3.Zero;
            atkRange *= 0.8f;
            float angle = Util.MyAngle(modpoint, GetPosition());
            int dir = Util.AngleToDir(angle);

            if (SearchPosition(ref point, dir, atkRange, id))
                return point;
            ResetModPositionIndex();
            SearchPosition(ref point, dir, atkRange, id);
            return point;
        }

        private bool SearchPosition(ref Vector3 point, int dir, float atkRange, ulong id)
        {
            int currdir = 0;
            for (int i = 0; i < SurroundEdge; i++)
            {
                for (int j = 0; j < MaxDir; j++)
                {
                    currdir = ModSurroundDir[dir-1, j] - 1;
                    if (mModPositionIndex[currdir, i] == 0 || mModPositionIndex[currdir, i] == id)
                    {
                        point.X = GetPosition().X + (float)(ModDirToPos[currdir].point[i].X / 10 * atkRange);
                        point.Z = GetPosition().Z + (float)(ModDirToPos[currdir].point[i].Y / 10 * atkRange);
                        mModPositionIndex[currdir, i] = id;
                        return true;
                    }
                }
            }
            point = Vector3.Zero;
            return false;
        }

        #endregion


        public void ClearUp()
        {
            FinalRoleValue.ClearUp();
            CleanMoveNodes();
            ClearEnmityList();
            PlayerStatus.CleanUp();
        }
    }
}
