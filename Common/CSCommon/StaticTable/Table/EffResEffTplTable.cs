/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/EffResEffTpl.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class EffResEffTplTable : IDataModel	
	{
		public Dictionary<int, EffResEffTplData> Dict = new Dictionary<int, EffResEffTplData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				EffResEffTplData data = new EffResEffTplData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public EffResEffTplData this[int id]
        {
            get 
            {
                EffResEffTplData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}