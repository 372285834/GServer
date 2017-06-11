using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 地图cell，每个地图由多个cell组成
    /// </summary>
    public class MapCellInstance
    {
        int m_x;
        public int X
        {
            get { return m_x; }
        }
        int m_y;
        public int Y
        {
            get { return m_y; }
        }

        Dictionary<ulong, RoleActor> mRoleDictionary = new Dictionary<ulong, RoleActor>();
        public Dictionary<ulong, RoleActor> RoleDictionary
        {
            get { return mRoleDictionary; }
        }

        List<PlayerInstance> m_players = new List<PlayerInstance>();
        public List<PlayerInstance> Players
        {
            get { return m_players; }
        }

        List<PlayerImage> m_images = new List<PlayerImage>();
        public List<PlayerImage> Images
        {
            get { return m_images; }
        }

        List<NPCInstance> m_npcs = new List<NPCInstance>();
        public List<NPCInstance> NPCs
        {
            get { return m_npcs; }
        }

        //List<RoleActor> m_EffectNpcs = new List<RoleActor>();
        //public List<RoleActor> EffectNpcs
        //{
        //    get { return m_EffectNpcs; }
        //}

        //List<DropedItem.DropedItemRole> mDropedItems = new List<DropedItem.DropedItemRole>();
        //public List<DropedItem.DropedItemRole> DropedItems
        //{
        //    get { return mDropedItems; }
        //}

        //List<Gather.GatherItem> mGatherItems = new List<Gather.GatherItem>();
        //public List<Gather.GatherItem> GatherItems
        //{
        //    get { return mGatherItems; }
        //}

        public MapCellInstance(int x, int y)
        {
            m_x = x;
            m_y = y;
        }

        public void AddRole(RoleActor role)
        {
            if (role.HostMapCell == this)
                return;

            if (role.HostMapCell != null)
            {
                role.HostMapCell.RemoveRole(role);
            }

            if (!RoleDictionary.ContainsKey(role.Id))
            {
                RoleDictionary[role.Id] = role;

                if (role is PlayerInstance)
                {
                    Players.Add(role as PlayerInstance);
                }
                else
                {
                    switch (role.GameType)
                    {
                        case eActorGameType.Npc:
                            NPCs.Add(role as NPCInstance);
                            break;
                        case eActorGameType.PlayerImage:
                            Images.Add(role as PlayerImage);
                            break;
//                         case CSCommon.Component.eActorGameType.EffectNpc:
//                             EffectNpcs.Add(role);
//                             break;
                        //case CSCommon.Component.eActorGameType.DropedItem:
                        //    DropedItems.Add(role as DropedItem.DropedItemRole);
                        //    break;
//                         case CSCommon.Component.eActorGameType.GatherItem:
//                             GatherItems.Add(role as Gather.GatherItem);
//                             break;
                        default:
                            break;
                    }
                }

                role.HostMapCell = this;
            }
        }

        public void RemoveRole(RoleActor role)
        {
            RoleDictionary.Remove(role.Id);
            role.HostMapCell = null;

            if (role is PlayerInstance)
            {
                Players.Remove(role as PlayerInstance);
            }
            else
            {
                switch (role.GameType)
                {
                    case eActorGameType.Npc:
                        NPCs.Remove(role as NPCInstance);
                        break;
                    case eActorGameType.PlayerImage:
                        Images.Remove(role as PlayerImage);
                        break;
//                     case CSCommon.Component.eActorGameType.EffectNpc:
//                         EffectNpcs.Remove(role);
//                         break;
                    //case CSCommon.Component.eActorGameType.DropedItem:
                    //    DropedItems.Remove(role as DropedItem.DropedItemRole);
                    //    break;
//                     case CSCommon.Component.eActorGameType.GatherItem:
//                         GatherItems.Remove(role as Gather.GatherItem);
//                         break;
                    default:
                        break;
                }
            }
        }

        //// TODO: 增加筛选条件，如根据阵营来筛选结果
        //public List<RoleActor> HandleRoleCollide(Summon.SummonRole summon, RoleActor ignore)
        //{
        //    //这里以后最好分和哪些类型对象碰撞来做，这样效率高
        //    List<RoleActor> retList = new List<RoleActor>();

        //    foreach (var role in mRoleDictionary)
        //    {
        //        if (role.Value.Id == ignore.Id)
        //            continue;
        //        if (role.Value.Id == summon.OwnerRole.Id)
        //            continue;

        //        var distance = summon.SummonData.Position - role.Value.GetPosition();
        //        if (distance.Length() < summon.SummonData.RuneData.Template.Radius)
        //        {
        //            retList.Add(role.Value);
        //        }
        //    }

        //    return retList;
        //}


        // 将player的信息同步给该格子中的其他玩家, 返回值是实际同步的玩家数量
        public int SendPkg2Client(PlayerInstance player, int maxCount, Random rand, RPC.PackageWriter pkg)
        {
            int retValue = maxCount;
            if (Players.Count <= maxCount)
            {
                retValue = Players.Count;
                foreach (var role in Players)
                {
                    if (player != null && role.Id == player.Id)
                    {
                        retValue--;
                        continue;
                    }

                    pkg.DoCommandPlanes2Client(role.Planes2GateConnect, role.ClientLinkId);
                }
            }
            else
            {
                retValue = maxCount;
                int startIdx = rand.Next(Players.Count - maxCount);
                for (int i = startIdx; i < startIdx + maxCount; i++)
                {
                    if (player != null && Players[i].Id == player.Id)
                    {
                        retValue--;
                        continue;
                    }

                    pkg.DoCommandPlanes2Client(Players[i].Planes2GateConnect, Players[i].ClientLinkId);
                }
            }

            return retValue;
        }

        // 将格子中的信息同步给某个玩家，返回值是实际同步的玩家数量
        public int AsyncMapCellDataToClient(PlayerInstance player, int maxCount, Random rand, RPC.DataWriter data)
        {
            int retValue = maxCount;
            if (Players.Count <= maxCount)
            {
                if (Players.Contains(player))
                    retValue = Players.Count - 1;
                else
                    retValue = Players.Count;

                data.Write((Byte)retValue);

                foreach (var role in Players)
                {
                    if (role.Id == player.Id)
                    {
                        continue;
                    }

                    data.Write(role.PlayerData.RoleDetail, true);
                    data.Write(role.Id);
                }
            }
            else
            {
                int startIdx = rand.Next(Players.Count - maxCount);

                if (Players.IndexOf(player) >= startIdx)
                    retValue = maxCount - 1;
                else
                    retValue = maxCount;

                data.Write((Byte)retValue);

                for (int i = startIdx; i < startIdx + maxCount; i++)
                {
                    if (Players[i].Id == player.Id)
                    {
                        continue;
                    }

                    data.Write(Players[i].PlayerData.RoleDetail, true);
                    data.Write(Players[i].Id);
                }
            }

            return retValue;
        }

        //         public void TellOthersThisRoleLeaveMap(PlayerInstance player)
        //         {
        //             foreach (var role in Players)
        //             {
        //                 var pkg = new RPC.PackageWriter();
        //                 // ClientRPC.H_RootObject.smInstance.HGet_Scene(pkg).RoleLeaveScene(pkg, player.RoleId);
        //                 pkg.DoCommandPlanes2Client(role.Planes2GateConnect, role.ClientLinkId);
        //             }
        //         }
    }
}
