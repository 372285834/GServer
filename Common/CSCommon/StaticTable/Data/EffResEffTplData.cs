/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/EffResEffTpl.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class EffResEffTplData{
		public EffResEffTplData()		
		{				
		}

		public EffResEffTplData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				constName = array[1];
				name = array[2];
					
		}
				
		//id;		
		public int id
		{			
			set;			
			get;		
		}		
		//变量名字;		
		public string constName
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

	}
}