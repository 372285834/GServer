/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemTpl.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class ItemTplTable : IDataModel	
	{
		public Dictionary<int, ItemTplData> Dict = new Dictionary<int, ItemTplData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				ItemTplData data = new ItemTplData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public ItemTplData this[int id]
        {
            get 
            {
                ItemTplData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}