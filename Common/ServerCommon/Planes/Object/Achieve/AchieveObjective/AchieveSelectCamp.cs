using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class AchieveSelectCamp : BaseAchieveObjective
    {
        public override CSCommon.eAchieveEventType LogicType { get { return CSCommon.eAchieveEventType.SelectCamp; } }

        /// <summary>
        /// 初始化
        /// </summary>
        public override bool OnInit(Achieve data)
        {
            if (null == data || null == data.mOwner) return false;
            data.mOwner.AddEventListener(EventType.SelectCamp, data, _Common_Listener);
            return true;
        }

        /// <summary>
        /// 目标完成
        /// </summary>
        public override bool OnFinish(Achieve data)
        {
            if (null == data || null == data.mOwner) return false;
            data.mOwner.RemoveEventListener(EventType.SelectCamp, data, _Common_Listener);
            return true;
        }

    }
}
