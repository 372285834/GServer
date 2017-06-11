using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class GemBag : BagBase
    {
        public GemBag()
        {
            InventoryType = CSCommon.eItemInventory.GemBag;
        }
    }


    public class EquipGemBag : BagBase
    {
        public EquipGemBag()
        {
            InventoryType = CSCommon.eItemInventory.EquipGemBag; 
        }

    }



}
