using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon
{

     [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class MapConnectInfo
    {
        ServerCommon.Data.Player.PlanesPlayerManager mPlanesPlayerManager;
        public ServerCommon.Data.Player.PlanesPlayerManager PlanesPlayerManager
        {
            get { return mPlanesPlayerManager; }
            set { mPlanesPlayerManager = value; }
        }
        public ServerFrame.NetEndPoint mConnect;
        public ServerFrame.NetEndPoint Connect
        {
            get { return mConnect; }
        }
        string mMapName;
        public string MapName
        {
            get { return mMapName; }
            set { mMapName = value; }
        }
        public ushort MapSourceId;
        public ulong MapInstanceId;

        public ulong InstanceCreator = 0;//副本谁创建的,也就是第一个进入部分的人

        Dictionary<ulong, CSCommon.Data.PlayerDataEx> mGuidDict = new Dictionary<ulong, CSCommon.Data.PlayerDataEx>();
        public Dictionary<ulong, CSCommon.Data.PlayerDataEx> GuidDict
        {
            get { return mGuidDict; }
        }
        Dictionary<string, CSCommon.Data.PlayerDataEx> mNameDict = new Dictionary<string, CSCommon.Data.PlayerDataEx>();
        public Dictionary<string, CSCommon.Data.PlayerDataEx> NameDict
        {
            get { return mNameDict; }
        }

        float DestroyTime = ServerCommon.GameSet.Instance.DungeonShutDownTime;

        public bool NeedDestroy()
        {
            return DestroyTime < 0;
        }

        public void AddPlayer(CSCommon.Data.PlayerDataEx pd)
        {
            mGuidDict[pd.RoleDetail.RoleId] = pd;
            mNameDict[pd.RoleDetail.RoleName] = pd;
            pd.CurMap = this;
        }

        public bool RemovePlayer(ulong roleId)
        {
            CSCommon.Data.PlayerDataEx pd;
            if (false == mGuidDict.TryGetValue(roleId, out pd))
            {
                return false;
            }
            mGuidDict.Remove(roleId);
            mNameDict.Remove(pd.RoleDetail.RoleName);
            pd.CurMap = null;
            if (mGuidDict.Count == 0)
            {
                DestroyTime = ServerCommon.GameSet.Instance.DungeonShutDownTime;
            }
            return true;
        }

        public void Tick()
        {
            if (mGuidDict.Count == 0)
            {
                DestroyTime -= ServerFrame.Time.delta; //(int)IServer.Instance.GetElapseMilliSecondTime();
            }
        }

        public CSCommon.Data.PlayerDataEx FindPlayerData(ulong roleId)
        {
            CSCommon.Data.PlayerDataEx pd;
            if (false == mGuidDict.TryGetValue(roleId, out pd))
            {
                return null;
            }
            return pd;
        }

        public CSCommon.Data.PlayerDataEx FindPlayerData(string roleName)
        {
            CSCommon.Data.PlayerDataEx pd;
            if (false == mNameDict.TryGetValue(roleName, out pd))
            {
                return null;
            }
            return pd;
        }
    }

    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class AllMapManager
    {
        static AllMapManager smInstance = new AllMapManager();
        public static AllMapManager Instance
        {
            get { return smInstance; }
        }

        Dictionary<string, MapConnectInfo> mGlobalMaps = new Dictionary<string, MapConnectInfo>();
        public Dictionary<string, MapConnectInfo> GlobalMaps
        {
            get { return mGlobalMaps; }
        }

        Dictionary<ulong, MapConnectInfo> mInstanceMaps = new Dictionary<ulong, MapConnectInfo>();
        public Dictionary<ulong, MapConnectInfo> InstanceMaps
        {
            get { return mInstanceMaps; }
        }


        public static bool IsInstanceMap(ushort mapSourceId)
        {
            var mapInit = Planes.MapInstanceManager.GetMapInitBySourceId(mapSourceId);
            if (mapInit == null)
                return false;

            return mapInit.IsInstance;
        }

        //开启世界地图、国战地图
        public void StartGlobalMap(ulong  aiPlanesSerId, ushort planesId, ushort mapSourceId)
        {
            string key = planesId.ToString() + ":" + mapSourceId.ToString();
            MapConnectInfo cinfo;
            if (false == mGlobalMaps.TryGetValue(key, out cinfo))
            {
                cinfo = new MapConnectInfo();
                cinfo.PlanesPlayerManager = IDataServer.Instance.PlayerManager.FindPlanesPlayerManager(planesId);

                //选择指定位面服务器
                var planesServers = IDataServer.Instance.PlanesServers;

                PlanesServerInfo selected = null;
                foreach (var kv in planesServers)
                {
                    if ( kv.Value.EndPoint.Id == aiPlanesSerId )
                    {
                        selected = kv.Value;
                        break;
                    }
                }
                if (selected == null)
                    return;
                cinfo.mConnect = selected.EndPoint;
                cinfo.MapSourceId = mapSourceId;
                cinfo.MapInstanceId = 0;

                mGlobalMaps.Add(key, cinfo);
            }
        }

        public MapConnectInfo GetMapConnectInfo(CSCommon.Data.PlayerDataEx pd, ushort planesId,ushort mapSourceId,ulong mapInstanceId)
        {
            if (IsInstanceMap(mapSourceId))
            {
                return GetInstanceMapConnectInfo(pd, mapSourceId, mapInstanceId);
            }
            else
            {
                return GetGlobalMapConnectInfo(planesId, mapSourceId);
            }
        }

        public MapConnectInfo GetGlobalMapConnectInfo(ushort planesId, ushort mapSourceId)
        {
            string key = planesId.ToString() + ":" + mapSourceId.ToString();
            MapConnectInfo cinfo;
            if (false == mGlobalMaps.TryGetValue(key, out cinfo))
            {
                cinfo = new MapConnectInfo();
                cinfo.PlanesPlayerManager = IDataServer.Instance.PlayerManager.FindPlanesPlayerManager(planesId);
                
                //评估一个压力小的服务器
                var planesServers = IDataServer.Instance.PlanesServers;
                Int32 minPlayerNumber = Int32.MaxValue;
                PlanesServerInfo selected = null;
                foreach (var kv in planesServers)
                {
                    if (kv.Value.PlayerNumber < minPlayerNumber)
                    {
                        selected = kv.Value;
                        minPlayerNumber = kv.Value.PlayerNumber;
                    }
                }
                if (selected == null)
                    return null;
                cinfo.mConnect = selected.EndPoint;
                cinfo.MapSourceId = mapSourceId;
                cinfo.MapInstanceId = 0;

                mGlobalMaps.Add(key, cinfo);
            }
            return cinfo;
        }
        private MapConnectInfo GetInstanceMapConnectInfo(CSCommon.Data.PlayerDataEx pd, ushort mapSourceId, ulong mapId)
        {
            MapConnectInfo cinfo;
            if (false == mInstanceMaps.TryGetValue(mapId, out cinfo))
            {
                //评估一个压力小的服务器
                var planesServers = IDataServer.Instance.PlanesServers;
                Int32 minPlanesNumber = Int32.MaxValue;
                PlanesServerInfo selected = null;
                foreach (var kv in planesServers)
                {
                    if (kv.Value.PlanesNumber < minPlanesNumber)
                    {
                        selected = kv.Value;
                        minPlanesNumber = kv.Value.PlanesNumber;
                    }
                }
                cinfo = new MapConnectInfo();
                cinfo.PlanesPlayerManager = null;
                cinfo.mConnect = selected.EndPoint;
                cinfo.MapSourceId = mapSourceId;
                cinfo.MapInstanceId = mapId;

                cinfo.InstanceCreator = pd.RoleDetail.RoleId;//如果是组队副本，这个地方存队伍Id

//                 var mapInit = Planes.MapInstanceManager.GetMapInitBySourceId(mapSourceId);
//                 if (mapInit != null)
//                 {
//                     switch ((CSCommon.eMapType)mapInit.MapData.mapType)
//                     {
//                         case CSCommon.eMapType.Master:
//                             cinfo.InstanceCreator = pd.RoleDetail.RoleId;//单人副本
//                             break;
//                         //case CSCommon.eMapType.TeamInstance:
//                         //cinfo.InstanceCreator = pd.RoleDetail.TeamId;//组队副本
//                         //   break;
//                     }
//                 }

                mInstanceMaps.Add(mapId, cinfo);
            }
            else
            {
                if (cinfo.MapSourceId != mapSourceId)
                {
                    Log.Log.Server.Info("副本Id错了 {0}:{1}!", mapSourceId, mapId);
                }
            }
            return cinfo;
        }


        //根据玩家和模板地图获得副本Id
        public ulong GetInstanceMapId(CSCommon.Data.PlayerData pd, ushort mapSourceId)
        {
            if (false == IsInstanceMap(mapSourceId))
                return 0;

            var mapInit = Planes.MapInstanceManager.GetMapInitBySourceId(mapSourceId);
            if (mapInit == null)
            {
                return 0;
            }

            return ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Map);
        }

        public void PlanesServerDisconnected(Iocp.TcpConnect connect)
        {
            List<string> removes = new List<string>();
            foreach (var i in mGlobalMaps)
            {
                if (i.Value.mConnect.Connect == connect)
                {
                    removes.Add(i.Key);
                }
            }
            foreach (var i in removes)
            {
                mGlobalMaps.Remove(i);
            }

            List<ulong> removeIns = new List<ulong>();
            foreach (var i in mInstanceMaps)
            {
                if (i.Value.mConnect.Connect == connect)
                {
                    removeIns.Add(i.Key);
                }
            }
            foreach (var i in removeIns)
            {
                mInstanceMaps.Remove(i);
            }
        }

        public void Tick()
        {
            try
            {
                var lst = new List<ulong>();
                foreach (var i in mInstanceMaps)
                {
                    i.Value.Tick();
                    if (i.Value.NeedDestroy())
                    {
                        lst.Add(i.Key);
                    }
                }

                foreach (var i in lst)
                {
                    MapConnectInfo map;
                    if (mInstanceMaps.TryGetValue(i, out map))
                    {
                        //这里要RPC告诉位面，关闭这个副本
                        var pkg = new RPC.PackageWriter();
                        H_RPCRoot.smInstance.HGet_PlanesServer(pkg).RemoveInstanceMap(pkg, map.MapInstanceId);
                        pkg.DoCommand(map.mConnect.Connect, RPC.CommandTargetType.DefaultType);
                        mInstanceMaps.Remove(i);
                    }
                }
            }
            catch(Exception e)
            {
                Log.Log.Server.Error(e.ToString());
            }

        }

        //开启planeserver对应的配置地图服务
        public void StartupWorldMap(int aiPlaneServerId)
        {
            if ( CSCommon.Data.CDbConfig.m_PlanesConfig.ContainsKey(aiPlaneServerId) )
            {
                foreach (CSCommon.Data.CAreaMap liAreaMap in CSCommon.Data.CDbConfig.m_PlanesConfig[aiPlaneServerId].ListMap)
                {
                    //如果是副本ID,不处理
                    if (IsInstanceMap((ushort)liAreaMap.Map))
                    {
                        continue;
                    }

                    StartGlobalMap((ulong)aiPlaneServerId, (ushort)liAreaMap.Area, (ushort)liAreaMap.Map);
                }                
            }
        }
    }

    
}
