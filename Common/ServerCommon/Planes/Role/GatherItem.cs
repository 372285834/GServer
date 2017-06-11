using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class GatherItem : RoleActor
    {
        CSCommon.Data.GatherData mGatherData = new CSCommon.Data.GatherData();
        public CSCommon.Data.GatherData GatherData
        {
            get { return mGatherData; }
        }

        public override ulong Id
        {
            get
            {
                return mGatherData.RoleId;
            }
        }

        //public override CSCommon.Data.RoleTemplate RoleTemplate
        //{
        //    get { return mGatherData.Template; }
        //}

        protected override void OnPlacementUpdatePosition(ref SlimDX.Vector3 pos)
        {
            base.OnPlacementUpdatePosition(ref pos);

            mGatherData.Position = pos;
        }

        public override void OnEnterMap(MapInstance map)
        {
            base.OnEnterMap(map);
            //把角色放到SceneGraph里面去

            if (!map.GatherDictionary.ContainsKey(this.Id))
                map.GatherDictionary.Add(this.Id, this);
        }

        public override void OnLeaveMap()
        {
            if (!HostMap.IsNullMap)
                HostMap.GatherDictionary.Remove(this.Id);
            base.OnLeaveMap();
        }
    }
}
