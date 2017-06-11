/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemCombine.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class ItemCombineData{
		public ItemCombineData()		
		{				
		}

		public ItemCombineData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				needIds = Array.ConvertAll(array[1].Split('|'), new Converter<String, int>(StaticDataManager.stringToInt));
				needNums = Array.ConvertAll(array[2].Split('|'), new Converter<String, int>(StaticDataManager.stringToInt));
				costType = StaticDataManager.stringToByte(array[3]);
				costNum = StaticDataManager.stringToInt(array[4]);
					
		}
				
		//物品id;		
		public int id
		{			
			set;			
			get;		
		}		
		//需要物品id集合;		
		public int[] needIds
		{			
			set;			
			get;		
		}		
		//需要物品数量集合;		
		public int[] needNums
		{			
			set;			
			get;		
		}		
		//消耗货币类型;		
		public byte costType
		{			
			set;			
			get;		
		}		
		//消耗货币数量;		
		public int costNum
		{			
			set;			
			get;		
		}

	}
}