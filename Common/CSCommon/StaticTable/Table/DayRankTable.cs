/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/DayRank.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class DayRankTable : IDataModel	
	{
		public Dictionary<int, DayRankData> Dict = new Dictionary<int, DayRankData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				DayRankData data = new DayRankData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public DayRankData this[int id]
        {
            get 
            {
                DayRankData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}