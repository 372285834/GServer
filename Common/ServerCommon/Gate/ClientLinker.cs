using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon.Gate
{
    public enum LandStep
    {
        TryLogin,
        SelectRole,
        EnterGame,
    }

    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class ClientLinker
    {
        public ClientLinker()
        {
            LinkerSerialId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Server);
        }
        public ulong LinkerSerialId;

        public uint ConnectedTime = IServer.timeGetTime();

        public bool OffLineKeepLinker = false;
        public System.DateTime OffLineKeepTime;


        CSCommon.Data.AccountInfo mAccountInfo = new CSCommon.Data.AccountInfo();
        public CSCommon.Data.AccountInfo AccountInfo
        {
            get { return mAccountInfo; }
        }

        public CSCommon.Data.PlayerDataEx mPlayerData;
        public CSCommon.Data.PlayerDataEx PlayerData
        {
            get { return mPlayerData; }
        }

        public LandStep mLandStep = LandStep.TryLogin;
        public LandStep LandStep
        {
            get { return mLandStep; }
        }
        
        public RPC.RPCForwardInfo mForwardInfo = new RPC.RPCForwardInfo();

        public void InvalidLinkerWhenRoleExit()
        {
            mPlayerData = null;
            mForwardInfo.RoleId = 0;
            mForwardInfo.MapIndexInServer = UInt16.MaxValue;
            mForwardInfo.PlayerIndexInMap = UInt16.MaxValue;
            mForwardInfo.Gate2PlanesConnect = null;
            mLandStep = Gate.LandStep.SelectRole;
        }
    }

    public class KeepLinkerManager
    {
        static KeepLinkerManager smInstance = new KeepLinkerManager();
        public static KeepLinkerManager Instance
        {
            get { return smInstance; }
        }
        Dictionary<ulong, ClientLinker> mLinkers = new Dictionary<ulong, ClientLinker>();
        public void KeepLinker(ClientLinker linker)
        {
            lock (this)
            {
                linker.OffLineKeepTime = System.DateTime.Now;
                linker.OffLineKeepLinker = true;
                mLinkers[linker.LinkerSerialId] = linker;
            }
        }
        public ClientLinker PeekLinker(ulong linkId)
        {
            lock (this)
            {
                ClientLinker linker;
                if (mLinkers.TryGetValue(linkId, out linker))
                {
                    linker.OffLineKeepLinker = false;
                    return linker;
                }
                return null;
            }
        }

        const int KeepSecond = int.MaxValue;//30;
        public void Tick(IGateServer gateSever)
        {
            //这里应该有一个倒计时，发现超过时间的就要让他断线啦
            lock (this)
            {
                var lst = new List<ulong>();
                System.DateTime nowTime = System.DateTime.Now;
                foreach (var i in mLinkers)
                {
                    if ((nowTime - i.Value.OffLineKeepTime).Seconds > KeepSecond)
                    {
                        TimeOutDisconnect(gateSever, i.Value);
                        lst.Add(i.Key);
                    }
                }

                foreach (var i in lst)
                {
                    mLinkers.Remove(i);
                }
            }
        }

        void TimeOutDisconnect(IGateServer gateSever, ClientLinker linker)
        {
            linker.OffLineKeepLinker = false;
            gateSever.NotifyOtherServers_ClientDisconnect(linker);
        }
    }
}
