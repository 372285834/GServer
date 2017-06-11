using ServerFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class ItemCoolDownTime
    {
        public int Id;
        public System.DateTime EndTime;
        public float Total;
    }

    public class CdManager
    {
        static CdManager mInstance = new CdManager();
        public static CdManager Instance
        {
            get { return mInstance; }
        }

        public Dictionary<int, ItemCoolDownTime> mItemCdList = new Dictionary<int, ItemCoolDownTime>();

        public void AddItemCd(int id, float total, DateTime endTime)
        {
            ItemCoolDownTime cd = new ItemCoolDownTime();
            cd.Id = id;
            cd.Total = total;
            cd.EndTime = endTime;
            mItemCdList[id] = cd;
            TimerManager.doOnce(total, OnCDTimerEnd, id);
        }
        public void OnCDTimerEnd(TimerEvent timerEvent)
        {
            mItemCdList.Remove((int)timerEvent.param);
        }
        public bool GetItemCd(int id)
        {
            if (mItemCdList.ContainsKey(id))
            {
                return true;
            }
            return false;
        }
    }

    public class ItemBag : BagBase
    {
        
        public ItemBag()
        {
            InventoryType = CSCommon.eItemInventory.ItemBag; 
        }

        public DateTime UseItemSucceed(int id)
        {
            var tpl = CSTable.ItemUtil.GetItem(id);
            DateTime endTime = DateTime.MinValue;
            if (tpl.ItemType == (int)CSCommon.eItemType.Consumable)
            {
                var temp = tpl as CSTable.ItemTplData;
                float delay = temp.CdDelay;
                if (delay <= 0)
                {
                    return endTime;
                }
                endTime = Time.AddSecondNow(delay);
                var list = GetCdGroup(id);
                if (list != null)
                {
                    foreach (var i in list)
                    {
                        CdManager.Instance.AddItemCd(i, delay, endTime);
                    }
                }
                else
                {
                    CdManager.Instance.AddItemCd(id, delay, endTime);
                }
            }
            return endTime;
        }

        public List<int> GetCdGroup(int id)
        {
            foreach (var i in CSCommon.ItemCommon.Instance.CdList)
            {
                if (i.list.Contains(id))
                {
                    return i.list;
                }
            }
            return null;
        }
    }

    public class BagBase
    {
        CSCommon.eItemInventory mInventoryType = CSCommon.eItemInventory.ItemBag;
        public CSCommon.eItemInventory InventoryType
        {
            get { return mInventoryType; }
            set { mInventoryType = value; }
        }

        UInt16 mBagSize = 0;
        public UInt16 BagSize
        {
            get { return mBagSize; }
        }

        public Item[] mItems;

        public IEnumerable<Item> IterItems()
        {
            foreach (var item in mItems)
            {
                if (item == null)
                {
                    continue;
                }
                yield return item;
            }
        }

        protected virtual void OnItemsChanged()
        {

        }

        protected virtual void OnPutItem(UInt16 index,Item item)
        {
            
        }

        public Item this[UInt16 index]
        {
            get
            {
                if (index >= BagSize)
                    return null;
                return mItems[index];
            }
            set
            {
                if (index >= BagSize)
                    return;
                mItems[index] = value;
                if (value != null)
                {
                    value.Position = index;
                    value.Inventory = this;
                    value.ItemData.OwnerId = mHostRole.Id;
                }
            }
        }

        protected RoleActor mHostRole;

        public void InitBag(RoleActor role, UInt16 size, List<CSCommon.Data.ItemData> items)
        {
            mHostRole = role;
            mBagSize = size;
            mItems = new Item[mBagSize];
            foreach (var i in items)
            {
                var item = Item.DangerousCreateItem(role, i, false);
                UInt16 index = item.Position;
                this[index] = item;
            }
        }

        public void ExpandBagSize(UInt16 nCount)
        {
            var player = mHostRole as PlayerInstance;
            if (player == null)
                return;
            var saveItems = mItems;
            mItems = new Item[saveItems.Length + nCount];
            for (int i = 0; i < saveItems.Length; i++)
            {
                mItems[i] = saveItems[i];
            }
            player.PlayerData.RoleDetail.BagSize = (ushort)mItems.Length;
        }

        public void TakeOffFashionByType(int type)
        {
            var player = mHostRole as PlayerInstance;
            foreach (var i in mItems)
            {
                if (i == null)
                {
                    continue;
                }
                var tType = (i.ItemTemplate as CSTable.ItemFashionData).FashionType;
                if (i.ItemData.WearState == (byte)CSCommon.eBoolState.True && tType == type)
                {
                    i.ItemData.WearState = (byte)CSCommon.eBoolState.False;
                    RPC.PackageWriter pkg = new RPC.PackageWriter();
                    Wuxia.H_RpcRoot.smInstance.RPC_TakeOffFashion(pkg, i.ItemData.ItemId);
                    pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
                }
            }
        }     

        public virtual UInt16 FindEmptyPosition()
        {
            for (UInt16 i = 0; i < BagSize; i++)
            {
                if (mItems[i] == null)
                    return i;
            }
            return UInt16.MaxValue;
        }

        public bool AddItem2Position(UInt16 pos, Item item)
        {
            if (mItems[pos] != null)
            {
                return false;
            }
            else
            {
                this[pos] = item;

                var player = mHostRole as PlayerInstance;
                if (player != null)
                {
                    RPC.PackageWriter pkg = new RPC.PackageWriter();
                    pkg.SetSinglePkg();
                    Wuxia.H_RpcRoot.smInstance.RPC_ItemAdd2Bag(pkg, item.ItemData, (byte)InventoryType, pos);
                    pkg.DoCommandPlanes2Client(player.Planes2GateConnect,player.ClientLinkId);
                }
            }
            OnItemsChanged();
            return true;
        }

        public List<CSCommon.Data.ItemData> GetBagSaver()
        {
            List<CSCommon.Data.ItemData> resultItems = new List<CSCommon.Data.ItemData>();
            if (mItems == null)
                return resultItems;
            for (UInt16 i = 0; i < mItems.Length; i++)
            {
                if (mItems[i] != null)
                {
                    resultItems.Add(mItems[i].ItemData);
                }
            }
            return resultItems;
        }

        public bool AutoAddItem(Item item)
        {
            //先找能堆叠的
            for (UInt16 i = 0; i < mBagSize; i++)
            {
                if (mItems[i] != null&&mItems[i].ItemTemplate==item.ItemTemplate)
                {
                    if(mItems[i].StackNum>=item.MaxStackNum)
                    {
                        continue;
                    }
                    int canPut = item.MaxStackNum - mItems[i].StackNum;
                    if (item.StackNum >= canPut)
                    {
                        item.StackNum -= canPut;
                        mItems[i].StackNum = item.MaxStackNum;
                    }
                    else
                    {
                        mItems[i].StackNum += item.StackNum;
                        item.StackNum = 0;
                    }

                    ItemStackNumChangedToClient(mItems[i].ItemData.ItemId, (byte)InventoryType, mItems[i].StackNum);

                    if (item.StackNum <= 0)
                    {
                        item.DestroyFromDB(mHostRole);
                        return true;
                    }
                }
            }

            for (UInt16 i = 0; i < mBagSize; i++)
            {
                if (mItems[i] == null)
                {
                    this[i] = item;

                    var player = mHostRole as PlayerInstance;
                    if (player != null)
                    {
                        RPC.PackageWriter pkg = new RPC.PackageWriter();
                        pkg.SetSinglePkg();
                        Wuxia.H_RpcRoot.smInstance.RPC_ItemAdd2Bag(pkg, item.ItemData, (byte)InventoryType, i);
                        pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
                    }
                    OnItemsChanged();
                    return true;
                }
            }
            return false;
        }

        public Item FindItemByPos(ushort pos)
        {
            if (pos >= mItems.Length)
            {
                return null;
            }
            return mItems[pos];
        }

        public Item FindItemById(ulong itemId)
        {
            foreach (var i in mItems)
            {
                if (null == i)
                    continue;

                if (i.ItemData.ItemId == itemId)
                    return i;
            }
            return null;
        }

        public Item FindItemById(int itemId)
        {
            foreach (var i in mItems)
            {
                if (null == i)
                    continue;

                if (itemId == i.ItemTemplate.id)
                {
                    return i;
                }
            }
            return null;
        }

        public int GetItemEmptyCount(int id)
        {
            var template = CSTable.StaticDataManager.ItemTpl[id];
            if (template == null)
                return 0;
            if (null == template)
                return 0;

            int itemEmptyCount = 0;

            foreach (var i in mItems)
            {
                if (null == i)
                    itemEmptyCount += template.ItemMaxStackNum;
                else
                {
                    if (id == i.ItemTemplate.id)
                        itemEmptyCount += template.ItemMaxStackNum - i.StackNum;
                }      
            }

            return itemEmptyCount;
        }
        
        public UInt16 GetEmptyCount()
        {
            UInt16 count = 0;
            foreach (var i in mItems)
            {
                if (i == null)
                    count++;
            }
            return count;
        }

        public UInt16 GetUsePosCount()
        {
            UInt16 count = 0;
            foreach (var i in mItems)
            {
                if (i != null)
                    count++;
            }
            return count;
        }

        public void RemoveItem(Item item)
        {
            for (UInt16 i = 0; i < BagSize; i++)
            {
                Item curItem = mItems[i];
                if (curItem != null && curItem.ItemData.ItemId == item.ItemData.ItemId)
                {
                    ItemRmoveToClient(item.ItemData.ItemId, (byte)mInventoryType);
                    this[i] = null;
                }
            }
        }

        public Item DeleteItem(ulong itemId)
        {
            for (UInt16 i = 0; i < BagSize; i++)
            {
                Item curItem = mItems[i];
                if (curItem != null && curItem.ItemData.ItemId == itemId)
                {
                    ItemRmoveToClient(itemId, (byte)mInventoryType);
                    curItem.DestroyFromDB(mHostRole);
                    this[i] = null;
                    return curItem;
                }
            }
            return null;
        }

        public int GetItemCount(int itemId)
        {
            int count = 0;
            foreach (var i in mItems)
            {
                if (i == null)
                    continue;
                if (i.ItemData.Template.id == itemId)
                {
                    count += i.StackNum;
                }
            }
            return count;
        }

        public bool RemoveItemCountByTid(int itemTid, int count)
        {
            foreach (var i in mItems)
            {
                if (i == null)
                    continue;
                if (count <= 0)
                {
                    return true;
                }
                if (i.ItemData.Template.id == itemTid)
                {
                    if(count >= i.StackNum)
                    {
                        DeleteItem(i.ItemData.ItemId);
                        count -= i.StackNum;
                    }
                    else
                    {
                        i.StackNum -= count;
                        ItemStackNumChangedToClient(i.ItemData.ItemId, (byte)InventoryType, i.StackNum);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool RemoveItemCountById(ulong itemId, int count)
        {
            foreach (var i in mItems)
            {
                if (i == null)
                    continue;
                if (i.ItemData.ItemId == itemId)
                {
                    if (count >= i.StackNum)
                    {
                        DeleteItem(i.ItemData.ItemId);
                        count -= i.StackNum;
                    }
                    else
                    {
                        i.StackNum -= count;
                        ItemStackNumChangedToClient(i.ItemData.ItemId, (byte)InventoryType, i.StackNum);
                        return true;
                    }
                }
            }
            return false;
        }

        public void Merge()
        {
            var player = mHostRole as PlayerInstance;
            if (player == null)
                return;

            List<UInt16> needItems = new List<UInt16>();
            for (UInt16 i = 0; i < mItems.Length; i++)
            {
                if (mItems[i] == null)
                    continue;
                needItems.Add(i);
            }

            for (int i = needItems.Count - 1; i > 0; i--)
            {
                var item = mItems[needItems[i]];
                if (item == null)
                    continue;

                for (int j = 0; j < i; j++)
                {
                    var targetItem = mItems[needItems[j]];
                    if (targetItem == null)
                        continue;

                    if (targetItem.ItemData.ItemTemlateId == item.ItemData.ItemTemlateId)
                    {
                        var emptySize = targetItem.MaxStackNum - targetItem.StackNum;
                        if (emptySize <= 0)
                            continue;

                        if (item.StackNum <= emptySize)
                        {
                            targetItem.StackNum += item.StackNum;
                            ItemStackNumChangedToClient(targetItem.ItemData.ItemId, (byte)mInventoryType, targetItem.StackNum);
                            item.StackNum = 0;
                            ItemRmoveToClient(item.ItemData.ItemId, (byte)mInventoryType);
                            item.DestroyFromDB(player);
                            this[needItems[i]] = null;
                        }
                        else
                        {
                            targetItem.StackNum = targetItem.MaxStackNum;
                            ItemStackNumChangedToClient(targetItem.ItemData.ItemId, (byte)mInventoryType, targetItem.StackNum);
                            item.StackNum -= (Int16)emptySize;
                            ItemStackNumChangedToClient(item.ItemData.ItemId, (byte)mInventoryType, item.StackNum);
                        }
                    }
                }
            }
        }

        public sbyte UseItem(int itemId, int count)
        {
            var item = FindItemById(itemId);
            if (item == null)
                return (sbyte)CSCommon.eRet_UseItem.ItemNull;
            if (item.StackNum < count)
                return (sbyte)CSCommon.eRet_UseItem.NoCount;
            if (item.ItemTemplate.CanManualUse == (byte)0)
                return (sbyte)CSCommon.eRet_UseItem.NotUse;
            var player = mHostRole as PlayerInstance;
            if (player == null)
                return (sbyte)CSCommon.eRet_UseItem.NoRole;
            if (CdManager.Instance.GetItemCd(itemId))
            {
                return (sbyte)CSCommon.eRet_UseItem.CdIng;
            }
            return item.Use(count, player);
        }


        public void ItemStackNumChangedToClient(ulong id, byte inventoryType, int stackNum)
        {
            var player = mHostRole as PlayerInstance;
            if (player != null)
            {
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                Wuxia.H_RpcRoot.smInstance.RPC_ItemStackNumChanged(pkg, id, inventoryType, stackNum);
                pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
            }
        }

        private void ItemRmoveToClient(ulong id, byte inventoryType)
        {
            var player = mHostRole as PlayerInstance;
            if (player != null)
            {
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                Wuxia.H_RpcRoot.smInstance.RPC_ItemRemove(pkg, id, inventoryType);
                pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
            }
        }

        // 整理背包..现不用
        //         public void ReOrganizeBag(List<Item> modifyItems, List<Item> deletedItems)
        //         {
        //             modifyItems.Clear();
        //             deletedItems.Clear();
        //             MergeItems(modifyItems, deletedItems);
        //             UInt16 firstEmpty = 0;
        //             UInt16 lastItem = (UInt16)((int)BagSize - 1);
        //             while (true)
        //             {
        //                 while (mItems[firstEmpty] != null && firstEmpty != lastItem)
        //                 {
        //                     firstEmpty++;
        //                 }
        // 
        //                 while (mItems[lastItem] == null && firstEmpty != lastItem)
        //                 {
        //                     lastItem--;
        //                 }
        // 
        //                 if (firstEmpty != lastItem)
        //                 {
        //                     mItems[firstEmpty] = mItems[lastItem];
        //                     mItems[lastItem] = null;
        //                 }
        //                 else
        //                 {
        //                     break;
        //                 }
        //             }
        //        }

        // 合并同模版对象
        //         public void MergeItems(List<Item> modifyItems, List<Item> deletedItems)
        //         {
        //             var player = mHostRole as PlayerInstance;
        //             if (player == null)
        //                 return;
        // 
        //             List<UInt16> needItems = new List<UInt16>();
        //             for (UInt16 i = 0; i < mItems.Length; i++)
        //             {
        //                 if (mItems[i] == null)
        //                     continue;
        // 
        //                 if ((mItems[i].MaxStackNum - mItems[i].StackNum) > 0)
        //                 {
        //                     needItems.Add(i);
        //                 }
        //             }
        // 
        //             for (int i = needItems.Count - 1; i > 0; i--)
        //             {
        //                 var item = mItems[needItems[i]];
        //                 if (item == null)
        //                     continue;
        // 
        //                 for (int j = 0; j < i; j++)
        //                 {
        //                     var targetItem = mItems[needItems[j]];
        //                     if (targetItem == null)
        //                         continue;
        // 
        //                     if (targetItem.ItemData.ItemTemlateId == item.ItemData.ItemTemlateId)
        //                     {
        //                         var emptySize = targetItem.MaxStackNum - targetItem.StackNum;
        //                         if (emptySize <= 0)
        //                             continue;
        // 
        //                         if (item.StackNum <= emptySize)
        //                         {
        //                             targetItem.StackNum += item.StackNum;
        //                             if(!modifyItems.Contains(targetItem))
        //                                 modifyItems.Add(targetItem);
        //                             item.StackNum = 0;
        //                             item.DestroyFromDB(player);
        //                             this[needItems[i]] = null;
        //                             if(!deletedItems.Contains(item))
        //                                 deletedItems.Add(item);
        //                         }
        //                         else
        //                         {
        //                             targetItem.StackNum = targetItem.MaxStackNum;
        //                             if(!modifyItems.Contains(targetItem))
        //                                 modifyItems.Add(targetItem);
        // 
        //                             item.StackNum -= (Int16)emptySize;
        //                             if(!modifyItems.Contains(item))
        //                                 modifyItems.Add(item);
        //                         }
        //                     }
        //                 }
        //             }
        //         }
    }
}
