using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes.Map
{
    public class PFQuery
    {
        public SlimDX.Vector3 FromPt;
        public SlimDX.Vector3 ToPt;
        //下面是远程调用的返回处理数据
        public Iocp.TcpConnect Connect;
        public UInt16 SerialId;
    }
    public class PFUnit
    {
        public ulong RoleId;
        public Queue<PFQuery> Querys = new Queue<PFQuery>();

        public void AddQuery(PFQuery qr,bool bClear)
        {
            lock (this)
            {
                if (bClear)
                    Querys.Clear();
                Querys.Enqueue(qr);
            }
        }

        public void Tick(PFMap map)
        {
            if (Querys.Count == 0)
                return;
            PFQuery qr = null;
            lock (this)
            {
                qr = Querys.Dequeue();
            }
            if (qr == null)
                return;
            
            //这里做真正的寻路处理
            FindPath(map, qr);
        }

        private void FindPath(PFMap map,PFQuery qr)
        {
            //这里找到的路径想办法通知给调用着，通常应该是一个RPC的return
            var pathPoints = new List<SlimDX.Vector2>();
            //var pathFindResult = map.NavigationProcess.FindPath(qr.FromPt.X, qr.FromPt.Z, qr.ToPt.X, qr.ToPt.Z, 20, map.NavigationData, map.PathFindContext, ref pathPoints);

            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            //retPkg.Write((Byte)pathFindResult);
            retPkg.Write(pathPoints.Count);
            foreach (var pt in pathPoints)
            {
                retPkg.Write(pt);
            }
            retPkg.DoReturnCommand2(qr.Connect, qr.SerialId);
        }
    }

    public class PFMap
    {
        ulong mMapId;
        public ulong MapId
        {
            get { return mMapId; }
        }
        //// 需保证context不再多个线程之间调用
        //Navigation.PathFindContextWrapper mPathFindContext = new Navigation.PathFindContextWrapper();
        //public Navigation.PathFindContextWrapper PathFindContext
        //{
        //    get { return mPathFindContext; }
        //}
        //Navigation.INavigationDataWrapper mNavigationData;
        //public Navigation.INavigationDataWrapper NavigationData
        //{
        //    get { return mNavigationData; }
        //}
        //Navigation.INavigationWrapper mNavigationProcess = new Navigation.INavigationWrapper();
        //public Navigation.INavigationWrapper NavigationProcess
        //{
        //    get { return mNavigationProcess; }
        //}
        //public void InitGlobalMap(ulong mapSourceId)
        //{
        //    mMapId = mapSourceId;
        //    //这里从地图管理器加载寻路信息
        //    mNavigationData = MapPathManager.Instance.GetGlobalMapPathData(mapSourceId);
        //    Navigation.INavigationInfo navInfo;
        //    mNavigationData.GetNavigationInfo(out navInfo);
        //    mPathFindContext.Initialize(navInfo);
        //}
        Dictionary<ulong, PFUnit> Units = new Dictionary<ulong, PFUnit>();
        List<PFUnit> PushingUnit = new List<PFUnit>();
        public bool IsEmptyPFQuery()
        {
            lock (this)
            {
                foreach (var i in PushingUnit)
                {
                    Units[i.RoleId] = i;
                }
                PushingUnit.Clear();
            }

            foreach (var i in Units)
            {
                if (i.Value.Querys.Count > 0)
                    return false;
            }
            return true;
        }

        public PFUnit GetUnit(ulong roleId)
        {
            lock (this)
            {
                PFUnit unit;
                if (Units.TryGetValue(roleId, out unit) == true)
                    return unit;
                foreach (var i in PushingUnit)
                {
                    if (i.RoleId == roleId)
                        return i;
                }

                unit = new PFUnit();
                unit.RoleId = roleId;
                PushingUnit.Add(unit);
                return unit;
            }
        }

        public void Tick()
        {
            lock (this)
            {
                foreach (var i in PushingUnit)
                {
                    Units[i.RoleId] = i;
                }
                PushingUnit.Clear();
            }

            foreach (var i in Units)
            {
                i.Value.Tick(this);
            }
        }
    }

    public class PFPlanes
    {
        public ulong PlanesId;
        Dictionary<ulong, PFMap> Maps = new Dictionary<ulong, PFMap>();
        List<PFMap> PushingMaps = new List<PFMap>();

        public PFMap GetMap(ulong mapId)
        {
            lock (this)
            {
                PFMap map;
                if (Maps.TryGetValue(mapId, out map) == true)
                {
                    return map;
                }
                foreach (var i in PushingMaps)
                {
                    if (i.MapId == mapId)
                        return i;
                }
                map = new PFMap();
                //map.InitGlobalMap(mapId);
                PushingMaps.Add(map);
                return map;
            }
        }

        public void Tick()
        {
            lock (this)
            {
                foreach (var i in PushingMaps)
                {
                    Maps[i.MapId] = i;
                }
                PushingMaps.Clear();
            }

            foreach (var i in Maps)
            {
                i.Value.Tick();
            }
        }

        public bool IsEmptyPFQuery()
        {
            lock (this)
            {
                foreach (var i in PushingMaps)
                {
                    Maps[i.MapId] = i;
                }
                PushingMaps.Clear();
            }

            foreach (var i in Maps)
            {
                if (i.Value.IsEmptyPFQuery()==false)
                    return false;
            }
            return true;
        }
    }

    public class PFThread
    {
        public List<PFPlanes> ExecutePlanes = new List<PFPlanes>();

        public void PushPlanes(PFPlanes planes)
        {
            lock (this)
            {
                foreach(var i in ExecutePlanes)
                {
                    if (i.PlanesId == planes.PlanesId)
                        return;
                }
                ExecutePlanes.Add(planes);
            }
        }

        public void Tick()
        {
            for (int i = 0; i < ExecutePlanes.Count; i++)
            {
                PFPlanes planes;
                lock (this)
                {
                    planes = ExecutePlanes[i];
                }

                planes.Tick();
            }
        }

        public bool IsEmptyPFQuery()
        {
            for (int i = 0; i < ExecutePlanes.Count; i++)
            {
                if (ExecutePlanes[i].IsEmptyPFQuery() == false)
                    return false;
            }
            return true;
        }

        System.Threading.Thread mThread;

        bool mRunning = false;
        public void StartThread()
        {
            mRunning = true;
            mThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ThreadLoop));
            mThread.Start();
        }

        public void StopThread()
        {
            mRunning = false;
        }

        public void ThreadLoop()
        {
            while (mRunning)
            {
                if(IsEmptyPFQuery()==true)
                    System.Threading.Thread.Sleep(1);

                Tick();
            }
        }
    }

    public class PathFinderManager
    {
        static PathFinderManager smInstance = new PathFinderManager();
        public static PathFinderManager Instance
        {
            get { return smInstance; }
        }

        Dictionary<ulong, PFPlanes> PlanesDic = new Dictionary<ulong, PFPlanes>();
        List<PFThread> mThreads = new List<PFThread>();

        PFThread SelectPFThread()
        {
            int MinValue = int.MaxValue;
            PFThread sltThread = null;
            foreach (var i in mThreads)
            {
                if (i.ExecutePlanes.Count < MinValue)
                {
                    MinValue = i.ExecutePlanes.Count;
                    sltThread = i;
                }
            }

            return sltThread;
        }

        public void StartThread(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var thread = new PFThread();
                mThreads.Add(thread);
                thread.StartThread();
            }
        }

        public void StopThread()
        {
            foreach (var i in mThreads)
            {
                i.StopThread();
            }
        }

        public void QueryGlobalMapPath(Iocp.TcpConnect connect, UInt16 serialId, ulong planesId, ulong mapSourceId, ulong roleId, SlimDX.Vector3 from, SlimDX.Vector3 to, bool bClear)
        {
            PFPlanes planes;
            if (PlanesDic.TryGetValue(planesId, out planes)==false)
            {
                planes = new PFPlanes();
                planes.PlanesId = planesId;
                PlanesDic.Add(planesId,planes);

                var thread = SelectPFThread();
                thread.PushPlanes(planes);
            }

            PFMap map = planes.GetMap(mapSourceId);

            PFUnit unit = map.GetUnit(roleId);

            PFQuery query = new PFQuery();
            query.FromPt = from;
            query.ToPt = to;
            query.Connect = connect;
            query.SerialId = serialId;
            unit.AddQuery(query, bClear);
        }
    }
}
