/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/MailInfo.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class MailInfoTable : IDataModel	
	{
		public Dictionary<int, MailInfoData> Dict = new Dictionary<int, MailInfoData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				MailInfoData data = new MailInfoData(array[i]);
				
				Dict[data.id] = data;
			
			}
		}
        public MailInfoData this[int id]
        {
            get 
            {
                MailInfoData data = null;
                Dict.TryGetValue(id, out data);
                return data;
            }
        }		
	}
}