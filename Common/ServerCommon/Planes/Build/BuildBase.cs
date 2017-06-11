using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class BuildBase
    {
        public PlayerInstance mRole;

        public CSCommon.Data.eMoneyChangeType mCostMoneyType;

        CSCommon.eBuildType mBuildType;
        public CSCommon.eBuildType BuildType
        {
            get { return mBuildType; }
            set { mBuildType = value; }
        }

        byte mLevel;
        public byte Level
        {
            get { return mLevel; }
            set 
            { 
                mLevel = value;
                SetTemplate(mLevel);
                SetOutPutInfo();
            }
        }

        byte mMaxLevel;
        public byte MaxLevel
        {
            get { return mMaxLevel; }
            set { mMaxLevel = value; }
        }

        CSTable.IBuildBase mTemplate;
        public CSTable.IBuildBase Template
        {
            get { return mTemplate; }
            set { mTemplate = value; }
        }

        Dictionary<int, int> mOutPutInfo = null;
        public Dictionary<int, int> OutPutInfo
        {
            get { return mOutPutInfo; }
        }

        public int GetOutPutCount(byte type)
        {
            int result = 0;
            if (mOutPutInfo != null)
            {
                mOutPutInfo.TryGetValue((int)type, out result);
            }
            return result;

        }

        public virtual void SetTemplate(byte id)
        {

        }

        public void SetOutPutInfo()
        {
            if (Template != null)
            {
                var str = Template.output;
                if (!string.IsNullOrEmpty(str))
                {
                    mOutPutInfo = ServerFrame.Util.ParsingStr(Template.output);
                }
            }
        }
        public void Init(PlayerInstance role, byte lv)
        {
            mRole = role;
            Level = lv;
        }

        public void Open()
        {
            Level = 1;
        }

        public virtual sbyte UpLevel()
        {
            if (Level <= 0)
            {
                //没开放
                return (sbyte)CSCommon.eRet_UpMartialLevel.NoOpen;
            }
            if (Level >= mRole.PlayerData.MartialData.MartialLv)
            {
                //不得超过武馆等级
                return (sbyte)CSCommon.eRet_UpMartialLevel.OverMartialLv;
            }
            if (Level >= mMaxLevel)
            {
                //满级
                return (sbyte)CSCommon.eRet_UpMartialLevel.OverMaxLv;
            }

            if (Template == null)
            {
                return (sbyte)CSCommon.eRet_UpMartialLevel.NoTemplate;
            }

            var costStr = Template.cost;
            if (string.IsNullOrEmpty(costStr))
            {
                return (sbyte)CSCommon.eRet_UpMartialLevel.TplIsError;
            }
            var dict = ServerFrame.Util.ParsingStr(Template.cost);
            foreach (var i in dict)
            {
                if (!mRole._IsMoneyEnough((CSCommon.eCurrenceType)i.Key, i.Value))
                {
                    return (sbyte)CSCommon.eRet_UpMartialLevel.LessCostMoney;
                }
            }
            foreach (var cost in dict)
            {
                mRole._ChangeMoney((CSCommon.eCurrenceType)cost.Key, mCostMoneyType, -cost.Value);
            }

            Level++;
            return (sbyte)CSCommon.eRet_UpMartialLevel.Succeed;
        }
    }
}
