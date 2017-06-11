using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 击杀怪物完成任务
    /// </summary>
    public class TaskKillMonster : BaseTaskObjective
    {
        public override CSCommon.eTaskType LogicType { get { return CSCommon.eTaskType.KillMonster; } }

        /// <summary>
        /// 初始化
        /// </summary>
        public override bool OnInit(Task task)
        {
            //yzb
            if (null == task || null == task.mHostPlayer) return false;
            task.mHostPlayer.AddEventListener(EventType.Kill, task, __TaskKillMonster_Listener);
            return true;
        }

        /// <summary>
        /// 目标完成
        /// </summary>
        public override bool OnFinish(Task task)
        {
            if (null == task || null == task.mHostPlayer) return false;
            task.mHostPlayer.RemoveEventListener(EventType.Kill, task, __TaskKillMonster_Listener);
            return true;
        }

        private static void __TaskKillMonster_Listener(EventDispatcher listener, object data)
        {
            Task task = listener as Task;
            if (null == task) return;

            if (task.TaskData.Template == null)
            {
                Log.Log.Common.Info("mTaskData Template is null");
                return;
            }
            if (string.IsNullOrEmpty(task.TaskData.Template.arg1) || string.IsNullOrEmpty(task.TaskData.Template.arg2))
            {
                Log.Log.Common.Info("mTaskData Template arg1 is null{0}", task.TaskData.TemplateId);
                return;
            }

            NPCInstance target = data as NPCInstance;
            if (null == target) return;


            if (target.NPCData.TemplateId == Convert.ToInt32(task.TaskData.Template.arg1))
            {
                var pro = task.TaskData.Process + 1;
                task.SetTaskProcess(pro);
               
                var needcount = Convert.ToInt32(task.TaskData.Template.arg2);
                if (task.TaskData.Process >= needcount)
                {
                    task.SetTaskProcess(needcount);
                    task.OnFinish();
                }
            }

        }

    }
}
