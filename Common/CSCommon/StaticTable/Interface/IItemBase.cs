using CSCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSTable
{
    public interface IItemBase
    {
        //物品Id;
        int id { get; }
        //物品名字;
        string ItemName { get; }
        //物品类型;
        int ItemType { get; }
        //物品品质;
        int ItemQuality { get; }
        //物品描述;
        string ItemDesc { get; }
        //物品图标;
        string ItemIconName { get; }
        //物品最大堆叠数量;
        int ItemMaxStackNum { get; }
        //物品最大保质期;
        int ItemMaxShelflife { get; }
        //物品绑定;
        int ItemBindType { get; }
        //阵营限制;
        int ItemUseCamp { get; }
        //性别限制;
        int ItemSex { get; }
        //职业限制;
        int ItemProfession { get; }
        //角色等级限制;
        ushort ItemUseRoleLv { get; }
        //角色官职等级限制;
        byte ItemUseRoleOfficeLv { get; }
        //vip等级限制;
        byte ItemUseVipLv { get; }
        //物品是否可手动使用;
        byte CanManualUse { get; }
        //物品使用对应模板;
        int ItemUseId { get; }
        //是否可出售;
        byte ItemIsSell { get; }
        //贩卖货币类型;
        int SellCurrenceType { get; }
        //出售价格;
        int SellingPrice { get; }
    }
}
