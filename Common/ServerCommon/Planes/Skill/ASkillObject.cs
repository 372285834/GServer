using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
  
    /// <summary>
    /// 所有技能的基类
    /// </summary>
    public abstract class ASkillObject
    {
        public abstract CSCommon.eSkillType BaseType { get; }
        public abstract int ID { get; }
        public abstract CSTable.ISkillTable Data { get; }
        public abstract int Level { get; }
        public abstract bool Init(int tid, byte lv);
        public abstract void SetOwner(RoleActor owner);
    }

}
