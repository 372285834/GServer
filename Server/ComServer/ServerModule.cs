using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComServer
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

        ServerCommon.IComServer mServer = new ServerCommon.IComServer();
        public ServerCommon.IComServer Server
        {
            get { return mServer; }
        }

        ServerCommon.RPCRoot mRoot;
        public void Start()
        {
            Log.FileLog.Instance.Deleget_WriteLog = ComServerFrm.Inst.PushLog;
//             if (!System.IO.Directory.Exists(ServerCommon.IServer.Instance.ExePath + "srvcfg"))
//                 System.IO.Directory.CreateDirectory(ServerCommon.IServer.Instance.ExePath + "srvcfg");
// 
//             ServerCommon.IComServerParameter parameter = new ServerCommon.IComServerParameter();
//             string fullPathname = "Bin/srvcfg/ComServer.comsrv";
//             if (false == ServerFrame.Config.IConfigurator.FillProperty(parameter, fullPathname))
//             {
//                 ServerFrame.Config.IConfigurator.SaveProperty(parameter, "ComServer", fullPathname);
//             }
            ServerCommon.IComServerParameter parameter = ServerCommon.ServerConfig.Instance.ComParam;
            ServerFrame.DB.DBConnect.ConnectIP = parameter.DateBaseIP;

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
            mRoot.mComServer = mServer;
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
