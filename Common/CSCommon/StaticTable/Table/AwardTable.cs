/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/Award.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class AwardTable : IDataModel	
	{
		public Dictionary<int, AwardData> Dict = new Dictionary<int, AwardData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				AwardData data = new AwardData(array[i]);
				
				Dict[data.ID] = data;
			}
		}
        public AwardData this[int id]
        {
            get 
            {
                AwardData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}