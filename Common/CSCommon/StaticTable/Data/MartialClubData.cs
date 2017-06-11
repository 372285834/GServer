/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/MartialClub.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class MartialClubData:IBuildBase{
		public MartialClubData()		
		{				
		}

		public MartialClubData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				name = array[1];
				output = array[2];
				cost = array[3];
					
		}
				
		//等级;		
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
		//产出;		
		public string output
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

	}
}