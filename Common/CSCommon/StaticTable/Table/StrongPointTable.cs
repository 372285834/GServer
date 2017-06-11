/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/StrongPoint.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class StrongPointTable : IDataModel	
	{
		public Dictionary<int, StrongPointData> Dict = new Dictionary<int, StrongPointData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				StrongPointData data = new StrongPointData(array[i]);
				
				Dict[data.id] = data;
			}
		}
        public StrongPointData this[int id]
        {
            get 
            {
                StrongPointData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}