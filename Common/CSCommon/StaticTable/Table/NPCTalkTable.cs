/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/NPCTalk.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class NPCTalkTable : IDataModel	
	{
		public Dictionary<int, NPCTalkData> Dict = new Dictionary<int, NPCTalkData>();
		public List<NPCTalkData> DataList = new List<NPCTalkData>();
		public void initData(string content)		
		{
			Dict.Clear();
            string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				NPCTalkData data = new NPCTalkData(array[i]);
				
				Dict[data.id] = data;
                DataList.Add(data);
			}
		}
        public NPCTalkData this[int id]
        {
            get 
            {
                NPCTalkData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}