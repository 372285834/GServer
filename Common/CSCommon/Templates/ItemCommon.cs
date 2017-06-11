using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    public class CdGroup
    {
        List<int> mlist = new List<int>();
        [System.ComponentModel.DisplayName("<1>cd组")]
        [ServerFrame.Config.DataValueAttribute("list")]
        public List<int> list
        {
            get { return mlist; }
            set { mlist = value; }
        }
    }

    /// <summary>
    /// 权值
    /// </summary>
    public class RateValue
    {
        int mRate;
        [ServerFrame.Config.DataValueAttribute("Rate")]
        public int Rate
        {
            get { return mRate; }
            set { mRate = value; }
        }
        int mValue;
        [ServerFrame.Config.DataValueAttribute("Value")]
        public int Value
        {
            get { return mValue; }
            set { mValue = value; }
        }
    }

    public class CurrencePath
    {
        eCurrenceType mType;
        [System.ComponentModel.DisplayName("<1>类型")]
        [ServerFrame.Config.DataValueAttribute("Type")]
        public eCurrenceType Type
        {
            get { return mType; }
            set { mType = value; }
        }

        string mPath;
        [System.ComponentModel.DisplayName("<2>路径")]
        [ServerFrame.Config.DataValueAttribute("Path")]
        public string Path
        {
            get { return mPath; }
            set { mPath = value; }
        }
        string mName;
        [System.ComponentModel.DisplayName("<3>名称")]
        [ServerFrame.Config.DataValueAttribute("Name")]
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
    }

    [ServerFrame.Editor.CDataEditorAttribute(".itemco")]
    [ServerFrame.Editor.Template]
    public class ItemCommon : ServerFrame.CommonTemplate<ItemCommon>
    {
        int mFreePrice = 25;
        [System.ComponentModel.Category("1.当铺")]
        [System.ComponentModel.DisplayName("<1>总价XX以下免佣金")]
        [ServerFrame.Config.DataValueAttribute("FreePrice")]
        public int FreePrice
        {
            get { return mFreePrice; }
            set { mFreePrice = value; }
        }

        float mPayRace = 0.1f;
        [System.ComponentModel.Category("1.当铺")]
        [System.ComponentModel.DisplayName("<2>收取佣金")]
        [ServerFrame.Config.DataValueAttribute("PayRace")]
        public float PayRace
        {
            get { return mPayRace; }
            set { mPayRace = value; }
        }

        int mRent = 500;
        [System.ComponentModel.Category("1.当铺")]
        [System.ComponentModel.DisplayName("<3>单件物品的手续费")]
        [ServerFrame.Config.DataValueAttribute("Rent")]
        public int Rent
        {
            get { return mRent; }
            set { mRent = value; }
        }

        byte mMaxConsignNum = 5;
        [System.ComponentModel.Category("1.当铺")]
        [System.ComponentModel.DisplayName("<3>单件物品的手续费")]
        [ServerFrame.Config.DataValueAttribute("MaxConsignNum")]
        public byte MaxConsignNum
        {
            get { return mMaxConsignNum; }
            set { mMaxConsignNum = value; }
        }

        byte mMaxConsignTime = 24;
        [System.ComponentModel.Category("1.当铺")]
        [System.ComponentModel.DisplayName("<4>寄售物品自动撤下时间期限")]
        [ServerFrame.Config.DataValueAttribute("MaxConsignTime")]
        public byte MaxConsignTime
        {
            get { return mMaxConsignTime; }
            set { mMaxConsignTime = value; }
        }

//         CSCommon.eCurrenceType mRemoveGemMoneyType;
//         [System.ComponentModel.Category("2.宝石")]
//         [System.ComponentModel.DisplayName("<1>宝石摘除花费类型")]
//         [ServerFrame.Config.DataValueAttribute("RemoveGemMoneyType")]
//         public CSCommon.eCurrenceType RemoveGemMoneyType
//         {
//             get { return mRemoveGemMoneyType; }
//             set { mRemoveGemMoneyType = value; }
//         }
// 
//         int mRemoveGemMoney;
//         [System.ComponentModel.Category("2.宝石")]
//         [System.ComponentModel.DisplayName("<2>宝石摘除花费")]
//         [ServerFrame.Config.DataValueAttribute("RemoveGemMoney")]
//         public int RemoveGemMoney
//         {
//             get { return mRemoveGemMoney; }
//             set { mRemoveGemMoney = value; }
//         }

        int mGemCombineNeedCount = 2;
        [System.ComponentModel.Category("2.宝石")]
        [System.ComponentModel.DisplayName("<1>宝石合成数量")]
        [ServerFrame.Config.DataValueAttribute("GemCombineNeedCount")]
        public int GemCombineNeedCount
        {
            get { return mGemCombineNeedCount; }
            set { mGemCombineNeedCount = value; }
        }

        List<CdGroup> mCdList = new List<CdGroup>();
        [System.ComponentModel.Category("3.CD分组")]
        [System.ComponentModel.DisplayName("<1>CD分组")]
        [ServerFrame.Config.DataValueAttribute("CdList")]
        public List<CdGroup> CdList
        {
            get { return mCdList; }
            set { mCdList = value; }
        }

        List<CurrencePath> mCurrencePathList = new List<CurrencePath>();
        [System.ComponentModel.Category("4.货币图片")]
        [System.ComponentModel.DisplayName("<1>货币图片")]
        [ServerFrame.Config.DataValueAttribute("CurrencePathList")]
        public List<CurrencePath> CurrencePathList
        {
            get { return mCurrencePathList; }
            set { mCurrencePathList = value; }
        }

        List<RateValue> mEquipIntensifyRate = new List<RateValue>();
        [System.ComponentModel.Category("5.装备养成")]
        [System.ComponentModel.DisplayName("<1>装备养成")]
        [ServerFrame.Config.DataValueAttribute("EquipIntensifyRate")]
        public List<RateValue> EquipIntensifyRate
        {
            get { return mEquipIntensifyRate; }
            set { mEquipIntensifyRate = value; }
        }

        int mOreId = 302001;
        [System.ComponentModel.Category("5.装备养成")]
        [System.ComponentModel.DisplayName("<2>需求灵石Id")]
        [ServerFrame.Config.DataValueAttribute("OreId")]
        public int OreId
        {
            get { return mOreId; }
            set { mOreId = value; }
        }


        public override string GetFilePath()
        {
            return "Common/Item.itemco";
        }
    }
}
