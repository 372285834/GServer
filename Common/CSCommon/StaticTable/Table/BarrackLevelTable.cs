/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/BarrackLevel.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class BarrackLevelTable : IDataModel	
	{
		public Dictionary<int, BarrackLevelData> Dict = new Dictionary<int, BarrackLevelData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				BarrackLevelData data = new BarrackLevelData(array[i]);
				
				Dict[data.id] = data;
			}
		}
        public BarrackLevelData this[int id]
        {
            get 
            {
                BarrackLevelData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}