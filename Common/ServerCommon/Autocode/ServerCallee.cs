//Server Callee
namespace RPC_ExecuterNamespace{
[RPC.RPCMethordExecuterTypeAttribute(1070481883,"RPC_GetSocialList",typeof(HExe_1070481883))]
public class HExe_1070481883: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.Byte type;
        pkg.Read(out type);
        host.RPC_GetSocialList(roleId,type,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1739064142,"RPC_AddSocial",typeof(HExe_1739064142))]
public class HExe_1739064142: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.String otherName;
        pkg.Read(out otherName);
        System.Byte type;
        pkg.Read(out type);
        host.RPC_AddSocial(roleId,otherName,type,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2456023713,"RPC_OperateAddSocial",typeof(HExe_2456023713))]
public class HExe_2456023713: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.String otherName;
        pkg.Read(out otherName);
        System.Byte type;
        pkg.Read(out type);
        System.Byte operate;
        pkg.Read(out operate);
        host.RPC_OperateAddSocial(roleId,otherName,type,operate,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4154793834,"RPC_RemoveSocial",typeof(HExe_4154793834))]
public class HExe_4154793834: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 otherId;
        pkg.Read(out otherId);
        System.Byte type;
        pkg.Read(out type);
        host.RPC_RemoveSocial(roleId,otherId,type,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2737966181,"RPC_SendGift",typeof(HExe_2737966181))]
public class HExe_2737966181: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 otherId;
        pkg.Read(out otherId);
        System.Int32 index;
        pkg.Read(out index);
        System.Int32 count;
        pkg.Read(out count);
        System.Int32 addvalue;
        pkg.Read(out addvalue);
        host.RPC_SendGift(roleId,otherId,index,count,addvalue,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2640677823,"RPC_ConsignItem",typeof(HExe_2640677823))]
public class HExe_2640677823: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.Int32 templateId;
        pkg.Read(out templateId);
        System.Int32 stack;
        pkg.Read(out stack);
        System.Int32 price;
        pkg.Read(out price);
        host.RPC_ConsignItem(roleId,templateId,stack,price,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1717183888,"RPC_BuyConsignItem",typeof(HExe_1717183888))]
public class HExe_1717183888: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 itemId;
        pkg.Read(out itemId);
        host.RPC_BuyConsignItem(roleId,itemId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(280160435,"RPC_GetConsignItem",typeof(HExe_280160435))]
public class HExe_280160435: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 itemId;
        pkg.Read(out itemId);
        host.RPC_GetConsignItem(roleId,itemId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(795329423,"RPC_GetRoleGird",typeof(HExe_795329423))]
public class HExe_795329423: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RPC_GetRoleGird(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2676902958,"RPC_GetRoleGirdByName",typeof(HExe_2676902958))]
public class HExe_2676902958: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.String name;
        pkg.Read(out name);
        System.Byte findType;
        pkg.Read(out findType);
        System.Byte page;
        pkg.Read(out page);
        host.RPC_GetRoleGirdByName(roleId,name,findType,page,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2810629639,"RPC_GetRoleGirdByType",typeof(HExe_2810629639))]
public class HExe_2810629639: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.Byte itemType;
        pkg.Read(out itemType);
        System.Byte page;
        pkg.Read(out page);
        host.RPC_GetRoleGirdByType(roleId,itemType,page,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(46938956,"RPC_RoleEnterPlanes",typeof(HExe_46938956))]
public class HExe_46938956: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        CSCommon.Data.RoleDetail roledata = new CSCommon.Data.RoleDetail();
        pkg.Read( roledata);
        host.RPC_RoleEnterPlanes(roledata,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3636771780,"RPC_RoleLogout",typeof(HExe_3636771780))]
public class HExe_3636771780: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.Byte[] offvalue;
        pkg.Read(out offvalue);
        host.RPC_RoleLogout(roleId,offvalue);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(627528374,"RPC_UpdateRoleComValue",typeof(HExe_627528374))]
public class HExe_627528374: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.String name;
        pkg.Read(out name);
        RPC.DataReader value;
        pkg.Read(out value);
        host.RPC_UpdateRoleComValue(roleId,name,value);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(803423025,"RPC_SearchPlayerByName",typeof(HExe_803423025))]
public class HExe_803423025: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.String roleName;
        pkg.Read(out roleName);
        host.RPC_SearchPlayerByName(roleName,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2647298097,"RPC_GetTopAndFriend",typeof(HExe_2647298097))]
public class HExe_2647298097: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt16 planesId;
        pkg.Read(out planesId);
        host.RPC_GetTopAndFriend(roleId,planesId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2306423160,"RPC_Visit",typeof(HExe_2306423160))]
public class HExe_2306423160: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.Byte type;
        pkg.Read(out type);
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 otherId;
        pkg.Read(out otherId);
        host.RPC_Visit(type,roleId,otherId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2359050701,"RPC_GetOffPlayerData",typeof(HExe_2359050701))]
public class HExe_2359050701: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RPC_GetOffPlayerData(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3027256072,"RPC_UpdateRankDataValue",typeof(HExe_3027256072))]
public class HExe_3027256072: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.String name;
        pkg.Read(out name);
        RPC.DataReader value;
        pkg.Read(out value);
        host.RPC_UpdateRankDataValue(roleId,name,value);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3601780197,"RPC_GetMyRank",typeof(HExe_3601780197))]
public class HExe_3601780197: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 id;
        pkg.Read(out id);
        System.Byte type;
        pkg.Read(out type);
        host.RPC_GetMyRank(id,type,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2245150845,"RPC_CreateTeam",typeof(HExe_2245150845))]
public class HExe_2245150845: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RPC_CreateTeam(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(565921859,"RPC_InviteToTeam",typeof(HExe_565921859))]
public class HExe_565921859: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.String otherName;
        pkg.Read(out otherName);
        host.RPC_InviteToTeam(roleId,otherName,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3349253733,"RPC_KickOutTeam",typeof(HExe_3349253733))]
public class HExe_3349253733: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 otherId;
        pkg.Read(out otherId);
        host.RPC_KickOutTeam(roleId,otherId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2086613904,"RPC_LeaveTeam",typeof(HExe_2086613904))]
public class HExe_2086613904: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RPC_LeaveTeam(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3202384357,"RPC_OperateTeamAsk",typeof(HExe_3202384357))]
public class HExe_3202384357: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.String otherName;
        pkg.Read(out otherName);
        System.Byte operate;
        pkg.Read(out operate);
        host.RPC_OperateTeamAsk(roleId,otherName,operate,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3591420715,"RPC_OperateTeamInvite",typeof(HExe_3591420715))]
public class HExe_3591420715: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.String otherName;
        pkg.Read(out otherName);
        System.Byte operate;
        pkg.Read(out operate);
        host.RPC_OperateTeamInvite(roleId,otherName,operate,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4216555195,"RPC_GetTeamPlayers",typeof(HExe_4216555195))]
public class HExe_4216555195: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RPC_GetTeamPlayers(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1531056815,"RPC_GetMails",typeof(HExe_1531056815))]
public class HExe_1531056815: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RPC_GetMails(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1784734408,"RPC_DelMail",typeof(HExe_1784734408))]
public class HExe_1784734408: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 mailId;
        pkg.Read(out mailId);
        host.RPC_DelMail(mailId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3303687663,"RPC_OpenMail",typeof(HExe_3303687663))]
public class HExe_3303687663: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 mailId;
        pkg.Read(out mailId);
        host.RPC_OpenMail(mailId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1693923301,"RPC_GetMailItems",typeof(HExe_1693923301))]
public class HExe_1693923301: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 mailId;
        pkg.Read(out mailId);
        host.RPC_GetMailItems(mailId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3541540232,"RPC_OneKeyDelMails",typeof(HExe_3541540232))]
public class HExe_3541540232: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RPC_OneKeyDelMails(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2247577661,"RPC_OneKeyGetItems",typeof(HExe_2247577661))]
public class HExe_2247577661: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RPC_OneKeyGetItems(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1045747085,"RPC_GetGuilds",typeof(HExe_1045747085))]
public class HExe_1045747085: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RPC_GetGuilds(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(402790377,"RPC_CreateGuild",typeof(HExe_402790377))]
public class HExe_402790377: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.String GuildName;
        pkg.Read(out GuildName);
        host.RPC_CreateGuild(roleId,GuildName,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2843346904,"RPC_LeaveGuild",typeof(HExe_2843346904))]
