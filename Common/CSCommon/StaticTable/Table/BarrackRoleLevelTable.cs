/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/BarrackRoleLevel.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class BarrackRoleLevelTable : IDataModel	
	{
		public Dictionary<int, BarrackRoleLevelData> Dict = new Dictionary<int, BarrackRoleLevelData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				BarrackRoleLevelData data = new BarrackRoleLevelData(array[i]);
				
				Dict[data.id] = data;
			}
		}
        public BarrackRoleLevelData this[int id]
        {
            get 
            {
                BarrackRoleLevelData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}