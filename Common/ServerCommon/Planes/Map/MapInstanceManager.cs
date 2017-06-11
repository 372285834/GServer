using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{


    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class MapInstanceManager : Singleton<MapInstanceManager>
    {
        #region 地图模板数据处理
        public string DefaultMapName
        {
            get
            {
                var mapInit = GetMapInitBySourceId(GameSet.Instance.DefaultMapId);
                if (mapInit != null)
                {
                    return mapInit.MapData.name;
                }

                return "";
            }
        }

        static Dictionary<int, TableWrap.MapInfoData> mMapInitDictionary = new Dictionary<int, TableWrap.MapInfoData>();
        public static TableWrap.MapInfoData GetMapInitBySourceId(int mapSourceId)
        {
            lock (mMapInitDictionary)
            {
                TableWrap.MapInfoData outInit;
                if (mMapInitDictionary.TryGetValue(mapSourceId, out outInit))
                {
                    return outInit;
                }

                outInit = new TableWrap.MapInfoData();
                outInit.Init(mapSourceId);
                mMapInitDictionary[mapSourceId] = outInit;
                return outInit;
            }
        }

        #endregion

        MapInstance[] mAllMaps = new MapInstance[UInt16.MaxValue];
        public MapInstance[] AllMaps
        {
            get { return mAllMaps; }
        }

        public MapInstance GetMapInstance(UInt16 index)
        {
            if (index == UInt16.MaxValue)
                return null;
            return mAllMaps[index];
        }

        public MapInstance GetMapBySourceId(int sourceId)
        {
            for (var i = 0; i < mAllMaps.Count(); i++)
            {
                if (mAllMaps[i].MapSourceId == sourceId)
                    return mAllMaps[i];
            }
            return null;
        }

        PlanesManager mPlanesManager = new PlanesManager();
        public PlanesManager PlanesManager
        {
            get { return mPlanesManager; }
        }
        Dictionary<ulong, MapInstance> mInstanceMaps = new Dictionary<ulong, MapInstance>();
        public Dictionary<ulong, MapInstance> InstanceMaps
        {
            get { return mInstanceMaps; }
        }

        public void RemoveInstanceMap(ulong mapInstanceId)
        {
            lock (this)
            {
                MapInstance map;
                if (mInstanceMaps.TryGetValue(mapInstanceId, out map))
                {
                    mAllMaps[map.IndexInServer] = null;
                    map.OnMapClosed();
                    map.ReleaseInstanceMap();
                    mInstanceMaps.Remove(mapInstanceId);
                }
            }
        }

        public MapInstance GetDefaultMapInstance(PlanesInstance planes)
        {
            MapInstance map;
            ulong mapId = 0;
            if (mInstanceMaps.TryGetValue(mapId, out map))
                return map;
            for (UInt16 i = 0; i < UInt16.MaxValue; i++)
            {
                if (mAllMaps[i] == null)
                {
                    var info = MapInstanceManager.GetMapInitBySourceId(GameSet.Instance.DefaultMapId);
                    if (info == null)
                        continue;;

                    map = new WolrdMap();
                    if (map.InitMap(planes, i, 0, info,null))
                    {
                        mAllMaps[i] = map;
                        LogicProcessorManager.Instance.PushMap(map);
                        return map;
                    }
                }
            }

            return null;
        }

        public MapInstance CreateMapInstance(PlanesInstance planes, ulong mapInstanceId, ushort mapSourceId, PlayerInstance creater)
        {
            for (UInt16 i = 0; i < UInt16.MaxValue; i++)
            {
                if (mAllMaps[i] == null)
                {
                    var info = MapInstanceManager.GetMapInitBySourceId(mapSourceId);
                    if (info == null)
                        return new NullMapInstance();
                    var mapType = (CSCommon.eMapType)info.MapData.mapType;
                    MapInstance map = null;
                    switch (mapType)
                    {
                        case CSCommon.eMapType.World:
                            map = new WolrdMap();
                            break;
                        case CSCommon.eMapType.NULL:
                            map = new NullMapInstance();
                            break;
                        case CSCommon.eMapType.InstanceStart:
                            map = new NullMapInstance();
                            break;
                        case CSCommon.eMapType.Master:
                            map = new MasterInstance();
                            break;
                        case CSCommon.eMapType.Arena:
                            map = new ArenaInstance();
                            break;
                        case CSCommon.eMapType.Challenge:
                            map = new ChallengeInstance();
                            break;
                        case CSCommon.eMapType.BattelStart:
                            break;
                        default:
                            map = new NullMapInstance();
                            break;
                    }
                    if(map==null)
                        return new NullMapInstance();

                    if (map.InitMap(planes, i, mapInstanceId, info,creater))
                    {
                        mAllMaps[i] = map;
                        LogicProcessorManager.Instance.PushMap(map);
                        return map;
                    }
                    else
                    {
                        Log.FileLog.WriteLine("CreateMapInstance {0} Failed", mapSourceId);
                        return null;
                    }
                }
            }
            return null;
        }

        public void PlayerEnterMap(PlayerInstance player, ushort mapSourceId, ulong mapInstanceId, SlimDX.Vector3 pos, bool bSaveRole)
        {
            MapInstance map;
            lock (this)
            {
                if (AllMapManager.IsInstanceMap(mapSourceId))
                {
                    if (false == mInstanceMaps.TryGetValue(mapInstanceId, out map))
                    {
                        map = CreateMapInstance(PlanesInstance.NullPlanesInstance, mapInstanceId, mapSourceId, player);
                        if (map == null)
                        {
                            map = GetDefaultMapInstance(player.PlanesInstance);
                        }
                        mInstanceMaps.Add(mapInstanceId, map);
                    }
                }
                else
                {
                    var planes = player.PlanesInstance;
                    map = planes.GetGlobalMap(mapSourceId);
                    if (map == null)
                    {
                        map = CreateMapInstance(planes, 0, mapSourceId, player);
                        if (map == null)
                        {
                            map = GetDefaultMapInstance(planes);
                        }
                        planes.AddGlobalMap(mapSourceId, map);
                    }
                }
            }

            if (MapInstance.enEnterMapResult.Success != map.PlayerEnterMap(player, pos, true))
            {

            }
        }

        public void PlayerLeaveMap(PlayerInstance player, bool bSaveRole)
        {
            player.HostMap.PlayerLeaveMap(player, true);
        }
    }

}
