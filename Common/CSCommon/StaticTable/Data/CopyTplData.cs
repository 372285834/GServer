/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/CopyTpl.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class CopyTplData{
		public CopyTplData()		
		{				
		}

		public CopyTplData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				copylevel = StaticDataManager.stringToInt(array[1]);
				name = array[2];
				target = array[3];
				mapid = StaticDataManager.stringToInt(array[4]);
				playerlevel = StaticDataManager.stringToInt(array[5]);
				CurrencyReward = array[6];
				ItemsReward = array[7];
				logic = StaticDataManager.stringToInt(array[8]);
				time = StaticDataManager.stringToFloat(array[9]);
				targetId = StaticDataManager.stringToInt(array[10]);
				arg1 = array[11];
					
		}
				
		//副本id;		
		public int id
		{			
			set;			
			get;		
		}		
		//关卡;		
		public int copylevel
		{			
			set;			
			get;		
		}		
		//关卡名称;		
		public string name
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
		//地图id;		
		public int mapid
		{			
			set;			
			get;		
		}		
		//等级;		
		public int playerlevel
		{			
			set;			
			get;		
		}		
		//奖励货币字符串;		
		public string CurrencyReward
		{			
			set;			
			get;		
		}		
		//奖励物品字符串;		
		public string ItemsReward
		{			
			set;			
			get;		
		}		
		//目标逻辑;		
		public int logic
		{			
			set;			
			get;		
		}		
		//时间限制;		
		public float time
		{			
			set;			
			get;		
		}		
		//目标id;		
		public int targetId
		{			
			set;			
			get;		
		}		
		//arg1;		
		public string arg1
		{			
			set;			
			get;		
		}

	}
}