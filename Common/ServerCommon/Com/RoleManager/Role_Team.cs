using CSCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Com
{
    public class Team
    {
        List<ulong> mTeamMembers = new List<ulong>();
        public List<ulong> TeamMembers
        {
            get { return mTeamMembers; }
        }

        public ulong RemoveRole(ulong id)
        {
            if (mTeamMembers.Contains(id))
            {
                mTeamMembers.Remove(id);
            }
            else
            {
                Log.Log.Common.Print("RemoveRole !Contains{0}", id);
            }

            if (mTeamMembers.Count < 1)
            {
                return 0;
            }
            else
            {
                return mTeamMembers[0];//返回队长Id
            }
        }

        public List<RoleCom> GetPlayerInfos()
        {
            List<RoleCom> infos = new List<RoleCom>();
            for (int i = 0; i < TeamMembers.Count; i++)
            {
                var player = UserRoleManager.Instance.GetRole(TeamMembers[i]);
                if (player == null || player.RoleData == null || player.PlanesConnect == null)
                {
                    TeamMembers.RemoveAt(i);
                    i--;
                    continue;
                }
                infos.Add(player.RoleData);
            }
            return infos;
        }

        public List<UserRole> GetPlayers()
        {
            List<UserRole> infos = new List<UserRole>();
            for (int i = 0; i < TeamMembers.Count; i++)
            {
                var player = UserRoleManager.Instance.GetRole(TeamMembers[i]);
                if (player == null || player.RoleData == null || player.PlanesConnect == null)
                {
                    TeamMembers.RemoveAt(i);
                    i--;
                    continue;
                }
                infos.Add(player);
            }
            return infos;
        }
    }

    public partial class UserRoleManager : RPC.RPCObject
    {
        Dictionary<ulong, Team> mTeams = new Dictionary<ulong, Team>();
        public Dictionary<ulong, Team> Teams
        {
            get { return mTeams; }
        }

        public Team GetTeam(ulong id)
        {
            Team team;
            if (mTeams.TryGetValue(id, out team))
            {
                return team;
            }
            return null;
        }

        public void _LeaveTeam(UserRole role, ulong headerId)
        {
            var team = GetTeam(headerId);
            if (team == null)
            {
                Log.Log.Common.Print("_LeaveTeam team == null");
                return;
            }
            ulong result = team.RemoveRole(role.RoleData.RoleId);
            SendToLeaveTeamPlayer(role);
            role.SetTeamHeaderId(0);
            if (result != headerId)
            {
                mTeams.Remove(headerId);
                if (result != 0)
                {
                    mTeams[result] = team;
                    foreach (var i in team.TeamMembers)
                    {
                        var tRole = GetRole(i);
                        if (tRole != null)
                        {
                            tRole.SetTeamHeaderId(result);
                        }
                    }
                }
            }
            SendTeamInfoToPlayers(team);
            return;
        }

        public bool _CreateTeam(ulong headerId)
        {
            Team team = new Team();
            if (team == null)
            {
                return false;
            }
            mTeams[headerId] = team;
            SendTeamInfoToPlayers(team);
            return true;
        }

        int MaxMemberCount = 4;
        public bool _TeamOverMaxCount(Team team)
        {
            if (team == null)
            {
                Log.Log.Common.Print("_TeamOverMaxCount team == null");
                return true;
            }
            if (team.TeamMembers.Count >= MaxMemberCount)
            {
                return true;
            }
            return false;
        }

        public bool _AddTeam(UserRole role, ulong headerId)
        {
            var team = GetTeam(headerId);
            if (team == null)
            {
                Log.Log.Common.Print("_AddTeam team == null");
                return false;
            }
            if (team.TeamMembers.Count >= MaxMemberCount)
            {
                return false;
            }
            team.TeamMembers.Add(role.RoleData.RoleId);
            role.SetTeamHeaderId(headerId);
            SendTeamInfoToPlayers(team);
            return true;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_CreateTeam(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.TeamHeaderId != 0)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.RoleHasTeam);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (_CreateTeam(roleId) == false)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.NewTeamError);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (_AddTeam(role, roleId) == false)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.OverTeamCount);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            pkg.Write((sbyte)CSCommon.eRet_Team.Succeed);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_InviteToTeam(ulong roleId, string otherName, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var other = GetRole(otherName);
            if (other == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.NoOther);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.TeamHeaderId == roleId)//A是队长
            {
                var team = GetTeam(role.TeamHeaderId);
                if (team == null)
                {
                    role.SetTeamHeaderId(0);
                    pkg.Write((sbyte)CSCommon.eRet_Team.TeamIsNull);
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                if (_TeamOverMaxCount(team))
                {
                    pkg.Write((sbyte)CSCommon.eRet_Team.OverTeamCount);
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                if (other.TeamHeaderId != 0)
                {
                    pkg.Write((sbyte)CSCommon.eRet_Team.OtherHasTeam);
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                var msg = CreateMessage(CSCommon.eMessageFrom.Team, CSCommon.eMessageType.Team_Invite, role.RoleData.Name, other.RoleData.RoleId);
                msg.ShowInfo = string.Format("{0}邀请你加入队伍", role.RoleData.Name);
                SendMessageToOther(other, msg);
                pkg.Write((sbyte)CSCommon.eRet_Team.Succeed);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            else if (role.TeamHeaderId == 0)//A无队伍
            {
                if (other.TeamHeaderId != 0) //B有队伍
                {
                    var team = GetTeam(other.TeamHeaderId);
                    if (team == null)
                    {
                        other.SetTeamHeaderId(0);
                        pkg.Write((sbyte)CSCommon.eRet_Team.TeamIsNull);
                        pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                        return;
                    }
                    if (_TeamOverMaxCount(team))
                    {
                        pkg.Write((sbyte)CSCommon.eRet_Team.OverTeamCount);
                        pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                        return;
                    }
                    var header = GetRole(other.TeamHeaderId);
                    if (header == null)
                    {
                        other.SetTeamHeaderId(0);
                        pkg.Write((sbyte)CSCommon.eRet_Team.HeaderIsNull);
                        pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                        return;
                    }
                    var msg = CreateMessage(CSCommon.eMessageFrom.Team, CSCommon.eMessageType.Team_Ask, role.RoleData.Name, header.RoleData.RoleId);
                    msg.ShowInfo = string.Format("{0}申请加入队伍", role.RoleData.Name);
                    SendMessageToOther(header, msg);
                    pkg.Write((sbyte)CSCommon.eRet_Team.Succeed);
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                else //B无队伍
                {
                    if (false == _CreateTeam(roleId))
                    {
                        pkg.Write((sbyte)CSCommon.eRet_Team.NewTeamError);
                        pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                        return;
                    }
                    if (_AddTeam(role, roleId) == false)
                    {
                        pkg.Write((sbyte)CSCommon.eRet_Team.OverTeamCount);
                        pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                        return;
                    }
                    var msg = CreateMessage(CSCommon.eMessageFrom.Team, CSCommon.eMessageType.Team_Invite, role.RoleData.Name, other.RoleData.RoleId);
                    msg.ShowInfo = string.Format("{0}邀请你加入队伍", role.RoleData.Name);
                    SendMessageToOther(other, msg);
                    pkg.Write((sbyte)CSCommon.eRet_Team.Succeed);
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_KickOutTeam(ulong roleId, ulong otherId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var other = GetRole(otherId);
            if (other == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.NoOther);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.TeamHeaderId != roleId)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.RoleIsNotHeader);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.TeamHeaderId != other.TeamHeaderId)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.DiffTeam);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var team = GetTeam(roleId);
            if (team == null)
            {
                role.SetTeamHeaderId(0);
                pkg.Write((sbyte)CSCommon.eRet_Team.TeamIsNull);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            _LeaveTeam(other, roleId);
            var msg = CreateMessage(CSCommon.eMessageFrom.Team, CSCommon.eMessageType.Team_KickOut, role.RoleData.Name, other.RoleData.RoleId);
            msg.ShowInfo = string.Format("你被踢出队伍");
            SendMessageToOther(other, msg);
            pkg.Write((sbyte)CSCommon.eRet_Team.Succeed);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_LeaveTeam(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.TeamHeaderId == 0)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.RoleNoTeam);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            _LeaveTeam(role, role.TeamHeaderId);
            pkg.Write((sbyte)CSCommon.eRet_Team.Succeed);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_OperateTeamAsk(ulong roleId, string otherName, byte operate, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.TeamHeaderId != roleId)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.RoleIsNotHeader);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var other = GetRole(otherName);
            if (other == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.NoOther);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (operate == (byte)CSCommon.eOperateAsk.Refuse)
            {
                var msg = CreateMessage(CSCommon.eMessageFrom.Team, CSCommon.eMessageType.Team_AskRefuse, role.RoleData.Name, other.RoleData.RoleId);
                msg.ShowInfo = string.Format("{0}拒绝了你的申请", role.RoleData.Name);
                SendMessageToOther(other, msg);
            }
            else if (operate == (byte)CSCommon.eOperateAsk.Accept)
            {
                if (other.TeamHeaderId != 0)
                {
                    pkg.Write((sbyte)CSCommon.eRet_Team.OtherHasTeam);
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }

                if (_AddTeam(other, roleId) == false)
                {
                    pkg.Write((sbyte)CSCommon.eRet_Team.OverTeamCount);
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                var msg = CreateMessage(CSCommon.eMessageFrom.Team, CSCommon.eMessageType.Team_AskAgree, role.RoleData.Name, other.RoleData.RoleId);
                msg.ShowInfo = string.Format("{0}同意了你的申请", role.RoleData.Name);
                SendMessageToOther(other, msg);
            }
            pkg.Write((sbyte)CSCommon.eRet_Team.Succeed);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_OperateTeamInvite(ulong roleId, string otherName, byte operate, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var other = GetRole(otherName);
            if (other == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.NoOther);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (operate == (byte)CSCommon.eOperateAsk.Accept)
            {
                if (other.TeamHeaderId == 0)
                {
                    pkg.Write((sbyte)CSCommon.eRet_Team.OtherNoTeam);
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                if (other.RoleData.RoleId != other.TeamHeaderId)
                {
                    pkg.Write((sbyte)CSCommon.eRet_Team.OtherIsNotHeader);
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                var team = GetTeam(other.TeamHeaderId);
                if (team == null)
                {
                    pkg.Write((sbyte)CSCommon.eRet_Team.TeamIsNull);
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                if (_TeamOverMaxCount(team))
                {
                    pkg.Write((sbyte)CSCommon.eRet_Team.OverTeamCount);
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                if (_AddTeam(role, other.TeamHeaderId) == false)
                {
                    pkg.Write((sbyte)CSCommon.eRet_Team.OverTeamCount);
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                var msg = CreateMessage(CSCommon.eMessageFrom.Team, CSCommon.eMessageType.Team_InviteAgree, role.RoleData.Name, other.RoleData.RoleId);
                msg.ShowInfo = string.Format("{0}同意了你的邀请", role.RoleData.Name);
                SendMessageToOther(other, msg);
            }
            else if (operate == (byte)CSCommon.eOperateAsk.Refuse)
            {
                var msg = CreateMessage(CSCommon.eMessageFrom.Team, CSCommon.eMessageType.Team_InviteRefuse, role.RoleData.Name, other.RoleData.RoleId);
                msg.ShowInfo = string.Format("{0}拒绝了你的邀请", role.RoleData.Name);
                SendMessageToOther(other, msg);
            }
            pkg.Write((sbyte)CSCommon.eRet_Team.Succeed);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_GetTeamPlayers(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.TeamHeaderId == 0)
            {
                pkg.Write((sbyte)CSCommon.eRet_Team.RoleNoTeam);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var team = GetTeam(role.TeamHeaderId);
            if (team == null)
            {
                role.SetTeamHeaderId(0);
                pkg.Write((sbyte)CSCommon.eRet_Team.RoleNoTeam);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            pkg.Write((sbyte)CSCommon.eRet_Team.Succeed);
            List<RoleCom> infos = team.GetPlayerInfos();
            byte count = (byte)infos.Count;
            pkg.Write(count);
            foreach (var i in infos)
            {
                pkg.Write(i);
            }
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        public void SendTeamInfoToPlayers(Team team)
        {
            RPC.DataWriter dw = new RPC.DataWriter();
            List<RoleCom> infos = team.GetPlayerInfos();
            RPC.IAutoSaveAndLoad.DaraWriteList<RoleCom>(infos, dw, false);
            foreach (var i in team.GetPlayers())
            {
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_PlanesServer(pkg).RPC_SendPlayerTeamInfo(pkg, i.RoleData.RoleId, dw);
                pkg.DoCommand(i.PlanesConnect, RPC.CommandTargetType.DefaultType);
            }

        }

        public void SendToLeaveTeamPlayer(UserRole role)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_PlanesServer(pkg).RPC_SendToLeaveTeamPlayer(pkg, role.RoleData.RoleId);
            pkg.DoCommand(role.PlanesConnect, RPC.CommandTargetType.DefaultType);
        }
    }
}
