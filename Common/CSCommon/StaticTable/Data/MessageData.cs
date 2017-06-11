/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/Message.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class MessageData{
		public MessageData()		
		{				
		}

		public MessageData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				eValue = array[1];
				eType = array[2];
				content = array[3];
				type = StaticDataManager.stringToInt(array[4]);
					
		}
				
		//id;		
		public int id
		{			
			set;			
			get;		
		}		
		//枚举值;		
		public string eValue
		{			
			set;			
			get;		
		}		
		//枚举类型;		
		public string eType
		{			
			set;			
			get;		
		}		
		//内容;		
		public string content
		{			
			set;			
			get;		
		}		
		//类型;		
		public int type
		{			
			set;			
			get;		
		}

	}
}