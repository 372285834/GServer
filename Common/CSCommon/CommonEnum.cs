using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    public enum eOnlineState
    {
        Online,//在线
        Offline,//离线
    }

    public enum eRoleSex
    {
        None = 0,
        Men = 1,//男
        Women = 2,//女         
    }

    public enum eProfession :byte //职业划分
    {
        Unknow = 0,
        Shaolin_Changzhang = 1,//少林 长杖
        Wudang_Qingjian = 2, //武当 轻剑
        Emei_Guqing = 3,   //峨眉 古琴
        Tangmen_Xiujian = 4,//唐门 袖箭
        Gaibang_Dadao = 5,//丐帮 大刀
        Mingjiao_ShuangYue = 6,//明教 双钺
        MAX_COUNT,
    }


    public enum eRoleType
    {
        Unknown,
        Player, //玩家
        Monster,//普通怪物
        Elite,//精英
        Boss,//boss
        Npc,//功能npc
        Guard,//守卫
        Collected,//采集NPC
    }

    public enum eCamp:byte
    {
        None = 0, //中立阵营
        Song = 1, //宋
        Jing = 2, //金
        Dali = 3, //大理
        Xixia= 4, //西夏

        CustemStart = 100,
        Friend = 101,
        Enemy = 102,
        
    }

    public enum ePKMode:byte
    {
        Peace,      //和平模式
        Kill,       //屠杀模式
    }

    public enum eFindType
    {
        Exact,//精确查询
        Fuzzy,//模糊查询
    }

    public enum eMailType
    {
        System,     //系统邮件
        Guild,      //帮会   
        Pawnshop,   //当铺
        Activity,   //活动
        Score,      //战报，战斗结果
    }

    public enum eItemType//物品类型
    {
        Unknow = 0,
        Package = 1,    //礼包
        Gem = 2,        //宝石
        Material = 3,   //材料
        Consumable = 4, //消耗品
        Fashion = 5,    //时装
        Equip = 6,      //装备            
        Gift = 7,       //礼物    
        End = 8,
    }

    public enum eEquipType//装备类型
    {
        Weapon = 0,         //武器
        Necklace = 1,       //项链
        Ring1 = 2,          //戒指1
        Ring2 = 3,          //戒指2
        JadePendant = 4,    //玉佩
        Head = 5,           //头盔
        Chest = 6,          //衣服
        Belt = 7,           //腰带
        Cuff = 8,           //护手
        Shoes = 9,          //鞋子
        Soul = 10,           //武魂
        Mount = 11,          //坐骑
        Pendant = 12,        //挂件
        MaxBagSize,

    }
    public enum eItemInventory
    {
        Unknown = 0xFF,
        ItemBag = 0,  //背包
        EquipBag = 1, //装备
        FashionBag = 2, //装备
        EquipGemBag = 3, //装备宝石背包
        GemBag = 4,//宝石背包
        StoreBag = 5, //仓库
        Mail = 6,     //邮件背包
        TavernTaskReward = 7,  //任务背包
    }

    public enum eItemQuality//物品品质
    {
        White = 0,          //白色
        Green = 1,          //绿色
        Blue = 2,           //蓝色
        Purple = 3,         //紫色
        Orange = 4,         //橙色
        Gold = 5,           //金色
    }

    public enum eShopType
    {
        HotItem,//热销
        Drug,   //药剂
        Mount,  //坐骑
        Rmb,    //元宝
        MaxType,
    }

    public enum eCurrenceType//货币类型
    {
        Unknow = 0,
        Gold = 1,           //银两
        Rmb = 2,            //元宝
        Yuan = 3,           //人民币
        Reputation = 4,     //声望
        Exploit = 5,        //战功
        Activeness = 6,     //活跃度
        Exp = 7,            //经验
        Air = 8,            //技能点
        Food = 9,          //粮食
        MaxCurrenceTypeNum = 10,
    }


    public enum ePackageType
    {
        OneToN = 0,         //1个人重复得到的礼包
        OneToOne = 1,       //1个人得到1次的礼包
        NToOne = 2,         //N个人可以兑换的礼包
    }

    public enum eBindType
    {
        None = 0,           //无绑定
        NatureBind = 1,     //天生绑定
        UseBind = 2,        //使用绑定
    }

    public enum eSocialType
    {
        None,                   //陌生人
        Friend = 1 << 0,        //好友
        ManWife = 1 << 1,       //夫妻
        Brother = 1 << 2,       //兄弟
        BasicFriends = 1 << 3,  //基友
        Lover = 1 << 4,         //情人
        Sister = 1 << 5,        //姐妹
        Lily = 1 << 6,          //百合
        Black = 1 << 7,         //黑名单
        Enemy = 1 << 8,         //仇人
    }

    public enum eNpcType
    {
        Monster=0,//怪物
        Leader=1,//头目
        Elite=2,//精英
        Boss=3,//boss
        FoodCar = 4,//粮车
        MoneyCar = 5,//军饷车
        Point = 6, //据点NPC

        Npc=10,//功能npc

        Guard=20,//守卫

        TaskCollected=30,//任务采集物
        //MineralCollected,//矿物采集NPC
        //WoodCollected,//木材采集NPC
        //GrassCollected,//草药采集NPC
    }

    public enum eSayChannel
    {
        WorldChannel,      //世界
        CampChannel,       //国家
        GuildChannel,      //帮派
        TeamChannel,       //队伍
        WhisperChannel,    //私聊
        SystemChannel,     //系统
        BroadcastChannel,  //喇叭
    }

    public enum eDailyState
    {
        None,       //没领取
        Accepted,   //领取中
        Finished,   //完成
    }

    public enum eTaskState
    {
        Accepted,//领取中
        Finished,//完成
        Rewarded,//领取完奖励
    }

    public enum eMailState
    {
        UnOpen, //未打开
        Opened, //打开
    }

    public enum eFsmState : byte //按动作划分
    {
        Unknown,
        Attack,
        Stand,
        Die,
        Run,
        Patrol,  //巡逻
        Return,  //返回出生点
        Stun,   //眩晕
    }   

    public enum eGuildPost
    {
        None,           //未加入帮会
        BangZhong,      //帮众
        JingYing,       //精英
        TangZhu,        //堂主
        ZhangLao,       //长老
        BangZhu,        //帮主
    }

    public enum eMessageFrom
    {
        Guild,
        Team,
        Social,
    }

    public enum eMessageType
    {
        Guild_Ask,                  //申请消息
        Guild_AcceptAsk,            //同意消息
        Guild_RefuseAsk,            //拒绝消息
        Guild_Invite,               //邀请加入帮会
        Guild_AcceptInvite,         //同意邀请
        Guild_RefuseInvite,         //拒绝邀请
        Guild_KickOut,              //踢出公會消息
        Guild_Dissolve,             //解散工会消息
        Guild_SystemDissolve,       //系统解散工会消息
        Guild_SetZhangLao,          //设为长老
        Guild_SetJingYing,          //设为精英
        Guild_SetTangZhu,           //设为堂主
        Guild_SetBangZhong,         //设为帮众
        Guild_TransferBangZhu,      //设为帮主
        Friend_Ask,                     //好友申请
        ManWife_Ask,                    //夫妻申请
        Brother_Ask,                    //兄弟申请
        BasicFriends_Ask,               //基友申请
        Lover_Ask,                      //情人申请
        Sister_Ask,                     //姐妹申请
        Lily_Ask,                       //百合申请
        Couple_Refuse,                  //拒绝亲人申请
        Couple_Accept,                  //同意亲人申请
        Couple_Remove,                  //解除亲人关系
        Team_Invite,//邀请加入队伍
        Team_Ask,//申请加入队伍
        Team_KickOut,//踢出队伍
        Team_AskRefuse,//拒绝申请
        Team_AskAgree,//同意申请
        Team_InviteRefuse,//拒绝邀请
        Team_InviteAgree,//同意邀请
    }

    public enum eOperateAsk
    {
        Accept,
        Refuse,
    }

    public enum eEquipValueType
    {
        Power = 1,              //内功值
        Body = 2,               //外功值
        Dex = 3,                //身法值
        Hp = 4,                 //生命值
        HpRate = 5,             //生命百分比
        Mp = 6,                 //内力值
        Atk = 7,                //攻击
        GoldDef = 8,            //金防御
        WoodDef = 9,            //木防御
        WaterDef = 10,          //水防御
        FireDef = 11,           //火防御
        EarthDef = 12,          //土防御
        AllDef = 13,            //全防御
        AllDefRate = 14,        //全防御百分比
        Crit = 15,              //暴击值
        CritRate = 16,          //暴击百分比
        CritDef = 17,           //暴抗值
        CritDefRate = 18,       //暴抗百分比
        Hit = 19,               //命中值
        Dodge = 20,             //闪避值
        Move = 21,              //移动值
        UpExpRate = 22,         //提升杀怪经验百分比
        DsRate = 23,            //致命一击百分比
        UpHurt = 24,            //伤害加深百分比
        DownHurt = 25,          //伤害减免百分比
        UnusualDefRate = 26,    //异常状态抗性百分比
    }



    public enum eBoolState
    {
        False = 0,
        True = 1,
    }

    public enum eTaskType
    {
        None = 0,
        KillMonster = 1,    //杀怪
        NpcTalk = 2,        //对话
        UpLevel = 3,        //升级
        GetItem = 4,        //收集物品
        Collect = 5,        //采集
        CampSelect = 6,     //阵营选择
    }

    public enum eElemType
    {
        None = 0,
        Gold,
        Wood,
        Water,
        Fire,
        Earth,

        MAX,
    }

    //哪里发邮件
    public enum eMailFromType
    {
        KickedOutGuild = 1,         //被T出帮会
        RoleDissolveGuild = 2,      //帮主解散帮会
        SystemDissolveGuild = 3,    //系统解散帮会
        GuildGoldLessInfo = 4,      //提醒帮会资金不足
        GuildSendGold = 5,          //帮会发放福利
        ConsignFailedReturn = 6,    //寄售物品失败
        BuyConsignSucceed = 7,      //购买寄售物品成功
    }

    public enum eDailyType
    {
        Country,        //国家
        Lake,           //江湖
        Yundart,        //运镖
    }

    public enum eDailyCountryType
    {
        Collect,    //采集龙脉
        Guard,      //守卫杀敌
    }

    public enum eDailyYundartType
    {
        Yundart,    //运镖
        Plunderdart,//劫镖
    }

    /// <summary>
    /// 复活模式
    /// </summary>
    public enum eReliveMode:byte
    {
        Current,    //原地复活
        RobornPos,  //复活点
    }

    public enum eRoleFightState
    {
        Normal,         //正常
        Stun,           //眩晕,无法做任何操作
        LockMove,       //定身，除了不能移动其他都可以
        Slient,         //沉默,不能释放技能
        Sleep,          //沉睡，无法做任何操作，受到任何伤害解除
        PKProtect,      //出生PK保护，无法被攻击
    }

    public enum eMartialItemType
    {
        MartialStart = 0,
        Exp = 1,// 经验
        Money = 2,// 银两
        FruitStart = 20,
        Fruit1 = 21,// 蟠桃
        Fruit2 = 22,// 真气果
        Food1 = 23,//粮食
        MineralStart = 40,
        Mineral1 = 41,// 星星灵石
        Mineral2 = 42,// 精铁矿
    }

    public enum eMartialItemBigType
    {
        CurrenceType = 0,//货币
        ItemType = 1,//物品
    }

    public enum eMapType
    {
        World = 0,      //大世界
        NULL,       //空世界

        InstanceStart = 10, //从这往下都是副本形式
        Master        = 11, //师门，单人副本
        Challenge     = 12, //挑战副本，获取武林秘籍
        Arena         = 13,     //竞技场

        //TeamInstance  ,    //多人副本
        BattelStart   = 100 ,     //战场
    }

    public enum eMapAttackRule
    {
        None = 0, //没有规则
        AtkOK = 1, //可以攻击
        AtkNO = 2,  //不能攻击
    }

    public enum eBuildType
    {
        Scene,
        Martial,
        Smelt,
        Train,
        Plant,
    }
    /// <summary>
    /// 使用/出售/展示/养成/精炼/镶嵌/合成/寄售/装备
    /// </summary>
    public enum eTipType
    {
        Use = 1,
        Sell = 2,
        Show = 3,
        Cultivate = 4,
        Refine = 5,
        Inlay = 6,
        Combin = 7,
        Consignment = 8,
        Equip = 9,
    }

    public enum eVisitType
    {
        WorldVisit,
        FriendVisit,
    }

    public enum eAchieveEventType
    {
        RoleReachLv = 1,    //1.玩家升至XX等级
        SelectCamp,         //2.加入阵营
        AddFriend,          //3.加好友
        EquipIntensify,     //4.装备养成
        EquipRefine,        //5.装备精练
        InlayGem,           //6.装备镶嵌
        SkillLearn,         //7.学习技能
        KillMonster,        //8.杀怪
    }

    public enum eAchieveType
    {
        Achieve = 0,//历练成就
        AchieveName =1,//称号成就
    }

    public enum eRankType
    {
        Exploit = 0,//战功
        Prestige = 1,//威望
//         KillEnemy = 1,//杀敌,杀玩家
//         BurnFood = 2,//烧粮,杀粮食车
//         Robbery = 3,//劫财,杀军饷车
//         KillArmy = 4,//阻敌,杀兵车
//         Challenge = 5,//挑战，师门挑战
        Max,
    }

    public enum ePatrolNodeType
    {
        None,
        Common,     //普通节点
        Portal,     //可传送点
    }

    public enum eFashionType
    {
        Clothes = 0,        //衣服
        weapon = 1,         //武器
        Pendant = 2,        //挂件
    }

    public enum eFashionChildType
    {
        Profession = 0,       //门派
        Office = 1,           //官职
        Vip = 2,              //vip
        KongWoo = 3,          //江湖
    }

    public enum eUpCheatsType
    {
        Normal,//普通领悟
        Advanced,//潜心领悟
    }

    public enum eForceType
    {
        Attack = 0,           // 长枪兵
        Defense,              // 刀盾兵
    }

    public enum eCarType
    {
        None,
        FoodCar = 1, //粮车
        MoneyCar = 2, //军饷车
    }
}