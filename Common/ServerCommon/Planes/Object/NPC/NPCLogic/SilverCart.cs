using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;
using ServerCommon.Planes;
using SlimDX;

namespace ServerCommon.Planes
{
    public class SilverCart : BaseNPCLogic
    {
        public override eNPCLogicType LogicType { get { return eNPCLogicType.SilverCart; } }

        public override bool OnIdle(NPCInstance npc)
        {
            if (npc.mStateLastTime > 1000)
            {
                StartTransport(npc);
            }
            
            return true;
        }

        public override bool StartTransport(NPCInstance npc)
        {
            if (null == npc.HostMap)
                return false;

            if (npc.mPathNodes.targetPosNode.Count == 0)
            {
                var tplMapWay = CommonUtil.GetMapWayDataByType(npc.Camp, npc.Type);
                if (null == tplMapWay)
                    return false;

                npc.mPathNodes.CleanUp();
                npc.mPathNodes.targetPosNode.AddLast(tplMapWay.path1);
                npc.mPathNodes.targetPosNode.AddLast(tplMapWay.path2);
                npc.mPathNodes.targetPosNode.AddLast(tplMapWay.path3);
                npc.mPathNodes.targetPosNode.AddLast(tplMapWay.path4);
                npc.mPathNodes.targetPosNode.AddLast(tplMapWay.path5);
                npc.mPathNodes.currNode = npc.mPathNodes.targetPosNode.First;
            }

            int targetPath = 0;
	        if (!npc.mPathNodes.GetCurTargetPos(ref targetPath))
		        return false;

            List<UInt64> wayPoints = npc.HostMap.GetWayPointList(targetPath);
            LinkedList<Vector3> nodes = new LinkedList<Vector3>();
            foreach (var id in wayPoints)
            {
                WayPoint wp = npc.HostMap.GetWayPoint(id);
                if (null == wp) continue;
                nodes.AddLast(wp.GetPosition());
            }
            npc.SetMoveNodes(nodes);

            npc.ChangeState(eNPCState.Transport);

            return true;
        }

        public override bool OnTransport(NPCInstance npc)
        {
            if (null == npc)
                return false;

            if (npc.IsMoving)
                return false;

            TriggerInstance trigger = npc.HostMap.InTriggerRange(npc);
            if (null == trigger)
            {
                npc.ChangeState(eNPCState.Pause);
                npc.mPathNodes.CleanUp();
                npc.OnLeaveMap();
            }
            else
            {
                if (!npc.mPathNodes.NextTargetPos())
                {
                    npc.ChangeState(eNPCState.Pause);
                    return false;
                }

                npc.Trigger = trigger;
                npc.OnJumpToMap();
                npc.ChangeState(eNPCState.WaitJumpMap);
            }

            return true;
        }

        public override bool OnWaitJumpMap(NPCInstance npc)
        {
            if (npc.mStateLastTime > 1000)
            {
                npc.ChangeState(eNPCState.Idle);
            }
            return true;
        }

        public override bool OnKilled(NPCInstance npc)
        {
            if (!base.OnKilled(npc))
                return false;

            return true;
        }

    }
}
