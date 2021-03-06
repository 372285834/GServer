/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/MartialItem.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class MartialItemTable : IDataModel	
	{
		public Dictionary<int, MartialItemData> Dict = new Dictionary<int, MartialItemData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				MartialItemData data = new MartialItemData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public MartialItemData this[int id]
        {
            get 
            {
                MartialItemData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}