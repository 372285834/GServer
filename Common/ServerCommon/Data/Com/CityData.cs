using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("CityData")]
    public class CityData : RPC.IAutoSaveAndLoad
    {
        ushort mPlaneid;
        [ServerFrame.DB.DBBindField("Planeid")]
        [RPC.AutoSaveLoad(true)]
        public ushort Planeid
        {
            get { return mPlaneid; }
            set { mPlaneid = value; }
        }

        byte mCamp;
        [ServerFrame.DB.DBBindField("Camp")]
        [RPC.AutoSaveLoad(true)]
        public byte Camp
        {
            get { return mCamp; }
            set { mCamp = value; }
        }

        ushort mMapid;
        [ServerFrame.DB.DBBindField("Mapid")]
        [RPC.AutoSaveLoad(true)]
        public ushort Mapid
        {
            get { return mMapid; }
            set { mMapid = value; }
        }

        byte[] mArms;
        [ServerFrame.DB.DBBindField("Arms")]
        [RPC.AutoSaveLoad(true)]
        public byte[] Arms
        {
            get { return mArms; }
            set { mArms = value; }
        }

    }
}
