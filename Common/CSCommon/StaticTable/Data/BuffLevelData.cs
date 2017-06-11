/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/BuffLevel.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class BuffLevelData{
		public BuffLevelData()		
		{				
		}

		public BuffLevelData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				level = StaticDataManager.stringToInt(array[1]);
				affectTime = StaticDataManager.stringToFloat(array[2]);
				affectRate = StaticDataManager.stringToFloat(array[3]);
				attri1 = StaticDataManager.stringToInt(array[4]);
				value1 = StaticDataManager.stringToFloat(array[5]);
				attri2 = StaticDataManager.stringToInt(array[6]);
				value2 = StaticDataManager.stringToFloat(array[7]);
				attri3 = StaticDataManager.stringToInt(array[8]);
				value3 = StaticDataManager.stringToFloat(array[9]);
				vfx = array[10];
					
		}
				
		//BuffID;		
		public int id
		{			
			set;			
			get;		
		}		
		//等级;		
		public int level
		{			
			set;			
			get;		
		}		
		//作用时间;		
		public float affectTime
		{			
			set;			
			get;		
		}		
		//作用频率;		
		public float affectRate
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
		public float value1
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
		public float value2
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
		public float value3
		{			
			set;			
			get;		
		}		
		//特效;		
		public string vfx
		{			
			set;			
			get;		
		}

	}
}