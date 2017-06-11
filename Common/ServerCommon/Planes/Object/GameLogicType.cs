using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public enum eGameLogicType
    {
        NPCLogic,           //NPC行为逻辑
        PlayerLogic,        //玩家行为逻辑
        SkillSelector,      //技能目标选择逻辑
        SkillChecker,       //技能检测逻辑
        SkillConsumer,      //技能消耗逻辑
        SkillLogic,         //技能释放逻辑
        InstanceLogic,      //副本目标逻辑
        TaskLogic,          //任务逻辑
        AchieveLogic,       //成就逻辑
        MAX
    }

    public enum ePlayerLogicType
    {
        None,
        Hook,               //挂机
        Robot,              //机器人
    }

    public enum eNPCLogicType
    {
        None,
        Common,             //普通NPC
        PlayerImage,        //玩家镜像
        SilverCart,         //镖车
    }

    public enum eSkillSelector
    {
        None,
        Single,             //单个目标
        Circle,             //圆形区域
        Rectangle,          //矩形区域
        Fanshaped,          //扇形区域
    }

    public enum eSkillChecker
    {
        None,
        Common,             //通用
        Gain,               //增益
    }

    public enum eSkillConsumer
    {
        None,
        MP,                 //法力值
    }

    public enum eSkillLogic
    {
        None,
        Attack,             //普通攻击
        Heal,               //治疗
        Effect,             //buff效果
    }

    public enum eInstanceLogic
    {
        None,
        KillMonster,        //击杀怪物
        CollectItem,        //收集物品
        ProtectTarget,      //保护目标
    }

//     public enum eTaskLogic
//     {
//         None,
//         KillMonster,        //击杀怪物
//         SelectCamp,         //选择阵营
//     }
}
