using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 单目标选择
    /// </summary>
    public class SingleTarget : BaseSkillSelector
    {
        public override eSkillSelector SelectorType { get { return eSkillSelector.Single; } }

        /// <summary>
        /// 实际选择目标
        /// </summary>
        public override bool SelectTarget(RoleActor caster, SkillActive skill, ref List<RoleActor> targets)
        {
            if (!base.SelectTarget(caster, skill, ref targets))
                return false;

            RoleActor target = caster.GetTarget(skill.mTargetId);
            if (null == target)
                return false;

            targets.Add(target);

            return true;
        }

        /// <summary>
        /// 检查目标是否有效
        /// </summary>
        public override bool TargetFilter(RoleActor caster, SkillActive skill, RoleActor target, eRelationType relation)
        {
            if (!base.TargetFilter(caster, skill, target, relation))
                return false;

            return true;
        }
    }
}
