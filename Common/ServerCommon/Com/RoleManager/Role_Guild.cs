using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Com
{
    public partial class UserRoleManager : RPC.RPCObject
    {   
        #region 帮会操作

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_GetGuilds(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            retPkg.SetSinglePkg();
            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_GetGuilds.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.RoleData.GuildId == 0)//没有帮会，返回帮会列表信息
            {
                retPkg.Write((sbyte)CSCommon.eRet_GetGuilds.ReturnGuilds);
                List<CSCommon.Data.GuildCom> guilds = new List<CSCommon.Data.GuildCom>();
                foreach (var guild in GuildManager.Instance.Guilds)
                {
                    if (guild.Value.GuildData.PlanesId == role.RoleData.PlanesId && guild.Value.GuildData.Camp == role.RoleData.Camp)
                    {
                        guilds.Add(guild.Value.GuildData);
                    }
                }

                int count = guilds.Count;
                retPkg.Write(count);
                foreach (var i in guilds)
                {
                    retPkg.Write(i);
                }

            }
            else //有帮会,返回帮会成员信息
            {
                if (role.GuildInstance == null)
                {
                    Log.Log.Guild.Print("role.GuildInstance is null {0}", role.RoleData.GuildId);
                    retPkg.Write((sbyte)CSCommon.eRet_GetGuilds.NoGuild);
                    retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                else
                {
                    retPkg.Write((sbyte)CSCommon.eRet_GetGuilds.ReturnGuildMembers);
                    retPkg.Write(role.GuildInstance.GuildData);
                    retPkg.Write(role.GuildInstance.Members.Count);
                    foreach (var member in role.GuildInstance.Members)
                    {
                        retPkg.Write(member.Value.RoleData);
                    }
                }
            }
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_CreateGuild(ulong roleId, string GuildName, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_CreateGuild.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance != null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_CreateGuild.RoleHasGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            GuildInstance GuildInstance = new GuildInstance();

            CSCommon.Data.GuildCom guild = new CSCommon.Data.GuildCom();
            guild.GuildName = GuildName;
            guild.PlanesId = role.RoleData.PlanesId;
            guild.PresidentName = role.RoleData.Name;
            guild.MemberNum = 1;
            if (DB_CreatGuildCom(guild) == false)
            {
                retPkg.Write((sbyte)CSCommon.eRet_CreateGuild.SameGuildName);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            role.RoleData.GuildId = guild.GuildId;
            role.RoleData.GuildPost = (byte)CSCommon.eGuildPost.BangZhu;
            role.GuildInstance = GuildInstance;

            GuildInstance.GuildData = guild;
            GuildInstance.Members.Add(roleId, role);
            GuildManager.Instance.AddGuildInstance(GuildInstance);

            //存盘，这里后面要改成异步执行
            DB_SaveRoleData(role);

            retPkg.Write((sbyte)CSCommon.eRet_CreateGuild.Succeed);
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_LeaveGuild(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_LeaveGuild.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.RoleData.GuildId == 0 || role.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_LeaveGuild.RoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.RoleData.GuildPost == (byte)CSCommon.eGuildPost.BangZhu)
            {
                retPkg.Write((sbyte)CSCommon.eRet_LeaveGuild.RoleIsBangZhu);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            retPkg.Write((sbyte)CSCommon.eRet_LeaveGuild.Succeed);
            LeaveGuild(role);
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        public void LeaveGuild(UserRole role)
        {
            GuildInstanceChange(role);
            RoleLeave(role);
            if (role.PlanesConnect == null)
            {
                DB_SaveRoleData(role);
            }
        }
        public void GuildInstanceChange(UserRole role)
        {
            if (role.GuildInstance.Members.ContainsKey(role.RoleData.RoleId))
            {
                role.GuildInstance.Members.Remove(role.RoleData.RoleId);
            }
            role.GuildInstance.GuildData.MemberNum = role.GuildInstance.Members.Count;
        }
        public void RoleLeave(UserRole role)
        {
            role.GuildInstance = null;
            role.RoleData.GuildId = 0;
            role.RoleData.GuildPost = (byte)CSCommon.eGuildPost.None;
        }

        public void DissolveGuildMember(ulong guildId, ulong roleId)
        {
            var role = GetRole(roleId);
            if (role == null)
                return;
            if (role.GuildInstance.GuildData.GuildId != guildId)
                return;
            RoleLeave(role);
            CreateMailAndSend(roleId, CSCommon.eMailFromType.SystemDissolveGuild);
        }



        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_AskToGuild(ulong roleId, ulong guildId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_AskToGuild.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var guild = GuildManager.Instance.GetGuild(guildId);
            if (guild == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_AskToGuild.NoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance != null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_AskToGuild.RoleHasGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            int count = guild.GuildData.MemberNum;
            if (CSCommon.GuildCommon.Instance.GuildLvUpList.Count < guild.GuildData.Level)
            {
                retPkg.Write((sbyte)CSCommon.eRet_AskToGuild.OverMaxNum);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (count >= (int)CSCommon.GuildCommon.Instance.GuildLvUpList[guild.GuildData.Level - 1].MaxMemberNum)
            {
                retPkg.Write((sbyte)CSCommon.eRet_AskToGuild.OverMaxNum);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            //生成消息
            CSCommon.Data.Message msg = CreateMessage(CSCommon.eMessageFrom.Guild, CSCommon.eMessageType.Guild_Ask, role.RoleData.Name, guildId);
            msg.ShowInfo = string.Format("玩家{0}申请加入帮会", role.RoleData.Name);
            //发送消息
            bool isSend = false;
            foreach (var i in guild.Members)
            {
                var member = GetRole(i.Key);
                if (member.RoleData.GuildPost > (byte)CSCommon.eGuildPost.JingYing)
                {
                    if (member.PlanesConnect != null)
                    {
                        isSend = true;
                        RPC.PackageWriter pkg = new RPC.PackageWriter();
                        H_RPCRoot.smInstance.HGet_PlanesServer(pkg).RPC_SendPlayerMsg(pkg, i.Key, msg);
                        pkg.DoCommand(member.PlanesConnect, RPC.CommandTargetType.DefaultType);
                    }
                }
            }
            if (isSend)
            {
                guild.Messages.Add(msg.MessageId, msg);
            }
            //申请成功
            retPkg.Write((sbyte)CSCommon.eRet_AskToGuild.Succeed);
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        public CSCommon.Data.Message CreateMessage(CSCommon.eMessageFrom from, CSCommon.eMessageType type, string sender, ulong targetId)
        {
            CSCommon.Data.Message msg = new CSCommon.Data.Message();
            msg.MessageId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Message);
            msg.MessageType = (byte)type;
            msg.MessageFrom = (byte)from;
            msg.OwnerId = targetId;
            msg.Sender = sender;
            return msg;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_OperateGuildAsk(ulong roleId, ulong messageId, byte operate, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_OperateGuildAsk.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_OperateGuildAsk.RoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.RoleData.GuildPost <= (byte)CSCommon.eGuildPost.JingYing)
            {
                retPkg.Write((sbyte)CSCommon.eRet_OperateGuildAsk.RoleNoPower);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            CSCommon.Data.Message msg = null;
            role.GuildInstance.Messages.TryGetValue(messageId, out msg);
            if (msg == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_OperateGuildAsk.YetOperate);//已审批
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var targetRole = GetRole(msg.Sender);
            if (targetRole == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_OperateGuildAsk.NoTargetRole);
                role.GuildInstance.Messages.Remove(messageId);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (targetRole.GuildInstance != null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_OperateGuildAsk.TargetRoleHasGuild);//已加入其他帮会
                role.GuildInstance.Messages.Remove(messageId);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance.GuildData.MemberNum >= CSCommon.GuildCommon.Instance.GuildLvUpList[role.GuildInstance.GuildData.Level - 1].MaxMemberNum)
            {
                retPkg.Write((sbyte)CSCommon.eRet_OperateGuildAsk.OverMaxNum);
                role.GuildInstance.Messages.Remove(messageId);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            retPkg.Write((sbyte)CSCommon.eRet_OperateGuildAsk.Succeed);
            role.GuildInstance.Messages.Remove(messageId);

            CSCommon.Data.Message operatemsg = null;
            if (operate == (byte)CSCommon.eOperateAsk.Accept)
            {
                role.GuildInstance.Members.Add(targetRole.RoleData.RoleId, targetRole);
                role.GuildInstance.GuildData.MemberNum = role.GuildInstance.Members.Count;
                targetRole.RoleData.GuildId = role.GuildInstance.GuildData.GuildId;
                targetRole.RoleData.GuildPost = (byte)CSCommon.eGuildPost.BangZhong;
                targetRole.GuildInstance = role.GuildInstance;
                operatemsg = CreateMessage(CSCommon.eMessageFrom.Guild, CSCommon.eMessageType.Guild_AcceptAsk, role.RoleData.Name, targetRole.RoleData.RoleId);
                msg.ShowInfo = string.Format("申请被接受");
            }
            else
            {
                operatemsg = CreateMessage(CSCommon.eMessageFrom.Guild, CSCommon.eMessageType.Guild_RefuseAsk, role.RoleData.Name, targetRole.RoleData.RoleId);
                msg.ShowInfo = string.Format("申请被拒绝");
            }
            if (targetRole.PlanesConnect != null)
            {
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_PlanesServer(pkg).RPC_SendPlayerMsg(pkg, targetRole.RoleData.RoleId, operatemsg);
                pkg.DoCommand(targetRole.PlanesConnect, RPC.CommandTargetType.DefaultType);
            }
            else
            {
                DB_CreateMessage(operatemsg);
            }
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_KickedOutGuild(ulong roleId, ulong targetId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();

            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_KickedOutGuild.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var targetRole = GetRole(targetId);
            if (targetRole == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_KickedOutGuild.NoTargetRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_KickedOutGuild.RoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (targetRole.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_KickedOutGuild.TargetRoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.RoleData.GuildPost <= (byte)CSCommon.eGuildPost.JingYing)
            {
                retPkg.Write((sbyte)CSCommon.eRet_KickedOutGuild.RoleNoPower);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.RoleData.GuildPost <= targetRole.RoleData.GuildPost)
            {
                retPkg.Write((sbyte)CSCommon.eRet_KickedOutGuild.RolePostLowTarget);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            LeaveGuild(targetRole);
            //邮件形式
            CreateMailAndSend(targetId, CSCommon.eMailFromType.KickedOutGuild, role.GuildInstance.GuildData.GuildName);
            
            retPkg.Write((sbyte)CSCommon.eRet_KickedOutGuild.Succeed);
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        //人为解散
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_DissolveGuild(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();

            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_DissolveGuild.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_DissolveGuild.RoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.RoleData.GuildPost != (byte)CSCommon.eGuildPost.BangZhu)
            {
                retPkg.Write((sbyte)CSCommon.eRet_DissolveGuild.RoleIsNotBangZhu);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            retPkg.Write((sbyte)CSCommon.eRet_DissolveGuild.Succeed);
            foreach (var i in role.GuildInstance.Members)
            {
                var member = GetRole(i.Key);
                if (member == null)
                {
                    continue;
                }
                if (member.RoleData.GuildPost == (byte)CSCommon.eGuildPost.BangZhu)
                    continue;
                RoleLeave(member);
                CreateMailAndSend(i.Key, CSCommon.eMailFromType.RoleDissolveGuild, role.RoleData.Name);
            }
            role.GuildInstance.Members.Clear();
            GuildManager.Instance.RemoveGuildInstance(role.GuildInstance.GuildData.GuildId);
            DB_DelGuildCom(role.GuildInstance.GuildData);
            RoleLeave(role);
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_SetGuildPost(ulong roleId, ulong targetId, byte post, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_SetGuildPost.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var targetRole = GetRole(targetId);
            if (targetRole == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_SetGuildPost.NoTargetRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_SetGuildPost.RoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (targetRole.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_SetGuildPost.TargetRoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance.GuildData.GuildId != targetRole.GuildInstance.GuildData.GuildId)
            {
                retPkg.Write((sbyte)CSCommon.eRet_SetGuildPost.DifferentGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.RoleData.GuildPost <= (byte)CSCommon.eGuildPost.JingYing)//没有权限
            {
                retPkg.Write((sbyte)CSCommon.eRet_SetGuildPost.RoleNoPower);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.RoleData.GuildPost <= targetRole.RoleData.GuildPost)//级别低于对方
            {
                retPkg.Write((sbyte)CSCommon.eRet_SetGuildPost.RolePostLowTarget);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.RoleData.GuildPost <= post)
            {
                retPkg.Write((sbyte)CSCommon.eRet_SetGuildPost.RolePostLowSet);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (targetRole.RoleData.GuildPost == post)
            {
                retPkg.Write((sbyte)CSCommon.eRet_SetGuildPost.TargetPostEqualSet);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            CSCommon.eMessageType msgType = CSCommon.eMessageType.Guild_SetBangZhong;
            byte maxnum = 0;
            if (post == (byte)CSCommon.eGuildPost.ZhangLao)
            {
                maxnum = CSCommon.GuildCommon.Instance.MaxZhangLao;
                msgType = CSCommon.eMessageType.Guild_SetZhangLao;
            }
            else if (post == (byte)CSCommon.eGuildPost.TangZhu)
            {
                maxnum = CSCommon.GuildCommon.Instance.MaxTangZhu;
                msgType = CSCommon.eMessageType.Guild_SetTangZhu;
            }
            else if (post == (byte)CSCommon.eGuildPost.JingYing)
            {
                maxnum = CSCommon.GuildCommon.Instance.MaxJingYing;
                msgType = CSCommon.eMessageType.Guild_SetJingYing;
            }
            byte num = role.GuildInstance.GetNumByPost(post);
            if (num >= maxnum)
            {
                retPkg.Write((sbyte)CSCommon.eRet_SetGuildPost.OverMaxNum);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            retPkg.Write((sbyte)CSCommon.eRet_SetGuildPost.Succeed);

            targetRole.RoleData.GuildPost = post;
            CSCommon.Data.Message msg = CreateMessage(CSCommon.eMessageFrom.Guild, msgType, role.RoleData.Name, targetId);
            msg.ShowInfo = string.Format("你的帮会职位被改为{0}", GetPostStr(post));
            SendMessageToOther(targetRole, msg);
            targetRole.GuildInstance.UpdateMember(targetRole);
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        public string GetPostStr(byte post)
        {
            string str = "";
            switch ((CSCommon.eGuildPost)post)
            {
                case CSCommon.eGuildPost.None:
                    break;
                case CSCommon.eGuildPost.BangZhong:
                    str = "帮众";
                    break;
                case CSCommon.eGuildPost.JingYing:
                    str = "精英";
                    break;
                case CSCommon.eGuildPost.TangZhu:
                    str = "堂主";
                    break;
                case CSCommon.eGuildPost.ZhangLao:
                    str = "长老";
                    break;
                case CSCommon.eGuildPost.BangZhu:
                    str = "帮主";
                    break;
            }
            return str;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_InviteToGuild(ulong roleId, string tarName, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_InviteToGuild.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var targetRole = GetRole(tarName);
            if (targetRole == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_InviteToGuild.NoTargetRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_InviteToGuild.RoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (targetRole.GuildInstance != null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_InviteToGuild.TargetRoleHasGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            if (role.RoleData.GuildPost <= (byte)CSCommon.eGuildPost.JingYing)
            {
                retPkg.Write((sbyte)CSCommon.eRet_InviteToGuild.RoleNoPower);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (targetRole.PlanesConnect == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_InviteToGuild.TargetRoleNotInPlay);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance.GuildData.MemberNum >= CSCommon.GuildCommon.Instance.GuildLvUpList[role.GuildInstance.GuildData.Level - 1].MaxMemberNum)
            {
                retPkg.Write((sbyte)CSCommon.eRet_InviteToGuild.OverMaxNum);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            retPkg.Write((sbyte)CSCommon.eRet_InviteToGuild.Succeed);
            CSCommon.Data.Message msg = CreateMessage(CSCommon.eMessageFrom.Guild, CSCommon.eMessageType.Guild_Invite, role.RoleData.Name, targetRole.RoleData.RoleId);
            msg.ShowInfo = string.Format("玩家{0}邀请您加入{1}帮会", role.RoleData.Name, role.GuildInstance.GuildData.GuildName);
            SendMessageToOther(targetRole, msg);

            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_OperateInvite(ulong roleId, ulong targetId, byte operate, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_OperateInvite.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var targetRole = GetRole(targetId);
            if (targetRole == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_OperateInvite.NoTargetRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance != null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_OperateInvite.RoleHasGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (targetRole.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_OperateInvite.TargetRoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (operate == (byte)CSCommon.eOperateAsk.Refuse)
            {
                retPkg.Write((sbyte)CSCommon.eRet_OperateInvite.Succeed);
                CSCommon.Data.Message msg = CreateMessage(CSCommon.eMessageFrom.Guild, CSCommon.eMessageType.Guild_RefuseInvite, role.RoleData.Name, targetId);
                msg.ShowInfo = string.Format("玩家{0}拒绝了你的邀请", role.RoleData.Name);
                SendMessageToOther(targetRole, msg);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            else
            {
                if (targetRole.GuildInstance.GuildData.MemberNum >= CSCommon.GuildCommon.Instance.GuildLvUpList[targetRole.GuildInstance.GuildData.Level - 1].MaxMemberNum)
                {
                    retPkg.Write((sbyte)CSCommon.eRet_OperateInvite.OverMaxNum);
                    retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                retPkg.Write((sbyte)CSCommon.eRet_OperateInvite.Succeed);
                role.RoleData.GuildId = targetRole.GuildInstance.GuildData.GuildId;
                role.RoleData.GuildPost = (byte)CSCommon.eGuildPost.BangZhong;
                role.GuildInstance = targetRole.GuildInstance;
                role.GuildInstance.Members.Add(roleId, role);
                role.GuildInstance.GuildData.MemberNum = role.GuildInstance.Members.Count;

                CSCommon.Data.Message msg = CreateMessage(CSCommon.eMessageFrom.Guild, CSCommon.eMessageType.Guild_AcceptInvite, role.RoleData.Name, targetId);
                msg.ShowInfo = string.Format("玩家{0}同意了你的邀请", role.RoleData.Name);
                SendMessageToOther(targetRole, msg);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
            }

        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_TransferGuildBangZhu(ulong roleId, ulong targetId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_TransferGuildBangZhu.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var targetRole = GetRole(targetId);
            if (targetRole == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_TransferGuildBangZhu.NoTargetRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_TransferGuildBangZhu.RoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (targetRole.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_TransferGuildBangZhu.TargetRoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.RoleData.GuildPost != (byte)CSCommon.eGuildPost.BangZhu)
            {
                retPkg.Write((sbyte)CSCommon.eRet_TransferGuildBangZhu.RoleIsNotBangZhu);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance.GuildData.GuildId != targetRole.GuildInstance.GuildData.GuildId)
            {
                retPkg.Write((sbyte)CSCommon.eRet_TransferGuildBangZhu.DifferentGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            role.RoleData.GuildPost = (byte)CSCommon.eGuildPost.BangZhong;
            targetRole.RoleData.GuildPost = (byte)CSCommon.eGuildPost.BangZhu;
            role.GuildInstance.GuildData.PresidentName = targetRole.RoleData.Name;
            DB_SaveGuildCom(role.GuildInstance.GuildData);

            retPkg.Write((sbyte)CSCommon.eRet_TransferGuildBangZhu.Succeed);
            foreach (var i in role.GuildInstance.Members)
            {
                var member = GetRole(i.Key);

                CSCommon.Data.Message msg = CreateMessage(CSCommon.eMessageFrom.Guild, CSCommon.eMessageType.Guild_TransferBangZhu, role.RoleData.Name, i.Key);
                msg.ShowInfo = string.Format("{0}已将帮主职位转让给了{1}，请大家紧密团结在以{2}为新帮主的周围，共同创建美好明天!", role.RoleData.Name, targetRole.RoleData.Name, targetRole.RoleData.Name);
                SendMessageToOther(member, msg);
            }
            role.GuildInstance.UpdateMember(role);
            targetRole.GuildInstance.UpdateMember(targetRole);
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_DonateGold(ulong roleId, int gold, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_DonateGold.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_DonateGold.RoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            role.GuildInstance.GuildData.GuildGold += (ulong)gold;

            int contribute = (int)gold;//贡献换算公式未定
            AddRoleGuildContribute(role, contribute);
            retPkg.Write((sbyte)CSCommon.eRet_DonateGold.Succeed);
            retPkg.Write(role.RoleData.TodayGuildContribute);
            retPkg.Write(role.RoleData.GuildContribute);
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        public void AddRoleGuildContribute(UserRole role, int contribute)
        {
            role.RoleData.TodayGuildContribute = GetRoleTodayGuildContribute(role);
            role.RoleData.GuildContribute += contribute;
            role.RoleData.TodayGuildContribute += contribute;
            role.RoleData.LastContributeTime = System.DateTime.Now;
            role.GuildInstance.UpdateMember(role);
        }

        public int GetRoleTodayGuildContribute(UserRole role)
        {
            int ftime = (int)CSCommon.GuildCommon.Instance.FlushContributeTime;//刷新时间
            System.DateTime lasttime = role.RoleData.LastContributeTime;
            if (ServerFrame.Time.InitTimes(ftime, lasttime))
            {
                role.RoleData.TodayGuildContribute = 0;
                role.GuildInstance.UpdateMember(role);
            }
            return role.RoleData.TodayGuildContribute;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_IssueGold(ulong roleId, int gold, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_IssueGold.NoRole);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.GuildInstance == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_IssueGold.RoleNoGuild);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.RoleData.GuildPost < (byte)CSCommon.eGuildPost.ZhangLao)
            {
                retPkg.Write((sbyte)CSCommon.eRet_IssueGold.RoleNoPower);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            ulong need = (ulong)(role.GuildInstance.GuildData.MemberNum * gold);
            if (need > role.GuildInstance.GuildData.GuildGold)
            {
                retPkg.Write((sbyte)CSCommon.eRet_IssueGold.GuildLessGold);
                retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            role.GuildInstance.GuildData.GuildGold -= need;

            //通过邮件发送给所有帮会成员
            string currencies = GetStr((int)CSCommon.eCurrenceType.Gold, gold);
            foreach (var i in role.GuildInstance.Members)
            {
                CreateMailAndSend(i.Key, CSCommon.eMailFromType.GuildSendGold, "", "",currencies);
            }
            retPkg.Write((sbyte)CSCommon.eRet_IssueGold.Succeed);
            retPkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        #endregion

        #region 帮会相关

        private bool DB_CreatGuildCom(CSCommon.Data.GuildCom Guild)
        {
            // 产生insert语句
            string condition = "GuildId = " + Guild.GuildId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, Guild, true);
            return mDBConnect._ExecuteInsert(dbOp);
        }

        private void DB_SaveGuildCom(CSCommon.Data.GuildCom Guild)
        {
            if (Guild == null)
            {
                return;
            }
            var condition = "GuildId=" + Guild.GuildId;
            var dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, Guild, null);
            mDBConnect._ExecuteUpdate(dbOp);
            return;
        }

        public void DB_DelGuildCom(CSCommon.Data.GuildCom Guild)
        {
            //产生Destroy语句
            string condition = "GuildId =" + Guild.GuildId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.DestroyData(condition, Guild);
            mDBConnect._ExecuteDestroy(dbOp);
            return;
        }

        #endregion

    }
}
