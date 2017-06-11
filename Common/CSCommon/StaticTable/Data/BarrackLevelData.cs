/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/BarrackLevel.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class BarrackLevelData{
		public BarrackLevelData()		
		{				
		}

		public BarrackLevelData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				trainSpeed = StaticDataManager.stringToInt(array[1]);
				upgradeMoney = array[2];
				addForceTimePerDay = StaticDataManager.stringToInt(array[3]);
				addForceMaxPerCity = StaticDataManager.stringToInt(array[4]);
				addForceNumPerTime = StaticDataManager.stringToInt(array[5]);
					
		}
				
		//兵营等级;		
		public int id
		{			
			set;			
			get;		
		}		
		//训练速度(人/分）;		
		public int trainSpeed
		{			
			set;			
			get;		
		}		
		//升级货币;		
		public string upgradeMoney
		{			
			set;			
			get;		
		}		
		//每日发兵次数;		
		public int addForceTimePerDay
		{			
			set;			
			get;		
		}		
		//单个场景发兵上限;		
		public int addForceMaxPerCity
		{			
			set;			
			get;		
		}		
		//单次发兵数量(为0表示不可发兵）;		
		public int addForceNumPerTime
		{			
			set;			
			get;		
		}

	}
}