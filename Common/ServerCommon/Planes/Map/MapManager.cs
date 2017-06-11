using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes.Map
{
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class PlanesMapManager
    {
        public PlanesInstance mPlanesInstance;
        public void InitPlanes(CSCommon.Data.PlanesData planesData)
        {
            mPlanesInstance = MapManager.Instance.CreatePlanes();
            mPlanesInstance.InitPlanesInstance(planesData);
        }
        Dictionary<System.Guid, GlobalMap> mMaps = new Dictionary<System.Guid, GlobalMap>();
        public Dictionary<System.Guid, GlobalMap> Maps
        {
            get { return mMaps; }
        }

        public MapInstance GetGlobalMap(System.Guid mapSourceId)
        {
            GlobalMap map;
            if (false == mMaps.TryGetValue(mapSourceId, out map))
            {
                map = new GlobalMap();
                map.InitGlobalMap(mapSourceId, mPlanesInstance);
                mMaps.Add(mapSourceId, map);
            }
            return map.GetNotFullMap();
        }
    }

    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class MapManager
    {
        public const Byte PlanesNumMax = 255;
        public const UInt16 MapNumMax = 8192;
        public string DefaultMapName
        {
            get
            {
                var mapInit = GetMapInitBySourceId(CSUtility.Support.IFileConfig.Instance.DefaultMapId);
                if (mapInit != null)
                {
                    return mapInit.SceneName;
                }

                return "";
            }
        }
        
        static MapManager smInstance = new MapManager();
        public static MapManager Instance
        {
            get { return smInstance; }
        }

        PlanesInstance[] mPlanesInstance = new PlanesInstance[PlanesNumMax];

        MapInstance[] mMapInstance = new MapInstance[MapNumMax];
        public MapInstance[] MapInstance
        {
            get { return mMapInstance; }
        }
        
        public int TotalPlayerNumber
        {
            get
            {
                int result = 0;
                foreach (var i in mPlanesInstance)
                {
                    if (i != null)
                        result += i.RoleIdManager.GetPlayerCount();
                }
                return result;
            }
        }

        public int PlanesNumber
        {
            get
            {
                int result = 0;
                foreach (Planes.PlanesInstance p in mPlanesInstance)
                {
                    if (p == null)
                        continue;
                    result++;
                }
                return result;
            }
        }

        public PlanesInstance CreatePlanes()
        {
            for (Byte i = 0; i < PlanesNumMax; i++)
            {
                if (mPlanesInstance[i] == null)
                {
                    PlanesInstance planes = new PlanesInstance();
                    mPlanesInstance[i] = planes;
                    planes.IndexInPlanesServer = i;
                    RPC.PackageWriter pkg = new RPC.PackageWriter();
                    H_RPCRoot.smInstance.HGet_DataServer(pkg).UpdatePlanesServerPlanesNumber(pkg,(int)i);
                    pkg.DoCommand(IPlanesServer.Instance.DataConnect, RPC.CommandTargetType.DefaultType);
                    return planes;
                }
            }
            return null;
        }

        public MapInstance CreateMap()
        {
            for (UInt16 i = 0; i < MapNumMax; i++)
            {
                if (mMapInstance[i] == null)
                {
                    MapInstance map = new MapInstance();
                    mMapInstance[i] = map;
                    map.IndexInPlanesServer = i;

                    RPC.PackageWriter pkg = new RPC.PackageWriter();
                    H_RPCRoot.smInstance.HGet_DataServer(pkg).UpdatePlanesServerGlobalMapNumber(pkg, (int)i);
                    pkg.DoCommand(IPlanesServer.Instance.DataConnect, RPC.CommandTargetType.DefaultType);
                    return map;
                }
            }
            return null;
        }

        public PlanesInstance GetPlanes(Byte index)
        {
            if (index >= PlanesNumMax)
                return null;
            return mPlanesInstance[index];
        }

        public MapInstance GetMap(UInt16 index)
        {
            if (index >= MapNumMax)
                return null;
            return mMapInstance[index];
        }

        Dictionary<Guid, PlanesMapManager> mPlanesMapManagers = new Dictionary<Guid, PlanesMapManager>();
        public Dictionary<Guid, PlanesMapManager> PlanesMapManagers
        {
            get { return mPlanesMapManagers; }
        }

        public MapInstance GetGlobalMap(CSCommon.Data.PlanesData planesData, System.Guid mapSourceId)
        {
            PlanesMapManager pmm;
            if (false == mPlanesMapManagers.TryGetValue(planesData.PlanesId, out pmm))
            {
                pmm = new PlanesMapManager();
                pmm.InitPlanes( planesData );
                mPlanesMapManagers.Add(planesData.PlanesId, pmm);
            }
            return pmm.GetGlobalMap(mapSourceId);
        }

        public MapInstance GetMapByMapId(Guid mapId)
        {
            foreach (var i in mMapInstance)
            {
                if (i != null)
                {
                    if (i.MapGuid == Guid.Empty)
                        continue;

                    if (i.MapGuid == mapId)
                        return i;
                }
            }
            return null;
        }

        public PlanesInstance GetPlanesByPlanesId(Guid planesId)
        {
            foreach (var i in mPlanesInstance)
            {
                if (i != null)
                {
                    if (i.PlanesId == planesId)
                    {
                        return i;
                    }
                }
            }
            return null;
        }

        public void TickPlanesInstance()
        {
            foreach (var i in mPlanesInstance)
            {
                if (i != null)
                    i.Tick();
            }
        }

        Dictionary<Guid, CSCommon.Data.MapInit> mMapInitDictionary = new Dictionary<Guid, CSCommon.Data.MapInit>();
        public CSCommon.Data.MapInit GetMapInitBySourceId(Guid mapSourceId)
        {
            CSCommon.Data.MapInit outInit;
            if (mMapInitDictionary.TryGetValue(mapSourceId, out outInit))
            {
                return outInit;
            }

            outInit = new CSCommon.Data.MapInit();

            var mapDir = FindMapConfigFolderInFolder(CSUtility.Support.IFileManager.Instance.Root + CSUtility.Support.IFileConfig.Instance.MapDirectory, mapSourceId);
            if (string.IsNullOrEmpty(mapDir))
                return null;

            var mapConfigFileName = CSUtility.Support.IFileManager.Instance._GetRelativePathFromAbsPath(mapDir + "\\Config.map");
            CSUtility.Support.IConfigurator.FillProperty(outInit, mapConfigFileName);
            mMapInitDictionary[mapSourceId] = outInit;

            return outInit;
        }

        private string FindMapConfigFolderInFolder(string folder, Guid mapSourceId)
        {
            foreach (var dir in System.IO.Directory.EnumerateDirectories(folder))
            {
                var dirName = CSUtility.Support.IFileManager.Instance.GetPureFileFromFullName(dir);

                Guid mapId;
                if (System.Guid.TryParse(dirName, out mapId))
                {
                    if (mapId == mapSourceId)
                        return dir;
                }
                else
                {
                    var result = FindMapConfigFolderInFolder(dir, mapSourceId);
                    if (!string.IsNullOrEmpty(result))
                        return result;
                }
            }

            return "";
        }
    }
}
