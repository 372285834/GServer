/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/TaskTpl.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class TaskTplData{
		public TaskTplData()		
		{				
		}

		public TaskTplData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				name = array[1];
				Target = array[2];
				FinishedTarget = array[3];
				Describe = array[4];
				NextId = StaticDataManager.stringToInt(array[5]);
				MapId = StaticDataManager.stringToInt(array[6]);
				posX = StaticDataManager.stringToFloat(array[7]);
				posZ = StaticDataManager.stringToFloat(array[8]);
				FinishMapId = StaticDataManager.stringToInt(array[9]);
				FinishNpcId = StaticDataManager.stringToInt(array[10]);
				CurrencyReward = array[11];
				ItemsReward = array[12];
				EventType = StaticDataManager.stringToInt(array[13]);
				arg1 = array[14];
				arg2 = array[15];
				arg3 = array[16];
				talkId1 = StaticDataManager.stringToInt(array[17]);
				talkId2 = StaticDataManager.stringToInt(array[18]);
				func = StaticDataManager.stringToInt(array[19]);
					
		}
				
		//任务ID;		
		public int id
		{			
			set;			
			get;		
		}		
		//任务名称;		
		public string name
		{			
			set;			
			get;		
		}		
		//任务目标;		
		public string Target
		{			
			set;			
			get;		
		}		
		//完成任务后的任务目标;		
		public string FinishedTarget
		{			
			set;			
			get;		
		}		
		//任务描述;		
		public string Describe
		{			
			set;			
			get;		
		}		
		//下一个任务id;		
		public int NextId
		{			
			set;			
			get;		
		}		
		//寻路地图;		
		public int MapId
		{			
			set;			
			get;		
		}		
		//寻路坐标X;		
		public float posX
		{			
			set;			
			get;		
		}		
		//寻路坐标Z;		
		public float posZ
		{			
			set;			
			get;		
		}		
		//完成任务地图;		
		public int FinishMapId
		{			
			set;			
			get;		
		}		
		//完成任务NPCid;		
		public int FinishNpcId
		{			
			set;			
			get;		
		}		
		//奖励货币字符串;		
		public string CurrencyReward
		{			
			set;			
			get;		
		}		
		//奖励物品字符串;		
		public string ItemsReward
		{			
			set;			
			get;		
		}		
		//任务事件类型;		
		public int EventType
		{			
			set;			
			get;		
		}		
		//事件参数1;		
		public string arg1
		{			
			set;			
			get;		
		}		
		//事件参数2;		
		public string arg2
		{			
			set;			
			get;		
		}		
		//事件参数3;		
		public string arg3
		{			
			set;			
			get;		
		}		
		//对话id;		
		public int talkId1
		{			
			set;			
			get;		
		}		
		//完成对话id;		
		public int talkId2
		{			
			set;			
			get;		
		}		
		//任务点击;		
		public int func
		{			
			set;			
			get;		
		}

	}
}