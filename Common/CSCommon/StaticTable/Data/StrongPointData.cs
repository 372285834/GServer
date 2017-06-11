/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/StrongPoint.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class StrongPointData{
		public StrongPointData()		
		{				
		}

		public StrongPointData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				mapid = StaticDataManager.stringToInt(array[1]);
				pointtemplateid = StaticDataManager.stringToInt(array[2]);
				Soldiertemplateid = StaticDataManager.stringToInt(array[3]);
					
		}
				
		//据点Id;		
		public int id
		{			
			set;			
			get;		
		}		
		//归属地图Id;		
		public int mapid
		{			
			set;			
			get;		
		}		
		//据点npc模板Id;		
		public int pointtemplateid
		{			
			set;			
			get;		
		}		
		//士兵npc模板Id;		
		public int Soldiertemplateid
		{			
			set;			
			get;		
		}

	}
}