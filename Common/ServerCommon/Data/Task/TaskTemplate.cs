using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data.Task
{
    public enum ETalker
    {
        Unknown,
        Me,
        NPC,
    }
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class TaskKillCondition : ICopyable
    {
        uint mTargetRoleId;
        [ServerFrame.Config.DataValueAttribute("TargetRoleId")]
        public uint TargetRoleId
        {
            get { return mTargetRoleId; }
            set { mTargetRoleId = value; }
        }

        string mTargetName = "";
        [ServerFrame.Config.DataValueAttribute("TargetName")]
        public string TargetName
        {
            get { return mTargetName; }
            set { mTargetName = value; }
        }

        Int32 mNumber;
        [ServerFrame.Config.DataValueAttribute("Number")]
        public Int32 Number
        {
            get { return mNumber; }
            set { mNumber = value; }
        }

        UInt16 mDropItem = UInt16.MaxValue;
        [ServerFrame.Config.DataValueAttribute("DropItem")]
        public UInt16 DropItem
        {//根据任务要求杀死对象就用此掉落，当只要求掉落物品，不要求杀死这个对象指定数量，Number填写0
            get { return mDropItem; }
            set { mDropItem = value; }
        }

        string mKillInfo = "";
        [ServerFrame.Config.DataValueAttribute("KillInfo")]
        public string KillInfo
        {//击杀描述信息 例如：以杀死什么什么什么多少个 
            get { return mKillInfo; }
            set { mKillInfo = value; }
        }

        float mTargetPosX = 0;
        [ServerFrame.Config.DataValueAttribute("TargetPosX")]
        public float TargetPosX
        {
            get { return mTargetPosX; }
            set { mTargetPosX = value; }
        }

        float mTargetPosY = 0;
        [ServerFrame.Config.DataValueAttribute("TargetPosY")]
        public float TargetPosY
        {
            get { return mTargetPosY; }
            set { mTargetPosY = value; }
        }

        float mTargetPosZ = 0;
        [ServerFrame.Config.DataValueAttribute("TargetPosZ")]
        public float TargetPosZ
        {
            get { return mTargetPosZ; }
            set { mTargetPosZ = value; }
        }
        //float mDropRate = 1;不需要了,DropItem代表的是一个ItemTemplate
        //[ServerFrame.Config.DataValueAttribute("DropRate")]
        //public float DropRate
        //{//掉落物品的概率0-1
        //    get { return mDropRate; }
        //    set { mDropRate = value; }
        //}
    }
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class TaskEscortCondition : ICopyable
    {

    }
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class ItemProvideCondition : ICopyable
    {
        UInt16 mItemId;
        [ServerFrame.Config.DataValueAttribute("ItemId")]
        public UInt16 ItemId
        {
            get { return mItemId; }
            set { mItemId = value; }
        }

        Int32 mNumber;
        [ServerFrame.Config.DataValueAttribute("Number")]
        public Int32 Number
        {
            get { return mNumber; }
            set { mNumber = value; }
        }

        string mItemGetInfo = "";
        [ServerFrame.Config.DataValueAttribute("ItemGetInfo")]
        public string ItemGetInfo
        {//物品获得信息 例如：已获得什么什么多少个 
            get { return mItemGetInfo; }
            set { mItemGetInfo = value; }
        }

        string mItemFromName = "";
        [ServerFrame.Config.DataValueAttribute("ItemFromName")]
        public string ItemFromName
        {//物品获得信息 例如：已获得什么什么多少个 
            get { return mItemFromName; }
            set { mItemFromName = value; }
        }

        float mTargetPosX = 0;
        [ServerFrame.Config.DataValueAttribute("TargetPosX")]
        public float TargetPosX
        {
            get { return mTargetPosX; }
            set { mTargetPosX = value; }
        }

        float mTargetPosY = 0;
        [ServerFrame.Config.DataValueAttribute("TargetPosY")]
        public float TargetPosY
        {
            get { return mTargetPosY; }
            set { mTargetPosY = value; }
        }

        float mTargetPosZ = 0;
        [ServerFrame.Config.DataValueAttribute("TargetPosZ")]
        public float TargetPosZ
        {
            get { return mTargetPosZ; }
            set { mTargetPosZ = value; }
        }
    }

    public class TaskTalkParam
    {
        ETalker mWhoSaid = ETalker.Unknown;
        [ServerFrame.Config.DataValueAttribute("WhoSaid")]
        public ETalker WhoSaid
        {
            get{return mWhoSaid;}
            set {mWhoSaid = value;}
        }

        string mSaidWhat;
        [ServerFrame.Config.DataValueAttribute("SaidWhat")]
        public string SaidWhat
        {
            get{ return mSaidWhat; }
            set { mSaidWhat = value; }
        }
    }

    [EditorCommon.Assist.DelegateMethodEditor_AllowedDelegate("Task")]
    public delegate bool FOnAccept(AISystem.StateHost host, TaskData task);
    [EditorCommon.Assist.DelegateMethodEditor_AllowedDelegate("Task")]
    public delegate bool FOnDiscard(AISystem.StateHost host, TaskData task);
    [EditorCommon.Assist.DelegateMethodEditor_AllowedDelegate("Task")]
    public delegate bool FOnCommit(AISystem.StateHost host, TaskData task);
    [EditorCommon.Assist.DelegateMethodEditor_AllowedDelegate("Task")]
    public delegate bool FOnConditionOK(AISystem.StateHost host, TaskData task);
    [EditorCommon.Assist.DelegateMethodEditor_AllowedDelegate("Task")]
    public delegate bool FOnFinish(AISystem.StateHost host, TaskData task);

    //[ServerFrame.Editor.CDataEditorAttribute(".task")]
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class TaskTemplate : ICopyable
    {
        public enum ETASKTYPE
        {
            MainTask = 0,// 主线
            BranchTask,// 支线
            ProfessionTask,// 职业
            GuildTask,//公会
            CountryTask,//国家
            EverydayTask,//每日
            Count
        }

        UInt16 mTaskId;
        [ServerFrame.Config.DataValueAttribute("TaskId")]
        public UInt16 TaskId
        {
            get { return mTaskId; }
            set { mTaskId = value; }
        }

        #region 显示描述
        string mTaskName = "";
        [ServerFrame.Config.DataValueAttribute("TaskName")]
        public string TaskName
        {
            get { return mTaskName; }
            set { mTaskName = value; }
        }

        string mTaskFinishedInfo = "";
        [ServerFrame.Config.DataValueAttribute("TaskFinishedInfo")]
        public string TaskFinishedInfo
        {
            get { return mTaskFinishedInfo; }
            set { mTaskFinishedInfo = value; }
        }

        string mTaskDescription;
        [ServerFrame.Config.DataValueAttribute("TaskDescription")]// 任务描述
        public string TaskDescription
        {
            get { return mTaskDescription; }
            set { mTaskDescription = value; }
        }

        List<TaskTalkParam> mTaskTalks = new List<TaskTalkParam>();
        [ServerFrame.Config.DataValueAttribute("TaskTalks")]// 任务对话
        public List<TaskTalkParam> TaskTalks
        {
            get { return mTaskTalks; }
            set { mTaskTalks = value; }
        }

        ETASKTYPE mTaskType = 0;
        [ServerFrame.Config.DataValueAttribute("TaskType")]//任务类型： 主线 支线 等等
        public ETASKTYPE TaskType
        {
            get { return mTaskType; }
            set { mTaskType = value; }
        }

        #endregion

        #region 接取条件
        int mMinLevel;
        [ServerFrame.Config.DataValueAttribute("MinLevel")]
        public int MinLevel
        {
            get { return mMinLevel; }
            set { mMinLevel = value; }
        }

        int mMaxLevel;
        [ServerFrame.Config.DataValueAttribute("MaxLevel")]
        public int MaxLevel
        {
            get { return mMaxLevel; }
            set { mMaxLevel = value; }
        }

        List<UInt16> mBeforeTasks = new List<UInt16>();
        [ServerFrame.Config.DataValueAttribute("BeforeTasks")]
        public List<UInt16> BeforeTasks
        {
            get { return mBeforeTasks; }
            set { mBeforeTasks = value; }
        }
        #endregion

        #region 完成条件
        ulong mFinishRoleId;
        [ServerFrame.Config.DataValueAttribute("FinishRoleId")]
        public ulong FinishRoleId
        {
            get { return mFinishRoleId; }
            set { mFinishRoleId = value; }
        }

        string mFinishRoleName = "";
        [ServerFrame.Config.DataValueAttribute("FinishRoleName")]
        public string FinishRoleName
        {
            get { return mFinishRoleName; }
            set { mFinishRoleName = value; }
        }

        ulong mFinishMapId;
        [ServerFrame.Config.DataValueAttribute("FinishMapId")]
        public ulong FinishMapId
        {
            get { return mFinishMapId; }
            set { mFinishMapId = value; }
        }

        string mFinishMapName = "";
        [ServerFrame.Config.DataValueAttribute("FinishMapName")]
        public string FinishMapName
        {
            get { return mFinishMapName; }
            set { mFinishMapName = value; }
        }

        float mFinishRoleLocationX = 0;
        [ServerFrame.Config.DataValueAttribute("FinishRoleLocationX")]
        public float FinishRoleLocationX
        {
            get { return mFinishRoleLocationX; }
            set { mFinishRoleLocationX = value; }
        }
        float mFinishRoleLocationY = 0;
        [ServerFrame.Config.DataValueAttribute("FinishRoleLocationY")]
        public float FinishRoleLocationY
        {
            get { return mFinishRoleLocationY; }
            set { mFinishRoleLocationY = value; }
        }
        float mFinishRoleLocationZ = 0;
        [ServerFrame.Config.DataValueAttribute("FinishRoleLocationZ")]
        public float FinishRoleLocationZ
        {
            get { return mFinishRoleLocationZ; }
            set { mFinishRoleLocationZ = value; }
        }

                float mTargetPosX = 0;
        [ServerFrame.Config.DataValueAttribute("TargetPosX")]
        public float TargetPosX
        {
            get { return mTargetPosX; }
            set { mTargetPosX = value; }
        }

        float mTargetPosY = 0;
        [ServerFrame.Config.DataValueAttribute("TargetPosY")]
        public float TargetPosY
        {
            get { return mTargetPosY; }
            set { mTargetPosY = value; }
        }

        float mTargetPosZ = 0;
        [ServerFrame.Config.DataValueAttribute("TargetPosZ")]
        public float TargetPosZ
        {
            get { return mTargetPosZ; }
            set { mTargetPosZ = value; }
        }

        List<TaskKillCondition> mKillCondition = new List<TaskKillCondition>();
        [ServerFrame.Config.DataValueAttribute("KillCondition")]
        public List<TaskKillCondition> KillCondition
        {
            get { return mKillCondition; }
            set { mKillCondition = value; }
        }

        List<ItemProvideCondition> mItemCondition = new List<ItemProvideCondition>();
        [ServerFrame.Config.DataValueAttribute("ItemCondition")]
        public List<ItemProvideCondition> ItemCondition
        {
            get { return mItemCondition; }
            set { mItemCondition = value; }
        }
        #endregion

        #region 奖励
        List<ItemProvideCondition> mItemMustRewards = new List<ItemProvideCondition>();
        [ServerFrame.Config.DataValueAttribute("ItemMustRewards")]
        public List<ItemProvideCondition> ItemMustRewards
        {
            get { return mItemMustRewards; }
            set { mItemMustRewards = value; }
        }

        List<ItemProvideCondition> mItemChooseRewards = new List<ItemProvideCondition>();
        [ServerFrame.Config.DataValueAttribute("ItemChooseRewards")]
        public List<ItemProvideCondition> ItemChooseRewards
        {
            get { return mItemChooseRewards; }
            set { mItemChooseRewards = value; }
        }

        sbyte mItemChooseNum = 1;
        [ServerFrame.Config.DataValueAttribute("ItemChooseNum")]
        public sbyte ItemChooseNum
        {
            get { return mItemChooseNum; }
            set { mItemChooseNum = value; }
        }

        Int32 mPayMoney;
        [ServerFrame.Config.DataValueAttribute("PayMoney")]
        public Int32 PayMoney
        {
            get { return mPayMoney; }
            set { mPayMoney = value; }
        }

        Int32 mPayExp;
        [ServerFrame.Config.DataValueAttribute("PayExp")]
        public Int32 PayExp
        {
            get { return mPayExp; }
            set { mPayExp = value; }
        }
        #endregion

        #region 回掉函数
        CSCommon.Helper.EventCallBack mOnAcceptCB;
        [System.ComponentModel.Browsable(false)]
        public CSCommon.Helper.EventCallBack OnAcceptCB
        {
            get { return mOnAcceptCB; }
        }
        Guid mOnAccept = Guid.Empty;
        [ServerFrame.Config.DataValueAttribute("OnAccept")]
        [EditorCommon.Assist.DelegateMethodEditor_DelegateType(typeof(FOnAccept))]
        public Guid OnAccept
        {//接受任务调用回调
            get { return mOnAccept; }
            set 
            { 
                mOnAccept = value;
                mOnAcceptCB = CSCommon.Helper.EventCallBackManager.Instance.GetCallee(typeof(FOnAccept), value);
            }
        }

        CSCommon.Helper.EventCallBack mOnDiscardCB;
        [System.ComponentModel.Browsable(false)]
        public CSCommon.Helper.EventCallBack OnDiscardCB
        {
            get { return mOnDiscardCB; }
        }
        //ulong mOnDiscard = ulong.Empty;
        //[ServerFrame.Config.DataValueAttribute("OnDiscard")]
        //[EditorCommon.Assist.DelegateMethodEditor_DelegateType(typeof(FOnDiscard))]
        //public ulong OnDiscard
        //{//接受任务调用回调
        //    get { return mOnDiscard; }
        //    set
        //    {
        //        mOnDiscard = value;
        //        mOnDiscardCB = CSCommon.Helper.EventCallBackManager.Instance.GetCallee(typeof(FOnDiscard), value);
        //    }
        //}

        CSCommon.Helper.EventCallBack mOnCommitCB;
        [System.ComponentModel.Browsable(false)]
        public CSCommon.Helper.EventCallBack OnCommitCB
        {
            get { return mOnCommitCB; }
        }
        Guid mOnCommit = Guid.Empty;
        [ServerFrame.Config.DataValueAttribute("OnCommit")]
        [EditorCommon.Assist.DelegateMethodEditor_DelegateType(typeof(FOnCommit))]
        public Guid OnCommit
        {//提交任务调用回调
            get { return mOnCommit; }
            set 
            {
                mOnCommit = value;
                mOnCommitCB = CSCommon.Helper.EventCallBackManager.Instance.GetCallee(typeof(FOnCommit), value);
            }
        }

        CSCommon.Helper.EventCallBack mOnConditionOKCB;
        [System.ComponentModel.Browsable(false)]
        public CSCommon.Helper.EventCallBack OnConditionOKCB
        {
            get { return mOnConditionOKCB; }
        }
        Guid mOnConditionOK = Guid.Empty;
        [ServerFrame.Config.DataValueAttribute("OnConditionOK")]
        [EditorCommon.Assist.DelegateMethodEditor_DelegateType(typeof(FOnConditionOK))]
        [System.ComponentModel.Browsable(false)]
        public Guid OnConditionOK
        {//提交任务调用回调
            get { return mOnConditionOK; }
            set
            {
                mOnConditionOK = value;
                mOnConditionOKCB = CSCommon.Helper.EventCallBackManager.Instance.GetCallee(typeof(FOnConditionOK), value);
            }
        }

        CSCommon.Helper.EventCallBack mOnFinishCB;
        [System.ComponentModel.Browsable(false)]
        public CSCommon.Helper.EventCallBack OnFinishCB
        {
            get { return mOnFinishCB; }
        }
        Guid mOnFinish = Guid.Empty;
        [ServerFrame.Config.DataValueAttribute("OnFinish")]
        [EditorCommon.Assist.DelegateMethodEditor_DelegateType(typeof(FOnFinish))]
        public Guid OnFinish
        {//完成任务调用回调
            get { return mOnFinish; }
            set 
            { 
                mOnFinish = value;
                mOnFinishCB = CSCommon.Helper.EventCallBackManager.Instance.GetCallee(typeof(FOnFinish), value);
            }
        }
        #endregion
    }

    public class TaskTemplateManager
    {
        static TaskTemplateManager smInstance = new TaskTemplateManager();
        public static TaskTemplateManager Instance
        {
            get { return smInstance; }
        }

        TaskTemplate[] mTasks = new TaskTemplate[UInt16.MaxValue];

        public TaskTemplate GetTask(UInt16 id,bool bForceLoad)
        {
            lock (this)
            {
                if (mTasks[id] == null)
                {
                    mTasks[id] = new TaskTemplate();
                    LoadTaskTemplate(id, mTasks[id]);
                }
                else
                {
                    if (bForceLoad)
                    {
                        LoadTaskTemplate(id, mTasks[id]);
                    }
                }
                return mTasks[id];
            }
        }

        public void LoadTaskTemplate(UInt16 Id,TaskTemplate task)
        {
            lock (this)
            {
                string pathname = ServerFrame.Support.IFileConfig.Instance.DefaultTaskTemplateDirectory + "/" + Id.ToString() + ".task";
                if (ServerFrame.Config.IConfigurator.FillProperty(task, pathname))
                {
                    mTasks[Id] = task;
                }
                else
                {
                    Log.FileLog.WriteLine(string.Format("LoadTaskTemplate {0} failed", Id));
                }
            }
        }

        public void LoadAllTemplate()
        {
            var files = System.IO.Directory.EnumerateFiles(ServerFrame.Support.IFileManager.Instance.Root + "ZeusGame/Template/Task");
            foreach (var i in files)
            {
                if (i.Substring(i.Length - 5, 5) == ".task")
                {
                    string fullPathname = i;
                    var item = new TaskTemplate();
                    if (ServerFrame.Config.IConfigurator.FillProperty(item, ServerFrame.Support.IFileManager.Instance._GetRelativePathFromAbsPath(fullPathname)))
                    {
                        mTasks[item.TaskId] = item;
                    }
                }
            }
        }
    }

    public enum ETaskState
    {
        Accepted,
        Finished,
        Failed,
    }

    [ServerFrame.DB.DBBindTable("TaskData")]
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class TaskData : RPC.IAutoSaveAndLoad
    {
        TaskTemplate mTemplate;
        [System.ComponentModel.Browsable(false)]
        public TaskTemplate Template
        {
            get { return mTemplate; }
        }
        
        //对应技能模板ID
        UInt16 mTaskTemlateId;
        [ServerFrame.DB.DBBindField("TaskTemlateId")]
        public UInt16 TaskTemlateId
        {
            get
            {
                if (mTemplate == null)
                    return mTaskTemlateId;
                return mTemplate.TaskId;
            }
            set
            {
                mTaskTemlateId = value;
                mTemplate = TaskTemplateManager.Instance.GetTask(value,false);
            }
        }

        ulong mTaskId;
        [ServerFrame.DB.DBBindField("TaskId")]
        public ulong TaskId
        {
            get { return mTaskId; }
            set { mTaskId = value; }
        }

        ulong mOwnerId;
        [ServerFrame.DB.DBBindField("OwnerId")]
        public ulong OwnerId
        {
            get { return mOwnerId; }
            set { mOwnerId = value; }
        }

        byte mTaskState;
        [ServerFrame.DB.DBBindField("TaskState")]
        public byte TaskState
        {
            get { return mTaskState; }
            set { mTaskState = value; }
        }

        System.DateTime mAcceptTime;
        [ServerFrame.DB.DBBindField("AcceptTime")]
        public System.DateTime AcceptTime
        {
            get { return mAcceptTime; }
            set { mAcceptTime = value; }
        }

        string mKillCondition = "";//格式：Index:个数;Index:个数;...
        [ServerFrame.DB.DBBindField("KillCondition")]
        public string KillCondition
        {
            get { return mKillCondition; }
            set { mKillCondition = value; }
        }

        //他就是KillCondition的直接表达，不存数据库，也不用网络间传输
        public Int32[] KillConditionList;
    }

    [ServerFrame.DB.DBBindTable("TavernTaskData")]
    public class TavernTaskData : RPC.IAutoSaveAndLoad
    {
        ulong mTaskId;
        [ServerFrame.DB.DBBindField("TaskId")]
        public ulong TaskId
        {
            get { return mTaskId; }
            set { mTaskId = value; }
        }

        ulong mPlanesId;
        [ServerFrame.DB.DBBindField("PlanesId")]
        public ulong PlanesId
        {
            get { return mPlanesId; }
            set { mPlanesId = value; }
        }

        ulong mIssuePlayerId;
        [ServerFrame.DB.DBBindField("IssuePlayerId")]
        public ulong IssuePlayerId
        {
            get { return mIssuePlayerId; }
            set { mIssuePlayerId = value; }
        }

        System.DateTime mIssueTime;
        [ServerFrame.DB.DBBindField("IssueTime")]
        public System.DateTime IssueTime
        {
            get { return mIssueTime; }
            set { mIssueTime = value; }
        }

        string mPlanesName = "";
        [ServerFrame.DB.DBBindField("PlanesName")]
        public string PlanesName
        {
            get { return mPlanesName; }
            set { mPlanesName = value; }
        }

        string mKillPlayer = "";
        [ServerFrame.DB.DBBindField("KillPlayer")]
        public string KillPlayer
        {
            get { return mKillPlayer; }
            set { mKillPlayer = value; }
        }

        Int32 mPayRMB;
        [ServerFrame.Config.DataValueAttribute("PayRMB")]
        public Int32 PayRMB
        {
            get { return mPayRMB; }
            set { mPayRMB = value; }
        }

        Int32 mPayMoney;
        [ServerFrame.Config.DataValueAttribute("PayMoney")]
        public Int32 PayMoney
        {
            get { return mPayMoney; }
            set { mPayMoney = value; }
        }
    }
}
