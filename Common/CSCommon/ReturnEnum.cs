using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    public enum eRet_LoginAccount : sbyte
    {
        NoAccount = -1,
        PasswordError = -2,
        AccountHasLogin = -3,
        Succeed = 1,
    }

    public enum eRet_TryCreatePlayer : sbyte
    {
        NetError = -1,
        SameName = -2,
        Succeed = 1,
    }

    public enum eRet_EnterTrigger : sbyte
    {
        EnterMap = 0,
        NotInArea = 1,
        NotEnable = 2,
        ActorIsNull = 3,
        NotFindActor = 4,
        ActorIsNotPlayer = 5,
        NotFindData,
        NotFindTargetMap,
    }

    public enum eRet_GotoMap : sbyte
    {
        CreatePlayerFailed = -11,
        NoPlaneConnect =-10,
        NoAccountId = -9,
        NoCltLinker = -8,
        GetMapInitFailed = -7,
        SameMap = -6,
        NoMap = -5,
        NullMapOrNoPlane = -4,
        FailedEnterMap = -3,
        NoPlayerData = -2,
        NoConnectInfo = -1,
        SamePlane = 1,
        OtherPlane = 2,
        EnterMap = 3,
    }

    public enum eRet_SetGuildPost : sbyte
    {
        NoRole = -1,
        NoTargetRole = -2,
        RoleNoGuild = -3,
        TargetRoleNoGuild = -4,
        DifferentGuild = -5,
        RoleNoPower = -6,
        RolePostLowTarget = -7,
        RolePostLowSet = -8,
        TargetPostEqualSet = -9,
        OverMaxNum = -10,
        Succeed = 1,
    }

    public enum eRet_KickedOutGuild : sbyte
    {
        NoRole = -1,
        NoTargetRole = -2,
        RoleNoGuild = -3,
        TargetRoleNoGuild = -4,
        RoleNoPower = -5,
        RolePostLowTarget = -6,
        Succeed = 1,
    }

    public enum eRet_OperateGuildAsk : sbyte
    {
        NoRole = -1,
        RoleNoGuild = -2,
        RoleNoPower = -3,
        YetOperate = -4,
        NoTargetRole = -5,
        TargetRoleHasGuild = -6,
        OverMaxNum = -7,
        Succeed = 1,
    }

    public enum eRet_AskToGuild : sbyte
    {
        NoRole = -1,
        NoGuild = -2,
        RoleHasGuild = -3,
        OverMaxNum = -4,
        Succeed = 1,
    }

    public enum eRet_DissolveGuild : sbyte
    {
        NoRole = -1,
        RoleNoGuild = -2,
        RoleIsNotBangZhu = -3,
        Succeed = 1,
    }

    public enum eRet_GetGuilds : sbyte
    {
        NoRole = -1,
        ReturnGuilds = 1,
        NoGuild = -2,
        ReturnGuildMembers = 2,
    }

    public enum eRet_CreateGuild : sbyte
    {
        LessLevel = -1,
        LessRmb = -2,
        NoRole = -3,
        RoleHasGuild = -4,
        SameGuildName = -5,
        Succeed = 1,
    }

    public enum eRet_LeaveGuild : sbyte
    {
        NoRole = -1,
        RoleNoGuild = -2,
        RoleIsBangZhu = -3,
        Succeed = 1,
    }

    public enum eRet_InviteToGuild : sbyte
    {
        NoRole = -1,
        NoTargetRole = -2,
        RoleNoGuild = -3,
        TargetRoleHasGuild = -4,
        RoleNoPower = -5,
        TargetRoleNotInPlay = -6,
        OverMaxNum = -7,
        Succeed = 1,
    }

    public enum eRet_OperateInvite : sbyte
    {
        NoRole = -1,
        NoTargetRole = -2,
        RoleHasGuild = -3,
        TargetRoleNoGuild = -4,
        OverMaxNum = -5,
        Succeed = 1,
    }


    public enum eRet_GetRoleCreateInfo : sbyte
    {
        OK_NPC,
        OK_Player,
        OK_PlayerImage,
        NotFindActor,
        OverDistance,
    }


    public enum eRet_TransferGuildBangZhu : sbyte
    {
        NoRole = -1,
        NoTargetRole = -2,
        RoleNoGuild = -3,
        TargetRoleNoGuild = -4,
        RoleIsNotBangZhu = -5,
        DifferentGuild = -6,
        Succeed = 1,
    }

    public enum eRet_DonateGold : sbyte
    {
        LessGold = -1,
        NoRole = -2,
        RoleNoGuild = -3,
        Succeed = 1,
    }

    public enum eRet_IssueGold : sbyte
    {
        NoRole = -1,
        RoleNoGuild = -2,
        RoleNoPower = -3,
        GuildLessGold = -5,
        Succeed = 1,
    }

    public enum eRet_ConsignItem : sbyte
    {
        LessTemp = -1,
        LessRent = -2,
        LessStack = -3,
        NoRole = -4,
        OverMaxNum = -5,
        DBError = -6,
        Succeed = 1,
    }

    public enum eRet_BuyConsignItem : sbyte
    {
        LessMoney = -1,
        NoItem = -2,
        NoRole = -3,
        Succeed = 1,
    }

    public enum eRet_BuyFashion : sbyte
    {
        NotInShop = -1,
        NoTemplate = -2,
        Has = -3,
        LessMoney = -4,
        CreateFailed = -5,
        NoShop = -6,
        ProError = -7,
        LessLv = -8,
        Succeed = 1,
    }

    public enum eRet_BuyShopItem : sbyte
    {
        NoShop = -1,
        NotInShop = -2,
        NoTemplate = -3,
        UpNum = -4,//数量超过最大堆叠
        BagNoPos = -5,
        LessMoney = -6,
        CreateFailed = -7,
        Succeed = 1,
    }

    public enum eRet_CombineItem : sbyte
    {
        BagNoPos = -1,
        NoCombineTpl = -2,
        IdNumError = -3,
        LessMaterial = -4,
        LessMoney = -5,
        CreateFailed = -6,
        Succeed = 1,
    }

    public enum eRet_EquipIntensify : sbyte
    {
        NoEquip = -1,
        NoEquipTpl = -2,
        OverPlayerLv = -3,
        MaxLv = -4,
        NoEquipLvTpl = -5,
        NoOreTpl = -6,
        LessMoneyOne = -7,
        LessOre = -8,
        Succeed = 1,
    }

    public enum eRet_EquipRefine : sbyte
    {
        NoEquip = -1,
        NoEquipTpl = -2,
        OverPlayerLv = -3,
        MaxLv = -4,
        NoRefineTpl = -5,
        NoMaterialTpl = -6,
        BagNoMaterial = -7,
        RemoveItemFailed = -8,
        Succeed = 1,
    }

    public enum eRet_InlayGem : sbyte
    {
        NoGem = -1,
        NotGem = -2,
        NoPos = -3,
        Succeed = 1,
    }

    public enum eRet_RemoveGem : sbyte
    {
        NoGem = -1,
        BagNoPos = -2,
        NoItemCom = -3,
        LessMoney = -4,
        AddToBagFailed = -5,
        Succeed = 1,
    }

    public enum eRet_GemCombine : sbyte
    {
        LessCount = -1,
        NotCombine = -2,
        Succeed = 1,
    }

    public enum eRet_AddSocial : sbyte
    {
        NoRole = -1,
        NoOther = -2,
        RoleIsOther = -3,
        IsSocial = -4,
        NotFriend = -5,
        AddSucceed = 1,
        AskSucceed = 2,
    }

    public enum eRet_OperateAddSocial : sbyte
    {
        NoRole = -1,
        NoOther = -2,
        RoleIsOther = -3,
        IsSocial = -4,
        TypeError = -5,
        Succeed = 1,
    }

    public enum eRet_RemoveSocial : sbyte
    {
        NoRole = -1,
        NoOther = -2,
        NotSocial = -3,
        RemoveError = -4,
        Succeed = 1,
    }

    public enum sRet_SendGift : sbyte
    {
        NoGftTpl = -1,
        NotGiftType = -2,
        ListNoGift = -3,
        LessMoney = -4,
        NoRole = -5,
        NoOther = -6,
        NotCouple = -7,
        Succeed = 1,
    }

    public enum eRet_Team : sbyte
    {
        NoRole = -1,
        NoOther = -2,
        RoleHasTeam = -3,
        NewTeamError = -4,
        OverTeamCount = -5,
        TeamIsNull = -6,
        OtherHasTeam = -7,
        HeaderIsNull = -8,
        RoleIsNotHeader = -9,
        DiffTeam = -10,
        OtherNoTeam = -11,
        OtherIsNotHeader = -12,
        RoleNoTeam = -13,
        Succeed = 1,
    }

    public enum eRet_UseItem : sbyte
    {
        BagNull = -1,
        ItemNull = -2,
        NoCount = -3,
        NoRole = -4,
        NotUse = -5,
        TplNull = -6,
        MethodNull = -7,
        HpMax = -8,
        MpMax = -9,
        CdIng = -10,
        Succeed = 1,
    }

    public enum eRet_UpSkillLv : sbyte
    {
        NoSkill = -1,
        LessRoleLv = -2,
        MaxLv = -3,
        LessMoney = -4,
        NoRoleCommon = -5,//没有角色公共筋脉模板
        UpNum = -6,//经脉突破次数用完
        NoSkillLvTpl = -7,
        Failure = -8,
        NotLearn = -9,
        Succeed = 1,
    }

    public enum eRet_WearEquip : sbyte
    {
        NoItem = -1,
        NoTpl = -2,
        NotEquip = -3,
        Succeed = 1,
    }

    public enum eRet_SellItem : sbyte
    {
        LessCount = -1,
        NoTpl = -2,
        NotSell = -3,
        LessMoney = -4,
        DelItemError = -5,
        Succeed = 1,
    }

    public enum eRet_GetMartialInfo : sbyte
    {
        NoBuild = -1,
        NoOpen = -2,
        NoOutPut = -3,
        BagFull = -4,
        Succeed = 1,
    }

    public enum eRet_UpMartialLevel : sbyte
    {
        NoBuild = -1,
        NoOpen = -2,
        OverMartialLv = -3,
        OverMaxLv = -4,
        NoTemplate = -5,
        TplIsError = -6,
        LessCostMoney = -7,
        OverRoleLv = -8,
        Succeed = 1,
    }

    public enum eRet_Visit : sbyte
    {
        NoRole = -1,
        TypeError = -2,
        IdError = -3,
        Visited = -4,
        NoOther = -5,
        Succeed = 1,
    }

    public enum eRet_GetDayDataReward : sbyte
    {
        EverGet = -1,
        CanotGet = -2,
        TplError = -3,
        BagFull = -4,
        AddBagError = -5,
        Succeed = 1,
    }

    public enum eRet_GetAchieveReward : sbyte
    {
        IdError = -1,
        YetGet = -2,
        NoFinish = -3,
        Succeed = 1,
    }

    public enum eRet_OtherPlane_EnterMap : sbyte
    {

    }

    public enum eRet_BarrackAddForce : sbyte
    {
        CoolDown = -1,
        DayNumMax = -2,
        CityNumMax = -3,
        Succeed = 1,
    }

    public enum eRet_BarrackUpgradeLv : sbyte
    {
        GoldNotEnough = -1,
        RoleLevelLimit = -2,
        MaxLevelLimit = -3,
        Succeed = 1,
    }

    public enum eRet_BarrackBuyEfficiencyItem : sbyte
    {
        GoldNotEnough = -1,
        Succeed = 1,
    }
}
