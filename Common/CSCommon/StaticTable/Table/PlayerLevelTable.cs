/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/PlayerLevel.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{
    public struct LevelProPair
    {
        public int lv;
        public byte pro;
    }

	public class PlayerLevelTable : IDataModel	
	{
        public Dictionary<LevelProPair, PlayerLevelData> Dict = new Dictionary<LevelProPair, PlayerLevelData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				PlayerLevelData data = new PlayerLevelData(array[i]);

                LevelProPair pair;
                pair.lv = data.level;
                pair.pro = data.pro;
                Dict[pair] = data;
            }
        }
        public PlayerLevelData this[int lv, byte pro]
        {
            get
            {
                LevelProPair pair;
                pair.lv = lv;
                pair.pro = pro;
                PlayerLevelData data;
                Dict.TryGetValue(pair, out data);
                return data;
            }
        }		
	}
}