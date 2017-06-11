/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemEquip.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class ItemEquipTable : IDataModel	
	{
		public Dictionary<int, ItemEquipData> Dict = new Dictionary<int, ItemEquipData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				ItemEquipData data = new ItemEquipData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public ItemEquipData this[int id]
        {
            get 
            {
                ItemEquipData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}