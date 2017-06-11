using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 消耗MP值
    /// </summary>
    public class MPValue : BaseSkillConsumer
    {
        public override eSkillConsumer ConsumerType { get { return eSkillConsumer.MP; } }

        /// <summary>
        /// 检测消耗
        /// </summary>
        public override bool CheckSkillConsume(RoleActor actor, SkillActive skill)
        {
            if (!base.CheckSkillConsume(actor, skill))
                return false;

            if (actor is NPCInstance)
                return true;

            if (actor.CurMP < skill.LevelData.consume)
            {
                skill.mResult = eSkillResult.MPNotEnough;             
                return false;
            }

            return true;
        }

        /// <summary>
        /// 实际消耗
        /// </summary>
        public override bool SkillConsume(RoleActor actor, SkillActive skill)
        {
            if (!base.SkillConsume(actor, skill))
                return false;

            PlayerInstance player = actor as PlayerInstance;
            var lvData = CSTable.StaticDataManager.PlayerLevel[player.RoleLevel, (byte)player.RolePro];
            float consumeMp = lvData.mp * skill.LevelData.consume;
            player.ChangeMP(-1 * (int)consumeMp);

            return true;
        }
    }
}
