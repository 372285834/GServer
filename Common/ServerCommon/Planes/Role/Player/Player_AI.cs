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
    public partial class PlayerInstance : RPC.RPCObject
    {
        bool mIsHook; //是否挂机
        public IPlayerLogic mLogic;
        public RoleActor mAttackTarget; //攻击目标

        public ePlayerState mState;
        public uint mStateLastTime;
        public uint mScanLastTime; //搜索玩家时间
        protected int mCurrentSpellID; //当前技能
        public float mCastSpellDistance; //攻击距离

        public bool InitState()
        {
            BaseGameLogic<IPlayerLogic> bgLogic = (BaseGameLogic<IPlayerLogic>)GameLogicManager.Instance.GetGameLogic(eGameLogicType.PlayerLogic, (short)ePlayerLogicType.Robot);
            if (null == bgLogic) return false;

            mLogic = bgLogic.Logic;

            if (IsDie)
                mState = ePlayerState.None;
            else
                mState = ePlayerState.Idle;

            return true;
        }

        public void StateTick(long elapsedMillisecond)
        {
            if (!mIsHook) return;

            mStateLastTime += LogicTime;
            switch (mState)
            {
                case ePlayerState.Idle:
                    OnIdle();
                    break;
                case ePlayerState.Quest:
                    OnQuest();
                    break;
                case ePlayerState.FollowTarget:
                    OnFollowTarget();
                    break;
                case ePlayerState.CastSpell:
                    OnCastSpell();
                    break;
                case ePlayerState.WaitCoolDown:
                    OnWaitCoolDown();
                    break;
                case ePlayerState.FixedBody:
                    OnFixedBody();
                    break;
                case ePlayerState.Pause:
                    OnPause();
                    break;
                case ePlayerState.Dead:
                    OnDead();
                    break;
                case ePlayerState.None:
                    Relive(eReliveMode.Current);
                    ChangeState(ePlayerState.Idle);
                    return;
                default:
                    break;
            }
            if (mState == ePlayerState.Quest || mState == ePlayerState.FollowTarget)
            {
                base.MoveLogic(LogicTime);
            }

            if (mScanLastTime > GameSet.Instance.ScanPlayerTime)
            {
                //主动怪
                if (mState == ePlayerState.Idle || mState == ePlayerState.Quest)
                {
                    SelectTarget();
                }
            }
            else
            {
                mScanLastTime += LogicTime;
            }
        }

        //玩家ai逻辑部分
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

        public void OnQuest() //做任务
        {
            if (null == mLogic)
                return;
            mLogic.OnQuest(this);
        }

        public void AcceptQuest() //接受任务
        {
            if (null == mLogic)
                return;
            mLogic.AcceptQuest(this);
        }

        public void SubmitQuest() //完成任务
        {
            if (null == mLogic)
                return;
            mLogic.SubmitQuest(this);
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

        int mCurrSpellNode; //当前技能节点
        List<int> mSpellSNodes = new List<int> { 3, 2, 1, 4, 3, 2, 1, 3, 2, 1 }; //技能释放顺序表
        public bool SelectSpell()
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

        public void OnCastSpell()
        {
            if (IsMoving) return;

            if (null == mAttackTarget || !mAttackTarget.CanBeAttacked())
            {
                ChangeState(ePlayerState.Idle);
                return;
            }
            //if (Spell(mCurrentSpellID, mAttackTarget) == eSkillResult.OK)
            eSkillResult ret = eSkillResult.OK;
            if (CastSpell(mCurrentSpellID, mAttackTarget.Id, ref ret))
            {
                mCurrSpellNode++;
                ChangeState(ePlayerState.WaitCoolDown);
            }
            else
            {
                ChangeState(ePlayerState.Idle);
            }
        }

        public void OnWaitCoolDown()
        {
            if (CanCastSpell()) //够攻击距离
            {
                ChangeState(ePlayerState.CastSpell);
            }
            else if (!FollowTarget(mAttackTarget))
            {
                ChangeState(ePlayerState.Idle);
            }
        }

        public bool SelectTarget() //选中攻击目标
        {
            if (null == mLogic)
                return false;

            return mLogic.SelectTarget(this);
        }

        public virtual void OnDead() //躺尸
        {
            if (!IsDie) return;

            if (mStateLastTime >= GameSet.Instance.DeadBodyTime)
            {
                mState = ePlayerState.None;
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

        public void ChangeState(ePlayerState state)
        {
            if (IsDie && mState != ePlayerState.None && state != ePlayerState.Dead)
                return;

            if (mState == state)
                return;

            mState = state;
            mStateLastTime = 0;

            //不是巡逻，追击，回出生点，移动停下来
            if (mState != ePlayerState.Quest && mState != ePlayerState.FollowTarget && mState != ePlayerState.Pause)
            {
                CleanMoveNodes();
            }
        }

        public bool MoveTo(Vector3 target)
        {
            if (Util.DistanceH(target, GetPosition()) < GameSet.Instance.PosSyncDistRange)
            {
                return true;
            }
            return base.MoveTo(target);
        }

        public bool CanCastSpell()
        {
            if (mAttackTarget == null)
                return false;

            if (Util.DistanceH(mAttackTarget.GetPosition(), GetPosition()) > mCastSpellDistance)
                return false;

            return true;
        }

        public RoleActor FindHatredTarget()
        {
            List<RoleActor> roleList = new List<RoleActor>();
            UInt32 actorTypes = (1 << (Int32)eActorGameType.Player) | (1 << (Int32)eActorGameType.Npc) | (1 << (Int32)eActorGameType.PlayerImage);
            var pos = GetPosition();
            if (!HostMap.TourRoles(ref pos, 10.0f, actorTypes, roleList))
                return null;

            foreach (var tar in roleList)
            {
                if (CanAttack(tar))
                    return tar;
            }

            return null;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_StartHook(RPC.RPCForwardInfo fwd)
        {
            mIsHook = !mIsHook;
        }

    }
}