public class HExe_2843346904: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RPC_LeaveGuild(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(889497932,"RPC_AskToGuild",typeof(HExe_889497932))]
public class HExe_889497932: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 guildId;
        pkg.Read(out guildId);
        host.RPC_AskToGuild(roleId,guildId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3586105770,"RPC_OperateGuildAsk",typeof(HExe_3586105770))]
public class HExe_3586105770: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 messageId;
        pkg.Read(out messageId);
        System.Byte operate;
        pkg.Read(out operate);
        host.RPC_OperateGuildAsk(roleId,messageId,operate,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(319076046,"RPC_KickedOutGuild",typeof(HExe_319076046))]
public class HExe_319076046: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 targetId;
        pkg.Read(out targetId);
        host.RPC_KickedOutGuild(roleId,targetId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1312967023,"RPC_DissolveGuild",typeof(HExe_1312967023))]
public class HExe_1312967023: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RPC_DissolveGuild(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(743345890,"RPC_SetGuildPost",typeof(HExe_743345890))]
public class HExe_743345890: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 targetId;
        pkg.Read(out targetId);
        System.Byte post;
        pkg.Read(out post);
        host.RPC_SetGuildPost(roleId,targetId,post,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3731998305,"RPC_InviteToGuild",typeof(HExe_3731998305))]
public class HExe_3731998305: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.String tarName;
        pkg.Read(out tarName);
        host.RPC_InviteToGuild(roleId,tarName,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(236811980,"RPC_OperateInvite",typeof(HExe_236811980))]
public class HExe_236811980: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 targetId;
        pkg.Read(out targetId);
        System.Byte operate;
        pkg.Read(out operate);
        host.RPC_OperateInvite(roleId,targetId,operate,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2973573372,"RPC_TransferGuildBangZhu",typeof(HExe_2973573372))]
public class HExe_2973573372: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 targetId;
        pkg.Read(out targetId);
        host.RPC_TransferGuildBangZhu(roleId,targetId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(861751381,"RPC_DonateGold",typeof(HExe_861751381))]
public class HExe_861751381: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.Int32 gold;
        pkg.Read(out gold);
        host.RPC_DonateGold(roleId,gold,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(548803112,"RPC_IssueGold",typeof(HExe_548803112))]
public class HExe_548803112: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.Int32 gold;
        pkg.Read(out gold);
        host.RPC_IssueGold(roleId,gold,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1209257994,"RPC_SayToRole",typeof(HExe_1209257994))]
public class HExe_1209257994: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.String tarName;
        pkg.Read(out tarName);
        System.String msg;
        pkg.Read(out msg);
        RPC.DataReader hyperlink;
        pkg.Read(out hyperlink);
        host.RPC_SayToRole(roleId,tarName,msg,hyperlink,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1977196125,"RPC_Say",typeof(HExe_1977196125))]
public class HExe_1977196125: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Com.UserRoleManager host = obj as ServerCommon.Com.UserRoleManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.SByte channel;
        pkg.Read(out channel);
        System.String msg;
        pkg.Read(out msg);
        RPC.DataReader hyperlink;
        pkg.Read(out hyperlink);
        host.RPC_Say(roleId,channel,msg,hyperlink,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(854750530,"RPC_EnterCopy",typeof(HExe_854750530))]
public class HExe_854750530: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 id;
        pkg.Read(out id);
        host.RPC_EnterCopy(id,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3095799854,"RPC_LeaveCopy",typeof(HExe_3095799854))]
public class HExe_3095799854: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_LeaveCopy(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3047086403,"RPC_GetPathFinderPoints",typeof(HExe_3047086403))]
public class HExe_3047086403: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_GetPathFinderPoints(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(602123432,"RPC_GetChiefRoleCalValue",typeof(HExe_602123432))]
public class HExe_602123432: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_GetChiefRoleCalValue(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(614746597,"RPC_AddRoleBasePoint",typeof(HExe_614746597))]
public class HExe_614746597: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte type;
        pkg.Read(out type);
        host.RPC_AddRoleBasePoint(type,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2146033032,"RPC_ResetRoleValue",typeof(HExe_2146033032))]
public class HExe_2146033032: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_ResetRoleValue(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4010485661,"RPC_GetPlayerInfo",typeof(HExe_4010485661))]
public class HExe_4010485661: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 id;
        pkg.Read(out id);
        host.RPC_GetPlayerInfo(id,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1990248090,"RPC_GetNPCInfo",typeof(HExe_1990248090))]
public class HExe_1990248090: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 id;
        pkg.Read(out id);
        host.RPC_GetNPCInfo(id,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(209426158,"RPC_SelectCamp",typeof(HExe_209426158))]
public class HExe_209426158: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte camp;
        pkg.Read(out camp);
        host.RPC_SelectCamp(camp,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4111965867,"RPC_CreateTeam",typeof(HExe_4111965867))]
public class HExe_4111965867: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_CreateTeam(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2387594059,"RPC_InviteToTeam",typeof(HExe_2387594059))]
public class HExe_2387594059: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.String otherName;
        pkg.Read(out otherName);
        host.RPC_InviteToTeam(otherName,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(981318454,"RPC_KickOutTeam",typeof(HExe_981318454))]
public class HExe_981318454: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 otherId;
        pkg.Read(out otherId);
        host.RPC_KickOutTeam(otherId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2936138087,"RPC_LeaveTeam",typeof(HExe_2936138087))]
public class HExe_2936138087: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_LeaveTeam(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1905163712,"RPC_OperateTeamAsk",typeof(HExe_1905163712))]
public class HExe_1905163712: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.String otherName;
        pkg.Read(out otherName);
        System.Byte operate;
        pkg.Read(out operate);
        host.RPC_OperateTeamAsk(otherName,operate,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3270236743,"RPC_OperateTeamInvite",typeof(HExe_3270236743))]
public class HExe_3270236743: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.String otherName;
        pkg.Read(out otherName);
        System.Byte operate;
        pkg.Read(out operate);
        host.RPC_OperateTeamInvite(otherName,operate,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(639174217,"RPC_GetTeamPlayers",typeof(HExe_639174217))]
public class HExe_639174217: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_GetTeamPlayers(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3918607182,"RPC_GetRewardTask",typeof(HExe_3918607182))]
public class HExe_3918607182: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 npcId;
        pkg.Read(out npcId);
        System.Int32 templateId;
        pkg.Read(out templateId);
        host.RPC_GetRewardTask(npcId,templateId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3106456801,"RPC_GetRoleCreateInfo",typeof(HExe_3106456801))]
public class HExe_3106456801: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 id;
        pkg.Read(out id);
        host.RPC_GetRoleCreateInfo(id,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2876258328,"RPC_GlobalMapFindPath",typeof(HExe_2876258328))]
public class HExe_2876258328: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        SlimDX.Vector3 from;
        pkg.Read(out from);
        SlimDX.Vector3 to;
        pkg.Read(out to);
        host.RPC_GlobalMapFindPath(from,to,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(18149753,"RPC_DelMail",typeof(HExe_18149753))]
public class HExe_18149753: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 mailId;
        pkg.Read(out mailId);
        host.RPC_DelMail(mailId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3920813871,"RPC_OpenMail",typeof(HExe_3920813871))]
public class HExe_3920813871: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 mailId;
        pkg.Read(out mailId);
        host.RPC_OpenMail(mailId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2489700813,"RPC_GetMailItems",typeof(HExe_2489700813))]
public class HExe_2489700813: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 mailId;
        pkg.Read(out mailId);
        host.RPC_GetMailItems(mailId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1399030718,"RPC_OneKeyDelMails",typeof(HExe_1399030718))]
public class HExe_1399030718: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_OneKeyDelMails(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3105973124,"RPC_OneKeyGetItems",typeof(HExe_3105973124))]
public class HExe_3105973124: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_OneKeyGetItems(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4100902645,"RPC_GetSocialList",typeof(HExe_4100902645))]
public class HExe_4100902645: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte type;
        pkg.Read(out type);
        host.RPC_GetSocialList(type,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4050085165,"RPC_AddSocialByName",typeof(HExe_4050085165))]
public class HExe_4050085165: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.String name;
        pkg.Read(out name);
        System.Byte type;
        pkg.Read(out type);
        host.RPC_AddSocialByName(name,type,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1809380907,"RPC_OperateAddSocial",typeof(HExe_1809380907))]
