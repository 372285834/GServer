using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using CSCommon.Data;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 攻击行动影响
    /// </summary>
    public class AttackImpact : Impact
    {
        /// <summary>
        /// 计算实际攻击影响
        /// </summary>
        public static int ComputeAttackImpact(double attackTotal, double defendTotal, eHitType hitType, RoleActor owner, RoleActor target)
        {
            //免伤率=防御/（防御参数+防御）*min（0.9/0.8,防御方等级/攻击方等级）							
            //其中防御参数=1400	
            var defRate = defendTotal / (1400 + defendTotal) * Math.Min(0.9 / 0.8, target.RoleLevel / owner.RoleLevel);
            if (defRate > 1)
                defRate = 1;
            if (defRate < 0)
                defRate = 0;

            double dmg = attackTotal * (1 - defRate);//skAtk * skAtk / (skAtk + def);
            dmg = dmg * (1 + owner.FinalRoleValue.UpHurtRate - target.FinalRoleValue.DownHurtRate);      //伤害加深或者削弱

            if (hitType == eHitType.Crit)
            {
                dmg *= 1.5f;
            }
            else if (hitType == eHitType.Deadly)
            {
                dmg *= 2.5f;
            }
            else if (hitType == eHitType.Block)
            {
                dmg *= 0.5f;
            }

            int damageHP = -1 * (int)dmg;
            //if (target.CurHP + damageHP < 0)
            //    damageHP = -1 * target.CurHP;
            return damageHP;
        }

        /// <summary>
        /// 计算实际影响
        /// </summary>
        public override void ComputeImpact()
        {
            if (null == Owner || null == Target || null == Skill)
                return;

            // 1、免伤公式
            // 免伤率=防御/（防御参数+防御）*min（0.9/0.8,防御方等级/攻击方等级）
            // 
            // 2、随机属性公式
            // 命中率=命中等级/3000
            // 闪避率=闪避等级/3000
            // 暴击率=暴击等级/3000
            // 暴抗率=暴抗等级/3000
            // 格挡率=格挡等级/3000
            // 
            // 战斗时先计算是否命中，命中概率=攻击方命中率/（攻击方命中率+防御方闪避率），
            // 如果命中进入减伤和伤害加深的计算
            // 之后再判断是否暴击，暴击概率=攻击方暴击率/（攻击方暴击率+防御方暴抗率），暴击的话乘以暴击系数，不暴击按正常伤害
            // 最后判断是否格挡，格挡的话在已有伤害的基础上乘以50%
            RoleValue OwnerVal = Owner.FinalRoleValue;
            RoleValue revVal = Target.FinalRoleValue;
            int hit = OwnerVal.Hit;
            if (hit == 0)
                hit = 1;

            if (!IsRateHit(hit, revVal.Dodge, 0, 0)) //未命中
            {
                mHitType = eHitType.Miss;
                return;
            }

            mHitType = eHitType.Hit;
            if (IsRateHit(0, 1, OwnerVal.DeadlyHit / 3000 + OwnerVal.DeadlyHitRate, 0))
            {
                mHitType = eHitType.Deadly; //致命
            }
            else if (IsRateHit(OwnerVal.Crit, revVal.CritDef, OwnerVal.CritRate, revVal.CritDefRate))
            {
                mHitType = eHitType.Crit; //暴击
            }
            else if (IsRateHit(0, 1, Target.FinalRoleValue.Block / 3000 + Target.FinalRoleValue.BlockRate, 0))
            {
                mHitType = eHitType.Block; //格挡
            }

            float attackTotal = Owner.FinalRoleValue.Atk * Skill.LvData.damagePercent + Skill.Level * Skill.LvData.levelParam + Skill.LvData.damage;
            float defendTotal = Target.FinalRoleValue.GetDef(Owner.ElemType);

            mDamageHP = ComputeAttackImpact(attackTotal, defendTotal, mHitType, Owner, Target);
        }

        /// <summary>
        /// 计算仇恨
        /// </summary>
        public override void ComputeHatred()
        {
            if (mDamageHP >= 0)
                return;

            mHatred = -1 * mDamageHP;
            //if (Target.CurHP - mHatred < 0)
            //    mHatred = Target.CurHP;

            Target.UpdateEnmityList(Owner.Id, mHatred);
            Owner.UpdateEnmityList(Target.Id, 0);
        }
    }
}
