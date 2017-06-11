using System;
using System.Collections.Generic;

namespace ServerCommon.Planes
{
    public partial class PlayerInstance : RPC.RPCObject
    {
        public Task mCurTask;//主线任务
        public void InitTask(CSCommon.Data.TaskData data)
        {
            mCurTask = new Task(this, data);
            mCurTask.InitLogic();
        }

        public void OnChangeTaskState()
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.RPC_UpdateTaskState(pkg, (byte)mCurTask.TaskData.State);
            pkg.DoCommandPlanes2Client(this.Planes2GateConnect, this.ClientLinkId);
        }

        public void OnChangeTaskProcess()
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.RPC_UpdateTaskProcess(pkg, mCurTask.TaskData.Process);
            pkg.DoCommandPlanes2Client(this.Planes2GateConnect, this.ClientLinkId);
        }


        public void AcceptTask()
        {
            //调用客户端接受任务了
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            pkg.SetSinglePkg();
            Wuxia.H_RpcRoot.smInstance.RPC_AcceptTask(pkg, this.mCurTask.TaskData);
            pkg.DoCommandPlanes2Client(this.Planes2GateConnect, this.ClientLinkId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetRewardTask(int npcId, int templateId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var tpl = CSTable.StaticDataManager.TaskTpl[templateId];
            if (null == tpl) return;

            if (PlayerData.RoleDetail.MapSourceId != tpl.FinishMapId)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (npcId != tpl.FinishNpcId)
            {
                pkg.Write((sbyte)-2);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }

            if (mCurTask.TaskState != CSCommon.eTaskState.Finished)
            {
                pkg.Write((sbyte)-3);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }

            GetReward(tpl.CurrencyReward, tpl.ItemsReward);
            mCurTask.SetTaskState(CSCommon.eTaskState.Rewarded);
            if (mCurTask.UpdateNextTask())
            {
                AcceptTask();
            }
            pkg.Write((sbyte)1);
            pkg.DoReturnPlanes2Client(fwd);
        }

    }
}