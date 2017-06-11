/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/PointSoldier.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class PointSoldierData{
		public PointSoldierData()		
		{				
		}

		public PointSoldierData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				PointId = StaticDataManager.stringToInt(array[1]);
				templateid = StaticDataManager.stringToInt(array[2]);
					
		}
				
		//士兵Id;		
		public int id
		{			
			set;			
			get;		
		}		
		//据点Id;		
		public int PointId
		{			
			set;			
			get;		
		}		
		//士兵npc模板Id;		
		public int templateid
		{			
			set;			
			get;		
		}

	}
}