/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/DailyYunDart.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class DailyYunDartTable : IDataModel	
	{
		public Dictionary<int, DailyYunDartData> Dict = new Dictionary<int, DailyYunDartData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				DailyYunDartData data = new DailyYunDartData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public DailyYunDartData this[int id]
        {
            get 
            {
                DailyYunDartData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}