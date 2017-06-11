using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class GuildLevelUpTemplate : RPC.IAutoSaveAndLoad
    {
        byte mMaxMemberNum;
        [ServerFrame.Config.DataValueAttribute("MaxMemberNum")]
        [System.ComponentModel.Description("帮会成员上限，随等级改变")]
        [System.ComponentModel.DisplayName("1.帮会成员上限")]
        [System.ComponentModel.Category("帮会等级数据")]
        public byte MaxMemberNum
        {
            get { return mMaxMemberNum; }
            set { mMaxMemberNum = value; }
        }

        byte mMaxLandNum;
        [ServerFrame.Config.DataValueAttribute("MaxLandNum")]
        [System.ComponentModel.Description("帮会领土上限，随等级改变")]
        [System.ComponentModel.DisplayName("2.帮会领土上限")]
        [System.ComponentModel.Category("帮会等级数据")]
        public byte MaxLandNum
        {
            get { return mMaxLandNum; }
            set { mMaxLandNum = value; }
        }
    }

    [ServerFrame.Editor.CDataEditorAttribute(".gco")]
    [ServerFrame.Editor.Template]
    public class GuildCommon : ServerFrame.CommonTemplate<GuildCommon>
    {
        UInt16 mNeedRoleLevel;
        [ServerFrame.Config.DataValueAttribute("NeedRoleLevel")]
        [System.ComponentModel.Description("创建帮会所需人物等级")]
        [System.ComponentModel.DisplayName("1.人物等级")]
        [System.ComponentModel.Category("1.创建帮会")]
        public UInt16 NeedRoleLevel
        {
            get { return mNeedRoleLevel; }
            set { mNeedRoleLevel = value; }
        }

        eCurrenceType mNeedCurrenceType;
        [ServerFrame.Config.DataValueAttribute("NeedCurrenceType")]
        [System.ComponentModel.Description("创建帮会所需货币类型")]
        [System.ComponentModel.DisplayName("2.货币类型")]
        [System.ComponentModel.Category("1.创建帮会")]
        public eCurrenceType NeedCurrenceType
        {
            get { return mNeedCurrenceType; }
            set { mNeedCurrenceType = value; }
        }

        int mNeedMoneyNum;
        [ServerFrame.Config.DataValueAttribute("NeedMoneyNum")]
        [System.ComponentModel.Description("创建帮会所需货币数量")]
        [System.ComponentModel.DisplayName("3.货币数量")]
        [System.ComponentModel.Category("1.创建帮会")]
        public int NeedMoneyNum
        {
            get { return mNeedMoneyNum; }
            set { mNeedMoneyNum = value; }
        }

        List<GuildLevelUpTemplate> mGuildLvUpList = new List<GuildLevelUpTemplate>();
        [ServerFrame.Config.DataValueAttribute("GuildLvUpList")]
        [System.ComponentModel.Description("帮会升级数据")]
        [System.ComponentModel.DisplayName("1.帮会升级数据")]
        [System.ComponentModel.Category("2.帮会数据")]
        public List<GuildLevelUpTemplate> GuildLvUpList
        {
            get { return mGuildLvUpList; }
            set { mGuildLvUpList = value; }
        }

        byte mMaxJingYing;
        [ServerFrame.Config.DataValueAttribute("MaxJingYing")]
        [System.ComponentModel.Description("精英最大数量")]
        [System.ComponentModel.DisplayName("2.精英数量")]
        [System.ComponentModel.Category("2.帮会数据")]
        public byte MaxJingYing
        {
            get { return mMaxJingYing; }
            set { mMaxJingYing = value; }
        }

        byte mMaxZhangLao;
        [ServerFrame.Config.DataValueAttribute("MaxZhangLao")]
        [System.ComponentModel.Description("长老最大数量")]
        [System.ComponentModel.DisplayName("3.长老数量")]
        [System.ComponentModel.Category("2.帮会数据")]
        public byte MaxZhangLao
        {
            get { return mMaxZhangLao; }
            set { mMaxZhangLao = value; }
        }

        byte mMaxTangZhu;
        [ServerFrame.Config.DataValueAttribute("MaxTangZhu")]
        [System.ComponentModel.Description("堂主最大数量")]
        [System.ComponentModel.DisplayName("4.堂主数量")]
        [System.ComponentModel.Category("2.帮会数据")]
        public byte MaxTangZhu
        {
            get { return mMaxTangZhu; }
            set { mMaxTangZhu = value; }
        }

        byte mFlushContributeTime = 0;
        [ServerFrame.Config.DataValueAttribute("FlushContributeTime")]
        [System.ComponentModel.Description("每日贡献值刷新时间，单位时")]
        [System.ComponentModel.DisplayName("5.每日贡献值刷新时间")]
        [System.ComponentModel.Category("2.帮会数据")]
        public byte FlushContributeTime
        {
            get { return mFlushContributeTime; }
            set { mFlushContributeTime = value; }
        }

        int mPayTax;
        [ServerFrame.Config.DataValueAttribute("PayTax")]
        [System.ComponentModel.Description("每日交税")]
        [System.ComponentModel.DisplayName("1.每日交税")]
        [System.ComponentModel.Category("3.帮会交税")]
        public int PayTax
        {
            get { return mPayTax; }
            set { mPayTax = value; }
        }

        float mPayTaxTime = 1;
        [ServerFrame.Config.DataValueAttribute("PayTaxTime")]
        [System.ComponentModel.Description("每日交税时间，单位时")]
        [System.ComponentModel.DisplayName("2.每日交税时间")]
        [System.ComponentModel.Category("3.帮会交税")]
        public float PayTaxTime
        {
            get { return mPayTaxTime; }
            set { mPayTaxTime = value; }
        }

        int mLessPayNum;
        [ServerFrame.Config.DataValueAttribute("LessPayNum")]
        [System.ComponentModel.Description("帮会资金少于这个数量时发送邮件提醒")]
        [System.ComponentModel.DisplayName("3.帮会资金最低数量")]
        [System.ComponentModel.Category("3.帮会交税")]
        public int LessPayNum
        {
            get { return mLessPayNum; }
            set { mLessPayNum = value; }
        }
        public override string GetFilePath()
        {
            return "Common/Guild.gco";
        }
    }
}
