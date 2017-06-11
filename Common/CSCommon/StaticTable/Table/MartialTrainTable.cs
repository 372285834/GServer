/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/MartialTrain.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class MartialTrainTable : IDataModel	
	{
		public Dictionary<int, MartialTrainData> Dict = new Dictionary<int, MartialTrainData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				MartialTrainData data = new MartialTrainData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public MartialTrainData this[int id]
        {
            get 
            {
                MartialTrainData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}