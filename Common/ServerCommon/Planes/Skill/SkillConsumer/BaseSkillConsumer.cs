using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class BaseSkillConsumer : ISkillConsumer
    {
        public virtual eSkillConsumer ConsumerType { get { return eSkillConsumer.None; } }

        public static bool LoadAllLogic()
        {
            var assmbly = System.Reflection.Assembly.GetExecutingAssembly();
            var types = assmbly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsSubclassOf(typeof(BaseSkillConsumer)))
                    continue;

                System.Activator.CreateInstance(type);
            }

            return true;
        }

        BaseGameLogic<ISkillConsumer> mInstance;

        public BaseSkillConsumer()
        {
            mInstance = new BaseGameLogic<ISkillConsumer>(eGameLogicType.SkillConsumer, (short)this.ConsumerType, this);
        }

        public virtual bool CheckSkillConsume(RoleActor actor, SkillActive skill)
        {
            if (null == actor || null == skill)
                return false;

            return true;
        }

        public virtual bool SkillConsume(RoleActor actor, SkillActive skill)
        {
            if (null == actor || null == skill)
                return false;

            return true;
        }
    }
}
