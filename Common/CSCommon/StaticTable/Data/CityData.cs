/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/City.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class CityData{
		public CityData()		
		{				
		}

		public CityData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				addForceLevel = StaticDataManager.stringToInt(array[1]);
				x = StaticDataManager.stringToInt(array[2]);
				y = StaticDataManager.stringToInt(array[3]);
				mapId = StaticDataManager.stringToInt(array[4]);
					
		}
				
		//id;		
		public int id
		{			
			set;			
			get;		
		}		
		//可发兵等级;		
		public int addForceLevel
		{			
			set;			
			get;		
		}		
		//在国战场景中的位置X;		
		public int x
		{			
			set;			
			get;		
		}		
		//在国战场景中的位置Y;		
		public int y
		{			
			set;			
			get;		
		}		
		//地图ID;		
		public int mapId
		{			
			set;			
			get;		
		}

	}
}