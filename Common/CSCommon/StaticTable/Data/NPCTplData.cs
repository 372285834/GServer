/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/NPCTpl.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class NPCTplData{
		public NPCTplData()		
		{				
		}

		public NPCTplData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				name = array[1];
				prefab = array[2];
				type = StaticDataManager.stringToByte(array[3]);
				monsterAtt = StaticDataManager.stringToInt(array[4]);
				speed = StaticDataManager.stringToFloat(array[5]);
				AutoAtk = StaticDataManager.stringToInt(array[6]);
				distance = StaticDataManager.stringToInt(array[7]);
				interval = StaticDataManager.stringToInt(array[8]);
				skill = Array.ConvertAll(array[9].Split('|'), new Converter<String, int>(StaticDataManager.stringToInt));
				logic = StaticDataManager.stringToInt(array[10]);
				deadBodyTime = StaticDataManager.stringToFloat(array[11]);
				dropType = StaticDataManager.stringToInt(array[12]);
				parameters = StaticDataManager.stringToInt(array[13]);
				Headmark = StaticDataManager.stringToInt(array[14]);
					
		}
				
		//怪物ID;		
		public int id
		{			
			set;			
			get;		
		}		
		//怪物名称;		
		public string name
		{			
			set;			
			get;		
		}		
		//资源;		
		public string prefab
		{			
			set;			
			get;		
		}		
		//怪物类型;		
		public byte type
		{			
			set;			
			get;		
		}		
		//怪物属性;		
		public int monsterAtt
		{			
			set;			
			get;		
		}		
		//移动速度;		
		public float speed
		{			
			set;			
			get;		
		}		
		//攻击类型;		
		public int AutoAtk
		{			
			set;			
			get;		
		}		
		//攻击距离;		
		public int distance
		{			
			set;			
			get;		
		}		
		//攻击间隔;		
		public int interval
		{			
			set;			
			get;		
		}		
		//怪物技能;		
		public int[] skill
		{			
			set;			
			get;		
		}		
		//怪物逻辑;		
		public int logic
		{			
			set;			
			get;		
		}		
		//躺尸时间;		
		public float deadBodyTime
		{			
			set;			
			get;		
		}		
		//掉落类型;		
		public int dropType
		{			
			set;			
			get;		
		}		
		//掉落参数;		
		public int parameters
		{			
			set;			
			get;		
		}		
		//头顶标志;		
		public int Headmark
		{			
			set;			
			get;		
		}

	}
}