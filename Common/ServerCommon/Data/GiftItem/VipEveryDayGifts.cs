//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace CSCommon.Data.GiftItem
//{
//    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
//    public class EveryDayGiftInfo : ICopyable
//    {
//        Int16 mGiftId;
//        [ServerFrame.Config.DataValueAttribute("GiftId")]
//        [System.ComponentModel.DisplayName("vip每日物品id")]  //ItemId
//        public Int16 GiftId
//        {
//            get { return mGiftId; }
//            set { mGiftId = value; }
//        }
//    }

//    //[ServerFrame.Editor.CDataEditorAttribute(".everyday")]
//    public class EveryDayGiftTemplate
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

//        List<EveryDayGiftInfo> mGifts = new List<EveryDayGiftInfo>();
//        [ServerFrame.Config.DataValueAttribute("Gifts")]
//        [System.ComponentModel.DisplayName("礼物物品")]
//        [System.ComponentModel.Description("")]
//        public List<EveryDayGiftInfo> Gifts
//        {
//            get { return mGifts; }
//            set { mGifts = value; }
//        }
//    }

//    public class VipEveryDayTemplateManager
//    {
//        static VipEveryDayTemplateManager smInstance = new VipEveryDayTemplateManager();
//        public static VipEveryDayTemplateManager Instance
//        {
//            get { return smInstance; }
//        }
//        EveryDayGiftTemplate mEveryDayGifts = new EveryDayGiftTemplate();
//        public EveryDayGiftTemplate EveryDayGifts
//        {
//            get { return mEveryDayGifts; }
//        }
//        Dictionary<UInt16, EveryDayGiftTemplate> mAllGiftTemplates = new Dictionary<UInt16, EveryDayGiftTemplate>();
//        public Dictionary<UInt16, EveryDayGiftTemplate> AllGiftTemplates
//        {
//            get { return mAllGiftTemplates; }
//        }
//        public void LoadAll()
//        {
//            mAllGiftTemplates.Clear();
//            var files = System.IO.Directory.EnumerateFiles(ServerFrame.Support.IFileManager.Instance.Root + "ZeusGame/Template/EveryDayVipGifts/");
//            foreach (var i in files)
//            {
//                if (i.Substring(i.Length - 9, 9) == ".everyday")
//                {
//                    string fullPathname = i;
//                    EveryDayGiftTemplate item = new EveryDayGiftTemplate();
//                    if (ServerFrame.Config.IConfigurator.FillProperty(item, fullPathname))
//                    {
//                        mAllGiftTemplates.Add(item.Id, item);
//                    }
//                }
//            }
//        }
//        public EveryDayGiftTemplate FindGift(UInt16 templateId)
//        {
//            EveryDayGiftTemplate item = null;
//            if (mAllGiftTemplates.TryGetValue(templateId, out item))
//                return item;
//            return null;
//        }
//    }

//}
