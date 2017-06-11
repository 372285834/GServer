using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSTable
{
    public interface IBuildBase
    {
        //等级;
        int id { get; }
        //产出;		
        string output { get; }
        //升级消耗;		
        string cost { get; }
    }

}
