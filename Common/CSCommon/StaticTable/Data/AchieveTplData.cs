/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/AchieveTpl.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class AchieveTplData:IAchieveBase{
		public AchieveTplData()		
		{				
		}

		public AchieveTplData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				atype = StaticDataManager.stringToInt(array[1]);
				name = array[2];
				chapter = StaticDataManager.stringToInt(array[3]);
				desc = array[4];
				itemsReward = array[5];
				currencyReward = array[6];
				type = StaticDataManager.stringToInt(array[7]);
				targetNum = StaticDataManager.stringToInt(array[8]);
				target = array[9];
					
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
		//章节;		
		public int chapter
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
		//物品奖励;		
		public string itemsReward
		{			
			set;			
			get;		
		}		
		//货币奖励;		
		public string currencyReward
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

	}
}