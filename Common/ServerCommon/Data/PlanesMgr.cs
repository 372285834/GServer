using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon.Data
{
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class PlanesMgr
    {
        IDataServer mDataServer;
        Dictionary<ushort, CSCommon.Data.PlanesData> mPlanes = new Dictionary<ushort, CSCommon.Data.PlanesData>();
        public Dictionary<ushort, CSCommon.Data.PlanesData> Planes
        {
            get { return mPlanes; }
        }
        public PlanesMgr(IDataServer server)
        {
            mDataServer = server;
            //加载所有活跃位面
            string condition = "ActiveState>0";
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, new CSCommon.Data.PlanesData(), "");
            System.Data.DataTable tab = mDataServer.DBLoaderConnect._ExecuteSelect(dbOp, "CSCommon.Data.PlanesData");
            if (tab == null)
                return;

            foreach (System.Data.DataRow r in tab.Rows)
            {
                CSCommon.Data.PlanesData pd = new CSCommon.Data.PlanesData();
                if (false == ServerFrame.DB.DBConnect.FillObject(pd, r))
                    continue;
                mPlanes.Add(pd.PlanesId, pd);
            }
        }

//         public void CreatePlanesData(byte level,string name,byte state)
//         {
//             CSCommon.Data.PlanesData pd = new CSCommon.Data.PlanesData();
//             pd.PlanesId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Server);
//             pd.PlanesName = name;
//             pd.CreateTime = System.DateTime.Now;
//             pd.ActiveState = state;
// 
//             mPlanes.Add(pd.PlanesId, pd);
// 
//             string condition = "PlanesName=\'" + name + "\'";
//             ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, pd, true);
//             mDataServer.DBLoaderConnect._ExecuteInsert(dbOp);
//         }

        CSCommon.Data.PlanesData LoadPlanesFromDB(string planesName)
        {
            string condition = "ActiveState>0 && PlanesName = \'" + planesName + "\'";
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, new CSCommon.Data.PlanesData(), "");
            System.Data.DataTable tab = mDataServer.DBLoaderConnect._ExecuteSelect(dbOp, "CSCommon.Data.PlanesData");
            if (tab == null||tab.Rows.Count!=1)
                return null;

            CSCommon.Data.PlanesData pd = new CSCommon.Data.PlanesData();
            if (false == ServerFrame.DB.DBConnect.FillObject(pd, tab.Rows[0]))
                return pd;
            return null;
        }

        public RPC.DataWriter GetAllActivePlanesInfo()
        {
            RPC.DataWriter result = new RPC.DataWriter();
            UInt16 count = (UInt16)mPlanes.Count;
            result.Write(count);
            foreach (var i in mPlanes)
            {
                result.Write(i.Value.PlanesId);
                result.Write(i.Value.PlanesName);
            }
            return result;
        }

        public CSCommon.Data.PlanesData FindPlanesByName(string name)
        {
            foreach (var i in mPlanes)
            {
                if (i.Value.PlanesName == name)
                    return i.Value;
            }

            var pd = LoadPlanesFromDB(name);
            if (pd != null)
            {
                mPlanes.Add(pd.PlanesId, pd);
                return pd;
            }

            return null;
        }

        public CSCommon.Data.PlanesData RandomPlanes()
        {
            int index = CSCommon.Data.ItemData.Rand.Next(0, mPlanes.Count);
            int itt = 0;
            foreach (var i in mPlanes)
            {
                if (itt == index)
                    return i.Value;

                itt++;
            }
            return null;
        }

        public ServerFrame.NetEndPoint SelectLowPlanesForGlobalMap()
        {
            return null;
        }

        public ServerFrame.NetEndPoint SelectLowPlanesForInstanceMap()
        {
            return null;
        }

        public CSCommon.Data.PlanesData GetRolePlanesData(ushort planesId)
        {
            CSCommon.Data.PlanesData planesData;
            if (false == mPlanes.TryGetValue(planesId, out planesData))
                return null;
            return planesData;
        }
        
        public void OnPlanesSeverDisconnect(Iocp.TcpConnect connect)
        {
            //AllMapManager.Instance.Clear();
            AllMapManager.Instance.PlanesServerDisconnected(connect);
        }
    }

}
