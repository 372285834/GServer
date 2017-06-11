using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 副本目标逻辑接口
    /// </summary>
    public interface IInstanceObjective
    {
        eInstanceLogic LogicType { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        bool OnInit(InstanceMap instance);

        /// <summary>
        /// 目标完成
        /// </summary>
        bool OnFinish(InstanceMap instance);
    }
}
