/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/Cheats.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class CheatsData:ISkillLevelTable{
		public CheatsData()		
		{				
		}

		public CheatsData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				level = StaticDataManager.stringToInt(array[1]);
				attri1 = StaticDataManager.stringToInt(array[2]);
				value1 = StaticDataManager.stringToFloat(array[3]);
				attri2 = StaticDataManager.stringToInt(array[4]);
				value2 = StaticDataManager.stringToFloat(array[5]);
				attri3 = StaticDataManager.stringToInt(array[6]);
				value3 = StaticDataManager.stringToFloat(array[7]);
				goldcost = StaticDataManager.stringToInt(array[8]);
				goldcostrate = StaticDataManager.stringToFloat(array[9]);
				rmbcost = StaticDataManager.stringToInt(array[10]);
				rmbcostrate = StaticDataManager.stringToFloat(array[11]);
				opencondition = StaticDataManager.stringToInt(array[12]);
				des = array[13];
				desParam = array[14];
				cost = array[15];
					
		}
				
		//ID;		
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
		//升级银两消耗;		
		public int goldcost
		{			
			set;			
			get;		
		}		
		//升级银两概率;		
		public float goldcostrate
		{			
			set;			
			get;		
		}		
		//升级元宝消耗;		
		public int rmbcost
		{			
			set;			
			get;		
		}		
		//升级元宝概率;		
		public float rmbcostrate
		{			
			set;			
			get;		
		}		
		//开启条件;		
		public int opencondition
		{			
			set;			
			get;		
		}		
		//技能描述;		
		public string des
		{			
			set;			
			get;		
		}		
		//描述参数;		
		public string desParam
		{			
			set;			
			get;		
		}		
		//升级消耗;		
		public string cost
		{			
			set;			
			get;		
		}

	}
}