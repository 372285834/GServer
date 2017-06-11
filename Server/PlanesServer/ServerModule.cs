using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanesServer
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

        ServerCommon.IPlanesServer mServer = new ServerCommon.IPlanesServer();
        public ServerCommon.IPlanesServer Server
        {
            get { return mServer; }
        }

        ServerCommon.RPCRoot mRoot;
        public void Start()
        {
            Log.FileLog.Instance.Deleget_WriteLog = PlanesServerFrm.Inst.PushLog;

//             if (!System.IO.Directory.Exists(ServerCommon.IServer.Instance.ExePath + "srvcfg"))
//                 System.IO.Directory.CreateDirectory(ServerCommon.IServer.Instance.ExePath + "srvcfg");
// 
//             ServerCommon.IPlanesServerParameter parameter = new ServerCommon.IPlanesServerParameter();
//             string fullPathname = "Bin/srvcfg/PlanesServer.planesrv";
//             if (false == ServerFrame.Config.IConfigurator.FillProperty(parameter, fullPathname))
//             {
//                 //System.Windows.Forms.MessageBox.Show("请检查服务器配置文件");
//                 ServerFrame.Config.IConfigurator.SaveProperty(parameter, "PlanesServer", fullPathname);
//             }
            ServerCommon.IPlanesServerParameter parameter = ServerCommon.ServerConfig.Instance.PlaneParam;

            try
            {
                mServer.Start(parameter);
            }
            catch (System.Exception ex)
            {
                Log.Log.Common.Print(ex.ToString());
                Log.Log.Common.Print(ex.StackTrace.ToString());
                Log.FileLog.Instance.Flush();
            }

            mRoot = new ServerCommon.RPCRoot();
            mRoot.mPlanesServer = mServer;
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
