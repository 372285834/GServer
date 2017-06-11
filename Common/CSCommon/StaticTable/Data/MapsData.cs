/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/Maps.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class MapsData{
		public MapsData()		
		{				
		}

		public MapsData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				name = array[1];
				nickName = array[2];
				sizeX = StaticDataManager.stringToUint(array[3]);
				sizeZ = StaticDataManager.stringToUint(array[4]);
				maxPlayerCount = StaticDataManager.stringToInt(array[5]);
				mapType = StaticDataManager.stringToInt(array[6]);
				camp = StaticDataManager.stringToInt(array[7]);
				startX = StaticDataManager.stringToFloat(array[8]);
				startY = StaticDataManager.stringToFloat(array[9]);
				width = StaticDataManager.stringToFloat(array[10]);
				height = StaticDataManager.stringToFloat(array[11]);
				ulX = StaticDataManager.stringToFloat(array[12]);
				ulZ = StaticDataManager.stringToFloat(array[13]);
				rdX = StaticDataManager.stringToFloat(array[14]);
				rdZ = StaticDataManager.stringToFloat(array[15]);
					
		}
				
		//id;		
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
		//客户端显示名字;		
		public string nickName
		{			
			set;			
			get;		
		}		
		//x方向大小;		
		public uint sizeX
		{			
			set;			
			get;		
		}		
		//z方向大小;		
		public uint sizeZ
		{			
			set;			
			get;		
		}		
		//容纳最大玩家数;		
		public int maxPlayerCount
		{			
			set;			
			get;		
		}		
		//地图类型;		
		public int mapType
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
		//默认进入x;		
		public float startX
		{			
			set;			
			get;		
		}		
		//默认进入y;		
		public float startY
		{			
			set;			
			get;		
		}		
		//小地图宽;		
		public float width
		{			
			set;			
			get;		
		}		
		//小地图高;		
		public float height
		{			
			set;			
			get;		
		}		
		//左上X;		
		public float ulX
		{			
			set;			
			get;		
		}		
		//左上Z;		
		public float ulZ
		{			
			set;			
			get;		
		}		
		//右下X;		
		public float rdX
		{			
			set;			
			get;		
		}		
		//右下Z;		
		public float rdZ
		{			
			set;			
			get;		
		}

	}
}