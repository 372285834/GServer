using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    
    public enum eSkillType
    {
        Unknow = 0,
        Normal = 1,         //普攻
        Active = 2,         //主动技能
        Cheats = 3,         //秘籍
        Hide = 4,           //隐藏技能
        BodyChannel = 5,    //经脉
    }


    /// <summary>
    /// 技能使用结果
    /// </summary>
    public enum eSkillResult
    {
        OK,
        CastFailed,
        FaceFailed,

        HasDie,
        NotTarget,
        NotFindSkill,
        NotActive,
        DistanceToFar,
        FightStateFailed,
        PKProtected, //PK保护

        MPNotEnough, //法力值不足

        NoAttack, //不能攻击
        NoAttackMap, //当前地图不能攻击
        NoAttackTarget, //目标不能攻击

        NotCoolDown,
    }

    /// <summary>
    /// 主角技能序号
    /// </summary>
    public enum eSkillIndex
    {
        Unknown,
        Normal,
        Skill1,
        Skill2,
        Buff,
    }

    public enum eHitType
    {
        Hit,        //普通命中
        Miss,       //未命中
        Crit,       //暴击
        Block,      //闪避
        Deadly,     //致命
    }

    public enum eSkillTargetType
    {
        Enemy,      //0.敌人
        Self,       //1.自己
        Friend,     //2.队友
    }

    public enum eSkillAttrIndex //技能属性ID， 前26确保与装备属性一一对应
    {
        None,

        Power = 1,              //内功值
        Body = 2,               //外功值
        Dex = 3,                //身法值
        HP = 4,                 //生命值
        HPRate = 5,             //生命百分比
        MP = 6,                 //内力值
        Atk = 7,                //攻击
        Def_Gold,               //金防御
        Def_Wood,               //木防御
        Def_Water,              //水防御
        Def_Fire,               //火防御
        Def_Earth,              //土防御
        Def_All,                //全防御
        Crit = 15,              //暴击值
        CritRate = 16,          //暴击百分比
        CritDef = 17,           //暴抗值
        CritDefRate = 18,       //暴抗百分比
        Hit = 19,               //命中值
        Dodge = 20,             //闪避值
        MoveSpeed = 21,         //移速值
        UpExpRate = 22,         //提升杀怪经验百分比
        DeadlyHitRate = 23,     //致命一击百分比
        UpHurt = 24,            //伤害加深百分比
        DownHurt = 25,          //伤害减免百分比
        UnusualDefRate = 26,    //异常状态抗性百分比

        Level, //等级
        MaxHP, //生命上限
        MaxMP, //内力上限
        MaxHPRate, //生命上限百分比
        SpellRate, //技能加成
        AtkRate, //攻击百分比
        DefRate, //防御百分比
        DodgeRate, //闪避百分比
        HitRate, //命中百分比
        SpeedRate, //移速百分比,
        Block, //格挡
        BlockRate, //格挡率
        HPRecover, //生命回复
        MPRecover, //内力回复
        DeadlyHit, //致命一击
        DamageReflect, //伤害反射百分比

        MAX,
    }

    public enum eBuffLogicType //buff逻辑类型
    {
        None,
        Static,
        Continual,
        Fixed,
        MAX,
    }

    //buff状态类型
    public enum eBuffStatusType
    {
        None = 0,

        眩晕,
        击跪,
        击飞,
        冰冻,
        中毒,
        回血,
        加移速,
        减移速,
        提升攻击,
        提高生命上限,
        无视防御,
        减伤护盾,
        破防,
        减目标攻击力,
        降低闪避,
        降低命中,
        免疫控制,
        解除控制,
        击倒,

        无敌,

        MAX
    }

    //免疫类型
    public enum eImmunityType
    {
        All = 0,

        眩晕,
        击跪,
        击飞,

        MAX = 20
    }

    //buff效果类型 参考 BuffLevel 表 buff效果类型 切页
    public enum eBuffEffectType
    {
        Unknown = 0,

        影响生命,
        影响生命百分比,
        影响生命上限百分比,
        影响技能加成,
        影响攻击力百分比,
        影响防御百分比,
        影响闪避百分比,
        影响命中百分比,
        影响移动速度百分比,
        免疫控制,
        解除控制,
        吸收伤害,
        击跪,
        眩晕,
        击飞,
        击倒,

        MAX
    }

    //被动效果,参考 SkillPassiveLevel表 效果类型 切页
    public enum ePassiveEffect
    {
        Unknown = 0,
        伤害反射百分比 = 1,
        提升伤害增加百分比 = 2,
        提升伤害减免百分比 = 3,
        提升致命一击几率 = 4,
        提升内功 = 5,
        提升外功 = 6,
        提升身法 = 7,
        提升最大生命值 = 8,
        提升最大法力值 = 9,
        提升攻击力 = 10,
        提升命中值 = 11,
        提升闪避值 = 12,
        提升金防御 = 13,
        提升木防御 = 14,
        提升水防御 = 15,
        提升火防御 = 16,
        提升土防御 = 17,
        提升全防御 = 18,
        提升移动值 = 19,
        提升异常抗性百分比 = 20,
        提升暴击值 = 21,
        提升暴抗值 = 22,

        MAX
    }

    public enum eSkillStatus //技能状态
    {
        Valid,         //有效
        Invalid,       //无效
        Invisible      //隐藏
    };

    public enum eSkillStep //技能释放阶段
    {
        Init,          //初始化
        Check,         //检查条件
        Delay,         //延迟释放
        Consume,       //消耗
        Target,        //实际行动
    };

    public enum eFlutterInfoType //技能飘字信息类型
    {
        None = -1,
        Hit = 0,    //普通命中
        Miss,       //未命中
        Crit,       //暴击
        Block,      //闪避
        Deadly,     //致命
        AddHp,      //吃药加血
        BuffAddHp,  //Buff加血
        BuffReduceHp,  //Buff减血
        Shield,     //护盾吸收
    }

    public enum eParamType //加成参数类型
    {
        Value = 1, //按数值
        Scale = 2, //按比例
        Chance = 3, //按几率
    }
}
