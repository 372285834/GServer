using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class AchieveUpLevel : BaseAchieveObjective
    {
        public override CSCommon.eAchieveEventType LogicType { get { return CSCommon.eAchieveEventType.RoleReachLv; } }

        /// <summary>
        /// 初始化
        /// </summary>
        public override bool OnInit(Achieve data)
        {
            if (null == data || null == data.mOwner) return false;
            data.mOwner.AddEventListener(EventType.UpLevel, data, _UpLevel_Listener);
            return true;
        }

        /// <summary>
        /// 目标完成
        /// </summary>
        public override bool OnFinish(Achieve data)
        {
            if (null == data || null == data.mOwner) return false;
            data.mOwner.RemoveEventListener(EventType.UpLevel, data, _UpLevel_Listener);
            return true;
        }


        public static void _UpLevel_Listener(EventDispatcher listener, object data)
        {
            ushort value = (ushort)data;
            var achieve = listener as Achieve;
            achieve.SetTargetNum((int)value);
        }
    }
}
