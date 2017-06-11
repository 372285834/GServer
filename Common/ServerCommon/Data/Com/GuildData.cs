using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("GuildCom")]
    public class GuildCom : RPC.IAutoSaveAndLoad
    {
        ulong mGuildId;
        [ServerFrame.DB.DBBindField("GuildId")]
        [RPC.AutoSaveLoad(true)]
        public ulong GuildId
        {
            get { return mGuildId; }
            set { mGuildId = value; }
        }

        ushort mPlanesId;
        [ServerFrame.DB.DBBindField("PlanesId")]
        [RPC.AutoSaveLoad]
        public ushort PlanesId
        {
            get { return mPlanesId; }
            set { mPlanesId = value; }
        }

        byte mCamp;
        [ServerFrame.DB.DBBindField("Camp")]
        [RPC.AutoSaveLoad]
        public byte Camp
        {
            get { return mCamp; }
            set { mCamp = value; }
        }

        string mGuildName;
        [ServerFrame.DB.DBBindField("GuildName")]
        [RPC.AutoSaveLoad(true)]
        public string GuildName
        {
            get { return mGuildName; }
            set { mGuildName = value; }
        }

        byte mLevel;
        [ServerFrame.DB.DBBindField("Level")]
        [RPC.AutoSaveLoad(true)]
        public byte Level
        {
            get { return mLevel; }
            set { mLevel = value; }
        }

        string mPresidentName;
        [ServerFrame.DB.DBBindField("PresidentName")]
        [RPC.AutoSaveLoad(true)]
        public string PresidentName
        {
            get { return mPresidentName; }
            set { mPresidentName = value; }
        }


        ulong mGuildExp;
        [ServerFrame.DB.DBBindField("GuildExp")]
        [RPC.AutoSaveLoad(true)]
        public UInt64 GuildExp
        {
            get { return mGuildExp; }
            set { mGuildExp = value; }
        }

        ulong mGuildGold;
        [ServerFrame.DB.DBBindField("GuildGold")]
        [RPC.AutoSaveLoad(true)]
        public UInt64 GuildGold
        {
            get { return mGuildGold; }
            set { mGuildGold = value; }
        }

        int mMemberNum;
        [ServerFrame.DB.DBBindField("MemberNum")]
        [RPC.AutoSaveLoad(true)]
        public int MemberNum
        {
            get { return mMemberNum; }
            set { mMemberNum = value; }
        }

        byte mGuildLandNum;
        [ServerFrame.DB.DBBindField("GuildLandNum")]
        [RPC.AutoSaveLoad(true)]
        public byte GuildLandNum
        {
            get { return mGuildLandNum; }
            set { mGuildLandNum = value; }
        }

        public GuildCom()
        {
            GuildId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Guild);
            Level = 1;
            GuildExp = 0;
            GuildGold = 0;
            GuildLandNum = 0;
        }

        public bool IsDirty = false;
    }
}
