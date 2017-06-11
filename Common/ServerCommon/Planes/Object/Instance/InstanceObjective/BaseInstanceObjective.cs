using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 副本目标逻辑基类
    /// </summary>
    public class BaseInstanceObjective : IInstanceObjective
    {
        public virtual eInstanceLogic LogicType { get { return eInstanceLogic.None; } }

        public static bool LoadAllLogic()
        {
            var assmbly = System.Reflection.Assembly.GetExecutingAssembly();
            var types = assmbly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsSubclassOf(typeof(BaseInstanceObjective)))
                    continue;

                System.Activator.CreateInstance(type);
            }

            return true;
        }

        BaseGameLogic<IInstanceObjective> mInstance;

        public BaseInstanceObjective()
        {
            mInstance = new BaseGameLogic<IInstanceObjective>(eGameLogicType.InstanceLogic, (short)this.LogicType, this);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual bool OnInit(InstanceMap instance)
        {
            return true;
        }

        /// <summary>
        /// 目标完成
        /// </summary>
        public virtual bool OnFinish(InstanceMap instance)
        {
            return true;
        }
    }
}
