/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/SkillLevel.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class SkillLevelData:ISkillLevelTable{
		public SkillLevelData()		
		{				
		}

		public SkillLevelData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				level = StaticDataManager.stringToInt(array[1]);
				damage = StaticDataManager.stringToInt(array[2]);
				damagePercent = StaticDataManager.stringToFloat(array[3]);
				levelParam = StaticDataManager.stringToFloat(array[4]);
				hitRate = StaticDataManager.stringToFloat(array[5]);
				consume = StaticDataManager.stringToFloat(array[6]);
				areaCount = StaticDataManager.stringToByte(array[7]);
				clientHitTime = Array.ConvertAll(array[8].Split('|'), new Converter<String, float>(StaticDataManager.stringToFloat));
				hitTime = Array.ConvertAll(array[9].Split('|'), new Converter<String, float>(StaticDataManager.stringToFloat));
				buff1ids = StaticDataManager.stringToInt(array[10]);
				buff1level = StaticDataManager.stringToInt(array[11]);
				buff1TarType = StaticDataManager.stringToInt(array[12]);
				buff1Rate = StaticDataManager.stringToFloat(array[13]);
				buff2ids = StaticDataManager.stringToInt(array[14]);
				buff2level = StaticDataManager.stringToInt(array[15]);
				buff2TarType = StaticDataManager.stringToInt(array[16]);
				buff2Rate = StaticDataManager.stringToFloat(array[17]);
				cost = array[18];
				des = array[19];
				desParam = array[20];
					
		}
				
		//技能ID;		
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
		//伤害固定值;		
		public int damage
		{			
			set;			
			get;		
		}		
		//伤害百分比;		
		public float damagePercent
		{			
			set;			
			get;		
		}		
		//等级系数;		
		public float levelParam
		{			
			set;			
			get;		
		}		
		//技能命中;		
		public float hitRate
		{			
			set;			
			get;		
		}		
		//技能消耗;		
		public float consume
		{			
			set;			
			get;		
		}		
		//作用数量;		
		public byte areaCount
		{			
			set;			
			get;		
		}		
		//客户端表现伤害事件;		
		public float[] clientHitTime
		{			
			set;			
			get;		
		}		
		//服务器实际命中时间;		
		public float[] hitTime
		{			
			set;			
			get;		
		}		
		//附加状态1id;		
		public int buff1ids
		{			
			set;			
			get;		
		}		
		//状态等级1;		
		public int buff1level
		{			
			set;			
			get;		
		}		
		//状态1目标类型;		
		public int buff1TarType
		{			
			set;			
			get;		
		}		
		//状态生效1概率;		
		public float buff1Rate
		{			
			set;			
			get;		
		}		
		//附加状态2id;		
		public int buff2ids
		{			
			set;			
			get;		
		}		
		//状态等级2;		
		public int buff2level
		{			
			set;			
			get;		
		}		
		//状态2目标类型;		
		public int buff2TarType
		{			
			set;			
			get;		
		}		
		//状态生效2概率;		
		public float buff2Rate
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

	}
}