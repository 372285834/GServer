using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 技能条件检测接口
    /// </summary>
    public interface ISkillChecker
    {
        eSkillChecker CheckerType { get; }

        /// <summary>
        /// 技能预先选择目标(玩家点击目标)
        /// </summary>
        bool PreSelectTarget(RoleActor actor, SkillActive skill, RoleActor target);

        /// <summary>
        /// 检查技能条件
        /// </summary>
        bool CheckSkillCondition(RoleActor actor, SkillActive skill);
    }
}
