using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class FashionBag : BagBase
    {
        public FashionBag()
        {
            InventoryType = CSCommon.eItemInventory.FashionBag; 
        }

        public bool CreateFashionToBag(RoleActor role, int templateId)
        {
            Item item = Item.DangerousCreateItemById(role, templateId, 1);
            bool result = this.AutoAddItem(item);
            return result;
        }


    }
}
