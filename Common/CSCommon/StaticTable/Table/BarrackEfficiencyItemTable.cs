/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/BarrackEfficiencyItem.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class BarrackEfficiencyItemTable : IDataModel	
	{
		public Dictionary<int, BarrackEfficiencyItemData> Dict = new Dictionary<int, BarrackEfficiencyItemData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				BarrackEfficiencyItemData data = new BarrackEfficiencyItemData(array[i]);
				
				Dict[data.id] = data;
			}
		}
        public BarrackEfficiencyItemData this[int id]
        {
            get 
            {
                BarrackEfficiencyItemData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}