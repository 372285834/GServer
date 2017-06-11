using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace ServerFrame.DB
{
    public class DBBindField : Attribute
    {
        public string Field;
        public DBBindField(string fld)
        {
            Field = fld;
        }
    }

    public class DBBindTable : Attribute
    {
        public string Table;
        public DBBindTable(string tab)
        {
            Table = tab;
        }
    }

    public enum DBLogSender
    {
        Gate,
        Planes,
        Data,
    }

    public enum DBLogOpPrimary
    {
        MoneyChanged,
        ItemCreated,
        GuildCreated,
    }

    public enum DBLogOpSecond
    {
        BuyFromNPC,
        SellToNPC,
    }

    [ServerFrame.DB.DBBindTable("DBLogData")]
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class DBLogData : RPC.IAutoSaveAndLoad
    {
        ulong mLogId;
        [ServerFrame.DB.DBBindField("LogId")]
        public ulong LogId
        {
            get { return mLogId; }
            set { mLogId = value; }
        }

        Byte mLogSender;
        [ServerFrame.DB.DBBindField("LogSender")]
        public Byte LogSender
        {
            get { return mLogSender; }
            set { mLogSender = value; }
        }

        System.DateTime mTime;
        [ServerFrame.DB.DBBindField("Time")]
        public System.DateTime Time
        {
            get { return mTime; }
            set { mTime = value; }
        }

        UInt16 mOpPrimary;
        [ServerFrame.DB.DBBindField("OpPrimary")]
        public UInt16 OpPrimary
        {
            get { return mOpPrimary; }
            set { mOpPrimary = value; }
        }

        UInt16 mOpSecond;
        [ServerFrame.DB.DBBindField("OpSecond")]
        public UInt16 OpSecond
        {
            get { return mOpSecond; }
            set { mOpSecond = value; }
        }

        Int64 mValue;
        [ServerFrame.DB.DBBindField("Value")]
        public Int64 Value
        {
            get { return mValue; }
            set { mValue = value; }
        }

        string mDescription;
        [ServerFrame.DB.DBBindField("Description")]
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }

        Guid mRelativeId1 = Guid.Empty;
        [ServerFrame.DB.DBBindField("RelativeId1")]
        public Guid RelativeId1
        {
            get { return mRelativeId1; }
            set { RelativeId1 = value; }
        }

        Guid mRelativeId2 = Guid.Empty;
        [ServerFrame.DB.DBBindField("RelativeId2")]
        public Guid RelativeId2
        {
            get { return mRelativeId2; }
            set { mRelativeId2 = value; }
        }
    }


}
