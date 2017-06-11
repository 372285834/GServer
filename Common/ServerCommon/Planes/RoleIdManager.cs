using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    //Guid到Int32的一个映射
    public class RoleIdManager
    {
        enum EIdIndexInfo : ulong
        {
            NpcStart = 1024,
            MaxNpcInPlanes = 65536*10,
            NpcEnd = NpcStart+MaxNpcInPlanes,

            PlayerStart = NpcEnd + 1,
            MaxPlayerInPlanes = 65536,
            PlayerEnd = PlayerStart+MaxPlayerInPlanes,

            SummonStart = PlayerEnd + 1,
            MaxSummonInPlanes = 65536*10,
            SummonEnd = SummonStart+MaxSummonInPlanes,

            DropedItemStart = SummonEnd + 1,
            MaxDropedItemInPlanes = 65536*10,
            DropedItemEnd = DropedItemStart + MaxDropedItemInPlanes,

            TriggerStart = DropedItemEnd + 1,
            MaxTriggerInPlanes = 65536,
            TriggerEnd = TriggerStart + MaxTriggerInPlanes,
        };

        PlanesInstance mPlanes;
        public RoleIdManager(PlanesInstance p)
        {
            mPlanes = p;
        }
        
        #region NPC
        UInt32 mNextNpcId = (UInt32)EIdIndexInfo.NpcStart;
        Dictionary<Guid, NPCInstance> mTableNpcs = new Dictionary<Guid, NPCInstance>();
        public Dictionary<Guid, NPCInstance> TableNPCs
        {
            get { return mTableNpcs; }
        }
        Dictionary<UInt32, NPCInstance> mSingeIdNPCs = new Dictionary<UInt32, NPCInstance>();
        public Dictionary<UInt32, NPCInstance> SingeIdNPCs
        {
            get { return mSingeIdNPCs; }
        }

        public bool MapNpcGuid2UInt32(NPCInstance role)
        {
            if (role.SingleId != 0)
                return false;
            NPCInstance temp;
            if (mTableNpcs.TryGetValue(role.Id, out temp))
                return false;
            UInt32 singleId = 0;
            for(int count = 0;count<(UInt32)EIdIndexInfo.MaxNpcInPlanes;count++)
            {
                if (mSingeIdNPCs.TryGetValue(mNextNpcId, out temp) == false)
                {
                    singleId = mNextNpcId;
                    mNextNpcId++;
                    break;
                }
                else
                {
                    ++mNextNpcId;
                    if (mNextNpcId >= (UInt32)EIdIndexInfo.NpcEnd)
                        mNextNpcId = (UInt32)EIdIndexInfo.NpcStart;
                }
            }

            if (singleId == 0)
            {
                return false;
            }
            role._SetSingleId(singleId);
            mTableNpcs.Add(role.Id, role);
            mSingeIdNPCs.Add(singleId, role);
            return true;
        }

        public void UnmapNpcGuid2UInt32(NPCInstance role)
        {
            mTableNpcs.Remove(role.Id);
            mSingeIdNPCs.Remove(role.SingleId);
            role._SetSingleId(0);
        }

        public NPCInstance FindNPC(Guid npcId)
        {
            NPCInstance npc;
            if (false == mTableNpcs.TryGetValue(npcId, out npc))
                return null;

            return npc;
        }

        public NPCInstance FindNPC(UInt32 singleId)
        {
            NPCInstance npc;
            if (false == mSingeIdNPCs.TryGetValue(singleId, out npc))
                return null;

            return npc;
        }
        #endregion

        #region Player
        UInt32 mNextPlayerId = (UInt32)EIdIndexInfo.PlayerStart;
        Dictionary<Guid, PlayerInstance> mPlayers = new Dictionary<Guid, PlayerInstance>();
        Dictionary<UInt32, PlayerInstance> mSingeIdPlayers = new Dictionary<UInt32, PlayerInstance>();

        public bool MapPlayerGuid2UInt32(PlayerInstance role)
        {
            if (role.SingleId != 0)
                return false;
            PlayerInstance temp;
            if (mPlayers.TryGetValue(role.Id, out temp))
                return false;
            UInt32 singleId = 0;
            for (int count = 0; count < (UInt32)EIdIndexInfo.MaxPlayerInPlanes; count++)
            {
                if (mSingeIdPlayers.TryGetValue(mNextPlayerId, out temp) == false)
                {
                    singleId = mNextPlayerId;
                    mNextPlayerId++;
                    break;
                }
                else
                {
                    ++mNextPlayerId;
                    if (mNextPlayerId >= (UInt32)EIdIndexInfo.PlayerEnd)
                        mNextPlayerId = (UInt32)EIdIndexInfo.PlayerStart;
                }
            }

            if (singleId == 0)
            {
                return false;
            }
            role._SetSingleId(singleId);
            mPlayers.Add(role.Id, role);
            mSingeIdPlayers.Add(singleId, role);
            return true;
        }

        public void UnmapPlayerGuid2UInt32(PlayerInstance player)
        {
            mPlayers.Remove(player.Id);
            mSingeIdPlayers.Remove(player.SingleId);
            player._SetSingleId(0);
        }

        public PlayerInstance FindPlayer(Guid roleId)
        {
            PlayerInstance role;
            if (false == mPlayers.TryGetValue(roleId, out role))
                return null;
            return role;
        }

        public PlayerInstance FindPlayer(UInt32 singleId)
        {
            PlayerInstance role;
            if (false == mSingeIdPlayers.TryGetValue(singleId, out role))
                return null;
            return role;
        }

        public PlayerInstance FindPlayer(string name)
        {
            foreach (var i in mPlayers)
            {
                if (i.Value.PlayerData.RoleDetail.RoleName == name)
                {
                    return i.Value;
                }
            }
            return null;
        }

        public int GetPlayerCount()
        {
            return mPlayers.Count;
        }
        #endregion

        #region Summon
        UInt32 mNextSummonId = (UInt32)EIdIndexInfo.SummonStart;
        Dictionary<Guid, Role.Summon.SummonRole> mTableSummons = new Dictionary<Guid, Role.Summon.SummonRole>();
        Dictionary<UInt32, Role.Summon.SummonRole> mSingeIdSummons = new Dictionary<UInt32, Role.Summon.SummonRole>();

        public bool MapSummonGuid2UInt32(Role.Summon.SummonRole role)
        {
            if (role.SingleId != 0)
                return false;
            Role.Summon.SummonRole temp;
            if (mTableSummons.TryGetValue(role.Id, out temp))
                return false;
            UInt32 singleId = 0;
            for (int count = 0; count < (UInt32)EIdIndexInfo.MaxSummonInPlanes; count++)
            {
                if (mSingeIdSummons.TryGetValue(mNextSummonId, out temp) == false)
                {
                    singleId = mNextSummonId;
                    mNextSummonId++;
                    break;
                }
                else
                {
                    ++mNextSummonId;
                    if (mNextSummonId >= (UInt32)EIdIndexInfo.SummonEnd)
                        mNextSummonId = (UInt32)EIdIndexInfo.SummonStart;
                }
            }

            if (singleId == 0)
            {
                return false;
            }
            role._SetSingleId(singleId);
            mTableSummons.Add(role.Id, role);
            mSingeIdSummons.Add(singleId, role);
            return true;
        }

        public void UnmapSummonGuid2UInt32(Role.Summon.SummonRole role)
        {
            mTableSummons.Remove(role.Id);
            mSingeIdSummons.Remove(role.SingleId);
            role._SetSingleId(0);
        }

        public Role.Summon.SummonRole FindSummonRole(Guid roleId)
        {
            Role.Summon.SummonRole role;
            if (false == mTableSummons.TryGetValue(roleId, out role))
                return null;

            return role;
        }

        public Role.Summon.SummonRole FindSummonRole(UInt32 singleId)
        {
            Role.Summon.SummonRole role;
            if (false == mSingeIdSummons.TryGetValue(singleId, out role))
                return null;

            return role;
        }
        #endregion

        #region DropedItemRole
        UInt32 mNextDropedItemId = (UInt32)EIdIndexInfo.DropedItemStart;
        Dictionary<Guid, Role.DropedItem.DropedItemRole> mTableDropedItems = new Dictionary<Guid, Role.DropedItem.DropedItemRole>();
        Dictionary<UInt32, Role.DropedItem.DropedItemRole> mSingeIdDropedItems = new Dictionary<UInt32, Role.DropedItem.DropedItemRole>();

        public bool MapDropedItemGuid2UInt32(Role.DropedItem.DropedItemRole role)
        {
            if (role.SingleId != 0)
                return false;
            Role.DropedItem.DropedItemRole temp;
            if (mTableDropedItems.TryGetValue(role.Id, out temp))
                return false;
            UInt32 singleId = 0;
            for (int count = 0; count < (UInt32)EIdIndexInfo.MaxDropedItemInPlanes; count++)
            {
                if (mSingeIdDropedItems.TryGetValue(mNextDropedItemId, out temp) == false)
                {
                    singleId = mNextDropedItemId;
                    mNextDropedItemId++;
                    break;
                }
                else
                {
                    ++mNextDropedItemId;
                    if (mNextDropedItemId >= (UInt32)EIdIndexInfo.SummonEnd)
                        mNextDropedItemId = (UInt32)EIdIndexInfo.SummonStart;
                }
            }

            if (singleId == 0)
            {
                return false;
            }
            role._SetSingleId(singleId);
            mTableDropedItems.Add(role.Id, role);
            mSingeIdDropedItems.Add(singleId, role);
            return true;
        }

        public void UnmapDropedItemGuid2UInt32(Role.DropedItem.DropedItemRole role)
        {
            mTableDropedItems.Remove(role.Id);
            mSingeIdDropedItems.Remove(role.SingleId);
            role._SetSingleId(0);
        }

        public Role.DropedItem.DropedItemRole FindDropedItemRole(Guid roleId)
        {
            Role.DropedItem.DropedItemRole role;
            if (false == mTableDropedItems.TryGetValue(roleId, out role))
                return null;

            return role;
        }

        public Role.DropedItem.DropedItemRole FindDropedItemRole(UInt32 singleId)
        {
            Role.DropedItem.DropedItemRole role;
            if (false == mSingeIdDropedItems.TryGetValue(singleId, out role))
                return null;

            return role;
        }
        #endregion

        #region Trigger

        UInt32 mNextTriggerId = (UInt32)EIdIndexInfo.TriggerStart;
        Dictionary<Guid, TriggerInstance> mTableTriggers = new Dictionary<Guid, TriggerInstance>();
        Dictionary<UInt32, TriggerInstance> mSingeIdTriggers = new Dictionary<UInt32, TriggerInstance>();

        public bool MapTriggerGuid2UInt32(TriggerInstance role)
        {
            if (role.SingleId != 0)
                return false;
            TriggerInstance temp;
            if (mTableTriggers.TryGetValue(role.Id, out temp))
                return false;
            UInt32 singleId = 0;
            for (int count = 0; count < (UInt32)EIdIndexInfo.MaxTriggerInPlanes; count++)
            {
                if (mSingeIdTriggers.TryGetValue(mNextTriggerId, out temp) == false)
                {
                    singleId = mNextTriggerId;
                    mNextTriggerId++;
                    break;
                }
                else
                {
                    ++mNextTriggerId;
                    if (mNextTriggerId >= (UInt32)EIdIndexInfo.TriggerEnd)
                        mNextTriggerId = (UInt32)EIdIndexInfo.TriggerStart;
                }
            }

            if (singleId == 0)
            {
                return false;
            }
            role._SetSingleId(singleId);
            mTableTriggers.Add(role.Id, role);
            mSingeIdTriggers.Add(singleId, role);
            return true;
        }

        public void UnmapTriggerGuid2UInt32(TriggerInstance role)
        {
            mTableTriggers.Remove(role.Id);
            mSingeIdTriggers.Remove(role.SingleId);
            role._SetSingleId(0);
        }

        public TriggerInstance FindTrigger(Guid triggerId)
        {
            TriggerInstance trigger;
            if (false == mTableTriggers.TryGetValue(triggerId, out trigger))
                return null;

            return trigger;
        }

        public TriggerInstance FindTrigger(UInt32 singleId)
        {
            TriggerInstance trigger;
            if (false == mSingeIdTriggers.TryGetValue(singleId, out trigger))
                return null;

            return trigger;
        }

        #endregion
    }
}
