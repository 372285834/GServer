using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    public enum EOfflineEvent
    {
        Unknown,
        AddFans,
        BeAttack,
    }
    [ServerFrame.DB.DBBindTable("PlayerOfflineData")]
    public class PlayerOfflineData : RPC.IAutoSaveAndLoad
    {
        Guid mOwnerId;
        [ServerFrame.DB.DBBindField("OwnerId")]
        public Guid OwnerId
        {
            get { return mOwnerId; }
            set { mOwnerId = value; }
        }

        System.DateTime mCreateTime;
        [ServerFrame.DB.DBBindField("CreateTime")]//进行数据库绑定
        public System.DateTime CreateTime
        {
            get { return mCreateTime; }
            set { mCreateTime = value; }
        }

        sbyte mOfflineEvent = (sbyte)EOfflineEvent.Unknown;
        [ServerFrame.DB.DBBindField("OfflineEvent")]
        public sbyte OfflineEvent
        {
            get { return mOfflineEvent; }
            set { mOfflineEvent = value; }
        }

        string mText;
        [ServerFrame.DB.DBBindField("Text")]
        public string Text
        {
            get { return mText; }
            set { mText = value; }
        }
    }
}
