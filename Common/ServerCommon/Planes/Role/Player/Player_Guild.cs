using CSCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public partial class PlayerInstance : RPC.RPCObject
    {
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetGuilds(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_GetGuilds(pkg, PlayerData.RoleDetail.RoleId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.SetSinglePkg();
                sbyte success;
                _io.Read(out success);
                retPkg.Write(success);
                if (success == (sbyte)CSCommon.eRet_GetGuilds.ReturnGuilds)
                {
                    int guildcount = 0;
                    _io.Read(out guildcount);
                    retPkg.Write(guildcount);
                    for (int i = 0; i < guildcount;i++ )
                    {
                        CSCommon.Data.GuildCom guild = new CSCommon.Data.GuildCom();
                        _io.Read(guild);
                        retPkg.Write(guild);
                    }
                }
                else if (success == (sbyte)CSCommon.eRet_GetGuilds.ReturnGuildMembers)
                {
                    CSCommon.Data.GuildCom ownguild = new CSCommon.Data.GuildCom();
                    _io.Read(ownguild);
                    retPkg.Write(ownguild);
                    int membercount = 0;
                    _io.Read(out membercount);
                    retPkg.Write(membercount);
                    for (int j = 0; j < membercount; j++)
                    {
                        RoleCom roleData = new RoleCom();
                        _io.Read(roleData);
                        retPkg.Write(roleData);
                    }
                }
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_CreateGuild(string GuildName, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            if (!_IsLevelEnough(CSCommon.GuildCommon.Instance.NeedRoleLevel))
            {
                retPkg.Write((sbyte)CSCommon.eRet_CreateGuild.LessLevel);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (!_IsMoneyEnough(CSCommon.GuildCommon.Instance.NeedCurrenceType, CSCommon.GuildCommon.Instance.NeedMoneyNum))
            {
                retPkg.Write((sbyte)CSCommon.eRet_CreateGuild.LessRmb);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }

            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_CreateGuild(pkg, PlayerData.RoleDetail.RoleId, GuildName);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;

                SByte success;
                _io.Read(out success);
                retPkg.Write(success);
                if (success == (sbyte)CSCommon.eRet_CreateGuild.Succeed)
                {
                    _ChangeMoney(CSCommon.GuildCommon.Instance.NeedCurrenceType, CSCommon.Data.eMoneyChangeType.CreateGuild, -CSCommon.GuildCommon.Instance.NeedMoneyNum);
                }
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_LeaveGuild(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_LeaveGuild(pkg, PlayerData.RoleDetail.RoleId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                SByte success;
                _io.Read(out success);
                retPkg.Write(success);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_AskToGuild(ulong Id, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_AskToGuild(pkg, PlayerData.RoleDetail.RoleId, Id);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                if (isTimeOut)
                    return;

                sbyte success;
                _io.Read(out success);
                retPkg.Write(success);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_OperateGuildAsk(ulong messageId, byte operate, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_OperateGuildAsk(pkg, PlayerData.RoleDetail.RoleId, messageId, operate);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte success;
                _io.Read(out success);
                retPkg.Write(success);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_KickedOutGuild(ulong id, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_KickedOutGuild(pkg, PlayerData.RoleDetail.RoleId, Id);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte success;
                _io.Read(out success);
                retPkg.Write(success);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_DissolveGuild(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_DissolveGuild(pkg, PlayerData.RoleDetail.RoleId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte success;
                _io.Read(out success);
                retPkg.Write(success);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_SetGuildPost(ulong id, byte post, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_SetGuildPost(pkg, PlayerData.RoleDetail.RoleId, id, post);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte success;
                _io.Read(out success);
                retPkg.Write(success);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_InviteToGuild(string name, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_InviteToGuild(pkg, PlayerData.RoleDetail.RoleId, name);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte success;
                _io.Read(out success);
                retPkg.Write(success);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_OperateInvite(ulong roleId, byte operate, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_OperateInvite(pkg, PlayerData.RoleDetail.RoleId, roleId, operate);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte success;
                _io.Read(out success);
                retPkg.Write(success);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_TransferGuildBangZhu(ulong targetId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_TransferGuildBangZhu(pkg, PlayerData.RoleDetail.RoleId, targetId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte success;
                _io.Read(out success);
                retPkg.Write(success);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_DonateGold(int gold, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            if (_IsMoneyEnough(CSCommon.eCurrenceType.Gold, gold) == false)
            {
                retPkg.Write((sbyte)CSCommon.eRet_DonateGold.LessGold);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }

            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_DonateGold(pkg, PlayerData.RoleDetail.RoleId, gold);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                
                sbyte success;
                _io.Read(out success);
                retPkg.Write(success);
                if (success == (sbyte)CSCommon.eRet_DonateGold.Succeed)
                {
                    int todaycontribute;
                    int contribute;
                    _io.Read(out todaycontribute);
                    _io.Read(out contribute);
                    retPkg.Write(todaycontribute);
                    retPkg.Write(contribute);
                    _ChangeMoney(CSCommon.eCurrenceType.Gold, CSCommon.Data.eMoneyChangeType.GuildContribute, gold);
                }
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_IssueGold(int gold, RPC.RPCForwardInfo fwd)
        {

            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_IssueGold(pkg, PlayerData.RoleDetail.RoleId, gold);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte success;
                _io.Read(out success);
                retPkg.Write(success);
                retPkg.DoReturnPlanes2Client(fwd);
            };



        }

    }
}
