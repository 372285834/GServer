/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/DailyCom.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class DailyComTable : IDataModel	
	{
		public Dictionary<int, DailyComData> Dict = new Dictionary<int, DailyComData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				DailyComData data = new DailyComData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public DailyComData this[int id]
        {
            get 
            {
                DailyComData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}