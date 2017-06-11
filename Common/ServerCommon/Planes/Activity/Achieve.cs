using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon.Data;

namespace ServerCommon.Planes
{
    public class Achieve : EventDispatcher
    {
        public PlayerInstance mOwner = null;

        public CSTable.IAchieveBase mTemplate;

        public AcceptAchieveData data;

        public Achieve(PlayerInstance player, CSTable.IAchieveBase tpl)
        {
            mOwner = player;
            mTemplate = tpl;
        }

        //任务目标逻辑
        IAchieveObjective mObjective;
        public IAchieveObjective Objective
        {
            get { return mObjective; }
        }

        public bool Init(AcceptAchieveData _data)
        {
            data = _data;
            data.id = mTemplate.id;
            if (data.targetNum >= mTemplate.targetNum)
            {
                return false;
            }

            BaseGameLogic<IAchieveObjective> bgLogic = (BaseGameLogic<IAchieveObjective>)GameLogicManager.Instance.GetGameLogic(eGameLogicType.AchieveLogic, (short)mTemplate.type);
            if (null == bgLogic) return false;

            mObjective = bgLogic.Logic;

            return mObjective.OnInit(this);
        }

        public void OnFinish()
        {
            if (mTemplate.atype == (int)CSCommon.eAchieveType.AchieveName)
            {
                //重新计算角色属性
                mOwner.CalcChangeValue(eValueType.Achieve);
            }

            if (null != mObjective)
                mObjective.OnFinish(this);
        }

        public void AddTargetNum(int add)
        {
            data.targetNum += add;
            CheckIsFinish();
        }

        private void CheckIsFinish()
        {
            if (data.targetNum >= mTemplate.targetNum)
            {
                data.targetNum = mTemplate.targetNum;
                OnFinish();
            }
        }

        public void SetTargetNum(int value)
        {
            data.targetNum = value;
            CheckIsFinish();
        }

        public bool IsFinished()
        {
            if (data.targetNum >= mTemplate.targetNum)
            {
                return true;
            }
            return false;
        }

        public bool IsAchieveType(CSCommon.eAchieveType type)
        {
            if (mTemplate.atype == (int)type)
            {
                return true;
            }
            return false;
        }

    }
}
