/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/Cheats.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class CheatsTable : IDataModel	
	{
        public Dictionary<SkillIdLevelPair, CheatsData> Dict = new Dictionary<SkillIdLevelPair, CheatsData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				CheatsData data = new CheatsData(array[i]);

                SkillIdLevelPair pair;
                pair.id = data.id;
                pair.level = data.level;
                Dict[pair] = data;
			
			}
		}
        public CheatsData this[int id, int lv]
        {
            get 
            {
                SkillIdLevelPair pair;
                pair.id = id;
                pair.level = lv;
                CheatsData data;
                Dict.TryGetValue(pair, out data);
                return data;
            }
        }		
	}
}