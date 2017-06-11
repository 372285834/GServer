using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 任务目标逻辑接口
    /// </summary>
    public interface ITaskObjective
    {
        CSCommon.eTaskType LogicType { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        bool OnInit(Task task);

        /// <summary>
        /// 目标完成
        /// </summary>
        bool OnFinish(Task task);
    }
}
