using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 通用检测
    /// </summary>
    public class CommonChecker : BaseSkillChecker
    {
        public override eSkillChecker CheckerType { get { return eSkillChecker.Common; } }

        /// <summary>
        /// 技能预先选择目标(玩家点击目标)
        /// </summary>
        public override bool PreSelectTarget(RoleActor actor, SkillActive skill, RoleActor target)
        {
            if (!base.PreSelectTarget(actor, skill, target))
                return false;

            if (null != target)
                skill.mTargetId = target.Id;

            return true;
        }

        /// <summary>
        /// 检查技能条件
        /// </summary>
        public override bool CheckSkillCondition(RoleActor actor, SkillActive skill)
        {
            if (!base.CheckSkillCondition(actor, skill))
                return false;

            RoleActor target = actor.GetTarget(skill.mTargetId);
            if (null == target)
            {
                skill.mResult = eSkillResult.NotTarget;
                return false;
            }

            if (actor.HostMap.MapInstanceId != target.HostMap.MapInstanceId)
            {
                Log.Log.Server.Info("Error, actor.HostMap.MapInstanceId != target.HostMap.MapInstanceId");
                return false;
            }

            if (!actor.CanAttack(target))
            {
                skill.mResult = eSkillResult.NoAttack;
                return false;
            }

            if (!skill.SkillCD.IsCoolDown() || !actor.SkillCD.IsCoolDown())
            {
                skill.mResult = eSkillResult.NotCoolDown;
                return false;
            }

            if (skill.mDistance > skill.RangeMax + GameSet.Instance.SkillRangeSync)
            {
                skill.mResult = eSkillResult.DistanceToFar;
                return false;
            }

            return true;
        }
    }

}
