using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    /*public class RefreshPoint
    {
        float m_positionX;
        public float PositionX
        {
            get { return m_positionX; }
            set { m_positionX = value; }
        }
        float m_positionY;
        public float PositionY
        {
            get { return m_positionY; }
            set { m_positionY = value; }
        }
        string m_name;
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        MapData mMapData;

        public void Init(System.Xml.XmlElement element, MapData mapData)
        {
            mMapData = mapData;
            foreach (System.Xml.XmlElement cElem in element.ChildNodes)
            {
                if (cElem.Name == "PositionX")
                {
                    PositionX = System.Convert.ToSingle(cElem.GetAttribute("Value"));
                }
                else if (cElem.Name == "PositionY")
                {
                    PositionY = mMapData.m_mapHeight - System.Convert.ToSingle(cElem.GetAttribute("Value"));
                }
                else if (cElem.Name == "Name")
                {
                    Name = cElem.GetAttribute("Value");
                }
            }
        }
    }*/
}
