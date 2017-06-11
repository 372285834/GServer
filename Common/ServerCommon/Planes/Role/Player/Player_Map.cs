using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;

namespace ServerCommon.Planes
{
    public partial class PlayerInstance : RPC.RPCObject
    {
        #region Trigger

        // 客户端通知服务器我进入了某Trigger
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_EnterTrigger(ulong triggerId, RPC.RPCForwardInfo fwd)
        {
            // 验证玩家位置合法性
            var trigger = this.HostMap.GetTrigger((int)triggerId);
            if (trigger == null)
                return;

            var ret = trigger.TryDoTrigger(this, fwd, 1.3f);

            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            retPkg.Write((byte)ret);
            retPkg.DoReturnPlanes2Client(fwd);


        }

        // 客户端通知服务器我离开了某Trigger
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_LeaveTrigger(ulong triggerId, RPC.RPCForwardInfo fwd)
        {
//             var trigger = this.HostMap.GetTrigger(triggerId);
//             if (trigger == null)
//                 return;
// 
//             trigger.ProcessActorLeave(this, 0.7f);
        }

        #endregion

        public void ReturnToHostMap(RPC.RPCForwardInfo fwd)
        {
            ushort mapSourceId = mPlayerData.RoleDetail.MapSourceId;
            float posX = mPlayerData.RoleDetail.LocationX;
            float posY = mPlayerData.RoleDetail.LocationY;
            float posZ = mPlayerData.RoleDetail.LocationZ;
            JumpToMap(mapSourceId, posX, posY, posZ, fwd);
        }

        public void JumpToMap(ushort mapSourceId, float posX, float posY, float posZ, RPC.RPCForwardInfo fwd)
        {
            if (mapSourceId == this.HostMap.MapSourceId)
            {
//                 var retPkg = new RPC.PackageWriter();
//                 retPkg.Write((sbyte)eRet_GotoMap.SameMap);
//                 retPkg.DoReturnPlanes2Client(fwd);
                return;
            }

            ushort targetPlanesId = this.PlayerData.RoleDetail.PlanesId;
            if (AllMapManager.IsInstanceMap(mapSourceId))
            {
                targetPlanesId = 0;
            }

            SlimDX.Vector3 pos = new SlimDX.Vector3(posX, posY, posZ);
            var pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).GotoMap(pkg, Id, targetPlanesId, mapSourceId, pos, fwd.Handle);
            pkg.WaitDoCommand(IPlanesServer.Instance.DataConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                sbyte successed = -1;
                _io.Read(out successed);
                switch ((eRet_GotoMap)successed)
                {
                    case eRet_GotoMap.FailedEnterMap:
                    case eRet_GotoMap.NoConnectInfo:
                    case eRet_GotoMap.NoPlayerData:
                        {
                            _JumpMapDoReturnClient(fwd, successed);
                            return;
                        }
                        break;
                    case eRet_GotoMap.SamePlane:
                        {
                            Planes.MapInstanceManager.Instance.PlayerLeaveMap(this, false);//离开地图
                            if (this.PlanesInstance != null)
                                this.PlanesInstance.LeavePlanes(this.Id);//离开位面

                            CSCommon.Data.PlanesData planesData = new CSCommon.Data.PlanesData();
                            _io.Read(planesData);
                            ulong mapInstanceId;
                            _io.Read(out mapInstanceId);
                            var planesInstance = Planes.MapInstanceManager.Instance.PlanesManager.GetPlanesInstance(planesData);
                            planesInstance.EnterPlanes(this);//进位面

                            Planes.MapInstanceManager.Instance.PlayerEnterMap(this, mapSourceId, mapInstanceId, pos, true);//进地图
                            var retPkg = new RPC.PackageWriter();
                            Wuxia.H_RpcRoot.smInstance.RPC_OnJumpMapOver(retPkg, (int)mapSourceId, pos.X, pos.Z);
                            retPkg.DoCommandPlanes2Client(this.Planes2GateConnect,this.ClientLinkId);
                        }
                        break;                        
                    case eRet_GotoMap.OtherPlane:
                        {
                            Planes.MapInstance map = HostMap;
                            Planes.PlanesInstance planes = PlanesInstance;
                            if (map == null || planes == null)
                            {
                                _JumpMapDoReturnClient(fwd, (sbyte)(eRet_GotoMap.NullMapOrNoPlane));
                                return;
                            }
                            else
                            {
                                map.PlayerLeaveMap(this, true);//退出地图，这里已经存盘过了
                                planes.LeavePlanes(Id);//退出位面
                                Planes.PlanesServerDataManager.Instance.RemovePlayerInstance(this);//退出服务器
                                 
                                var retPkg = new RPC.PackageWriter();
                                retPkg.Write("跳转成功2");
                                retPkg.DoReturnPlanes2Client(fwd);
                                return;
                            }
                        }
                }
            };
        }

