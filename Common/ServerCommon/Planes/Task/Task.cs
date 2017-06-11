using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class Task : EventDispatcher
    {
        public PlayerInstance mHostPlayer = null;

        public Task(PlayerInstance owner, CSCommon.Data.TaskData data)
        {
            mTaskData = data;
            mHostPlayer = owner;
        }

        CSCommon.Data.TaskData mTaskData;
        public CSCommon.Data.TaskData TaskData
        {
            get { return mTaskData; }
            set { mTaskData = value; }
        }

        public CSCommon.eTaskState TaskState
        {
            get { return (CSCommon.eTaskState)mTaskData.State; }
            set { mTaskData.State = (byte)value; }
        }

        public void SetTaskState(CSCommon.eTaskState ts)
        {
            mTaskData.State = (byte)ts;

            if (mHostPlayer != null)
                mHostPlayer.OnChangeTaskState();
        }

        public void SetTaskProcess(int pro)
        {
            mTaskData.Process = pro;

            if (mHostPlayer != null)
                mHostPlayer.OnChangeTaskProcess();
        }

        //任务目标逻辑
        ITaskObjective mObjective;
        public ITaskObjective Objective
        {
            get { return mObjective; }
        }

        public bool InitLogic()
        {
            BaseGameLogic<ITaskObjective> bgLogic = (BaseGameLogic<ITaskObjective>)GameLogicManager.Instance.GetGameLogic(eGameLogicType.TaskLogic, (short)mTaskData.Template.EventType);
            if (null == bgLogic) return false;

            mObjective = bgLogic.Logic;

            return mObjective.OnInit(this);
        }

        public void OnFinish()
        {
            SetTaskState(CSCommon.eTaskState.Finished);

            if (null != mObjective)
                mObjective.OnFinish(this);
        }

        public bool UpdateNextTask()
        {
            return UpdateTask(GetNextId());
        }

        public bool UpdateTask(int id)
        {
            int lastId = mTaskData.TemplateId;
            mTaskData.TemplateId = id;
            if (mTaskData.Template == null)
            {
                Log.Log.Server.Info("UpdateTask Template= null id = {0}", mTaskData.TemplateId);
                mTaskData.TemplateId = lastId;
                return false;
            }
            SetTaskState(CSCommon.eTaskState.Accepted);
            if (mTaskData.Template.EventType == (int)CSCommon.eTaskType.NpcTalk)
            {
                SetTaskState(CSCommon.eTaskState.Finished);
            }
            SetTaskProcess(0);
            InitLogic();
            return true;
        }

        public int GetNextId()
        {
            CSTable.TaskTplData template = CSTable.StaticDataManager.TaskTpl[mTaskData.TemplateId];
            if (template == null)
                return 0;
            //如果是阵营选择，下个id根据另找
            if (template.EventType == (int)CSCommon.eTaskType.CampSelect)
            {
                return mTaskData.Process;
            }
            return template.NextId;

        }
    }
}
