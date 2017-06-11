using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 成就目标逻辑接口
    /// </summary>
    public interface IAchieveObjective
    {
        CSCommon.eAchieveEventType LogicType { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        bool OnInit(Achieve data);

        /// <summary>
        /// 目标完成
        /// </summary>
        bool OnFinish(Achieve data);
    }
}
