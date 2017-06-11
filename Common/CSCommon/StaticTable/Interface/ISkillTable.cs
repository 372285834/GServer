using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSTable
{
    public interface ISkillTable
    {
        int id { get; }
        string name { get; }
        int type { get; }
        int maxLv { get; }
        string icon { get; }
    }

    public interface ISkillLevelTable
    {
        int id { get; }
        int level { get; }
        string cost { get; }
        string des { get; }
        string desParam { get; }
    }

}
