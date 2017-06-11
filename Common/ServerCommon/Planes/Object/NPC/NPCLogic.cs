using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// NPC行为逻辑接口
    /// </summary>
    public interface INPCLogic
    {
        eNPCLogicType LogicType { get; }

        bool OnKilled(NPCInstance npc);
        bool Revive(NPCInstance npc);
        bool OnIdle(NPCInstance npc);
        bool OnPatrol(NPCInstance npc);
        bool StartPatrol(NPCInstance npc);
        bool OnFollowTarget(NPCInstance npc);
        bool FollowTarget(NPCInstance npc, RoleActor player);
        bool SelectTarget(NPCInstance npc);
        bool OnTransport(NPCInstance npc);
        bool OnWaitJumpMap(NPCInstance npc);
        bool StartTransport(NPCInstance npc);
        bool OnDead(NPCInstance npc);
        bool OnFixedBody(NPCInstance npc);
        bool OnPause(NPCInstance npc);

    }
}
