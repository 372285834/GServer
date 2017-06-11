using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("MailData")]
    public class MailData : RPC.IAutoSaveAndLoad
    {
        [ServerFrame.DB.DBBindField("Deleted")]
        public byte Deleted { get; set; }

        byte mState = (byte)CSCommon.eMailState.UnOpen;
        [ServerFrame.DB.DBBindField("State")]
        [RPC.AutoSaveLoad(true)]
        public byte State
        {
            get { return mState; }
            set { mState = value; }
        }

        ulong mMailId = 0;
        [ServerFrame.DB.DBBindField("MailId")]
        [RPC.AutoSaveLoad(true)]
        public ulong MailId
        {
            get { return mMailId; }
            set { mMailId = value; }
        }
        ulong mOwnerId = 0;
        [ServerFrame.DB.DBBindField("OwnerId")]
        [RPC.AutoSaveLoad]
        public ulong OwnerId
        {
            get { return mOwnerId; }
            set { mOwnerId = value; }
        }

        System.DateTime mCreateTime = System.DateTime.Now;
        [ServerFrame.DB.DBBindField("CreateTime")]
        [RPC.AutoSaveLoad(true)]
        public System.DateTime CreateTime
        {
            get { return mCreateTime; }
            set { mCreateTime = value; }
        }

        System.DateTime mEndTime;
        [ServerFrame.DB.DBBindField("EndTime")]
        [RPC.AutoSaveLoad(true)]
        public System.DateTime EndTime
        {
            get { return mEndTime; }
            set { mEndTime = value; }
        }

        string mType = "";
        [ServerFrame.DB.DBBindField("Type")]
        [RPC.AutoSaveLoad(true)]
        public string Type
        {
            get { return mType; }
            set { mType = value; }
        }

        string mTitle="";
        [ServerFrame.DB.DBBindField("Title")]
        [RPC.AutoSaveLoad(true)]
        public string Title
        {
            get { return mTitle; }
            set { mTitle = value; }
        }

        string mContentStr="";
        [ServerFrame.DB.DBBindField("ContentStr")]
        [RPC.AutoSaveLoad(true)]
        public string ContentStr
        {
            get { return mContentStr; }
            set { mContentStr = value; }
        }

        //物品集合
        string mStrItems = "";                                       
        [ServerFrame.DB.DBBindField("StrItems")]
        [RPC.AutoSaveLoad(true)]
        public string StrItems
        {
            get { return mStrItems; }
            set { mStrItems = value; }
        }

        //货币集合
        string mStrCurrencies = "";
        [ServerFrame.DB.DBBindField("StrCurrencies")]
        [RPC.AutoSaveLoad(true)]
        public string StrCurrencies
        {
            get { return mStrCurrencies; }
            set { mStrCurrencies = value; }
        }

    }

    [ServerFrame.DB.DBBindTable("SystemMailData")]
    public class SystemMailData : RPC.IAutoSaveAndLoad
    {
        [ServerFrame.DB.DBBindField("MailId")]
        public ulong MailId { get; set; }

        [ServerFrame.DB.DBBindField("CreateTime")]
        [RPC.AutoSaveLoad(true)]
        public System.DateTime CreateTime { get; set; }

        [ServerFrame.DB.DBBindField("EndTime")]
        public System.DateTime EndTime { get; set; }

        [ServerFrame.DB.DBBindField("Type")]
        public string Type { get; set; }

        [ServerFrame.DB.DBBindField("Title")]
        public string Title { get; set; }

        [ServerFrame.DB.DBBindField("ContentStr")]
        public string ContentStr { get; set; }

        //物品集合
        [ServerFrame.DB.DBBindField("StrItems")]
        public string StrItems { get; set; }

        //货币集合
        [ServerFrame.DB.DBBindField("StrCurrencies")]
        public string StrCurrencies { get; set; }

        //查询条件
        [ServerFrame.DB.DBBindField("SelectSql")]
        public string SelectSql { get; set; }

    }
}
