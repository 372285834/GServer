/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemEquipLevel.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class ItemEquipLevelData{
		public ItemEquipLevelData()		
		{				
		}

		public ItemEquipLevelData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				tid = StaticDataManager.stringToInt(array[1]);
				level = StaticDataManager.stringToInt(array[2]);
				playerlevel = StaticDataManager.stringToInt(array[3]);
				needcount = StaticDataManager.stringToInt(array[4]);
				needExp = StaticDataManager.stringToInt(array[5]);
				itemid = StaticDataManager.stringToInt(array[6]);
				attri1 = StaticDataManager.stringToInt(array[7]);
				value1 = StaticDataManager.stringToInt(array[8]);
				value2 = StaticDataManager.stringToInt(array[9]);
					
		}
				
		//职业Id;		
		public int id
		{			
			set;			
			get;		
		}		
		//部位Id;		
		public int tid
		{			
			set;			
			get;		
		}		
		//养成等级;		
		public int level
		{			
			set;			
			get;		
		}		
		//人物等级限制;		
		public int playerlevel
		{			
			set;			
			get;		
		}		
		//消耗灵石数量;		
		public int needcount
		{			
			set;			
			get;		
		}		
		//升级所需个数;		
		public int needExp
		{			
			set;			
			get;		
		}		
		//物品Id;		
		public int itemid
		{			
			set;			
			get;		
		}		
		//属性1;		
		public int attri1
		{			
			set;			
			get;		
		}		
		//value1;		
		public int value1
		{			
			set;			
			get;		
		}		
		//value2;		
		public int value2
		{			
			set;			
			get;		
		}

	}
}