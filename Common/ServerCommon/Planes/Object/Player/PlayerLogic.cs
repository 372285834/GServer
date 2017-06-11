using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// Player行为逻辑接口
    /// </summary>
    public interface IPlayerLogic
    {
        ePlayerLogicType LogicType { get; }

        bool OnKilled(PlayerInstance player);
        bool Revive(PlayerInstance player);
        bool OnIdle(PlayerInstance player);
        bool OnQuest(PlayerInstance player);
        bool AcceptQuest(PlayerInstance player);
        bool SubmitQuest(PlayerInstance player);
        bool OnFollowTarget(PlayerInstance player);
        bool FollowTarget(PlayerInstance player, RoleActor target);
        bool SelectTarget(PlayerInstance player);
        bool OnDead(PlayerInstance player);
        bool OnFixedBody(PlayerInstance player);
        bool OnPause(PlayerInstance player);

    }
}
