/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/DailyLake.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class DailyLakeData{
		public DailyLakeData()		
		{				
		}

		public DailyLakeData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				Target = array[1];
				MonsterId = StaticDataManager.stringToInt(array[2]);
				Count = StaticDataManager.stringToInt(array[3]);
				CurrencyReward = array[4];
				ItemsReward = array[5];
					
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
		//怪物id;		
		public int MonsterId
		{			
			set;			
			get;		
		}		
		//击杀数量;		
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