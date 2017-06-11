using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("MartialData")]
    public class MartialData : RPC.IAutoSaveAndLoad
    {
        [ServerFrame.DB.DBBindField("RoleId")]
        [RPC.AutoSaveLoad]
        public ulong RoleId { get; set; }

        //粮食
        [ServerFrame.DB.DBBindField("Food")]
        [RPC.AutoSaveLoad(true)]
        public uint Food { get; set; }

        //武馆等级
        [ServerFrame.DB.DBBindField("MartialLv")]
        [RPC.AutoSaveLoad(true)]
        public byte MartialLv { get; set; }

        //种植场等级
        [ServerFrame.DB.DBBindField("PlantLv")]
        [RPC.AutoSaveLoad(true)]
        public byte PlantLv { get; set; }

        //训练营等级
        [ServerFrame.DB.DBBindField("TrainLv")]
        [RPC.AutoSaveLoad(true)]
        public byte TrainLv { get; set; }

        //冶炼场等级
        [ServerFrame.DB.DBBindField("SmeltLv")]
        [RPC.AutoSaveLoad(true)]
        public byte SmeltLv { get; set; }

        //几种分身数量
        [ServerFrame.DB.DBBindField("ClonedCounts")]
        [RPC.AutoSaveLoad(true)]
        public string ClonedCounts { get; set; }

        //祈福次数
        [ServerFrame.DB.DBBindField("PrayCount")]
        [RPC.AutoSaveLoad(true)]
        public byte PrayCount { get; set; }

        //产出信息
        [ServerFrame.DB.DBBindField("OutPutInfo")]
        [RPC.AutoSaveLoad]
        public byte[] OutPutInfo { get; set; }
    }
}
