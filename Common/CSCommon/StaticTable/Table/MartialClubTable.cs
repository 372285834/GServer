/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/MartialClub.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class MartialClubTable : IDataModel	
	{
		public Dictionary<int, MartialClubData> Dict = new Dictionary<int, MartialClubData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				MartialClubData data = new MartialClubData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public MartialClubData this[int id]
        {
            get 
            {
                MartialClubData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}