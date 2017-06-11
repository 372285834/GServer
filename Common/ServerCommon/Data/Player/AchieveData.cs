using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("AchieveData")]
    public class AchieveData : RPC.IAutoSaveAndLoad
    {
        [ServerFrame.DB.DBBindField("RoleId")]
        [RPC.AutoSaveLoad]
        public ulong RoleId { get; set; }

        [ServerFrame.DB.DBBindField("Achievement")]
        [RPC.AutoSaveLoad]
        public byte[] Achievement { get; set; }

        [ServerFrame.DB.DBBindField("Exploit")]//总战功
        [RPC.AutoSaveLoad]
        public uint Exploit { get; set; }

        [ServerFrame.DB.DBBindField("CopyInfo")]//副本通关记录
        [RPC.AutoSaveLoad]
        public byte[] CopyInfo { get; set; }
        
    }


}
