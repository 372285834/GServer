using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class InstanceMap : MapInstance
    {
        public override CSCommon.eMapType MapType
        {
            get { return CSCommon.eMapType.InstanceStart; }
        }

        public bool InitInstanceMap(string mapName)
        {
            return true;
        }

        public override void OnRoleLeaveMap(RoleActor role)
        {
            base.OnRoleLeaveMap(role);
        }
    }

    //单人副本,将来可以优化，只走客户端处理
    public class SingleInstanceMap : InstanceMap
    {
        public override CSCommon.eMapType MapType
        {
            get { return CSCommon.eMapType.InstanceStart; }
        }
    }

}
