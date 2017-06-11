using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    //寄售格子数据
    [ServerFrame.DB.DBBindTable("ConsignGridData")]
    public class ConsignGridData : RPC.IAutoSaveAndLoad
    {
        ulong mGirdId;
        [ServerFrame.DB.DBBindField("GirdId")]
        public ulong GirdId
        {
            get { return mGirdId; }
            set { mGirdId = value; }
        }

        ulong mOwnerId;
        [ServerFrame.DB.DBBindField("OwnerId")]
        public ulong OwnerId
        {
            get { return mOwnerId; }
            set { mOwnerId = value; }
        }

        ushort mPlanesId;
        [ServerFrame.DB.DBBindField("PlanesId")]
        public ushort PlanesId
        {
            get { return mPlanesId; }
            set { mPlanesId = value; }
        }

        int mTemplateId;
        [ServerFrame.DB.DBBindField("TemplateId")]
        public int TemplateId
        {
            get { return mTemplateId; }
            set { mTemplateId = value; }
        }

        int mStackNum;
        [ServerFrame.DB.DBBindField("StackNum")]
        public int StackNum
        {
            get { return mStackNum; }
            set { mStackNum = value; }
        }

        int mPrice;
        [ServerFrame.DB.DBBindField("Price")]
        public int Price
        {
            get { return mPrice; }
            set { mPrice = value; }
        }

        string mName;
        [ServerFrame.DB.DBBindField("Name")]
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        byte mType;
        [ServerFrame.DB.DBBindField("Type")]
        public byte Type
        {
            get { return mType; }
            set { mType = value; }
        }

        System.DateTime mDelTime;
        [ServerFrame.DB.DBBindField("DelTime")]
        public System.DateTime DelTime
        {
            get { return mDelTime; }
            set { mDelTime = value; }
        }

    }
}
