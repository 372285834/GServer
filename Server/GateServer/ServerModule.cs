using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateServer
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

        ServerCommon.IGateServer mServer = new ServerCommon.IGateServer();
        public ServerCommon.IGateServer Server
        {
            get { return mServer; }
        }

        ServerCommon.RPCRoot mRoot;
        public void Start()
        {
            Log.FileLog.Instance.Deleget_WriteLog = GateServerFrm.Inst.PushLog;

//             if (!System.IO.Directory.Exists(ServerCommon.IServer.Instance.ExePath + "srvcfg"))
//                 System.IO.Directory.CreateDirectory(ServerCommon.IServer.Instance.ExePath + "srvcfg");
// 
//             ServerCommon.IGateServerParameter parameter = new ServerCommon.IGateServerParameter();
//             string fullPathname = "Bin/srvcfg/GateServer.gatesrv";
//             if (false == ServerFrame.Config.IConfigurator.FillProperty(parameter, fullPathname))
//             {
//                 //System.Windows.Forms.MessageBox.Show("请检查服务器配置文件");
//                 ServerFrame.Config.IConfigurator.SaveProperty(parameter, "GateServer", fullPathname);
//             }
            ServerCommon.IGateServerParameter parameter = ServerCommon.ServerConfig.Instance.GateParam;

            mServer.Start(parameter);
            mRoot = new ServerCommon.RPCRoot();
            mRoot.mGateServer = mServer;
            RPC.RPCNetworkMgr.Instance.mRootObject = mRoot;
        }

        public void Stop()
        {
            mServer.Stop();
        }

        public void Tick()
        {
            try
            {
                mServer.Tick();
            }
            catch (System.Exception ex)
            {
                Log.Log.Common.Print(ex.ToString());
                Log.Log.Common.Print(ex.StackTrace.ToString());
                Log.FileLog.Instance.Flush();
            }            
        }
    }
}
