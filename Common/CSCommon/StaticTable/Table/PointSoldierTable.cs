/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/PointSoldier.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class PointSoldierTable : IDataModel	
	{
		public Dictionary<int, PointSoldierData> Dict = new Dictionary<int, PointSoldierData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				PointSoldierData data = new PointSoldierData(array[i]);
				
				Dict[data.id] = data;
			}
		}
        public PointSoldierData this[int id]
        {
            get 
            {
                PointSoldierData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}