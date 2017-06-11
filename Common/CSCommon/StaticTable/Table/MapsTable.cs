/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/Maps.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class MapsTable : IDataModel	
	{
		public Dictionary<int, MapsData> Dict = new Dictionary<int, MapsData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				MapsData data = new MapsData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public MapsData this[int id]
        {
            get 
            {
                MapsData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}