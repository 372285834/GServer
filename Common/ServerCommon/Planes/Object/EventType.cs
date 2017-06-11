using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public enum EventType
    {
        None,
        Kill,          //击杀玩家或怪物
        CollectItem,   //收集物品
        SelectCamp,    //选择阵营
        UpLevel,        //角色升级
        MAX
    }
}
