using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("CityForceData")]
    public class CityForceData : RPC.IAutoSaveAndLoad
    {
        [ServerFrame.DB.DBBindField("CityId")]
        [RPC.AutoSaveLoad(true)]
        public int CityId { get; set; }

        [ServerFrame.DB.DBBindField("TotalForce")]
        [RPC.AutoSaveLoad(true)]
        public int TotalForce { get; set; }

        [ServerFrame.DB.DBBindField("MyForceNum")]
        [RPC.AutoSaveLoad(true)]
        public int MyForceNum { get; set; }
    }
}
