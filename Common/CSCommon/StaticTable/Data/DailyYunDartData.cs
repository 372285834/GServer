/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/DailyYunDart.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class DailyYunDartData{
		public DailyYunDartData()		
		{				
		}

		public DailyYunDartData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				Target = array[1];
				Type = StaticDataManager.stringToByte(array[2]);
				Count = StaticDataManager.stringToByte(array[3]);
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
		//类型;		
		public byte Type
		{			
			set;			
			get;		
		}		
		//次数;		
		public byte Count
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