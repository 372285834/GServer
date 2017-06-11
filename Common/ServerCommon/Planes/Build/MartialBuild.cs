using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class MartialBuild : BuildBase
    {
        public MartialBuild()
        {
            BuildType = CSCommon.eBuildType.Martial;
            mCostMoneyType = CSCommon.Data.eMoneyChangeType.UpMartialLevel;
        }

        public override void SetTemplate(byte id)
        {
            Template = CSTable.StaticDataManager.MartialClub[(int)id];
            MaxLevel = (byte)CSTable.StaticDataManager.MartialClub.Dict.Count;
        }

        public override sbyte UpLevel()
        {
            if (Level <= 0)
            {
                //没开放
                return (sbyte)CSCommon.eRet_UpMartialLevel.NoOpen;
            }
            if (Level >= mRole.PlayerData.RoleDetail.RoleLevel)
            {
                //不得武馆等级
                return (sbyte)CSCommon.eRet_UpMartialLevel.OverRoleLv;
            }
            if (Level >= MaxLevel)
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
