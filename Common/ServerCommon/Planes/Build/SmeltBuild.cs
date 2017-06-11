using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class SmeltBuild : BuildBase
    {
        public SmeltBuild()
        {
            BuildType = CSCommon.eBuildType.Smelt;
            mCostMoneyType = CSCommon.Data.eMoneyChangeType.UpSmeltLevel;
            
        }

        public override void SetTemplate(byte id)
        {
            Template = CSTable.StaticDataManager.MartialSmelt[id];
            MaxLevel = (byte)CSTable.StaticDataManager.MartialSmelt.Dict.Count;
        }
    }
}
