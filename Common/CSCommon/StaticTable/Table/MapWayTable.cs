/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/MapWay.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class MapWayTable : IDataModel	
	{
		public Dictionary<int, MapWayData> Dict = new Dictionary<int, MapWayData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				MapWayData data = new MapWayData(array[i]);
				
				Dict[data.id] = data;
			}
		}
        public MapWayData this[int id]
        {
            get 
            {
                MapWayData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}