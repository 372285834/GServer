/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/TaskTalk.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class TaskTalkTable : IDataModel	
	{
		public Dictionary<int, TaskTalkData> Dict = new Dictionary<int, TaskTalkData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				TaskTalkData data = new TaskTalkData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public TaskTalkData this[int id]
        {
            get 
            {
                TaskTalkData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}