public class HExe_1809380907: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.String otherName;
        pkg.Read(out otherName);
        System.Byte type;
        pkg.Read(out type);
        System.Byte operate;
        pkg.Read(out operate);
        host.RPC_OperateAddSocial(otherName,type,operate,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1284634388,"RPC_RemoveSocial",typeof(HExe_1284634388))]
public class HExe_1284634388: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.Byte type;
        pkg.Read(out type);
        host.RPC_RemoveSocial(roleId,type,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2205559883,"RPC_SendGift",typeof(HExe_2205559883))]
public class HExe_2205559883: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.Int32 templateId;
        pkg.Read(out templateId);
        System.Int32 count;
        pkg.Read(out count);
        host.RPC_SendGift(roleId,templateId,count,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3563699092,"RPC_GetGuilds",typeof(HExe_3563699092))]
public class HExe_3563699092: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_GetGuilds(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4216301954,"RPC_CreateGuild",typeof(HExe_4216301954))]
public class HExe_4216301954: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.String GuildName;
        pkg.Read(out GuildName);
        host.RPC_CreateGuild(GuildName,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1872967421,"RPC_LeaveGuild",typeof(HExe_1872967421))]
public class HExe_1872967421: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_LeaveGuild(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4270034289,"RPC_AskToGuild",typeof(HExe_4270034289))]
public class HExe_4270034289: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 Id;
        pkg.Read(out Id);
        host.RPC_AskToGuild(Id,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2494350067,"RPC_OperateGuildAsk",typeof(HExe_2494350067))]
public class HExe_2494350067: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 messageId;
        pkg.Read(out messageId);
        System.Byte operate;
        pkg.Read(out operate);
        host.RPC_OperateGuildAsk(messageId,operate,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2575541438,"RPC_KickedOutGuild",typeof(HExe_2575541438))]
public class HExe_2575541438: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 id;
        pkg.Read(out id);
        host.RPC_KickedOutGuild(id,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1211475058,"RPC_DissolveGuild",typeof(HExe_1211475058))]
public class HExe_1211475058: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_DissolveGuild(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4092167642,"RPC_SetGuildPost",typeof(HExe_4092167642))]
public class HExe_4092167642: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 id;
        pkg.Read(out id);
        System.Byte post;
        pkg.Read(out post);
        host.RPC_SetGuildPost(id,post,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1733762118,"RPC_InviteToGuild",typeof(HExe_1733762118))]
public class HExe_1733762118: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.String name;
        pkg.Read(out name);
        host.RPC_InviteToGuild(name,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(435609608,"RPC_OperateInvite",typeof(HExe_435609608))]
public class HExe_435609608: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.Byte operate;
        pkg.Read(out operate);
        host.RPC_OperateInvite(roleId,operate,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(110493767,"RPC_TransferGuildBangZhu",typeof(HExe_110493767))]
public class HExe_110493767: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 targetId;
        pkg.Read(out targetId);
        host.RPC_TransferGuildBangZhu(targetId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2203248439,"RPC_DonateGold",typeof(HExe_2203248439))]
public class HExe_2203248439: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 gold;
        pkg.Read(out gold);
        host.RPC_DonateGold(gold,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2990328239,"RPC_IssueGold",typeof(HExe_2990328239))]
public class HExe_2990328239: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 gold;
        pkg.Read(out gold);
        host.RPC_IssueGold(gold,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3526749364,"RPC_GetItemCdInfo",typeof(HExe_3526749364))]
public class HExe_3526749364: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_GetItemCdInfo(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2337522896,"RPC_UseItem",typeof(HExe_2337522896))]
public class HExe_2337522896: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte bag;
        pkg.Read(out bag);
        System.Int32 itemId;
        pkg.Read(out itemId);
        System.Int32 count;
        pkg.Read(out count);
        host.RPC_UseItem(bag,itemId,count,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3985894704,"RPC_DestroyItem",typeof(HExe_3985894704))]
public class HExe_3985894704: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte bag;
        pkg.Read(out bag);
        System.UInt64 itemid;
        pkg.Read(out itemid);
        host.RPC_DestroyItem(bag,itemid,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4228769425,"RPC_SellItem",typeof(HExe_4228769425))]
public class HExe_4228769425: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 id;
        pkg.Read(out id);
        System.Int32 count;
        pkg.Read(out count);
        host.RPC_SellItem(id,count,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2699925958,"RPC_ReOrganizeBag",typeof(HExe_2699925958))]
public class HExe_2699925958: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte bag;
        pkg.Read(out bag);
        host.RPC_ReOrganizeBag(bag,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(397951486,"RPC_OpenShop",typeof(HExe_397951486))]
public class HExe_397951486: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_OpenShop(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1737887591,"RPC_BuyShopItem",typeof(HExe_1737887591))]
public class HExe_1737887591: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte type;
        pkg.Read(out type);
        System.Int32 id;
        pkg.Read(out id);
        System.Int32 count;
        pkg.Read(out count);
        host.RPC_BuyShopItem(type,id,count,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1470110452,"RPC_CombineItem",typeof(HExe_1470110452))]
public class HExe_1470110452: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 itemId;
        pkg.Read(out itemId);
        System.Int32 itemNum;
        pkg.Read(out itemNum);
        host.RPC_CombineItem(itemId,itemNum,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3325750559,"RPC_ConsignItem",typeof(HExe_3325750559))]
public class HExe_3325750559: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 itemId;
        pkg.Read(out itemId);
        System.Int32 stack;
        pkg.Read(out stack);
        System.Int32 price;
        pkg.Read(out price);
        host.RPC_ConsignItem(itemId,stack,price,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1909472311,"RPC_BuyConsignItem",typeof(HExe_1909472311))]
public class HExe_1909472311: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 itemId;
        pkg.Read(out itemId);
        host.RPC_BuyConsignItem(itemId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(689178625,"RPC_GetRoleGird",typeof(HExe_689178625))]
public class HExe_689178625: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_GetRoleGird(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(858680085,"RPC_GetRoleGirdByName",typeof(HExe_858680085))]
public class HExe_858680085: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.String name;
        pkg.Read(out name);
        System.Byte findType;
        pkg.Read(out findType);
        System.Byte page;
        pkg.Read(out page);
        host.RPC_GetRoleGirdByName(name,findType,page,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3281807773,"RPC_GetRoleGirdByType",typeof(HExe_3281807773))]
public class HExe_3281807773: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte itemType;
        pkg.Read(out itemType);
        System.Byte page;
        pkg.Read(out page);
        host.RPC_GetRoleGirdByType(itemType,page,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1562578405,"RPC_WearEquip",typeof(HExe_1562578405))]
public class HExe_1562578405: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 itemId;
        pkg.Read(out itemId);
        host.RPC_WearEquip(itemId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3460975487,"RPC_TakeOffEquip",typeof(HExe_3460975487))]
public class HExe_3460975487: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 equipId;
        pkg.Read(out equipId);
        host.RPC_TakeOffEquip(equipId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4137106746,"RPC_EquipIntensify",typeof(HExe_4137106746))]
public class HExe_4137106746: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 itemId;
        pkg.Read(out itemId);
        host.RPC_EquipIntensify(itemId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1672586707,"RPC_EquipRefine",typeof(HExe_1672586707))]
public class HExe_1672586707: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 itemId;
        pkg.Read(out itemId);
        System.Int32 count;
        pkg.Read(out count);
        host.RPC_EquipRefine(itemId,count,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(272436567,"RPC_InlayGem",typeof(HExe_272436567))]
public class HExe_272436567: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt16 pos;
        pkg.Read(out pos);
        System.Int32 id;
        pkg.Read(out id);
        host.RPC_InlayGem(pos,id,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2388876356,"RPC_RemoveGem",typeof(HExe_2388876356))]
public class HExe_2388876356: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt16 pos;
        pkg.Read(out pos);
        host.RPC_RemoveGem(pos,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2148466191,"RPC_GemCombine",typeof(HExe_2148466191))]
public class HExe_2148466191: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 id;
        pkg.Read(out id);
        host.RPC_GemCombine(id,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3510396002,"RPC_GetFashion",typeof(HExe_3510396002))]
public class HExe_3510396002: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 id;
        pkg.Read(out id);
        host.RPC_GetFashion(id,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3402823320,"RPC_WearFashion",typeof(HExe_3402823320))]
public class HExe_3402823320: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 itemId;
        pkg.Read(out itemId);
        host.RPC_WearFashion(itemId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(405232310,"RPC_TakeOffFashion",typeof(HExe_405232310))]
