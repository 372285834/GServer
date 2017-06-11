using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon.Planes
{
    [RPC.RPCClassAttribute(typeof(GameMaster))]
    public class GameMaster : RPC.RPCObject
    {
        #region RPC必须的基础定义
        public static RPC.RPCClassInfo smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
        {
            return smRpcClassInfo;
        }
        #endregion        

        //public IPlanesServer mPlanesSever;

        //#region RPC method
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        public void RPC_CreateItem2Bag(int itemId, RPC.RPCForwardInfo fwd)
        {
            var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
            if (player == null)
                return;
            var item = Item.DangerousCreateItemById(player, itemId, 1);
            player.Bag.AutoAddItem(item);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        public void RPC_AddExp(int exp, RPC.RPCForwardInfo fwd)
        {
            var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
            if (player == null)
                return;
            player.GainExp(exp);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        public void RPC_AddMoney(byte type, int num, RPC.RPCForwardInfo fwd)
        {
            var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
            if (player == null)
                return;
            player._ChangeMoney((CSCommon.eCurrenceType)type, CSCommon.Data.eMoneyChangeType.GM, num);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        public void RPC_Revive(RPC.RPCForwardInfo fwd)
        {
            var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
            if (player == null)
                return;
            player.Relive(CSCommon.eReliveMode.Current);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        public void RPC_JumpToMap(ushort mapid, float x, float z,RPC.RPCForwardInfo fwd)
        {
            var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
            if (player == null)
                return;
            player.JumpToMap(mapid, x, 0, z, fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        public void RPC_UpdateTaskById(int taskId, RPC.RPCForwardInfo fwd)
        {
            var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
            if (player == null)
                return;
            player.mCurTask.UpdateTask(taskId);
            player.AcceptTask();
        }

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void SetChiefRoleLevel(Int16 level, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    //PlayerInstance.SetLevel(player,level);
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void CreateNpc(CSCommon.Data.NPCData nd , RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;
        //    NPCInstance npc = NPCInstance.CreateNPCInstance(nd,player.HostMap);
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void ModifyNPC(CSCommon.Data.NPCData nd, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    var npc = player.HostMap.GetNPC(nd.RoleId);
        //    if (npc != null)
        //        npc.ReInitNPCInstance(nd, player.HostMap);
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void RemoveNPC(ulong npcId, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    var npc = player.HostMap.GetNPC(npcId);
        //    if(npc !=null)
        //        npc.OnLeaveMap();
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void CreateTrigger(CSCommon.Data.Trigger.TriggerData td, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    var trigger = TriggerInstance.CreateTriggerInstance(td, player.HostMap);
        //}
        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void ModifyTrigger(CSCommon.Data.Trigger.TriggerData td, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    var trigger = player.HostMap.GetTrigger(td.RoleId);
        //    if (trigger != null)
        //        trigger.ReInitTriggerInstance(td, player.HostMap);
        //}
        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void RemoveTrigger(ulong triggerId, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    var trigger = player.HostMap.GetTrigger(triggerId);
        //    trigger.OnLeaveMap();
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void RefreshFSMTemplate(ulong id, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    CSCommon.AISystem.FSMTemplateVersionManager.Instance.Load(CSCommon.Helper.enCSType.Server);
        //    //CSCommon.AISystem.FStateMachineTemplateManager.Instance.GetFSMTemplate(id, CSCommon.AISystem.Attribute.enCSType.Server, true);
        //    IPlanesServer.Instance.RefreshFSMTemplate(id);
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void RefreshEventCallBack(ulong id, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    CSCommon.Helper.EventCallBackVersionManager.Instance.Load(CSCommon.Helper.enCSType.Server);
        //    IPlanesServer.Instance.RefreshEventCallBack(id);
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void RefreshActionNotify(string name, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    //CSCommon.Animation.ActionNodeManager.Instance.GetActionSource(name, true);
        //    IPlanesServer.Instance.RefreshActionNotify(name);
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void SetTargetPosition(UInt32 singleId , SlimDX.Vector3 pos , RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    var target = player.GetTarget(singleId);
        //    if (target == null)
        //        return;

        //    target.Placement.SetLocation(ref pos);
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void SetTaskFinishedState(UInt32 roleId, UInt16 taskId, sbyte state, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    var target = player.HostMap.FindPlayer(roleId);
        //    if (target == null)
        //        return;

        //    target.TaskManager.SetTaskFinishedState(taskId, state != 0 ? true : false);
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void DiscardTask(UInt32 roleId, UInt16 taskId, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    var target = player.HostMap.FindPlayer(roleId);
        //    if (target == null)
        //        return;

        //    target.TaskManager.DiscardTask(taskId);
        //}
       

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.GM, false)]
        //public void CreatePlanes(byte level, string name, byte state, RPC.RPCForwardInfo fwd)
        //{
        //    var pkg = new RPC.PackageWriter();
        //    H_RPCRoot.smInstance.HGet_DataServer(pkg).CreatePlanes(pkg,level, name, state);
        //    pkg.DoCommand(IPlanesServer.Instance.DataConnect, RPC.CommandTargetType.DefaultType);
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        //public void GiveMeSomeExp(UInt32 exp, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    player.GainExp((int)exp);
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        //public void GiveMeSomeMoney(UInt32 money, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    player.GainMoney((int)money);
        //}

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        //public void GiveMeSomeExistentialPower(UInt32 power, RPC.RPCForwardInfo fwd)
        //{
        //    var player = IPlanesServer.Instance.GetPlayerByForwordInfo(fwd);
        //    if (player == null)
        //        return;

        //    player.GainExistentialPower((int)power);
        //}

        //#endregion
    }
}
