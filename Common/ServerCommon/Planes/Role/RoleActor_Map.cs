using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public partial class RoleActor 
    {
        PlanesInstance mPlanesInstance;
        public PlanesInstance PlanesInstance
        {
            get { return mPlanesInstance; }
            set { mPlanesInstance = value; }
        }

        TriggerInstance mTrigger;
        public TriggerInstance Trigger
        {
            get { return mTrigger; }
            set { mTrigger = value; }
        }

        //protected MapInstance mHostMap = NullMapInstance.Instance;
//         public  MapInstance HostMap
//         {
//             override get { return mHostMap; }
//             set { mHostMap = value; }
//         }

//         MapCellInstance mHostMapCell;
//         public MapCellInstance HostMapCell
//         {
//             get { return mHostMapCell; }
//             set { mHostMapCell = value; }
//         }


        public override void OnEnterMap(MapInstance map)
        {
            mHostMap = map;
            mHostMap.OnRoleEnterMap(this);
        }

        public override void OnLeaveMap()
        {
            mHostMap.OnRoleLeaveMap(this);
            mHostMap = NullMapInstance.Instance;
        }

        public override void OnJumpToMap()
        {
            if (null == mTrigger) return;

            PlanesInstance planes = mHostMap.Planes;
            OnLeaveMap();
            MapInstance map = null;
            ushort mapId = (ushort)mTrigger.TriggerData.mapId;
            map = planes.GetGlobalMap(mapId);
            if (map == null)
            {
                map = Planes.MapInstanceManager.Instance.CreateMapInstance(planes, 0, mapId, null);
                if (map == null)
                {
                    map = Planes.MapInstanceManager.Instance.GetDefaultMapInstance(planes);
                }
                planes.AddGlobalMap(mapId, map);
            }
            SlimDX.Vector3 newPos = new SlimDX.Vector3(mTrigger.TriggerData.targetX, 0, mTrigger.TriggerData.targetZ);
            Placement.SetLocation(ref newPos);
            OnEnterMap(map);

        }
        
    }
}
