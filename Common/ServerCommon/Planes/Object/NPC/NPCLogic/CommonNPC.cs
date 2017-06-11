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
    public class CommonNPC : BaseNPCLogic
    {
        public override eNPCLogicType LogicType { get { return eNPCLogicType.Common; } }

        public override bool OnIdle(NPCInstance npc)
        {
            if (npc.mNextPatrolTime == 0)
            {
                int minMillisecond = (int)npc.InMapData.patrolInternalMin * 1000;
                int maxMillisecond = (int)npc.InMapData.patrolInternalMax * 1000;
                npc.mNextPatrolTime = (uint)Util.Rand.Next(minMillisecond, maxMillisecond);
            }
            if (npc.mStateLastTime > npc.mNextPatrolTime)
            {
                if (!StartPatrol(npc))
                {
                    npc.ChangeState(eNPCState.Idle);
                }
                npc.mNextPatrolTime = 0;
            }
            return true;
        }

        public override bool OnPatrol(NPCInstance npc)
        {
            if (!npc.IsMoving)
            {
                npc.ChangeState(eNPCState.Idle);
            }
            return true;
        }

        public override bool StartPatrol(NPCInstance npc)
        {
            if (npc.InMapData.patrolRange <= 0)
                return false;

            Vector3 randPos = new Vector3();
            //Vector3 randPos = Util.RandInRadius(npc.mSpawnPoint, npc.InMapData.patrolRange);
            //if (Util.PassableForMod(npc.HostMap.Navigator, randPos))
            if (Util.FindRandomPoint(npc.HostMap.Navigator, npc.GetPosition(), npc.mSpawnPoint, npc.InMapData.patrolRange, ref randPos))
            {
                if (npc.MoveTo(randPos))
                {
                    npc.ChangeState(eNPCState.Patrol);
                    return true;
                }
            }
            return false;
        }

        public override bool OnFollowTarget(NPCInstance npc)
        {
            if (npc.CanCastSpell())
            {
                //到释放技能距离
                npc.ChangeState(eNPCState.WaitCoolDown);
            }
            else
            {
                if (npc.IsMoving)
                    return false;

                if (!npc.FollowTarget(npc.GetTarget(npc.mAttackTarget)))
                {
                    npc.ReturnSpawnPoint();
                    return false;
                }
            }

            return true;
        }

        public override bool FollowTarget(NPCInstance npc, RoleActor target)
        {
            if (!base.FollowTarget(npc, target))
                return false;

            //超出追击范围
            if (Util.DistanceH(npc.GetPosition(), npc.mSpawnPoint) > npc.InMapData.followRange)
                return false;

            npc.SelectSpell();

            if (npc.CanCastSpell())
            {
                //到释放技能距离
                npc.ChangeState(eNPCState.WaitCoolDown);
            }
            else
            {
                if (npc.InMapData.followRange > 0)
                {
                    Vector3 standPos = target.GetSurroundPosition(npc.GetPosition(), (int)npc.mCastSpellDistance, npc.Id);
                    if (!Util.PassableForMod(npc.HostMap.Navigator, standPos))
                    {
                        if (!Util.PassableForMod(npc.HostMap.Navigator, target.GetPosition()))
                            return false;

                        standPos = target.GetPosition();
                    }

                    if (!npc.MoveTo(standPos))
                        return false;
                }

                npc.ChangeState(eNPCState.FollowTarget);
            }

            return true;
        }

        public override bool SelectTarget(NPCInstance npc)
        {
            if (!base.SelectTarget(npc))
                return false;

            RoleActor target = npc.GetTarget(npc.mAttackTarget);
            return npc.FollowTarget(target);
        }

        public override bool OnKilled(NPCInstance npc)
        {
            if (!base.OnKilled(npc))
                return false;

            return true;
        }

    }
}
