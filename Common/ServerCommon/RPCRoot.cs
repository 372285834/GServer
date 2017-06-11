using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon
{
    [RPC.RPCClassAttribute(typeof(RPCRoot))]
    public class RPCRoot : RPC.RPCObject
    {
        public static RPC.RPCClassInfo	smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
		{
			return smRpcClassInfo;
		}

#region RPC ChildObject
        public IRegisterServer mRegServer;
        [RPC.RPCChildObjectAttribute(0, (int)RPC.RPCExecuteLimitLevel.All, false)]
        public IRegisterServer RegServer
        {
            get { return mRegServer; }
        }
        public IGateServer mGateServer;
        [RPC.RPCChildObjectAttribute(1, (int)RPC.RPCExecuteLimitLevel.All, false)]
        public IGateServer GateServer
        {
            get { return mGateServer; }
        }
        public IDataServer mDataServer;
        [RPC.RPCChildObjectAttribute(2, (int)RPC.RPCExecuteLimitLevel.All, false)]
        public IDataServer DataServer
        {
            get { return mDataServer; }
        }
        public IPlanesServer mPlanesServer;
        [RPC.RPCChildObjectAttribute(3, (int)RPC.RPCExecuteLimitLevel.All, false)]
        public IPlanesServer PlanesServer
        {
            get { return mPlanesServer; }
        }
        public IComServer mComServer;
        [RPC.RPCChildObjectAttribute(4, (int)RPC.RPCExecuteLimitLevel.All, false)]
        public IComServer ComServer
        {
            get { return mComServer; }
        }
#endregion        

#region RPC_Method
        
#endregion        
    }
}

