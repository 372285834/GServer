/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/Award.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class AwardData{
		public AwardData()		
		{				
		}

		public AwardData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			ID = StaticDataManager.stringToInt(array[0]);
				State = StaticDataManager.stringToInt(array[1]);
				Request = StaticDataManager.stringToInt(array[2]);
				ExpendId = StaticDataManager.stringToInt(array[3]);
				Expend = StaticDataManager.stringToInt(array[4]);
				CurrencyReward = array[5];
				ItemsReward = array[6];
				Degree = StaticDataManager.stringToInt(array[7]);
				Date = array[8];
					
		}
				
		// 奖励ID;		
		public int ID
		{			
			set;			
			get;		
		}		
		//奖励的状态;		
		public int State
		{			
			set;			
			get;		
		}		
		//奖励的条件;		
		public int Request
		{			
			set;			
			get;		
		}		
		//消耗品ID;		
		public int ExpendId
		{			
			set;			
			get;		
		}		
		//消耗;		
		public int Expend
		{			
			set;			
			get;		
		}		
		//奖励的货币;		
		public string CurrencyReward
		{			
			set;			
			get;		
		}		
		//奖励的物品;		
		public string ItemsReward
		{			
			set;			
			get;		
		}		
		//奖励的次数;		
		public int Degree
		{			
			set;			
			get;		
		}		
		//签到的日期;		
		public string Date
		{			
			set;			
			get;		
		}

	}
}