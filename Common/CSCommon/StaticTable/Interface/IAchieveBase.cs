using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSTable
{
    public interface IAchieveBase
    {
        //Id
        int id { get; }
        //类型	
        int atype { get; }
        //类型	
        int type { get; }
        //目标值		
        int targetNum { get; }
        //目标
        string target { get; }
    }

}
