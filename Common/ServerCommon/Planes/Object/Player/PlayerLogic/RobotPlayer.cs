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
    /// <summary>
    /// 机器人行为逻辑
    /// </summary>
    public class RobotPlayer : BasePlayerLogic
    {
        public override ePlayerLogicType LogicType { get { return ePlayerLogicType.Robot; } }

        public override bool OnIdle(PlayerInstance player)
        {
            player.ChangeState(ePlayerState.Quest);

            return true;
        }

        public override bool OnQuest(PlayerInstance player)
        {
            return true;
        }

        public override bool AcceptQuest(PlayerInstance player)
        {
            return true;
        }

        public override bool SubmitQuest(PlayerInstance player)
        {
            return true;
        }

        public override bool OnFollowTarget(PlayerInstance player)
        {
            if (player.IsMoving)
                return false;

            if (player.CanCastSpell())
            {
                //到释放技能距离
                player.ChangeState(ePlayerState.CastSpell);
            }
            else
            {
                if (!player.FollowTarget(player.mAttackTarget))
                {
                    player.ChangeState(ePlayerState.Idle);
                    return false;
                }
            }

            return true;
        }

        public override bool FollowTarget(PlayerInstance player, RoleActor target)
        {
            if (!base.FollowTarget(player, target))
                return false;

            player.SelectSpell();

            if (player.CanCastSpell())
            {
                //到释放技能距离
                player.ChangeState(ePlayerState.WaitCoolDown);
            }
            else
            {
                Vector3 standPos = target.GetSurroundPosition(player.GetPosition(), (int)player.mCastSpellDistance, player.Id);
                if (!Util.PassableForMod(player.HostMap.Navigator, standPos))
                {
                    if (!Util.PassableForMod(player.HostMap.Navigator, player.GetPosition()))
                    {
                        return false;
                    }
                    standPos = player.GetPosition();
                }

                if (!player.MoveTo(standPos))
                {
                    return false;
                }

                player.ChangeState(ePlayerState.FollowTarget);
            }

            return true;
        }

        public override bool SelectTarget(PlayerInstance player)
        {
            player.mScanLastTime = 0;
            RoleActor target = player.GetTarget(player.GetFirstEnmityCanHit());
            //从警戒范围找目标
            if (null == target)
            {
                target = player.FindHatredTarget();
            }
            if (null == target)
            {
                player.mAttackTarget = null;
                return false;
            }

            player.mAttackTarget = target;

            return player.FollowTarget(target);
        }

        public override bool OnKilled(PlayerInstance player)
        {
            if (!base.OnKilled(player))
                return false;

            return true;
        }

        public override bool OnDead(PlayerInstance player)
        {
            return true;
        }

        public override bool Revive(PlayerInstance player)
        {
            return true;
        }

        public override bool OnFixedBody(PlayerInstance player)
        {
            return true;
        }

        public override bool OnPause(PlayerInstance player)
        {
            return true;
        }
    }
}
