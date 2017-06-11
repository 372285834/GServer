/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemEquipLevel.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{
    public struct EquipLevelKey
    {
        public int id;//门派id
        public int tid;//部位id
        public int level;//装备等级
    }
	public class ItemEquipLevelTable : IDataModel	
	{
        public Dictionary<EquipLevelKey, ItemEquipLevelData> Dict = new Dictionary<EquipLevelKey, ItemEquipLevelData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				ItemEquipLevelData data = new ItemEquipLevelData(array[i]);

                EquipLevelKey pair;
                pair.id = data.id;
                pair.tid = data.tid;
                pair.level = data.level;
                Dict[pair] = data;
			}
		}
        public ItemEquipLevelData this[int id, int tid, int lv]
        {
            get
            {
                EquipLevelKey pair;
                pair.id = id;
                pair.tid = tid;
                pair.level = lv;
                ItemEquipLevelData data;
                Dict.TryGetValue(pair, out data);
                return data;
            }
        }	
	}
}