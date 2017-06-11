/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/NPC_Attri.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class NPC_AttriData{
		public NPC_AttriData()		
		{				
		}

		public NPC_AttriData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			type = StaticDataManager.stringToInt(array[0]);
				level = StaticDataManager.stringToInt(array[1]);
				hp = StaticDataManager.stringToInt(array[2]);
				atk = StaticDataManager.stringToInt(array[3]);
				goldDef = StaticDataManager.stringToInt(array[4]);
				woodDef = StaticDataManager.stringToInt(array[5]);
				waterDef = StaticDataManager.stringToInt(array[6]);
				fireDef = StaticDataManager.stringToInt(array[7]);
				soilDef = StaticDataManager.stringToInt(array[8]);
				hitVal = StaticDataManager.stringToInt(array[9]);
				dodgeVal = StaticDataManager.stringToInt(array[10]);
				speed = StaticDataManager.stringToFloat(array[11]);
				critVal = StaticDataManager.stringToInt(array[12]);
				defCritVal = StaticDataManager.stringToInt(array[13]);
				hpRecovery = StaticDataManager.stringToInt(array[14]);
				exp = StaticDataManager.stringToInt(array[15]);
					
		}
				
		//怪物类型;		
		public int type
		{			
			set;			
			get;		
		}		
		//怪物等级;		
		public int level
		{			
			set;			
			get;		
		}		
		//生命值;		
		public int hp
		{			
			set;			
			get;		
		}		
		//攻击力;		
		public int atk
		{			
			set;			
			get;		
		}		
		//金防御;		
		public int goldDef
		{			
			set;			
			get;		
		}		
		//木防御;		
		public int woodDef
		{			
			set;			
			get;		
		}		
		//水防御;		
		public int waterDef
		{			
			set;			
			get;		
		}		
		//火防御;		
		public int fireDef
		{			
			set;			
			get;		
		}		
		//土防御;		
		public int soilDef
		{			
			set;			
			get;		
		}		
		//命中值;		
		public int hitVal
		{			
			set;			
			get;		
		}		
		//闪避值;		
		public int dodgeVal
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
		//暴击率;		
		public int critVal
		{			
			set;			
			get;		
		}		
		//暴抗率;		
		public int defCritVal
		{			
			set;			
			get;		
		}		
		//HP恢复;		
		public int hpRecovery
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

	}
}