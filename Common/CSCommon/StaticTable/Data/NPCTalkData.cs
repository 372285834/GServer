/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/NPCTalk.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class NPCTalkData{
		public NPCTalkData()		
		{				
		}

		public NPCTalkData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				referral = array[1];
					
		}
				
		//文字ID;		
		public int id
		{			
			set;			
			get;		
		}		
		//加载文字;		
		public string referral
		{			
			set;			
			get;		
		}

	}
}