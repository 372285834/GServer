/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/MartialSmelt.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class MartialSmeltTable : IDataModel	
	{
		public Dictionary<int, MartialSmeltData> Dict = new Dictionary<int, MartialSmeltData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				MartialSmeltData data = new MartialSmeltData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public MartialSmeltData this[int id]
        {
            get 
            {
                MartialSmeltData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}