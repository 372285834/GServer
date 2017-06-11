using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerFrame.DB;

namespace Tool
{
    class ToolThread
    {
        static ToolThread smInstance = new ToolThread();
        public static ToolThread Instance
        {
            get { return smInstance; }
        }
        DBConnect mDBConnect = new DBConnect();
        public DBConnect DBConnect
        {
            get { return mDBConnect; }
        }

        ServerCommon.IComServer mServer = new ServerCommon.IComServer();
        public ServerCommon.IComServer Server
        {
            get { return mServer; }
        }

        public Dictionary<string, CSTable.ItemTplData> Dict = new Dictionary<string, CSTable.ItemTplData>();
        public List<string> names = new List<string>();
        public void InitNames()
        {
            foreach(var i in  CSTable.StaticDataManager.ItemTpl.Dict)
            {
                Dict[i.Value.ItemName] = i.Value;
                names.Add(i.Value.ItemName);
            }
        }

        public void Start()
        {
            ServerCommon.IComServer.Instance = mServer;
            mDBConnect.OpenConnect("192.168.1.251");
            ServerCommon.TemplateTableLoader.LoadTable(ServerCommon.ServerConfig.Instance.TablePath);
            InitNames();
        }

    }
}
