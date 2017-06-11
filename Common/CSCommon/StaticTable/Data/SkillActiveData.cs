/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/SkillActive.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class SkillActiveData:ISkillTable{
		public SkillActiveData()		
		{				
		}

		public SkillActiveData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				profession = StaticDataManager.stringToInt(array[1]);
				name = array[2];
				relation = StaticDataManager.stringToInt(array[3]);
				relationTime = Array.ConvertAll(array[4].Split('|'), new Converter<String, float>(StaticDataManager.stringToFloat));
				resMan = array[5];
				resWoman = array[6];
				actionIndex = StaticDataManager.stringToByte(array[7]);
				type = StaticDataManager.stringToInt(array[8]);
				checker = StaticDataManager.stringToInt(array[9]);
				consumer = StaticDataManager.stringToInt(array[10]);
				action = StaticDataManager.stringToInt(array[11]);
				selector = StaticDataManager.stringToInt(array[12]);
				selparam1 = StaticDataManager.stringToFloat(array[13]);
				selparam2 = StaticDataManager.stringToFloat(array[14]);
				preId = StaticDataManager.stringToInt(array[15]);
				maxLv = StaticDataManager.stringToInt(array[16]);
				range = StaticDataManager.stringToFloat(array[17]);
				cdTime = StaticDataManager.stringToFloat(array[18]);
				protectTime = StaticDataManager.stringToFloat(array[19]);
				tarType = StaticDataManager.stringToByte(array[20]);
				throwEffectSpeed = StaticDataManager.stringToFloat(array[21]);
				icon = array[22];
					
		}
				
		//技能ID;		
		public int id
		{			
			set;			
			get;		
		}		
		//职业;		
		public int profession
		{			
			set;			
			get;		
		}		
		//技能名称;		
		public string name
		{			
			set;			
			get;		
		}		
		//技能关联id;		
		public int relation
		{			
			set;			
			get;		
		}		
		//关联时间;		
		public float[] relationTime
		{			
			set;			
			get;		
		}		
		//资源男;		
		public string resMan
		{			
			set;			
			get;		
		}		
		//资源女;		
		public string resWoman
		{			
			set;			
			get;		
		}		
		//技能动作序号;		
		public byte actionIndex
		{			
			set;			
			get;		
		}		
		//技能类型;		
		public int type
		{			
			set;			
			get;		
		}		
		//技能检查逻辑;		
		public int checker
		{			
			set;			
			get;		
		}		
		//技能消耗逻辑;		
		public int consumer
		{			
			set;			
			get;		
		}		
		//技能行动逻辑;		
		public int action
		{			
			set;			
			get;		
		}		
		//技能选择目标逻辑;		
		public int selector
		{			
			set;			
			get;		
		}		
		//技能选择目标参数1;		
		public float selparam1
		{			
			set;			
			get;		
		}		
		//技能选择目标参数2;		
		public float selparam2
		{			
			set;			
			get;		
		}		
		//前置技能;		
		public int preId
		{			
			set;			
			get;		
		}		
		//技能最高等级;		
		public int maxLv
		{			
			set;			
			get;		
		}		
		//释放距离;		
		public float range
		{			
			set;			
			get;		
		}		
		//冷却时间;		
		public float cdTime
		{			
			set;			
			get;		
		}		
		//保护时间;		
		public float protectTime
		{			
			set;			
			get;		
		}		
		//目标类型;		
		public byte tarType
		{			
			set;			
			get;		
		}		
		//投掷物移动速度;		
		public float throwEffectSpeed
		{			
			set;			
			get;		
		}		
		//技能图标;		
		public string icon
		{			
			set;			
			get;		
		}

	}
}