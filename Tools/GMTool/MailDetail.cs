using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Tool
{
    public class ItemNameConverter : StringConverter 
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(ToolThread.Instance.names.ToArray());
        }
    }

    class MailDetail
    {
        string mType = "邮件类型";
        [System.ComponentModel.DisplayName("01.邮件类型")]
        [System.ComponentModel.Category("1.基本信息")]
        public string Type
        {
            set { mType = value; }
            get { return mType; }
        }

        string mTitle = "邮件标题";
        [System.ComponentModel.DisplayName("02.邮件标题")]
        [System.ComponentModel.Category("1.基本信息")]
        public string Title
        {
            set { mTitle = value; }
            get { return mTitle; }
        }


        string mContent = "邮件内容";
        [System.ComponentModel.DisplayName("03.邮件内容")]
        [System.ComponentModel.Category("1.基本信息")]
        public string Content
        {
            set { mContent = value; }
            get { return mContent; }
        }


        string mItemAName;
        [System.ComponentModel.DisplayName("01.物品A")]
        [System.ComponentModel.Category("2.附件物品")]
        [System.ComponentModel.TypeConverter(typeof(ItemNameConverter))]
        public string ItemAName
        {
            get { return mItemAName; }
            set { mItemAName = value; }
        }

        int mItemANum = 0;
        [System.ComponentModel.DisplayName("01.物品A数量")]
        [System.ComponentModel.Category("2.附件物品")]
        public int ItemANum
        {
            get { return mItemANum; }
            set { mItemANum = value; }
        }

        string mItemBName;
        [System.ComponentModel.DisplayName("02.物品B")]
        [System.ComponentModel.Category("2.附件物品")]
        [System.ComponentModel.TypeConverter(typeof(ItemNameConverter))]
        public string ItemBName
        {
            get { return mItemBName; }
            set { mItemBName = value; }
        }

        int mItemBNum = 0;
        [System.ComponentModel.DisplayName("02.物品B数量")]
        [System.ComponentModel.Category("2.附件物品")]
        public int ItemBNum
        {
            get { return mItemBNum; }
            set { mItemBNum = value; }
        }

        string mItemCName;
        [System.ComponentModel.DisplayName("03.物品")]
        [System.ComponentModel.Category("2.附件物品")]
        [System.ComponentModel.TypeConverter(typeof(ItemNameConverter))]
        public string ItemCName
        {
            get { return mItemCName; }
            set { mItemCName = value; }
        }

        int mItemCNum = 0;
        [System.ComponentModel.DisplayName("03.物品C数量")]
        [System.ComponentModel.Category("2.附件物品")]
        public int ItemCNum
        {
            get { return mItemCNum; }
            set { mItemCNum = value; }
        }

        string mItemDName;
        [System.ComponentModel.DisplayName("04.物品D")]
        [System.ComponentModel.Category("2.附件物品")]
        [System.ComponentModel.TypeConverter(typeof(ItemNameConverter))]
        public string ItemDName
        {
            get { return mItemDName; }
            set { mItemDName = value; }
        }

        int mItemDNum = 0;
        [System.ComponentModel.DisplayName("04.物品D数量")]
        [System.ComponentModel.Category("2.附件物品")]
        public int ItemDNum
        {
            get { return mItemDNum; }
            set { mItemDNum = value; }
        }

        int mRmb = 0;
        [System.ComponentModel.DisplayName("01.元宝数量")]
        [System.ComponentModel.Category("4.附件货币")]
        public int Rmb
        {
            get { return mRmb; }
            set { mRmb = value; }
        }

        int mGold = 0;
        [System.ComponentModel.DisplayName("02.银两数量")]
        [System.ComponentModel.Category("4.附件货币")]
        public int Gold
        {
            get { return mGold; }
            set { mGold = value; }
        }

        int mPrivateRmb = 0;
        [System.ComponentModel.DisplayName("03.绑定元宝数量")]
        [System.ComponentModel.Category("4.附件货币")]
        public int PrivateRmb
        {
            get { return mPrivateRmb; }
            set { mPrivateRmb = value; }
        }

        int mReputation = 0;
        [System.ComponentModel.DisplayName("04.声望")]
        [System.ComponentModel.Category("4.附件货币")]
        public int Reputation
        {
            get { return mReputation; }
            set { mReputation = value; }
        }

        int mExploit = 0;
        [System.ComponentModel.DisplayName("05.战功")]
        [System.ComponentModel.Category("4.附件货币")]
        public int Exploit
        {
            get { return mExploit; }
            set { mExploit = value; }
        }

        int mActiveness = 0;
        [System.ComponentModel.DisplayName("06.活跃度")]
        [System.ComponentModel.Category("4.附件货币")]
        public int Activeness
        {
            get { return mActiveness; }
            set { mActiveness = value; }
        }

        int mShelfLife = 6;     //保质期，天数
        [System.ComponentModel.DisplayName("04.保质期(天)")]
        [System.ComponentModel.Category("1.基本信息")]
        public int ShelfLife
        {
            get { return mShelfLife; }
            set { mShelfLife = value; }
        }


        List<ushort> mPlanesIds = new List<ushort>();
        [System.ComponentModel.DisplayName("05.区Id集合")]
        [System.ComponentModel.Category("1.基本信息")]
        public List<ushort> PlanesIds
        {
            get { return mPlanesIds; }
            set { mPlanesIds = value; }
        }

        List<ulong> mRoleIds = new List<ulong>();
        [System.ComponentModel.DisplayName("06.接收者id集合")]
        [System.ComponentModel.Category("1.基本信息")]
        public List<ulong> RoleIds
        {
            get { return mRoleIds; }
            set { mRoleIds = value; }
        }


    }


}
