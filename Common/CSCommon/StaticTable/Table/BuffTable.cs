/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/Buff.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class BuffTable : IDataModel	
	{
		public Dictionary<int, BuffData> Dict = new Dictionary<int, BuffData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				BuffData data = new BuffData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public BuffData this[int id]
        {
            get 
            {
                BuffData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}