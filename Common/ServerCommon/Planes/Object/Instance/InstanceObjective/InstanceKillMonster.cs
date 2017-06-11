using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 击杀怪物通关副本事件
    /// </summary>
    public class KillMonster : BaseInstanceObjective
    {
        public override eInstanceLogic LogicType { get { return eInstanceLogic.KillMonster; } }

        /// <summary>
        /// 初始化
        /// </summary>
        public override bool OnInit(InstanceMap instance)
        {
            if (null == instance || null == instance.Owner) return false;
            //yzb
            instance.Owner.AddEventListener(EventType.Kill, instance, __InstanceKillMonster_Listener);

            return true;
        }

        /// <summary>
        /// 目标完成
        /// </summary>
        public override bool OnFinish(InstanceMap instance)
        {
            if (null == instance || null == instance.Owner) return false;

            instance.Owner.RemoveEventListener(EventType.Kill, instance, __InstanceKillMonster_Listener);
            return true;
        }

        private static void __InstanceKillMonster_Listener(EventDispatcher listener, object data)
        {
            //获取副本数据
            ChallengeInstance instance = listener as ChallengeInstance;
            if (null == instance || null == instance.mCopyTplData) return;

            NPCInstance target = data as NPCInstance;
            if (null == target) return;

            instance.mKilledMobNum++;
            if (instance.mCopyTplData.targetId == 0) //杀光所有怪物
            {
                if (instance.mKilledMobNum == instance.mTotalMobNum)
                    instance.OnFinish(true);
            }
            else
            {
                if (instance.mCopyTplData.targetId == target.NPCData.TemplateId)
                    instance.OnFinish(true);
            }
            
        }
    }
}
