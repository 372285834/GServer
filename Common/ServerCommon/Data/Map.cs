using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    //[ServerFrame.Editor.CDataEditorAttribute(".MapInit")]
    public class MapInit
    {
        protected string mSceneName = "";
        [ServerFrame.Config.DataValueAttribute("SceneName")]
        public string SceneName
        {
            get { return mSceneName; }
            set { mSceneName = value; }
        }
       
        protected UInt32 mSceneSizeX = 1;
        [ServerFrame.Config.DataValueAttribute("SceneSizeX")]
        public UInt32 SceneSizeX
        {
            get { return mSceneSizeX; }
            set { mSceneSizeX = value; }
        }
        protected UInt32 mSceneSizeZ = 1;
        [ServerFrame.Config.DataValueAttribute("SceneSizeZ")]
        public UInt32 SceneSizeZ
        {
            get { return mSceneSizeZ; }
            set { mSceneSizeZ = value; }
        }      

        protected System.UInt16 mMaxPlayerCount = 1000;
        public System.UInt16 MaxPlayerCount
        {
            get { return mMaxPlayerCount; }
            set { mMaxPlayerCount = value; }
        }
    }

    /*public class MapData
    {
        //public enum enMapType
        //{
        //    DongHaiYuCun,    // 东海渔村
        //    Changan,        // 长安
        //    LuoYang,        // 洛阳
        //    WuZhiShan,      // 五指山
        //    ChangShouCun,   // 长寿村

        //    InstanceBegin = 64,//副本开始
        //    InstanceEnd = 255,//副本结束
        //}

        //public static string MapType2MapName(enMapType type)
        //{
        //    return MapConfig.Instance.GetMapData(type).MapName;
        //}

        //public static enMapType MapName2MapType(string name)
        //{
        //    foreach (var i in MapConfig.Instance.MapDatas)
        //    {
        //        if(i.MapName == name)
        //            return i.MapType;
        //    }
        //    return enMapType.DongHaiYuCun;
        //}

        //public static bool IsInstance(enMapType type)
        //{
        //    return type >= enMapType.InstanceBegin && type <= enMapType.InstanceEnd;
        //}

        //enMapType m_mapType;
        //[ServerFrame.Config.DataValueAttribute("MapType")]
        //[System.ComponentModel.DisplayName("枚举")]
        //public enMapType MapType
        //{
        //    get { return m_mapType; }
        //    set { m_mapType = value; }
        //}

        bool m_bPreCreate;
        [ServerFrame.Config.DataValueAttribute("PreCreate")]
        [System.ComponentModel.DisplayName("预创建")]
        public bool PreCreate
        {
            get { return m_bPreCreate; }
            set { m_bPreCreate = value; }
        }

        System.UInt16 m_maxPlayerNum = 4096;
        [ServerFrame.Config.DataValueAttribute("MaxPlayerNum")]
        [System.ComponentModel.DisplayName("玩家最大数量")]
        public System.UInt16 MaxPlayerNum
        {
            get { return m_maxPlayerNum; }
            set { m_maxPlayerNum = value; }
        }

        string m_mapName;
        [ServerFrame.Config.DataValueAttribute("MapName")]
        [System.ComponentModel.DisplayName("地图名称")]
        public string MapName
        {
            get { return m_mapName; }
            set { m_mapName = value; }
        }

        string m_mapDataFile;
        [ServerFrame.Config.DataValueAttribute("MapDataFile")]
        [System.ComponentModel.DisplayName("地图数据文件")]
        public string MapDataFile
        {
            get { return m_mapDataFile; }
            set
            {
                m_mapDataFile = value;

                //InitializeMap();
            }
        }

        float m_cellWidthInServer = 500;
        [ServerFrame.Config.DataValueAttribute("CellWidthInServer")]
        [System.ComponentModel.DisplayName("服务器端地图格宽度")]
        public float CellWidthInServer
        {
            get { return m_cellWidthInServer; }
            set { m_cellWidthInServer = value; }
        }

        float m_cellHeightInServer = 500;
        [ServerFrame.Config.DataValueAttribute("CellHeightInServer")]
        [System.ComponentModel.DisplayName("服务器端地图格高度")]
        public float CellHeightInServer
        {
            get { return m_cellHeightInServer; }
            set { m_cellHeightInServer = value; }
        }

        string m_BGMusic = "";
        [System.ComponentModel.Editor(typeof(CSCommon.Editor.FileStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [ServerFrame.Config.DataValueAttribute("Music")]
        [System.ComponentModel.DisplayName("背景音乐")]
        public string Music
        {
            get { return m_BGMusic; }
            set { m_BGMusic = value; }
        }


        #region 地图基础数据

        public float m_mapWidth;
        public float m_mapHeight;
        public List<NPCData> m_npcs = new List<NPCData>();
        public List<PolygonData> m_polygonDatas = new List<PolygonData>();
        public List<RefreshPoint> m_refreshPoints = new List<RefreshPoint>();
        public BornPoint mBornPoint = new BornPoint();

        #endregion

        public override string ToString()
        {
            return m_mapName;
        }

        public void InitializeMap()
        {
            try
            {
                var file = "../" + MapDataFile;
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(file);

                System.Xml.XmlElement node = doc.DocumentElement.FirstChild.FirstChild as System.Xml.XmlElement;
                m_mapWidth = System.Convert.ToSingle(((System.Xml.XmlElement)(node.GetElementsByTagName("BackgroundWidth")[0])).GetAttribute("Value"));
                m_mapHeight = System.Convert.ToSingle(((System.Xml.XmlElement)(node.GetElementsByTagName("BackgroundHeight")[0])).GetAttribute("Value"));

                // DataList 中其他数据读取
                System.Xml.XmlElement element = (System.Xml.XmlElement)node.GetElementsByTagName("DataList")[0];
                foreach (System.Xml.XmlElement cElem in element.ChildNodes)
                {
                    var strType = cElem.GetAttribute("Type");
                    switch(strType)
                    {
                        case "SceneEditor.exe@SceneEditor.ControlsData.NPCData":
                            {
                                NPCData npc = new NPCData();                                
                            }
                            break;

                        case "SceneEditor.exe@SceneEditor.ControlsData.PolygonData":
                            {
                                PolygonData poly = new PolygonData();
                                poly.Init(cElem, this);
                                m_polygonDatas.Add(poly);
                            }
                            break;

                        case "SceneEditor.exe@SceneEditor.ControlsData.RefreshPointData":
                            {
                                RefreshPoint rp = new RefreshPoint();
                                rp.Init(cElem, this);
                                m_refreshPoints.Add(rp);
                            }
                            break;
                        case "SceneEditor.exe@SceneEditor.ControlsData.BornPointData":
                            {
                                // 出生点只有一个
                                BornPoint bp = new BornPoint();
                                bp.Init(cElem, this);
                                mBornPoint = bp;
                            }
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
            	Log.FileLog.WriteLine("地图(" + m_mapName + ")数据读取失败\r\n" + ex.ToString());
            }
        }
    }*/

    //地图入口数据，用来地图存盘或服务器传送到客户端构造传送门
    public class MapPotal : RPC.IAutoSaveAndLoad
    {
        string mFromMap = "";
        [ServerFrame.Config.DataValueAttribute("FromMap")]
        [System.ComponentModel.DisplayName("跳转起始地图")]
        public string FromMap
        {
            get { return mFromMap; }
            set { mFromMap = value; }
        }

        string mToMap = "";
        [ServerFrame.Config.DataValueAttribute("ToMap")]
        [System.ComponentModel.DisplayName("跳转目标地图")]
        public string ToMap
        {
            get { return mToMap; }
            set { mToMap = value; }
        }

        SlimDX.Vector3 mTargetPoint;
        [ServerFrame.Config.DataValueAttribute("TargetPoint")]
        public SlimDX.Vector3 TargetPoint
        {
            get { return mTargetPoint; }
            set { mTargetPoint = value; }
        }

        int mMinLevel = 0;
        [ServerFrame.Config.DataValueAttribute("MinLevel")]
        [System.ComponentModel.DisplayName("通过的等级限制下限")]
        public int MinLevel
        {
            get { return mMinLevel; }
            set { mMinLevel = value; }
        }

        int mMaxLevel = int.MaxValue;
        [ServerFrame.Config.DataValueAttribute("MaxLevel")]
        [System.ComponentModel.DisplayName("通过的等级限制下限")]
        public int MaxLevel
        {
            get { return mMaxLevel; }
            set { mMaxLevel = value; }
        }

        List<UInt16> mNeedFinishedTasks = new List<UInt16>();
        [ServerFrame.Config.DataValueAttribute("NeedFinishedTasks")]
        [System.ComponentModel.DisplayName("通过需要完成的任务列表")]
        public List<UInt16> NeedFinishedTasks
        {
            get { return mNeedFinishedTasks; }
            set { mNeedFinishedTasks = value; }
        }
    }

    /*[Editor.CDataEditorAttribute(".mcg")]
    public class MapConfig
    {
        List<MapData> m_mapDatas = new List<MapData>();
        [ServerFrame.Config.DataValueAttribute("MapDatas")]
        public List<MapData> MapDatas
        {
            get { return m_mapDatas; }
            set { m_mapDatas = value; }
        }

        static MapConfig smConfig = new MapConfig();
        public static MapConfig Instance
        {
            get { return smConfig; }
        }

        string m_defaultMapName = "";
        [ServerFrame.Config.DataValueAttribute("DefaultMapName")]
        public string DefaultMapName
        {
            get { return m_defaultMapName; }
            set { m_defaultMapName = value; }
        }

        List<MapPotal> mPotals = new List<MapPotal>();
        [ServerFrame.Config.DataValueAttribute("Potals")]
        public List<MapPotal> Potals
        {
            get { return mPotals; }
            set { mPotals = value; }
        }

        string mExclamationPoint = "";
        [System.ComponentModel.Editor(typeof(CSCommon.Editor.FileStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [System.ComponentModel.DisplayName("感叹号")]
        [ServerFrame.Config.DataValueAttribute("ExclamationPoint")]
        public string ExclamationPoint
        {
            get { return mExclamationPoint; }
            set { mExclamationPoint = value; }
        }

        string mExclamationPointDisable = "";
        [System.ComponentModel.Editor(typeof(CSCommon.Editor.FileStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [System.ComponentModel.DisplayName("感叹号灰")]
        [ServerFrame.Config.DataValueAttribute("ExclamationPointDisable")]
        public string ExclamationPointDisable
        {
            get { return mExclamationPointDisable; }
            set { mExclamationPointDisable = value; }
        }

        string mQuestionMark = "";
        [System.ComponentModel.Editor(typeof(CSCommon.Editor.FileStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [System.ComponentModel.DisplayName("问号")]
        [ServerFrame.Config.DataValueAttribute("QuestionMark")]
        public string QuestionMark
        {
            get { return mQuestionMark; }
            set { mQuestionMark = value; }
        }

        string mQuestionMaskDisable = "";
        [System.ComponentModel.Editor(typeof(CSCommon.Editor.FileStringEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [System.ComponentModel.DisplayName("问号灰")]
        [ServerFrame.Config.DataValueAttribute("QuestionMaskDisable")]
        public string QuestionMaskDisable
        {
            get { return mQuestionMaskDisable; }
            set { mQuestionMaskDisable = value; }
        }

        //float mDefaultBornPosX = 0;
        //[ServerFrame.Config.DataValueAttribute("DefaultBornPosX")]
        //public float DefaultBornPosX
        //{
        //    get { return mDefaultBornPosX; }
        //    set { mDefaultBornPosX = value; }
        //}

        //float mDefaultBornPosY = 0;
        //[ServerFrame.Config.DataValueAttribute("DefaultBornPosY")]
        //public float DefaultBornPosY
        //{
        //    get { return mDefaultBornPosY; }
        //    set { mDefaultBornPosY = value; }
        //}

        public MapConfig()
        {
            LoadConfig("ZeusGame/Map/MapConfig.mcg");
        }

        public void LoadConfig(string strFile)
        {
            string fileName = strFile;
            ServerFrame.Config.IConfigurator.FillProperty(this, fileName);
        }

        public MapData GetMapData(string mapName)
        {
            foreach (var data in MapDatas)
            {
                if (data.MapName == mapName)
                    return data;
            }

            return null;
        }

        //public MapData GetMapData(MapData.enMapType mapType)
        //{
        //    foreach (var data in MapDatas)
        //    {
        //        if (data.MapType == mapType)
        //            return data;
        //    }

        //    return null;
        //}

        public MapData GetDefaultMapData()
        {
            return GetMapData(DefaultMapName);
        }
    }*/
}
