/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/MartialItem.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class MartialItemData{
		public MartialItemData()		
		{				
		}

		public MartialItemData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				type = StaticDataManager.stringToInt(array[1]);
				tempId = StaticDataManager.stringToInt(array[2]);
				icon = array[3];
				openLevel = StaticDataManager.stringToInt(array[4]);
				cost = StaticDataManager.stringToInt(array[5]);
				name = array[6];
				awardCount = StaticDataManager.stringToInt(array[7]);
					
		}
				
		//产出物品id;		
		public int id
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
		//对应id;		
		public int tempId
		{			
			set;			
			get;		
		}		
		//图标;		
		public string icon
		{			
			set;			
			get;		
		}		
		//开放等级;		
		public int openLevel
		{			
			set;			
			get;		
		}		
		//产出消耗;		
		public int cost
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
		//领取次数上限;		
		public int awardCount
		{			
			set;			
			get;		
		}

	}
}