using CSCommon;
using CSTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
//     public class TriggerProcessData
//     {
//         public Int64 EnterTime;
//     }

//     public class TriggerProcessData_Server : TriggerProcessData
//     {
//         public RoleActor RoleActor;
//         public RPC.RPCForwardInfo fwd;
//     }

    public class TriggerInit : IActorInitBase
    {
        public MapInfo_Portal PortalDate;
    }

    [RPC.RPCClassAttribute(typeof(TriggerInstance))]
    public class TriggerInstance : RoleActor
    {

        public override void OnEnterMap(MapInstance map)
        {
            base.OnEnterMap(map);

            map.AddTrigger(this);
        }

        public override void OnLeaveMap()
        {
            if (!HostMap.IsNullMap)
            {
                HostMap.RemoveTrigger(this);
            }

            base.OnLeaveMap();
        }

        public bool Enable = true;

        //Dictionary<ulong, TriggerProcessData_Server> mProcessDatas = new Dictionary<ulong, TriggerProcessData_Server>();
        // delta为检测范围调整阈值
        public eRet_EnterTrigger TryDoTrigger(RoleActor actor, RPC.RPCForwardInfo fwd, float delta = 1.0f)
        {
            if (!Enable)
                return eRet_EnterTrigger.NotEnable;

            if (actor == null)
                return eRet_EnterTrigger.ActorIsNull;

            if (!IsPositionIn(actor.GetPosition().X, actor.GetPosition().Z, delta))
                return eRet_EnterTrigger.NotInArea;

            var player = actor as PlayerInstance;
            if (player == null)
                return eRet_EnterTrigger.ActorIsNotPlayer;
            if (TriggerData == null)
                return eRet_EnterTrigger.NotFindData;

            player.JumpToMap((ushort)TriggerData.mapId, TriggerData.targetX, 0, TriggerData.targetZ, fwd);
            return eRet_EnterTrigger.EnterMap;
        }



        private MapInfo_Portal mTriggerData;
        public MapInfo_Portal TriggerData
        {
            get { return mTriggerData; }
        }

        ulong mInstId;
        public override ulong Id
        {
            get { return mInstId; }
        }

        public bool IsPositionIn(float x,float z,float delta)
        {
            var range = TriggerData.triggerRange + delta;
            var selfX = this.Placement.GetLocation().X;
            var selfZ = this.Placement.GetLocation().Z;
            if (x > selfX - delta && x < selfX + delta
                && z > selfZ - delta && z < selfZ + delta)
            {
                return true;
            }
            

            return false;
        }


        public override bool Initialize(IActorInitBase init)
        {
 	        base.Initialize(init);

            TriggerInit myInit = init as TriggerInit;

            if (myInit == null || myInit.PortalDate == null)
                return false;
            mTriggerData = myInit.PortalDate;
            var pos = new SlimDX.Vector3(mTriggerData.posX, 0, mTriggerData.posZ);
            mPlacement.SetLocation(ref pos);
            //mScript = new PortalScript();

            return true;

        }


        public static TriggerInstance CreateTriggerInstance(MapInfo_Portal td, MapInstance map)
        {
            if (td == null)
                return null;

            TriggerInstance ret = new TriggerInstance();

            //ret.mInstId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Trigger);
            var init = new TriggerInit();
            init.GameType = eActorGameType.Potal;
            init.PortalDate = td;
            ret.Initialize(init);
            ret.OnEnterMap(map);

            return ret;
        }

        public bool ReInitTriggerInstance(MapInfo_Portal td, MapInstance map)
        {
            mTriggerData = td;
            this.OnEnterMap(map);
            var pos = new SlimDX.Vector3(td.posX, 0, td.posZ);
            mPlacement.SetLocation(ref pos);
            return true;
        }

    }
}
