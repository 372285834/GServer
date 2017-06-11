using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    
    /// <summary>
    ///竞技场 
    /// </summary>
    public class ArenaInstance : SingleInstanceMap
    {
        public override CSCommon.eMapType MapType
        {
            get { return CSCommon.eMapType.Arena; }
        }

        public override void OnInit()
        {
            foreach (var NpcData in MapInfo.MapDetail.NpcList)
            {
                PlayerImage.CreatePlayerImage(NpcData, this, Owner.Id);
            }
        }

        public bool IsReady;
        public override void OnClientEnterMapOver(PlayerInstance role)
        {
            IsReady = true;
        }

        public override CSCommon.eMapAttackRule CanAttack(RoleActor atk, RoleActor def)
        {
            return CSCommon.eMapAttackRule.AtkOK;
        }
    }
    

}

