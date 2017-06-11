/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/CopyTpl.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class CopyTplTable : IDataModel	
	{
		public Dictionary<int, CopyTplData> Dict = new Dictionary<int, CopyTplData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				CopyTplData data = new CopyTplData(array[i]);
				
				Dict[data.id] = data;
			}
		}
        public CopyTplData this[int id]
        {
            get 
            {
                CopyTplData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}