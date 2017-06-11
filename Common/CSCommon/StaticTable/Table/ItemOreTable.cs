/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemOre.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class ItemOreTable : IDataModel	
	{
		public Dictionary<int, ItemOreData> Dict = new Dictionary<int, ItemOreData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				ItemOreData data = new ItemOreData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public ItemOreData this[int id]
        {
            get 
            {
                ItemOreData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}