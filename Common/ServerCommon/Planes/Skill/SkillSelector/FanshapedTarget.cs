using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;
using SlimDX;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 扇形区域目标选择
    /// </summary>
    public class FanshapedTarget : BaseSkillSelector
    {
        public override eSkillSelector SelectorType { get { return eSkillSelector.Fanshaped; } }

        /// <summary>
        /// 实际选择目标
        /// </summary>
        public override bool SelectTarget(RoleActor caster, SkillActive skill, ref List<RoleActor> targets)
        {
            if (!base.SelectTarget(caster, skill, ref targets))
                return false;

            List<RoleActor> roleList = new List<RoleActor>();
            UInt32 actorTypes = (1 << (Int32)eActorGameType.Player) | (1 << (Int32)eActorGameType.Npc) | (1 << (Int32)eActorGameType.PlayerImage);
            var pos = caster.GetPosition();
            if (!caster.HostMap.TourRoles(ref pos, skill.RangeMax, actorTypes, roleList))
                return false;

            foreach (var tar in roleList)
            {
                Vector3 dir = Util.DirToVector(caster.GetDirection());
                Vector3 toTarDir = tar.GetPosition() - caster.GetPosition();
                float angle = Util.Angle(dir, toTarDir);
                if (Math.Abs(angle) <= skill.SkillData.selparam1 && Util.DistanceH(tar.GetPosition(), caster.GetPosition()) <= skill.RangeMax)
                    targets.Add(tar);
            }

            return true;
        }

        /// <summary>
        /// 检查目标是否有效
        /// </summary>
        public override bool TargetFilter(RoleActor caster, SkillActive skill, RoleActor target, eRelationType relation)
        {
            if (!base.TargetFilter(caster, skill, target, relation))
                return false;

            return true;
        }
    }
}
