/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/PlayerTpl.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class PlayerTplData{
		public PlayerTplData()		
		{				
		}

		public PlayerTplData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				name = array[1];
				career = StaticDataManager.stringToInt(array[2]);
				prefab = array[3];
				sex = StaticDataManager.stringToInt(array[4]);
				attribute = StaticDataManager.stringToInt(array[5]);
				describe = array[6];
				speed = StaticDataManager.stringToFloat(array[7]);
				skill1 = StaticDataManager.stringToInt(array[8]);
				skill2 = StaticDataManager.stringToInt(array[9]);
				skill3 = StaticDataManager.stringToInt(array[10]);
				skill4 = StaticDataManager.stringToInt(array[11]);
				skill5 = StaticDataManager.stringToInt(array[12]);
				weapon = StaticDataManager.stringToInt(array[13]);
				head = StaticDataManager.stringToInt(array[14]);
				chest = StaticDataManager.stringToInt(array[15]);
				cuff = StaticDataManager.stringToInt(array[16]);
				belt = StaticDataManager.stringToInt(array[17]);
				shoes = StaticDataManager.stringToInt(array[18]);
				necklace = StaticDataManager.stringToInt(array[19]);
				ring1 = StaticDataManager.stringToInt(array[20]);
				ring2 = StaticDataManager.stringToInt(array[21]);
				jadependant = StaticDataManager.stringToInt(array[22]);
				chestfashion = StaticDataManager.stringToInt(array[23]);
				weaponfashion = StaticDataManager.stringToInt(array[24]);
					
		}
				
		//id;		
		public int id
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
		//门派;		
		public int career
		{			
			set;			
			get;		
		}		
		//模型;		
		public string prefab
		{			
			set;			
			get;		
		}		
		//性别;		
		public int sex
		{			
			set;			
			get;		
		}		
		//属性;		
		public int attribute
		{			
			set;			
			get;		
		}		
		//门派描述;		
		public string describe
		{			
			set;			
			get;		
		}		
		//速度;		
		public float speed
		{			
			set;			
			get;		
		}		
		//技能1;		
		public int skill1
		{			
			set;			
			get;		
		}		
		//技能2;		
		public int skill2
		{			
			set;			
			get;		
		}		
		//技能3;		
		public int skill3
		{			
			set;			
			get;		
		}		
		//技能4;		
		public int skill4
		{			
			set;			
			get;		
		}		
		//技能5;		
		public int skill5
		{			
			set;			
			get;		
		}		
		//武器;		
		public int weapon
		{			
			set;			
			get;		
		}		
		//头盔;		
		public int head
		{			
			set;			
			get;		
		}		
		//衣服;		
		public int chest
		{			
			set;			
			get;		
		}		
		//护手;		
		public int cuff
		{			
			set;			
			get;		
		}		
		//腰带;		
		public int belt
		{			
			set;			
			get;		
		}		
		//鞋子;		
		public int shoes
		{			
			set;			
			get;		
		}		
		//项链;		
		public int necklace
		{			
			set;			
			get;		
		}		
		//戒指1;		
		public int ring1
		{			
			set;			
			get;		
		}		
		//戒指2;		
		public int ring2
		{			
			set;			
			get;		
		}		
		//玉佩;		
		public int jadependant
		{			
			set;			
			get;		
		}		
		//衣服时装;		
		public int chestfashion
		{			
			set;			
			get;		
		}		
		//武器时装;		
		public int weaponfashion
		{			
			set;			
			get;		
		}

	}
}