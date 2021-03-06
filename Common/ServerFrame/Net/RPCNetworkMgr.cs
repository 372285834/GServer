﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace RPC
{

    public class RPCForwardInfo
    {
        public RPCForwardInfo()
        {
            Handle = System.UInt16.MaxValue;
            PlayerIndexInMap = System.UInt16.MaxValue;
            MapIndexInServer = System.Byte.MaxValue;
            ReturnSerialId = 0;
        }
        //客户端的权限
        public System.UInt16 LimitLevel;
        //客户端在接入服务器的句柄
        public System.UInt16 Handle;
        //客户端返回值序列化变化，不用Read和Write来处理
        public System.UInt16 ReturnSerialId;
        //玩家在地图管理器内的索引
        public System.UInt16 PlayerIndexInMap;
        //所在地图在位面的索引
        public System.UInt16 MapIndexInServer;
        //客户端角色Id
        public ulong RoleId;


        //在GateServer上用的，连接到客户端，连接到这个客户端所在的位面服务器
        public Iocp.NetConnection Gate2ClientConnect;
        public Iocp.NetConnection Gate2PlanesConnect;

        //在位面服务器上用的，连接到这个客户端所经历的GateServer
        public Iocp.NetConnection Planes2GateConnect;

        public static int GetBlockSize()
        {

            return sizeof(System.UInt16) + sizeof(System.UInt16) + sizeof(System.UInt16) + sizeof(System.UInt16) + sizeof(ulong);
        }

        public void Write(PackageWriter pkg)
        {
            //int begin = pkg->GetPosition();
            pkg.Write(LimitLevel);
            pkg.Write(Handle);
            pkg.Write(PlayerIndexInMap);
            pkg.Write(MapIndexInServer);
            pkg.Write(RoleId);
            //System::Diagnostics::Debug::Assert(pkg->GetPosition()-begin==RPCForwardInfo::GetBlockSize());
        }
        public void Read(PackageProxy pkg)
        {
            pkg.Read(out LimitLevel);
            pkg.Read(out Handle);
            pkg.Read(out PlayerIndexInMap);
            pkg.Read(out MapIndexInServer);
            pkg.Read(out RoleId);
        }
    }

    public class RPCGateServerForward
    {
        public virtual RPCForwardInfo GetForwardInfo(Iocp.NetConnection sender)
        {
            return null;
        }
        public virtual Iocp.NetConnection GetConnectByHandle(System.UInt16 handle)
        {
            return null;
        }
    }

    public class RPCPlanesServerSpecialRoot
    {
        public virtual void Push2Processor(RPCSpecialHolder holder, int ptype)
        {
            return;
        }
        public virtual System.String GetPlayerInfoString(RPCForwardInfo fwd)
        {
            return "";
        }
    }

    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
	public class RPCSpecialHolderProcessor
	{
		Queue<RPCSpecialHolder>	mPackages = new Queue<RPCSpecialHolder>();
	
		public int PackageNumber
		{
			get
            {
                return mPackages.Count;
            }
        }
        public void Tick()
        {
            if (mPackages.Count == 0)
                return;
            RPCSpecialHolder pkg = null;
            lock (this)
            {
                pkg = mPackages.Dequeue();
            }
            while (pkg != null)
            {
                Process(pkg);

                lock (this)
                {
                    if (mPackages.Count == 0)
                        break;
                    pkg = mPackages.Dequeue();
                }
            }
        }
        public void PushHolder(RPCSpecialHolder pkg)
        {
            lock (this)
            {
                mPackages.Enqueue(pkg);
            }
        }

        public static void Process(RPCSpecialHolder pkg)
        {
            var type = (PackageType)pkg.PkgType;
            switch (type)
            {
                case PackageType.PKGT_C2P_Player_SendAndWait:
                    {
                        System.Object retObject = RPCEntrance.Execute(pkg.mForward.LimitLevel, pkg.mForward.Planes2GateConnect, pkg.mRoot, pkg, pkg.mForward);
                        if (retObject == null || retObject.GetType() == typeof(void))
                        {
                            break;
                        }
                        var retPkg = new PackageWriter();
                        retPkg.SerialId = pkg.SerialId;
                        retPkg.WritePODObject(retObject);
                        retPkg.Write(pkg.mForward.Handle);
                        retPkg.DoReturnCommand(pkg.mForward.Planes2GateConnect, CommandTargetType.Planes);
                    }
                    break;
                case PackageType.PKGT_C2P_Player_Send:
                    {
                        System.Object retObject = RPCEntrance.Execute(pkg.mForward.LimitLevel, pkg.mForward.Planes2GateConnect, pkg.mRoot, pkg, pkg.mForward);
                    }
                    break;
            }
        }
    }

    public class RPCNetworkMgr
    {
        public const float Sync2ClientRange = 25;
        public const float Sync2ClientRangeSq = 25 * 25;
        public const UInt32 Sync2ClientPlayersCountWhenEnterMap = 50;
        public const UInt32 Sync2ClientNPCsCountWhenEnterMap = 60;

        public const float Sync2ClientMaxPlayerCount = 50;  //最大广播人数


        static RPCNetworkMgr smInstance = new RPCNetworkMgr();
        public static RPCNetworkMgr Instance
        {
            get { return smInstance; }
        }
        ~RPCNetworkMgr()
        {
            ClearWaitHandles();
        }
        System.UInt16 CurrentId;
        Dictionary<System.UInt16, RPCWaitHandle> mWaitHandles = new Dictionary<System.UInt16, RPCWaitHandle>();
        public Dictionary<System.UInt16, RPCWaitHandle> WaitHandles
        {
            get { return mWaitHandles; }
        }

        static Dictionary<System.UInt32, System.Type> mExecuterTypes = new Dictionary<System.UInt32, System.Type>();
        static Dictionary<System.UInt32, System.Byte> mExecuterIndices = new Dictionary<System.UInt32, System.Byte>();
        static Dictionary<System.UInt32, System.Type> mIndexerTypes = new Dictionary<System.UInt32, System.Type>();

        public RPCObject mRootObject;
        public RPCGateServerForward mGateServerForward;
        public RPCPlanesServerSpecialRoot mPlanesServerSpecialRoot;


//         public void PrintAllCallCounter()
//         {
//             FileStream fs = new FileStream(@"d:\test.xml", FileMode.OpenOrCreate,
//                                                                     FileAccess.ReadWrite, FileShare.Read);
//             var reader = XmlReader.Create(fs);
// 
//             // Pass the validating reader to the XML document.
//             // Validation fails due to an undefined attribute, but the 
//             // data is still loaded into the document.
//             XmlDocument doc = new XmlDocument();
//             //doc.Load(reader);
//             var root = doc.CreateNode(XmlNodeType.Element, "CallCounter", null); //CSUtility.Support.IXmlHolder.NewXMLHolder("CallCounter", "");
//             foreach (var i in RPC.RPCEntrance.RpcAllCaller)
//             {
//                 var node =  doc.CreateElement(i.Value.Name);
//                 node.SetAttribute("CallCount", i.Value.CallCounter.ToString());
//                 node.SetAttribute("WriteSize", i.Value.WriteSize.ToString());
//                 root.AppendChild(node);
//             }
// 
//             doc.Save(fs);
//             fs.Close();
//         }

//         public CSUtility.Support.IXmlHolder PrintAllRPCCounter()
//         {
//             var xml = CSUtility.Support.IXmlHolder.NewXMLHolder("RPCCounter", "");
//             foreach (var i in RPC.RPCEntrance.RpcAllClassInfo)
//             {
//                 var node = xml.RootNode.AddNode(i.ObjType.FullName, "", xml);
//                 PrintRPCCounter(i, node, xml);
//             }
//             return xml;
//         }
// 
//         public void PrintRPCCounter(RPC.RPCClassInfo cinfo, CSUtility.Support.IXmlNode node, CSUtility.Support.IXmlHolder xml)
//         {
//             var methods = node.AddNode("Methods", "", xml);
//             foreach (var i in cinfo.mMethods)
//             {
//                 if (i == null)
//                     continue;
//                 var cn = methods.AddNode(i.mMethodFullName, "", xml);
//                 cn.AddAttrib("CallCount", i.TotalCallCount.ToString());
//                 cn.AddAttrib("ReadSize", i.TotalReadSize.ToString());
//             }
//         }

		public RPCWaitHandle NewWaitHandle(System.Diagnostics.StackTrace st)
        {
            lock (this)
            {
                RPCWaitHandle result = new RPCWaitHandle();
                result.CallID = ++CurrentId;
                try
                {
                    mWaitHandles.Add(result.CallID, result);
                }
                catch (System.Exception)
                {
                    ClearTimeoutHandles();
                    mWaitHandles.Add(result.CallID, result);
                    Log.Log.Common.Print("NewWaitHandle Add Failed,ClearTimeoutHandles");
                }

                if (st != null)
                {
                    result.SrcFile = st.GetFrame(0).GetMethod().Name;
                }
                return result;
            }
        }
        public RPCWaitHandle NewWaitHandleTimeOut(System.Diagnostics.StackTrace st, float timeOut)
        {
            lock (this)
            {
                RPCWaitHandle result = new RPCWaitHandle();
                result.CallID = ++CurrentId;
                result.mTimeOut = timeOut;
                try
                {
                    mWaitHandles.Add(result.CallID, result);
                }
                catch (System.Exception)
                {
                    ClearTimeoutHandles();
                    mWaitHandles.Add(result.CallID, result);
                    Log.Log.Common.Print("NewWaitHandle Add Failed,ClearTimeoutHandles");
                }
                if (st != null)
                {
                    result.SrcFile = st.GetFrame(0).GetMethod().Name;
                }

                return result;
            }
        }
        public RPCWaitHandle GetWaitHandle(System.UInt16 id)
        {
            RPCWaitHandle result;
            if (mWaitHandles.TryGetValue(id, out result))
                return result;
            return null;
        }

        public static void AddExecuterType(System.UInt32 hashCode, System.Type type)
        {
            System.Type result;
            if (mExecuterTypes.TryGetValue(hashCode, out result))
            {
                System.Diagnostics.Debug.Assert(result == type);
                return;
            }
            mExecuterTypes.Add(hashCode, type);
        }
        public static void AddExecuterIndxer(System.UInt32 hashCode, System.Byte index)
        {
            mExecuterIndices.Add(hashCode, index);
        }

        public static System.Type FindExecuterType(System.UInt32 hashCode)
        {
            System.Type result;
            if (mExecuterTypes.TryGetValue(hashCode, out result))
            {
                return result;
            }
            return null;
        }
        public static System.Byte FindExecuterIndexer(System.UInt32 hashCode)
        {
            System.Byte result;
            if (mExecuterIndices.TryGetValue(hashCode, out result))
            {
                return result;
            }
            return 0;
        }
        public static void AddIndexExecuterType(System.UInt32 hashCode, System.Type type)
        {
            System.Type result;
            if (mIndexerTypes.TryGetValue(hashCode, out result))
            {
                System.Diagnostics.Debug.Assert(result == type);
                return;
            }
            mIndexerTypes.Add(hashCode, type);
        }
        public static System.Type FindIndexExecuterType(System.UInt32 hashCode)
        {
            System.Type result;
            if (mIndexerTypes.TryGetValue(hashCode, out result))
            {
                return result;
            }
            return null;
        }

        public void OnFarCallFinished(System.UInt16 id, PackageProxy ret, bool bTimeOut)
        {
            RPCWaitHandle result = null;

            lock (this)
            {
                if (mWaitHandles.TryGetValue(id, out result))
                {
                    mWaitHandles.Remove(id);
                }
            }

            if (result != null)
            {
                try
                {
                    result.Raise_OnFarCallFinished(ret, bTimeOut);
                }
                catch (System.Exception ex)
                {
                    Log.Log.Common.Print(ex.ToString());
                    Log.Log.Common.Print(ex.StackTrace.ToString());
                }
            }
        }

        public void ClearWaitHandles()
        {
            lock (this)
            {
                foreach (var kv in mWaitHandles)
                {
                    try
                    {
                        kv.Value.Raise_OnFarCallFinished(null, true);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Log.Common.Print(ex.ToString());
                        Log.Log.Common.Print(ex.StackTrace.ToString());
                    }
                }
                mWaitHandles.Clear();
            }
        }

        public void ClearTimeoutHandles()
        {
            lock (this)
            {
                List<ushort> removeList = new List<ushort>();
                foreach (var kv in mWaitHandles)
                {
                    if (kv.Value.mTimeOut < 0)
                    {
                        continue;
                    }
                    try
                    {
                        kv.Value.Raise_OnFarCallFinished(null, true);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Log.Common.Print(ex.ToString());
                        Log.Log.Common.Print(ex.StackTrace.ToString());
                    }
                    removeList.Add(kv.Key);
                }

                foreach (var i in removeList)
                {
                    mWaitHandles.Remove(i);
                }
            }
        }

        public void Tick(Int64 elapseMillisecond)
        {
            //这里淘汰掉那些超时没有用的WaitHandle
            float elapseTime = (float)elapseMillisecond / 1000;
            lock (this)
            {
                List<ushort> removeList = new List<ushort>();
                foreach (var kv in mWaitHandles)
                {
                    if (kv.Value.mTimeOut < 0)
                    {
                        continue;
                    }
                    else
                    {
                        if (elapseTime >= kv.Value.mTimeOut)
                        {
                            try
                            {
                                kv.Value.Raise_OnFarCallFinished(null, true);
                            }
                            catch (System.Exception ex)
                            {
                                Log.Log.Common.Print(ex.ToString());
                                Log.Log.Common.Print(ex.StackTrace.ToString());
                            }
                            removeList.Add(kv.Key);
                        }
                        else
                        {
                            kv.Value.mTimeOut -= elapseTime;
                        }
                    }
                }

                foreach (var i in removeList)
                {
                    mWaitHandles.Remove(i);
                }
            }
        }

        public void ClientReceiveData(Iocp.TcpClient pClient, byte[] pData, int nLength)
        {
            ReceiveData(pClient, pData, nLength);
        }
        public void ServerReceiveData(Iocp.TcpConnect pConnect, Iocp.TcpServer pServer, byte[] pData, int nLength)
        {
            ReceiveData(pConnect, pData, nLength);
        }

        public delegate void FOnP2CCall_WhenClientDisConnect(UInt16 handle, Iocp.TcpConnect conn);
        public FOnP2CCall_WhenClientDisConnect OnP2CCall_WhenClientDisConnect;

        protected void ReceiveData(Iocp.NetConnection sender, byte[] pData, int nLength)
        {
            var pkg = new PackageProxy(pData, nLength);
            System.UInt16 serialId = pkg.SerialId;

            RPCForwardInfo fwd = null;
            if (mGateServerForward != null)
            {
                fwd = mGateServerForward.GetForwardInfo(sender);
            }

            Iocp.TcpConnect tcpConnect = sender as Iocp.TcpConnect;

            int LimitLevel = (int)RPCExecuteLimitLevel.Developer;
            if (tcpConnect != null)
                LimitLevel = tcpConnect.mLimitLevel;

            //只有对外的GateServer才需要判断权限，更新最高权限
            if (fwd != null)
                LimitLevel = fwd.LimitLevel;

            if (fwd != null && (PackageType)pkg.PkgType >= PackageType.PKGT_C2P_Send)
            {
                switch ((PackageType)pkg.PkgType)
                {
                    case PackageType.PKGT_C2P_Send:
                    case PackageType.PKGT_C2P_Player_Send:
				    {
					    Iocp.NetConnection conn = fwd.Gate2PlanesConnect;
					    if(conn!=null)
					    {
						    var fwdPkg = new PackageWriter(pData,nLength);
						    fwd.Write(fwdPkg);
						    fwdPkg.SendBuffer(conn);
					    }
				    }
				    return;
                    case PackageType.PKGT_C2P_SendAndWait:
                    case PackageType.PKGT_C2P_Player_SendAndWait:
                        {
                            Iocp.NetConnection conn = fwd.Gate2PlanesConnect;
                            if (conn != null)
                            {
                                var fwdPkg = new PackageWriter(pData, nLength);
                                fwd.Write(fwdPkg);
                                fwdPkg.SendBuffer(conn);
                            }
                        }
                        return;
                    case PackageType.PKGT_C2P_Return:
                        {
                            if (mGateServerForward == null)
                            {
                                Log.Log.Common.Print("肯定有人改分包了，不是Gate但是收到PKGT_C2P_Return");
                            }
                            System.UInt16 handle;
                            //pkg.SeekTo(nLength-sizeof(RPCHeader)-sizeof(System.UInt16));
                            pkg.SeekTo(nLength - PackageProxy.HeaderSize - sizeof(System.UInt16));
                            pkg.Read(out handle);
                            pkg.SeekTo(0);
                            Iocp.NetConnection conn = mGateServerForward.GetConnectByHandle(handle);
                            if (conn != null)
                            {
                                var fwdPkg = new PackageWriter(pData, nLength - sizeof(System.UInt16));
                                fwdPkg.PkgType = PackageType.PKGT_Return;
                                fwdPkg.SendBuffer(conn);
                            }
                        }
                        return;
                }
            }

            if (fwd == null)
            {
                fwd = new RPCForwardInfo();
                fwd.ReturnSerialId = serialId;
                fwd.Gate2ClientConnect = sender;
                fwd.Planes2GateConnect = sender;
            }
            else
            {
                fwd.ReturnSerialId = serialId;
            }

            switch ((PackageType)pkg.PkgType)
            {
                case PackageType.PKGT_Send:
                    {//远端直接执行

                        RPCEntrance.Execute(LimitLevel, sender, mRootObject, pkg, fwd);
                    }
                    break;
                case PackageType.PKGT_SendAndWait:
                    {//远端执行后，返回执行结束
                        System.Object retObject = RPCEntrance.Execute(LimitLevel, sender, mRootObject, pkg, fwd);
                        if (retObject == null || retObject.GetType() == typeof(void))
                        {
                            break;
                        }
                        var retPkg = new PackageWriter();
                        retPkg.SerialId = serialId;
                        retPkg.WritePODObject(retObject);
                        retPkg.DoReturnCommand(sender, CommandTargetType.DefaultType);
                    }
                    break;
                case PackageType.PKGT_C2P_Send:
                    {
                        //pkg.SeekTo(nLength-sizeof(RPCHeader)-RPCForwardInfo.GetBlockSize());
                        pkg.SeekTo(nLength - PackageProxy.HeaderSize - RPCForwardInfo.GetBlockSize());
                        fwd.Read(pkg);
                        pkg.SeekTo(0);
                        System.Object retObject = RPCEntrance.Execute(fwd.LimitLevel, fwd.Planes2GateConnect, mRootObject, pkg, fwd);
                    }
                    break;
                case PackageType.PKGT_C2P_Player_Send:
                    {
                        if (mPlanesServerSpecialRoot == null)
                        {
                            Log.Log.Common.Print("有人改分包了,不是Planes但是收到PKGT_C2P_Player_Send消息");
                            break;
                        }

                        RPCSpecialHolder holder = new RPCSpecialHolder(pData, nLength - RPCForwardInfo.GetBlockSize());
                        holder.mForward = new RPCForwardInfo();

                        //pkg.SeekTo(nLength-sizeof(RPCHeader)-RPCForwardInfo.GetBlockSize());
                        pkg.SeekTo(nLength - PackageProxy.HeaderSize - RPCForwardInfo.GetBlockSize());
                        holder.mForward.Read(pkg);
                        pkg.SeekTo(0);
                        holder.mForward.ReturnSerialId = serialId;
                        holder.mForward.Planes2GateConnect = sender;

                        mPlanesServerSpecialRoot.Push2Processor(holder, (int)PackageType.PKGT_C2P_Player_Send);
                    }
                    break;
                case PackageType.PKGT_C2P_SendAndWait:
                    {
                        //pkg.SeekTo(nLength-sizeof(RPCHeader)-RPCForwardInfo.GetBlockSize());
                        pkg.SeekTo(nLength - PackageProxy.HeaderSize - RPCForwardInfo.GetBlockSize());
                        fwd.Read(pkg);
                        pkg.SeekTo(0);
                        System.Object retObject = RPCEntrance.Execute(fwd.LimitLevel, fwd.Planes2GateConnect, mRootObject, pkg, fwd);
                        if (retObject == null || retObject.GetType() == typeof(void))
                        {
                            //Log.Log.Common.Print("有人改分包了,不是Planes但是收到PKGT_C2P_SendAndWait消息");
                            break;
                        }
                        var retPkg = new PackageWriter();
                        retPkg.SerialId = serialId;
                        retPkg.WritePODObject(retObject);
                        retPkg.Write(fwd.Handle);
                        retPkg.DoReturnCommand(sender, CommandTargetType.Planes);
                    }
                    break;
                case PackageType.PKGT_C2P_Player_SendAndWait:
                    {
                        if (mPlanesServerSpecialRoot == null)
                        {
                            Log.Log.Common.Print("有人改分包了,不是Planes但是收到PKGT_C2P_Player_SendAndWait消息");
                            break;
                        }
                        var holder = new RPCSpecialHolder(pData, nLength - RPCForwardInfo.GetBlockSize());
                        holder.mForward = new RPCForwardInfo();
                        holder.mForward.ReturnSerialId = serialId;
                        //pkg.SeekTo(nLength-sizeof(RPCHeader)-RPCForwardInfo.GetBlockSize());				
                        pkg.SeekTo(nLength - PackageProxy.HeaderSize - RPCForwardInfo.GetBlockSize());
                        holder.mForward.Read(pkg);
                        pkg.SeekTo(0);
                        holder.mForward.Planes2GateConnect = sender;

                        mPlanesServerSpecialRoot.Push2Processor(holder, (int)PackageType.PKGT_C2P_Player_SendAndWait);
                    }
                    break;
                case PackageType.PKGT_Return:
                    {
                        OnFarCallFinished(serialId, pkg, false);
                    }
                    break;
                case PackageType.PKGT_C2P_Return:
                    {
                        if (mGateServerForward == null)
                        {
                            Log.Log.Common.Print("有人改分包了,不是Gate但是收到PKGT_C2P_Return消息");
                            return;
                        }
                        System.UInt16 handle;
                        //pkg.SeekTo(nLength-sizeof(RPCHeader)-sizeof(System.UInt16));
                        pkg.SeekTo(nLength - PackageProxy.HeaderSize - sizeof(System.UInt16));
                        pkg.Read(out handle);
                        pkg.SeekTo(0);
                        Iocp.NetConnection conn = mGateServerForward.GetConnectByHandle(handle);
                        if (conn != null)
                        {
                            var fwdPkg = new PackageWriter(pData, nLength - sizeof(System.UInt16));
                            fwdPkg.PkgType = PackageType.PKGT_Return;
                            fwdPkg.SendBuffer(conn);
                        }
                    }
                    break;
                case PackageType.PKGT_P2C_Send:
                    {
                        if (mGateServerForward == null)
                        {
                            Log.Log.Common.Print("有人改分包了,不是Gate但是收到PKGT_C2P_Return消息");
                            return;
                        }
                        System.UInt16 handle;
                        //pkg.SeekTo(nLength-sizeof(RPCHeader)-sizeof(System.UInt16));
                        pkg.SeekTo(nLength - PackageProxy.HeaderSize - sizeof(System.UInt16));
                        pkg.Read(out handle);
                        pkg.SeekTo(0);
                        Iocp.TcpConnect conn = mGateServerForward.GetConnectByHandle(handle) as Iocp.TcpConnect;
                        if (conn != null)
                        {
                            if (conn.State != Iocp.NetState.Connect)
                            {
                                if (OnP2CCall_WhenClientDisConnect != null)
                                    OnP2CCall_WhenClientDisConnect(handle, conn);
                            }
                            else
                            {
                                pkg.PkgType = PackageType.PKGT_Send;
                                //pkg.DangrouseSetPkgLength(nLength - sizeof(System.UInt16));

                                conn.SendBuffer(pkg.Ptr, 0, (int)(nLength - sizeof(System.UInt16)));
                            }
                        }
                    }
                    return;
            }
        }
    }
}
