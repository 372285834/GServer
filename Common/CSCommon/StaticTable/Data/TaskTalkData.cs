/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/TaskTalk.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class TaskTalkData{
		public TaskTalkData()		
		{				
		}

		public TaskTalkData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				content = array[1];
					
		}
				
		//对话id;		
		public int id
		{			
			set;			
			get;		
		}		
		//对话内容;		
		public string content
		{			
			set;			
			get;		
		}

	}
}