/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemGem.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class ItemGemTable : IDataModel	
	{
		public Dictionary<int, ItemGemData> Dict = new Dictionary<int, ItemGemData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				ItemGemData data = new ItemGemData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public ItemGemData this[int id]
        {
            get 
            {
                ItemGemData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}