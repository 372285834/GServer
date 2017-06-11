using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using CSCommon.Data;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 治疗行动影响
    /// </summary>
    public class HealImpact : Impact
    {
        /// <summary>
        /// 计算实际影响
        /// </summary>
        public override void ComputeImpact()
        {
            if (null == Owner || null == Target || null == Skill)
                return;

            RoleValue OwnerVal = Owner.FinalRoleValue;
            var addHp = Owner.FinalRoleValue.Atk * Skill.LvData.damagePercent + Skill.Level * Skill.LvData.levelParam + Skill.LvData.damage;
            if (IsRateHit(OwnerVal.Crit, 0, OwnerVal.CritRate, 0))
            {
                addHp *= 1.5f;
            }
            mDamageHP = (int)addHp;
        }

        /// <summary>
        /// 计算仇恨
        /// </summary>
        public override void ComputeHatred()
        {
            
        }
    }
}
