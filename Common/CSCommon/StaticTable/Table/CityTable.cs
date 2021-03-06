/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/City.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class CityTable : IDataModel	
	{
		public Dictionary<int, CityData> Dict = new Dictionary<int, CityData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				CityData data = new CityData(array[i]);
				
				Dict[data.id] = data;
			}
		}
        public CityData this[int id]
        {
            get 
            {
                CityData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}