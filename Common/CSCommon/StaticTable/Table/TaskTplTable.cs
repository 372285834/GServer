/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/TaskTpl.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class TaskTplTable : IDataModel	
	{
		public Dictionary<int, TaskTplData> Dict = new Dictionary<int, TaskTplData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				TaskTplData data = new TaskTplData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public TaskTplData this[int id]
        {
            get 
            {
                TaskTplData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}