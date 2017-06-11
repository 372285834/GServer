using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace CSCommon.Data
{

    public enum eMoneyChangeType
    {
        GM = -1,
        SendGift = 0,
        BuyShopItem = 1,
        TaskReward = 2,
        Mail = 3,
        CreateGuild = 4,
        GuildContribute = 5,
        BuyConsignItem = 6,
        ConsignItem = 7,
        BuyFashion = 8,
        CombineItem = 9,
        EquipIntensify = 10,
        EquipRefine = 11,
        RemoveGem = 12,
        DailyReward = 13,
        UpSkill = 14,
        LearnSkill = 15,
        KillMonster = 16,
        SellItem = 17,
        UpMartialLevel = 18,
        UpPlantLevel = 19,
        UpTrainLevel = 20,
        UpSmeltLevel = 21,
        GetMartialOut = 22,
        BuyRmb = 23,
        ItemUse = 24,
        Drop = 25,
    }

    public enum eBagInfo
    {
        Helmet,//头盔
        Chest,//胸甲
        Belt,//腰带
        Cuff, // 护腕
        Shoes,//鞋子

        Weapon, //武器
        NeckLace, //项链
        Ring1,    //戒指1
        Ring2,   //戒指2


        WeaponSoul, // 武魂，武器的时装
        Fashion,//时装

        EquipmentCount,

        Other,//其他
    }

    public enum RState
    {
        WhiteName,     //白名
        PinkName,      //粉名
        RedName,       //红名
        BlueName       //蓝名
    }

    public enum RModel
    {
        PeaceModel,     //和平模式
        GuildModel,     //帮会模式
        AllModel,       //全体模式
        TeamModel,      //组队模式
        GoodEvilModel,  //善恶模式
    }

    public enum eRoleAttType
    {
        DontAttack,//不攻击
        PassiveAttack,//被动攻击
        ActiveAttack,//主动攻击
    }

    public enum eRoleCreateType 
    {
        Unknown,//未知
        Player,//角色
        Npc,//npc
        Summon,//召唤物
        DropedItem,//可拾取物品
        Trigger,//触发器
        Collected,//采集物
    }
     
    

   
}
