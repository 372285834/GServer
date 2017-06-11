using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon.Planes
{
    public class PlanesInstance 
    {
        public static PlanesInstance NullPlanesInstance = new PlanesInstance(new CSCommon.Data.PlanesData());

        CSCommon.Data.PlanesData mPlanesData;
        public CSCommon.Data.PlanesData PlanesData
        {
            get { return mPlanesData; }
        }
        public ushort PlanesId
        {
            get { return mPlanesData.PlanesId; }
        }

        public PlanesInstance(CSCommon.Data.PlanesData planesData)
        {
            mPlanesData = planesData;
            if (mPlanesData.PlanesId >= 2048)
            {
                Log.Log.Server.Error("位面ID不能超过2048！！！！！！！！！！！,不然创建唯一id可能会有重复！！！");
            }
            if (mPlanesData.PlanesId!=0)
            {//为0的位面是临时副本逻辑位面，不是真正的服务器
                ServerFrame.Util.InitServerID(mPlanesData.PlanesId);
            }
        }

        Dictionary<ushort, MapInstance> mGlobalMaps = new Dictionary<ushort, MapInstance>();
        public Dictionary<ushort, MapInstance> GlobalMaps
        {
            get { return mGlobalMaps; }
        }
        public MapInstance GetGlobalMap(ushort mapSourceId)
        {
            MapInstance result;
            if (mGlobalMaps.TryGetValue(mapSourceId, out result))
                return result;
            return null;
        }

        public void AddGlobalMap(ushort mapSourceId, MapInstance map)
        {
            mGlobalMaps[mapSourceId] = map;
            //yzb  在世界地图创建的时候，刷新国战信息,并在地图上呈现
            PlanesCountryWar.CCountryWarMgr.Instance.RefreshCountryWar(PlanesId,mapSourceId);
        }

        Dictionary<ulong, PlayerInstance> mPlayers = new Dictionary<ulong, PlayerInstance>();
        public Dictionary<ulong, PlayerInstance> AllPlayers
        {//这里现在纯粹就是一个统计功能，不能做任何逻辑处理，因为同一个位面的不同地图，可能运行在多个物理服务器进程上
            get { return mPlayers; }
        }


        public void EnterPlanes(PlayerInstance player)
        {
            if (player.PlanesInstance == this)
                return;

            lock (this)
            {
                mPlayers[player.Id] = player;
                player.PlanesInstance = this;
                if (this.PlanesId != 0)//没有在副本里面，副本没有PlanesId
                    player.PlayerData.RoleDetail.PlanesId = this.PlanesId;
            }

            {
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_DataServer(pkg).HGet_PlayerManager(pkg).RoleEnterPlanesSuccessed(pkg, player.Id);
                pkg.DoCommand(IPlanesServer.Instance.DataConnect, RPC.CommandTargetType.DefaultType);
            }

            {
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_RoleEnterPlanes(pkg, player.PlayerData.RoleDetail);
                pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
                {
                    if (isTimeOut)
                        return;
                    CSCommon.Data.RankData rd = new CSCommon.Data.RankData();
                    _io.Read(rd);
                    player.PlayerData.RankData = rd;
                    player.PlayerData.RankData._SetHostPlayer(player);
                };
            }
        }
        public void LeavePlanes(ulong roleId)
        {
            OffPlayerData data = new OffPlayerData();
            lock (this)
            {
                var player = mPlayers[roleId];
                if (player != null)
                {
                    player.CalcAllOffValues();
                    data.value = player.FinalRoleValue;
                    var equip = player.EquipBag.FindItemByPos((int)CSCommon.eEquipType.Soul);
                    if (equip == null)
                    {
                        equip = player.EquipBag.FindItemByPos((int)CSCommon.eEquipType.Weapon);
                    }
                    data.WeapFacdeid = equip.ItemTemplate.id;
                    data.skills = player.PlayerData.SkillDatas;

                    player.PlanesInstance = null;
                }
                mPlayers.Remove(roleId);
            }

            {
                if (data.value != null)
                {
                    byte[] offvaluebytes = data.Serialize();
                    RPC.PackageWriter pkg = new RPC.PackageWriter();
                    H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_RoleLogout(pkg, roleId, offvaluebytes);
                    pkg.DoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType);
                }
            }
        }
    }
}