public class HExe_405232310: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 itemId;
        pkg.Read(out itemId);
        host.RPC_TakeOffFashion(itemId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(248097711,"RPC_EnterTrigger",typeof(HExe_248097711))]
public class HExe_248097711: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 triggerId;
        pkg.Read(out triggerId);
        host.RPC_EnterTrigger(triggerId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2007571158,"RPC_LeaveTrigger",typeof(HExe_2007571158))]
public class HExe_2007571158: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 triggerId;
        pkg.Read(out triggerId);
        host.RPC_LeaveTrigger(triggerId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4089844448,"RPC_OnClientEnterMapOver",typeof(HExe_4089844448))]
public class HExe_4089844448: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_OnClientEnterMapOver(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3676617206,"RPC_SendFoodCar",typeof(HExe_3676617206))]
public class HExe_3676617206: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 level;
        pkg.Read(out level);
        System.Int32 num;
        pkg.Read(out num);
        host.RPC_SendFoodCar(level,num,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1069892221,"RPC_Say",typeof(HExe_1069892221))]
public class HExe_1069892221: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.SByte channel;
        pkg.Read(out channel);
        System.String msg;
        pkg.Read(out msg);
        RPC.DataReader hyperlink;
        pkg.Read(out hyperlink);
        host.RPC_Say(channel,msg,hyperlink,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1048097490,"RPC_Whisper",typeof(HExe_1048097490))]
public class HExe_1048097490: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.String targetName;
        pkg.Read(out targetName);
        System.String msg;
        pkg.Read(out msg);
        RPC.DataReader hyperlink;
        pkg.Read(out hyperlink);
        host.RPC_Whisper(targetName,msg,hyperlink,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3634436165,"RPC_GetAchieve",typeof(HExe_3634436165))]
public class HExe_3634436165: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_GetAchieve(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3308281991,"RPC_GetAchieveName",typeof(HExe_3308281991))]
public class HExe_3308281991: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_GetAchieveName(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3162720426,"RPC_GetAchieveReward",typeof(HExe_3162720426))]
public class HExe_3162720426: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 id;
        pkg.Read(out id);
        host.RPC_GetAchieveReward(id,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4225678388,"RPC_GetMyRank",typeof(HExe_4225678388))]
public class HExe_4225678388: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte type;
        pkg.Read(out type);
        host.RPC_GetMyRank(type,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1496608088,"RPC_OpenExploitBox",typeof(HExe_1496608088))]
public class HExe_1496608088: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_OpenExploitBox(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1745093561,"RPC_GetDayRankDataReward",typeof(HExe_1745093561))]
public class HExe_1745093561: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte type;
        pkg.Read(out type);
        host.RPC_GetDayRankDataReward(type,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2144528820,"RPC_StartHook",typeof(HExe_2144528820))]
public class HExe_2144528820: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_StartHook(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2065666321,"RPC_FetchCity",typeof(HExe_2065666321))]
public class HExe_2065666321: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_FetchCity(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(919698329,"RPC_FetchAddForceShortcutData",typeof(HExe_919698329))]
public class HExe_919698329: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 cityId;
        pkg.Read(out cityId);
        host.RPC_FetchAddForceShortcutData(cityId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1654219612,"RPC_SuggestCamp",typeof(HExe_1654219612))]
public class HExe_1654219612: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_SuggestCamp(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2383081553,"RPC_FetchBarrack",typeof(HExe_2383081553))]
public class HExe_2383081553: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_FetchBarrack(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3384109364,"RPC_AddForce",typeof(HExe_3384109364))]
public class HExe_3384109364: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 cityId;
        pkg.Read(out cityId);
        System.Byte forceType;
        pkg.Read(out forceType);
        host.RPC_AddForce(cityId,forceType,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4112323569,"RPC_AddForceShortcut",typeof(HExe_4112323569))]
public class HExe_4112323569: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 cityId;
        pkg.Read(out cityId);
        System.Byte forceType;
        pkg.Read(out forceType);
        host.RPC_AddForceShortcut(cityId,forceType,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(794399681,"RPC_CastSpell",typeof(HExe_794399681))]
public class HExe_794399681: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.UInt64 targetId;
        pkg.Read(out targetId);
        System.Int32 skillId;
        pkg.Read(out skillId);
        host.RPC_CastSpell(targetId,skillId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2592128971,"RPC_UpdatePosition",typeof(HExe_2592128971))]
public class HExe_2592128971: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        SlimDX.Vector3 pos;
        pkg.Read(out pos);
        System.Single dir;
        pkg.Read(out dir);
        host.RPC_UpdatePosition(pos,dir,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4237676595,"RPC_Relive",typeof(HExe_4237676595))]
public class HExe_4237676595: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte _mode;
        pkg.Read(out _mode);
        host.RPC_Relive(_mode,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2463694110,"RPC_UpMartialLevel",typeof(HExe_2463694110))]
public class HExe_2463694110: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte type;
        pkg.Read(out type);
        host.RPC_UpMartialLevel(type,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2400247181,"RPC_GetMartialInfo",typeof(HExe_2400247181))]
public class HExe_2400247181: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte type;
        pkg.Read(out type);
        host.RPC_GetMartialInfo(type,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2712795904,"RPC_GetOutPutReward",typeof(HExe_2712795904))]
public class HExe_2712795904: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte type;
        pkg.Read(out type);
        host.RPC_GetOutPutReward(type,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3114715086,"RPC_Visit",typeof(HExe_3114715086))]
public class HExe_3114715086: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Byte type;
        pkg.Read(out type);
        System.UInt64 otherId;
        pkg.Read(out otherId);
        host.RPC_Visit(type,otherId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3526576787,"RPC_OpenVisit",typeof(HExe_3526576787))]
public class HExe_3526576787: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_OpenVisit(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1388439319,"RPC_UpSkillLv",typeof(HExe_1388439319))]
public class HExe_1388439319: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 skillId;
        pkg.Read(out skillId);
        host.RPC_UpSkillLv(skillId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3529032970,"RPC_UpCheats",typeof(HExe_3529032970))]
public class HExe_3529032970: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        System.Int32 skillId;
        pkg.Read(out skillId);
        System.Byte costType;
        pkg.Read(out costType);
        host.RPC_UpCheats(skillId,costType,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3850687818,"RPC_GetMails",typeof(HExe_3850687818))]
public class HExe_3850687818: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.PlayerInstance host = obj as ServerCommon.Planes.PlayerInstance;
        if (host == null) return null;
        host.RPC_GetMails(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4168519198,"RPC_GetOffPlayer",typeof(HExe_4168519198))]
public class HExe_4168519198: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Data.Player.PlayerManager host = obj as ServerCommon.Data.Player.PlayerManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RPC_GetOffPlayer(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2160127548,"RPC_GetOffPlayerList",typeof(HExe_2160127548))]
public class HExe_2160127548: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Data.Player.PlayerManager host = obj as ServerCommon.Data.Player.PlayerManager;
        if (host == null) return null;
        RPC.DataReader dr;
        pkg.Read(out dr);
        host.RPC_GetOffPlayerList(dr,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2168762344,"LoginAccount",typeof(HExe_2168762344))]
public class HExe_2168762344: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Data.Player.PlayerManager host = obj as ServerCommon.Data.Player.PlayerManager;
        if (host == null) return null;
        System.UInt16 cltIndex;
        pkg.Read(out cltIndex);
        System.String name;
        pkg.Read(out name);
        System.String psw;
        pkg.Read(out psw);
        System.UInt16 planesId;
        pkg.Read(out planesId);
        System.UInt64 LinkSerialId;
        pkg.Read(out LinkSerialId);
        host.LoginAccount(cltIndex,name,psw,planesId,LinkSerialId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(434900464,"LogoutAccount",typeof(HExe_434900464))]
public class HExe_434900464: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Data.Player.PlayerManager host = obj as ServerCommon.Data.Player.PlayerManager;
        if (host == null) return null;
        System.UInt64 accountId;
        pkg.Read(out accountId);
        System.SByte serverType;
        pkg.Read(out serverType);
        host.LogoutAccount(accountId,serverType);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3124269720,"LoginRole",typeof(HExe_3124269720))]
