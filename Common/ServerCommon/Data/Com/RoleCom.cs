using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("RoleCom")]
    public class RoleCom : RPC.IAutoSaveAndLoad
    {
        ulong mRoleId;
        [ServerFrame.DB.DBBindField("RoleId")]
        [RPC.AutoSaveLoad(true)]
        public ulong RoleId
        {
            get { return mRoleId; }
            set { mRoleId = value; }
        }

        ulong mGuildId = 0;
        [ServerFrame.DB.DBBindField("GuildId")]
        [RPC.AutoSaveLoad(true)]
        public ulong GuildId
        {
            get { return mGuildId; }
            set { mGuildId = value; }
        }

        byte mGuildPost = (byte)CSCommon.eGuildPost.None;
        [ServerFrame.DB.DBBindField("GuildPost")]
        [RPC.AutoSaveLoad(true)]
        public byte GuildPost
        {
            get { return mGuildPost; }
            set { mGuildPost = value; }
        }

        int mGuildContribute;
        [ServerFrame.DB.DBBindField("GuildContribute")]
        [RPC.AutoSaveLoad(true)]
        public int GuildContribute
        {
            get { return mGuildContribute; }
            set { mGuildContribute = value; }
        }

        int mTodayGuildContribute;
        [ServerFrame.DB.DBBindField("TodayGuildContribute")]
        [RPC.AutoSaveLoad(true)]
        public int TodayGuildContribute
        {
            get { return mTodayGuildContribute; }
            set { mTodayGuildContribute = value; }
        }

        DateTime mLastContributeTime;
        [ServerFrame.DB.DBBindField("LastContributeTime")]
        [RPC.AutoSaveLoad]
        public DateTime LastContributeTime
        {
            get { return mLastContributeTime; }
            set { mLastContributeTime = value; }
        }

        string mName;
        [ServerFrame.DB.DBBindField("Name")]
        [RPC.AutoSaveLoad(true)]
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        byte mSex;
        [ServerFrame.DB.DBBindField("Sex")]
        [RPC.AutoSaveLoad(true)]
        public byte Sex
        {
            get { return mSex; }
            set { mSex = value; }
        }

        UInt16 mLevel;
        [ServerFrame.DB.DBBindField("Level")]
        [RPC.AutoSaveLoad(true)]
        public UInt16 Level
        {
            get { return mLevel; }
            set { mLevel = value; }
        }

        Byte mCamp;
        [ServerFrame.DB.DBBindField("Camp")]
        [RPC.AutoSaveLoad]
        public Byte Camp
        {
            get { return mCamp; }
            set { mCamp = value; }
        }

        Byte mProfession;
        [ServerFrame.DB.DBBindField("Profession")]
        [RPC.AutoSaveLoad(true)]
        public Byte Profession
        {
            get { return mProfession; }
            set { mProfession = value; }
        }

        ushort mPlanesId;
        [ServerFrame.DB.DBBindField("PlanesId")]
        [RPC.AutoSaveLoad]
        public ushort PlanesId
        {
            get { return mPlanesId; }
            set { mPlanesId = value; }
        }

        string mMapName;
        [ServerFrame.DB.DBBindField("MapName")]
        [RPC.AutoSaveLoad]
        public string MapName
        {
            get { return mMapName; }
            set { mMapName = value; }
        }

        //世界拜访次数
        [ServerFrame.DB.DBBindField("WorldVisitCount")]
        [RPC.AutoSaveLoad]
        public byte WorldVisitCount { get; set; }

        //好友拜访次数
        [ServerFrame.DB.DBBindField("FriendVisitCount")]
        [RPC.AutoSaveLoad]
        public byte FriendVisitCount { get; set; }

        //发红包获取拜访的次数
        [ServerFrame.DB.DBBindField("BuyVisitCount")]
        [RPC.AutoSaveLoad]
        public byte BuyVisitCount { get; set; }

        //被拜访次数
        [ServerFrame.DB.DBBindField("ByVisitCount")]
        [RPC.AutoSaveLoad]
        public ushort ByVisitCount { get; set; }

        //最后一次拜访时间
        [ServerFrame.DB.DBBindField("LastVisitTime")]
        [RPC.AutoSaveLoad]
        public System.DateTime LastVisitTime { get; set; }

        //已拜访的玩家Id集合
        [ServerFrame.DB.DBBindField("VisitPlayers")]
        [RPC.AutoSaveLoad]
        public byte[] VisitPlayers { get; set; }

        [ServerFrame.DB.DBBindField("FinalValue")]
        [RPC.AutoSaveLoad(true)]
        public byte[] FinalValue { get; set; }
    }
}
