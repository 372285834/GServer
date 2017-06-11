//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace CSCommon.Data.GiftItem
//{
//    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
//    public class GiftInfo : ICopyable
//    {
//        int mPassTime;
//        [ServerFrame.Config.DataValueAttribute("PassTime")]
//        public int PassTime
//        {
//            get { return mPassTime; }
//            set { mPassTime = value; }
//        }
//        byte mDropNum;
//        [ServerFrame.Config.DataValueAttribute("DropNum")]
//        public byte DropNum
//        {
//            get { return mDropNum; }
//            set { mDropNum = value; }
//        }
//        byte mGetAwardTime;
//        [ServerFrame.Config.DataValueAttribute("GetAwardTime")]
//        public byte GetAwardTime
//        {
//            get { return mGetAwardTime; }
//            set { mGetAwardTime = value; }
//        }
//        Int16 mDropGroupTemplateId;
//        [ServerFrame.Config.DataValueAttribute("DropGroupTemplateId")]
//        public Int16 DropGroupTemplateId
//        {
//            get { return mDropGroupTemplateId; }
//            set { mDropGroupTemplateId = value; }
//        }
//    }

//    //[ServerFrame.Editor.CDataEditorAttribute(".gift")]
//    public class OnlineGiftTemplate
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

//        List<GiftInfo> mGifts = new List<GiftInfo>();
//        [ServerFrame.Config.DataValueAttribute("Gifts")]
//        [System.ComponentModel.DisplayName("礼物物品")]
//        [System.ComponentModel.Description("")]
//        public List<GiftInfo> Gifts
//        {
//            get { return mGifts; }
//            set { mGifts = value; }
//        }
//    }

//    public class GiftTemplateManager
//    {
//        static GiftTemplateManager smInstance = new GiftTemplateManager();
//        public static GiftTemplateManager Instance
//        {
//            get { return smInstance; }
//        }

//        OnlineGiftTemplate mOnlineGift = new OnlineGiftTemplate();
//        public OnlineGiftTemplate OnlineGift
//        {
//            get { return mOnlineGift; }
//        }

//        Dictionary<UInt16, OnlineGiftTemplate> mAllGiftTemplates = new Dictionary<UInt16, OnlineGiftTemplate>();
//        public Dictionary<UInt16, OnlineGiftTemplate> AllGiftTemplates
//        {
//            get { return mAllGiftTemplates; }
//        }
//        public void LoadAll()
//        {
//            var icFile = ServerFrame.Support.IFileManager.Instance.Root + "ZeusGame/Template/OnlineAward/OnlineGift.gift";
//            if (System.IO.File.Exists(icFile))
//                ServerFrame.Config.IConfigurator.FillProperty(mOnlineGift, icFile);
//        }
//    }

//}
