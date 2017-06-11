//Server Caller
namespace ServerCommon{
public class H_IDataServer
{
    public static ServerCommon.H_IDataServer smInstance = new ServerCommon.H_IDataServer();
    public ServerCommon.Data.Player.H_PlayerManager HGet_PlayerManager(RPC.PackageWriter pkg)
    {
        pkg.PushStack(0);
        return ServerCommon.Data.Player.H_PlayerManager.smInstance;
    }
    public void GetStrongPoint(RPC.PackageWriter pkg,System.Int32 aiAreaId,System.Int32 aiMapId)
    {
        pkg.Write(aiAreaId);
        pkg.Write(aiMapId);
        pkg.SetMethod(0);
    }
    public void RegGateServer(RPC.PackageWriter pkg,System.String ip,System.UInt16 port,System.UInt64 id)
    {
        pkg.Write(ip);
        pkg.Write(port);
        pkg.Write(id);
        pkg.SetMethod(1);
    }
    public void RegPlanesServer(RPC.PackageWriter pkg,System.String ip,System.UInt16 port,System.UInt64 id)
    {
        pkg.Write(ip);
        pkg.Write(port);
        pkg.Write(id);
        pkg.SetMethod(2);
    }
    public void TryRegAccount(RPC.PackageWriter pkg,System.UInt16 cltLinker,System.String usr,System.String psw,System.String mobileNum)
    {
        pkg.Write(cltLinker);
        pkg.Write(usr);
        pkg.Write(psw);
        pkg.Write(mobileNum);
        pkg.SetMethod(3);
    }
    public void TryRegGuest(RPC.PackageWriter pkg,System.UInt16 cltLinker)
    {
        pkg.Write(cltLinker);
        pkg.SetMethod(4);
    }
    public void QueryAllActivePlanesInfo(RPC.PackageWriter pkg,System.UInt16 lnk)
    {
        pkg.Write(lnk);
        pkg.SetMethod(5);
    }
    public void RPC_RandRoleName(RPC.PackageWriter pkg,System.Byte sex)
    {
        pkg.Write(sex);
        pkg.SetMethod(6);
    }
    public void TryCreatePlayer(RPC.PackageWriter pkg,System.UInt16 lnk,System.UInt64 accountId,System.String planeName,System.String plyName,System.Byte pro,System.Byte sex)
    {
        pkg.Write(lnk);
        pkg.Write(accountId);
        pkg.Write(planeName);
        pkg.Write(plyName);
        pkg.Write(pro);
        pkg.Write(sex);
        pkg.SetMethod(7);
    }
    public void DeleteRole(RPC.PackageWriter pkg,System.UInt16 planesId,System.String roleName)
    {
        pkg.Write(planesId);
        pkg.Write(roleName);
        pkg.SetMethod(8);
    }
    public void UpdateItem(RPC.PackageWriter pkg,System.UInt64 roleId,CSCommon.Data.ItemData item)
    {
        pkg.Write(roleId);
        pkg.Write(item);
        pkg.SetMethod(9);
    }
    public void DelItem(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 itemId,System.SByte bDestroy)
    {
        pkg.Write(roleId);
        pkg.Write(itemId);
        pkg.Write(bDestroy);
        pkg.SetMethod(10);
    }
    public void DelMail(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 mailId)
    {
        pkg.Write(roleId);
        pkg.Write(mailId);
        pkg.SetMethod(11);
    }
    public void GetAccountInfoData(RPC.PackageWriter pkg,System.UInt64 accountID)
    {
        pkg.Write(accountID);
        pkg.SetMethod(12);
    }
    public void GetRoleDetailByName(RPC.PackageWriter pkg,System.UInt16 planesId,System.String roleName)
    {
        pkg.Write(planesId);
        pkg.Write(roleName);
        pkg.SetMethod(13);
    }
    public void GetRoleDetailData(RPC.PackageWriter pkg,System.UInt64 accountID)
    {
        pkg.Write(accountID);
        pkg.SetMethod(14);
    }
    public void GetPlayerGuidByName(RPC.PackageWriter pkg,System.UInt16 planesId,System.String name)
    {
        pkg.Write(planesId);
        pkg.Write(name);
        pkg.SetMethod(15);
    }
    public void GotoMap(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt16 planesId,System.UInt16 mapSourceId,SlimDX.Vector3 pos,System.UInt16 cltHandle)
    {
        pkg.Write(roleId);
        pkg.Write(planesId);
        pkg.Write(mapSourceId);
        pkg.Write(pos);
        pkg.Write(cltHandle);
        pkg.SetMethod(16);
    }
    public void GetPlanesInfo(RPC.PackageWriter pkg,System.UInt16 planesId)
    {
        pkg.Write(planesId);
        pkg.SetMethod(17);
    }
    public void UpdatePlanesServerPlanesNumber(RPC.PackageWriter pkg,System.Int32 num)
    {
        pkg.Write(num);
        pkg.SetMethod(18);
    }
    public void UpdatePlanesServerGlobalMapNumber(RPC.PackageWriter pkg,System.Int32 num)
    {
        pkg.Write(num);
        pkg.SetMethod(19);
    }
    public void UpdatePlanesServerPlayerNumber(RPC.PackageWriter pkg,System.Int32 num)
    {
        pkg.Write(num);
        pkg.SetMethod(20);
    }
    public void SYS_ReloadItemTemplate(RPC.PackageWriter pkg,System.UInt16 item)
    {
        pkg.Write(item);
        pkg.SetMethod(21);
    }
    public void SYS_ReloadTaskTemplate(RPC.PackageWriter pkg,System.UInt16 item)
    {
        pkg.Write(item);
        pkg.SetMethod(22);
    }
    public void SYS_ReloadDropTemplate(RPC.PackageWriter pkg,System.UInt16 item)
    {
        pkg.Write(item);
        pkg.SetMethod(23);
    }
    public void SYS_ReloadSellerTemplate(RPC.PackageWriter pkg,System.UInt16 item)
    {
        pkg.Write(item);
        pkg.SetMethod(24);
    }
}
}
namespace ServerCommon.Com{
public class H_UserRoleManager
{
    public static ServerCommon.Com.H_UserRoleManager smInstance = new ServerCommon.Com.H_UserRoleManager();
    public void RPC_GetSocialList(RPC.PackageWriter pkg,System.UInt64 roleId,System.Byte type)
    {
        pkg.Write(roleId);
        pkg.Write(type);
        pkg.SetMethod(0);
    }
    public void RPC_AddSocial(RPC.PackageWriter pkg,System.UInt64 roleId,System.String otherName,System.Byte type)
    {
        pkg.Write(roleId);
        pkg.Write(otherName);
        pkg.Write(type);
        pkg.SetMethod(1);
    }
    public void RPC_OperateAddSocial(RPC.PackageWriter pkg,System.UInt64 roleId,System.String otherName,System.Byte type,System.Byte operate)
    {
        pkg.Write(roleId);
        pkg.Write(otherName);
        pkg.Write(type);
        pkg.Write(operate);
        pkg.SetMethod(2);
    }
    public void RPC_RemoveSocial(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 otherId,System.Byte type)
    {
        pkg.Write(roleId);
        pkg.Write(otherId);
        pkg.Write(type);
        pkg.SetMethod(3);
    }
    public void RPC_SendGift(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 otherId,System.Int32 index,System.Int32 count,System.Int32 addvalue)
    {
        pkg.Write(roleId);
        pkg.Write(otherId);
        pkg.Write(index);
        pkg.Write(count);
        pkg.Write(addvalue);
        pkg.SetMethod(4);
    }
    public void RPC_ConsignItem(RPC.PackageWriter pkg,System.UInt64 roleId,System.Int32 templateId,System.Int32 stack,System.Int32 price)
    {
        pkg.Write(roleId);
        pkg.Write(templateId);
        pkg.Write(stack);
        pkg.Write(price);
        pkg.SetMethod(5);
    }
    public void RPC_BuyConsignItem(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 itemId)
    {
        pkg.Write(roleId);
        pkg.Write(itemId);
        pkg.SetMethod(6);
    }
    public void RPC_GetConsignItem(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 itemId)
    {
        pkg.Write(roleId);
        pkg.Write(itemId);
        pkg.SetMethod(7);
    }
    public void RPC_GetRoleGird(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(8);
    }
    public void RPC_GetRoleGirdByName(RPC.PackageWriter pkg,System.UInt64 roleId,System.String name,System.Byte findType,System.Byte page)
    {
        pkg.Write(roleId);
        pkg.Write(name);
        pkg.Write(findType);
        pkg.Write(page);
        pkg.SetMethod(9);
    }
    public void RPC_GetRoleGirdByType(RPC.PackageWriter pkg,System.UInt64 roleId,System.Byte itemType,System.Byte page)
    {
        pkg.Write(roleId);
        pkg.Write(itemType);
        pkg.Write(page);
        pkg.SetMethod(10);
    }
    public void RPC_RoleEnterPlanes(RPC.PackageWriter pkg,CSCommon.Data.RoleDetail roledata)
    {
        pkg.Write(roledata);
        pkg.SetMethod(11);
    }
    public void RPC_RoleLogout(RPC.PackageWriter pkg,System.UInt64 roleId,System.Byte[] offvalue)
    {
        pkg.Write(roleId);
        pkg.Write(offvalue);
        pkg.SetMethod(12);
    }
    public void RPC_UpdateRoleComValue(RPC.PackageWriter pkg,System.UInt64 roleId,System.String name,RPC.DataWriter value)
    {
        pkg.Write(roleId);
        pkg.Write(name);
        pkg.Write(value);
        pkg.SetMethod(13);
    }
    public void RPC_SearchPlayerByName(RPC.PackageWriter pkg,System.String roleName)
    {
        pkg.Write(roleName);
        pkg.SetMethod(14);
    }
    public void RPC_GetTopAndFriend(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt16 planesId)
    {
        pkg.Write(roleId);
        pkg.Write(planesId);
        pkg.SetMethod(15);
    }
    public void RPC_Visit(RPC.PackageWriter pkg,System.Byte type,System.UInt64 roleId,System.UInt64 otherId)
    {
        pkg.Write(type);
        pkg.Write(roleId);
        pkg.Write(otherId);
        pkg.SetMethod(16);
    }
    public void RPC_GetOffPlayerData(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(17);
    }
    public void RPC_UpdateRankDataValue(RPC.PackageWriter pkg,System.UInt64 roleId,System.String name,RPC.DataWriter value)
    {
        pkg.Write(roleId);
        pkg.Write(name);
        pkg.Write(value);
        pkg.SetMethod(18);
    }
    public void RPC_GetMyRank(RPC.PackageWriter pkg,System.UInt64 id,System.Byte type)
    {
        pkg.Write(id);
        pkg.Write(type);
        pkg.SetMethod(19);
    }
    public void RPC_CreateTeam(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(20);
    }
    public void RPC_InviteToTeam(RPC.PackageWriter pkg,System.UInt64 roleId,System.String otherName)
    {
        pkg.Write(roleId);
        pkg.Write(otherName);
        pkg.SetMethod(21);
    }
    public void RPC_KickOutTeam(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 otherId)
    {
        pkg.Write(roleId);
        pkg.Write(otherId);
        pkg.SetMethod(22);
    }
    public void RPC_LeaveTeam(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(23);
    }
    public void RPC_OperateTeamAsk(RPC.PackageWriter pkg,System.UInt64 roleId,System.String otherName,System.Byte operate)
    {
        pkg.Write(roleId);
        pkg.Write(otherName);
        pkg.Write(operate);
        pkg.SetMethod(24);
    }
    public void RPC_OperateTeamInvite(RPC.PackageWriter pkg,System.UInt64 roleId,System.String otherName,System.Byte operate)
    {
        pkg.Write(roleId);
        pkg.Write(otherName);
        pkg.Write(operate);
        pkg.SetMethod(25);
    }
    public void RPC_GetTeamPlayers(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(26);
    }
    public void RPC_GetMails(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(27);
    }
    public void RPC_DelMail(RPC.PackageWriter pkg,System.UInt64 mailId)
    {
        pkg.Write(mailId);
        pkg.SetMethod(28);
    }
    public void RPC_OpenMail(RPC.PackageWriter pkg,System.UInt64 mailId)
    {
        pkg.Write(mailId);
        pkg.SetMethod(29);
    }
    public void RPC_GetMailItems(RPC.PackageWriter pkg,System.UInt64 mailId)
    {
        pkg.Write(mailId);
        pkg.SetMethod(30);
    }
    public void RPC_OneKeyDelMails(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(31);
    }
    public void RPC_OneKeyGetItems(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(32);
    }
    public void RPC_GetGuilds(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(33);
    }
    public void RPC_CreateGuild(RPC.PackageWriter pkg,System.UInt64 roleId,System.String GuildName)
    {
        pkg.Write(roleId);
        pkg.Write(GuildName);
        pkg.SetMethod(34);
    }
    public void RPC_LeaveGuild(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(35);
    }
    public void RPC_AskToGuild(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 guildId)
    {
        pkg.Write(roleId);
        pkg.Write(guildId);
        pkg.SetMethod(36);
    }
    public void RPC_OperateGuildAsk(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 messageId,System.Byte operate)
    {
        pkg.Write(roleId);
        pkg.Write(messageId);
        pkg.Write(operate);
        pkg.SetMethod(37);
    }
    public void RPC_KickedOutGuild(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 targetId)
    {
        pkg.Write(roleId);
        pkg.Write(targetId);
        pkg.SetMethod(38);
    }
    public void RPC_DissolveGuild(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(39);
    }
    public void RPC_SetGuildPost(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 targetId,System.Byte post)
    {
        pkg.Write(roleId);
        pkg.Write(targetId);
        pkg.Write(post);
        pkg.SetMethod(40);
    }
    public void RPC_InviteToGuild(RPC.PackageWriter pkg,System.UInt64 roleId,System.String tarName)
    {
        pkg.Write(roleId);
        pkg.Write(tarName);
        pkg.SetMethod(41);
    }
    public void RPC_OperateInvite(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 targetId,System.Byte operate)
    {
        pkg.Write(roleId);
        pkg.Write(targetId);
        pkg.Write(operate);
        pkg.SetMethod(42);
    }
    public void RPC_TransferGuildBangZhu(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt64 targetId)
    {
        pkg.Write(roleId);
        pkg.Write(targetId);
        pkg.SetMethod(43);
    }
    public void RPC_DonateGold(RPC.PackageWriter pkg,System.UInt64 roleId,System.Int32 gold)
    {
        pkg.Write(roleId);
        pkg.Write(gold);
        pkg.SetMethod(44);
    }
    public void RPC_IssueGold(RPC.PackageWriter pkg,System.UInt64 roleId,System.Int32 gold)
    {
        pkg.Write(roleId);
        pkg.Write(gold);
        pkg.SetMethod(45);
    }
    public void RPC_SayToRole(RPC.PackageWriter pkg,System.UInt64 roleId,System.String tarName,System.String msg,RPC.DataWriter hyperlink)
    {
        pkg.Write(roleId);
        pkg.Write(tarName);
        pkg.Write(msg);
        pkg.Write(hyperlink);
        pkg.SetMethod(46);
    }
    public void RPC_Say(RPC.PackageWriter pkg,System.UInt64 roleId,System.SByte channel,System.String msg,RPC.DataWriter hyperlink)
    {
        pkg.Write(roleId);
        pkg.Write(channel);
        pkg.Write(msg);
        pkg.Write(hyperlink);
        pkg.SetMethod(47);
    }
}
}
namespace ServerCommon.Com{
public class H_WorldManager
{
    public static ServerCommon.Com.H_WorldManager smInstance = new ServerCommon.Com.H_WorldManager();
}
}
namespace ServerCommon{
public class H_IPlanesServer
{
    public static ServerCommon.H_IPlanesServer smInstance = new ServerCommon.H_IPlanesServer();
    public ServerCommon.Planes.H_GameMaster HGet_GameMaster(RPC.PackageWriter pkg)
    {
        pkg.PushStack(0);
        return ServerCommon.Planes.H_GameMaster.smInstance;
    }
    public void GetCitySoldier(RPC.PackageWriter pkg,RPC.DataWriter dr)
    {
        pkg.Write(dr);
        pkg.SetMethod(0);
    }
    public void RegGateServer(RPC.PackageWriter pkg,System.String ip,System.UInt16 port,System.UInt64 id)
    {
        pkg.Write(ip);
        pkg.Write(port);
        pkg.Write(id);
        pkg.SetMethod(1);
    }
    public void GetPlanesServerId(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(2);
    }
    public void ClientDisConnect(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(3);
    }
    public void RemoveInstanceMap(RPC.PackageWriter pkg,System.UInt64 mapInstanceId)
    {
        pkg.Write(mapInstanceId);
        pkg.SetMethod(4);
    }
    public void ReturnToRoleSelect(RPC.PackageWriter pkg,System.UInt64 planesId,System.UInt64 roleId,System.UInt64 accountId)
    {
        pkg.Write(planesId);
        pkg.Write(roleId);
        pkg.Write(accountId);
        pkg.SetMethod(5);
    }
    public void SYS_ReloadItemTemplate(RPC.PackageWriter pkg,System.UInt16 item)
    {
        pkg.Write(item);
        pkg.SetMethod(6);
    }
    public void SYS_ReloadRoleTemplate(RPC.PackageWriter pkg,System.UInt16 item)
    {
        pkg.Write(item);
        pkg.SetMethod(7);
    }
    public void SYS_ReloadTaskTemplate(RPC.PackageWriter pkg,System.UInt16 item)
    {
        pkg.Write(item);
        pkg.SetMethod(8);
    }
    public void SYS_ReloadDropTemplate(RPC.PackageWriter pkg,System.Int32 item)
    {
        pkg.Write(item);
        pkg.SetMethod(9);
    }
    public void SYS_ReloadSellerTemplate(RPC.PackageWriter pkg,System.UInt16 item)
    {
        pkg.Write(item);
        pkg.SetMethod(10);
    }
    public void EnterMap(RPC.PackageWriter pkg,CSCommon.Data.PlayerData pd,CSCommon.Data.PlanesData planesData,System.UInt16 mapSourceId,System.UInt64 instanceId,SlimDX.Vector3 pos,System.UInt16 cltHandle)
    {
        pkg.Write(pd);
        pkg.Write(planesData);
        pkg.Write(mapSourceId);
        pkg.Write(instanceId);
        pkg.Write(pos);
        pkg.Write(cltHandle);
        pkg.SetMethod(11);
    }
    public void RPC_DSTalkMsg(RPC.PackageWriter pkg,System.UInt64 planesId,System.String sender,System.SByte channel,System.UInt64 targetId,System.String msg,RPC.DataWriter hyperlink)
    {
        pkg.Write(planesId);
        pkg.Write(sender);
        pkg.Write(channel);
        pkg.Write(targetId);
        pkg.Write(msg);
        pkg.Write(hyperlink);
        pkg.SetMethod(12);
    }
    public void RPC_AddGiftCount(RPC.PackageWriter pkg,System.UInt64 id,System.Int32 index,System.Int32 count)
    {
        pkg.Write(id);
        pkg.Write(index);
        pkg.Write(count);
        pkg.SetMethod(13);
    }
    public void RPC_SendPlayerMsg(RPC.PackageWriter pkg,System.UInt64 id,CSCommon.Data.Message msg)
    {
        pkg.Write(id);
        pkg.Write(msg);
        pkg.SetMethod(14);
    }
    public void RPC_SendPlayerMail(RPC.PackageWriter pkg,System.UInt64 id)
    {
        pkg.Write(id);
        pkg.SetMethod(15);
    }
    public void RPC_SendPlayerTeamInfo(RPC.PackageWriter pkg,System.UInt64 id,RPC.DataWriter dr)
    {
        pkg.Write(id);
        pkg.Write(dr);
        pkg.SetMethod(16);
    }
    public void RPC_SendToLeaveTeamPlayer(RPC.PackageWriter pkg,System.UInt64 id)
    {
        pkg.Write(id);
        pkg.SetMethod(17);
    }
}
}
namespace ServerCommon.Planes{
public class H_NPCInstance
{
    public static ServerCommon.Planes.H_NPCInstance smInstance = new ServerCommon.Planes.H_NPCInstance();
}
}
namespace ServerCommon.Planes{
public class H_PlayerImage
{
    public static ServerCommon.Planes.H_PlayerImage smInstance = new ServerCommon.Planes.H_PlayerImage();
}
}
namespace ServerCommon.Planes{
public class H_PlayerInstance
{
    public static ServerCommon.Planes.H_PlayerInstance smInstance = new ServerCommon.Planes.H_PlayerInstance();
    public void RPC_EnterCopy(RPC.PackageWriter pkg,System.Int32 id)
    {
        pkg.Write(id);
        pkg.SetMethod(0);
    }
    public void RPC_LeaveCopy(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(1);
    }
    public void RPC_GetPathFinderPoints(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(2);
    }
    public void RPC_GetChiefRoleCalValue(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(3);
    }
    public void RPC_AddRoleBasePoint(RPC.PackageWriter pkg,System.Byte type)
    {
        pkg.Write(type);
        pkg.SetMethod(4);
    }
    public void RPC_ResetRoleValue(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(5);
    }
    public void RPC_GetPlayerInfo(RPC.PackageWriter pkg,System.UInt64 id)
    {
        pkg.Write(id);
        pkg.SetMethod(6);
    }
    public void RPC_GetNPCInfo(RPC.PackageWriter pkg,System.UInt64 id)
    {
        pkg.Write(id);
        pkg.SetMethod(7);
    }
    public void RPC_SelectCamp(RPC.PackageWriter pkg,System.Byte camp)
    {
        pkg.Write(camp);
        pkg.SetMethod(8);
    }
    public void RPC_CreateTeam(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(9);
    }
    public void RPC_InviteToTeam(RPC.PackageWriter pkg,System.String otherName)
    {
        pkg.Write(otherName);
        pkg.SetMethod(10);
    }
    public void RPC_KickOutTeam(RPC.PackageWriter pkg,System.UInt64 otherId)
    {
        pkg.Write(otherId);
        pkg.SetMethod(11);
    }
    public void RPC_LeaveTeam(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(12);
    }
    public void RPC_OperateTeamAsk(RPC.PackageWriter pkg,System.String otherName,System.Byte operate)
    {
        pkg.Write(otherName);
        pkg.Write(operate);
        pkg.SetMethod(13);
    }
    public void RPC_OperateTeamInvite(RPC.PackageWriter pkg,System.String otherName,System.Byte operate)
    {
        pkg.Write(otherName);
        pkg.Write(operate);
        pkg.SetMethod(14);
    }
    public void RPC_GetTeamPlayers(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(15);
    }
    public void RPC_GetRewardTask(RPC.PackageWriter pkg,System.Int32 npcId,System.Int32 templateId)
    {
        pkg.Write(npcId);
        pkg.Write(templateId);
        pkg.SetMethod(16);
    }
    public void RPC_GetRoleCreateInfo(RPC.PackageWriter pkg,System.UInt64 id)
    {
        pkg.Write(id);
        pkg.SetMethod(17);
    }
    public void RPC_GlobalMapFindPath(RPC.PackageWriter pkg,SlimDX.Vector3 from,SlimDX.Vector3 to)
    {
        pkg.Write(from);
        pkg.Write(to);
        pkg.SetMethod(18);
    }
    public void RPC_DelMail(RPC.PackageWriter pkg,System.UInt64 mailId)
    {
        pkg.Write(mailId);
        pkg.SetMethod(19);
    }
    public void RPC_OpenMail(RPC.PackageWriter pkg,System.UInt64 mailId)
    {
        pkg.Write(mailId);
        pkg.SetMethod(20);
    }
    public void RPC_GetMailItems(RPC.PackageWriter pkg,System.UInt64 mailId)
    {
        pkg.Write(mailId);
        pkg.SetMethod(21);
    }
    public void RPC_OneKeyDelMails(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(22);
    }
    public void RPC_OneKeyGetItems(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(23);
    }
    public void RPC_GetSocialList(RPC.PackageWriter pkg,System.Byte type)
    {
        pkg.Write(type);
        pkg.SetMethod(24);
    }
    public void RPC_AddSocialByName(RPC.PackageWriter pkg,System.String name,System.Byte type)
    {
        pkg.Write(name);
        pkg.Write(type);
        pkg.SetMethod(25);
    }
    public void RPC_OperateAddSocial(RPC.PackageWriter pkg,System.String otherName,System.Byte type,System.Byte operate)
    {
        pkg.Write(otherName);
        pkg.Write(type);
        pkg.Write(operate);
        pkg.SetMethod(26);
    }
    public void RPC_RemoveSocial(RPC.PackageWriter pkg,System.UInt64 roleId,System.Byte type)
    {
        pkg.Write(roleId);
        pkg.Write(type);
        pkg.SetMethod(27);
    }
    public void RPC_SendGift(RPC.PackageWriter pkg,System.UInt64 roleId,System.Int32 templateId,System.Int32 count)
    {
        pkg.Write(roleId);
        pkg.Write(templateId);
        pkg.Write(count);
        pkg.SetMethod(28);
    }
    public void RPC_GetGuilds(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(29);
    }
    public void RPC_CreateGuild(RPC.PackageWriter pkg,System.String GuildName)
    {
        pkg.Write(GuildName);
        pkg.SetMethod(30);
    }
    public void RPC_LeaveGuild(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(31);
    }
    public void RPC_AskToGuild(RPC.PackageWriter pkg,System.UInt64 Id)
    {
        pkg.Write(Id);
        pkg.SetMethod(32);
    }
    public void RPC_OperateGuildAsk(RPC.PackageWriter pkg,System.UInt64 messageId,System.Byte operate)
    {
        pkg.Write(messageId);
        pkg.Write(operate);
        pkg.SetMethod(33);
    }
    public void RPC_KickedOutGuild(RPC.PackageWriter pkg,System.UInt64 id)
    {
        pkg.Write(id);
        pkg.SetMethod(34);
    }
    public void RPC_DissolveGuild(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(35);
    }
    public void RPC_SetGuildPost(RPC.PackageWriter pkg,System.UInt64 id,System.Byte post)
    {
        pkg.Write(id);
        pkg.Write(post);
        pkg.SetMethod(36);
    }
    public void RPC_InviteToGuild(RPC.PackageWriter pkg,System.String name)
    {
        pkg.Write(name);
        pkg.SetMethod(37);
    }
    public void RPC_OperateInvite(RPC.PackageWriter pkg,System.UInt64 roleId,System.Byte operate)
    {
        pkg.Write(roleId);
        pkg.Write(operate);
        pkg.SetMethod(38);
    }
    public void RPC_TransferGuildBangZhu(RPC.PackageWriter pkg,System.UInt64 targetId)
    {
        pkg.Write(targetId);
        pkg.SetMethod(39);
    }
    public void RPC_DonateGold(RPC.PackageWriter pkg,System.Int32 gold)
    {
        pkg.Write(gold);
        pkg.SetMethod(40);
    }
    public void RPC_IssueGold(RPC.PackageWriter pkg,System.Int32 gold)
    {
        pkg.Write(gold);
        pkg.SetMethod(41);
    }
    public void RPC_GetItemCdInfo(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(42);
    }
    public void RPC_UseItem(RPC.PackageWriter pkg,System.Byte bag,System.Int32 itemId,System.Int32 count)
    {
        pkg.Write(bag);
        pkg.Write(itemId);
        pkg.Write(count);
        pkg.SetMethod(43);
    }
    public void RPC_DestroyItem(RPC.PackageWriter pkg,System.Byte bag,System.UInt64 itemid)
    {
        pkg.Write(bag);
        pkg.Write(itemid);
        pkg.SetMethod(44);
    }
    public void RPC_SellItem(RPC.PackageWriter pkg,System.Int32 id,System.Int32 count)
    {
        pkg.Write(id);
        pkg.Write(count);
        pkg.SetMethod(45);
    }
    public void RPC_ReOrganizeBag(RPC.PackageWriter pkg,System.Byte bag)
    {
        pkg.Write(bag);
        pkg.SetMethod(46);
    }
    public void RPC_OpenShop(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(47);
    }
    public void RPC_BuyShopItem(RPC.PackageWriter pkg,System.Byte type,System.Int32 id,System.Int32 count)
    {
        pkg.Write(type);
        pkg.Write(id);
        pkg.Write(count);
        pkg.SetMethod(48);
    }
    public void RPC_CombineItem(RPC.PackageWriter pkg,System.Int32 itemId,System.Int32 itemNum)
    {
        pkg.Write(itemId);
        pkg.Write(itemNum);
        pkg.SetMethod(49);
    }
    public void RPC_ConsignItem(RPC.PackageWriter pkg,System.UInt64 itemId,System.Int32 stack,System.Int32 price)
    {
        pkg.Write(itemId);
        pkg.Write(stack);
        pkg.Write(price);
        pkg.SetMethod(50);
    }
    public void RPC_BuyConsignItem(RPC.PackageWriter pkg,System.UInt64 itemId)
    {
        pkg.Write(itemId);
        pkg.SetMethod(51);
    }
    public void RPC_GetRoleGird(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(52);
    }
    public void RPC_GetRoleGirdByName(RPC.PackageWriter pkg,System.String name,System.Byte findType,System.Byte page)
    {
        pkg.Write(name);
        pkg.Write(findType);
        pkg.Write(page);
        pkg.SetMethod(53);
    }
    public void RPC_GetRoleGirdByType(RPC.PackageWriter pkg,System.Byte itemType,System.Byte page)
    {
        pkg.Write(itemType);
        pkg.Write(page);
        pkg.SetMethod(54);
    }
    public void RPC_WearEquip(RPC.PackageWriter pkg,System.Int32 itemId)
    {
        pkg.Write(itemId);
        pkg.SetMethod(55);
    }
    public void RPC_TakeOffEquip(RPC.PackageWriter pkg,System.UInt64 equipId)
    {
        pkg.Write(equipId);
        pkg.SetMethod(56);
    }
    public void RPC_EquipIntensify(RPC.PackageWriter pkg,System.Int32 itemId)
    {
        pkg.Write(itemId);
        pkg.SetMethod(57);
    }
    public void RPC_EquipRefine(RPC.PackageWriter pkg,System.Int32 itemId,System.Int32 count)
    {
        pkg.Write(itemId);
        pkg.Write(count);
        pkg.SetMethod(58);
    }
    public void RPC_InlayGem(RPC.PackageWriter pkg,System.UInt16 pos,System.Int32 id)
    {
        pkg.Write(pos);
        pkg.Write(id);
        pkg.SetMethod(59);
    }
    public void RPC_RemoveGem(RPC.PackageWriter pkg,System.UInt16 pos)
    {
        pkg.Write(pos);
        pkg.SetMethod(60);
    }
    public void RPC_GemCombine(RPC.PackageWriter pkg,System.Int32 id)
    {
        pkg.Write(id);
        pkg.SetMethod(61);
    }
    public void RPC_GetFashion(RPC.PackageWriter pkg,System.Int32 id)
    {
        pkg.Write(id);
        pkg.SetMethod(62);
    }
    public void RPC_WearFashion(RPC.PackageWriter pkg,System.UInt64 itemId)
    {
        pkg.Write(itemId);
        pkg.SetMethod(63);
    }
    public void RPC_TakeOffFashion(RPC.PackageWriter pkg,System.UInt64 itemId)
    {
        pkg.Write(itemId);
        pkg.SetMethod(64);
    }
    public void RPC_EnterTrigger(RPC.PackageWriter pkg,System.UInt64 triggerId)
    {
        pkg.Write(triggerId);
        pkg.SetMethod(65);
    }
    public void RPC_LeaveTrigger(RPC.PackageWriter pkg,System.UInt64 triggerId)
    {
        pkg.Write(triggerId);
        pkg.SetMethod(66);
    }
    public void RPC_OnClientEnterMapOver(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(67);
    }
    public void RPC_SendFoodCar(RPC.PackageWriter pkg,System.Int32 level,System.Int32 num)
    {
        pkg.Write(level);
        pkg.Write(num);
        pkg.SetMethod(68);
    }
    public void RPC_Say(RPC.PackageWriter pkg,System.SByte channel,System.String msg,RPC.DataWriter hyperlink)
    {
        pkg.Write(channel);
        pkg.Write(msg);
        pkg.Write(hyperlink);
        pkg.SetMethod(69);
    }
    public void RPC_Whisper(RPC.PackageWriter pkg,System.String targetName,System.String msg,RPC.DataWriter hyperlink)
    {
        pkg.Write(targetName);
        pkg.Write(msg);
        pkg.Write(hyperlink);
        pkg.SetMethod(70);
    }
    public void RPC_GetAchieve(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(71);
    }
    public void RPC_GetAchieveName(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(72);
    }
    public void RPC_GetAchieveReward(RPC.PackageWriter pkg,System.Int32 id)
    {
        pkg.Write(id);
        pkg.SetMethod(73);
    }
    public void RPC_GetMyRank(RPC.PackageWriter pkg,System.Byte type)
    {
        pkg.Write(type);
        pkg.SetMethod(74);
    }
    public void RPC_OpenExploitBox(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(75);
    }
    public void RPC_GetDayRankDataReward(RPC.PackageWriter pkg,System.Byte type)
    {
        pkg.Write(type);
        pkg.SetMethod(76);
    }
    public void RPC_StartHook(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(77);
    }
    public void RPC_FetchCity(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(78);
    }
    public void RPC_FetchAddForceShortcutData(RPC.PackageWriter pkg,System.Int32 cityId)
    {
        pkg.Write(cityId);
        pkg.SetMethod(79);
    }
    public void RPC_SuggestCamp(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(80);
    }
    public void RPC_FetchBarrack(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(81);
    }
    public void RPC_AddForce(RPC.PackageWriter pkg,System.Int32 cityId,System.Byte forceType)
    {
        pkg.Write(cityId);
        pkg.Write(forceType);
        pkg.SetMethod(82);
    }
    public void RPC_AddForceShortcut(RPC.PackageWriter pkg,System.Int32 cityId,System.Byte forceType)
    {
        pkg.Write(cityId);
        pkg.Write(forceType);
        pkg.SetMethod(83);
    }
    public void RPC_CastSpell(RPC.PackageWriter pkg,System.UInt64 targetId,System.Int32 skillId)
    {
        pkg.Write(targetId);
        pkg.Write(skillId);
        pkg.SetMethod(84);
    }
    public void RPC_UpdatePosition(RPC.PackageWriter pkg,SlimDX.Vector3 pos,System.Single dir)
    {
        pkg.Write(pos);
        pkg.Write(dir);
        pkg.SetMethod(85);
    }
    public void RPC_Relive(RPC.PackageWriter pkg,System.Byte _mode)
    {
        pkg.Write(_mode);
        pkg.SetMethod(86);
    }
    public void RPC_UpMartialLevel(RPC.PackageWriter pkg,System.Byte type)
    {
        pkg.Write(type);
        pkg.SetMethod(87);
    }
    public void RPC_GetMartialInfo(RPC.PackageWriter pkg,System.Byte type)
    {
        pkg.Write(type);
        pkg.SetMethod(88);
    }
    public void RPC_GetOutPutReward(RPC.PackageWriter pkg,System.Byte type)
    {
        pkg.Write(type);
        pkg.SetMethod(89);
    }
    public void RPC_Visit(RPC.PackageWriter pkg,System.Byte type,System.UInt64 otherId)
    {
        pkg.Write(type);
        pkg.Write(otherId);
        pkg.SetMethod(90);
    }
    public void RPC_OpenVisit(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(91);
    }
    public void RPC_UpSkillLv(RPC.PackageWriter pkg,System.Int32 skillId)
    {
        pkg.Write(skillId);
        pkg.SetMethod(92);
    }
    public void RPC_UpCheats(RPC.PackageWriter pkg,System.Int32 skillId,System.Byte costType)
    {
        pkg.Write(skillId);
        pkg.Write(costType);
        pkg.SetMethod(93);
    }
    public void RPC_GetMails(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(94);
    }
}
}
namespace ServerCommon.Data.Player{
public class H_PlayerManager
{
    public static ServerCommon.Data.Player.H_PlayerManager smInstance = new ServerCommon.Data.Player.H_PlayerManager();
    public void RPC_GetOffPlayer(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(0);
    }
    public void RPC_GetOffPlayerList(RPC.PackageWriter pkg,RPC.DataWriter dr)
    {
        pkg.Write(dr);
        pkg.SetMethod(1);
    }
    public void LoginAccount(RPC.PackageWriter pkg,System.UInt16 cltIndex,System.String name,System.String psw,System.UInt16 planesId,System.UInt64 LinkSerialId)
    {
        pkg.Write(cltIndex);
        pkg.Write(name);
        pkg.Write(psw);
        pkg.Write(planesId);
        pkg.Write(LinkSerialId);
        pkg.SetMethod(2);
    }
    public void LogoutAccount(RPC.PackageWriter pkg,System.UInt64 accountId,System.SByte serverType)
    {
        pkg.Write(accountId);
        pkg.Write(serverType);
        pkg.SetMethod(3);
    }
    public void LoginRole(RPC.PackageWriter pkg,System.UInt64 linkSerialId,System.UInt16 cltIndex,System.UInt64 roleId,System.UInt64 accountId)
    {
        pkg.Write(linkSerialId);
        pkg.Write(cltIndex);
        pkg.Write(roleId);
        pkg.Write(accountId);
        pkg.SetMethod(4);
    }
    public void LogoutRole(RPC.PackageWriter pkg,System.UInt64 accountId,CSCommon.Data.PlayerData pd)
    {
        pkg.Write(accountId);
        pkg.Write(pd);
        pkg.SetMethod(5);
    }
    public void SaveRole(RPC.PackageWriter pkg,System.UInt64 roleId,CSCommon.Data.PlayerData pd)
    {
        pkg.Write(roleId);
        pkg.Write(pd);
        pkg.SetMethod(6);
    }
    public void RoleEnterPlanesSuccessed(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(7);
    }
    public void UpdataPlayerGuildData(RPC.PackageWriter pkg,System.UInt64 roleId,System.Guid GuildId,System.String GuildName,System.UInt32 GuildContribution)
    {
        pkg.Write(roleId);
        pkg.Write(GuildId);
        pkg.Write(GuildName);
        pkg.Write(GuildContribution);
        pkg.SetMethod(8);
    }
}
}
namespace ServerCommon{
public class H_IComServer
{
    public static ServerCommon.H_IComServer smInstance = new ServerCommon.H_IComServer();
    public ServerCommon.Com.H_UserRoleManager HGet_UserRoleManager(RPC.PackageWriter pkg)
    {
        pkg.PushStack(0);
        return ServerCommon.Com.H_UserRoleManager.smInstance;
    }
    public ServerCommon.Com.H_WorldManager HGet_WolrdManager(RPC.PackageWriter pkg)
    {
        pkg.PushStack(1);
        return ServerCommon.Com.H_WorldManager.smInstance;
    }
}
}
namespace ServerCommon{
public class H_IGateServer
{
    public static ServerCommon.H_IGateServer smInstance = new ServerCommon.H_IGateServer();
    public void NewPlanesServerStarted(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(0);
    }
    public void RPC_GuestLogin(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(1);
    }
    public void ClientTryLogin(RPC.PackageWriter pkg,System.String usr,System.String psw,System.UInt16 planesid)
    {
        pkg.Write(usr);
        pkg.Write(psw);
        pkg.Write(planesid);
        pkg.SetMethod(2);
    }
    public void RoleTryEnterGame(RPC.PackageWriter pkg,System.UInt64 roleId)
    {
        pkg.Write(roleId);
        pkg.SetMethod(3);
    }
    public void ReturnToRoleSelect(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(4);
    }
    public void PlayerEnterMapInPlanes(RPC.PackageWriter pkg,System.UInt64 roleId,System.UInt16 lnk,System.UInt16 indexInMap,System.UInt16 indexInServer)
    {
        pkg.Write(roleId);
        pkg.Write(lnk);
        pkg.Write(indexInMap);
        pkg.Write(indexInServer);
        pkg.SetMethod(5);
    }
    public void PlayerLeaveMapInPlanes(RPC.PackageWriter pkg,System.UInt16 lnk)
    {
        pkg.Write(lnk);
        pkg.SetMethod(6);
    }
    public void OtherPlane_EnterMap(RPC.PackageWriter pkg,System.UInt64 planesSeverId,System.UInt16 cltHandle,CSCommon.Data.PlayerData pd,CSCommon.Data.PlanesData planesData,System.UInt16 mapSourceId,System.UInt64 mapInstanceId,SlimDX.Vector3 pos)
    {
        pkg.Write(planesSeverId);
        pkg.Write(cltHandle);
        pkg.Write(pd);
        pkg.Write(planesData);
        pkg.Write(mapSourceId);
        pkg.Write(mapInstanceId);
        pkg.Write(pos);
        pkg.SetMethod(7);
    }
    public void TryRegAccount(RPC.PackageWriter pkg,System.String usr,System.String psw,System.String mobileNum)
    {
        pkg.Write(usr);
        pkg.Write(psw);
        pkg.Write(mobileNum);
        pkg.SetMethod(8);
    }
    public void RPC_RandRoleName(RPC.PackageWriter pkg,System.Byte sex)
    {
        pkg.Write(sex);
        pkg.SetMethod(9);
    }
    public void TryCreatePlayer(RPC.PackageWriter pkg,System.String planesName,System.String playerName,System.Byte pro,System.Byte sex)
    {
        pkg.Write(planesName);
        pkg.Write(playerName);
        pkg.Write(pro);
        pkg.Write(sex);
        pkg.SetMethod(10);
    }
    public void DeleteRole(RPC.PackageWriter pkg,System.UInt16 planesId,System.String roleName)
    {
        pkg.Write(planesId);
        pkg.Write(roleName);
        pkg.SetMethod(11);
    }
    public void SYS_ReloadItemTemplate(RPC.PackageWriter pkg,System.UInt16 item)
    {
        pkg.Write(item);
        pkg.SetMethod(12);
    }
    public void SYS_ReloadTaskTemplate(RPC.PackageWriter pkg,System.UInt16 item)
    {
        pkg.Write(item);
        pkg.SetMethod(13);
    }
    public void SYS_ReloadDropTemplate(RPC.PackageWriter pkg,System.UInt16 item)
    {
        pkg.Write(item);
        pkg.SetMethod(14);
    }
    public void SYS_ReloadSellerTemplate(RPC.PackageWriter pkg,System.UInt16 item)
    {
        pkg.Write(item);
        pkg.SetMethod(15);
    }
    public void DisconnectPlayer(RPC.PackageWriter pkg,System.UInt64 accountId,System.SByte serverType)
    {
        pkg.Write(accountId);
        pkg.Write(serverType);
        pkg.SetMethod(16);
    }
    public void DisconnectPlayerByConnectHandle(RPC.PackageWriter pkg,System.UInt16 lnk)
    {
        pkg.Write(lnk);
        pkg.SetMethod(17);
    }
}
}
namespace ServerCommon{
public class H_ILogServer
{
    public static ServerCommon.H_ILogServer smInstance = new ServerCommon.H_ILogServer();
    public void WriteDBLog(RPC.PackageWriter pkg,ServerFrame.DB.DBLogData data)
    {
        pkg.Write(data);
        pkg.SetMethod(0);
    }
}
}
namespace ServerCommon{
public class H_IRegisterServer
{
    public static ServerCommon.H_IRegisterServer smInstance = new ServerCommon.H_IRegisterServer();
    public void RegGateServer(RPC.PackageWriter pkg,System.String ipAddress,System.UInt64 id)
    {
        pkg.Write(ipAddress);
        pkg.Write(id);
        pkg.SetMethod(0);
    }
    public void GetGateServers(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(1);
    }
    public void RegPlanesServer(RPC.PackageWriter pkg,System.UInt64 id)
    {
        pkg.Write(id);
        pkg.SetMethod(2);
    }
    public void RegDataServer(RPC.PackageWriter pkg,System.String ListenIp,System.UInt16 ListenPort,System.UInt64 id)
    {
        pkg.Write(ListenIp);
        pkg.Write(ListenPort);
        pkg.Write(id);
        pkg.SetMethod(3);
    }
    public void RegPathFindServer(RPC.PackageWriter pkg,System.String ListenIp,System.UInt64 id)
    {
        pkg.Write(ListenIp);
        pkg.Write(id);
        pkg.SetMethod(4);
    }
    public void RegComServer(RPC.PackageWriter pkg,System.String ListenIp,System.UInt64 id)
    {
        pkg.Write(ListenIp);
        pkg.Write(id);
        pkg.SetMethod(5);
    }
    public void RegLogServer(RPC.PackageWriter pkg,System.String ListenIp,System.UInt16 ListenPort,System.UInt64 id)
    {
        pkg.Write(ListenIp);
        pkg.Write(ListenPort);
        pkg.Write(id);
        pkg.SetMethod(6);
    }
    public void SetGateLinkNumber(RPC.PackageWriter pkg,System.Int32 num)
    {
        pkg.Write(num);
        pkg.SetMethod(7);
    }
    public void RPC_QueryAllActivePlanesInfo(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(8);
    }
    public void GetLowGateServer(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(9);
    }
    public void GetDataServer(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(10);
    }
    public void GetComServer(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(11);
    }
    public void GetLogServer(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(12);
    }
    public void GetPlanesServers(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(13);
    }
}
}
namespace ServerCommon.Planes{
public class H_GameMaster
{
    public static ServerCommon.Planes.H_GameMaster smInstance = new ServerCommon.Planes.H_GameMaster();
    public void RPC_CreateItem2Bag(RPC.PackageWriter pkg,System.Int32 itemId)
    {
        pkg.Write(itemId);
        pkg.SetMethod(0);
    }
    public void RPC_AddExp(RPC.PackageWriter pkg,System.Int32 exp)
    {
        pkg.Write(exp);
        pkg.SetMethod(1);
    }
    public void RPC_AddMoney(RPC.PackageWriter pkg,System.Byte type,System.Int32 num)
    {
        pkg.Write(type);
        pkg.Write(num);
        pkg.SetMethod(2);
    }
    public void RPC_Revive(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(3);
    }
    public void RPC_JumpToMap(RPC.PackageWriter pkg,System.UInt16 mapid,System.Single x,System.Single z)
    {
        pkg.Write(mapid);
        pkg.Write(x);
        pkg.Write(z);
        pkg.SetMethod(4);
    }
    public void RPC_UpdateTaskById(RPC.PackageWriter pkg,System.Int32 taskId)
    {
        pkg.Write(taskId);
        pkg.SetMethod(5);
    }
}
}
namespace ServerCommon.Planes{
public class H_TriggerInstance
{
    public static ServerCommon.Planes.H_TriggerInstance smInstance = new ServerCommon.Planes.H_TriggerInstance();
}
}
namespace ServerCommon{
public class H_RPCRoot
{
    public static ServerCommon.H_RPCRoot smInstance = new ServerCommon.H_RPCRoot();
    public ServerCommon.H_IRegisterServer HGet_RegServer(RPC.PackageWriter pkg)
    {
        pkg.PushStack(0);
        return ServerCommon.H_IRegisterServer.smInstance;
    }
    public ServerCommon.H_IGateServer HGet_GateServer(RPC.PackageWriter pkg)
    {
        pkg.PushStack(1);
        return ServerCommon.H_IGateServer.smInstance;
    }
    public ServerCommon.H_IDataServer HGet_DataServer(RPC.PackageWriter pkg)
    {
        pkg.PushStack(2);
        return ServerCommon.H_IDataServer.smInstance;
    }
    public ServerCommon.H_IPlanesServer HGet_PlanesServer(RPC.PackageWriter pkg)
    {
        pkg.PushStack(3);
        return ServerCommon.H_IPlanesServer.smInstance;
    }
    public ServerCommon.H_IComServer HGet_ComServer(RPC.PackageWriter pkg)
    {
        pkg.PushStack(4);
        return ServerCommon.H_IComServer.smInstance;
    }
}
}
