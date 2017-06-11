/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/SkillLevel.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{
    public struct SkillIdLevelPair
    {
        public int id;
        public int level;
    }
	public class SkillLevelTable : IDataModel	
	{
        public Dictionary<SkillIdLevelPair, SkillLevelData> Dict = new Dictionary<SkillIdLevelPair, SkillLevelData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				SkillLevelData data = new SkillLevelData(array[i]);

                SkillIdLevelPair pair;
                pair.id = data.id;
                pair.level = data.level;
                Dict[pair] = data;
            }
        }
        public SkillLevelData this[int id, int lv]
        {
            get
            {
                SkillIdLevelPair pair;
                pair.id = id;
                pair.level = lv;
                SkillLevelData data;
                Dict.TryGetValue(pair, out data);
                return data;
            }
        }		
	}
}