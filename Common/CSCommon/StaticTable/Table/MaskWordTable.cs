/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/MaskWord.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class MaskWordTable : IDataModel	
	{
		public Dictionary<int, MaskWordData> Dict = new Dictionary<int, MaskWordData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				MaskWordData data = new MaskWordData(array[i]);
				
				Dict[data.id] = data;
			}
		}
        public MaskWordData this[int id]
        {
            get 
            {
                MaskWordData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}