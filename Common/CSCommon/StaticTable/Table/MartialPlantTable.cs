/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/MartialPlant.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class MartialPlantTable : IDataModel	
	{
		public Dictionary<int, MartialPlantData> Dict = new Dictionary<int, MartialPlantData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				MartialPlantData data = new MartialPlantData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public MartialPlantData this[int id]
        {
            get 
            {
                MartialPlantData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}