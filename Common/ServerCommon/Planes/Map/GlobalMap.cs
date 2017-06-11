using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes.Map
{
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class GlobalMap
    {
        List<MapInstance> mMaps = new List<MapInstance>();
        public bool InitGlobalMap(System.Guid mapSourceId, PlanesInstance planes)
        {
            MapInstance map = MapManager.Instance.CreateMap();
            map.InitMap(planes, Guid.Empty, mapSourceId);
            mMaps.Add(map);
            LogicProcessorManager.Instance.PushMap(map);
            return true;
        }
        public List<MapInstance> Maps
        {
            get { return mMaps; }
        }
        public MapInstance GetNotFullMap()
        {
            foreach (var i in mMaps)
            {
                if (i.IsFull == false)
                    return i;
            }

            MapInstance map = new MapInstance();
            //map.InitInstanceZones()
            mMaps.Add(map);
            LogicProcessorManager.Instance.PushMap(map);
            return map;
        }
    }
}
