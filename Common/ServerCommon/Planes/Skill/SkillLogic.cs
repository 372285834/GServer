using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 技能逻辑接口
    /// </summary>
    public interface ISkillLogic
    {
        eSkillLogic LogicType { get; }

        /// <summary>
        /// 释放动作
        /// </summary>
	    bool Cast(RoleActor target, SkillActive skill);
	
	    /// <summary>
	    /// 初始化回调
	    /// </summary>
	    bool OnInit(RoleActor target, SkillActive skill);
	
	    /// <summary>
	    /// 结束回调
	    /// </summary>
	    bool OnEnd(RoleActor target, SkillActive skill);
    }
}
