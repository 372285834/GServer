/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/NPCTpl.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class NPCTplTable : IDataModel	
	{
		public Dictionary<int, NPCTplData> Dict = new Dictionary<int, NPCTplData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				NPCTplData data = new NPCTplData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public NPCTplData this[int id]
        {
            get 
            {
                NPCTplData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}