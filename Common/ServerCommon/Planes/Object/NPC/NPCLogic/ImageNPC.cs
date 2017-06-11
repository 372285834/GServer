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
    public class ImageNPC : BaseNPCLogic
    {
        public override eNPCLogicType LogicType { get { return eNPCLogicType.PlayerImage; } }

        public override bool OnIdle(NPCInstance npc)
        {
            if (npc.mStateLastTime > GameSet.Instance.ChallengeWaitTime)
            {
                if (npc.SelectSpell())
                {
                    npc.ChangeState(eNPCState.FollowTarget);
                }
            }
            return true;
        }

        public override bool OnFollowTarget(NPCInstance npc)
        {
            if (npc.IsMoving)
                return false;

            if (npc.CanCastSpell())
            {
                //到释放技能距离
                npc.ChangeState(eNPCState.WaitCoolDown);
            }
            else
            {
                if (!npc.FollowTarget(npc.GetTarget(npc.mAttackTarget)))
                {
                    npc.ChangeState(eNPCState.Idle);
                    return false;
                }
            }

            return true;
        }

        public override bool FollowTarget(NPCInstance npc, RoleActor target)
        {
            if (!base.FollowTarget(npc, target))
                return false;

            Vector3 standPos = target.GetSurroundPosition(npc.GetPosition(), (int)npc.mCastSpellDistance, npc.Id);
            if (!Util.PassableForMod(npc.HostMap.Navigator, standPos))
            {
                if (!Util.PassableForMod(npc.HostMap.Navigator, target.GetPosition()))
                {
                    return false;
                }
                standPos = target.GetPosition();
            }
            
            if (!npc.MoveTo(standPos))
            {
                return false;
            }

            npc.ChangeState(eNPCState.FollowTarget);

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