        private static void _JumpMapDoReturnClient(RPC.RPCForwardInfo fwd, sbyte successed)
        {
//             if (fwd != null)
//             {
//                 var retPkg = new RPC.PackageWriter();
//                 retPkg.Write((sbyte)successed);
//                 retPkg.DoReturnPlanes2Client(fwd);
//             }
        }


        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_OnClientEnterMapOver(RPC.RPCForwardInfo fwd)
        {
            var retPkg = new RPC.PackageWriter();
            retPkg.SetSinglePkg();

            // 通知客户端周围玩家及NPC
            if (!mHostMap.IsNullMap)
            {
                retPkg.Write((sbyte)1);
                mHostMap.OnClientEnterMapOver(this);
                var loc = GetPosition();
                List<RoleActor> roleList = new List<RoleActor>();

                // 先同步NPC（todo: 如果同步压力过大则区分功能NPC和其他NPC进行同步，以便减小同步压力）
                //RPC.DataWriter sceneDW = new RPC.DataWriter();
                UInt32 actorTypes = (1 << (Int32)eActorGameType.Npc);// | (1<<(Int32)CSCommon.Component.EActorGameType.Player);
                mHostMap.TourRoles(ref loc, RPC.RPCNetworkMgr.Sync2ClientRange, actorTypes, this.OnTellClientNPC, roleList);
                Byte npcCount = (Byte)(roleList.Count);
                retPkg.Write(npcCount);
                foreach (NPCInstance npc in roleList)
                {
                    retPkg.Write(npc.NPCData);
                }

                // 同步玩家镜像
                roleList.Clear();
                actorTypes = (1 << (Int32)eActorGameType.PlayerImage);
                mHostMap.TourRoles(ref loc, RPC.RPCNetworkMgr.Sync2ClientRange, actorTypes, this.OnTellClientPlayer, roleList);
                Byte imageCount = (Byte)(roleList.Count);
                retPkg.Write(imageCount);
                foreach (var image in roleList)
                {
                    CSCommon.Data.RoleSyncInfo info = new CSCommon.Data.RoleSyncInfo((PlayerImage)image);
                    retPkg.Write(info);
                }

                // 再同步玩家
                roleList.Clear();
                actorTypes = (1 << (Int32)eActorGameType.Player);
                mHostMap.TourRoles(ref loc, RPC.RPCNetworkMgr.Sync2ClientRange, actorTypes, this.OnTellClientPlayer, roleList);
                Byte playerCount = (Byte)(roleList.Count - 1);//需要去掉自己
                if (playerCount < 0)
                {
                    Log.Log.Server.Error("怎么可能！至少有自己吧！");
                }
                retPkg.Write(playerCount);    
                foreach (var player in roleList)
                {
                    if (player.Id == this.Id)
                        continue;

                    CSCommon.Data.RoleSyncInfo info = new CSCommon.Data.RoleSyncInfo((PlayerInstance)player);
                    retPkg.Write(info);
                }

                //RPC.DataWriter stateParamDW = new RPC.DataWriter();
                //stateParamDW.Write(this.CurrentState.Parameter, true);
            }
            else
                retPkg.Write((sbyte)0);


            retPkg.DoReturnPlanes2Client(fwd);
        }

        /// <summary>
        /// 开始运粮
        /// </summary>
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_SendFoodCar(int level, int num, RPC.RPCForwardInfo fwd)
        {
            sbyte result = 0;
            eCamp tempCamp = eCamp.Song;
            var tplWay = CommonUtil.GetMapWayDataByType(tempCamp, eNpcType.FoodCar);
            if (null == tplWay)
            {
                Log.Log.Server.Error("RPC_SendFoodCar.GetMapWayDataByType is null: {0}, {1}", (int)Camp, (int)eNpcType.FoodCar);
            }
            else
            {
                var map = MapInstanceManager.Instance.GetMapBySourceId(tplWay.startmapID);
                if (null == map)
                {
                    Log.Log.Server.Error("RPC_SendFoodCar.GetMapInstance is null: {0}", tplWay.startmapID);
                    map = Planes.MapInstanceManager.Instance.CreateMapInstance(mHostMap.Planes, 0, (ushort)tplWay.startmapID, null);
                    if (map == null)
                    {
                        map = Planes.MapInstanceManager.Instance.GetDefaultMapInstance(mHostMap.Planes);
                    }
                    mHostMap.Planes.AddGlobalMap((ushort)tplWay.startmapID, map);
                }
                foreach (var npcData in map.MapInfo.MapDetail.NpcList)
                {
                    if (npcData.camp == (int)tempCamp)
                    {
                        var tplNpc = CSTable.StaticDataManager.NPCTpl[npcData.tid];
                        if (tplNpc.type == (int)eNpcType.FoodCar)
                        {
                            map.PushCreatingNpc(eNpcType.FoodCar, npcData);
                            result = 1;
                            break;
                        }
                    }
                }
            }
            var retPkg = new RPC.PackageWriter();
            retPkg.Write(result);
            retPkg.DoReturnPlanes2Client(fwd);
        }
    }

}
