using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("EfficiencyData")]
    public class EfficiencyData : RPC.IAutoSaveAndLoad
    {
        [ServerFrame.DB.DBBindField("EfficiencyItemId")]
        [RPC.AutoSaveLoad(true)]
        public int EfficiencyItemId { get; set; }

        [ServerFrame.DB.DBBindField("TimeLeft")]
        [RPC.AutoSaveLoad(true)]
        public int TimeLeft { get; set; }
    }
}
