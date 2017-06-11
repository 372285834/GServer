using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class EquipBag : BagBase
    {
        public EquipBag()
        {
            InventoryType = CSCommon.eItemInventory.EquipBag; 
        }
        public void WearEquip(UInt16 index, Item item)
        {
            OnPutItem(index, item);
        }

        protected override void OnPutItem(UInt16 index, Item item)
        {
            this[index] = item;
            var player = mHostRole as PlayerInstance;
            if (player != null)
            {
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                pkg.SetSinglePkg();
                Wuxia.H_RpcRoot.smInstance.RPC_ItemAdd2Bag(pkg, item.ItemData, (byte)InventoryType, index);
                pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
            }
        }

        public void CreateEquipToBag(RoleActor owner, UInt16 index, int templateId)
        {
            if (templateId <= 0)
            {
                return;
            }
            CSTable.IItemBase template = CSTable.ItemUtil.GetItem(templateId);
            if (template == null)
            {
                Log.Log.Item.Print("CreateEquipToBag: template is null");
                return;
            }
            var item = Item.DangerousCreateItemById(owner, templateId, 1);
            if (item == null)
            {
                return;
            }
            this[index] = item;
        }

    }
}