public class HExe_3124269720: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Data.Player.PlayerManager host = obj as ServerCommon.Data.Player.PlayerManager;
        if (host == null) return null;
        System.UInt64 linkSerialId;
        pkg.Read(out linkSerialId);
        System.UInt16 cltIndex;
        pkg.Read(out cltIndex);
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 accountId;
        pkg.Read(out accountId);
        host.LoginRole(linkSerialId,cltIndex,roleId,accountId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4190075605,"LogoutRole",typeof(HExe_4190075605))]
public class HExe_4190075605: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Data.Player.PlayerManager host = obj as ServerCommon.Data.Player.PlayerManager;
        if (host == null) return null;
        System.UInt64 accountId;
        pkg.Read(out accountId);
        CSCommon.Data.PlayerData pd = new CSCommon.Data.PlayerData();
        pkg.Read( pd);
        return host.LogoutRole(accountId,pd);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(311709998,"SaveRole",typeof(HExe_311709998))]
public class HExe_311709998: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Data.Player.PlayerManager host = obj as ServerCommon.Data.Player.PlayerManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        CSCommon.Data.PlayerData pd = new CSCommon.Data.PlayerData();
        pkg.Read( pd);
        host.SaveRole(roleId,pd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1528706922,"RoleEnterPlanesSuccessed",typeof(HExe_1528706922))]
public class HExe_1528706922: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Data.Player.PlayerManager host = obj as ServerCommon.Data.Player.PlayerManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RoleEnterPlanesSuccessed(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(189218286,"UpdataPlayerGuildData",typeof(HExe_189218286))]
public class HExe_189218286: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Data.Player.PlayerManager host = obj as ServerCommon.Data.Player.PlayerManager;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.Guid GuildId;
        pkg.Read(out GuildId);
        System.String GuildName;
        pkg.Read(out GuildName);
        System.UInt32 GuildContribution;
        pkg.Read(out GuildContribution);
        host.UpdataPlayerGuildData(roleId,GuildId,GuildName,GuildContribution,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2955744724,"RegGateServer",typeof(HExe_2955744724))]
public class HExe_2955744724: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.String ip;
        pkg.Read(out ip);
        System.UInt16 port;
        pkg.Read(out port);
        System.UInt64 id;
        pkg.Read(out id);
        host.RegGateServer(ip,port,id,connect);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2486208040,"RegPlanesServer",typeof(HExe_2486208040))]
public class HExe_2486208040: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.String ip;
        pkg.Read(out ip);
        System.UInt16 port;
        pkg.Read(out port);
        System.UInt64 id;
        pkg.Read(out id);
        return host.RegPlanesServer(ip,port,id,connect);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(403376892,"TryRegAccount",typeof(HExe_403376892))]
public class HExe_403376892: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 0; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt16 cltLinker;
        pkg.Read(out cltLinker);
        System.String usr;
        pkg.Read(out usr);
        System.String psw;
        pkg.Read(out psw);
        System.String mobileNum;
        pkg.Read(out mobileNum);
        return host.TryRegAccount(cltLinker,usr,psw,mobileNum,connect);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1352123346,"TryRegGuest",typeof(HExe_1352123346))]
public class HExe_1352123346: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 0; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt16 cltLinker;
        pkg.Read(out cltLinker);
        return host.TryRegGuest(cltLinker,connect);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3844801550,"QueryAllActivePlanesInfo",typeof(HExe_3844801550))]
public class HExe_3844801550: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt16 lnk;
        pkg.Read(out lnk);
        return host.QueryAllActivePlanesInfo(lnk,connect);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3244051206,"RPC_RandRoleName",typeof(HExe_3244051206))]
public class HExe_3244051206: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.Byte sex;
        pkg.Read(out sex);
        host.RPC_RandRoleName(sex,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4210366257,"TryCreatePlayer",typeof(HExe_4210366257))]
public class HExe_4210366257: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt16 lnk;
        pkg.Read(out lnk);
        System.UInt64 accountId;
        pkg.Read(out accountId);
        System.String planeName;
        pkg.Read(out planeName);
        System.String plyName;
        pkg.Read(out plyName);
        System.Byte pro;
        pkg.Read(out pro);
        System.Byte sex;
        pkg.Read(out sex);
        host.TryCreatePlayer(lnk,accountId,planeName,plyName,pro,sex,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1948931381,"DeleteRole",typeof(HExe_1948931381))]
public class HExe_1948931381: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt16 planesId;
        pkg.Read(out planesId);
        System.String roleName;
        pkg.Read(out roleName);
        return host.DeleteRole(planesId,roleName);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2174697914,"UpdateItem",typeof(HExe_2174697914))]
public class HExe_2174697914: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        CSCommon.Data.ItemData item = new CSCommon.Data.ItemData();
        pkg.Read( item);
        return host.UpdateItem(roleId,item);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(719670561,"DelItem",typeof(HExe_719670561))]
public class HExe_719670561: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 itemId;
        pkg.Read(out itemId);
        System.SByte bDestroy;
        pkg.Read(out bDestroy);
        host.DelItem(roleId,itemId,bDestroy);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1990082952,"DelMail",typeof(HExe_1990082952))]
public class HExe_1990082952: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 mailId;
        pkg.Read(out mailId);
        host.DelMail(roleId,mailId);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2596602672,"GetAccountInfoData",typeof(HExe_2596602672))]
public class HExe_2596602672: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt64 accountID;
        pkg.Read(out accountID);
        return host.GetAccountInfoData(accountID);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1200153343,"GetRoleDetailByName",typeof(HExe_1200153343))]
public class HExe_1200153343: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt16 planesId;
        pkg.Read(out planesId);
        System.String roleName;
        pkg.Read(out roleName);
        host.GetRoleDetailByName(planesId,roleName,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1825685927,"GetRoleDetailData",typeof(HExe_1825685927))]
public class HExe_1825685927: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt64 accountID;
        pkg.Read(out accountID);
        return host.GetRoleDetailData(accountID);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(362635390,"GetPlayerGuidByName",typeof(HExe_362635390))]
public class HExe_362635390: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt16 planesId;
        pkg.Read(out planesId);
        System.String name;
        pkg.Read(out name);
        return host.GetPlayerGuidByName(planesId,name);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4234756755,"GotoMap",typeof(HExe_4234756755))]
public class HExe_4234756755: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt16 planesId;
        pkg.Read(out planesId);
        System.UInt16 mapSourceId;
        pkg.Read(out mapSourceId);
        SlimDX.Vector3 pos;
        pkg.Read(out pos);
        System.UInt16 cltHandle;
        pkg.Read(out cltHandle);
        host.GotoMap(roleId,planesId,mapSourceId,pos,cltHandle,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(260844906,"GetPlanesInfo",typeof(HExe_260844906))]
public class HExe_260844906: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt16 planesId;
        pkg.Read(out planesId);
        host.GetPlanesInfo(planesId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2072066771,"UpdatePlanesServerPlanesNumber",typeof(HExe_2072066771))]
public class HExe_2072066771: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.Int32 num;
        pkg.Read(out num);
        host.UpdatePlanesServerPlanesNumber(num,connect);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3897933873,"UpdatePlanesServerGlobalMapNumber",typeof(HExe_3897933873))]
public class HExe_3897933873: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.Int32 num;
        pkg.Read(out num);
        host.UpdatePlanesServerGlobalMapNumber(num,connect);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(476093070,"UpdatePlanesServerPlayerNumber",typeof(HExe_476093070))]
public class HExe_476093070: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.Int32 num;
        pkg.Read(out num);
        host.UpdatePlanesServerPlayerNumber(num,connect);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1737416267,"SYS_ReloadItemTemplate",typeof(HExe_1737416267))]
public class HExe_1737416267: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt16 item;
        pkg.Read(out item);
        host.SYS_ReloadItemTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2844492937,"SYS_ReloadTaskTemplate",typeof(HExe_2844492937))]
public class HExe_2844492937: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt16 item;
        pkg.Read(out item);
        host.SYS_ReloadTaskTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(93353386,"SYS_ReloadDropTemplate",typeof(HExe_93353386))]
public class HExe_93353386: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt16 item;
        pkg.Read(out item);
        host.SYS_ReloadDropTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(80915548,"SYS_ReloadSellerTemplate",typeof(HExe_80915548))]
public class HExe_80915548: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IDataServer host = obj as ServerCommon.IDataServer;
        if (host == null) return null;
        System.UInt16 item;
        pkg.Read(out item);
        host.SYS_ReloadSellerTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2275548944,"NewPlanesServerStarted",typeof(HExe_2275548944))]
