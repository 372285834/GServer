/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/BarrackRoleLevel.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class BarrackRoleLevelData{
		public BarrackRoleLevelData()		
		{				
		}

		public BarrackRoleLevelData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				addForcePerDay = StaticDataManager.stringToInt(array[1]);
				addForcePerCity = StaticDataManager.stringToInt(array[2]);
				addForcePerTime = StaticDataManager.stringToInt(array[3]);
					
		}
				
		//id;		
		public int id
		{			
			set;			
			get;		
		}		
		//每日发兵上限;		
		public int addForcePerDay
		{			
			set;			
			get;		
		}		
		//单个场景发兵上限;		
		public int addForcePerCity
		{			
			set;			
			get;		
		}		
		//单次发兵上限;		
		public int addForcePerTime
		{			
			set;			
			get;		
		}

	}
}