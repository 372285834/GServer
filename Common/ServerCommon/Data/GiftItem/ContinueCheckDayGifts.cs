//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace CSCommon.Data.GiftItem
//{
//    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
//    public class CheckDayGiftInfo : ICopyable
//    {
//        Int16 mDropGroupTemplateId;
//        [ServerFrame.Config.DataValueAttribute("DropGroupTemplateId")]
//        public Int16 DropGroupTemplateId
//        {
//            get { return mDropGroupTemplateId; }
//            set { mDropGroupTemplateId = value; }
//        }
//    }

//    //[ServerFrame.Editor.CDataEditorAttribute(".check")]
//    public class CheckDayGiftTemplate
//    {
//        string mGiftName = "";
//        [ServerFrame.Config.DataValueAttribute("GiftName")]
//        [System.ComponentModel.DisplayName("礼包名字")]
//        [System.ComponentModel.Description("")]
//        public string GiftName
//        {
//            get { return mGiftName; }
//            set { mGiftName = value; }
//        }

//        UInt16 mId;
//        [ServerFrame.Config.DataValueAttribute("Id")]
//        [System.ComponentModel.DisplayName("模板ID")]
//        [System.ComponentModel.Description("")]
//        public UInt16 Id
//        {
//            get { return mId; }
//            set { mId = value; }
//        }

//        List<CheckDayGiftInfo> mGifts = new List<CheckDayGiftInfo>();
//        [ServerFrame.Config.DataValueAttribute("Gifts")]
//        [System.ComponentModel.DisplayName("礼物物品")]
//        [System.ComponentModel.Description("")]
//        public List<CheckDayGiftInfo> Gifts
//        {
//            get { return mGifts; }
//            set { mGifts = value; }
//        }
//    }

//    public class CheckDayGiftTemplateManager
//    {
//        static CheckDayGiftTemplateManager smInstance = new CheckDayGiftTemplateManager();
//        public static CheckDayGiftTemplateManager Instance
//        {
//            get { return smInstance; }
//        }

//        CheckDayGiftTemplate mCheckDayGift = new CheckDayGiftTemplate();
//        public CheckDayGiftTemplate CheckDayGift
//        {
//            get { return mCheckDayGift; }
//        }

//        Dictionary<UInt16, CheckDayGiftTemplate> mAllGiftTemplates = new Dictionary<UInt16, CheckDayGiftTemplate>();
//        public Dictionary<UInt16, CheckDayGiftTemplate> AllGiftTemplates
//        {
//            get { return mAllGiftTemplates; }
//        }
//        public void LoadAll()
//        {
//            var icFile = ServerFrame.Support.IFileManager.Instance.Root + "ZeusGame/Template/CountinueCheckDay/CountinueCheckday.check";
//            if (System.IO.File.Exists(icFile))
//                ServerFrame.Config.IConfigurator.FillProperty(mCheckDayGift, icFile);
//        }
//    }
//}
