/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/BuffLevel.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{
    public struct BuffIdLevelPair
    {
        public int id;
        public int level;
    }
	public class BuffLevelTable : IDataModel	
	{
        public Dictionary<BuffIdLevelPair, BuffLevelData> Dict = new Dictionary<BuffIdLevelPair, BuffLevelData>();
		public void initData(string content)		
		{
			Dict.Clear();
            string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				BuffLevelData data = new BuffLevelData(array[i]);

                BuffIdLevelPair pair;
                pair.id = data.id;
                pair.level = data.level;
                Dict[pair] = data;
            }
        }
        public BuffLevelData this[int id, int lv]
        {
            get
            {
                BuffIdLevelPair pair;
                pair.id = id;
                pair.level = lv;
                BuffLevelData data;
                Dict.TryGetValue(pair, out data);
                return data;
            }
        }		
	}
}