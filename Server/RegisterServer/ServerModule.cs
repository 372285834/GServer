using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RegisterServer
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

        ServerCommon.IRegisterServer mServer = new ServerCommon.IRegisterServer();
        public ServerCommon.IRegisterServer Server
        {
            get { return mServer; }
        }

        ServerCommon.RPCRoot mRoot;
        public void Start()
        {
            Log.FileLog.Instance.Deleget_WriteLog = RegisterServerFrm.Inst.PushLog;

//             if (!System.IO.Directory.Exists(ServerCommon.IServer.Instance.ExePath + "srvcfg"))
//                 System.IO.Directory.CreateDirectory(ServerCommon.IServer.Instance.ExePath + "srvcfg");
//            ServerCommon.IRegisterServerParameter parameter = new ServerCommon.IRegisterServerParameter();
//             string fullPathname = "Bin/srvcfg/RegServer.regsrv";
//             if (false == ServerFrame.Config.IConfigurator.FillProperty(parameter, fullPathname))
//             {
//                 //System.Windows.Forms.MessageBox.Show("请检查服务器配置文件");
//                 ServerFrame.Config.IConfigurator.SaveProperty(parameter, "RegServer", fullPathname);
//             }
            ServerCommon.IRegisterServerParameter parameter = ServerCommon.ServerConfig.Instance.RegParam;
            mServer.Start(parameter);
            mRoot = new ServerCommon.RPCRoot();
            mRoot.mRegServer = mServer;
            RPC.RPCNetworkMgr.Instance.mRootObject = mRoot;

            SocketPolicyServer server = new SocketPolicyServer(SocketPolicyServer.AllPolicy);
            int result = server.Start();
            if (result != 0)
            {
                Log.Log.Server.Info("unity plicy Server Start failed!");
            }
         
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
