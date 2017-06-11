using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class MapPathManager
    {
        static MapPathManager smInstance = new MapPathManager();
        public static MapPathManager Instance
        {
            get { return smInstance; }
        }

        //Dictionary<Guid, Navigation.INavigationDataWrapper> mNavigationDataDic = new Dictionary<Guid, Navigation.INavigationDataWrapper>();

        //public Navigation.INavigationDataWrapper GetGlobalMapPathData(ulong mapSourceId, bool forceLoad = false)
        //{
        //    if (!forceLoad)
        //    {
        //        Navigation.INavigationDataWrapper navData = null;
        //        if (mNavigationDataDic.TryGetValue(mapSourceId, out navData))
        //            return navData;
        //    }

        //    return LoadGlobalMapPathData(mapSourceId);
        //}

        //public Navigation.INavigationDataWrapper LoadGlobalMapPathData(ulong mapSourceId)
        //{
        //    lock (this)
        //    {
        //        // 读取mapInit
        //        var mapInit = new CSCommon.Data.MapInit();
        //        var mapDir = PlanesManager.FindMapConfigFolderInFolder(ServerFrame.Support.IFileManager.Instance.Root + ServerFrame.Support.IFileConfig.Instance.MapDirectory, mapSourceId);
        //        if (string.IsNullOrEmpty(mapDir))
        //            return null;

        //        var mapConfigFileName = ServerFrame.Support.IFileManager.Instance._GetRelativePathFromAbsPath(mapDir + "\\Config.map");
        //        ServerFrame.Config.IConfigurator.FillProperty(mapInit, mapConfigFileName);

        //        Navigation.INavigationInfo navInfo = new Navigation.INavigationInfo();
        //        navInfo.ResetDefault();
        //        navInfo.mXValidLevelCount = mapInit.SceneSizeX;
        //        navInfo.mZValidLevelCount = mapInit.SceneSizeZ;
        //        navInfo.mLevelLengthX = (UInt32)(mapInit.SceneMeterX / mapInit.SceneSizeX / navInfo.mMeterPerPixelX);
        //        navInfo.mLevelLengthZ = (UInt32)(mapInit.SceneSizeZ / mapInit.SceneSizeZ / navInfo.mMeterPerPixelZ);

        //        var navDir = mapDir + "\\" + mapInit.Navigation;
        //        var path = ServerFrame.Support.IFileManager.Instance.GetPathFromFullName(navDir);
        //        var file = ServerFrame.Support.IFileManager.Instance.GetPureFileFromFullName(navDir);

        //        var navData = new Navigation.INavigationDataWrapper();
        //        navData.ConstrutNavigationData(file, path, navInfo);
        //        navData.LoadNavigationData(file, path);

        //        mNavigationDataDic[mapSourceId] = navData;

        //        return navData;
        //    }
        //}
    }
}
