using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{  
    /// <summary>
    /// 师门
    /// </summary>
    public class MasterInstance : SingleInstanceMap
    {
        public override CSCommon.eMapType MapType
        {
            get { return CSCommon.eMapType.Master; }
        }

        public override bool IsSavePos(PlayerInstance role)
        {
            if (role.PlayerData.RoleDetail.Camp == (byte)CSCommon.eCamp.None)
            {
                return true;
            }

            return false;
        }

        public override void OnRoleEnterMap(RoleActor role)
        {
            base.OnRoleEnterMap(role);

        }
    }
    

}