public class HExe_2275548944: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        host.NewPlanesServerStarted();
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1682507160,"RPC_GuestLogin",typeof(HExe_1682507160))]
public class HExe_1682507160: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        host.RPC_GuestLogin(connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2767285982,"ClientTryLogin",typeof(HExe_2767285982))]
public class HExe_2767285982: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.String usr;
        pkg.Read(out usr);
        System.String psw;
        pkg.Read(out psw);
        System.UInt16 planesid;
        pkg.Read(out planesid);
        host.ClientTryLogin(usr,psw,planesid,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3787358996,"RoleTryEnterGame",typeof(HExe_3787358996))]
public class HExe_3787358996: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.RoleTryEnterGame(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2799907353,"ReturnToRoleSelect",typeof(HExe_2799907353))]
public class HExe_2799907353: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        host.ReturnToRoleSelect(connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4120668445,"PlayerEnterMapInPlanes",typeof(HExe_4120668445))]
public class HExe_4120668445: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt16 lnk;
        pkg.Read(out lnk);
        System.UInt16 indexInMap;
        pkg.Read(out indexInMap);
        System.UInt16 indexInServer;
        pkg.Read(out indexInServer);
        host.PlayerEnterMapInPlanes(roleId,lnk,indexInMap,indexInServer);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2626970278,"PlayerLeaveMapInPlanes",typeof(HExe_2626970278))]
public class HExe_2626970278: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.UInt16 lnk;
        pkg.Read(out lnk);
        host.PlayerLeaveMapInPlanes(lnk);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1907698985,"OtherPlane_EnterMap",typeof(HExe_1907698985))]
public class HExe_1907698985: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.UInt64 planesSeverId;
        pkg.Read(out planesSeverId);
        System.UInt16 cltHandle;
        pkg.Read(out cltHandle);
        CSCommon.Data.PlayerData pd = new CSCommon.Data.PlayerData();
        pkg.Read( pd);
        CSCommon.Data.PlanesData planesData = new CSCommon.Data.PlanesData();
        pkg.Read( planesData);
        System.UInt16 mapSourceId;
        pkg.Read(out mapSourceId);
        System.UInt64 mapInstanceId;
        pkg.Read(out mapInstanceId);
        SlimDX.Vector3 pos;
        pkg.Read(out pos);
        host.OtherPlane_EnterMap(planesSeverId,cltHandle,pd,planesData,mapSourceId,mapInstanceId,pos,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2768013429,"TryRegAccount",typeof(HExe_2768013429))]
public class HExe_2768013429: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.String usr;
        pkg.Read(out usr);
        System.String psw;
        pkg.Read(out psw);
        System.String mobileNum;
        pkg.Read(out mobileNum);
        host.TryRegAccount(usr,psw,mobileNum,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2915244535,"RPC_RandRoleName",typeof(HExe_2915244535))]
public class HExe_2915244535: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.Byte sex;
        pkg.Read(out sex);
        host.RPC_RandRoleName(sex,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3309354601,"TryCreatePlayer",typeof(HExe_3309354601))]
public class HExe_3309354601: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.String planesName;
        pkg.Read(out planesName);
        System.String playerName;
        pkg.Read(out playerName);
        System.Byte pro;
        pkg.Read(out pro);
        System.Byte sex;
        pkg.Read(out sex);
        host.TryCreatePlayer(planesName,playerName,pro,sex,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1832266355,"DeleteRole",typeof(HExe_1832266355))]
public class HExe_1832266355: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.UInt16 planesId;
        pkg.Read(out planesId);
        System.String roleName;
        pkg.Read(out roleName);
        return host.DeleteRole(planesId,roleName,connect);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3477596733,"SYS_ReloadItemTemplate",typeof(HExe_3477596733))]
public class HExe_3477596733: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.UInt16 item;
        pkg.Read(out item);
        host.SYS_ReloadItemTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1981872387,"SYS_ReloadTaskTemplate",typeof(HExe_1981872387))]
public class HExe_1981872387: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.UInt16 item;
        pkg.Read(out item);
        host.SYS_ReloadTaskTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3254671043,"SYS_ReloadDropTemplate",typeof(HExe_3254671043))]
public class HExe_3254671043: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.UInt16 item;
        pkg.Read(out item);
        host.SYS_ReloadDropTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2763839682,"SYS_ReloadSellerTemplate",typeof(HExe_2763839682))]
public class HExe_2763839682: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.UInt16 item;
        pkg.Read(out item);
        host.SYS_ReloadSellerTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4237333123,"DisconnectPlayer",typeof(HExe_4237333123))]
public class HExe_4237333123: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.UInt64 accountId;
        pkg.Read(out accountId);
        System.SByte serverType;
        pkg.Read(out serverType);
        return host.DisconnectPlayer(accountId,serverType);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4203455931,"DisconnectPlayerByConnectHandle",typeof(HExe_4203455931))]
public class HExe_4203455931: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IGateServer host = obj as ServerCommon.IGateServer;
        if (host == null) return null;
        System.UInt16 lnk;
        pkg.Read(out lnk);
        host.DisconnectPlayerByConnectHandle(lnk);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2800653044,"WriteDBLog",typeof(HExe_2800653044))]
public class HExe_2800653044: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.ILogServer host = obj as ServerCommon.ILogServer;
        if (host == null) return null;
        ServerFrame.DB.DBLogData data = new ServerFrame.DB.DBLogData();
        pkg.Read( data);
        host.WriteDBLog(data);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1784571847,"RegGateServer",typeof(HExe_1784571847))]
public class HExe_1784571847: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.String ip;
        pkg.Read(out ip);
        System.UInt16 port;
        pkg.Read(out port);
        System.UInt64 id;
        pkg.Read(out id);
        host.RegGateServer(ip,port,id,connect);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3751269449,"GetPlanesServerId",typeof(HExe_3751269449))]
public class HExe_3751269449: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        return host.GetPlanesServerId();
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1079763792,"ClientDisConnect",typeof(HExe_1079763792))]
public class HExe_1079763792: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt64 roleId;
        pkg.Read(out roleId);
        host.ClientDisConnect(roleId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2534136153,"RemoveInstanceMap",typeof(HExe_2534136153))]
public class HExe_2534136153: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt64 mapInstanceId;
        pkg.Read(out mapInstanceId);
        host.RemoveInstanceMap(mapInstanceId);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3459940684,"ReturnToRoleSelect",typeof(HExe_3459940684))]
public class HExe_3459940684: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt64 planesId;
        pkg.Read(out planesId);
        System.UInt64 roleId;
        pkg.Read(out roleId);
        System.UInt64 accountId;
        pkg.Read(out accountId);
        host.ReturnToRoleSelect(planesId,roleId,accountId,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1082721781,"SYS_ReloadItemTemplate",typeof(HExe_1082721781))]
public class HExe_1082721781: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt16 item;
        pkg.Read(out item);
        host.SYS_ReloadItemTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2313787927,"SYS_ReloadRoleTemplate",typeof(HExe_2313787927))]
public class HExe_2313787927: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt16 item;
        pkg.Read(out item);
        host.SYS_ReloadRoleTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3376175977,"SYS_ReloadTaskTemplate",typeof(HExe_3376175977))]
public class HExe_3376175977: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt16 item;
        pkg.Read(out item);
        host.SYS_ReloadTaskTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2378633290,"SYS_ReloadDropTemplate",typeof(HExe_2378633290))]
public class HExe_2378633290: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.Int32 item;
        pkg.Read(out item);
        host.SYS_ReloadDropTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1285457189,"SYS_ReloadSellerTemplate",typeof(HExe_1285457189))]
public class HExe_1285457189: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt16 item;
        pkg.Read(out item);
        host.SYS_ReloadSellerTemplate(item,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3995985805,"EnterMap",typeof(HExe_3995985805))]
public class HExe_3995985805: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        CSCommon.Data.PlayerData pd = new CSCommon.Data.PlayerData();
        pkg.Read( pd);
        CSCommon.Data.PlanesData planesData = new CSCommon.Data.PlanesData();
        pkg.Read( planesData);
        System.UInt16 mapSourceId;
        pkg.Read(out mapSourceId);
        System.UInt64 instanceId;
        pkg.Read(out instanceId);
        SlimDX.Vector3 pos;
        pkg.Read(out pos);
        System.UInt16 cltHandle;
        pkg.Read(out cltHandle);
        host.EnterMap(pd,planesData,mapSourceId,instanceId,pos,cltHandle,connect,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(767739261,"RPC_DSTalkMsg",typeof(HExe_767739261))]
