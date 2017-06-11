using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 技能目标选择接口
    /// </summary>
    public interface ISkillSelector
    {
        eSkillSelector SelectorType { get; }

        /// <summary>
        /// 实际选择目标
        /// </summary>
        bool SelectTarget(RoleActor caster, SkillActive skill, ref List<RoleActor> targets);

        /// <summary>
        /// 检查目标是否有效
        /// </summary>
        bool TargetFilter(RoleActor caster, SkillActive skill, RoleActor target, eRelationType relation);
    }
}
