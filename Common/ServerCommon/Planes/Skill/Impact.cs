using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 攻击影响
    /// </summary>
    public class Impact
    {
        protected eHitType mHitType;
        public eHitType HitType
        {
            get { return mHitType; }
        }

        protected int mDamageHP;
        public int DamageHP
        {
            get { return mDamageHP; }
        }

        protected int mDamageMP;
        public int DamageMP
        {
            get { return mDamageMP; }
        }

        protected int mHatred;
        public bool mKilled;

        SkillActive mSkill;
        public SkillActive Skill
        {
            get { return mSkill; }
        }

        RoleActor mOwner;
        public RoleActor Owner
        {
            get { return mOwner; }
        }

        RoleActor mTarget;
        public RoleActor Target
        {
            get { return mTarget; }
        }

        /// <summary>
        /// 初始化攻击对象
        /// </summary>
        public void Init(SkillActive skill, RoleActor target)
        {
            mSkill = skill;
            mTarget = target;
            mOwner = skill.Owner;
            mKilled = false;
            mHitType = eHitType.Hit;
            mDamageHP = 0;
            mDamageMP = 0;
            mHatred = 0;
        }

        /// <summary>
        /// 计算实际攻击
        /// </summary>
        public virtual void ComputeImpact()
        {

        }

        /// <summary>
        /// 计算仇恨
        /// </summary>
        public virtual void ComputeHatred()
        {

        }

        /// <summary>
        /// 是否命中
        /// </summary>
        public virtual bool IsRateHit(float atk, float def, float atkRate, float defRate)
        {
            var rate = atk / (atk + def) + atkRate - defRate;
            if (rate >= 1)
                return true;
            if (rate <= 0)
                return false;

            var hit = (int)(rate * ServerDefine.FightRandomMax);
            var r = ServerFrame.Util.Rand.Next(ServerDefine.FightRandomMax);
            if (r <= hit)
                return true;

            return false;
        }

        /// <summary>
        /// 运行攻击
        /// </summary>
        public void Run(bool isBroadCast = true)
        {
            if (null == mOwner || null == mTarget)
                return;

            //战斗状态

            //pk状态

            //pk保护

            //开始攻击
            mOwner.OnAttack();

            //开始计算伤害
            mTarget.OnImpactBegin(this);

            //计算伤害
            ComputeImpact();

            //计算仇恨
            ComputeHatred();

            //结束计算伤害
            mTarget.OnImpactEnd();

            //结束攻击
            mTarget.OnImpacted(isBroadCast);

            if (mKilled)
            {
                //触发击杀事件
                if (mTarget.GameType == eActorGameType.Npc)
                {
                    if (mOwner.GameType == eActorGameType.Player)
                    {
                        mOwner.DispatchEvent(EventType.Kill, mTarget);
                    }
                    else if (mOwner.GameType == eActorGameType.Npc)
                    {
                        Owner.HostMap.Owner.DispatchEvent(EventType.Kill, mTarget);
                    }
                }
                mOwner.OnKillActor(mTarget);
                mOwner.KickOneFromEnmityList(mTarget.Id);
            }
        }
    }
}
