using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 保护目标通关副本事件
    /// </summary>
    public class ProtectTarget : BaseInstanceObjective
    {
        public override eInstanceLogic LogicType { get { return eInstanceLogic.ProtectTarget; } }

        /// <summary>
        /// 初始化
        /// </summary>
        public override bool OnInit(InstanceMap instance)
        {
            if (null == instance || null == instance.Owner) return false;

            instance.Owner.AddEventListener(EventType.Kill, instance, __InstanceProtectTarget_Listener);

            return true;
        }

        /// <summary>
        /// 目标完成
        /// </summary>
        public override bool OnFinish(InstanceMap instance)
        {
            if (null == instance || null == instance.Owner) return false;

            instance.Owner.RemoveEventListener(EventType.Kill, instance, __InstanceProtectTarget_Listener);
            return true;
        }

        private static void __InstanceProtectTarget_Listener(EventDispatcher listener, object data)
        {
            //获取副本数据
            ChallengeInstance instance = listener as ChallengeInstance;
            if (null == instance || null == instance.mCopyTplData) return;

            NPCInstance target = data as NPCInstance;
            if (null == target) return;

            instance.mKilledMobNum++;
            if (instance.mCopyTplData.targetId == target.NPCData.TemplateId) //保护目标已死，任务失败
                instance.OnFinish(false);
            else
            {
                if (instance.mKilledMobNum == instance.mTotalMobNum)
                {
                    NPCInstance protectTarget = instance.GetNPC(instance.mCopyTplData.targetId);
                    if (null != protectTarget && !protectTarget.IsDie)
                        instance.OnFinish(true);
                }
            }
        }
    }
}
