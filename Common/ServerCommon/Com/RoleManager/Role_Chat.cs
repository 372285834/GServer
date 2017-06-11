using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Com
{
    public partial class UserRoleManager : RPC.RPCObject
    {
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_SayToRole(ulong roleId, string tarName, string msg, RPC.DataReader hyperlink, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {

            sbyte success = 0;
            RPC.DataWriter data = new RPC.DataWriter();
            if (hyperlink != null)
            {
                hyperlink.Read(out success);
                data.Write(success);
                if (success == (sbyte)1)//物品
                {
                    CSCommon.Data.ItemData item = new CSCommon.Data.ItemData();
                    hyperlink.Read(item, true);
                    data.Write(item, true);
                }
            }
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var tarRole = GetRole(tarName);
            if (tarRole == null)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            if (tarRole.PlanesConnect == null)
            {
                pkg.Write((sbyte)-2);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            var role = GetRole(roleId);
            if (role == null)
            {
                Log.Log.Common.Print("RPC_SayToRole role is null,{0}", roleId);
                pkg.Write((sbyte)-3);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            if (tarRole.RoleData.RoleId == roleId)
            {
                pkg.Write((sbyte)-4);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_PlanesServer(retPkg).RPC_DSTalkMsg(retPkg, tarRole.RoleData.PlanesId, role.RoleData.Name,
                (sbyte)CSCommon.eSayChannel.WhisperChannel, tarRole.RoleData.RoleId, msg, data);
            retPkg.DoCommand(tarRole.PlanesConnect, RPC.CommandTargetType.DefaultType);

            pkg.Write((sbyte)1);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }
  
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_Say(ulong roleId, sbyte channel, string msg, RPC.DataReader hyperlink, RPC.RPCForwardInfo fwd)
        {
            RPC.DataWriter data = new RPC.DataWriter();
            var link = hyperlink.ReadDataReader();
            data.Write(link.mHandle);
            var role = GetRole(roleId);
            if (role == null)
                return;

            switch ((CSCommon.eSayChannel)channel)
            {
                case CSCommon.eSayChannel.WorldChannel:
                case CSCommon.eSayChannel.SystemChannel:
                    SayToWorld(role, channel, msg, data);
                    break;
                case CSCommon.eSayChannel.CampChannel:
                    if (!SayToCamp(role, channel, msg, data))
                    {
                        SayError(role, channel, "未加入国家", new RPC.DataWriter());
                    }
                    break;
                case CSCommon.eSayChannel.GuildChannel:
                    if (!SayToGuild(role, channel, msg, data))
                    {
                        SayError(role, channel, "未加入帮派", new RPC.DataWriter());
                    }
                    break;
                case CSCommon.eSayChannel.TeamChannel:
                    if (!SayToTeam(role, channel, msg, data))
                    {
                        SayError(role, channel, "未加入队伍", new RPC.DataWriter());
                    }
                    break;
            }
        }

        public void SayError(UserRole role, sbyte channel, string msg, RPC.DataWriter data)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_PlanesServer(retPkg).RPC_DSTalkMsg(retPkg, role.RoleData.PlanesId, "", channel, role.RoleData.RoleId, msg, data);
            retPkg.DoCommand(role.PlanesConnect, RPC.CommandTargetType.DefaultType);
        }

        public void SayToWorld(UserRole role, sbyte channel, string msg, RPC.DataWriter data)
        {
            foreach (var i in Roles)
            {
                if (i.Value.PlanesConnect != null)
                {
                    RPC.PackageWriter retPkg = new RPC.PackageWriter();
                    H_RPCRoot.smInstance.HGet_PlanesServer(retPkg).RPC_DSTalkMsg(retPkg, i.Value.RoleData.PlanesId, role.RoleData.Name, channel, i.Value.RoleData.RoleId, msg, data);
                    retPkg.DoCommand(i.Value.PlanesConnect, RPC.CommandTargetType.DefaultType);
                }
            }
        }

        public bool SayToCamp(UserRole role, sbyte channel, string msg, RPC.DataWriter data)
        {
            if (role.RoleData.Camp == (byte)CSCommon.eCamp.None)
            {
                return false;
            }
            foreach (var i in Roles)
            {
                if (i.Value.PlanesConnect != null && i.Value.RoleData.Camp == role.RoleData.Camp)
                {
                    RPC.PackageWriter retPkg = new RPC.PackageWriter();
                    H_RPCRoot.smInstance.HGet_PlanesServer(retPkg).RPC_DSTalkMsg(retPkg, i.Value.RoleData.PlanesId, role.RoleData.Name, channel, i.Value.RoleData.RoleId, msg, data);
                    retPkg.DoCommand(i.Value.PlanesConnect, RPC.CommandTargetType.DefaultType);
                }
            }
            return true;
        }

        public bool SayToGuild(UserRole role, sbyte channel, string msg, RPC.DataWriter data)
        {
            if (role.GuildInstance == null)
                return false;
            var members = role.GuildInstance.Members;
            foreach (var i in members)
            {
                if (i.Value.PlanesConnect == null)
                    continue;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_PlanesServer(retPkg).RPC_DSTalkMsg(retPkg, i.Value.RoleData.PlanesId, role.RoleData.Name, channel, i.Value.RoleData.RoleId, msg, data);
                retPkg.DoCommand(i.Value.PlanesConnect, RPC.CommandTargetType.DefaultType);
            }
            return true;
        }

        public bool SayToTeam(UserRole role, sbyte channel, string msg, RPC.DataWriter data)
        {
            var team = GetTeam(role.TeamHeaderId);
            if (team == null)
            {
                return false;
            }
            foreach (var id in team.TeamMembers)
            {
                var teamRole = GetRole(id);
                if (teamRole == null)
                {
                    continue;
                }
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_PlanesServer(pkg).RPC_DSTalkMsg(pkg, teamRole.RoleData.PlanesId, role.RoleData.Name, channel, teamRole.RoleData.RoleId, msg, data);
                pkg.DoCommand(teamRole.PlanesConnect, RPC.CommandTargetType.DefaultType);
            }
            return true;
        }

    }
}
