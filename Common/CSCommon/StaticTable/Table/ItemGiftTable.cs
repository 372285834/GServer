/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemGift.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class ItemGiftTable : IDataModel	
	{
		public Dictionary<int, ItemGiftData> Dict = new Dictionary<int, ItemGiftData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				ItemGiftData data = new ItemGiftData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public ItemGiftData this[int id]
        {
            get 
            {
                ItemGiftData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}