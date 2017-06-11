using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("CampForceData")]
    public class CampForceData : RPC.IAutoSaveAndLoad
    {
        [ServerFrame.DB.DBBindField("CityId")]
        [RPC.AutoSaveLoad(true)]
        public int CityId { get; set; }

        [ServerFrame.DB.DBBindField("Camp")]
        [RPC.AutoSaveLoad(true)]
        public byte Camp { get; set; }

        [ServerFrame.DB.DBBindField("MyForce")]
        [RPC.AutoSaveLoad(true)]
        public int MyForce { get; set; }
    }
}
