using ServerFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class 持续加血 : IUseCommand
    {
        PlayerInstance user;
        object mTimeHandle;
        int 一次加血;
        float 时间间隔;
        float 持续时间;
        public void used(ServerFrame.TimerEvent et)
        {
            持续时间 -= 时间间隔;
            if (持续时间 <=0 )
            {
                ServerFrame.TimerManager.clearTimer(mTimeHandle);
                return;
            }
            user.ChangeHP(一次加血, user);
            var pkg = new RPC.PackageWriter();
            //Wuxia.H_RpcRoot.smInstance.HIndex(pkg, user.Id).RPC_SkillReceiveData(pkg, 0, 0, (byte)CSCommon.eHitType.AddHp, 一次加血);
            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, user.Id).RPC_FlutterInfo(pkg, (byte)CSCommon.eFlutterInfoType.AddHp, 0, 0, 0);
            pkg.DoCommandPlanes2Client(user.Planes2GateConnect, user.ClientLinkId);
        }

        public int Execute(PlayerInstance _user, int count, string arg1, string arg2, string arg3)
        {
            user = _user;
            if (user.PlayerData.RoleDetail.RoleHp >= user.PlayerData.RoleDetail.RoleMaxHp)
            {
                return (int)CSCommon.eRet_UseItem.HpMax;
            }
            一次加血 = Convert.ToInt32(arg1);
            时间间隔 = Convert.ToSingle(arg2);
            持续时间 = Convert.ToSingle(arg3);
            mTimeHandle = ServerFrame.TimerManager.doLoop(时间间隔, used);
            return 1;
        }
    }

    public class 立即加血 : IUseCommand
    {
        public int Execute(PlayerInstance _user, int count, string arg1, string arg2, string arg3)
        {
            if (_user.PlayerData.RoleDetail.RoleHp >= _user.PlayerData.RoleDetail.RoleMaxHp)
            {
                return (int)CSCommon.eRet_UseItem.HpMax;
            }
            var 一次加血 = Convert.ToInt32(arg1);
            _user.ChangeHP(一次加血, _user);
            var pkg = new RPC.PackageWriter();
            //Wuxia.H_RpcRoot.smInstance.HIndex(pkg, _user.Id).RPC_SkillReceiveData(pkg, 0, 0, (byte)CSCommon.eHitType.AddHp, 一次加血);
            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, _user.Id).RPC_FlutterInfo(pkg, (byte)CSCommon.eFlutterInfoType.AddHp, 0, 0, 0);
            pkg.DoCommandPlanes2Client(_user.Planes2GateConnect, _user.ClientLinkId);
            return 1;
        }
    }

    public class 立即加蓝 : IUseCommand
    {
        public int Execute(PlayerInstance _user, int count, string arg1, string arg2, string arg3)
        {
            if (_user.PlayerData.RoleDetail.RoleMp >= _user.FinalRoleValue.MaxMP)
            {
                return (int)CSCommon.eRet_UseItem.MpMax;
            }
            var 一次加蓝 = Convert.ToInt32(arg1);
            _user.ChangeMP(一次加蓝);
            return 1;
        }
    }

    public class 立即加血蓝 : IUseCommand
    {
        public int Execute(PlayerInstance _user, int count, string arg1, string arg2, string arg3)
        {
            if (_user.PlayerData.RoleDetail.RoleMp >= _user.FinalRoleValue.MaxMP && _user.PlayerData.RoleDetail.RoleHp >= _user.PlayerData.RoleDetail.RoleMaxHp)
            {
                return (int)CSCommon.eRet_UseItem.MpMax;
            }
            //arg1 按百分比加
            var 一次加蓝 = (int)(_user.FinalRoleValue.MaxMP * Convert.ToSingle(arg1));
            _user.ChangeMP(一次加蓝);

            var 一次加血 = (int)(_user.FinalRoleValue.MaxHP * Convert.ToSingle(arg1));
            _user.ChangeHP(一次加血, _user);

            var pkg = new RPC.PackageWriter();
            //Wuxia.H_RpcRoot.smInstance.HIndex(pkg, _user.Id).RPC_SkillReceiveData(pkg, 0, 0, (byte)CSCommon.eHitType.AddHp, 一次加血);
            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, _user.Id).RPC_FlutterInfo(pkg, (byte)CSCommon.eFlutterInfoType.AddHp, 0, 0, 0);
            pkg.DoCommandPlanes2Client(_user.Planes2GateConnect, _user.ClientLinkId);
            return 1;
        }
    }

    public class 增加人物货币 : IUseCommand
    {
        public int Execute(PlayerInstance _user, int count, string arg1, string arg2, string arg3)
        {
            var 类型 = Convert.ToInt32(arg1);
            var 数量 = Convert.ToInt32(arg2);
            _user._ChangeMoney((CSCommon.eCurrenceType)类型, CSCommon.Data.eMoneyChangeType.ItemUse, 数量);
            return 1;
        }
    }

    public interface IUseCommand
    {
        int Execute(PlayerInstance _user, int count, string arg1, string arg2, string arg3);
    }
    public class Item
    {
        public IUseCommand Script;
        public sbyte Use(int count, PlayerInstance player)
        {
            sbyte result = (sbyte)CSCommon.eRet_UseItem.Succeed;
            var useTemplate = CSTable.StaticDataManager.ItemUse[ItemData.Template.ItemUseId];
            if (useTemplate == null)
            {
                result = (sbyte)CSCommon.eRet_UseItem.TplNull;
                Log.Log.Item.Print("useTemplate == null id={0}", ItemData.Template.ItemUseId);
                return result;
            }
            if (Script==null)
            {
                Script = System.Activator.CreateInstance(Type.GetType(string.Format("ServerCommon.Planes.{0}", useTemplate.ItemUseMethod)), null) as IUseCommand;
                if (Script==null)
                {
                    result = (sbyte)CSCommon.eRet_UseItem.MethodNull;
                    Log.Log.Item.Print("ERROR:item Use,CreateInstance={0}", useTemplate.ItemUseMethod);
                    return result;
                }
            }
            int useResult = Script.Execute(player, count, useTemplate.Arg1, useTemplate.Arg2, useTemplate.Arg3);
            if (useResult < 0)
            {
                result = (sbyte)useResult;
            }
            else
            {
                if (useResult > 0)
                {
                    mItemData.StackNum -= useResult;
                    if (mItemData.StackNum > 0)
                    {
                        RPC.PackageWriter pkg = new RPC.PackageWriter();
                        Wuxia.H_RpcRoot.smInstance.RPC_ItemStackNumChanged(pkg, mItemData.ItemId, (byte)mItemData.Inventory, mItemData.StackNum);
                        pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
                    }
                    else
                    {
                        Inventory.DeleteItem(mItemData.ItemId);
                    }
                }
            }
            return result;
        }

        bool ForceUseLimit(PlayerInstance player)
        {

            return true;//ture 为可以用 false 为不可以用
        }

        public static Item DangerousCreateItem(RoleActor owner,CSCommon.Data.ItemData data,bool bAllocItemId)
        {
            if (data.Template == null)
                return null;

            data.Inventory = (byte)CSCommon.eItemInventory.Unknown;
            if (owner != null)
            {
                data.OwnerId = owner.Id;
            }
            if (bAllocItemId)
            {
                data.ItemId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Item);
                data.CreateTime = System.DateTime.Now;
            }
            Item result = new Item(data);
            return result;
        }

        public static Item DangerousCreateItemById(RoleActor owner, int templateId, int stackNum)
        {
            CSCommon.Data.ItemData data = new CSCommon.Data.ItemData();
            data.ItemTemlateId = templateId;
            data.StackNum = stackNum;
            if (owner != null)
            {
                data.OwnerId = owner.Id;
            }
            data.ItemId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Item);
            data.CreateTime = System.DateTime.Now;

            Item result = new Item(data);
            return result;
        }

        public static List<Item> DangerouseDropItems(RoleActor picker, CSCommon.DropTemplate drop)
        {
            var result = new List<Item>();
//             foreach (var i in drop.DropItems)//这里处理随机掉落
//             {
//                 if (i.Items.Count == 0)//这个用来占不掉落概率的
//                     continue;
// 
//                 int dice = RoleActor.Random.Next(0, drop.TotalDropValue);
//                 if (dice < i.DropValue)
//                 {
//                     for (int j = 0; j < i.Number; ++j)
//                     {
//                         int index = RoleActor.Random.Next(0, i.Items.Count);
//                         var itemData = new CSCommon.Data.ItemData();
//                         itemData.ItemTemlateId = i.Items[index];
//                         itemData.DangrousReInitData();                                            
//                         //if (i.Number < itemData.Template.ItemMaxStackNum)
//                         //    itemData.StackNum = (UInt16)i.Number;
//                         //这里是任务物品时，如果没接此物品的任务或背包有足够物品时不掉落
//                         if (itemData.Template.ItemType == CSCommon.eItemType.Other && CheckTaskItem(picker,itemData) ==false)
//                             continue;                   
//                         var item = ServerCommon.Planes.Bag.Item.DangerousCreateItem(null, itemData, true);
//                         result.Add(item);
//                     }
//                 }
//             }
// 
//             foreach (var i in drop.SureDropItems)//这里处理一定掉落
//             {
//                 if (i.Items.Count == 0)//这个用来占不掉落概率的
//                     continue;
//                 for (int j = 0; j < i.Items.Count; ++j)
//                 {
//                     var itemData = new CSCommon.Data.ItemData();
//                     itemData.ItemTemlateId = i.Items[j];
//                     itemData.DangrousReInitData();
//                     var item = ServerCommon.Planes.Bag.Item.DangerousCreateItem(null, itemData, true);
//                     result.Add(item);
//                 }
//             }
            return result;
        }

        public static bool CheckTaskItem(RoleActor picker, CSCommon.Data.ItemData itemData)
        {
//             PlayerInstance player = picker as PlayerInstance;
//             if (player == null)
//                 return false;
// 
//             foreach (var i in player.TaskManager.AcceptTasks)
//             {
//                 var conditions = i.TaskData.Template.KillCondition;
//                 for (sbyte j = 0; j < conditions.Count; j++)
//                 {
//                     if (conditions[j].DropItem == itemData.ItemTemlateId && player.Bag.GetItemCount(conditions[j].DropItem) <= conditions[j].Number) //我接了调这个物品的任务,并且任务物品不够时
//                         return true;
//                 }
//             }
            return false;
        }

        public void DestroyFromDB(RoleActor role)
        {
            sbyte destroy = 1;
            //以后这里要根据物品重要程度决定是否真实从数据库删除
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            ServerCommon.H_RPCRoot.smInstance.HGet_DataServer(pkg).DelItem(pkg, role.Id, ItemData.ItemId, destroy);
            pkg.DoCommand(IPlanesServer.Instance.DataConnect, RPC.CommandTargetType.DefaultType);
        }

        BagBase mInventory = null;
        public BagBase Inventory
        {
            get { return mInventory; }
            set 
            { 
                mInventory = value;
                if (mInventory!=null)
                {
                    mItemData.Inventory = (byte)mInventory.InventoryType;
                }
                else
                {
                    mItemData.Inventory = (byte)CSCommon.eItemInventory.Unknown;
                }
            }
        }

        UInt16 mPosition;
        public UInt16 Position
        {
            get 
            {
                if (ItemData != null)
                    return ItemData.Position;
                return mPosition; 
            }
            set 
            {
                if (ItemData != null)
                    ItemData.Position = value;
                mPosition = value; 
            }
        }

        CSTable.IItemBase mItemTemplate;
        public CSTable.IItemBase ItemTemplate
        {
            get { return mItemTemplate; }
            set { mItemTemplate = value; }
        }

        public Item(CSCommon.Data.ItemData item)
        {
            mItemTemplate = item.Template;
            mItemData = item;
            mPosition = item.Position;
        }

        CSCommon.Data.ItemData mItemData = null;
        public CSCommon.Data.ItemData ItemData
        {
            get 
            {
                return mItemData; 
            }
        }

        int mStackNum;
        public int StackNum
        {
            get 
            {
                if (ItemData != null)
                    return ItemData.StackNum;
                return mStackNum;
            }
            set 
            {
                if (ItemData!=null)
                    ItemData.StackNum = value;
                mStackNum = value;
            }
        }

        public int MaxStackNum
        {
            get
            {
                return mItemTemplate.ItemMaxStackNum;
            }
        }

        public void UpLevel()
        {
            ItemData.ItemLv++;
            var etemp = CSTable.ItemUtil.GetEquipLvTpl(mItemTemplate.ItemProfession, (mItemTemplate as CSTable.ItemEquipData).EquipType, mItemData.ItemLv);
            ItemData.ItemTemlateId = etemp.itemid;
        }

        public void GetEquipExp(int addExp)
        {
            while (addExp > 0)
            {
                var etemp = CSTable.ItemUtil.GetEquipLvTpl(mItemTemplate.ItemProfession, (mItemTemplate as CSTable.ItemEquipData).EquipType, mItemData.ItemLv);
                if (etemp == null || etemp.needExp <= 0)
                {
                    return;
                }
                var needExp = etemp.needExp - mItemData.CurExp;
                if (addExp >= needExp)
                {
                    addExp -= needExp;
                    ItemData.CurExp = 0;
                    ItemData.ItemLv++;
                    ItemData.ItemTemlateId = etemp.itemid;
                }
                else
                {
                    ItemData.CurExp += addExp;
                    addExp = 0;
                }
            }
        }

        public float GetEquipValue(CSCommon.eEquipValueType type, short lv)
        {
            var temp = CSTable.ItemUtil.GetEquipLvTpl(mItemTemplate.ItemProfession, (mItemTemplate as CSTable.ItemEquipData).EquipType, lv);
            if (temp.attri1 == (int)type)
            {
                return temp.value1;
            }
            return GetDefaultValue(type);
        }

        public float GetDefaultValue(CSCommon.eEquipValueType type)
        {
            switch (type)
            {
                case CSCommon.eEquipValueType.Power:
                    return 0;
                case CSCommon.eEquipValueType.Body:
                    return 0;
                case CSCommon.eEquipValueType.Dex:
                    return 0;
                case CSCommon.eEquipValueType.Hp:
                    return 0;
                case CSCommon.eEquipValueType.HpRate:
                    return 1;
                case CSCommon.eEquipValueType.Mp:
                    return 0;
                case CSCommon.eEquipValueType.Atk:
                    return 0;
                case CSCommon.eEquipValueType.GoldDef:
                    return 0;
                case CSCommon.eEquipValueType.WoodDef:
                    return 0;
                case CSCommon.eEquipValueType.WaterDef:
                    return 0;
                case CSCommon.eEquipValueType.FireDef:
                    return 0;
                case CSCommon.eEquipValueType.EarthDef:
                    return 0;
                case CSCommon.eEquipValueType.AllDef:
                    return 0;
                case CSCommon.eEquipValueType.AllDefRate:
                    return 1;
                case CSCommon.eEquipValueType.Crit:
                    return 0;
                case CSCommon.eEquipValueType.CritRate:
                    return 1;
                case CSCommon.eEquipValueType.CritDef:
                    return 0;
                case CSCommon.eEquipValueType.CritDefRate:
                    return 1;
                case CSCommon.eEquipValueType.Hit:
                    return 0;
                case CSCommon.eEquipValueType.Dodge:
                    return 0;
                case CSCommon.eEquipValueType.Move:
                    return 0;
                case CSCommon.eEquipValueType.UpExpRate:
                    return 1;
                case CSCommon.eEquipValueType.DsRate:
                    return 1;
                case CSCommon.eEquipValueType.UpHurt:
                    return 0;
                case CSCommon.eEquipValueType.DownHurt:
                    return 0;
                case CSCommon.eEquipValueType.UnusualDefRate:
                    return 1;
                default:
                    return 0;
            }
        }
    }
}
