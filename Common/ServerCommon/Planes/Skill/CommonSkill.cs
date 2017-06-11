//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ServerFrame;
//using CSCommon;
//using CSCommon.Data;

//namespace ServerCommon.Planes
//{   
//    /// <summary>
//    /// 通用技能， 远程弹道用假的模拟，延迟处理
//    /// </summary>
//    public class CommonSkill : SkillActive
//    {
//        public override void Init(RoleActor owner, int skillid,byte lv)
//        {
//            base.Init(owner, skillid, lv);
//        }

//        public override void Cast(RoleActor caster, RoleActor target)
//        {
//            mHitIndex = 0;
//            Use(caster, target);
//        }

//        public override void StopSkill(RoleActor stoper)
//        {
//            mHitIndex = -1;
//        }

//        public override void OnUse(RoleActor _caster, RoleActor _target)
//        {
//            base.OnUse(_caster, _target);
//            //公共cd
//            _caster.SkillCD.AddCoolDown(CSCommon.SCDefine.cCommonCD, Time.time + CSCommon.SCDefine.cCommonCD);
//            _caster.SkillCD.AddCoolDown(mData.protectTime, Time.time + mData.protectTime);
//            SkillCD.AddCoolDown(mData.cdTime, Time.time + mData.cdTime);

//            RoleActor caster = _caster as RoleActor;
//            RoleActor target = _target as RoleActor;

//            TimeParam param = new TimeParam();
//            param.caster = caster;
//            param.target = target;

//            mHitIndex = 0;
//            if (mLvData.hitTime.Length == 0 || mLvData.hitTime[mHitIndex] == 0)
//            {
//                var ev = new TimerEvent();
//                ev.param = param;
//                OnTimeRealCast(ev);          
//            }
//            else
//                TimerManager.doOnce(mLvData.hitTime[mHitIndex], OnTimeRealCast, param);

//        }

//        void OnTimeRealCast(TimerEvent timerEvent)
//        {
//            if (mHitIndex < 0)
//                return;

//            TimeParam param = timerEvent.param as TimeParam;
//            _DoRealCast(param);
//            mHitIndex++;
//            if (mHitIndex < mLvData.hitCount)
//            {
//                float delay = this.mLvData.hitTime[mHitIndex];
//                TimerManager.doOnce(delay, OnTimeRealCast, param);
//            }
//        }

//        private void _DoRealCast(TimeParam param)
//        {
//            RoleActor caster = param.caster;
//            RoleActor target = param.target;
//            if (mData.throwEffectSpeed > 0)
//            {//有弹道
//                //caster.CreateSummonSyncClient(target, this);
//                float delay = Util.DistanceH(caster.GetPosition(), target.GetPosition()) / mData.throwEffectSpeed;
//                TimerManager.doOnce(delay, OnTimerReceive, param); 
//            }
//            else
//            {
//                _DoReceive(caster, target);
//            }  
//        }

//        void OnTimerReceive(TimerEvent timerEvent)
//        {
//            TimeParam param = timerEvent.param as TimeParam;
//            _DoReceive(param.caster, param.target);
//        }

//        private void _DoReceive(RoleActor caster, RoleActor target)
//        {
//            if (mData.areaType == (byte)eSkillAreaType.One)
//            {
//                ReceiveTarget(target, caster, null);
//            }
//            else if (mData.areaType == (byte)eSkillAreaType.SelfCircle)
//            {
//                RoleActor actor = caster as RoleActor;
//                var targets = actor.GetCirclRangeActor(mData.range);
//                foreach (var t in targets)
//                {
//                    if (t == caster)
//                        continue;
//                    ReceiveTarget(t, caster, null);
//                }
//            }
//            else if (mData.areaType == (byte)eSkillAreaType.TargetCircle)
//            {
//                RoleActor actor = target as RoleActor;
//                var targets = actor.GetCirclRangeActor(mData.range);
//                foreach (var t in targets)
//                {
//                    if (t == caster)
//                        continue;
//                    ReceiveTarget(t, caster, null);
//                }
//            }
//        }

//        /// <summary>
//        /// 技能到达目标
//        /// </summary>
//        public override void ReceiveTarget(RoleActor receiver, RoleActor caster, object param0)
//        {
//            base.ReceiveTarget(receiver, caster, param0);
//            RoleActor rev = receiver as RoleActor;
//            RoleActor ct = caster as RoleActor;
//            var hitRet = RoleActor.CheckHitType(ct, rev);
//            int dmg = GetDamage(ct, rev, hitRet);
//            dmg = (int)(dmg / this.mLvData.hitCount);

//            SkillHitData hit = new SkillHitData();
//            hit.HitType = (byte)hitRet;
//            hit.Damage = dmg;

//            if (mData.tarType == (byte)eSkillTargetType.Self)
//            {
//                ct.OnSkillReceive(ct, this, hit);
//            }
//            else
//                rev.OnSkillReceive(ct, this, hit);  

//        }



//    }
//}
