/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/MapWay.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class MapWayData{
		public MapWayData()		
		{				
		}

		public MapWayData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				name = array[1];
				cartype = StaticDataManager.stringToInt(array[2]);
				camp = StaticDataManager.stringToInt(array[3]);
				startmapID = StaticDataManager.stringToInt(array[4]);
				path1 = StaticDataManager.stringToInt(array[5]);
				path2 = StaticDataManager.stringToInt(array[6]);
				path3 = StaticDataManager.stringToInt(array[7]);
				path4 = StaticDataManager.stringToInt(array[8]);
				path5 = StaticDataManager.stringToInt(array[9]);
					
		}
				
		//路线ID;		
		public int id
		{			
			set;			
			get;		
		}		
		//名称;		
		public string name
		{			
			set;			
			get;		
		}		
		//车辆类型;		
		public int cartype
		{			
			set;			
			get;		
		}		
		//阵营;		
		public int camp
		{			
			set;			
			get;		
		}		
		//起始场景ID;		
		public int startmapID
		{			
			set;			
			get;		
		}		
		//路线1;		
		public int path1
		{			
			set;			
			get;		
		}		
		//路线2;		
		public int path2
		{			
			set;			
			get;		
		}		
		//路线3;		
		public int path3
		{			
			set;			
			get;		
		}		
		//路线4;		
		public int path4
		{			
			set;			
			get;		
		}		
		//路线5;		
		public int path5
		{			
			set;			
			get;		
		}

	}
}