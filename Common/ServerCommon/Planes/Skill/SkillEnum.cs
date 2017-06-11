using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public enum eSkillAreaType
    {
        One,            //0.单体攻击
        SelfCircle,     //1.自身圆形范围攻击
        TargetCircle,   //2.目标圆形范围攻击
    }

    
    public enum eBuffReplaceMode
    {
        ReplaceLevel = 0,  //等级优先
        ReplaceTime = 1,   //时间优先
        NoReplace = 2,  //不发生替换
        StackNum = 3,  //数量叠加
    }

    public enum eRelationType
    {
        None,
        Self = 1,       //自身
        Enemy = 2,      //敌对
        Neutrality = 3, //中立
        TeamMember = 4, //队员
        Friend = 5,     //友好
        Mod = 6,        //怪物
        Faction = 7,    //帮众
        Couple = 8,     //夫妻
        Max
    };
}
