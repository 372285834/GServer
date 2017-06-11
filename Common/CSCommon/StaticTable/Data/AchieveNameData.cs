/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/AchieveName.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class AchieveNameData:IAchieveBase{
		public AchieveNameData()		
		{				
		}

		public AchieveNameData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				atype = StaticDataManager.stringToInt(array[1]);
				name = array[2];
				desc = array[3];
				type = StaticDataManager.stringToInt(array[4]);
				targetNum = StaticDataManager.stringToInt(array[5]);
				target = array[6];
				attri1 = StaticDataManager.stringToInt(array[7]);
				value1 = StaticDataManager.stringToFloat(array[8]);
					
		}
				
		//id;		
		public int id
		{			
			set;			
			get;		
		}		
		//成就类型;		
		public int atype
		{			
			set;			
			get;		
		}		
		//名字;		
		public string name
		{			
			set;			
			get;		
		}		
		//描述;		
		public string desc
		{			
			set;			
			get;		
		}		
		//类型;		
		public int type
		{			
			set;			
			get;		
		}		
		//目标值;		
		public int targetNum
		{			
			set;			
			get;		
		}		
		//目标;		
		public string target
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
		//增加属性值;		
		public float value1
		{			
			set;			
			get;		
		}

	}
}