using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    /*public class MapItemPreItemData
    {
        string mMapItemName = "";
        public string MapItemName
        {
            get { return mMapItemName; }
            set { mMapItemName = value; }
        }

        UInt16 mSceneNPCForceTemplateId = UInt16.MaxValue;
        [System.ComponentModel.DisplayName("战斗模板(.nScene)")]
        [System.ComponentModel.Category("战斗")]
        public UInt16 SceneNPCForceTemplateId
        {
            get { return mSceneNPCForceTemplateId; }
            set { mSceneNPCForceTemplateId = value; }
        }
    }

    public class MapItemData
    {
        public enum enMapItemDataType
        {
            Unknow,
            Jump,
            Fight,
        }
        enMapItemDataType mMapItemDataType = enMapItemDataType.Unknow;
        public enMapItemDataType MapItemDataType
        {
            get { return mMapItemDataType; }
            set { mMapItemDataType = value; }
        }

        string mMapItemName = "";
        public string MapItemName
        {
            get { return mMapItemName; }
            set { mMapItemName = value; }
        }

        List<MapItemPreItemData> mPreItems = new List<MapItemPreItemData>();
        public List<MapItemPreItemData> PreItems
        {
            get { return mPreItems; }
            set { mPreItems = value; }
        }

        string mJumpToMapName = "";
        [System.ComponentModel.DisplayName("跳转目标地图")]
        [System.ComponentModel.Category("跳转")]
        public string JumpToMapName
        {
            get { return mJumpToMapName; }
            set { mJumpToMapName = value; }
        }

        float mJumpToPositionX = 0;
        [System.ComponentModel.DisplayName("跳转目标位置X")]
        [System.ComponentModel.Category("跳转")]
        public float JumpToPositionX
        {
            get { return mJumpToPositionX; }
            set { mJumpToPositionX = value; }
        }

        float mJumpToPositionY = 0;
        [System.ComponentModel.DisplayName("跳转目标位置Y")]
        [System.ComponentModel.Category("跳转")]
        public float JumpToPositionY
        {
            get { return mJumpToPositionY; }
            set { mJumpToPositionY = value; }
        }

        UInt16 mSceneNPCForceTemplateId = UInt16.MaxValue;
        [System.ComponentModel.DisplayName("战斗模板(.nScene)")]
        [System.ComponentModel.Category("战斗")]
        public UInt16 SceneNPCForceTemplateId
        {
            get { return mSceneNPCForceTemplateId; }
            set { mSceneNPCForceTemplateId = value; }
        }

    }

    public class PolygonData
    {
        public enum enRegionType
        {
            PotalTrigger,
        }

        List<System.Drawing.PointF> m_pointList = new List<System.Drawing.PointF>();
        public List<System.Drawing.PointF> PointList
        {
            get { return m_pointList; }
            set { m_pointList = value; }
        }

        string m_name = "";
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        enRegionType mRegionType;
        public enRegionType RegionType
        {
            get { return mRegionType; }
        }

        List<UInt16> mSceneNPCForceTemplateList = new List<UInt16>();
        public List<UInt16> SceneNPCForceTemplateList
        {
            get { return mSceneNPCForceTemplateList; }
        }

        List<MapItemData> mMapItemDatas = new List<MapItemData>();
        public List<MapItemData> MapItemDatas
        {
            get { return mMapItemDatas; }
        }

        string mWorldMapCenterItemName = "";
        public string WorldMapCenterItemName
        {
            get { return mWorldMapCenterItemName; }
        }

        CSCommon.Data.MinMaxValue mLevelLimit = new CSCommon.Data.MinMaxValue();
        public CSCommon.Data.MinMaxValue LevelLimit
        {
            get { return mLevelLimit; }
        }

        MapData mMapData;

        public void Init(System.Xml.XmlElement element, MapData mapData)
        {
            mMapData = mapData;
            PointList.Clear();
            SceneNPCForceTemplateList.Clear();
            MapItemDatas.Clear();

            foreach (System.Xml.XmlElement cElem in element.ChildNodes)
            {
                if (cElem.Name == "PointList")
                {
                    foreach (System.Xml.XmlElement ppElement in cElem.ChildNodes)
                    {
                        var splits = ppElement.GetAttribute("Value").Split(',');
                        System.Drawing.PointF pt = new System.Drawing.PointF();
                        pt.X = System.Convert.ToSingle(splits[0]);
                        pt.Y = mMapData.m_mapHeight - System.Convert.ToSingle(splits[1]);
                        PointList.Add(pt);
                    }
                }
                else if (cElem.Name == "RegionType")
                {
                    mRegionType = (enRegionType)(System.Convert.ToInt32(cElem.GetAttribute("Value")));
                }
                else if (cElem.Name == "Name")
                {
                    Name = cElem.GetAttribute("Value");
                }
                else if (cElem.Name == "NPCForceList")
                {
                    foreach (System.Xml.XmlElement ppElement in cElem.ChildNodes)
                    {
                        SceneNPCForceTemplateList.Add(System.Convert.ToUInt16(ppElement.GetAttribute("Value")));
                    }
                }
                else if (cElem.Name == "MapItemList")
                {
                    foreach (System.Xml.XmlElement ccElement in cElem.ChildNodes)
                    {
                        MapItemData data = new MapItemData();
                        foreach (System.Xml.XmlElement dataElement in ccElement.ChildNodes)
                        {
                            if (dataElement.Name == "MapItemDataType")
                            {
                                data.MapItemDataType = (MapItemData.enMapItemDataType)(System.Convert.ToInt32(dataElement.GetAttribute("Value")));
                            }
                            else if (dataElement.Name == "MapItemName")
                            {
                                data.MapItemName = dataElement.GetAttribute("Value");
                            }
                            else if (dataElement.Name == "PreItems")
                            {
                                foreach (System.Xml.XmlElement preNameElement in dataElement.ChildNodes)
                                {
                                    MapItemPreItemData preItem = new MapItemPreItemData();
                                    foreach (System.Xml.XmlElement preNameDataElement in preNameElement.ChildNodes)
                                    {
                                        if (preNameDataElement.Name == "MapItemName")
                                        {
                                            preItem.MapItemName = preNameDataElement.GetAttribute("Value");
                                        }
                                        else if (preNameDataElement.Name == "SceneNPCForceTemplateId")
                                        {
                                            preItem.SceneNPCForceTemplateId = System.Convert.ToUInt16(preNameDataElement.GetAttribute("Value"));
                                        }
                                    }
                                    data.PreItems.Add(preItem);
                                }
                            }
                            else if (dataElement.Name == "JumpToMapName")
                            {
                                data.JumpToMapName = dataElement.GetAttribute("Value");
                            }
                            else if (dataElement.Name == "JumpToPositionX")
                            {
                                data.JumpToPositionX = System.Convert.ToSingle(dataElement.GetAttribute("Value"));
                            }
                            else if (dataElement.Name == "JumpToPositionY")
                            {
                                data.JumpToPositionY = System.Convert.ToSingle(dataElement.GetAttribute("Value"));
                            }
                            else if (dataElement.Name == "SceneNPCForceTemplateId")
                            {
                                data.SceneNPCForceTemplateId = System.Convert.ToUInt16(dataElement.GetAttribute("Value"));
                            }
                        }

                        MapItemDatas.Add(data);
                    }
                }
                else if (cElem.Name == "WorldMapCenterItemName")
                {
                    mWorldMapCenterItemName = cElem.GetAttribute("Value");
                }
                else if (cElem.Name == "LevelLimit")
                {
                    foreach (System.Xml.XmlElement lElement in cElem.ChildNodes)
                    {
                        if (lElement.Name == "Min")
                        {
                            mLevelLimit.Min = System.Convert.ToDouble(lElement.GetAttribute("Value"));
                        }
                        else if(lElement.Name == "Max")
                        {
                            mLevelLimit.Max = System.Convert.ToDouble(lElement.GetAttribute("Value"));
                        }
                    }
                }
            }
        }
    }*/
}
