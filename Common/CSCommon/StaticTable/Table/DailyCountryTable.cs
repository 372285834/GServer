/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/DailyCountry.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class DailyCountryTable : IDataModel	
	{
		public Dictionary<int, DailyCountryData> Dict = new Dictionary<int, DailyCountryData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				DailyCountryData data = new DailyCountryData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public DailyCountryData this[int id]
        {
            get 
            {
                DailyCountryData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}