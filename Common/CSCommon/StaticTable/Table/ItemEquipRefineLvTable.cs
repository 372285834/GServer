/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemEquipRefineLv.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{
    public struct IdLevelPair
    {
        public int id;
        public int level;
    }
	public class ItemEquipRefineLvTable : IDataModel	
	{
        public Dictionary<IdLevelPair, ItemEquipRefineLvData> Dict = new Dictionary<IdLevelPair, ItemEquipRefineLvData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				ItemEquipRefineLvData data = new ItemEquipRefineLvData(array[i]);

                IdLevelPair pair;
                pair.id = data.id;
                pair.level = data.level;
                Dict[pair] = data;
                Dict[pair] = data;
			
			}
		}
        public ItemEquipRefineLvData this[int id, int lv]
        {
            get
            {
                IdLevelPair pair;
                pair.id = id;
                pair.level = lv;
                ItemEquipRefineLvData data = null;
                Dict.TryGetValue(pair, out data);
                return data;
            }
        }		
	}
}