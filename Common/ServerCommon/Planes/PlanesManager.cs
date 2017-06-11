using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class PlanesManager
    {
        Dictionary<ulong, PlanesInstance> mAllPlanesInstance = new Dictionary<ulong, PlanesInstance>();
        public Dictionary<ulong, PlanesInstance> AllPlanesInstance
        {
            get { return mAllPlanesInstance; }
        }

        //如果PlanesId为Guid.Empty那么说明角色在副本，不在真正的位面内
        public PlanesInstance GetPlanesInstance(CSCommon.Data.PlanesData planesData)
        {
            PlanesInstance planes;
            if (AllPlanesInstance.TryGetValue(planesData.PlanesId, out planes))
                return planes;
            planes = new PlanesInstance(planesData);
            AllPlanesInstance.Add(planesData.PlanesId, planes);
            return planes;
        }
    }
}
