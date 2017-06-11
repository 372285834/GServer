using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    public class CAreaMap
    {
        //区
        int m_iArea;
        public int Area
        {
            get {return m_iArea;}
            set {m_iArea = value;}
        }

        //地图ID
        int m_iMap;
        public int Map
        {
            get { return m_iMap; }
            set { m_iMap = value; }
        }
    }


    [ServerFrame.DB.DBBindTable("planesconfig")]
    public class CPlanesConfig : RPC.IAutoSaveAndLoad
    {
        int m_iId;
        [ServerFrame.DB.DBBindField("F_Id")]
        [RPC.AutoSaveLoad]
        public int Id
        {
            get { return m_iId; }
            set { m_iId = value; }
        }

        /// <summary>
        /// 位面服务器ID
        /// </summary>
        int m_iPlanesServerId;
        [ServerFrame.DB.DBBindField("F_PlanesServerId")]
        [RPC.AutoSaveLoad]
        public int PlanesServerId
        {
            get { return m_iPlanesServerId; }
            set { m_iPlanesServerId = value; }
        }

        /// <summary>
        /// 地图ID列表
        /// </summary>
        string m_szMapId;
        [ServerFrame.DB.DBBindField("F_MapId")]
        [RPC.AutoSaveLoad]
        public string MapId
        {
            get { return m_szMapId; }
            set 
            { 
                m_szMapId = value;
                string[] lszArray = m_szMapId.Split('|');
                foreach( string lsz in lszArray )
                {
                    string[] lszAreaMap = lsz.Split(':');
                    if( 2 == lszAreaMap.Length )
                    {
                        CAreaMap loAreaMap = new CAreaMap();
                        int li = 0;
                        int.TryParse(lszAreaMap[0], out li);
                        loAreaMap.Area = li;
                        int.TryParse(lszAreaMap[1], out li);
                        loAreaMap.Map = li;
                        m_ListMap.Add(loAreaMap);
                    }
                }
            }
        }

        /// <summary>
        /// 地图ID列表
        /// </summary>
        List<CAreaMap> m_ListMap = new List<CAreaMap>();
        public List<CAreaMap> ListMap
        {
            get { return m_ListMap; }
        }

    }
}

