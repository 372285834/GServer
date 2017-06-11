/*
 * 本代码由工具自动生成，请勿手工修改
 * */
using System.Collections;
using UnityEngine;
using System;
using CSCommon;
using System.Collections.Generic;
namespace CSTable
{
    public interface IDataModel
    {
        void initData(string content);
    }
	public class StaticDataManager
	{
        public static string TableDataPath = "TableData/";
        //ModelPool;
        public static Dictionary<string, object> Dic = new Dictionary<string, object>();
        	public static AchieveNameTable AchieveName = new AchieveNameTable();
		public static AchieveTplTable AchieveTpl = new AchieveTplTable();
		public static AwardTable Award = new AwardTable();
		public static BarrackEfficiencyItemTable BarrackEfficiencyItem = new BarrackEfficiencyItemTable();
		public static BarrackLevelTable BarrackLevel = new BarrackLevelTable();
		public static BuffTable Buff = new BuffTable();
		public static BuffLevelTable BuffLevel = new BuffLevelTable();
		public static CheatsTable Cheats = new CheatsTable();
		public static CityTable City = new CityTable();
		public static CopyTplTable CopyTpl = new CopyTplTable();
		public static DailyComTable DailyCom = new DailyComTable();
		public static DailyCountryTable DailyCountry = new DailyCountryTable();
		public static DailyLakeTable DailyLake = new DailyLakeTable();
		public static DailyYunDartTable DailyYunDart = new DailyYunDartTable();
		public static DayRankTable DayRank = new DayRankTable();
		public static EffResEffTplTable EffResEffTpl = new EffResEffTplTable();
		public static ItemCombineTable ItemCombine = new ItemCombineTable();
		public static ItemEquipTable ItemEquip = new ItemEquipTable();
		public static ItemEquipLevelTable ItemEquipLevel = new ItemEquipLevelTable();
		public static ItemEquipRefineTable ItemEquipRefine = new ItemEquipRefineTable();
		public static ItemEquipRefineLvTable ItemEquipRefineLv = new ItemEquipRefineLvTable();
		public static ItemFashionTable ItemFashion = new ItemFashionTable();
		public static ItemGemTable ItemGem = new ItemGemTable();
		public static ItemGiftTable ItemGift = new ItemGiftTable();
		public static ItemOreTable ItemOre = new ItemOreTable();
		public static ItemPackageTable ItemPackage = new ItemPackageTable();
		public static ItemTplTable ItemTpl = new ItemTplTable();
		public static ItemUseTable ItemUse = new ItemUseTable();
		public static LoadContextTable LoadContext = new LoadContextTable();
		public static LoadingTipsTable LoadingTips = new LoadingTipsTable();
		public static MailInfoTable MailInfo = new MailInfoTable();
		public static MapsTable Maps = new MapsTable();
		public static MapWayTable MapWay = new MapWayTable();
		public static MartialClubTable MartialClub = new MartialClubTable();
		public static MartialItemTable MartialItem = new MartialItemTable();
		public static MartialPlantTable MartialPlant = new MartialPlantTable();
		public static MartialSmeltTable MartialSmelt = new MartialSmeltTable();
		public static MartialTrainTable MartialTrain = new MartialTrainTable();
		public static MaskWordTable MaskWord = new MaskWordTable();
		public static MessageTable Message = new MessageTable();
		public static NPCTalkTable NPCTalk = new NPCTalkTable();
		public static NPCTplTable NPCTpl = new NPCTplTable();
		public static NPC_AttriTable NPC_Attri = new NPC_AttriTable();
		public static PlayerLevelTable PlayerLevel = new PlayerLevelTable();
		public static PlayerTplTable PlayerTpl = new PlayerTplTable();
		public static PointSoldierTable PointSoldier = new PointSoldierTable();
		public static SkillActiveTable SkillActive = new SkillActiveTable();
		public static SkillLevelTable SkillLevel = new SkillLevelTable();
		public static SkillPassiveTable SkillPassive = new SkillPassiveTable();
		public static SkillPassiveLevelTable SkillPassiveLevel = new SkillPassiveLevelTable();
		public static StrongPointTable StrongPoint = new StrongPointTable();
		public static TaskTalkTable TaskTalk = new TaskTalkTable();
		public static TaskTplTable TaskTpl = new TaskTplTable();
	

