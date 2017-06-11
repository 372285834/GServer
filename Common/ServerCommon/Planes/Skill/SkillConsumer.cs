using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 技能消耗计算接口
    /// </summary>
    public interface ISkillConsumer
    {
        eSkillConsumer ConsumerType { get; }

        /// <summary>
        /// 检测技能消耗
        /// </summary>
        bool CheckSkillConsume(RoleActor actor, SkillActive skill);

        /// <summary>
        /// 实际消耗
        /// </summary>
        bool SkillConsume(RoleActor actor, SkillActive skill);
    }
}
