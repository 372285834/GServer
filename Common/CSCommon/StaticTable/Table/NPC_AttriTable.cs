/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/NPC_Attri.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{
    public struct NpcTypeLevelPair
    {
        public int type;
        public int level;
    }
	public class NPC_AttriTable : IDataModel	
	{
        public Dictionary<NpcTypeLevelPair, NPC_AttriData> Dict = new Dictionary<NpcTypeLevelPair, NPC_AttriData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				NPC_AttriData data = new NPC_AttriData(array[i]);
                NpcTypeLevelPair pair;
                pair.type = data.type;
                pair.level = data.level;
                Dict[pair] = data;
            }
        }
        public NPC_AttriData this[int roleType, int level]
        {
            get
            {
                NpcTypeLevelPair pair;
                pair.type = roleType;
                pair.level = level;
                NPC_AttriData data;
                Dict.TryGetValue(pair, out data);
                return data;
            }
        }			
	}
}