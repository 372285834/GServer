using CSCommon;
using CSTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class WayPointInit : IActorInitBase
    {
        public MapInfo_PatrolNode PatrolNode;
    }

    public class WayPoint : RoleActor
    {

        public override void OnEnterMap(MapInstance map)
        {
            base.OnEnterMap(map);
        }

        public override void OnLeaveMap()
        {
            if (!HostMap.IsNullMap)
            {
                HostMap.RemoveWayPoint(Id);
            }

            base.OnLeaveMap();
        }

        public bool Enable = true;

        MapInfo_PatrolNode mPatrolNodeData;
        public MapInfo_PatrolNode PatrolNodeData
        {
            get { return mPatrolNodeData; }
        }

        ulong mInstId;
        public override ulong Id
        {
            get { return mInstId; }
        }

        public override bool Initialize(IActorInitBase init)
        {
            base.Initialize(init);

            WayPointInit myInit = init as WayPointInit;

            if (myInit == null || myInit.PatrolNode == null)
                return false;
            mPatrolNodeData = myInit.PatrolNode;
            var pos = new SlimDX.Vector3(mPatrolNodeData.posX, 0, mPatrolNodeData.posZ);
            mPlacement.SetLocation(ref pos);

            return true;

        }

        public static WayPoint CreateWayPoint(MapInfo_PatrolNode data, MapInstance map)
        {
            if (data == null)
                return null;

            WayPoint ret = new WayPoint();

            ret.mInstId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.WayPoint);
            var init = new WayPointInit();
            init.GameType = eActorGameType.Potal;
            init.PatrolNode = data;
            ret.Initialize(init);
            ret.OnEnterMap(map);

            return ret;
        }

    }
}
