/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemEquipRefineLv.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class ItemEquipRefineLvData{
		public ItemEquipRefineLvData()		
		{				
		}

		public ItemEquipRefineLvData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				tid = StaticDataManager.stringToInt(array[1]);
				level = StaticDataManager.stringToInt(array[2]);
				ItemQuality = StaticDataManager.stringToInt(array[3]);
				attri1 = StaticDataManager.stringToInt(array[4]);
				value1 = StaticDataManager.stringToInt(array[5]);
				value11 = StaticDataManager.stringToInt(array[6]);
				attri2 = StaticDataManager.stringToInt(array[7]);
				value2 = StaticDataManager.stringToInt(array[8]);
				value22 = StaticDataManager.stringToInt(array[9]);
				attri3 = StaticDataManager.stringToInt(array[10]);
				value3 = StaticDataManager.stringToInt(array[11]);
				value33 = StaticDataManager.stringToInt(array[12]);
				attri4 = StaticDataManager.stringToInt(array[13]);
				value4 = StaticDataManager.stringToFloat(array[14]);
				value44 = StaticDataManager.stringToInt(array[15]);
				attri5 = StaticDataManager.stringToInt(array[16]);
				value5 = StaticDataManager.stringToFloat(array[17]);
				attri6 = StaticDataManager.stringToInt(array[18]);
				value6 = StaticDataManager.stringToFloat(array[19]);
					
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
		//精炼等级;		
		public int level
		{			
			set;			
			get;		
		}		
		//物品品质;		
		public int ItemQuality
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
		//value11;		
		public int value11
		{			
			set;			
			get;		
		}		
		//属性2;		
		public int attri2
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
		//value22;		
		public int value22
		{			
			set;			
			get;		
		}		
		//属性3;		
		public int attri3
		{			
			set;			
			get;		
		}		
		//value3;		
		public int value3
		{			
			set;			
			get;		
		}		
		//value33;		
		public int value33
		{			
			set;			
			get;		
		}		
		//属性4;		
		public int attri4
		{			
			set;			
			get;		
		}		
		//value4;		
		public float value4
		{			
			set;			
			get;		
		}		
		//value44;		
		public int value44
		{			
			set;			
			get;		
		}		
		//属性5;		
		public int attri5
		{			
			set;			
			get;		
		}		
		//value5;		
		public float value5
		{			
			set;			
			get;		
		}		
		//属性6;		
		public int attri6
		{			
			set;			
			get;		
		}		
		//value6;		
		public float value6
		{			
			set;			
			get;		
		}

	}
}