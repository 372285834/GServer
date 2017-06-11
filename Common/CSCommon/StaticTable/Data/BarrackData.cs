/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/Barrack.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class BarrackData{
		public BarrackData()		
		{				
		}

		public BarrackData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				trainSpeed = StaticDataManager.stringToInt(array[1]);
				upgradeMoney = array[2];
					
		}
				
		//兵营等级;		
		public int id
		{			
			set;			
			get;		
		}		
		//训练速度(人/分）;		
		public int trainSpeed
		{			
			set;			
			get;		
		}		
		//升级货币;		
		public string upgradeMoney
		{			
			set;			
			get;		
		}

	}
}