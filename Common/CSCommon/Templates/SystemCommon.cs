using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    public class TipInfoData
    {
        eTipType mTipType = eTipType.Use;
        [System.ComponentModel.DisplayName("<1>按钮类型")]
        [ServerFrame.Config.DataValueAttribute("TipType")]
        public eTipType TipType
        {
            get { return mTipType; }
            set { mTipType = value; }
        }
        string name = "使 用";
        [System.ComponentModel.DisplayName("<2>按钮名")]
        [ServerFrame.Config.DataValueAttribute("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
    public class PropTipConfig
    {
        eItemType mItemType = eItemType.Consumable;
        [System.ComponentModel.DisplayName("<1>物品类型")]
        [ServerFrame.Config.DataValueAttribute("ItemType")]
        public eItemType ItemType
        {
            get { return mItemType; }
            set { mItemType = value; }
        }
        List<TipInfoData> mItemList = new List<TipInfoData>();
        [System.ComponentModel.DisplayName("<2>物品Tip设置列表")]
        [ServerFrame.Config.DataValueAttribute("ItemList")]
        public List<TipInfoData> ItemList
        {
            get { return mItemList; }
            set { mItemList = value; }
        }
        
    }

    public class Chapter
    {
        [System.ComponentModel.DisplayName("章节名字")]
        [ServerFrame.Config.DataValueAttribute("Name")]
        public string Name { get; set; }
    }

    [ServerFrame.Editor.CDataEditorAttribute(".scommon")]
    [ServerFrame.Editor.Template]
    public class SystemCommon : ServerFrame.CommonTemplate<SystemCommon>
    {

        List<PropTipConfig> mBagTipList = new List<PropTipConfig>();
        [System.ComponentModel.Category("1.物品Tip设置")]
        [System.ComponentModel.DisplayName("<1>物品Tip设置列表")]
        [ServerFrame.Config.DataValueAttribute("BagTipList")]
        public List<PropTipConfig> BagTipList
        {
            get { return mBagTipList; }
            set { mBagTipList = value; }
        }
        List<TipInfoData> mShopTipList = new List<TipInfoData>();
        [System.ComponentModel.Category("1.物品Tip设置")]
        [System.ComponentModel.DisplayName("<2>商店物品Tip设置列表")]
        [ServerFrame.Config.DataValueAttribute("ShopTipList")]
        public List<TipInfoData> ShopItemList
        {
            get { return mShopTipList; }
            set { mShopTipList = value; }
        }
        List<TipInfoData> mRoleTipList = new List<TipInfoData>();
        [System.ComponentModel.Category("1.物品Tip设置")]
        [System.ComponentModel.DisplayName("<3>人物物品Tip设置列表")]
        [ServerFrame.Config.DataValueAttribute("RoleTipList")]
        public List<TipInfoData> RoleTipList
        {
            get { return mRoleTipList; }
            set { mRoleTipList = value; }
        }


        int mLoadingCount = 1;
        [System.ComponentModel.Category("2.loading设置")]
        [System.ComponentModel.DisplayName("<1>loading张数")]
        [ServerFrame.Config.DataValueAttribute("LoadingCount")]
        public int LoadingCount
        {
            get { return mLoadingCount; }
            set { mLoadingCount = value; }
        }

        string mNamePreix = "";
        [System.ComponentModel.Category("2.loading设置")]
        [System.ComponentModel.DisplayName("<1>loading贴图名称前缀")]
        [ServerFrame.Config.DataValueAttribute("NamePreix")]
        public string NamePreix
        {
            get { return mNamePreix; }
            set { mNamePreix = value; }
        }

        List<int> mDrugList = new List<int>();
        [System.ComponentModel.Category("3.自动喝药")]
        [System.ComponentModel.DisplayName("<1>药品列表")]
        [ServerFrame.Config.DataValueAttribute("DrugList")]
        public List<int> DrugList
        {
            get { return mDrugList; }
            set { mDrugList = value; }
        }

        List<Chapter> mChapters = new List<Chapter>();
        [System.ComponentModel.Category("4.章节列表")]
        [System.ComponentModel.DisplayName("<1>章节名字")]
        [ServerFrame.Config.DataValueAttribute("Chapters")]
        public List<Chapter> Chapters
        {
            get { return mChapters; }
            set { mChapters = value; }
        }


        public override string GetFilePath()
        {
            return "Common/SystemCommon.scommon";
        }
    }
}
