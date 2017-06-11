using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class AchieveKilled : BaseAchieveObjective
    {
        public override CSCommon.eAchieveEventType LogicType { get { return CSCommon.eAchieveEventType.KillMonster; } }

        /// <summary>
        /// 初始化
        /// </summary>
        public override bool OnInit(Achieve data)
        {
            if (null == data || null == data.mOwner) return false;
            data.mOwner.AddEventListener(EventType.Kill, data, __Kill_Listener);
            return true;
        }

        /// <summary>
        /// 目标完成
        /// </summary>
        public override bool OnFinish(Achieve data)
        {
            if (null == data || null == data.mOwner) return false;
            data.mOwner.RemoveEventListener(EventType.Kill, data, __Kill_Listener);
            return true;
        }

        private static void __Kill_Listener(EventDispatcher listener, object data)
        {
            var achieve = listener as Achieve;
            if (null == achieve) return;
            NPCInstance target = data as NPCInstance;
            if (null == target) return;
            List<int> targetIds = GetListIdParseString(achieve.mTemplate.target);
            if (targetIds.Count > 0 && !targetIds.Contains(target.NPCData.TemplateId))
            {
                return;
            }
            achieve.AddTargetNum(1);
        }

    }
}
