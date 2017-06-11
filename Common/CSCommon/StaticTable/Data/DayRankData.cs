/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/DayRank.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class DayRankData{
		public DayRankData()		
		{				
		}

		public DayRankData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				lv = StaticDataManager.stringToInt(array[1]);
				rewardId = StaticDataManager.stringToInt(array[2]);
				reward = array[3];
					
		}
				
		//类型;		
		public int id
		{			
			set;			
			get;		
		}		
		//角色等级;		
		public int lv
		{			
			set;			
			get;		
		}		
		//奖励id;		
		public int rewardId
		{			
			set;			
			get;		
		}		
		//奖励;		
		public string reward
		{			
			set;			
			get;		
		}

	}
}