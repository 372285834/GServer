/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/SkillPassive.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class SkillPassiveData:ISkillTable{
		public SkillPassiveData()		
		{				
		}

		public SkillPassiveData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				profession = StaticDataManager.stringToInt(array[1]);
				name = array[2];
				type = StaticDataManager.stringToInt(array[3]);
				maxLv = StaticDataManager.stringToInt(array[4]);
				icon = array[5];
					
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
		//技能类型;		
		public int type
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
		//图标;		
		public string icon
		{			
			set;			
			get;		
		}

	}
}