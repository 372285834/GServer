/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/Message.txt
/// 仅第一次生成，若已有该文件则不会覆盖
/// </summary>
using System.Collections;
using System.Collections.Generic;
namespace CSTable
{	
	public class MessageTable : IDataModel	
	{
        public Dictionary<string, MessageData> Dict = new Dictionary<string, MessageData>();
        public Dictionary<int, MessageData> IdDict = new Dictionary<int, MessageData>();
		public void initData(string content)		
		{
			Dict.Clear();
			string[] array = content.Split(new string[] { StaticDataConstant.WIN_ENTER }, System.StringSplitOptions.RemoveEmptyEntries);
			
			int length = array.Length;
			
			for (int i = 4; i < length; i++)
			
			{
				
				MessageData data = new MessageData(array[i]);

                Dict[data.eType + "." + data.eValue] = data;
                IdDict[data.id] = data;
			
			}
		}
        public MessageData this[string type, string value]
        {
            get
            {
                MessageData data = null;
                Dict.TryGetValue(type + "." + value, out data);
                return data;
            }
        }
        public MessageData this[int id]
        {
            get
            {
                MessageData data = null;
                IdDict.TryGetValue(id, out data);
                return data;
            }
        }	
	}
}