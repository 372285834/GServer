/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/PlayerLevel.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class PlayerLevelData{
		public PlayerLevelData()		
		{				
		}

		public PlayerLevelData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			level = StaticDataManager.stringToInt(array[0]);
				pro = StaticDataManager.stringToByte(array[1]);
				exp = StaticDataManager.stringToInt(array[2]);
				power = StaticDataManager.stringToFloat(array[3]);
				body = StaticDataManager.stringToFloat(array[4]);
				dex = StaticDataManager.stringToFloat(array[5]);
				hp = StaticDataManager.stringToInt(array[6]);
				mp = StaticDataManager.stringToInt(array[7]);
				atk = StaticDataManager.stringToInt(array[8]);
				gold = StaticDataManager.stringToInt(array[9]);
				wood = StaticDataManager.stringToInt(array[10]);
				water = StaticDataManager.stringToInt(array[11]);
				fire = StaticDataManager.stringToInt(array[12]);
				earth = StaticDataManager.stringToInt(array[13]);
				hit = StaticDataManager.stringToInt(array[14]);
				dodge = StaticDataManager.stringToInt(array[15]);
				point = StaticDataManager.stringToInt(array[16]);
					
		}
				
		//等级;		
		public int level
		{			
			set;			
			get;		
		}		
		//职业;		
		public byte pro
		{			
			set;			
			get;		
		}		
		//经验;		
		public int exp
		{			
			set;			
			get;		
		}		
		//内功;		
		public float power
		{			
			set;			
			get;		
		}		
		//外功;		
		public float body
		{			
			set;			
			get;		
		}		
		//身法;		
		public float dex
		{			
			set;			
			get;		
		}		
		//生命;		
		public int hp
		{			
			set;			
			get;		
		}		
		//内力;		
		public int mp
		{			
			set;			
			get;		
		}		
		//攻击;		
		public int atk
		{			
			set;			
			get;		
		}		
		//金;		
		public int gold
		{			
			set;			
			get;		
		}		
		//木;		
		public int wood
		{			
			set;			
			get;		
		}		
		//水;		
		public int water
		{			
			set;			
			get;		
		}		
		//火;		
		public int fire
		{			
			set;			
			get;		
		}		
		//土;		
		public int earth
		{			
			set;			
			get;		
		}		
		//命中;		
		public int hit
		{			
			set;			
			get;		
		}		
		//闪避;		
		public int dodge
		{			
			set;			
			get;		
		}		
		//潜能点;		
		public int point
		{			
			set;			
			get;		
		}

	}
}