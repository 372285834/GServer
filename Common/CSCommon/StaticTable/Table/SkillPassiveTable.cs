/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/SkillPassive.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class SkillPassiveTable : IDataModel	
	{
		public Dictionary<int, SkillPassiveData> Dict = new Dictionary<int, SkillPassiveData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				SkillPassiveData data = new SkillPassiveData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public SkillPassiveData this[int id]
        {
            get 
            {
                SkillPassiveData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}