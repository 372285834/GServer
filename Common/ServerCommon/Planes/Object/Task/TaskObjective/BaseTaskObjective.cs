using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 副本目标逻辑基类
    /// </summary>
    public class BaseTaskObjective : ITaskObjective
    {
        public virtual CSCommon.eTaskType LogicType { get { return CSCommon.eTaskType.None; } }

        public static bool LoadAllLogic()
        {
            var assmbly = System.Reflection.Assembly.GetExecutingAssembly();
            var types = assmbly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsSubclassOf(typeof(BaseTaskObjective)))
                    continue;

                System.Activator.CreateInstance(type);
            }

            return true;
        }

        BaseGameLogic<ITaskObjective> mInstance;

        public BaseTaskObjective()
        {
            mInstance = new BaseGameLogic<ITaskObjective>(eGameLogicType.TaskLogic, (short)this.LogicType, this);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual bool OnInit(Task task)
        {
            return true;
        }

        /// <summary>
        /// 目标完成
        /// </summary>
        public virtual bool OnFinish(Task task)
        {
            return true;
        }
    }
}