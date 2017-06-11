using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("SkillData")]
    public class SkillData : RPC.IAutoSaveAndLoad
    {
        [ServerFrame.DB.DBBindField("OwnerId")]
        [RPC.AutoSaveLoad(true)]
        public ulong OwnerId { get; set; }

        [ServerFrame.DB.DBBindField("TemplateId")]
        [RPC.AutoSaveLoad(true)]
        public int TemplateId{get;set;}

        [ServerFrame.DB.DBBindField("Level")]
        [RPC.AutoSaveLoad(true)]
        public byte Level { get; set; }
    }
}