        /// <summary>
        /// ???????????;
        /// </summary>
        public static void registerModel()
        {
			Dic.Clear();
            Dic.Add("AchieveName", AchieveName);
			Dic.Add("AchieveTpl", AchieveTpl);
			Dic.Add("Award", Award);
			Dic.Add("BarrackEfficiencyItem", BarrackEfficiencyItem);
			Dic.Add("BarrackLevel", BarrackLevel);
			Dic.Add("Buff", Buff);
			Dic.Add("BuffLevel", BuffLevel);
			Dic.Add("Cheats", Cheats);
			Dic.Add("City", City);
			Dic.Add("CopyTpl", CopyTpl);
			Dic.Add("DailyCom", DailyCom);
			Dic.Add("DailyCountry", DailyCountry);
			Dic.Add("DailyLake", DailyLake);
			Dic.Add("DailyYunDart", DailyYunDart);
			Dic.Add("DayRank", DayRank);
			Dic.Add("EffResEffTpl", EffResEffTpl);
			Dic.Add("ItemCombine", ItemCombine);
			Dic.Add("ItemEquip", ItemEquip);
			Dic.Add("ItemEquipLevel", ItemEquipLevel);
			Dic.Add("ItemEquipRefine", ItemEquipRefine);
			Dic.Add("ItemEquipRefineLv", ItemEquipRefineLv);
			Dic.Add("ItemFashion", ItemFashion);
			Dic.Add("ItemGem", ItemGem);
			Dic.Add("ItemGift", ItemGift);
			Dic.Add("ItemOre", ItemOre);
			Dic.Add("ItemPackage", ItemPackage);
			Dic.Add("ItemTpl", ItemTpl);
			Dic.Add("ItemUse", ItemUse);
			Dic.Add("LoadContext", LoadContext);
			Dic.Add("LoadingTips", LoadingTips);
			Dic.Add("MailInfo", MailInfo);
			Dic.Add("Maps", Maps);
			Dic.Add("MapWay", MapWay);
			Dic.Add("MartialClub", MartialClub);
			Dic.Add("MartialItem", MartialItem);
			Dic.Add("MartialPlant", MartialPlant);
			Dic.Add("MartialSmelt", MartialSmelt);
			Dic.Add("MartialTrain", MartialTrain);
			Dic.Add("MaskWord", MaskWord);
			Dic.Add("Message", Message);
			Dic.Add("NPCTalk", NPCTalk);
			Dic.Add("NPCTpl", NPCTpl);
			Dic.Add("NPC_Attri", NPC_Attri);
			Dic.Add("PlayerLevel", PlayerLevel);
			Dic.Add("PlayerTpl", PlayerTpl);
			Dic.Add("PointSoldier", PointSoldier);
			Dic.Add("SkillActive", SkillActive);
			Dic.Add("SkillLevel", SkillLevel);
			Dic.Add("SkillPassive", SkillPassive);
			Dic.Add("SkillPassiveLevel", SkillPassiveLevel);
			Dic.Add("StrongPoint", StrongPoint);
			Dic.Add("TaskTalk", TaskTalk);
			Dic.Add("TaskTpl", TaskTpl);
			
        }
		/// <summary>
        /// ???????????flaot;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float stringToFloat(string value)
        {
			if (value == "")
                return 0;

            return float.Parse(value);
        }
			
		/// <summary>
        /// ???????????int;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int stringToInt(string value)
        {
 			if (value == "")
                return 0;
            return Int32.Parse(value);
        }
			
		/// <summary>
        /// ???????????long;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long stringToLong(string value)
        {
			if (value == "")
                return 0;
            return long.Parse(value);
        }
		public static uint stringToUint(string value)
        {
            if (value == "")
                return 0;
            return uint.Parse(value);
        }
        public static ushort stringToUshort(string value)
        {
            if (value == "")
                return 0;
            return ushort.Parse(value);
        }
		public static byte stringToByte(string value)
        {
            if (value == "")
                return 0;
            return byte.Parse(value);
        }

		/// <summary>
        /// 本地加载静态数据，
        /// </summary>
        public static void InitTable(string path = "")
        {
            if (string.IsNullOrEmpty(path))
                path = Application.dataPath + "/StreamingAssets/" + CSTable.StaticDataManager.TableDataPath;

            CSTable.StaticDataManager.registerModel();
            var fileName = path + "files.txt";
            string context = System.IO.File.ReadAllText(fileName);
            var files = context.Split('\n');
            foreach (var file in files)
            {
                if (string.IsNullOrEmpty(file))
                {
                    continue;
                }
                string url = path + file + ".txt";
                context = System.IO.File.ReadAllText(url);
                if (CSTable.StaticDataManager.Dic.ContainsKey(file))
                {
                    CSTable.IDataModel model = (CSTable.IDataModel)CSTable.StaticDataManager.Dic[file];
                    model.initData(context);
                }
            }
        }
	}
}