//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace CSCommon.Data.GiftItem
//{  
//    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
//    public class VipGiftInfo : ICopyable
//    {
//        Int16 mVipGiftId;
//        [ServerFrame.Config.DataValueAttribute("VipGiftId")]
//        [System.ComponentModel.DisplayName("vip物品id")]  //ItemId
//        public Int16 VipGiftId
//        {
//            get { return mVipGiftId; }
//            set { mVipGiftId = value; }
//        }

//        Int16 mItemCount = 1;
//        [ServerFrame.Config.DataValueAttribute("ItemCount")]
//        public Int16 ItemCount
//        {
//            get { return mItemCount; }
//            set { mItemCount = value; }
//        }
//    }

//    //[ServerFrame.Editor.CDataEditorAttribute(".vip")]
//    public class VipGiftTemplate
//    {
//        UInt16 mVipId;
//        [ServerFrame.Config.DataValueAttribute("VipId")]
//        [System.ComponentModel.DisplayName("Vip等级")]
//        [System.ComponentModel.Description("")]
//        public UInt16 VipId
//        {
//            get { return mVipId; }
//            set { mVipId = value; }
//        }

//        string mVipGiftName = "";
//        [ServerFrame.Config.DataValueAttribute("VipGiftName")]
//        [System.ComponentModel.DisplayName("升级礼包礼包名字")]
//        [System.ComponentModel.Description("")]
//        public string VipGiftName 
//        {
//            get { return mVipGiftName; }
//            set { mVipGiftName = value; }
//        }


//        List<VipGiftInfo> mVipGifts = new List<VipGiftInfo>();
//        [ServerFrame.Config.DataValueAttribute("VipGifts")]
//        [System.ComponentModel.DisplayName("Vip礼物物品")]
//        [System.ComponentModel.Description("")]
//        public List<VipGiftInfo> VipGifts
//        {
//            get { return mVipGifts; }
//            set { mVipGifts = value; }
//        }
//    }
//    public class VipGiftTemplateManager
//    {
//        static VipGiftTemplateManager smInstance = new VipGiftTemplateManager();
//        public static VipGiftTemplateManager Instance
//        {
//            get { return smInstance; }
//        }
//        Dictionary<UInt16, VipGiftTemplate> mAllVipGiftTemplates = new Dictionary<UInt16, VipGiftTemplate>();
//        public Dictionary<UInt16, VipGiftTemplate> AllVipGiftTemplates 
//        {
//            get { return mAllVipGiftTemplates; }
//        }
//        public void LoadAllTemplate()
//        {
//            mAllVipGiftTemplates.Clear();
//            var files = System.IO.Directory.EnumerateFiles(ServerFrame.Support.IFileManager.Instance.Root + "ZeusGame/Template/VipUplevelGift/");
//            foreach (var i in files)
//            {
//                if (i.Substring(i.Length - 4, 4) == ".vip")
//                {
//                    string fullPathname = i;
//                    VipGiftTemplate item = new VipGiftTemplate();
//                    if (ServerFrame.Config.IConfigurator.FillProperty(item, fullPathname))
//                    {
//                        mAllVipGiftTemplates.Add(item.VipId, item);
//                    }
//                }
//            }
//        }
//        public VipGiftTemplate FindGift(UInt16 templateId)
//        {
//            VipGiftTemplate item = null;
//            if (mAllVipGiftTemplates.TryGetValue(templateId, out item))
//                return item;
//            return null;
//        }
//    }
//}
