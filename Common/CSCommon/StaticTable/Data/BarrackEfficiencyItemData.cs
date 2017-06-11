/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/BarrackEfficiencyItem.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class BarrackEfficiencyItemData{
		public BarrackEfficiencyItemData()		
		{				
		}

		public BarrackEfficiencyItemData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				efficiency = StaticDataManager.stringToInt(array[1]);
				price = array[2];
				duration = StaticDataManager.stringToInt(array[3]);
					
		}
				
		//id;		
		public int id
		{			
			set;			
			get;		
		}		
		//效率;		
		public int efficiency
		{			
			set;			
			get;		
		}		
		//价格;		
		public string price
		{			
			set;			
			get;		
		}		
		//有效时间;		
		public int duration
		{			
			set;			
			get;		
		}

	}
}