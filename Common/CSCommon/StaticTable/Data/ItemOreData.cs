/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemOre.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class ItemOreData{
		public ItemOreData()		
		{				
		}

		public ItemOreData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				exp = StaticDataManager.stringToInt(array[1]);
				type1 = StaticDataManager.stringToByte(array[2]);
				count1 = StaticDataManager.stringToInt(array[3]);
					
		}
				
		//矿石id;		
		public int id
		{			
			set;			
			get;		
		}		
		//经验;		
		public int exp
		{			
			set;			
			get;		
		}		
		//消耗货币类型1;		
		public byte type1
		{			
			set;			
			get;		
		}		
		//消耗货币1;		
		public int count1
		{			
			set;			
			get;		
		}

	}
}