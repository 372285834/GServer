/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/Barrack.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class BarrackTable : IDataModel	
	{
		public Dictionary<int, BarrackData> Dict = new Dictionary<int, BarrackData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				BarrackData data = new BarrackData(array[i]);
				
				Dict[data.id] = data;
			}
		}
        public BarrackData this[int id]
        {
            get 
            {
                BarrackData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}