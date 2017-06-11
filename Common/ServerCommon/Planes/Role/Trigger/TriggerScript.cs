using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
//     public class TriggerScript
//     {
//         //PlayerInstance mPlayer;
//         //public void Init(PlayerInstance actor)
//         //{
//         //    mPlayer = actor;
//         //}
// 
//         public virtual void OnEnter(TriggerProcessData_Server proData, object data)
//         {
// 
//         }
// 
//         public virtual void OnLeave(TriggerProcessData_Server proData, object data)
//         {
// 
//         }
//     }
// 
// 
//     public class PortalScript : TriggerScript
//     {
//         public override void OnEnter(TriggerProcessData_Server proData, object data)
//         {
//             var player = proData.RoleActor as PlayerInstance;
//             if(player==null)
//                 return;
//             var pData = (KeyValuePair<GameEngine.TriggerSphere, GameEngine.EventPortal>)data;
//             if(pData.Value==null)
//                 return;
// 
//             ushort tarMapId;
//             if(ushort.TryParse(pData.Value.getLevelName(),out tarMapId))
//             {
//                 player.JumpToMap(tarMapId, pData.Value.getPosition().x, 0, pData.Value.getPosition().z, proData.fwd);
//             }            
// 
//         }
// 
//         public override void OnLeave(TriggerProcessData_Server proData, object data)
//         {
// 
//         }
//     }


}
