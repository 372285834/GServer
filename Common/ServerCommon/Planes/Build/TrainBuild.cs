using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class TrainBuild : BuildBase
    {
        public TrainBuild()
        {
            BuildType = CSCommon.eBuildType.Train;
            mCostMoneyType = CSCommon.Data.eMoneyChangeType.UpTrainLevel;
        }

        public override void SetTemplate(byte id)
        {
            Template = CSTable.StaticDataManager.MartialTrain[id];
            MaxLevel = (byte)CSTable.StaticDataManager.MartialTrain.Dict.Count;
        }
    }
}
