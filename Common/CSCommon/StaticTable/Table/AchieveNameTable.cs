/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/AchieveName.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class AchieveNameTable : IDataModel	
	{
		public Dictionary<int, AchieveNameData> Dict = new Dictionary<int, AchieveNameData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				AchieveNameData data = new AchieveNameData(array[i]);
				
				Dict[data.id] = data;
			}
		}
        public AchieveNameData this[int id]
        {
            get 
            {
                AchieveNameData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}