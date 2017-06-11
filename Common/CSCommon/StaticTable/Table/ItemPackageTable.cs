/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemPackage.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class ItemPackageTable : IDataModel	
	{
		public Dictionary<int, ItemPackageData> Dict = new Dictionary<int, ItemPackageData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				ItemPackageData data = new ItemPackageData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public ItemPackageData this[int id]
        {
            get 
            {
                ItemPackageData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}