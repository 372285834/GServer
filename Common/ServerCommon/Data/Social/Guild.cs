using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("GuildData")]
    public class GuildInfo : RPC.IAutoSaveAndLoad
    {
        byte mDeleted;
        [ServerFrame.DB.DBBindField("Deleted")]
        public byte Deleted
        {
            get { return mDeleted; }
            set
            {
                mDeleted = value;
            }
        }

        ulong mGuildId;//帮会ID
        [ServerFrame.DB.DBBindField("GuildId")]
        public ulong GuildId
        {
            get { return mGuildId; }
            set
            {
                mGuildId = value;
            }
        }

        DateTime mCreateTime;//创建时间
        [ServerFrame.DB.DBBindField("CreateTime")]
        public DateTime CreateTime
        {
            get { return mCreateTime; }
            set
            {
                mCreateTime = value;
            }
        }

        ulong mLeaderId;//会长ID
        [ServerFrame.DB.DBBindField("LeaderId")]
        public ulong LeaderId
        {
            get { return mLeaderId; }
            set
            {
                mLeaderId = value;
            }
        }

        ushort mPlanesId;//位面ID
        [ServerFrame.DB.DBBindField("PlanesId")]
        public ushort PlanesId
        {
            get { return mPlanesId; }
            set
            {
                mPlanesId = value;
            }
        }

        string mName;//帮会名
        [ServerFrame.DB.DBBindField("Name")]
        public string Name
        {
            get { return mName; }
            set
            {
                mName = value;
            }
        }

        int mGuildLevel;//帮会等级
        [ServerFrame.DB.DBBindField("GuildLevel")]
        public int GuildLevel
        {
            get { return mGuildLevel; }
            set
            {
                mGuildLevel = value;
            }
        }

        string mMembers;//成员
        [ServerFrame.DB.DBBindField("Members")]
        public string Members
        {
            get { return mMembers; }
            set
            {
                mMembers = value;
            }
        }

        UInt64 mRoleGuildFunds;//角色帮会资金
        [ServerFrame.DB.DBBindField("RoleGuildFunds")]
        public UInt64 RoleGuildFunds
        {
            get { return mRoleGuildFunds; }
            set
            {
                mRoleGuildFunds = value;
            }
        }

        UInt64 mRoleGuildProsperity;//角色帮会繁荣
        [ServerFrame.DB.DBBindField("RoleGuildProsperity")]
        public UInt64 RoleGuildProsperity
        {
            get { return mRoleGuildProsperity; }
            set
            {
                mRoleGuildProsperity = value;
            }
        }

        UInt64 mRoleGuildExp;//角色帮会经验
        [ServerFrame.DB.DBBindField("RoleGuildExp")]
        public UInt64 RoleGuildExp
        {
            get { return mRoleGuildExp; }
            set
            {
                mRoleGuildExp = value;
            }
        }

    }
}
