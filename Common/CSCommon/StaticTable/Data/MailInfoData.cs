/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/MailInfo.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class MailInfoData{
		public MailInfoData()		
		{				
		}

		public MailInfoData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				Type = array[1];
				Title = array[2];
				Content = array[3];
					
		}
				
		//邮件信息id;		
		public int id
		{			
			set;			
			get;		
		}		
		//类型;		
		public string Type
		{			
			set;			
			get;		
		}		
		//标题;		
		public string Title
		{			
			set;			
			get;		
		}		
		//内容;		
		public string Content
		{			
			set;			
			get;		
		}

	}
}