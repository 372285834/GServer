//Server Caller
namespace Wuxia{
public class H_Barrack
{
    public static Wuxia.H_Barrack smInstance = new Wuxia.H_Barrack();
}
}
namespace Wuxia{
public class H_ChiefRole
{
    public static Wuxia.H_ChiefRole smInstance = new Wuxia.H_ChiefRole();
    public void RPC_SyncPostion(RPC.PackageWriter pkg,System.Single x,System.Single z)
    {
        pkg.Write(x);
        pkg.Write(z);
        pkg.SetMethod(0);
    }
    public void RPC_SyncDirection(RPC.PackageWriter pkg,System.Single dir)
    {
        pkg.Write(dir);
        pkg.SetMethod(1);
    }
    public void RPC_UpdateRoleValue(RPC.PackageWriter pkg,System.String name,RPC.DataWriter value)
    {
        pkg.Write(name);
        pkg.Write(value);
        pkg.SetMethod(2);
    }
    public void RPC_UpdateRoleAttr(RPC.PackageWriter pkg,RPC.DataWriter _pkg)
    {
        pkg.Write(_pkg);
        pkg.SetMethod(3);
    }
    public void RPC_FlutterInfo(RPC.PackageWriter pkg,System.Byte type,System.Int32 param1,System.Int32 param2,System.Int32 value)
    {
        pkg.Write(type);
        pkg.Write(param1);
        pkg.Write(param2);
        pkg.Write(value);
        pkg.SetMethod(4);
    }
    public void RPC_SpellSkill(RPC.PackageWriter pkg,System.Int32 skillid,System.UInt64 targetId)
    {
        pkg.Write(skillid);
        pkg.Write(targetId);
        pkg.SetMethod(5);
    }
    public void RPC_ChangeState(RPC.PackageWriter pkg,System.Byte state,System.Single time)
    {
        pkg.Write(state);
        pkg.Write(time);
        pkg.SetMethod(6);
    }
    public void RPC_Killed(RPC.PackageWriter pkg,System.Int32 skillId,System.Int32 level,System.UInt64 targetId)
    {
        pkg.Write(skillId);
        pkg.Write(level);
        pkg.Write(targetId);
        pkg.SetMethod(7);
    }
    public void RPC_Relive(RPC.PackageWriter pkg,System.Single x,System.Single z)
    {
        pkg.Write(x);
        pkg.Write(z);
        pkg.SetMethod(8);
    }
    public void RPC_NpcReborn(RPC.PackageWriter pkg,System.Single x,System.Single y,System.Single z)
    {
        pkg.Write(x);
        pkg.Write(y);
        pkg.Write(z);
        pkg.SetMethod(9);
    }
    public void RPC_AddBuff(RPC.PackageWriter pkg,System.Int32 buffId,System.Int32 level,System.Int32 time)
    {
        pkg.Write(buffId);
        pkg.Write(level);
        pkg.Write(time);
        pkg.SetMethod(10);
    }
    public void RPC_RemoveBuff(RPC.PackageWriter pkg,System.Int32 buffId,System.Int32 level)
    {
        pkg.Write(buffId);
        pkg.Write(level);
        pkg.SetMethod(11);
    }
    public void RPC_UpdateBuff(RPC.PackageWriter pkg,System.Int32 buffId,System.Int32 level,System.Int32 time)
    {
        pkg.Write(buffId);
        pkg.Write(level);
        pkg.Write(time);
        pkg.SetMethod(12);
    }
}
}
namespace Wuxia{
public class H_ChiefRole_Barrack
{
    public static Wuxia.H_ChiefRole_Barrack smInstance = new Wuxia.H_ChiefRole_Barrack();
}
}
namespace Wuxia{
public class H_RoleActor
{
    public static Wuxia.H_RoleActor smInstance = new Wuxia.H_RoleActor();
    public void RPC_SyncPostion(RPC.PackageWriter pkg,System.Single x,System.Single z)
    {
        pkg.Write(x);
        pkg.Write(z);
        pkg.SetMethod(0);
    }
    public void RPC_SyncDirection(RPC.PackageWriter pkg,System.Single dir)
    {
        pkg.Write(dir);
        pkg.SetMethod(1);
    }
    public void RPC_UpdateRoleValue(RPC.PackageWriter pkg,System.String name,RPC.DataWriter value)
    {
        pkg.Write(name);
        pkg.Write(value);
        pkg.SetMethod(2);
    }
    public void RPC_UpdateRoleAttr(RPC.PackageWriter pkg,RPC.DataWriter _pkg)
    {
        pkg.Write(_pkg);
        pkg.SetMethod(3);
    }
    public void RPC_FlutterInfo(RPC.PackageWriter pkg,System.Byte type,System.Int32 param1,System.Int32 param2,System.Int32 value)
    {
        pkg.Write(type);
        pkg.Write(param1);
        pkg.Write(param2);
        pkg.Write(value);
        pkg.SetMethod(4);
    }
    public void RPC_SpellSkill(RPC.PackageWriter pkg,System.Int32 skillid,System.UInt64 targetId)
    {
        pkg.Write(skillid);
        pkg.Write(targetId);
        pkg.SetMethod(5);
    }
    public void RPC_ChangeState(RPC.PackageWriter pkg,System.Byte state,System.Single time)
    {
        pkg.Write(state);
        pkg.Write(time);
        pkg.SetMethod(6);
    }
    public void RPC_Killed(RPC.PackageWriter pkg,System.Int32 skillId,System.Int32 level,System.UInt64 targetId)
    {
        pkg.Write(skillId);
        pkg.Write(level);
        pkg.Write(targetId);
        pkg.SetMethod(7);
    }
    public void RPC_Relive(RPC.PackageWriter pkg,System.Single x,System.Single z)
    {
        pkg.Write(x);
        pkg.Write(z);
        pkg.SetMethod(8);
    }
    public void RPC_NpcReborn(RPC.PackageWriter pkg,System.Single x,System.Single y,System.Single z)
    {
        pkg.Write(x);
        pkg.Write(y);
        pkg.Write(z);
        pkg.SetMethod(9);
    }
    public void RPC_AddBuff(RPC.PackageWriter pkg,System.Int32 buffId,System.Int32 level,System.Int32 time)
    {
        pkg.Write(buffId);
        pkg.Write(level);
        pkg.Write(time);
        pkg.SetMethod(10);
    }
    public void RPC_RemoveBuff(RPC.PackageWriter pkg,System.Int32 buffId,System.Int32 level)
    {
        pkg.Write(buffId);
        pkg.Write(level);
        pkg.SetMethod(11);
    }
    public void RPC_UpdateBuff(RPC.PackageWriter pkg,System.Int32 buffId,System.Int32 level,System.Int32 time)
    {
        pkg.Write(buffId);
        pkg.Write(level);
        pkg.Write(time);
        pkg.SetMethod(12);
    }
}
}
namespace Wuxia{
public class H_RpcRoot
{
    public static Wuxia.H_RpcRoot smInstance = new Wuxia.H_RpcRoot();
    public Wuxia.H_RoleActor HIndex(RPC.PackageWriter pkg,System.UInt64 i)
    {
        pkg.PushStack(11+0);
        pkg.Write(i);
        return Wuxia.H_RoleActor.smInstance;
    }
    public Wuxia.H_ChiefRole HGet_mChiefRole(RPC.PackageWriter pkg)
    {
        pkg.PushStack(0);
        return Wuxia.H_ChiefRole.smInstance;
    }
    public void RPC_RoleLeave(RPC.PackageWriter pkg,System.UInt64 uuid)
    {
        pkg.Write(uuid);
        pkg.SetMethod(0);
    }
    public void RPC_UpdateRoleValue(RPC.PackageWriter pkg,System.String name,RPC.DataWriter value)
    {
        pkg.Write(name);
        pkg.Write(value);
        pkg.SetMethod(1);
    }
    public void RPC_CreateSummon(RPC.PackageWriter pkg,System.UInt64 ownerId,System.UInt64 targetid,System.Int32 skillid)
    {
        pkg.Write(ownerId);
        pkg.Write(targetid);
        pkg.Write(skillid);
        pkg.SetMethod(2);
    }
    public void RPC_ItemStackNumChanged(RPC.PackageWriter pkg,System.UInt64 itemId,System.Byte bag,System.Int32 num)
    {
        pkg.Write(itemId);
        pkg.Write(bag);
        pkg.Write(num);
        pkg.SetMethod(3);
    }
    public void RPC_ItemAdd2Bag(RPC.PackageWriter pkg,CSCommon.Data.ItemData data,System.Byte bag,System.UInt16 pos)
    {
        pkg.Write(data);
        pkg.Write(bag);
        pkg.Write(pos);
        pkg.SetMethod(4);
    }
    public void RPC_ItemRemove(RPC.PackageWriter pkg,System.UInt64 itemId,System.Byte bag)
    {
        pkg.Write(itemId);
        pkg.Write(bag);
        pkg.SetMethod(5);
    }
    public void RPC_AddGiftCount(RPC.PackageWriter pkg,System.Int32 index,System.Int32 count)
    {
        pkg.Write(index);
        pkg.Write(count);
        pkg.SetMethod(6);
    }
    public void RPC_AddTalkMsg(RPC.PackageWriter pkg,System.SByte channel,System.String sender,System.String msg,RPC.DataWriter hyperlink)
    {
        pkg.Write(channel);
        pkg.Write(sender);
        pkg.Write(msg);
        pkg.Write(hyperlink);
        pkg.SetMethod(7);
    }
    public void RPC_AcceptTask(RPC.PackageWriter pkg,CSCommon.Data.TaskData data)
    {
        pkg.Write(data);
        pkg.SetMethod(8);
    }
    public void RPC_UpdateTaskState(RPC.PackageWriter pkg,System.Byte state)
    {
        pkg.Write(state);
        pkg.SetMethod(9);
    }
    public void RPC_UpdateTaskProcess(RPC.PackageWriter pkg,System.Int32 process)
    {
        pkg.Write(process);
        pkg.SetMethod(10);
    }
    public void RPC_AddMail(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(11);
    }
    public void RPC_MartialOpen(RPC.PackageWriter pkg,System.Byte type)
    {
        pkg.Write(type);
        pkg.SetMethod(12);
    }
    public void RPC_ReceiveTeam(RPC.PackageWriter pkg,RPC.DataWriter dr)
    {
        pkg.Write(dr);
        pkg.SetMethod(13);
    }
    public void RPC_ReceiveLeaveTeam(RPC.PackageWriter pkg)
    {
        pkg.SetMethod(14);
    }
    public void RPC_ReceiveMsg(RPC.PackageWriter pkg,CSCommon.Data.Message msg)
    {
        pkg.Write(msg);
        pkg.SetMethod(15);
    }
    public void RPC_TakeOffFashion(RPC.PackageWriter pkg,System.UInt64 itemId)
    {
        pkg.Write(itemId);
        pkg.SetMethod(16);
    }
    public void RPC_OnJumpMapOver(RPC.PackageWriter pkg,System.Int32 mapid,System.Single posx,System.Single posz)
    {
        pkg.Write(mapid);
        pkg.Write(posx);
        pkg.Write(posz);
        pkg.SetMethod(17);
    }
    public void RPC_OnCopyEnd(RPC.PackageWriter pkg,System.Byte result)
    {
        pkg.Write(result);
        pkg.SetMethod(18);
    }
    public void RPC_OnCopyCountDown(RPC.PackageWriter pkg,System.Single second)
    {
        pkg.Write(second);
        pkg.SetMethod(19);
    }
}
}