public class HExe_767739261: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt64 planesId;
        pkg.Read(out planesId);
        System.String sender;
        pkg.Read(out sender);
        System.SByte channel;
        pkg.Read(out channel);
        System.UInt64 targetId;
        pkg.Read(out targetId);
        System.String msg;
        pkg.Read(out msg);
        RPC.DataReader hyperlink;
        pkg.Read(out hyperlink);
        host.RPC_DSTalkMsg(planesId,sender,channel,targetId,msg,hyperlink);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3750218020,"RPC_AddGiftCount",typeof(HExe_3750218020))]
public class HExe_3750218020: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt64 id;
        pkg.Read(out id);
        System.Int32 index;
        pkg.Read(out index);
        System.Int32 count;
        pkg.Read(out count);
        host.RPC_AddGiftCount(id,index,count);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1073943803,"RPC_SendPlayerMsg",typeof(HExe_1073943803))]
public class HExe_1073943803: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt64 id;
        pkg.Read(out id);
        CSCommon.Data.Message msg = new CSCommon.Data.Message();
        pkg.Read( msg);
        host.RPC_SendPlayerMsg(id,msg);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1962052750,"RPC_SendPlayerMail",typeof(HExe_1962052750))]
public class HExe_1962052750: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt64 id;
        pkg.Read(out id);
        host.RPC_SendPlayerMail(id);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1258362875,"RPC_SendPlayerTeamInfo",typeof(HExe_1258362875))]
public class HExe_1258362875: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt64 id;
        pkg.Read(out id);
        RPC.DataReader dr;
        pkg.Read(out dr);
        host.RPC_SendPlayerTeamInfo(id,dr);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4007196713,"RPC_SendToLeaveTeamPlayer",typeof(HExe_4007196713))]
public class HExe_4007196713: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IPlanesServer host = obj as ServerCommon.IPlanesServer;
        if (host == null) return null;
        System.UInt64 id;
        pkg.Read(out id);
        host.RPC_SendToLeaveTeamPlayer(id);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3346785807,"RegGateServer",typeof(HExe_3346785807))]
public class HExe_3346785807: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        System.String ipAddress;
        pkg.Read(out ipAddress);
        System.UInt64 id;
        pkg.Read(out id);
        return host.RegGateServer(ipAddress,id,connect);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1735719627,"GetGateServers",typeof(HExe_1735719627))]
public class HExe_1735719627: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        return host.GetGateServers();
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2348150991,"RegPlanesServer",typeof(HExe_2348150991))]
public class HExe_2348150991: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        System.UInt64 id;
        pkg.Read(out id);
        return host.RegPlanesServer(id,connect);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2180561876,"RegDataServer",typeof(HExe_2180561876))]
public class HExe_2180561876: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        System.String ListenIp;
        pkg.Read(out ListenIp);
        System.UInt16 ListenPort;
        pkg.Read(out ListenPort);
        System.UInt64 id;
        pkg.Read(out id);
        return host.RegDataServer(ListenIp,ListenPort,id,connect);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2688647704,"RegPathFindServer",typeof(HExe_2688647704))]
public class HExe_2688647704: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        System.String ListenIp;
        pkg.Read(out ListenIp);
        System.UInt64 id;
        pkg.Read(out id);
        return host.RegPathFindServer(ListenIp,id,connect);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1218250649,"RegComServer",typeof(HExe_1218250649))]
public class HExe_1218250649: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        System.String ListenIp;
        pkg.Read(out ListenIp);
        System.UInt64 id;
        pkg.Read(out id);
        return host.RegComServer(ListenIp,id,connect);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1621165804,"RegLogServer",typeof(HExe_1621165804))]
public class HExe_1621165804: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        System.String ListenIp;
        pkg.Read(out ListenIp);
        System.UInt16 ListenPort;
        pkg.Read(out ListenPort);
        System.UInt64 id;
        pkg.Read(out id);
        return host.RegLogServer(ListenIp,ListenPort,id,connect);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4014122605,"SetGateLinkNumber",typeof(HExe_4014122605))]
public class HExe_4014122605: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        System.Int32 num;
        pkg.Read(out num);
        host.SetGateLinkNumber(connect,num);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(4072318702,"RPC_QueryAllActivePlanesInfo",typeof(HExe_4072318702))]
public class HExe_4072318702: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        host.RPC_QueryAllActivePlanesInfo(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(545341920,"GetLowGateServer",typeof(HExe_545341920))]
public class HExe_545341920: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 100; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        return host.GetLowGateServer(connect);
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1211040613,"GetDataServer",typeof(HExe_1211040613))]
public class HExe_1211040613: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        return host.GetDataServer();
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3254270965,"GetComServer",typeof(HExe_3254270965))]
public class HExe_3254270965: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        return host.GetComServer();
    }
}
[RPC.RPCMethordExecuterTypeAttribute(3183704535,"GetLogServer",typeof(HExe_3183704535))]
public class HExe_3183704535: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        return host.GetLogServer();
    }
}
[RPC.RPCMethordExecuterTypeAttribute(470079979,"GetPlanesServers",typeof(HExe_470079979))]
public class HExe_470079979: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 400; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.IRegisterServer host = obj as ServerCommon.IRegisterServer;
        if (host == null) return null;
        return host.GetPlanesServers();
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1756780771,"RPC_CreateItem2Bag",typeof(HExe_1756780771))]
public class HExe_1756780771: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 300; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.GameMaster host = obj as ServerCommon.Planes.GameMaster;
        if (host == null) return null;
        System.Int32 itemId;
        pkg.Read(out itemId);
        host.RPC_CreateItem2Bag(itemId,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(2313438677,"RPC_AddExp",typeof(HExe_2313438677))]
public class HExe_2313438677: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 300; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.GameMaster host = obj as ServerCommon.Planes.GameMaster;
        if (host == null) return null;
        System.Int32 exp;
        pkg.Read(out exp);
        host.RPC_AddExp(exp,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1658667155,"RPC_AddMoney",typeof(HExe_1658667155))]
public class HExe_1658667155: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 300; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.GameMaster host = obj as ServerCommon.Planes.GameMaster;
        if (host == null) return null;
        System.Byte type;
        pkg.Read(out type);
        System.Int32 num;
        pkg.Read(out num);
        host.RPC_AddMoney(type,num,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(1071207192,"RPC_Revive",typeof(HExe_1071207192))]
public class HExe_1071207192: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 300; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.GameMaster host = obj as ServerCommon.Planes.GameMaster;
        if (host == null) return null;
        host.RPC_Revive(fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(716149553,"RPC_JumpToMap",typeof(HExe_716149553))]
public class HExe_716149553: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 300; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.GameMaster host = obj as ServerCommon.Planes.GameMaster;
        if (host == null) return null;
        System.UInt16 mapid;
        pkg.Read(out mapid);
        System.Single x;
        pkg.Read(out x);
        System.Single z;
        pkg.Read(out z);
        host.RPC_JumpToMap(mapid,x,z,fwd);
