using CSCommon.Data;
using System;
using System.Collections.Generic;

namespace ServerCommon.Planes
{
    public class IdNum
    {
        public int templateId;
        public int stackNum;
    }
    
    public partial class PlayerInstance : RPC.RPCObject
    {
        System.Random mRandom = new System.Random();

        ItemBag mBag = new ItemBag();
        public ItemBag Bag
        {
            get { return mBag; }
        }

        EquipBag mEquipBag = new EquipBag();
        public EquipBag EquipBag
        {
            get { return mEquipBag; }
        }

        FashionBag mFashionBag = new FashionBag();
        public FashionBag FashionBag
        {
            get { return mFashionBag; }
        }

        EquipGemBag mEquipGemBag = new EquipGemBag();
        public EquipGemBag EquipGemBag
        {
            get { return mEquipGemBag; }
        }

        GemBag mGemBag = new GemBag();
        public GemBag GemBag
        {
            get { return mGemBag; }
        }

        BagBase mStore = new BagBase();
        public BagBase Store
        {
            get { return mStore; }
        }
        public BagBase _GetBagWithType(byte bag)
        {
            BagBase opBag = null;
            switch ((CSCommon.eItemInventory)bag)
            {
                case CSCommon.eItemInventory.ItemBag:
                    opBag = this.Bag;
                    break;
                case CSCommon.eItemInventory.StoreBag:
                    opBag = this.Store;
                    break;
                case CSCommon.eItemInventory.EquipBag:
                    opBag = this.EquipBag;
                    break;
                case CSCommon.eItemInventory.EquipGemBag:
                    opBag = this.EquipGemBag;
                    break;
                case CSCommon.eItemInventory.GemBag:
                    opBag = this.GemBag;
                    break;
            }
            return opBag;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetItemCdInfo(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            Dictionary<int, ItemCoolDownTime> ItemCdList = CdManager.Instance.mItemCdList;
            retPkg.Write(ItemCdList.Count);
            foreach (var t in ItemCdList.Values)
            {
                retPkg.Write(t.Id);
                retPkg.Write(t.EndTime);
                retPkg.Write(t.Total);
            }
            retPkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_UseItem(byte bag, int itemId, int count, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            BagBase opBag = _GetBagWithType(bag);
            if (opBag ==  null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_UseItem.BagNull);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }
            sbyte result = opBag.UseItem(itemId, count);
            opBag.Merge();
            retPkg.Write(result);

            if (opBag == this.Bag && result == (sbyte)CSCommon.eRet_UseItem.Succeed)
            {
                var endtime = this.Bag.UseItemSucceed(itemId);
                if (endtime == System.DateTime.MinValue)
                {
                    retPkg.Write((sbyte)-1);
                }
                else
                {
                    retPkg.Write((sbyte)1);
                    retPkg.Write(itemId);
                    retPkg.Write(endtime);
                }
            }
            retPkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_DestroyItem(byte bag, ulong itemid, RPC.RPCForwardInfo fwd)
        {
            return;
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            BagBase opBag = _GetBagWithType(bag);

            if (opBag != null)
                opBag.DeleteItem(itemid);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_SellItem(int id, int count, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var num = this.Bag.GetItemCount(id);
            if (num < count)
            {
                pkg.Write((sbyte)CSCommon.eRet_SellItem.LessCount);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var tpl = CSTable.ItemUtil.GetItem(id);
            if (tpl == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_SellItem.NoTpl);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (tpl.ItemIsSell == (byte)CSCommon.eBoolState.False)
            {
                pkg.Write((sbyte)CSCommon.eRet_SellItem.NoTpl);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var type = (CSCommon.eCurrenceType)tpl.SellCurrenceType;
            var price = tpl.SellingPrice * count;
            if (!_IsMoneyEnough(type, price))
            {
                pkg.Write((sbyte)CSCommon.eRet_SellItem.LessMoney);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (!this.Bag.RemoveItemCountByTid(id, count))
            {
                pkg.Write((sbyte)CSCommon.eRet_SellItem.DelItemError);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            _ChangeMoney(type, CSCommon.Data.eMoneyChangeType.SellItem, price);
            pkg.Write((sbyte)CSCommon.eRet_SellItem.Succeed);
            pkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_ReOrganizeBag(byte bag, RPC.RPCForwardInfo fwd)
        {
//             BagBase opBag = _GetBagWithType(bag);
//             if (opBag != null)
//             {
//                 List<ServerCommon.Planes.Item> modifyItems = new List<ServerCommon.Planes.Item>();
//                 List<ServerCommon.Planes.Item> deletedItems = new List<ServerCommon.Planes.Item>();
//                 opBag.ReOrganizeBag(modifyItems, deletedItems);
// 
//                 RPC.PackageWriter retPkg = new RPC.PackageWriter();
//                 retPkg.Write((sbyte)1);
// 
//                 retPkg.Write((UInt16)modifyItems.Count);
//                 foreach (var item in modifyItems)
//                 {
//                     retPkg.Write(item.ItemData.ItemId);
//                     retPkg.Write(item.StackNum);
//                 }
// 
//                 retPkg.Write((UInt16)deletedItems.Count);
//                 foreach (var item in deletedItems)
//                 {
//                     retPkg.Write(item.ItemData.ItemId);
//                 }
// 
//                 UInt16 count = opBag.GetUsePosCount();
//                 retPkg.Write(count);
//                 for (UInt16 i = 0; i < count; i++)
//                 {
//                     retPkg.Write(opBag[i].ItemData.ItemId);
//                     retPkg.Write(i);
//                 }
//                 retPkg.DoReturnPlanes2Client(fwd);
//             }
//             else
//             {
//                 RPC.PackageWriter retPkg = new RPC.PackageWriter();
//                 retPkg.Write((sbyte)-2);
//                 retPkg.DoReturnPlanes2Client(fwd);
//             }
        }

        public void RPC_ExpandBagSize(UInt16 nCount)
        {
            this.Bag.ExpandBagSize(nCount);
        }

        public List<CSCommon.ShopItemInfo> _GetShopItems(CSCommon.eShopType type)
        {
            var shopcn = CSCommon.ShopCommon.Instance;
            if (shopcn == null)
            {
                return null;
            }
            List<CSCommon.ShopItemInfo> list = null;
            switch ((CSCommon.eShopType)type)
            {
                case CSCommon.eShopType.HotItem:
                    list = shopcn.ItemList;
                    break;
                case CSCommon.eShopType.Drug:
                    list = shopcn.DrugList;
                    break;
                case CSCommon.eShopType.Mount:
                    list = shopcn.MountList;
                    break;
                case CSCommon.eShopType.Rmb:
                    list = shopcn.RmbList;
                    break;
            }
            return list;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_OpenShop(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            pkg.Write((byte)CSCommon.eShopType.MaxType);
            for (byte type = (byte)CSCommon.eShopType.HotItem; type < (byte)CSCommon.eShopType.MaxType; type++)
            {
                var list = _GetShopItems((CSCommon.eShopType)type);
                pkg.Write(type);
                byte count = 0;
                if (list != null)
                {
                    count = (byte)list.Count;
                }
                pkg.Write(count);
                foreach (var i in list)
                {
                    pkg.Write(i.Id);
                    pkg.Write((byte)i.Currencey);
                    pkg.Write(i.Price);
                    pkg.Write(i.Discount);
                    int ishot = i.IsHot == true ? 1 : 0;
                    pkg.Write((byte)ishot);
                    pkg.Write(i.RmbIcon);
                }
            }
            pkg.DoReturnPlanes2Client(fwd);
            return;
        }
        public CSCommon.ShopItemInfo _GetShopItemInfo(CSCommon.eShopType type, int id)
        {
            var list = _GetShopItems((CSCommon.eShopType)type);
            if (list == null)
            {
                return null;
            }
            foreach(var i in list)
            {
                if (i.Id == id)
                {
                    return i;
                }
            }
            return null;
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_BuyShopItem(byte type, int id, int count, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var shopitem = _GetShopItemInfo((CSCommon.eShopType)type, id);
            if (shopitem == null)
            {
                retPkg.Write((sbyte)CSCommon.eRet_BuyShopItem.NotInShop);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }

            if (type == (byte)CSCommon.eShopType.Rmb)
            {
                if (_BuyRmb(shopitem.Price))
                {
                    _ChangeMoney(CSCommon.eCurrenceType.Rmb, CSCommon.Data.eMoneyChangeType.BuyRmb, shopitem.Id);
                }
            }
            else
            {
                int templateId = shopitem.Id;
                var template = CSTable.ItemUtil.GetItem(templateId);
                if (template == null)
                {
                    retPkg.Write((sbyte)CSCommon.eRet_BuyShopItem.NoTemplate);
                    retPkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                if (count > template.ItemMaxStackNum)
                {
                    retPkg.Write((sbyte)CSCommon.eRet_BuyShopItem.UpNum);
                    retPkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                int price = (int)(shopitem.Price * count * shopitem.Discount);
                if (_IsMoneyEnough(shopitem.Currencey, price) == false)
                {
                    retPkg.Write((sbyte)CSCommon.eRet_BuyShopItem.LessMoney);
                    retPkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                if (this.Bag.GetEmptyCount() <= 0)
                {
                    retPkg.Write((sbyte)CSCommon.eRet_BuyShopItem.BagNoPos);
                    retPkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                _ChangeMoney(shopitem.Currencey, CSCommon.Data.eMoneyChangeType.BuyShopItem, -price);
                CreateItemToBag(templateId, count);
            }
            retPkg.Write((sbyte)CSCommon.eRet_BuyShopItem.Succeed);
            retPkg.DoReturnPlanes2Client(fwd);
        }

        public bool _BuyRmb(int price)
        {
            //充值
            return true;
        }

        public bool CreateItemToBag(int templateId, int count)
        {
            if (templateId <= 0)
            {
                return false;
            }
            CSTable.IItemBase template = CSTable.ItemUtil.GetItem(templateId);
            if (template == null)
            {
                Log.Log.Item.Print("CreateItemToBag: template is null");
                return false;
            }
            BagBase bag = this.Bag;
            if (template.ItemType == (int)CSCommon.eItemType.Gem)
            {
                bag = this.GemBag;
            }
            else if (template.ItemType == (int)CSCommon.eItemType.Fashion)
            {
                bag = this.FashionBag;
            }

            while (count > 0)
            {
                if (template.ItemMaxStackNum >= count)
                {
                    var item = Item.DangerousCreateItemById(this, templateId, count);
                    bag.AutoAddItem(item);
                    count = 0;
                }
                else
                {
                    var item = Item.DangerousCreateItemById(this, templateId, count);
                    bag.AutoAddItem(item);
                    count -= template.ItemMaxStackNum;
                }
            }
            return true;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_CombineItem(int itemId, int itemNum, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            if (itemNum <= 0)
            {
                pkg.Write((sbyte)CSCommon.eRet_CombineItem.IdNumError);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (this.Bag.GetEmptyCount() <= 0)
            {
                pkg.Write((sbyte)CSCommon.eRet_CombineItem.BagNoPos);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var temp = CSTable.StaticDataManager.ItemCombine[itemId];
            if (temp == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_CombineItem.NoCombineTpl);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            int[] ids = temp.needIds;
            int[] nums = temp.needNums;
            if (ids.Length != nums.Length)
            {
                pkg.Write((sbyte)CSCommon.eRet_CombineItem.IdNumError);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            for (int i = 0; i < ids.Length; i++)
            {
                var count = this.Bag.GetItemCount(ids[i]);
                if (count < nums[i] * itemNum)
                {
                    pkg.Write((sbyte)CSCommon.eRet_CombineItem.LessMaterial);
                    pkg.DoReturnPlanes2Client(fwd);
                    return;
                }
            }
            CSCommon.eCurrenceType moneyType = (CSCommon.eCurrenceType)temp.costType;
            int cost = temp.costNum * itemNum;
            if (_IsMoneyEnough(moneyType, cost) == false)
            {
                pkg.Write((sbyte)CSCommon.eRet_CombineItem.LessMoney);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (CreateItemToBag(itemId, itemNum) == false)
            {
                pkg.Write((sbyte)CSCommon.eRet_CombineItem.CreateFailed);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            for (int i = 0; i < ids.Length; i++)
            {
                this.Bag.RemoveItemCountByTid(ids[i], nums[i] * itemNum);
            }
            _ChangeMoney(moneyType, CSCommon.Data.eMoneyChangeType.CombineItem, -cost);
            pkg.Write((sbyte)CSCommon.eRet_CombineItem.Succeed);
            pkg.DoReturnPlanes2Client(fwd);
        }

        #region 拍卖行,社交服务器
        //寄售物品(id,数量,总价)
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_ConsignItem(ulong itemId, int stack, int price, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            if (CSCommon.ItemCommon.Instance == null)
            {
                Log.Log.Item.Print("RPC_ConsignItem,CSCommon.ItemCommon.Instance == null");
                retPkg.Write((sbyte)CSCommon.eRet_ConsignItem.LessTemp);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var rent = CSCommon.ItemCommon.Instance.Rent;
            int needPayRent = stack * rent;
            if (_IsMoneyEnough(CSCommon.eCurrenceType.Gold, needPayRent) == false)
            {
                retPkg.Write((sbyte)CSCommon.eRet_ConsignItem.LessRent);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var item = this.Bag.FindItemById(itemId);
            if (item.StackNum < stack)
            {
                retPkg.Write((sbyte)CSCommon.eRet_ConsignItem.LessStack);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }

            //通知从comserver已寄售背包里面创建寄售的物品
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_ConsignItem(pkg, PlayerData.RoleDetail.RoleId, item.ItemData.ItemTemlateId, stack, price);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                sbyte result;
                _io.Read(out result);
                retPkg.Write(result);
                if (result == (sbyte)CSCommon.eRet_ConsignItem.Succeed)
                {
                    _ChangeMoney(CSCommon.eCurrenceType.Gold, CSCommon.Data.eMoneyChangeType.ConsignItem, -needPayRent);
                    this.Bag.RemoveItemCountById(itemId, stack);
                }
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_BuyConsignItem(ulong itemId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_GetConsignItem(pkg, PlayerData.RoleDetail.RoleId, itemId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result;
                _io.Read(out result);
                if ((CSCommon.eRet_BuyConsignItem)result == CSCommon.eRet_BuyConsignItem.Succeed)
                {
                    int price = 0;
                    _io.Read(out price);
                    if (_IsMoneyEnough(CSCommon.eCurrenceType.Rmb, price) == false)
                    {
                        retPkg.Write((sbyte)CSCommon.eRet_BuyConsignItem.LessMoney);
                        retPkg.DoReturnPlanes2Client(fwd);
                        return;
                    }
                    RPC.PackageWriter rpkg = new RPC.PackageWriter();
                    H_RPCRoot.smInstance.HGet_ComServer(rpkg).HGet_UserRoleManager(rpkg).RPC_BuyConsignItem(rpkg, PlayerData.RoleDetail.RoleId, itemId);
                    pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io2, bool isTimeOut2)
                    {
                        _io2.Read(out result);
                        if ((CSCommon.eRet_BuyConsignItem)result == CSCommon.eRet_BuyConsignItem.Succeed)
                        {
                            _ChangeMoney(CSCommon.eCurrenceType.Rmb, CSCommon.Data.eMoneyChangeType.BuyConsignItem, -price);
                        }
                        retPkg.Write(result);
                        retPkg.DoReturnPlanes2Client(fwd);
                        return;
                    };
                }
                else
                {
                    retPkg.Write(result);
                    retPkg.DoReturnPlanes2Client(fwd);
                    return;
                }
            };
        }

        //查看物品列表
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_GetRoleGird(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_GetRoleGird(pkg, PlayerData.RoleDetail.RoleId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.SetSinglePkg();
                sbyte result;
                _io.Read(out result);
                retPkg.Write(result);
                if (result == (sbyte)1)
                {
                    int count = 0;
                    _io.Read(out count);
                    retPkg.Write(count);
                    for (int i = 0; i < count; i++)
                    {
                        CSCommon.Data.ConsignGridData data = new CSCommon.Data.ConsignGridData();
                        _io.Read(data);
                        pkg.Write(data);
                    }
                }
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_GetRoleGirdByName(string name, byte findType, byte page, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_GetRoleGirdByName(pkg, PlayerData.RoleDetail.RoleId, name, findType, page);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.SetSinglePkg();
                sbyte result;
                _io.Read(out result);
                retPkg.Write(result);
                if (result == (sbyte)1)
                {
                    int count = 0;
                    _io.Read(out count);
                    retPkg.Write(count);
                    for (int i = 0; i < count; i++)
                    {
                        CSCommon.Data.ConsignGridData data = new CSCommon.Data.ConsignGridData();
                        _io.Read(data);
                        pkg.Write(data);
                    }
                }
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_GetRoleGirdByType(byte itemType, byte page, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_GetRoleGirdByType(pkg, PlayerData.RoleDetail.RoleId, itemType, page);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.SetSinglePkg();
                sbyte result;
                _io.Read(out result);
                retPkg.Write(result);
                if (result == (sbyte)1)
                {
                    int count = 0;
                    _io.Read(out count);
                    retPkg.Write(count);
                    for (int i = 0; i < count; i++)
                    {
                        CSCommon.Data.ConsignGridData data = new CSCommon.Data.ConsignGridData();
                        _io.Read(data);
                        pkg.Write(data);
                    }
                }
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }


        #endregion

        #region 装备
        
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_WearEquip(int itemId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var item = this.Bag.FindItemById(itemId);
            if (item == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_WearEquip.NoItem);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var temp = CSTable.ItemUtil.GetItem(item.ItemData.ItemTemlateId);
            if (temp == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_WearEquip.NoTpl);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (temp.ItemType != (int)CSCommon.eItemType.Equip)
            {
                pkg.Write((sbyte)CSCommon.eRet_WearEquip.NotEquip);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var etemp = temp as CSTable.ItemEquipData;
            UInt16 index = (UInt16)etemp.EquipType;
            var equip = this.EquipBag[index];
            if (equip != null)
            {
                this.EquipBag.RemoveItem(equip);
                this.Bag.AutoAddItem(equip);
            }
            this.EquipBag.WearEquip(index, item);
            this.Bag.RemoveItem(item);
            CalcChangeType(eValueType.Equip);
            pkg.Write((sbyte)CSCommon.eRet_WearEquip.Succeed);
            pkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_TakeOffEquip(ulong equipId, RPC.RPCForwardInfo fwd)
        {
            return;
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var equip = this.EquipBag.FindItemById(equipId);
            if (equip == null)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            this.EquipBag.RemoveItem(equip);
            this.Bag.AutoAddItem(equip);
            pkg.Write((sbyte)1);
            pkg.DoReturnPlanes2Client(fwd);
        }

        /// <summary>
        /// 养成，每次扣除所需材料几个，获得几点经验
        /// </summary>
        /// <param name="itemId"></param>
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_EquipIntensify(int itemId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var equip = this.EquipBag.FindItemById(itemId);
            if (equip == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_EquipIntensify.NoEquip);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var equiptpl = CSTable.ItemUtil.GetItem(itemId) as CSTable.ItemEquipData;
            if (equiptpl == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_EquipIntensify.NoEquipTpl);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }

            short maxLv = (short)equiptpl.MaxLevel;
            if (equip.ItemData.ItemLv >= maxLv)
            {
                pkg.Write((sbyte)CSCommon.eRet_EquipIntensify.MaxLv);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var etemp = CSTable.ItemUtil.GetEquipLvTpl(equiptpl.ItemProfession, equiptpl.EquipType, equip.ItemData.ItemLv);
            if (etemp == null || etemp.needExp <= 0)
            {
                pkg.Write((sbyte)CSCommon.eRet_EquipIntensify.NoEquipLvTpl);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }

            if (!_IsLevelEnough((ushort)etemp.playerlevel))
            {
                pkg.Write((sbyte)CSCommon.eRet_EquipIntensify.OverPlayerLv);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }

            int oreId = CSCommon.ItemCommon.Instance.OreId;
            int costCount = etemp.needcount;    //需求灵石数量
            if (Bag.GetItemCount(oreId) < costCount)
            {
                pkg.Write((sbyte)CSCommon.eRet_EquipIntensify.LessOre);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            int getExp = _GetMultiple(CSCommon.ItemCommon.Instance.EquipIntensifyRate);//获取经验,基础是1，有暴击
            this.Bag.RemoveItemCountByTid(oreId, costCount);
            equip.GetEquipExp(getExp);
            if (equip.ItemData.ItemLv >= maxLv)
            {
                equip.ItemData.ItemLv = maxLv;
                equip.ItemData.CurExp = 0;
            }
            pkg.Write((sbyte)CSCommon.eRet_EquipIntensify.Succeed);
            CalcChangeType(eValueType.Equip);
            pkg.Write(equip.ItemData.ItemLv);
            pkg.Write(equip.ItemData.CurExp);
            pkg.Write(equip.ItemData.ItemTemlateId);
            pkg.DoReturnPlanes2Client(fwd);
        }

        public int _GetMultiple(List<CSCommon.RateValue> Rate)
        {
            int multiple = 1;
            int totalnumvalue = 0;
            foreach (var i in Rate)
            {
                totalnumvalue += i.Rate;
            }
            int dice = CSCommon.Data.ItemData.Rand.Next(0, totalnumvalue);
            int sig = 0;
            for (int i = 0; i < Rate.Count; i++)
            {
                if (dice >= sig && dice < sig + Rate[i].Rate)
                {
                    multiple = Rate[i].Value;
                    break;
                }
                sig += Rate[i].Rate;
            }
            return multiple;
        }

        public int _GetIntensifyoreId()
        {
            var temps = CSTable.StaticDataManager.ItemOre.Dict;//矿石表
            foreach (var i in temps)
            {
                return i.Key;
            }
            return 0;
        }

        // <param name="count"></param> 精炼次数
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_EquipRefine(int itemId, int count, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var equip = this.EquipBag.FindItemById(itemId);
            if (equip == null)
            {
                Log.Log.Item.Info("RPC_EquipRefine equip id is error!");
                pkg.Write((sbyte)CSCommon.eRet_EquipRefine.NoEquip);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (equip.ItemTemplate == null)
            {
                Log.Log.Item.Info("RPC_EquipRefine equip ItemTemplate == null!");
                pkg.Write((sbyte)CSCommon.eRet_EquipRefine.NoEquipTpl);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var equiptpl = equip.ItemTemplate as CSTable.ItemEquipData;
            if (equiptpl == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_EquipRefine.NoEquipTpl);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (equip.ItemData.ItemRefineLv >= this.PlayerData.RoleDetail.RoleLevel)
            {
                pkg.Write((sbyte)CSCommon.eRet_EquipRefine.OverPlayerLv);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            short maxLv = (short)equiptpl.MaxRefineLv;
            if (equip.ItemData.ItemRefineLv >= maxLv)
            {
                pkg.Write((sbyte)CSCommon.eRet_EquipRefine.MaxLv);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var lvtpl = CSTable.StaticDataManager.ItemEquipRefine[equip.ItemData.ItemRefineLv];
            if (lvtpl == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_EquipRefine.NoRefineTpl);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            int owncount = this.Bag.GetItemCount(lvtpl.mid);
            if (count > owncount)
            {
                pkg.Write((sbyte)CSCommon.eRet_EquipRefine.BagNoMaterial);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            short lv = equip.ItemData.ItemRefineLv;
            int costcount = 0;
            while (costcount < count)
            {
                lvtpl = CSTable.StaticDataManager.ItemEquipRefine[lv];
                if (lvtpl == null)
                {
                    break;
                }
                float rate = lvtpl.rate;
                float rand = (float)mRandom.NextDouble();
                if (rate > rand)
                {
                    lv++;
                }
                costcount++;
            }
            if (false == this.Bag.RemoveItemCountByTid(lvtpl.mid, costcount))
            {
                pkg.Write((sbyte)CSCommon.eRet_EquipRefine.RemoveItemFailed);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            equip.ItemData.ItemRefineLv = lv;
            pkg.Write((sbyte)CSCommon.eRet_EquipRefine.Succeed);
            CalcChangeType(eValueType.Equip);
            pkg.Write(equip.ItemData.ItemRefineLv);
            pkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_InlayGem(UInt16 pos, int id, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var gem = this.GemBag.FindItemById(id);
            if (gem == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_InlayGem.NoGem);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (gem.ItemTemplate.ItemType != (int)CSCommon.eItemType.Gem)
            {
                pkg.Write((sbyte)CSCommon.eRet_InlayGem.NotGem);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            Item item = Item.DangerousCreateItemById(this, id, 1);
            if (this.EquipGemBag.AddItem2Position(pos, item) == false)
            {
                pkg.Write((sbyte)CSCommon.eRet_InlayGem.NoPos);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            this.GemBag.RemoveItemCountByTid(id, 1);
            pkg.Write((sbyte)CSCommon.eRet_InlayGem.Succeed);
            CalcChangeType(eValueType.Gem);
            pkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_RemoveGem(UInt16 pos, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var gem = this.EquipGemBag.FindItemByPos(pos);
            if (gem == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_RemoveGem.NoGem);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (this.GemBag.GetEmptyCount() <= 0)
            {
                pkg.Write((sbyte)CSCommon.eRet_RemoveGem.BagNoPos);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var itemcom = CSCommon.ItemCommon.Instance;
            if (itemcom == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_RemoveGem.NoItemCom);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
//             CSCommon.eCurrenceType type = itemcom.RemoveGemMoneyType;
//             int count = itemcom.RemoveGemMoney;
//             if (_IsMoneyEnough(type, count) == false)
//             {
//                 pkg.Write((sbyte)CSCommon.eRet_RemoveGem.LessMoney);
//                 pkg.DoReturnPlanes2Client(fwd);
//                 return;
//             }

            if (this.GemBag.AutoAddItem(gem) == false)
            {
                pkg.Write((sbyte)CSCommon.eRet_RemoveGem.AddToBagFailed);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            this.EquipGemBag.RemoveItem(gem);
//            _ChangeMoney(type, CSCommon.Data.eMoneyChangeType.RemoveGem, -count);
            pkg.Write((sbyte)CSCommon.eRet_RemoveGem.Succeed);
            CalcChangeType(eValueType.Gem);
            pkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GemCombine(int id, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            int count = GemBag.GetItemCount(id);
            int need = CSCommon.ItemCommon.Instance.GemCombineNeedCount;
            if (count < need)
            {
                pkg.Write((sbyte)CSCommon.eRet_GemCombine.LessCount);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var getId = id + 1;
            var getTpl = CSTable.ItemUtil.GetItem(getId);
            if (getTpl == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_GemCombine.NotCombine);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            int getCount = count / need;
            int costCount = getCount * need;
            GemBag.RemoveItemCountByTid(id, costCount);
            CreateItemToBag(getId, getCount);
            pkg.Write((sbyte)CSCommon.eRet_GemCombine.Succeed);
            pkg.DoReturnPlanes2Client(fwd);
        }

        
        #endregion

        #region 时装

        /// <summary>
        /// 解封时装
        /// </summary>
        /// <param name="id"></param>
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetFashion(int id, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var tpl = CSTable.ItemUtil.GetItem(id) as CSTable.ItemFashionData;
            if (tpl == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_BuyFashion.NoTemplate);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (this.FashionBag.GetItemCount(id) != 0)
            {
                pkg.Write((sbyte)CSCommon.eRet_BuyFashion.Has);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (!_IsProMatch((byte)tpl.ItemProfession))
            {
                pkg.Write((sbyte)CSCommon.eRet_BuyFashion.ProError);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (!_IsLevelEnough(tpl.ItemUseRoleLv))
            {
                pkg.Write((sbyte)CSCommon.eRet_BuyFashion.LessLv);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (!CreateItemToBag(id, 1))
            {
                pkg.Write((sbyte)CSCommon.eRet_BuyFashion.CreateFailed);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            pkg.Write((sbyte)CSCommon.eRet_BuyFashion.Succeed);
            pkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_WearFashion(ulong itemId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var item = this.FashionBag.FindItemById(itemId);
            if (item == null || item.ItemData == null)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (item.ItemData.WearState == (byte)CSCommon.eBoolState.True)
            {
                pkg.Write((sbyte)-2);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (item.ItemTemplate == null)
            {
                pkg.Write((sbyte)-3);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var tType = (item.ItemTemplate as CSTable.ItemFashionData).FashionType;
            this.FashionBag.TakeOffFashionByType(tType);
            item.ItemData.WearState = (byte)CSCommon.eBoolState.True;
            pkg.Write((sbyte)1);
            pkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_TakeOffFashion(ulong itemId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var item = this.FashionBag.FindItemById(itemId);
            if (item == null || item.ItemData == null)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (item.ItemData.WearState == (byte)CSCommon.eBoolState.False)
            {
                pkg.Write((sbyte)-2);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (item.ItemTemplate == null)
            {
                pkg.Write((sbyte)-3);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            var tType = (item.ItemTemplate as CSTable.ItemFashionData).FashionType;
            this.FashionBag.TakeOffFashionByType(tType);

            pkg.Write((sbyte)1);
            pkg.DoReturnPlanes2Client(fwd);

        }

        #endregion

        #region 物品掉落



        public int GetDropItems(int dropId, List<IdNum> items)
        {
            items.Clear();
            int mul = 1;//倍数
            var template = CSCommon.DropTemplateManager.Instance.GetTemplate(dropId);
            if (template == null)
                return mul;

            CSCommon.eDropType type = template.DropType;
            if (type == CSCommon.eDropType.Drop_1)//权值掉落
            {
                int dropnum = 0;//掉落次数
                int totalnumvalue = 0;
                foreach (var i in template.DropNumRate)
                {
                    totalnumvalue += i.DropValue;
                }
                int dice = CSCommon.Data.ItemData.Rand.Next(0, totalnumvalue);
                int sig = 0;
                for (int i = 0; i < template.DropNumRate.Count; i++)
                {
                    if (dice >= sig && dice < sig + template.DropNumRate[i].DropValue)
                    {
                        dropnum = template.DropNumRate[i].DropNum;
                        break;
                    }
                    sig += template.DropNumRate[i].DropValue;
                }

                List<CSCommon.DropElement> dropElems = template.DropElems;
                if (template.NoRepeatElem)
                {
                    dropElems = new List<CSCommon.DropElement>();
                    dropElems.AddRange(template.DropElems);
                }

                int sigma = 0;
                foreach (var i in dropElems)
                {
                    sigma += i.DropValue;
                }

                for (int i = 0; i < dropnum; i++)
                {
                    if (template.NoRepeatElem)
                    {
                        sigma = 0;
                        foreach (var j in dropElems)
                        {
                            sigma += j.DropValue;
                        }
                    }

                    dice = CSCommon.Data.ItemData.Rand.Next(0, sigma);

                    int index = -1;
                    sig = 0;
                    for (int k = 0; k < dropElems.Count; k++)
                    {
                        if (dice >= sig && dice < sig + dropElems[k].DropValue)
                        {
                            index = k;
                            break;
                        }
                        sig += dropElems[k].DropValue;
                    }

                    if (index == -1)
                        continue;
                    if (dropElems[index].Items == null)
                        continue;
                    if (dropElems[index].Items.Count == 0)
                        continue;

                    int itemindex = CSCommon.Data.ItemData.Rand.Next(0, dropElems[index].Items.Count);
                    int num = dropElems[index].DropNum.RandValue(CSCommon.Data.ItemData.Rand);
                    if (num == 0)
                        continue;
                    IdNum item = new IdNum();
                    item.templateId = dropElems[index].Items[itemindex];
                    item.stackNum = num;
                    items.Add(item);
                }

                if (template.IsMultiple)//多倍
                {

                    foreach (var i in template.DropMultiple)
                    {
                        totalnumvalue += i.DropValue;
                    }
                    dice = CSCommon.Data.ItemData.Rand.Next(0, totalnumvalue);
                    sig = 0;
                    for (int i = 0; i < template.DropMultiple.Count; i++)
                    {
                        if (dice >= sig && dice < sig + template.DropMultiple[i].DropValue)
                        {
                            mul = template.DropMultiple[i].DropNum;
                            break;
                        }
                        sig += template.DropMultiple[i].DropValue;
                    }
                }
            }
            else if (type == CSCommon.eDropType.Drop_2)//非权值掉落
            {
                int randValue = template.DropNum.RandValue(CSCommon.Data.ItemData.Rand);
                foreach (var i in template.DropList)
                {
                    if (i.DropValue < randValue)//值小于随机值掉落
                    {
                        int num = i.DropNum.RandValue(CSCommon.Data.ItemData.Rand);//随机物品数量
                        if (num == 0)
                            continue;

                        IdNum item = new IdNum();
                        item.templateId = i.ItemId;
                        item.stackNum = num;
                        items.Add(item);
                    }
                }
            }

            if (items.Count > 0)
            {
                CreatDropItems(items, mul);
            }

            return mul;
        }

        public void CreatDropItems(List<IdNum> items, int multiple)
        {
            foreach (var i in items)
            {
                if (i.templateId < (int)CSCommon.eCurrenceType.MaxCurrenceTypeNum && i.templateId > (int)CSCommon.eCurrenceType.Unknow)
                {
                    //货币
                    _ChangeMoney((CSCommon.eCurrenceType)i.templateId, eMoneyChangeType.Drop, i.stackNum * multiple);
                }
                else
                {
                    //物品
                    CreateItemToBag(i.templateId, i.stackNum * multiple);
                }
                
            }
        }



        #endregion
    }
}