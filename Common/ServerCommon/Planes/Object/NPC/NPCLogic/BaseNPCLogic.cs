using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;

namespace ServerCommon.Planes
{
    public class BaseNPCLogic : INPCLogic
    {
        public virtual eNPCLogicType LogicType { get { return eNPCLogicType.None; } }

        public static bool LoadAllLogic()
        {
            var assmbly = System.Reflection.Assembly.GetExecutingAssembly();
            var types = assmbly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsSubclassOf(typeof(BaseNPCLogic)))
                    continue;

                System.Activator.CreateInstance(type);
            }

            return true;
        }

        BaseGameLogic<INPCLogic> mInstance;

        public BaseNPCLogic()
        {
            mInstance = new BaseGameLogic<INPCLogic>(eGameLogicType.NPCLogic, (short)this.LogicType, this);
        }

        public virtual bool OnIdle(NPCInstance npc)
        {
            return true;
        }

        public virtual bool OnPatrol(NPCInstance npc)
        {
            return true;
        }

        public virtual bool StartPatrol(NPCInstance npc)
        {
            return true;
        }

        public virtual bool OnFollowTarget(NPCInstance npc)
        {
            return true;
        }

        public virtual bool FollowTarget(NPCInstance npc, RoleActor target)
        {
            if (null == npc)
                return false;

            if (null == target || target.IsDie)
                return false;

            return true;
        }

        public virtual bool SelectTarget(NPCInstance npc)
        {
            RoleActor target = null;
            npc.mScanLastTime = 0;
            if (npc.mAttackTarget != 0)
            {
                target = npc.GetTarget(npc.mAttackTarget);
                if (null != target)
                {
                    if (target.IsDie)
                    {
                        npc.KickOneFromEnmityList(npc.mAttackTarget);
                        npc.mAttackTarget = 0;
                        target = null;
                    }
                }
                else
                {
                    npc.mAttackTarget = 0;
                }
            }
            //从警戒范围找目标
            if (null == target)
            {
                target = npc.FindHatredTarget();
            }
            if (null == target)
                return false;

            npc.UpdateEnmityList(target.Id, 1);

            return true;
        }

        public virtual bool OnKilled(NPCInstance npc)
        {
            npc.ChangeState(eNPCState.Dead);
            npc.mAttackTarget = 0;

            return true;
        }

        public virtual bool OnTransport(NPCInstance npc)
        {
            return true;
        }

        public virtual bool OnWaitJumpMap(NPCInstance npc)
        {
            return true;
        }

        public virtual bool StartTransport(NPCInstance npc)
        {
            return true;
        }

        public virtual bool OnDead(NPCInstance npc)
        {
            return true;
        }

        public virtual bool Revive(NPCInstance npc)
        {
            return true;
        }

        public virtual bool OnFixedBody(NPCInstance npc)
        {
            return true;
        }

        public virtual bool OnPause(NPCInstance npc)
        {
            return true;
        }

    }
}
