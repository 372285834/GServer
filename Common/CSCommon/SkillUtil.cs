using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSTable
{
    public static class SkillUtil
    {
        public static ISkillTable GetSkillTpl(int id)
        {
            ISkillTable tpl = null;
            tpl = CSTable.StaticDataManager.SkillActive[id];
            if (tpl == null)
            {
                tpl = CSTable.StaticDataManager.SkillPassive[id];
            }
            return tpl;
        }

        public static ISkillLevelTable GetSkillLevelTpl(int id, int lv)
        {
            ISkillLevelTable tpl = null;
            tpl = CSTable.StaticDataManager.SkillLevel[id, lv];
            if (tpl == null)
            {
                tpl = CSTable.StaticDataManager.SkillPassiveLevel[id, lv];
            }

            if (tpl == null)
            {
                tpl = CSTable.StaticDataManager.Cheats[id, lv];
            }

            return tpl;
        }

    }
}
