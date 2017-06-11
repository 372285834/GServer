using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("SocialData")]
    public class SocialData : RPC.IAutoSaveAndLoad
    {
        ulong mOwnerId;
        [ServerFrame.DB.DBBindField("OwnerId")]
        [RPC.AutoSaveLoad(true)]
        public ulong OwnerId
        {
            get { return mOwnerId; }
            set { mOwnerId = value; }
        }

        ulong mOtherId;
        [ServerFrame.DB.DBBindField("OtherId")]
        [RPC.AutoSaveLoad(true)]
        public ulong OtherId
        {
            get { return mOtherId; }
            set { mOtherId = value; }
        }

        uint mSocail;
        [ServerFrame.DB.DBBindField("Socail")]
        [RPC.AutoSaveLoad(true)]
        public uint Socail
        {
            get { return mSocail; }
            set { mSocail = value; }
        }

        int mSocailIntimacy = 0;//亲密度
        [ServerFrame.DB.DBBindField("SocailIntimacy")]
        [RPC.AutoSaveLoad(true)]
        public int SocailIntimacy
        {
            get { return mSocailIntimacy; }
            set { mSocailIntimacy = value; }
        }

        int mYesterdayIntimacy = 0;//昨日亲密度
        [ServerFrame.DB.DBBindField("YesterdayIntimacy")]
        [RPC.AutoSaveLoad(true)]
        public int YesterdayIntimacy
        {
            get { return mYesterdayIntimacy; }
            set { mYesterdayIntimacy = value; }
        }

        int mTodayIntimacy = 0;//今天亲密度
        [ServerFrame.DB.DBBindField("TodayIntimacy")]
        [RPC.AutoSaveLoad(true)]
        public int TodayIntimacy
        {
            get { return mTodayIntimacy; }
            set { mTodayIntimacy = value; }
        }

        System.DateTime mLastAddIntimacyTime;//最后一次加亲密度时间
        [ServerFrame.DB.DBBindField("LastAddIntimacyTime")]
        [RPC.AutoSaveLoad]
        public System.DateTime LastAddIntimacyTime
        {
            get { return mLastAddIntimacyTime; }
            set { mLastAddIntimacyTime = value; }
        }

    }

    public class SocialRoleInfo : RPC.IAutoSaveAndLoad
    {
        [RPC.AutoSaveLoad(true)]
        public ulong id { get; set; }

        [RPC.AutoSaveLoad(true)]
        public string name{ get; set; }

        [RPC.AutoSaveLoad(true)]
        public byte profession { get; set; }

        [RPC.AutoSaveLoad(true)]
        public UInt16 level { get; set; }

        [RPC.AutoSaveLoad(true)]
        public byte camp { get; set; }

        [RPC.AutoSaveLoad(false)]
        public CSCommon.Data.SocialData socialData = new CSCommon.Data.SocialData();

        [RPC.AutoSaveLoad(true)]
        public byte state { get; set; }
    }
}
