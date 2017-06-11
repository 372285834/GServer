/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/PlayerTpl.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class PlayerTplTable : IDataModel	
	{
		public Dictionary<int, PlayerTplData> Dict = new Dictionary<int, PlayerTplData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				PlayerTplData data = new PlayerTplData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public PlayerTplData this[int id]
        {
            get 
            {
                PlayerTplData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }
        public List<PlayerTplData> GetDataByCareer(CSCommon.eProfession type)
        {
            List<PlayerTplData> list = new List<PlayerTplData>();
            foreach (var data in Dict.Values)
            {
                if (data.career == (int)type)
                {
                    list.Add(data);
                }
            }
            return list;
        }
	}
}