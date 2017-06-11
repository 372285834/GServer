using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 技能目标选择逻辑
    /// </summary>
    public class BaseSkillSelector : ISkillSelector
    {
        public virtual eSkillSelector SelectorType { get { return eSkillSelector.None; } }

        public static bool LoadAllLogic()
        {
            var assmbly = System.Reflection.Assembly.GetExecutingAssembly();
            var types = assmbly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsSubclassOf(typeof(BaseSkillSelector)))
                    continue;

                System.Activator.CreateInstance(type);
            }

            return true;
        }

        BaseGameLogic<ISkillSelector> mInstance;

        public BaseSkillSelector()
        {
            mInstance = new BaseGameLogic<ISkillSelector>(eGameLogicType.SkillSelector, (short)this.SelectorType, this);
        }

        /// <summary>
        /// 实际选择目标
        /// </summary>
        public virtual bool SelectTarget(RoleActor caster, SkillActive skill, ref List<RoleActor> targets)
        {
            if (null == caster || null == skill)
                return false;

            return true;
        }

        /// <summary>
        /// 检查目标是否有效
        /// </summary>
        public virtual bool TargetFilter(RoleActor caster, SkillActive skill, RoleActor target, eRelationType relation)
        {
            if (null == caster || null == skill || null == target)
                return false;

            return true;
        }
    }
}
