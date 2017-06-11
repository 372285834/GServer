using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("Message")]
    public class Message : RPC.IAutoSaveAndLoad
    {
        ulong mMessageId = 0;
        [ServerFrame.DB.DBBindField("MessageId")]
        [RPC.AutoSaveLoad(true)]
        public ulong MessageId
        {
            get { return mMessageId; }
            set { mMessageId = value; }
        }
        ulong mOwnerId = 0;
        [ServerFrame.DB.DBBindField("OwnerId")]
        [RPC.AutoSaveLoad]
        public ulong OwnerId
        {
            get { return mOwnerId; }
            set { mOwnerId = value; }
        }
        string mSender = "";
        [ServerFrame.DB.DBBindField("Sender")]
        [RPC.AutoSaveLoad(true)]
        public string Sender
        {
            get { return mSender; }
            set { mSender = value; }
        }

        System.DateTime mCreateTime = System.DateTime.Now;
        [ServerFrame.DB.DBBindField("CreateTime")]  //建立时间  
        [RPC.AutoSaveLoad]
        public System.DateTime CreateTime
        {
            get { return mCreateTime; }
            set { mCreateTime = value; }
        }

        byte mMessageFrom;
        [ServerFrame.DB.DBBindField("MessageFrom")]  //消息来源
        [RPC.AutoSaveLoad(true)]
        public byte MessageFrom
        {
            get { return mMessageFrom; }
            set { mMessageFrom = value; }
        }

        byte mMessageType;
        [ServerFrame.DB.DBBindField("MessageType")]  //消息类型
        [RPC.AutoSaveLoad(true)]
        public byte MessageType
        {
            get { return mMessageType; }
            set { mMessageType = value; }
        }

        string mShowInfo;
        [ServerFrame.DB.DBBindField("ShowInfo")]  //消息内容
        [RPC.AutoSaveLoad(true)]
        public string ShowInfo
        {
            get { return mShowInfo; }
            set { mShowInfo = value; }
        }
    }
}
