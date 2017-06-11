using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerFrame;
using CSCommon;

namespace ServerCommon.Planes
{
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public abstract class MapInstance : EventDispatcher //: RPC.RPCObject
    {
        #region 基础信息及初始化和销毁
        public string MapNickName
        {
            get { return mMapInfo.MapData.nickName; }
        }

        public virtual bool IsNullMap
        {
            get { return false; }
        }

        // 世界地图ID为0，副本则有对应ID
        ulong mMapInstanceId = 0;
        public ulong MapInstanceId
        {
            get { return mMapInstanceId; }
        }

        //模版id
        ushort mMapSourceId = 0;
        public ushort MapSourceId
        {
            get { return mMapSourceId; }
        }

        public string MapName
        {
            get
            {
                if (mMapInfo != null)
                    return mMapInfo.MapData.name;
                return "";
            }
        }

        public enum enEnterMapResult
        {
            Error_PlayerFull,
            Error_InvalidMap,
            Success,
        }

        UInt16 mIndexInServer = UInt16.MaxValue;
        public UInt16 IndexInServer
        {
            get { return mIndexInServer; }
        }

        PlanesInstance mPlanes;
        public PlanesInstance Planes
        {
            get { return mPlanes; }
        }

        PlayerInstance mOwner;
        public PlayerInstance Owner
        {
            get { return mOwner; }
        }

        Navigator mNavigator;
        public Navigator Navigator
        {
            get { return mNavigator; }
        }

        public string GetNavFilePath(string name)
        {
            string scenePath = ServerConfig.Instance.MapNavPath;// "../../../../design/editor/Assets/Resources/Editor/Scene";
             
            return string.Format(@"{0}/{1}_nav.bytes", scenePath, name);
        }

        public void Tick(Int64 elapsedMiliSeccond)
        {
            try
            {
                mRpcProcessor.Tick();
                foreach (PlayerInstance i in mPlayerPool)
                {//这个不怕迭代中删除
                    if (i != null)
                    {
                        i.Tick(elapsedMiliSeccond);
                    }
                }

                lock (mAddedNPCList)
                {
                    foreach (var i in mAddedNPCList)
                    {
                        mNpcDictionary[i.Id] = i;
                    }
                    mAddedNPCList.Clear();
                }
                lock (mRemovedNPCList)
                {
                    foreach (var npc in mRemovedNPCList)
                    {
                        mNpcDictionary.Remove(npc.Id);
                    }
                    mRemovedNPCList.Clear();
                }

                foreach (var npc in mNpcDictionary)
                {
                    npc.Value.Tick(elapsedMiliSeccond);
                }


                foreach (var npc in mGatherDictionary)
                {
                    npc.Value.Tick(elapsedMiliSeccond);
                }

                // tick trigger
                foreach (var trigger in mTriggerDictionary.Values)
                {
                    trigger.Tick(elapsedMiliSeccond);
                }

                if (null == mTransmitingNpc && mNpcCreatingQueue.Count == 0)
                {
                    for (var i = 0; i < mNpcCreatingPool.Count; i++)
                    {
                        if (mNpcCreatingQueue.Count < mLimitCount)
                        {
                            mNpcCreatingQueue.Add(mNpcCreatingPool[i]);
                            mNpcCreatingPool.RemoveAt(i);
                            i--;
                        }
                    }
                }

                for (var i = 0; i < mNpcCreatingQueue.Count; i++)
                {
                    if (null == mNpcCreatingQueue[i]) continue;
                    if (IServer.timeGetTime() - mLastCreateTime >= mIntervalTime)
                    {
                        NPCInstance npc = NPCInstance.CreateNPCInstance(mNpcCreatingQueue[i], this);
                        mLastCreateTime = IServer.timeGetTime();
                        mNpcCreatingQueue[i] = null;
                        if (i == (int)mLimitCount - 1)
                            mTransmitingNpc = npc;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.Common.Print(ex.ToString() + "==>");
                Log.Log.Common.Print(ex.StackTrace.ToString());
            }
        }

        uint mLimitCount = 10; //一波数量限制
        uint mLastCreateTime = 0; //上次创建时间间隔
        uint mIntervalTime = 3000; //单个NPC创建时间间隔限制
        NPCInstance mTransmitingNpc = null; //等待传送的最后一个NPC
        List<MapInfo_Npc> mNpcCreatingPool = new List<MapInfo_Npc>(); //npc等待创建池
        List<MapInfo_Npc> mNpcCreatingQueue = new List<MapInfo_Npc>(); //npc准备创建队列

        /// <summary>
        /// 需求排队创建的npc进入队列
        /// </summary>
        public void PushCreatingNpc(eNpcType type, MapInfo_Npc npcData)
        {
            if (mNpcCreatingQueue.Count < mLimitCount)
            {
                mNpcCreatingQueue.Add(npcData);
            }
            else
            {
                mNpcCreatingPool.Add(npcData);
            }
        }

        bool mWaitDestory;
        public bool WaitDestory
        {
            get { return mWaitDestory; }
        }
        public void ReleaseInstanceZones()
        {
            mWaitDestory = true;
        }
        public void ReleaseInstanceMap()
        {
            mWaitDestory = true;
            mNpcCreatingQueue.Clear();
            mNpcCreatingPool.Clear();
            CleanUp();
        }

        LogicProcessor mLogicProcessor;
        public LogicProcessor LogicProcessor
        {
            get { return mLogicProcessor; }
            set { mLogicProcessor = value; }
        }

        RPC.RPCSpecialHolderProcessor mRpcProcessor = new RPC.RPCSpecialHolderProcessor();
        public RPC.RPCSpecialHolderProcessor RpcProcessor
        {
            get { return mRpcProcessor; }
        }

        TableWrap.MapInfoData mMapInfo = null;
        public TableWrap.MapInfoData MapInfo
        {
            get { return mMapInfo; }
        }

        public bool InitMap(PlanesInstance planes, ushort index, ulong mapInstanceId, TableWrap.MapInfoData info, PlayerInstance creater)
        {
            mPlanes = planes;
            mOwner = creater;
            mIndexInServer = index;
            mMapInfo = info;
            //             mMapInfo = MapInstanceManager.GetMapInitBySourceId(mapSourceId);
            //             if (mMapInfo == null)
            //                 return false;
            //mUUID = mapInstanceId;

            mMapInstanceId = mapInstanceId;
            mMapSourceId = (ushort)info.MapData.id;// mapSourceId;
            InitPlayerPool((ushort)mMapInfo.MapData.maxPlayerCount);
            mNavigator = NavigatorMgr.Instance.InitNavigator(mMapSourceId, GetNavFilePath(mMapInfo.MapData.name));

            // 从地图数据中创建实例数据
            m_cellXCount = (int)(mMapInfo.MapData.sizeX / mServerMapCellWidth) + ((mMapInfo.MapData.sizeX % mServerMapCellWidth) > 0 ? 1 : 0);
            m_cellZCount = (int)(mMapInfo.MapData.sizeZ / mServerMapCellHeight) + ((mMapInfo.MapData.sizeZ % mServerMapCellHeight) > 0 ? 1 : 0);

            m_mapCells = new MapCellInstance[m_cellZCount, m_cellXCount];
            for (int i = 0; i < m_cellZCount; i++)
            {
                for (int j = 0; j < m_cellXCount; j++)
                {
                    MapCellInstance mapCell = new MapCellInstance(j, i);
                    m_mapCells[i, j] = mapCell;
                }
            }

            //LoadMapData();
            OnInit();
            return true;
        }

        //bool LoadMapData()
        //{

        //    //             foreach (var PortalData in mMapInfo.MapDetail)
        //    //             {
        //    //                 var trigger = TriggerInstance.CreateTriggerInstance(PortalData,this);
        //    //                 AddTrigger(trigger);
        //    //             }

        //    foreach (var NpcData in mMapInfo.MapDetail.NpcList)
        //    {
        //        var npc = NPCInstance.CreateNPCInstance(NpcData, this);
        //        AddNPC(npc);
        //    }



        //    return true;
        //}

        MapCellInstance mDefaultCellInstance = new MapCellInstance(0, 0);
        public MapCellInstance GetMapCell(float x, float z)
        {
            if (m_cellZCount == 0 && m_cellXCount == 0)
                return mDefaultCellInstance;
            int cellX = (int)(x / mServerMapCellWidth);
            int cellZ = (int)(z / mServerMapCellHeight);

            if (cellX < 0)
                cellX = 0;
            if (cellX >= m_cellXCount)
                cellX = m_cellXCount - 1;

            if (cellZ < 0)
                cellZ = 0;
            if (cellZ >= m_cellZCount)
                cellZ = m_cellZCount - 1;

            return m_mapCells[cellZ, cellX];
        }

        public List<MapCellInstance> GetMapCell(float startx, float startz, float endx, float endz)
        {
            if (m_cellZCount == 0 && m_cellXCount == 0)
                return null;
            List<MapCellInstance> retMapCells = new List<MapCellInstance>();

            int cellStartX = (int)(startx / mServerMapCellWidth);
            int cellStartZ = (int)(startz / mServerMapCellHeight);

            if (cellStartX < 0)
                cellStartX = 0;
            if (cellStartX >= m_cellXCount)
                cellStartX = m_cellXCount - 1;

            if (cellStartZ < 0)
                cellStartZ = 0;
            if (cellStartZ >= m_cellZCount)
                cellStartZ = m_cellZCount - 1;

            int cellEndX = (int)(endx / mServerMapCellWidth);
            int cellEndZ = (int)(endz / mServerMapCellHeight);

            if (cellEndX < 0)
                cellEndX = 0;
            if (cellEndX >= m_cellXCount)
                cellEndX = m_cellXCount - 1;

            if (cellEndZ < 0)
                cellEndZ = 0;
            if (cellEndZ >= m_cellZCount)
                cellEndZ = m_cellZCount - 1;

            for (int x = cellStartX; x < cellEndX + 1; ++x)
            {
                for (int z = cellStartZ; z < cellEndZ + 1; ++z)
                {
                    retMapCells.Add(m_mapCells[z, x]);
                }
            }

            return retMapCells;
        }

        #endregion 

        #region Player Manager //////////////////////////////////////////////////////////////////////////
        System.UInt16 mMaxPlayerCount;
        PlayerInstance[] mPlayerPool;
        public PlayerInstance[] PlayerPool
        {
            get { return mPlayerPool; }
        }

        Dictionary<ulong, PlayerInstance> mPlayerDictionary = new Dictionary<ulong, PlayerInstance>();
        Stack<System.UInt16> mFreeSlot = new Stack<System.UInt16>();
        public void InitPlayerPool(System.UInt16 maxPly)
        {
            mMaxPlayerCount = maxPly;
            mPlayerPool = new PlayerInstance[maxPly];
            mFreeSlot.Clear();
            for (UInt16 i = 0; i < maxPly - 1; i++)
            {
                mFreeSlot.Push(i);
            }
        }

        public bool IsFull
        {
            get { return mFreeSlot.Count == 0; }
        }

        public RoleActor GetRole(ulong singleId)
        {
            RoleActor role = FindPlayer(singleId);
            if(role!=null)
                return role;

            role = GetNPC(singleId);
            if (role != null)
                return role;


            role = GetGatherItem(singleId);
            if (role != null)
                return role;

            return null;
        }
        
        //public PlayerInstance FindPlayer(ulong singleId)
        //{
        //    return RoleIdManager.FindPlayer(singleId);
        //}

        public PlayerInstance GetPlayer(System.UInt16 index)
        {
            return mPlayerPool[index];
        }

        public PlayerInstance FindPlayer(ulong id)
        {
            PlayerInstance retPlayer = null;
            if (mPlayerDictionary.TryGetValue(id, out retPlayer))
                return retPlayer;

            return null;
        }

        public PlayerInstance FindPlayerByName(string name)
        {
            PlayerInstance retPlayer = null;
            foreach (var i in mPlayerPool)
            {
                if (i == null)
                    continue;
                if (i.RoleName.CompareTo(name) == 0)//在线
                {
                    retPlayer = i;
                    break;
                }
            }
            return retPlayer;
        }

        #endregion

        #region NPC Manager //////////////////////////////////////////////////////////////////////////
        // 用于记录当前地图中的NPC
        Dictionary<ulong, NPCInstance> mNpcDictionary = new Dictionary<ulong, NPCInstance>();
        public Dictionary<ulong, NPCInstance> NpcDictionary { get { return mNpcDictionary; } }

        public NPCInstance GetNPC(ulong id)
        {
            NPCInstance npc;
            if (mNpcDictionary.TryGetValue(id, out npc))
                return npc;
            return null;
        }

        public NPCInstance GetNPC(int tplId)
        {
            foreach (var npc in mNpcDictionary.Values)
            {
                if (npc.NPCData.TemplateId == tplId)
                    return npc;
            }
            return null;
        }

        List<NPCInstance> mAddedNPCList = new List<NPCInstance>();
        List<NPCInstance> mRemovedNPCList = new List<NPCInstance>();
        public void AddNPC(NPCInstance npc)
        {
            lock (mAddedNPCList)
            {
                mAddedNPCList.Add(npc);
            }
        }
        public void RemoveNPC(NPCInstance npc)
        {
            lock (mRemovedNPCList)
            {
                mRemovedNPCList.Add(npc);
            }
            //mNpcDictionary.Remove(npc.Id);
        }
      

        Dictionary<ulong, GatherItem> mGatherDictionary = new Dictionary<ulong, GatherItem>();
        public Dictionary<ulong, GatherItem> GatherDictionary
        {
            get { return mGatherDictionary; }
        }
        public GatherItem GetGatherItem(ulong singleId)
        {
            GatherItem role;
            if (mGatherDictionary.TryGetValue(singleId, out role))
                return role;
            return null;
        }
        #endregion

        #region Trigger Manager //////////////////////////////////////////////////////////

        // 用于记录在当前地图中的Trigger
        Dictionary<int, ServerCommon.Planes.TriggerInstance> mTriggerDictionary = new Dictionary<int, ServerCommon.Planes.TriggerInstance>();
        public Dictionary<int, TriggerInstance> TriggerDictionary { get { return mTriggerDictionary; } }

        public TriggerInstance GetTrigger(int id)
        {
            TriggerInstance trigger;
            if (mTriggerDictionary.TryGetValue(id, out trigger))
                return trigger;
            return null;
        }

        public void AddTrigger(TriggerInstance trigger)
        {
            if (GetTrigger(trigger.TriggerData.id) == null)
                mTriggerDictionary[trigger.TriggerData.id] = trigger;
        }

        public void RemoveTrigger(TriggerInstance trigger)
        {
            mTriggerDictionary.Remove(trigger.TriggerData.id);
        }

        public TriggerInstance InTriggerRange(RoleActor role)
        {
            if (null == role)
                return null;

            foreach (var val in mTriggerDictionary)
            {
                if (Util.DistanceH(val.Value.GetPosition(), role.GetPosition()) < 1.0f)
                    return val.Value;
            }

            return null;
        }

        #endregion

        #region 路点场景对象
        Dictionary<UInt64, ServerCommon.Planes.WayPoint> mWayPointDictionary = new Dictionary<UInt64, ServerCommon.Planes.WayPoint>();
        public Dictionary<UInt64, WayPoint> WayPointDictionary { get { return mWayPointDictionary; } }
        Dictionary<int, List<UInt64>> mPathNodes = new Dictionary<int, List<ulong>>();
        public WayPoint GetWayPoint(UInt64 id)
        {
            WayPoint waypoint;
            if (mWayPointDictionary.TryGetValue(id, out waypoint))
                return waypoint;
            return null;
        }

        public List<UInt64> GetWayPointList(int pathId)
        {
            List<UInt64> wayPoints = new List<ulong>();
            if (mPathNodes.ContainsKey(pathId))
                wayPoints = mPathNodes[pathId];

            return wayPoints;
        }

        public void AddWayPoint(int pathId, WayPoint waypoint)
        {
            if (GetWayPoint(waypoint.Id) == null)
                mWayPointDictionary[waypoint.Id] = waypoint;

            int objectId = mMapSourceId * 10 + pathId;
            if (!mPathNodes.ContainsKey(objectId))
                mPathNodes[objectId] = new List<ulong>();
            mPathNodes[objectId].Add(waypoint.Id);
        }

        public void RemoveWayPoint(UInt64 id)
        {
            WayPointDictionary.Remove(id);
            foreach (var i in mPathNodes.Keys.ToList())
            {
                mPathNodes[i].Remove(id);
            }
        }
        #endregion

        #region MapCell //////////////////////////////////////////////////////////////////////////

        MapCellInstance[,] m_mapCells;
        int m_cellXCount;
        int m_cellZCount;
        public int CellXCount { get { return m_cellXCount; } }
        public int CellZCount { get { return m_cellZCount; } }
        public MapCellInstance[,] MapCells { get { return m_mapCells; } }

        const int m_maxPlayerToAsync = 50;    // 一次同步的最大玩家数量
        public int MaxPlayerToAsync
        {
            get { return m_maxPlayerToAsync; }
        }
        const float m_playerAsyncRateInSameCell = 0.7f;   // 同一格的同步数量比例
        public float PlayerAsyncRateInSameCell
        {
            get { return m_playerAsyncRateInSameCell; }
        }
        const int m_asyncRange = 200;         // 同步范围
        Random m_rand = new Random();
        public Random AsyncRand
        {
            get { return m_rand; }
        }
        
        // 服务器端地图格宽度
        public const float mServerMapCellWidth = 8;
        // 服务器端地图格高度
        public const float mServerMapCellHeight = 8;

        #endregion
              
        #region 玩家进出地图的管理

        public enEnterMapResult PlayerEnterMap(PlayerInstance role,SlimDX.Vector3 pos,bool bTellClient)
        {
            if (mPlayerPool == null)
                return enEnterMapResult.Error_InvalidMap;

            if (mFreeSlot.Count == 0)
                return enEnterMapResult.Error_PlayerFull;

            role.Placement.SetLocation(ref pos);

            if (role.HostMap != this)
            {
                if (!role.HostMap.IsNullMap)
                {
                    role.HostMap.PlayerLeaveMap(role,false);
                }
                role._SetIndexInMap(mFreeSlot.Pop());
                System.Diagnostics.Debug.Assert(mPlayerPool[role.IndexInMap] == null);
                mPlayerPool[role.IndexInMap] = role;
                mPlayerDictionary[role.Id] = role;

                if (AllMapManager.IsInstanceMap(this.MapSourceId)==false)
                {
                    //role.PlayerData.RoleDetail.MapName = this.MapName;
                    role.PlayerData.RoleDetail.DungeonID = 0;
                    role.PlayerData.RoleDetail.MapSourceId = this.MapSourceId;
                }
                else
                {
                    role.PlayerData.RoleDetail.DungeonID = this.MapInstanceId;
                }

                role.OnEnterMap(this);
            }

            OnRoleEnterMap(role);

            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_GateServer(pkg).PlayerEnterMapInPlanes(pkg, role.Id, role.ClientLinkId, role.IndexInMap, this.IndexInServer);
            pkg.DoCommand(role.Planes2GateConnect, RPC.CommandTargetType.DefaultType);

            role.HostMap = this;
            return enEnterMapResult.Success;
        }

        public bool PlayerLeaveMap(PlayerInstance role,bool bTellGateSver)
        {
            if (mPlayerPool == null)
                return false;

            if (role.IndexInMap >= mPlayerPool.Length)
            {
                //System.Diagnostics.Debugger.Break();
                for (UInt16 i = 0; i < mPlayerPool.Length; i++)
                {
                    if (mPlayerPool[i] == role)
                    {
                        role._SetIndexInMap(i);
                        break;
                    }
                }
                return false;
            }
            else
            {
                if (mPlayerPool[role.IndexInMap] != role)
                    return false;
            }

            //先存盘
            role.OnLeaveMap();

            //后清理
            mPlayerDictionary.Remove(role.Id);
            mPlayerPool[role.IndexInMap] = null;
            mFreeSlot.Push(role.IndexInMap);
            role._SetIndexInMap(System.UInt16.MaxValue);

            if (bTellGateSver)
            {
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_GateServer(pkg).PlayerLeaveMapInPlanes(pkg, role.ClientLinkId);
                pkg.DoCommand(role.Planes2GateConnect, RPC.CommandTargetType.DefaultType);
            }

            return true;
        }

        #endregion

        #region 子类可重载的函数实现

        public abstract CSCommon.eMapType MapType
        {
            get;
        }

        public virtual void OnMapClosed()
        {

        }

        public virtual void OnRoleEnterMap(RoleActor role)
        {
            var loc = role.GetPosition();
            var mapCell = GetMapCell(loc.X, loc.Z);
            mapCell.AddRole(role);
        }

        public virtual void OnRoleLeaveMap(RoleActor role)
        {
            // 广播给客户端
            BroadcastRoleLeave(role);

            var loc = role.GetPosition();
            var mapCell = GetMapCell(loc.X, loc.Z);
            mapCell.RemoveRole(role);
            if (mTransmitingNpc == role)
            {
                mTransmitingNpc = null;
                mNpcCreatingQueue.Clear();
            }
        }

        public virtual CSCommon.eMapAttackRule CanAttack(RoleActor atk, RoleActor def)
        {
            return CSCommon.eMapAttackRule.None;
        }

        public virtual void OnInit()
        {
            foreach (var NpcData in mMapInfo.MapDetail.NpcList)
            {
                try
                {
                    var tplNpc = CSTable.StaticDataManager.NPCTpl[NpcData.tid];
                    if (tplNpc.type == (int)CSCommon.eNpcType.FoodCar || tplNpc.type == (int)CSCommon.eNpcType.MoneyCar) continue;
                    
                    NPCInstance.CreateNPCInstance(NpcData, this);
                }
                catch (System.Exception ex)
                {
                    Log.Log.Server.Warning("CreateNPCInstance Error,id={0}", NpcData.tid);
                }
            }

            foreach (var portal in mMapInfo.MapDetail.PortalList)
            {
                try
                {
                    var p = TriggerInstance.CreateTriggerInstance(portal, this);
                    if (null == p)
                        continue;

                    AddTrigger(p);
                }
                catch (System.Exception ex)
                {
                    Log.Log.Server.Warning("CreateTriggerInstance Error,id={0}", portal.id);
                }
            }

            foreach (var path in mMapInfo.MapDetail.PathList)
            {
                foreach (var node in path.PatrolNodeList)
                {
                    try
                    {
                        var p = WayPoint.CreateWayPoint(node, this);
                        if (null == p)
                            continue;

                        AddWayPoint(path.id, p);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Log.Server.Warning("CreateWayPoint Error,pathId={0},nodeId={1}", path.id, node.id);
                    }
                }
            }

        }

        /// <summary>
        /// 客户端进入地图结束
        /// </summary>
        /// <param name="role"></param>
        public virtual void OnClientEnterMapOver(PlayerInstance role)
        {

        }

        //玩家移动时，是否将移动信息记录到数据库
        public virtual bool IsSavePos(PlayerInstance role)
        {
            if (MapInfo.IsInstance)
                return false;

            return true;
        }

        #endregion

        #region 广播信息

        public delegate bool FOnVisitRole(RoleActor role, object arg);
        bool TourCellInner(MapCellInstance cell, ref SlimDX.Vector3 loc, float radius, UInt32 actorTypes, FOnVisitRole visit, object arg, List<RoleActor> outActors = null)
        {
            try
            {
                if ((actorTypes & (1 << (Int32)eActorGameType.Player)) != 0)
                {
                    for (int i = 0; i < cell.Players.Count; i++)
                    {
                        var ply = cell.Players[i];
                        if (ply == null)
                            continue;

                        float dist = Util.DistanceH(loc, ply.GetPosition());
                        if (dist > radius)
                            continue;

                        if (visit != null && visit(ply, arg) == false)
                            continue;

                        if (outActors != null)
                            outActors.Add(ply);
                    }
                }
                if ((actorTypes & (1 << (Int32)eActorGameType.PlayerImage)) != 0)
                {
                    for (int i = 0; i < cell.Images.Count; i++)
                    {
                        var img = cell.Images[i];
                        if (img == null)
                            continue;

                        float dist = Util.DistanceH(loc, img.GetPosition());
                        if (dist > radius)
                            continue;

                        if (visit != null && visit(img, arg) == false)
                            continue;

                        if (outActors != null)
                            outActors.Add(img);
                    }
                }
                if ((actorTypes & (1 << (Int32)eActorGameType.Npc)) != 0)
                {
                    for (int i = 0; i < cell.NPCs.Count; i++)
                    {
                        var npc = cell.NPCs[i];
                        if (npc == null)
                            continue;

                        float dist = Util.DistanceH(loc, npc.GetPosition());
                        if (dist > radius)
                            continue;

                        if (visit != null && visit(npc, arg) == false)
                            continue;

                        if (outActors != null)
                            outActors.Add(npc);
                    }
                }
                return true;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("TourCellInner1 foreach except\r\n" + ex.ToString());
                return true;
            }

        }

        public bool IsPositionIn(SlimDX.Vector3 pos, SlimDX.Matrix invMat)
        {
            var triggerSpacePos = SlimDX.Vector3.TransformCoordinate(pos, invMat);
            if ((Math.Abs(triggerSpacePos.X) < (0.5f)) &&
                //(Math.Abs(triggerSpacePos.Y) < (0.5f)) &&
                (Math.Abs(triggerSpacePos.Z) < (0.5f)))
                return true;

            return false;
        }

        bool TourCellInner(MapCellInstance cell, List<SlimDX.Matrix> invMatList, UInt32 actorTypes, FOnVisitRole visit, object arg, List<RoleActor> outActors = null)
        {
            try
            {
                if ((actorTypes & (1 << (Int32)eActorGameType.Player)) != 0)
                {
                    for (int i = 0; i < cell.Players.Count; i++)
                    {
                        var ply = cell.Players[i];
                        if (ply == null)
                            continue;

                        var tarLoc = ply.GetPosition();
                        foreach (var invMat in invMatList)
                        {
                            //var ctype = SlimDX.BoundingBox.Contains(box, tarLoc);
                            //if(ctype!=SlimDX.ContainmentType.Contains)
                            //    continue;
                            if (IsPositionIn(tarLoc, invMat) == false)
                                continue;

                            if (visit != null && visit(ply, arg) == false)
                                continue;

                            if (outActors != null)
                                outActors.Add(ply);
                        }
                    }
                }
                if ((actorTypes & (1 << (Int32)eActorGameType.PlayerImage)) != 0)
                {
                    for (int i = 0; i < cell.Images.Count; i++)
                    {
                        var img = cell.Images[i];
                        if (img == null)
                            continue;

                        var tarLoc = img.GetPosition();
                        foreach (var invMat in invMatList)
                        {
                            //var ctype = SlimDX.BoundingBox.Contains(box, tarLoc);
                            //if(ctype!=SlimDX.ContainmentType.Contains)
                            //    continue;
                            if (IsPositionIn(tarLoc, invMat) == false)
                                continue;

                            if (visit != null && visit(img, arg) == false)
                                continue;

                            if (outActors != null)
                                outActors.Add(img);
                        }
                    }
                }
                if ((actorTypes & (1 << (Int32)eActorGameType.Npc)) != 0)
                {
                    for (int i = 0; i < cell.NPCs.Count; i++)
                    {
                        var npc = cell.NPCs[i];
                        if (npc == null)
                            continue;

                        var tarLoc = npc.GetPosition();
                        foreach (var invMat in invMatList)
                        {
                            //var ctype = SlimDX.BoundingBox.Contains(box, tarLoc);
                            //if (ctype != SlimDX.ContainmentType.Contains)
                            //    continue;
                            if (IsPositionIn(tarLoc, invMat) == false)
                                continue;

                            if (visit != null && visit(npc, arg) == false)
                                continue;

                            if (outActors != null)
                                outActors.Add(npc);
                        }
                    }
                }
                return true;
            }
            catch (System.Exception)
            {
                System.Diagnostics.Debug.WriteLine("TourCellInner2 foreach except");
                return true;
            }
        }

        public bool TourRoles(ref SlimDX.Vector3 loc, float radius, UInt32 actorTypes, List<RoleActor> outActors)
        {
            return TourRoles(ref loc, radius, actorTypes, null, null, outActors);
        }

        public bool TourRoles(ref SlimDX.Vector3 loc, float radius, UInt32 actorTypes, FOnVisitRole visit, object arg, List<RoleActor> outActors = null)
        {
            float startX = loc.X - radius;
            float endX = loc.X + radius;
            float startZ = loc.Z - radius;
            float endZ = loc.Z + radius;
            var mapCells = GetMapCell(startX, startZ, endX, endZ);
            if (mapCells == null)
                return false;

            for (var i = 0; i < mapCells.Count(); i++) //方便查看第几个格子
            {
                if (false == TourCellInner(mapCells[i], ref loc, radius, actorTypes, visit, arg, outActors))
                    return false;
            }

            return true;
        }

        public bool TourRoles(SlimDX.BoundingBox absBox, List<SlimDX.Matrix> invMatList, UInt32 actorTypes, FOnVisitRole visit, object arg)
        {
            if (invMatList.Count == 0)
                return true;

            var mapCells = GetMapCell(absBox.Minimum.X, absBox.Minimum.Z, absBox.Maximum.X, absBox.Maximum.Z);
            if (mapCells == null)
                return false;

            for (var i = 0; i < mapCells.Count(); i++) //方便查看第几个格子
            {
                if (false == TourCellInner(mapCells[i], invMatList, actorTypes, visit, arg))
                    return false;
            }

            return true;
        }

        class PkgSender
        {
            public RPC.PackageWriter Pkg;
            public RoleActor IgnoreRole;
        }

        int mTourRolesCount;    //计算最大广播人数
        public void SendPkg2Clients(RoleActor ignore, SlimDX.Vector3 pos, RPC.PackageWriter pkg)
        {
            //这个range是以pos为中心广播多大范围内的玩家
            UInt32 actorTypes = (1 << (Int32)eActorGameType.Player);
            var arg = new PkgSender();
            arg.Pkg = pkg;
            arg.IgnoreRole = ignore;
            mTourRolesCount = 0;
            TourRoles(ref pos, RPC.RPCNetworkMgr.Sync2ClientRange, actorTypes, this.OnVisitRole_SendPkg2Client, arg);
        }

        protected bool OnVisitRole_SendPkg2Client(RoleActor role, object arg)
        {
            var player = role as PlayerInstance;
            if (player == null)
                return true;

            var sender = arg as PkgSender;
            if (sender.IgnoreRole == role)
                return true;

            //if (mTourRolesCount > RPC.RPCNetworkMgr.Sync2ClientMaxPlayerCount)
            //    return true;

            sender.Pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
            mTourRolesCount++;
            return true;
        }


        public void RolePositionChanged(RoleActor role, ref SlimDX.Vector3 loc)
        {
            if (this.IsNullMap)
            {
                Log.Log.Common.Print(string.Format("Role {0} call RoleDirectionChanged in nullmap", role.RoleName));
                return;
            }
            if (role is NPCInstance)
            {
                NPCInstance npc = role as NPCInstance;
                if (npc.Type == eNpcType.FoodCar)
                {
                    var a = 0;
                    a++;
                }
            }
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, role.Id).RPC_SyncPostion(pkg, loc.X, loc.Z);
            this.SendPkg2Clients(role, loc, pkg);
        }

        public void RoleDirectionChanged(RoleActor role, float angle)
        {
            if (this.IsNullMap)
            {
                Log.Log.Common.Print(string.Format("Role {0} call RoleDirectionChanged in nullmap", role.RoleName));
                return;
            }

            RPC.PackageWriter pkg = new RPC.PackageWriter();

            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, role.Id).RPC_SyncDirection(pkg, angle);

            this.SendPkg2Clients(role, role.GetPosition(), pkg);
        }

        public void BroadcastRoleLeave(RoleActor role)
        {
            if (this.IsNullMap)
            {
                Log.Log.Common.Print(string.Format("Role {0} call RoleDirectionChanged in nullmap", role.RoleName));
                return;
            }

            RPC.PackageWriter pkg = new RPC.PackageWriter();

            Wuxia.H_RpcRoot.smInstance.RPC_RoleLeave(pkg, role.Id);

            this.SendPkg2Clients(role, role.GetPosition(), pkg);
        }
        #endregion
     
    }

    public class NullMapInstance : MapInstance
    {
        public override CSCommon.eMapType MapType
        {
            get { return CSCommon.eMapType.NULL; }
        }

        static NullMapInstance smInstance = new NullMapInstance();
        public static NullMapInstance Instance
        {
            get { return smInstance; }
        }
        public override bool IsNullMap
        {
            get { return true; }
        }
    }

    public class WolrdMap : MapInstance
    {
        public override CSCommon.eMapType MapType
        {
            get { return CSCommon.eMapType.World; }
        }
    }
}
