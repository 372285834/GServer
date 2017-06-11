using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;

namespace ServerCommon.Planes
{
    public class BasePlayerLogic : IPlayerLogic
    {
        public virtual ePlayerLogicType LogicType { get { return ePlayerLogicType.None; } }

        public static bool LoadAllLogic()
        {
            var assmbly = System.Reflection.Assembly.GetExecutingAssembly();
            var types = assmbly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsSubclassOf(typeof(BasePlayerLogic)))
                    continue;

                System.Activator.CreateInstance(type);
            }

            return true;
        }

        BaseGameLogic<IPlayerLogic> mInstance;

        public BasePlayerLogic()
        {
            mInstance = new BaseGameLogic<IPlayerLogic>(eGameLogicType.PlayerLogic, (short)this.LogicType, this);
        }

        public virtual bool OnIdle(PlayerInstance player)
        {
            return true;
        }

        public virtual bool OnQuest(PlayerInstance player)
        {
            return true;
        }

        public virtual bool AcceptQuest(PlayerInstance player)
        {
            return true;
        }

        public virtual bool SubmitQuest(PlayerInstance player)
        {
            return true;
        }

        public virtual bool OnFollowTarget(PlayerInstance player)
        {
            return true;
        }

        public virtual bool FollowTarget(PlayerInstance player, RoleActor target)
        {
            if (null == player)
                return false;

            if (null == target || target.IsDie)
                return false;

            return true;
        }

        public virtual bool SelectTarget(PlayerInstance player)
        {
            return true;
        }

        public virtual bool OnKilled(PlayerInstance player)
        {
            player.ChangeState(ePlayerState.Dead);
            player.mAttackTarget = null;

            return true;
        }

        public virtual bool OnDead(PlayerInstance player)
        {
            return true;
        }

        public virtual bool Revive(PlayerInstance player)
        {
            return true;
        }

        public virtual bool OnFixedBody(PlayerInstance player)
        {
            return true;
        }

        public virtual bool OnPause(PlayerInstance player)
        {
            return true;
        }

    }
}
