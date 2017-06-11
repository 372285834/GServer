using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using CSCommon.Data;
using ServerFrame;

namespace ServerCommon.Planes
{
    /// <summary>
    /// Buff效果影响
    /// </summary>
    public class EffectImpact : Impact
    {
        /// <summary>
        /// 计算实际影响
        /// </summary>
        public override void ComputeImpact()
        {
            if (null == Owner || null == Target || null == Skill)
                return;

            Action action = Skill.mAction;
            if (null == action.actionParams || action.actionParams.Count != 4)
                return;

            int buffId = (int)action.actionParams[0];
            int buffLevel = (int)action.actionParams[1];
            int buffTarget = (int)action.actionParams[2];
            float buffRate = (float)action.actionParams[3];

            if (Util.Rand.Next(100) > buffRate)
                return;

            CSTable.BuffData buffData = CSTable.StaticDataManager.Buff[buffId];
            if (null == buffData)
                return;

            if (null != Owner.BuffMgr.GetBuff(buffId))
            {
                if (Skill.mStep == eSkillStep.Delay)
                    return;
            }

            if (Target.PlayerStatus.IsImmunityBuffStatus(buffData.type))
                return;

            if (!Target.IsImmunityDebuff())
            {
                
            }

            Target.BuffMgr.CreateBuffBySkill(buffId, buffLevel, Skill);
        }

        /// <summary>
        /// 计算仇恨
        /// </summary>
        public override void ComputeHatred()
        {

        }
    }
}
