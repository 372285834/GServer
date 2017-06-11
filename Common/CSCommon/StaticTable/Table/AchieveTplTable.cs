/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/AchieveTpl.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class AchieveTplTable : IDataModel	
	{
		public Dictionary<int, List<int>> TypeDict = new Dictionary<int, List<int>>();
		public Dictionary<int, AchieveTplData> Dict = new Dictionary<int, AchieveTplData>();
		public void initData(string content)		
		{
			TypeDict.Clear();
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				AchieveTplData data = new AchieveTplData(array[i]);
				
				Dict[data.id] = data;
				if (TypeDict.ContainsKey(data.type))
                {
                    TypeDict[data.type].Add(data.id);
                }
                else
                {
                    List<int> ids  = new List<int>();
                    ids.Add(data.id);
                    TypeDict[data.type] = ids;
                }
			
			}
		}
        public AchieveTplData this[int id]
        {
            get 
            {
                AchieveTplData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}