using CSCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public partial class PlayerInstance : RPC.RPCObject
    {
        public static void SendTeam2Client(PlayerInstance player, RPC.DataWriter dw)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.RPC_ReceiveTeam(pkg, dw);
            pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
        }

        public static void SendToLeaveTeamPlayerToClient(PlayerInstance player)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.RPC_ReceiveLeaveTeam(pkg);
            pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
        }
        

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, true)]
        public void RPC_CreateTeam(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_CreateTeam(pkg, PlayerData.RoleDetail.RoleId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result = _io.ReadSByte();
                retPkg.Write(result);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, true)]
        public void RPC_InviteToTeam(string otherName, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_InviteToTeam(pkg, PlayerData.RoleDetail.RoleId, otherName);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result = _io.ReadSByte();
                retPkg.Write(result);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, true)]
        public void RPC_KickOutTeam(ulong otherId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_KickOutTeam(pkg, PlayerData.RoleDetail.RoleId, otherId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result = _io.ReadSByte();
                retPkg.Write(result);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, true)]
        public void RPC_LeaveTeam(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_LeaveTeam(pkg, PlayerData.RoleDetail.RoleId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result = _io.ReadSByte();
                retPkg.Write(result);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, true)]
        public void RPC_OperateTeamAsk(string otherName, byte operate, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_OperateTeamAsk(pkg, PlayerData.RoleDetail.RoleId, otherName, operate);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result = _io.ReadSByte();
                retPkg.Write(result);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, true)]
        public void RPC_OperateTeamInvite(string otherName, byte operate, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_OperateTeamInvite(pkg, PlayerData.RoleDetail.RoleId, otherName, operate);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result = _io.ReadSByte();
                retPkg.Write(result);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, true)]
        public void RPC_GetTeamPlayers(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_GetTeamPlayers(pkg, PlayerData.RoleDetail.RoleId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.SetSinglePkg();
                sbyte result = _io.ReadSByte();
                retPkg.Write(result);
                if (result == (sbyte)CSCommon.eRet_Team.Succeed)
                {
                    byte count = _io.ReadByte();
                    retPkg.Write(count);
                    for (byte i = 0; i < count; i++)
                    {
                        RoleCom data = new RoleCom();
                        _io.Read(data);
                        retPkg.Write(data);
                    }
                }
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }
    }
}
