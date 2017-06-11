using CSCommon;
using CSCommon.Data;
using ServerFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public struct CoolDownTime
    {
        public uint Id;
        public float EndTime;
        public float Total;
    }

    public class SkillCD
    {
        protected Dictionary<uint, CoolDownTime> mCDTimeList = new Dictionary<uint, CoolDownTime>();
        uint mAutoCdId = 0;

        public void AddCoolDown(float longTime, float endTime)
        {
            if (longTime <= 0 || Time.time >= endTime) return;

            mCDTimeList.Add(mAutoCdId, new CoolDownTime() { Id = mAutoCdId, Total = longTime, EndTime = endTime });
            TimerManager.doOnce(longTime, OnCDTimerEnd, mAutoCdId);
            mAutoCdId++;
        }

        public void OnCDTimerEnd(TimerEvent timerEvent)
        {
            mCDTimeList.Remove((uint)timerEvent.param);
        }

        public bool IsCoolDown()
        {
            var now = Time.time;
            foreach (var time in mCDTimeList.Values)
            {
                if (time.EndTime > now)
                    return false;
            }

            return true;
        }
    }

    public struct ActionGroup
    {
        public int limit; //技能目标个数限制
        public List<Action> actions; //行动队列
        public ActionGroup(int num)
        {
            limit = num;
            actions = new List<Action>();
        }
    }

    public struct Action
    {
        public ISkillLogic actionLogic; //逻辑
        public List<object> actionParams; //参数列表
        public Action(ISkillLogic logic)
        {
            actionLogic = logic;
            actionParams = new List<object>();
        }
    }

    public class SkillActive : ASkillObject
    {
        public override eSkillType BaseType
        {
            get { return eSkillType.Active; }
        }

        protected CSTable.SkillActiveData mData;
        protected CSTable.SkillLevelData mLvData;
        public CSTable.SkillLevelData LvData
        {
            get { return mLvData; }
        }

        public override int ID
        {
            get { return mData.id; }
        }

        public override CSTable.ISkillTable Data
        {
            get { return mData; }
        }

        public CSTable.SkillActiveData SkillData
        {
            get { return mData; }
        }

        public CSTable.SkillLevelData LevelData
        {
            get { return mLvData; }
        }

        public override int Level
        {
            get { return LevelData.level; }
        }

        public int mHitIndex;

        SkillCD mSkillCD = new SkillCD();
        public SkillCD SkillCD
        {
            get { return mSkillCD; }
        }

        RoleActor mOwner = null;
        public RoleActor Owner //技能所有者
        {
            get { return mOwner; }
        }

        public ulong mTargetId; //技能选定目标ID
        public float mDistance; //技能选定目标距离
        public float RangeMax //技能释放最大距离
        {
            get
            {
                if (mOwner is NPCInstance)
                    return (mOwner as NPCInstance).NPCData.Template.distance;

                return mData.range;
            }
        }

        public eSkillStatus mStatus = eSkillStatus.Invalid;
        public eSkillStep mStep = eSkillStep.Init;
        public eSkillResult mResult = eSkillResult.CastFailed;

        ISkillSelector mSelector;
        public ISkillSelector Selector //取得选择目标处理
        {
            get { return mSelector; }
        }

        ISkillChecker mChecker;
        public ISkillChecker Checker //取得检查处理
        {
            get { return mChecker; }
        }

        ISkillConsumer mConsumer;
        public ISkillConsumer Consumer //取得消耗处理
        {
            get { return mConsumer; }
        }

        ActionGroup mActionGroupData;
        public ActionGroup ActionGroupData //取得行动组
        {
            get { return mActionGroupData; }
            set { mActionGroupData = value; }
        }

        public Action mAction; //当前行动

        public override bool Init(int tid, byte lv)
        {
            mData = CSTable.StaticDataManager.SkillActive[tid];
            mLvData = CSTable.StaticDataManager.SkillLevel[tid, lv];

            if (null == mData || null == mLvData)
                return false;

            BaseGameLogic<ISkillSelector> selectorLogic = (BaseGameLogic<ISkillSelector>)GameLogicManager.Instance.GetGameLogic(eGameLogicType.SkillSelector, (short)mData.selector);
            if (null != selectorLogic)
                mSelector = selectorLogic.Logic;

            BaseGameLogic<ISkillChecker> checkerLogic = (BaseGameLogic<ISkillChecker>)GameLogicManager.Instance.GetGameLogic(eGameLogicType.SkillChecker, (short)mData.checker);
            if (null != checkerLogic)
                mChecker = checkerLogic.Logic;

            BaseGameLogic<ISkillConsumer> consumerLogic = (BaseGameLogic<ISkillConsumer>)GameLogicManager.Instance.GetGameLogic(eGameLogicType.SkillConsumer, (short)mData.consumer);
            if (null != consumerLogic)
                mConsumer = consumerLogic.Logic;

            InitActionGroup();

            //已学会技能
            mStatus = eSkillStatus.Valid;

            return true;
        }

        /// <summary>
        /// 初始化行动组
        /// </summary>
        public bool InitActionGroup()
        {
            mActionGroupData = new ActionGroup(0);
            BaseGameLogic<ISkillLogic> actionLogic = (BaseGameLogic<ISkillLogic>)GameLogicManager.Instance.GetGameLogic(eGameLogicType.SkillLogic, (short)mData.action);
            if (null != actionLogic)
            {
                mActionGroupData.actions.Add(new Action(actionLogic.Logic));
            }

            if (mLvData.buff1ids > 0)
            {
                actionLogic = (BaseGameLogic<ISkillLogic>)GameLogicManager.Instance.GetGameLogic(eGameLogicType.SkillLogic, (short)eSkillLogic.Effect);
                if (null != actionLogic)
                {
                    Action action = new Action(actionLogic.Logic);
                    action.actionParams.Add(mLvData.buff1ids);
                    action.actionParams.Add(mLvData.buff1level);
                    action.actionParams.Add(mLvData.buff1TarType);
                    action.actionParams.Add(mLvData.buff1Rate);
                    mActionGroupData.actions.Add(action);
                }
            }

            if (mLvData.buff2ids > 0)
            {
                actionLogic = (BaseGameLogic<ISkillLogic>)GameLogicManager.Instance.GetGameLogic(eGameLogicType.SkillLogic, (short)eSkillLogic.Effect);
                if (null != actionLogic)
                {
                    Action action = new Action(actionLogic.Logic);
                    action.actionParams.Add(mLvData.buff2ids);
                    action.actionParams.Add(mLvData.buff2level);
                    action.actionParams.Add(mLvData.buff2TarType);
                    action.actionParams.Add(mLvData.buff2Rate);
                    mActionGroupData.actions.Add(action);
                }
            }

            return true;
        }

        /// <summary>
        /// 设置技能属主
        /// </summary>
        public override void SetOwner(RoleActor owner)
        {
            mOwner = owner;
        }

        /// <summary>
        /// 技能重置
        /// </summary>
        public void Reset()
        {
            mTargetId = 0;
            mStep = eSkillStep.Init;
            mHitIndex = 0;
            mActionGroupData.limit = 0;
        }

    }
}
