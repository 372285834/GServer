using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class PlantBuild : BuildBase
    {
        public PlantBuild()
        {
            BuildType = CSCommon.eBuildType.Plant;
            mCostMoneyType = CSCommon.Data.eMoneyChangeType.UpPlantLevel;
            
        }

        public override void SetTemplate(byte id)
        {
            Template = CSTable.StaticDataManager.MartialPlant[id];
            MaxLevel = (byte)CSTable.StaticDataManager.MartialPlant.Dict.Count;
        }
    }
}
