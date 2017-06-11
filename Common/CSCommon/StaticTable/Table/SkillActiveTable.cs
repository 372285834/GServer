/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/SkillActive.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class SkillActiveTable : IDataModel	
	{
		public Dictionary<int, SkillActiveData> Dict = new Dictionary<int, SkillActiveData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				SkillActiveData data = new SkillActiveData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public SkillActiveData this[int id]
        {
            get 
            {
                SkillActiveData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}