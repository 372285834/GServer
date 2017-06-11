using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerFrame;
using CSCommon;
using CSCommon.Data;

namespace ServerCommon.Planes.Skill
{   
    /// <summary>
    /// 普通近战攻击
    /// </summary>
    public class NormalAttackNear : SkillActive
    {
        public override void Init(int skillid,byte lv)
        {
            base.Init(skillid, lv);
        }

        public override void OnUse(IActor caster, CSCommon.IActor target)
        {
            base.OnUse(caster, target);
            //公共cd
            AddCoolDown(CSCommon.SCDefine.cCommonCD, Time.time + CSCommon.SCDefine.cCommonCD);
            AddCoolDown(mData.cdTime, Time.time + mData.cdTime);            
        }

        /// <summary>
        /// 技能到达
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="casterid"></param>
        /// <param name="param0"></param>
        /// <param name="?"></param>
        public override void ReceiveTarget(IActor receiver, IActor caster, object param0)
        {
            base.ReceiveTarget(receiver, caster, param0);

            byte hitIndex = (byte)param0;
            RoleActor rev = receiver as RoleActor;
            RoleActor ct = caster as RoleActor;
            var hitRet = CheckHitType(ct, rev);
            int dmg = GetDamage(ct, rev, hitRet);
            dmg = (int)(dmg / this.mData.hitCount);

            SkillHitData hit = new SkillHitData();
            hit.HitType = (byte)hitRet;
            hit.Damage = dmg;
            rev.OnSkillReceive(ct, this, hit);
        }

        

    }
}
