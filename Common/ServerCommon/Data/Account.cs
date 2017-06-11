using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{


    [ServerFrame.DB.DBBindTable("Account")]
    public class AccountInfo : RPC.IAutoSaveAndLoad
    {
        System.DateTime mCreateTime = System.DateTime.Now;
        [ServerFrame.DB.DBBindField("CreateTime")]
        [RPC.AutoSaveLoad]
        public System.DateTime CreateTime
        {
            get { return mCreateTime; }
            set { mCreateTime = value; }
        }
        string mUserName;
        [ServerFrame.DB.DBBindField("UserName")]
        [RPC.AutoSaveLoad]
        public string UserName
        {
            get { return mUserName; }
            set { mUserName = value; }
        }
        string mPassword;
        [ServerFrame.DB.DBBindField("Password")]
        [RPC.AutoSaveLoad]
        public string Password
        {
            get { return mPassword; }
            set { mPassword = value; }
        }
        ulong mId;
        [ServerFrame.DB.DBBindField("Id")]
        [RPC.AutoSaveLoad]
        public ulong Id
        {
            get { return mId; }
            set { mId = value; }
        }

        UInt16 mLimitLevel = (Byte)RPC.RPCExecuteLimitLevel.Player;
        [ServerFrame.DB.DBBindField("LimitLevel")]
        [RPC.AutoSaveLoad]
        public UInt16 LimitLevel
        {
            get { return mLimitLevel; }
            set { mLimitLevel = value; }
        }

        List<RoleInfo> mRoles = new List<RoleInfo>();
        public List<RoleInfo> Roles
        {
            get { return mRoles; }
        }

        public PlayerDataEx CurPlayer = null;
        Iocp.NetConnection mData2GateConnect;
        public Iocp.NetConnection Data2GateConnect
        {
            get { return mData2GateConnect; }
            set { mData2GateConnect = value; }
        }
        public AccountInfo DBClone()
        {
            AccountInfo retInfo = new AccountInfo();
            retInfo.UserName = this.UserName;
            retInfo.Password = this.Password;
            retInfo.Id = this.Id;
            retInfo.LimitLevel = this.LimitLevel;

            return retInfo;
        }


    }

}
