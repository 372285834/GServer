using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    /*public class BornPoint
    {
        float m_positionX = 0;
        public float PositionX
        {
            get { return m_positionX; }
            set { m_positionX = value; }
        }
        float m_positionY = 0;
        public float PositionY
        {
            get { return m_positionY; }
            set { m_positionY = value; }
        }
        string m_name = "";
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        double mRadius = 30;
        public double Radius
        {
            get { return mRadius; }
            set { mRadius = value; }
        }
        MapData mMapData;

        Random mRand = new Random();

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
                else if (cElem.Name == "Radius")
                {
                    Radius = System.Convert.ToDouble(cElem.GetAttribute("Value"));
                }
            }
        }

        public float GetRandomPositionX()
        {
            return (float)(PositionX + mRand.NextDouble() * Radius * 2 - Radius);
        }

        public float GetRandomPositionY()
        {
            return (float)(PositionY + mRand.NextDouble() * Radius * 2 - Radius);
        }
    }*/
}
