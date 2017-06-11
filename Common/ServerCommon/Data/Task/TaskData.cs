using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("TaskData")]
    public class TaskData : RPC.IAutoSaveAndLoad
    {
        CSTable.TaskTplData mTemplate;
        public CSTable.TaskTplData Template
        {
            get { return mTemplate; }
        }

        ulong mOwnerId;
        [ServerFrame.DB.DBBindField("OwnerId")]
        [RPC.AutoSaveLoad]
        public ulong OwnerId
        {
            get { return mOwnerId; }
            set { mOwnerId = value; }
        }

        int mTemplateId;
        [ServerFrame.DB.DBBindField("TemplateId")]
        [RPC.AutoSaveLoad(true)]
        public int TemplateId
        {
            get { return mTemplateId; }
            set
            {
                mTemplateId = value;
                mTemplate = CSTable.StaticDataManager.TaskTpl[value];
            }
        }

        byte mState;
        [ServerFrame.DB.DBBindField("State")]
        [RPC.AutoSaveLoad(true)]
        public byte State
        {
            get { return mState; }
            set { mState = value; }
        }

        int mProcess = 0;
        [ServerFrame.DB.DBBindField("Process")]
        [RPC.AutoSaveLoad(true)]
        public int Process
        {
            get { return mProcess; }
            set { mProcess = value; }
        }
    }
}
