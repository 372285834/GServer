/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/DailyLake.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class DailyLakeTable : IDataModel	
	{
		public Dictionary<int, DailyLakeData> Dict = new Dictionary<int, DailyLakeData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				DailyLakeData data = new DailyLakeData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public DailyLakeData this[int id]
        {
            get 
            {
                DailyLakeData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}