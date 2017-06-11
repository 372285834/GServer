using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class BaseSkillChecker : ISkillChecker
    {
        public virtual eSkillChecker CheckerType { get { return eSkillChecker.None; } }

        public static bool LoadAllLogic()
        {
            var assmbly = System.Reflection.Assembly.GetExecutingAssembly();
            var types = assmbly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsSubclassOf(typeof(BaseSkillChecker)))
                    continue;

                System.Activator.CreateInstance(type);
            }

            return true;
        }

        BaseGameLogic<ISkillChecker> mInstance;

        public BaseSkillChecker()
        {
            mInstance = new BaseGameLogic<ISkillChecker>(eGameLogicType.SkillChecker, (short)this.CheckerType, this);
        }

        /// <summary>
        /// 技能预先选择目标(玩家点击目标)
        /// </summary>
        public virtual bool PreSelectTarget(RoleActor actor, SkillActive skill, RoleActor target)
        {
            if (null == actor || null == skill)
                return false;

            return true;
        }

        /// <summary>
        /// 检查技能条件
        /// </summary>
        public virtual bool CheckSkillCondition(RoleActor actor, SkillActive skill)
        {
            if (null == actor || null == skill)
                return false;

            return true;
        }
    }
}
