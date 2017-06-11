using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 普通buff效果
    /// </summary>
    public class Effect : BaseSkillLogic
    {
        public override eSkillLogic LogicType { get { return eSkillLogic.Effect; } }

        /// <summary>
        /// 释放动作
        /// </summary>
        public override bool Cast(RoleActor target, SkillActive skill)
        {
            if (!base.Cast(target, skill))
                return false;

            EffectImpact impact = new EffectImpact();
            impact.Init(skill, target);
            impact.Run();

            return true;
        }

        /// <summary>
        /// 初始化回调
        /// </summary>
        public override bool OnInit(RoleActor target, SkillActive skill)
        {
            if (!base.OnInit(target, skill))
                return false;

            return true;
        }

        /// <summary>
        /// 结束回调
        /// </summary>
        public override bool OnEnd(RoleActor target, SkillActive skill)
        {
            if (!base.OnEnd(target, skill))
                return false;

            return true;
        }
    }
}
