using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 技能逻辑
    /// </summary>
    public class BaseSkillLogic : ISkillLogic
    {
        public virtual eSkillLogic LogicType { get { return eSkillLogic.None; } }

        public static bool LoadAllLogic()
        {
            var assmbly = System.Reflection.Assembly.GetExecutingAssembly();
            var types = assmbly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsSubclassOf(typeof(BaseSkillLogic)))
                    continue;

                System.Activator.CreateInstance(type);
            }

            return true;
        }

        BaseGameLogic<ISkillLogic> mInstance;

        public BaseSkillLogic()
        {
            mInstance = new BaseGameLogic<ISkillLogic>(eGameLogicType.SkillLogic, (short)this.LogicType, this);
        }

        /// <summary>
        /// 释放动作
        /// </summary>
        public virtual bool Cast(RoleActor target, SkillActive skill)
        {
            if (null == target || null == skill)
                return false;

            return true;
        }

        /// <summary>
        /// 初始化回调
        /// </summary>
        public virtual bool OnInit(RoleActor target, SkillActive skill)
        {
            if (null == target || null == skill)
                return false;

            return true;
        }

        /// <summary>
        /// 结束回调
        /// </summary>
        public virtual bool OnEnd(RoleActor target, SkillActive skill)
        {
            if (null == target || null == skill)
                return false;

            return true;
        }
    }
}
