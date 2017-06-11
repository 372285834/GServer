/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemEquipRefine.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class ItemEquipRefineData{
		public ItemEquipRefineData()		
		{				
		}

		public ItemEquipRefineData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				mid = StaticDataManager.stringToInt(array[1]);
				rate = StaticDataManager.stringToFloat(array[2]);
					
		}
				
		//精炼等级;		
		public int id
		{			
			set;			
			get;		
		}		
		//需要灵石id;		
		public int mid
		{			
			set;			
			get;		
		}		
		//单个灵石精炼概率;		
		public float rate
		{			
			set;			
			get;		
		}

	}
}