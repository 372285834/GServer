using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class TaskSelectCamp : BaseTaskObjective
    {
       public override CSCommon.eTaskType LogicType { get { return CSCommon.eTaskType.CampSelect; } }

        /// <summary>
        /// 初始化
        /// </summary>
       public override bool OnInit(Task task)
        {
            if (null == task || null == task.mHostPlayer) return false;
            task.mHostPlayer.AddEventListener(EventType.SelectCamp, task, __TaskSelectCamp_Listener);
            return true;
        }

        /// <summary>
        /// 目标完成
        /// </summary>
       public override bool OnFinish(Task task)
        {
            if (null == task || null == task.mHostPlayer) return false;
            task.mHostPlayer.RemoveEventListener(EventType.SelectCamp, task, __TaskSelectCamp_Listener);
            return true;
        }

        private static void __TaskSelectCamp_Listener(EventDispatcher listener, object data)
        {
            Task task = listener as Task;
            if (null == task) return;

            if (task.TaskData.Template == null)
            {
                Log.Log.Common.Info("mTaskData Template is null");
                return;
            }
            if (task.TaskData.Template.EventType != (int)CSCommon.eTaskType.CampSelect)
            {
                return;
            }
            List<int> ids = new List<int>();
            var strids = task.TaskData.Template.arg1.Split('|');
            foreach (var i in strids)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    ids.Add(Convert.ToInt32(i));
                }
            }

            int camp = Convert.ToInt32(data);

            if (camp > ids.Count)
            {
                Log.Log.Common.Info("SelectCamp tpl error");
                return;
            }
            task.SetTaskProcess(ids[camp - 1]);
            task.OnFinish();
        }


        //public void UpLevel(byte roleLevel)
        //{
        //    if (mTaskData.Template == null)
        //    {
        //        Log.Log.Common.Info("mTaskData Template is null");
        //        return;
        //    }
        //    if (mTaskData.Template.EventType != (int)CSCommon.eTaskType.UpLevel)
        //    {
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(mTaskData.Template.arg1))
        //    {
        //        Log.Log.Common.Info("mTaskData Template arg1 is null{0}", mTaskData.TemplateId);
        //        return;
        //    }
        //    if (roleLevel >= Convert.ToByte(mTaskData.Template.arg1))
        //    {
        //        mTaskData.Process++;
        //        Finished();
        //    }
        //}

        //public void GetItem(int templateId, int count)
        //{
        //    if (mTaskData.Template == null)
        //    {
        //        Log.Log.Common.Info("mTaskData Template is null");
        //        return;
        //    }
        //    if (mTaskData.Template.EventType != (int)CSCommon.eTaskType.GetItem)
        //    {
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(mTaskData.Template.arg3) || string.IsNullOrEmpty(mTaskData.Template.arg2))
        //    {
        //        Log.Log.Common.Info("mTaskData Template arg is null{0}", mTaskData.TemplateId);
        //        return;
        //    }
        //    if (templateId == Convert.ToInt32(mTaskData.Template.arg3))
        //    {
        //        mTaskData.Process += count;
        //        var needcount = Convert.ToInt32(mTaskData.Template.arg2);
        //        if (mTaskData.Process >= needcount)
        //        {
        //            mTaskData.Process = needcount;
        //            Finished();
        //        }
        //    }
        //}

        //public void Collect(int count)
        //{
        //    if (mTaskData.Template == null)
        //    {
        //        Log.Log.Common.Info("mTaskData Template is null");
        //        return;
        //    }
        //    if (mTaskData.Template.EventType != (int)CSCommon.eTaskType.Collect)
        //    {
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(mTaskData.Template.arg2))
        //    {
        //        Log.Log.Common.Info("mTaskData Template arg is null{0}", mTaskData.TemplateId);
        //        return;
        //    }
        //    mTaskData.Process += count;
        //    var needcount = Convert.ToInt32(mTaskData.Template.arg2);
        //    if (mTaskData.Process >= needcount)
        //    {
        //        mTaskData.Process = needcount;
        //        Finished();
        //    }
        //}
    }
}
