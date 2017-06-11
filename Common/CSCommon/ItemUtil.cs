using CSCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSTable
{
    public static class ItemUtil
    {
        public static CurrencePath GetCurrencePath(eCurrenceType type)
        {
            foreach (var i in ItemCommon.Instance.CurrencePathList)
            {
                if (type == i.Type)
                {
                    return i;
                }
            }
            return null;
        }

        public static IItemBase GetItem(int id)
        {
            IItemBase item = null;
            item = CSTable.StaticDataManager.ItemTpl[id];
            if (item == null)
            {
                item = CSTable.StaticDataManager.ItemEquip[id];
            }
            if (item == null)
            {
                item = CSTable.StaticDataManager.ItemFashion[id];
            }
            if (item == null)
            {
                item = CSTable.StaticDataManager.ItemPackage[id];
            }
            if (item == null)
            {
                item = CSTable.StaticDataManager.ItemGift[id];
            }
            if (item == null)
            {
                item = CSTable.StaticDataManager.ItemGem[id];
            }
            return item;
        }
        public static ItemEquipLevelData GetEquipLvTpl(int id, int tid, short lv)
        {
            ItemEquipLevelData item = null;
            item = CSTable.StaticDataManager.ItemEquipLevel[id, tid, (int)lv];
            return item;
        }

        public static ItemEquipRefineLvData GetEquipRefineLvTpl(int id, short lv)
        {
            ItemEquipRefineLvData item = null;
            item = CSTable.StaticDataManager.ItemEquipRefineLv[id, (int)lv];
            return item;
        }

        public static IAchieveBase GetAchieve(int id)
        {
            IAchieveBase item = null;
            item = CSTable.StaticDataManager.AchieveTpl[id];
            if (item == null)
            {
                item = CSTable.StaticDataManager.AchieveName[id];
            }
            return item;
        }
    }
}
