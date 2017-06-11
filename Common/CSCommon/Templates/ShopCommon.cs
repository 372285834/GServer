using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    public class ShopItemInfo
    {
        int mId;
        [System.ComponentModel.DisplayName("1.模板Id")]
        [ServerFrame.Config.DataValueAttribute("Id")]
        public int Id
        {
            get { return mId; }
            set { mId = value; }
        }

        eCurrenceType mCurrencey ;
        [System.ComponentModel.DisplayName("2.货币类型")]
        [ServerFrame.Config.DataValueAttribute("Currencey")]
        public eCurrenceType Currencey
        {
            get { return mCurrencey; }
            set { mCurrencey = value; }
        }

        int mPrice;
        [System.ComponentModel.DisplayName("3.价格")]
        [ServerFrame.Config.DataValueAttribute("Price")]
        public int Price
        {
            get { return mPrice; }
            set { mPrice = value; }
        }

        float mDiscount = 1;
        [System.ComponentModel.DisplayName("4.折扣")]
        [ServerFrame.Config.DataValueAttribute("Discount")]
        public float Discount
        {
            get { return mDiscount; }
            set { mDiscount = value; }
        }

        string mRmbIcon;
        [System.ComponentModel.DisplayName("5.元宝路径")]
        [ServerFrame.Config.DataValueAttribute("RmbIcon")]
        public string RmbIcon
        {
            get { return mRmbIcon; }
            set { mRmbIcon = value; }
        }

        bool mIsHot = false;
        [System.ComponentModel.DisplayName("6.是否热销")]
        [ServerFrame.Config.DataValueAttribute("IsHot")]
        public bool IsHot
        {
            get { return mIsHot; }
            set { mIsHot = value; }
        }

        public byte Index;
    }

    [ServerFrame.Editor.CDataEditorAttribute(".shop")]
    [ServerFrame.Editor.Template]
    public class ShopCommon : ServerFrame.CommonTemplate<ShopCommon>
    {
        List<ShopItemInfo> mItemList = new List<ShopItemInfo>();
        [System.ComponentModel.Category("1.热销")]
        [System.ComponentModel.DisplayName("<1>热销商店")]
        [ServerFrame.Config.DataValueAttribute("ItemList")]
        public List<ShopItemInfo> ItemList
        {
            get { return mItemList; }
            set { mItemList = value; }
        }

        List<ShopItemInfo> mDrugList = new List<ShopItemInfo>();
        [System.ComponentModel.Category("2.药剂")]
        [System.ComponentModel.DisplayName("<1>药剂商店")]
        [ServerFrame.Config.DataValueAttribute("DrugList")]
        public List<ShopItemInfo> DrugList
        {
            get { return mDrugList; }
            set { mDrugList = value; }
        }

        List<ShopItemInfo> mMountList = new List<ShopItemInfo>();
        [System.ComponentModel.Category("3.坐骑")]
        [System.ComponentModel.DisplayName("<1>坐骑商店")]
        [ServerFrame.Config.DataValueAttribute("MountList")]
        public List<ShopItemInfo> MountList
        {
            get { return mMountList; }
            set { mMountList = value; }
        }

        List<ShopItemInfo> mRmbList = new List<ShopItemInfo>();
        [System.ComponentModel.Category("4.元宝")]
        [System.ComponentModel.DisplayName("<1>元宝商店")]
        [ServerFrame.Config.DataValueAttribute("RmbList")]
        public List<ShopItemInfo> RmbList
        {
            get { return mRmbList; }
            set { mRmbList = value; }
        }

        public override string GetFilePath()
        {
            return "Common/Shop.shop";
        }
//         public override void OnInitIndex()
//         {
//             for (int i = 0; i < this.mItemList.Count; i++)
//             {
//                 this.mItemList[i].Index = (byte)i;
//             }
//         }
        public ShopItemInfo GetByIndex(int index)
        {
            return this.mItemList.Find(x => x.Index == index);
        }
        
    }
}
