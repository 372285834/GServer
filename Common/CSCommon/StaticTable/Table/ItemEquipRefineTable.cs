/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemEquipRefine.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class ItemEquipRefineTable : IDataModel	
	{
		public Dictionary<int, ItemEquipRefineData> Dict = new Dictionary<int, ItemEquipRefineData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				ItemEquipRefineData data = new ItemEquipRefineData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public ItemEquipRefineData this[int id]
        {
            get 
            {
                ItemEquipRefineData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}