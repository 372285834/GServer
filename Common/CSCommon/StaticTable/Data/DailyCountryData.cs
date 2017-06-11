/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/DailyCountry.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class DailyCountryData{
		public DailyCountryData()		
		{				
		}

		public DailyCountryData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				Target = array[1];
				Type = StaticDataManager.stringToByte(array[2]);
				Npcid = StaticDataManager.stringToInt(array[3]);
				Count = StaticDataManager.stringToInt(array[4]);
				CurrencyReward = array[5];
				ItemsReward = array[6];
					
		}
				
		//ID;		
		public int id
		{			
			set;			
			get;		
		}		
		//目标;		
		public string Target
		{			
			set;			
			get;		
		}		
		//类型;		
		public byte Type
		{			
			set;			
			get;		
		}		
		//龙脉Id;		
		public int Npcid
		{			
			set;			
			get;		
		}		
		//数量;		
		public int Count
		{			
			set;			
			get;		
		}		
		//奖励货币字符串;		
		public string CurrencyReward
		{			
			set;			
			get;		
		}		
		//奖励物品字符串;		
		public string ItemsReward
		{			
			set;			
			get;		
		}

	}
}