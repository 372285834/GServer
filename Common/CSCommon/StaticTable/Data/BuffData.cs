/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/Buff.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class BuffData{
		public BuffData()		
		{				
		}

		public BuffData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				name = array[1];
				category = StaticDataManager.stringToInt(array[2]);
				type = StaticDataManager.stringToInt(array[3]);
				debuff = StaticDataManager.stringToInt(array[4]);
				maxLevel = StaticDataManager.stringToInt(array[5]);
				assId = StaticDataManager.stringToInt(array[6]);
				assLevel = StaticDataManager.stringToInt(array[7]);
				replaceMode = StaticDataManager.stringToInt(array[8]);
				showPrior = StaticDataManager.stringToInt(array[9]);
				des = array[10];
					
		}
				
		//BuffID;		
		public int id
		{			
			set;			
			get;		
		}		
		//Buff名称;		
		public string name
		{			
			set;			
			get;		
		}		
		//Buff分类;		
		public int category
		{			
			set;			
			get;		
		}		
		//Buff类型;		
		public int type
		{			
			set;			
			get;		
		}		
		//是否debuff;		
		public int debuff
		{			
			set;			
			get;		
		}		
		//buff最高等级;		
		public int maxLevel
		{			
			set;			
			get;		
		}		
		//后续BuffID;		
		public int assId
		{			
			set;			
			get;		
		}		
		//后续Buff等级;		
		public int assLevel
		{			
			set;			
			get;		
		}		
		//同buff替换模式;		
		public int replaceMode
		{			
			set;			
			get;		
		}		
		//显示互斥优先级;		
		public int showPrior
		{			
			set;			
			get;		
		}		
		//buff描述;		
		public string des
		{			
			set;			
			get;		
		}

	}
}