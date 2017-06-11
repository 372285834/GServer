using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCommon.Data
{

    public class ServerPartData : RPC.IAutoSaveAndLoad
    {

    }


    [ServerFrame.DB.DBBindTable("PlanesData")]
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class PlanesData : RPC.IAutoSaveAndLoad
    {
        public PlanesData()
        {
            mCreateTime = System.DateTime.Now;
        }
        System.DateTime mCreateTime;// = System.DateTime.Now;
        [ServerFrame.DB.DBBindField("CreateTime")]
        [RPC.AutoSaveLoad]
        public System.DateTime CreateTime
        {
            get { return mCreateTime; }
            set { mCreateTime = value; }
        }

        ushort mPlanesId;
        [ServerFrame.DB.DBBindField("PlanesId")]
        [RPC.AutoSaveLoad]
        public ushort PlanesId
        {
            get { return mPlanesId; }
            set { mPlanesId = value; }
        }
        string mPlanesName;
        [ServerFrame.DB.DBBindField("PlanesName")]
        [RPC.AutoSaveLoad]
        public string PlanesName
        {
            get { return mPlanesName; }
            set { mPlanesName = value; }
        }
        Byte mActiveState;
        [ServerFrame.DB.DBBindField("ActiveState")]
        [RPC.AutoSaveLoad]
        public Byte ActiveState
        {
            get { return mActiveState; }
            set { mActiveState = value; }
        }        
    }
    
}
