using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableWrap
{
    public class MapInfoData
    {
        public CSTable.MapsData MapData;
        public CSCommon.MapInfo MapDetail;

        public bool IsInstance
        {
            get
            {
                return MapData.mapType >= (int)CSCommon.eMapType.InstanceStart;
            }
        }

        public bool Init(int mapid)
        {
            if (!CSTable.StaticDataManager.Maps.Dict.ContainsKey(mapid))
            {
                Log.Log.Table.Warning("can't find map table id={0}", mapid);
                return false;
            }

            MapData = CSTable.StaticDataManager.Maps.Dict[mapid];
            var path = ServerCommon.ServerConfig.Instance.MapInfoPath + mapid.ToString() + ".mapinfo";
            if (!System.IO.File.Exists(path))
            {
                MapDetail = new CSCommon.MapInfo();
                return true; ;
            }
            using (System.IO.FileStream sr = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(CSCommon.MapInfo));
                MapDetail = xml.Deserialize(sr) as CSCommon.MapInfo;
            }

            //GameEngine.LevelManager lm = GameEngine.LevelManager.getSingletonPtr();
            //GameEngine.Level lvl = lm.getResourceByName(mapid.ToString() + ".xml").get<GameEngine.Level>();
//             if (lvl != null)
//             {
//                 lvl.load();
// 
//                 // npc
//                 foreach (var p in lvl.getObjects())
//                 {
//                     if (p.Key == GameEngine.NpcFactory.FACTORY_TYPE_NAME)
//                     {
//                         foreach (var obj in p.Value)
//                         {
//                             GameEngine.NpcInfo npc = (GameEngine.NpcInfo)obj;
// 
//                             //npc.dataId;
//                         }
//                     }
//                 }
// 
//                 // 触发器
//                 foreach (var p in lvl.getTriggerList())
//                 {
//                     if (p.getType() == GameEngine.TriggerSphereFactory.FACTORY_TYPE_NAME)
//                     {
//                         GameEngine.TriggerSphere sphere = (GameEngine.TriggerSphere)p;
// 
//                         //sphere.getPosition();
//                         //sphere.getRadius();
// 
//                         foreach (var act in sphere.getTrueActions())
//                         {
//                             if (act.getType() == GameEngine.EventType.EVE_PORTAL)
//                             {
//                                 GameEngine.EventPortal portal = (GameEngine.EventPortal)act;
//                                 //portal.getLevelName();
//                                 //portal.getPosition();
//                             }
//                         }
//                     }
//                 }
//             }
 
            return true;
        }
    }
}
