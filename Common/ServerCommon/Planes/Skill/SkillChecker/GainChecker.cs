using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 通用检测
    /// </summary>
    public class GainChecker : BaseSkillChecker
    {
        public override eSkillChecker CheckerType { get { return eSkillChecker.Gain; } }

        /// <summary>
        /// 技能预先选择目标(玩家点击目标)
        /// </summary>
        public override bool PreSelectTarget(RoleActor actor, SkillActive skill, RoleActor target)
        {
            if (!base.PreSelectTarget(actor, skill, target))
                return false;

            skill.mTargetId = actor.Id;

            return true;
        }

        /// <summary>
        /// 检查技能条件
        /// </summary>
        public override bool CheckSkillCondition(RoleActor actor, SkillActive skill)
        {
            if (!base.CheckSkillCondition(actor, skill))
                return false;

            RoleActor target = actor.GetTarget(skill.mTargetId);
            if (null == target)
                return false;

            if (target is NPCInstance)
                return false;

            if (actor.Camp != target.Camp)
                return false;

            if (actor.HostMap.MapInstanceId != target.HostMap.MapInstanceId)
                return false;

            if (!skill.SkillCD.IsCoolDown() || !actor.SkillCD.IsCoolDown())
                return false;

            if (skill.mDistance > skill.RangeMax + GameSet.Instance.SkillRangeSync)
                return false;

            return true;
        }
    }

}