return null;
    }
}
[RPC.RPCMethordExecuterTypeAttribute(600417167,"RPC_UpdateTaskById",typeof(HExe_600417167))]
public class HExe_600417167: RPC.RPCMethodExecuter
{
    public override int LimitLevel
    {
        get { return 300; }
    }
    public override object Execute(Iocp.NetConnection connect,RPC.RPCObject obj,RPC.PackageProxy pkg,RPC.RPCForwardInfo fwd)
    {
        ServerCommon.Planes.GameMaster host = obj as ServerCommon.Planes.GameMaster;
        if (host == null) return null;
        System.Int32 taskId;
        pkg.Read(out taskId);
        host.RPC_UpdateTaskById(taskId,fwd);
return null;
    }
}
public class MappingHashCode2Index{
public static void BuildMapping(){
    RPC.RPCNetworkMgr.AddExecuterIndxer(1070481883 , 0);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1739064142 , 1);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2456023713 , 2);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4154793834 , 3);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2737966181 , 4);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2640677823 , 5);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1717183888 , 6);
    RPC.RPCNetworkMgr.AddExecuterIndxer(280160435 , 7);
    RPC.RPCNetworkMgr.AddExecuterIndxer(795329423 , 8);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2676902958 , 9);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2810629639 , 10);
    RPC.RPCNetworkMgr.AddExecuterIndxer(46938956 , 11);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3636771780 , 12);
    RPC.RPCNetworkMgr.AddExecuterIndxer(627528374 , 13);
    RPC.RPCNetworkMgr.AddExecuterIndxer(803423025 , 14);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2647298097 , 15);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2306423160 , 16);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2359050701 , 17);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3027256072 , 18);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3601780197 , 19);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2245150845 , 20);
    RPC.RPCNetworkMgr.AddExecuterIndxer(565921859 , 21);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3349253733 , 22);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2086613904 , 23);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3202384357 , 24);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3591420715 , 25);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4216555195 , 26);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1531056815 , 27);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1784734408 , 28);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3303687663 , 29);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1693923301 , 30);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3541540232 , 31);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2247577661 , 32);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1045747085 , 33);
    RPC.RPCNetworkMgr.AddExecuterIndxer(402790377 , 34);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2843346904 , 35);
    RPC.RPCNetworkMgr.AddExecuterIndxer(889497932 , 36);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3586105770 , 37);
    RPC.RPCNetworkMgr.AddExecuterIndxer(319076046 , 38);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1312967023 , 39);
    RPC.RPCNetworkMgr.AddExecuterIndxer(743345890 , 40);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3731998305 , 41);
    RPC.RPCNetworkMgr.AddExecuterIndxer(236811980 , 42);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2973573372 , 43);
    RPC.RPCNetworkMgr.AddExecuterIndxer(861751381 , 44);
    RPC.RPCNetworkMgr.AddExecuterIndxer(548803112 , 45);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1209257994 , 46);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1977196125 , 47);
    RPC.RPCNetworkMgr.AddExecuterIndxer(854750530 , 0);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3095799854 , 1);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3047086403 , 2);
    RPC.RPCNetworkMgr.AddExecuterIndxer(602123432 , 3);
    RPC.RPCNetworkMgr.AddExecuterIndxer(614746597 , 4);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2146033032 , 5);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4010485661 , 6);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1990248090 , 7);
    RPC.RPCNetworkMgr.AddExecuterIndxer(209426158 , 8);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4111965867 , 9);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2387594059 , 10);
    RPC.RPCNetworkMgr.AddExecuterIndxer(981318454 , 11);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2936138087 , 12);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1905163712 , 13);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3270236743 , 14);
    RPC.RPCNetworkMgr.AddExecuterIndxer(639174217 , 15);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3918607182 , 16);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3106456801 , 17);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2876258328 , 18);
    RPC.RPCNetworkMgr.AddExecuterIndxer(18149753 , 19);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3920813871 , 20);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2489700813 , 21);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1399030718 , 22);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3105973124 , 23);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4100902645 , 24);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4050085165 , 25);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1809380907 , 26);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1284634388 , 27);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2205559883 , 28);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3563699092 , 29);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4216301954 , 30);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1872967421 , 31);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4270034289 , 32);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2494350067 , 33);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2575541438 , 34);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1211475058 , 35);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4092167642 , 36);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1733762118 , 37);
    RPC.RPCNetworkMgr.AddExecuterIndxer(435609608 , 38);
    RPC.RPCNetworkMgr.AddExecuterIndxer(110493767 , 39);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2203248439 , 40);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2990328239 , 41);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3526749364 , 42);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2337522896 , 43);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3985894704 , 44);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4228769425 , 45);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2699925958 , 46);
    RPC.RPCNetworkMgr.AddExecuterIndxer(397951486 , 47);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1737887591 , 48);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1470110452 , 49);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3325750559 , 50);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1909472311 , 51);
    RPC.RPCNetworkMgr.AddExecuterIndxer(689178625 , 52);
    RPC.RPCNetworkMgr.AddExecuterIndxer(858680085 , 53);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3281807773 , 54);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1562578405 , 55);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3460975487 , 56);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4137106746 , 57);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1672586707 , 58);
    RPC.RPCNetworkMgr.AddExecuterIndxer(272436567 , 59);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2388876356 , 60);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2148466191 , 61);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3510396002 , 62);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3402823320 , 63);
    RPC.RPCNetworkMgr.AddExecuterIndxer(405232310 , 64);
    RPC.RPCNetworkMgr.AddExecuterIndxer(248097711 , 65);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2007571158 , 66);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4089844448 , 67);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3676617206 , 68);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1069892221 , 69);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1048097490 , 70);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3634436165 , 71);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3308281991 , 72);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3162720426 , 73);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4225678388 , 74);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1496608088 , 75);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1745093561 , 76);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2144528820 , 77);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2065666321 , 78);
    RPC.RPCNetworkMgr.AddExecuterIndxer(919698329 , 79);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1654219612 , 80);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2383081553 , 81);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3384109364 , 82);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4112323569 , 83);
    RPC.RPCNetworkMgr.AddExecuterIndxer(794399681 , 84);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2592128971 , 85);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4237676595 , 86);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2463694110 , 87);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2400247181 , 88);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2712795904 , 89);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3114715086 , 90);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3526576787 , 91);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1388439319 , 92);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3529032970 , 93);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3850687818 , 94);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4168519198 , 0);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2160127548 , 1);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2168762344 , 2);
    RPC.RPCNetworkMgr.AddExecuterIndxer(434900464 , 3);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3124269720 , 4);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4190075605 , 5);
    RPC.RPCNetworkMgr.AddExecuterIndxer(311709998 , 6);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1528706922 , 7);
    RPC.RPCNetworkMgr.AddExecuterIndxer(189218286 , 8);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2955744724 , 0);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2486208040 , 1);
    RPC.RPCNetworkMgr.AddExecuterIndxer(403376892 , 2);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1352123346 , 3);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3844801550 , 4);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3244051206 , 5);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4210366257 , 6);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1948931381 , 7);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2174697914 , 8);
    RPC.RPCNetworkMgr.AddExecuterIndxer(719670561 , 9);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1990082952 , 10);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2596602672 , 11);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1200153343 , 12);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1825685927 , 13);
    RPC.RPCNetworkMgr.AddExecuterIndxer(362635390 , 14);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4234756755 , 15);
    RPC.RPCNetworkMgr.AddExecuterIndxer(260844906 , 16);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2072066771 , 17);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3897933873 , 18);
    RPC.RPCNetworkMgr.AddExecuterIndxer(476093070 , 19);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1737416267 , 20);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2844492937 , 21);
    RPC.RPCNetworkMgr.AddExecuterIndxer(93353386 , 22);
    RPC.RPCNetworkMgr.AddExecuterIndxer(80915548 , 23);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2275548944 , 0);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1682507160 , 1);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2767285982 , 2);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3787358996 , 3);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2799907353 , 4);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4120668445 , 5);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2626970278 , 6);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1907698985 , 7);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2768013429 , 8);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2915244535 , 9);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3309354601 , 10);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1832266355 , 11);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3477596733 , 12);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1981872387 , 13);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3254671043 , 14);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2763839682 , 15);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4237333123 , 16);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4203455931 , 17);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2800653044 , 0);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1784571847 , 0);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3751269449 , 1);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1079763792 , 2);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2534136153 , 3);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3459940684 , 4);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1082721781 , 5);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2313787927 , 6);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3376175977 , 7);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2378633290 , 8);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1285457189 , 9);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3995985805 , 10);
    RPC.RPCNetworkMgr.AddExecuterIndxer(767739261 , 11);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3750218020 , 12);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1073943803 , 13);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1962052750 , 14);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1258362875 , 15);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4007196713 , 16);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3346785807 , 0);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1735719627 , 1);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2348150991 , 2);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2180561876 , 3);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2688647704 , 4);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1218250649 , 5);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1621165804 , 6);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4014122605 , 7);
    RPC.RPCNetworkMgr.AddExecuterIndxer(4072318702 , 8);
    RPC.RPCNetworkMgr.AddExecuterIndxer(545341920 , 9);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1211040613 , 10);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3254270965 , 11);
    RPC.RPCNetworkMgr.AddExecuterIndxer(3183704535 , 12);
    RPC.RPCNetworkMgr.AddExecuterIndxer(470079979 , 13);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1756780771 , 0);
    RPC.RPCNetworkMgr.AddExecuterIndxer(2313438677 , 1);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1658667155 , 2);
    RPC.RPCNetworkMgr.AddExecuterIndxer(1071207192 , 3);
    RPC.RPCNetworkMgr.AddExecuterIndxer(716149553 , 4);
    RPC.RPCNetworkMgr.AddExecuterIndxer(600417167 , 5);
}
}

}
