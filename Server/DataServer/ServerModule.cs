using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataServer
{
    class ServerModule
    {
        static ServerModule smInstance = new ServerModule();
        public static ServerModule Instance
        {
            get { return smInstance; }
        }
        static ServerModule()
        {
            ServerCommon.IServer.Instance.LoadRPCModule("ServerCommon.dll");
        }

        ServerCommon.IDataServer mServer = new ServerCommon.IDataServer();
        public ServerCommon.IDataServer Server
        {
            get { return mServer; }
        }

        ServerCommon.RPCRoot mRoot;
        public void Start()
        {
            Log.FileLog.Instance.Deleget_WriteLog = DataServerFrm.Inst.PushLog;

//             if (!System.IO.Directory.Exists(ServerCommon.IServer.Instance.ExePath + "srvcfg"))
//                 System.IO.Directory.CreateDirectory(ServerCommon.IServer.Instance.ExePath + "srvcfg");
//             ServerCommon.IDataServerParameter parameter = new ServerCommon.IDataServerParameter();
//             string fullPathname = "Bin/srvcfg/DataServer.datasrv";
//             if (false == ServerFrame.Config.IConfigurator.FillProperty(parameter, fullPathname))
//             {
//                 //System.Windows.Forms.MessageBox.Show("请检查服务器配置文件");
//                 ServerFrame.Config.IConfigurator.SaveProperty(parameter, "DataServer", fullPathname);
//             }
           
            ServerCommon.IDataServerParameter parameter = ServerCommon.ServerConfig.Instance.DataParam;

            ServerCommon.IDataServer.Instance = mServer;

            ServerFrame.DB.DBConnect.ConnectIP = parameter.DateBaseIP;
            mServer.Start(parameter);
            
            mRoot = new ServerCommon.RPCRoot();
            mRoot.mDataServer = mServer;
            RPC.RPCNetworkMgr.Instance.mRootObject = mRoot;
        }

        public void Stop()
        {
            mServer.Stop();
        }

        public void Tick()
        {
            mServer.Tick();
        }
    }
}
