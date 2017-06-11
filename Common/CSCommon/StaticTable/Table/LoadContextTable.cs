/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/LoadContext.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class LoadContextTable : IDataModel	
	{
		public Dictionary<int, LoadContextData> Dict = new Dictionary<int, LoadContextData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				LoadContextData data = new LoadContextData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public LoadContextData this[int id]
        {
            get 
            {
                LoadContextData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}