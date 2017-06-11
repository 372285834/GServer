/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/DailyCom.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class DailyComData{
		public DailyComData()		
		{				
		}

		public DailyComData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				Lv = StaticDataManager.stringToInt(array[1]);
				Camp = StaticDataManager.stringToByte(array[2]);
				Type = StaticDataManager.stringToByte(array[3]);
				TempId = StaticDataManager.stringToInt(array[4]);
				Name = array[5];
				Describe = array[6];
				TotalNum = StaticDataManager.stringToByte(array[7]);
					
		}
				
		//Id;		
		public int id
		{			
			set;			
			get;		
		}		
		//等级;		
		public int Lv
		{			
			set;			
			get;		
		}		
		//阵营;		
		public byte Camp
		{			
			set;			
			get;		
		}		
		//类型;		
		public byte Type
		{			
			set;			
			get;		
		}		
		//模板id;		
		public int TempId
		{			
			set;			
			get;		
		}		
		//名称;		
		public string Name
		{			
			set;			
			get;		
		}		
		//描述;		
		public string Describe
		{			
			set;			
			get;		
		}		
		//总环数;		
		public byte TotalNum
		{			
			set;			
			get;		
		}

	}
}