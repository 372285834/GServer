/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemUse.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class ItemUseData{
		public ItemUseData()		
		{				
		}

		public ItemUseData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				UseIsDelete = StaticDataManager.stringToByte(array[1]);
				ItemUseMethod = array[2];
				Arg1 = array[3];
				Arg2 = array[4];
				Arg3 = array[5];
					
		}
				
		//物品使用id;		
		public int id
		{			
			set;			
			get;		
		}		
		//物品使用后删除该物品;		
		public byte UseIsDelete
		{			
			set;			
			get;		
		}		
		//物品使用调用方法;		
		public string ItemUseMethod
		{			
			set;			
			get;		
		}		
		//参数1;		
		public string Arg1
		{			
			set;			
			get;		
		}		
		//参数2;		
		public string Arg2
		{			
			set;			
			get;		
		}		
		//参数3;		
		public string Arg3
		{			
			set;			
			get;		
		}

	}
}