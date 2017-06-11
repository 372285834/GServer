using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Data
{
    public class TaverTaskInstance
    {
        CSCommon.Data.Task.TavernTaskData mTaskData;
        public CSCommon.Data.Task.TavernTaskData TaskData
        {
            get { return mTaskData; }
        }

        public void InitTask(CSCommon.Data.Task.TavernTaskData data)
        {
            mTaskData = data;
        }

        public bool IsConditionOK(CSCommon.Data.PlayerDataEx player)
        {
            /*foreach (var i in player.TaskMgr.AcceptTavernTasks)
            {
                if (i.Finished == true)
                    return true;
            }*/
            return false;
        }
    }
    public class TavernTaskManager
    {
        Dictionary<ulong, TaverTaskInstance> mTasks = new Dictionary<ulong, TaverTaskInstance>();
        Dictionary<ulong, ulong> mAllRewardItems = new Dictionary<ulong, ulong>();
        public TaverTaskInstance GetTavernTask(ulong taskId)
        {
            TaverTaskInstance taskInstance;
            if (mTasks.TryGetValue(taskId, out taskInstance))
                return taskInstance;
            return null;
        }

        bool mIsInited = false;
        public bool IsInited
        {
            get { return mIsInited; }
        }

        public bool IsInitedTaverTaskManager()
        {
            if (mIsInited == false)
            {
                //请求Data得到TaverTasks
                return false;
            }
            else
            {
                return true;
            }
        }

        //public bool InitTavernTaskManager(List<CSCommon.Data.Task.TavernTaskData> tasks, List<CSCommon.Data.ItemData> items)
        //{
        //    if (mIsInited == true)
        //        return false;
            
        //    mTasks.Clear();
        //    foreach (var i in tasks)
        //    {
        //        var taskInstance = new TaverTaskInstance();
        //        taskInstance.InitTask(i);
        //        mTasks.Add(i.TaskId, taskInstance);
        //    }
        //    mAllRewardItems.Clear();
        //    foreach (var i in items)
        //    {
        //        if (i.Inventory != (byte)CSCommon.eItemInventory.TavernTaskReward)
        //        {
        //            System.Diagnostics.Debugger.Break();
        //        }
                
        //        var item = Inventory.Item.DangerousCreateItem(i);
        //        mAllRewardItems.Add(i.ItemId,item);
        //    }
        //    return true;
        //}

        public CSCommon.Data.Task.TavernTaskData IssueTask(ulong planesId, ulong playerId, string planesName, string killPlayer, List<ulong> itemRewards, int payRMB, int payMoney)
        {
            var taskData = new CSCommon.Data.Task.TavernTaskData();
            taskData.TaskId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Task);
            taskData.PlanesId = planesId;
            taskData.IssuePlayerId = playerId;
            taskData.IssueTime = System.DateTime.Now;
            taskData.PlanesName = planesName;
            taskData.KillPlayer = killPlayer;
            taskData.PayRMB = payRMB;
            taskData.PayMoney = payMoney;
            var task = new TaverTaskInstance();
            task.InitTask(taskData);
            //if (player.PlayerData.RoleDetail.RolePrivateRmb < (ulong)payMoney)
            //    return null;
            //player.PlayerData.RoleDetail.RolePrivateRmb -= (ulong)payMoney;
            
            //foreach (var i in itemRewards)
            //{
            //    var item = player.ItemBag.RemoveItem(i);
            //    if (item!=null)
            //    {
            //        mAllRewardItems[i] = item;
            //        item.ItemData.Inventory = (byte)CSCommon.eItemInventory.TavernTaskReward;
            //        taskData.ItemRewards += i.ToString() + ';';
            //    }
            //}
            mTasks[taskData.TaskId] = task;
            return taskData;
        }
        //public bool CancelTask(PlayerInstance player, Guid taskId)
        //{
        //    if (false == IsInitedTaverTaskManager())
        //    {
        //        return false;
        //    }

        //    TaverTaskInstance taskInstance;
        //    if (mTasks.TryGetValue(taskId, out taskInstance))
        //    {
        //        if (taskInstance.TaskData.IssuePlayerId != player.Id)
        //            return false;

        //        //这里应该先判断终止条件是否满足，比如背包空间够不够

        //        player.PlayerData.RoleDetail.RolePrivateRmb += (ulong)taskInstance.TaskData.PayMoney;

        //        var items = taskInstance.TaskData.ItemRewards.Split(';');
        //        foreach (var i in items)
        //        {
        //            var itemId = Guid.Parse(i);

        //            Inventory.Item item;
        //            if (mAllRewardItems.TryGetValue(itemId, out item))
        //            {
        //                mAllRewardItems.Remove(itemId);
        //                player.ItemBag.AddItemAuto(item, player);//这里会自动设置CSCommon.eItemInventory.ItemBag
        //            }
        //        }

        //        mTasks.Remove(taskId);
        //        return true;
        //    }
        //    return false;
        //}

        //public bool AcceptTask(ulong playerId, Guid taskId)
        //{
        //    if (false == IsInitedTaverTaskManager())
        //    {
        //        return false;
        //    }

        //    var taskInstance = GetTavernTask(taskId);
        //    if (taskInstance == null)
        //    {
        //        return false;
        //    }

            

        //    if (taskInstance.TaskData.PlanesId != player.PlanesInstance.PlanesId)
        //    {
        //        //只有本位面的人才能接本位面的酒馆任务
        //        return false;
        //    }

        //    var taskState = new TaverTaskInstanceState();
        //    taskState.TaskInstance = taskInstance;
        //    taskState.Finished = false;

        //    player.TaskMgr.AcceptTavernTasks.Add(taskState);

        //    return true;
        //}
        //public bool DiscardTask(PlayerInstance player, Guid taskId)
        //{
        //    if (false == IsInitedTaverTaskManager())
        //    {
        //        return false;
        //    }

        //    player.TaskMgr.RemoveTavernTask(taskId);
        //    return true;
        //}
        //public bool CommitTask(PlayerInstance player,Guid taskId)
        //{
        //    if (false == IsInitedTaverTaskManager())
        //    {
        //        return false;
        //    }

        //    var taskInstance = GetTavernTask(taskId);
        //    if (taskInstance == null)
        //    {
        //        //任务已经过期
        //        player.TaskMgr.RemoveTavernTask(taskInstance);
        //        return false;
        //    }

        //    if (false == taskInstance.IsConditionOK(player))
        //        return false;

        //    //这里应该先判断终止条件是否满足，比如背包空间够不够
        //    player.PlayerData.RoleDetail.RolePrivateRmb += (ulong)taskInstance.TaskData.PayMoney;

        //    var items = taskInstance.TaskData.ItemRewards.Split(';');
        //    foreach (var i in items)
        //    {
        //        var itemId = Guid.Parse(i);

        //        Inventory.Item item;
        //        if (mAllRewardItems.TryGetValue(itemId, out item))
        //        {
        //            mAllRewardItems.Remove(itemId);
        //            player.ItemBag.AddItemAuto(item, player);//这里会自动设置CSCommon.eItemInventory.ItemBag
        //        }
        //    }

        //    mTasks.Remove(taskId);
            
        //    return true;
        //}
    }
}
