using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    public enum ETaskType
    {
        [Description("普通任务")]
        Single,
        [Description("可重复任务")]
        Repeatable,     // 可重复
        [Description("日常")]
        Daily,          // 日常
        [Description("玩家发布")]
        PlayerPublish,  // 玩家发布的
    }

    public enum ETaskClass
    {
        [Description("主线")]
        Main,       // 主线
        [Description("支线")]
        Branch,     // 支线
        [Description("活动")]
        Campaign,   // 活动
    }

    // 存盘的任务状态
    public enum ETaskStateType
    {
        Active,
        Completed,
        Discard,
    }

    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class MinMaxValue
    {
        double mMin = 1;
        [ServerFrame.Config.DataValueAttribute("Min")]
        public double Min
        {
            get { return mMin; }
            set { mMin = value; }
        }

        double mMax = 1;
        [ServerFrame.Config.DataValueAttribute("Max")]
        public double Max
        {
            get { return mMax; }
            set { mMax = value; }
        }

        public bool IsInRange(double value)
        {
            if (value >= Min && value <= Max)
                return true;

            return false;
        }
    }
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class MinMaxDate
    {
        System.DateTime mMin = new System.DateTime(1970, 1, 1);
        [ServerFrame.Config.DataValueAttribute("Min")]
        public System.DateTime Min
        {
            get { return mMin; }
            set { mMin = value; }
        }

        System.DateTime mMax = new System.DateTime(2970, 1, 1);
        [ServerFrame.Config.DataValueAttribute("Max")]
        public System.DateTime Max
        {
            get { return mMax; }
            set { mMax = value; }
        }

        public bool IsInRange(System.DateTime value)
        {
            if (value >= Min && value <= Max)
                return true;

            return false;
        }
    }

    public class TaskCondition
    {
        string m_targetDescribe = "";
        [ServerFrame.Config.DataValueAttribute("TargetDescribe")]
        public string TargetDescribe
        {
            get { return m_targetDescribe; }
            set { m_targetDescribe = value; }
        }

        //UInt16 m_levelNeed_Min = 1;
        //[ServerFrame.Config.DataValueAttribute("LevelNeed_Min")]
        //public UInt16 LevelNeed_Min
        //{
        //    get { return m_levelNeed_Min; }
        //    set { m_levelNeed_Min = value; }
        //}

        //UInt16 m_levelNeed_Max = 1;
        //[ServerFrame.Config.DataValueAttribute("LevelNeed_Max")]
        //public UInt16 LevelNeed_Max
        //{
        //    get { return m_levelNeed_Max; }
        //    set { m_levelNeed_Max = value; }
        //}

        //// 跟某某对话
        //Guid m_talkTarget = Guid.Empty;
        //[ServerFrame.Config.DataValueAttribute("TalkTarget")]
        //public Guid TalkTarget
        //{
        //    get { return m_talkTarget; }
        //    set { m_talkTarget = value; }
        //}

        // 杀死目标
        UInt16 mKillSceneId = UInt16.MaxValue;
        [ServerFrame.Config.DataValueAttribute("KillSceneId")]
        public UInt16 KillSceneId
        {
            get { return mKillSceneId; }
            set { mKillSceneId = value; }
        }

        UInt16 mKillTarget = UInt16.MaxValue;
        [ServerFrame.Config.DataValueAttribute("KillTarget")]
        public UInt16 KillTarget
        {
            get { return mKillTarget; }
            set { mKillTarget = value; }
        }
        UInt16 mKillNumber = UInt16.MaxValue;
        [ServerFrame.Config.DataValueAttribute("KillNumber")]
        public UInt16 KillNumber
        {
            get { return mKillNumber; }
            set { mKillNumber = value; }
        }

        // 需求物品
        UInt16 mItemNeed = UInt16.MaxValue;
        [ServerFrame.Config.DataValueAttribute("ItemNeed")]
        public UInt16 ItemNeed
        {
            get { return mItemNeed; }
            set { mItemNeed = value; }
        }
        UInt16 mItemNumber = UInt16.MaxValue;
        [ServerFrame.Config.DataValueAttribute("ItemNumber")]
        public UInt16 ItemNumber
        {
            get { return mItemNumber; }
            set { mItemNumber = value; }
        }

        // 某项技能升级到N级

        // 钱达到多少
        UInt32 mMinMoney = UInt32.MaxValue;
        [ServerFrame.Config.DataValueAttribute("MinMoney")]
        public UInt32 MinMoney
        {
            get { return mMinMoney; }
            set { mMinMoney = value; }
        }
    }

    public class TaskAwardItem
    {
        UInt16 m_item = UInt16.MaxValue;
        [ServerFrame.Config.DataValueAttribute("Item")]
        public UInt16 Item
        {
            get { return m_item; }
            set { m_item = value; }
        }

        Byte m_itemNumber;
        [ServerFrame.Config.DataValueAttribute("ItemNumber")]
        public Byte ItemNumber
        {
            get { return m_itemNumber; }
            set { m_itemNumber = value; }
        }
    }

    public class TaskAwardMercenary
    {
        UInt16 m_mercenary = UInt16.MaxValue;
        [ServerFrame.Config.DataValueAttribute("Mercenary")]
        public UInt16 Mercenary
        {
            get { return m_mercenary; }
            set { m_mercenary = value; }
        }

        Byte m_mercenaryNumber;
        [ServerFrame.Config.DataValueAttribute("MercenaryNumber")]
        public Byte MercenaryNumber
        {
            get { return m_mercenaryNumber; }
            set { m_mercenaryNumber = value; }
        }
    }
    public class TaskAward
    {
        List<TaskAwardItem> mAwardItems = new List<TaskAwardItem>();
        [ServerFrame.Config.DataValueAttribute("AwardItems")]
        [DisplayName("奖励物品")]
        public List<TaskAwardItem> AwardItems
        {
            get { return mAwardItems; }
            set { mAwardItems = value; }
        }

        List<TaskAwardMercenary> mAwardMercenarys = new List<TaskAwardMercenary>();
        [ServerFrame.Config.DataValueAttribute("AwardMercenarys")]
        [DisplayName("奖励弟子")]
        public List<TaskAwardMercenary> AwardMercenarys
        {
            get { return mAwardMercenarys; }
            set { mAwardMercenarys = value; }
        }

        UInt32 mExperience = 0;
        [ServerFrame.Config.DataValueAttribute("Experience")]
        [DisplayName("经验")]
        public UInt32 Experience
        {
            get { return mExperience; }
            set { mExperience = value; }
        }

        UInt32 mGold = 0;
        [ServerFrame.Config.DataValueAttribute("Gold")]
        [DisplayName("金币")]
        public UInt32 Gold
        {
            get { return mGold; }
            set { mGold = value; }
        }
    }

    public class TaskTalkDescribe
    {
        string mTalkString = "";
        [ServerFrame.Config.DataValueAttribute("TalkString")]
        [DisplayName("对话")]
        public string TalkString
        {
            get { return mTalkString; }
            set { mTalkString = value; }
        }
        
        public override string ToString()
        {
 	         return TalkString;
        }
    }

    // 任务状态
    public class TaskStep
    {
        string mTrackString = "";
        [ServerFrame.Config.DataValueAttribute("TrackString")]
        [DisplayName("任务追踪文字")]
        //[System.ComponentModel.EditorAttribute(typeof(CSCommon.Editor.MultilineTextEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string TrackString
        {
            get { return mTrackString; }
            set { mTrackString = value; }
        }

        string mTaskTalk = "";
        [ServerFrame.Config.DataValueAttribute("TaskTalk")]
        [DisplayName("任务对话")]
        //[System.ComponentModel.EditorAttribute(typeof(CSCommon.Editor.MultilineTextEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string TaskTalk
        {
            get { return mTaskTalk; }
            set { mTaskTalk = value; }
        }

        string mTaskTarget = "";
        [ServerFrame.Config.DataValueAttribute("TaskTarget")]
        [DisplayName("任务目标")]
        //[System.ComponentModel.EditorAttribute(typeof(CSCommon.Editor.MultilineTextEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string TaskTarget
        {
            get { return mTaskTarget; }
            set { mTaskTarget = value; }
        }

        string mTaskDescription = "";
        [ServerFrame.Config.DataValueAttribute("TaskDescription")]
        [DisplayName("任务描述")]
        //[System.ComponentModel.EditorAttribute(typeof(CSCommon.Editor.MultilineTextEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string TaskDescription
        {
            get { return mTaskDescription; }
            set { mTaskDescription = value; }
        }

        List<TaskCondition> m_finishConditions = new List<TaskCondition>();
        [ServerFrame.Config.DataValueAttribute("FinishConditions")]
        [DisplayName("完成条件")]
        public List<TaskCondition> FinishConditions
        {
            get { return m_finishConditions; }
            set { m_finishConditions = value; }
        }

        Guid mTalkTargetId = Guid.Empty;
        [ServerFrame.Config.DataValueAttribute("TalkTargetId")]
        [DisplayName("对话目标")]
        public Guid TalkTargetId
        {
            get { return mTalkTargetId; }
            set { mTalkTargetId = value; }
        }

        //System.Drawing.PointF mTargetPoint = System.Drawing.PointF.Empty;
        //[TypeConverter(typeof(CSCommon.Editor.PointFConverter))]
        //[ServerFrame.Config.DataValueAttribute("TargetPoint")]
        //[DisplayName("目标位置")]
        //public System.Drawing.PointF TargetPoint
        //{
        //    get { return mTargetPoint; }
        //    set { mTargetPoint = value; }
        //}
    }

    //[ServerFrame.Editor.CDataEditorAttribute(".task")]
    public class TaskTemplate
    {
        ETaskType m_taskType;
        [ServerFrame.Config.DataValueAttribute("TaskType")]
        [DisplayName("任务类型")]
        [TypeConverter(typeof(ServerFrame.Editor.EnumConverter))]
        public ETaskType TaskType
        {
            get { return m_taskType; }
            set { m_taskType = value; }
        }

        ETaskClass mTaskClass;
        [ServerFrame.Config.DataValueAttribute("TaskClass")]
        [DisplayName("任务种类")]
        [TypeConverter(typeof(ServerFrame.Editor.EnumConverter))]
        public ETaskClass TaskClass
        {
            get { return mTaskClass; }
            set { mTaskClass = value; }
        }

        UInt16 m_taskId;
        [ServerFrame.Config.DataValueAttribute("TaskId")]
        //[System.ComponentModel.ReadOnly(true)]
        public UInt16 TaskId
        {
            get { return m_taskId; }
            set { m_taskId = value; }
        }

        bool mDiscardAble = true;
        [ServerFrame.Config.DataValueAttribute("DiscardAble")]
        [DisplayName("可否放弃")]
        public bool DiscardAble
        {
            get { return mDiscardAble; }
            set { mDiscardAble = value; }
        }

        // 等级需求
        MinMaxValue mLevelNeed = new MinMaxValue();
        [ServerFrame.Config.DataValueAttribute("LevelRange")]
        [DisplayName("等级范围")]
        public MinMaxValue LevelRange
        {
            get { return mLevelNeed; }
            set { mLevelNeed = value; }
        }

        string mTaskName = "";
        [ServerFrame.Config.DataValueAttribute("TaskName")]
        [DisplayName("任务名称")]
        public string TaskName
        {
            get { return mTaskName; }
            set { mTaskName = value; }
        }

        // 至少有两步，开始和结束
        List<TaskStep> mTaskSteps = new List<TaskStep>();
        [ServerFrame.Config.DataValueAttribute("TaskSteps")]
        [DisplayName("任务步骤")]
        public List<TaskStep> TaskSteps
        {
            get { return mTaskSteps; }
            set { mTaskSteps = value; }
        }

        //string mTrackString = "";
        //[ServerFrame.Config.DataValueAttribute("TrackString")]
        //[DisplayName("任务追踪文字")]
        //public string TrackString
        //{
        //    get { return mTrackString; }
        //    set { mTrackString = value; }
        //}

        List<UInt16> m_preTasks = new List<UInt16>();
        [ServerFrame.Config.DataValueAttribute("PreTasks")]
        [DisplayName("前置任务列表")]
        public List<UInt16> PreTasks
        {
            get { return m_preTasks; }
            set { m_preTasks = value; }
        }

        List<UInt16> mNextTasks = new List<UInt16>();
        [ServerFrame.Config.DataValueAttribute("NextTasks")]
        [DisplayName("后置任务列表")]
        public List<UInt16> NextTasks
        {
            get { return mNextTasks; }
            set { mNextTasks = value; }
        }

        // 任务描述对话，每段为一个分页
        List<TaskTalkDescribe> m_taskDescribes = new List<TaskTalkDescribe>();
        [ServerFrame.Config.DataValueAttribute("TaskDescribes")]
        [DisplayName("任务对话")]
        [System.ComponentModel.EditorAttribute(typeof(ServerFrame.Editor.MultilineTextEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public List<TaskTalkDescribe> TaskDescribes
        {
            get { return m_taskDescribes; }
            set { m_taskDescribes = value; }
        }

        //[Browsable(false)]
        //public string TaskTalk
        //{
        //    get
        //    {
        //        string retStr = "";
        //        foreach (var str in TaskDescribes)
        //        {
        //            retStr += str;
        //        }
        //        return retStr;
        //    }
        //}
        //string mTaskTalk = "";
        //[ServerFrame.Config.DataValueAttribute("TaskTalk")]
        //[DisplayName("任务对话")]
        //public string TaskTalk
        //{
        //    get { return mTaskTalk; }
        //    set { mTaskTalk = value; }
        //}

        //List<TaskCondition> m_finishConditions = new List<TaskCondition>();
        //[ServerFrame.Config.DataValueAttribute("FinishConditions")]
        //[DisplayName("完成条件")]
        //public List<TaskCondition> FinishConditions
        //{
        //    get { return m_finishConditions; }
        //    set { m_finishConditions = value; }
        //}

        List<TaskAward> m_awards = new List<TaskAward>();
        [ServerFrame.Config.DataValueAttribute("Awards")]
        [DisplayName("奖励")]
        public List<TaskAward> Awards
        {
            get { return m_awards; }
            set { m_awards = value; }
        }

        List<UInt16> m_resetTaskWhenFinished = new List<UInt16>();
        [ServerFrame.Config.DataValueAttribute("ResetTaskWhenFinished")]
        [DisplayName("完成后重置的任务列表")]
        public List<UInt16> ResetTaskWhenFinished
        {
            get { return m_resetTaskWhenFinished; }
            set { m_resetTaskWhenFinished = value; }
        }

        MinMaxDate mValidTime = new MinMaxDate();//second
        [ServerFrame.Config.DataValueAttribute("ValidTime")]
        [DisplayName("有效时间")]
        public MinMaxDate ValidTime
        {
            get { return mValidTime; }
            set { mValidTime = value; }
        }

        Int16 mResetTime = -1;//每天的第几点重置
        [ServerFrame.Config.DataValueAttribute("ResetTime")]
        [DisplayName("每日几点重置")]
        public Int16 ResetTime
        {
            get { return mResetTime; }
            set { mResetTime = value; }
        }

        string mOnAccept = "";
        [ServerFrame.Config.DataValueAttribute("OnAccept")]
        public string OnAccept
        {
            get { return mOnAccept; }
            set { mOnAccept = value; }
        }
        string mOnFinished = "";
        [ServerFrame.Config.DataValueAttribute("OnFinished")]
        public string OnFinished
        {
            get { return mOnFinished; }
            set { mOnFinished = value; }
        }
        string mOnDiscard = "";
        [ServerFrame.Config.DataValueAttribute("OnDiscard")]
        public string OnDiscard
        {
            get { return mOnDiscard; }
            set { mOnDiscard = value; }
        }
    }

    public class TaskTemplateManager
    {
        public const int MaxTaskTemplateNumber = 8192;

        static TaskTemplateManager smInstance = new TaskTemplateManager();
        public static TaskTemplateManager Instance
        {
            get { return smInstance; }
        }

        TaskTemplate[] mTasks = new TaskTemplate[MaxTaskTemplateNumber];
        public TaskTemplate[] Tasks
        {
            get { return mTasks; }
        }

        public void LoadAllTemplate()
        {
            mTasks = new TaskTemplate[MaxTaskTemplateNumber];
            var files = System.IO.Directory.EnumerateFiles(ServerFrame.Support.IFileManager.Instance.Root + "ZeusGame/Template/Task");
            foreach (var i in files)
            {
                if (i.Substring(i.Length - 5, 5) == ".task")
                {
                    string fullPathname = i;
                    TaskTemplate item = new TaskTemplate();
                    if (ServerFrame.Config.IConfigurator.FillProperty(item, fullPathname))
                    {
                        mTasks[item.TaskId] = item;
                    }
                }
            }
        }

        public TaskTemplate FindItem(UInt16 id)
        {
            if (id < 0 || id >= MaxTaskTemplateNumber)
                return null;

            return mTasks[id];
        }
    }

    [ServerFrame.DB.DBBindTable("TaskData")]
    public class TaskData : RPC.IAutoSaveAndLoad
    {
        TaskTemplate mTemplate;
        public TaskTemplate Template
        {
            get { return mTemplate; }
        }
        UInt16 mTemplateId;
        [ServerFrame.DB.DBBindField("TemplateId")]
        public UInt16 TemplateId
        {
            get { return mTemplateId; }
            set
            {
                mTemplateId = value;
                mTemplate = TaskTemplateManager.Instance.FindItem(value);
            }
        }
        Guid mTaskId;
        [ServerFrame.DB.DBBindField("TaskId")]
        public Guid TaskId
        {
            get { return mTaskId; }
            set { mTaskId = value; }
        }
        Guid mOwnerId;
        [ServerFrame.DB.DBBindField("OwnerId")]
        public Guid OwnerId
        {
            get { return mOwnerId; }
            set { mOwnerId = value; }
        }
        SByte mTaskStateType;//可重复任务是否已经结束
        [ServerFrame.DB.DBBindField("TaskStateType")]
        public SByte TaskStateType
        {
            get { return mTaskStateType; }
            set { mTaskStateType = value; }
        }
        string mTaskState;
        [ServerFrame.DB.DBBindField("TaskState")]
        public string TaskState
        {//xml保存任务的状态
            get { return mTaskState; }
            set { mTaskState = value; }
        }
        // 任务步骤
        Byte mTaskStep = 0;
        [ServerFrame.DB.DBBindField("TaskStep")]
        public Byte TaskStep
        {
            get { return mTaskStep; }
            set { mTaskStep = value; }
        }
        System.DateTime mAcceptTime;
        [ServerFrame.DB.DBBindField("AcceptTime")]
        public System.DateTime AcceptTime
        {
            get { return mAcceptTime; }
            set { mAcceptTime = value; }
        }
        System.DateTime mFinishTime;
        [ServerFrame.DB.DBBindField("FinishTime")]
        public System.DateTime FinishTime
        {
            get { return mFinishTime; }
            set { mFinishTime = value; }
        }
        #region 玩家特殊定制任务信息
        string mSpecialName = "";
        [ServerFrame.DB.DBBindField("SpecialName")]
        public string SpecialName
        {
            get
            {
                if (mSpecialName != null && mSpecialName != "")
                    return mSpecialName;
                if (Template != null && !string.IsNullOrEmpty(Template.TaskName))
                {
                    return Template.TaskName;
                }
                return "";
            }
            set { mSpecialName = value; }
        }

        string mSpecialTaskDescribe = "";
        [ServerFrame.DB.DBBindField("SpecialTaskDescribe")]
        public string SpecialTaskDescribe
        {
            get
            {
                if (mSpecialTaskDescribe != null && mSpecialTaskDescribe != "")
                    return mSpecialTaskDescribe;
                //if (Template != null && Template.TaskTalk != null)
                //    return Template.TaskTalk;
                return "";
                //return Template.TaskTalk;
            }
            set { mSpecialTaskDescribe = value; }
        }

        Guid mSpecialAwardItem;
        [ServerFrame.DB.DBBindField("SpecialAwardItem")]
        public Guid SpecialAwardItem
        {
            get { return mSpecialAwardItem; }
            set { mSpecialAwardItem = value; }
        }
        string mKillPlayerName = "";
        [ServerFrame.DB.DBBindField("KillPlayerName")]
        public string KillPlayerName
        {//需要杀死的玩家名,这个在接任务的时候特殊OnAccept来处理
            get { return mKillPlayerName; }
            set { mKillPlayerName = value; }
        }
        Byte mSpecialCount = Byte.MaxValue;
        [ServerFrame.DB.DBBindField("SpecialCount")]
        public Byte SpecialCount
        {//需要杀死的玩家次数,杀死一次减一，等于0就完成，这个在接任务的时候特殊OnAccept来处理
            get { return mSpecialCount; }
            set { mSpecialCount = value; }
        }
        #endregion
    }
}
