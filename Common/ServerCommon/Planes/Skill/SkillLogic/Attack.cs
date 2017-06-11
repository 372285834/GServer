using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 普遍攻击技能
    /// </summary>
    public class Attack : BaseSkillLogic
    {
        public override eSkillLogic LogicType { get { return eSkillLogic.Attack; } }

        /// <summary>
        /// 释放动作
        /// </summary>
        public override bool Cast(RoleActor target, SkillActive skill)
        {
            if (!base.Cast(target, skill))
                return false;

            RoleActor owner = skill.Owner;
            if (null == owner || null == target || owner == target)
                return false;

            AttackImpact impact = new AttackImpact();
            impact.Init(skill, target);
            impact.Run();
            if (impact.HitType == eHitType.Miss)
            {
                return false;
            }
            switch( owner.GameType )
		    {
			    case eActorGameType.Player:
				    {

                    }
                    break;

			    case  eActorGameType.Npc:
				    {
					    
				    }
				    break;
		    }

            return true;
        }

        /// <summary>
        /// 初始化回调
        /// </summary>
        public override bool OnInit(RoleActor target, SkillActive skill)
        {
            if (!base.OnInit(target, skill))
                return false;

            return true;
        }

        /// <summary>
        /// 结束回调
        /// </summary>
        public override bool OnEnd(RoleActor target, SkillActive skill)
        {
            if (!base.OnEnd(target, skill))
                return false;

            return true;
        }
    }
}
