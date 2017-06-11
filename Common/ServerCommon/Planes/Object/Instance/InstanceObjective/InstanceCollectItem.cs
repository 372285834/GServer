using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 收集物品通关副本事件
    /// </summary>
    public class CollectItem : BaseInstanceObjective
    {
        public override eInstanceLogic LogicType { get { return eInstanceLogic.CollectItem; } }

        /// <summary>
        /// 初始化
        /// </summary>
        public override bool OnInit(InstanceMap instance)
        {
            if (null == instance || null == instance.Owner) return false;

            instance.Owner.AddEventListener(EventType.CollectItem, instance, __InstanceGatherItem_Listener);

            return true;
        }

        /// <summary>
        /// 目标完成
        /// </summary>
        public override bool OnFinish(InstanceMap instance)
        {
            if (null == instance || null == instance.Owner) return false;

            instance.Owner.RemoveEventListener(EventType.CollectItem, instance, __InstanceGatherItem_Listener);
            return true;
        }

        private static void __InstanceGatherItem_Listener(EventDispatcher listener, object data)
        {
            //获取副本数据
            ChallengeInstance instance = listener as ChallengeInstance;
            if (null == instance || null == instance.mCopyTplData) return;
        }
    }
}
