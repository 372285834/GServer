using ServerFrame.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Thread
{
    public class AccountLoginHolder
    {
        public UInt16 LinkHandle;
        public ulong LinkSerialId;
        public string Name;
        public string Password;
        public ushort PlanesId;
        public Iocp.NetConnection Connect;
        public UInt16 ReturnSerialId;
    }

    public class AccountLoginThread
    {
        static AccountLoginThread smInstance = new AccountLoginThread();
        public static AccountLoginThread Instance
        {
            get { return smInstance; }
        }

        public void PushLogin(AccountLoginHolder loginHolder)
        {
            int queueCount = 0;
            lock (this)
            {
                foreach (var i in mLoginQueue)
                {
                    if (i.Name == loginHolder.Name)
                        break;
                }
                mLoginQueue.Add(loginHolder);
                queueCount = mLoginQueue.Count;
                if (queueCount > 1)
                {
                    //RPC.PackageWriter pkg = new RPC.PackageWriter();
                    //H_RPCRoot.smInstance.HGet_GateServer(pkg).ClientTryLoginSuccessed(pkg, loginHolder.LinkHandle, loginHolder.Name, loginHolder.Id);
                    //这里要告诉客户端，还有多少在排队
                    //pkg.DoCommand(loginHolder.Connect, RPC.CommandTargetType.DefaultType);
                }
            }
        }
        public void CancelLogin(string name)
        {
            lock (this)
            {
                for (int i = 0; i < mLoginQueue.Count; i++)
                {
                    if (mLoginQueue[i].Name == name)
                    {
                        mLoginQueue.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        List<AccountLoginHolder> mLoginQueue = new List<AccountLoginHolder>();
        public int GetNumber()
        {
            return mLoginQueue.Count;
        }
        //Queue<AtomHolder> mEnterGameQueue;
        void Tick()
        {
            DBConnect.Tick();
            if (mLoginQueue.Count > 0)
            {
                AccountLoginHolder atom = null; 
                lock (this)
                {
                    atom = mLoginQueue[0];
                    mLoginQueue.RemoveAt(0);
                }
                try
                {
                    IDataServer.Instance.PlayerManager.Do_AccountLogin(atom);
                }
                catch (System.Exception ex)
                {
                    Log.Log.Common.Print(ex.ToString());
                    Log.Log.Common.Print(ex.StackTrace.ToString());
                }
            }
        }
        bool mRunning;
        private ServerFrame.DB.DBConnect mDBConnect;
        public ServerFrame.DB.DBConnect DBConnect
        {
            get { return mDBConnect; }
        }
        private System.Threading.Thread mThread;
        public void ThreadLoop()
        {
            while (mRunning)
            {
                Tick();
                System.Threading.Thread.Sleep(5);
            }
            mThread = null;
            Log.Log.Common.Print("PlayerLoginThread Thread exit!");
        }
        public void StartThread()
        {
            mDBConnect = new ServerFrame.DB.DBConnect();
            mDBConnect.OpenConnect();
            mRunning = true;
            mThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ThreadLoop));
            mThread.Start();
        }
        public void StopThread()
        {
            mRunning = false;
        }
    }

    public class RoleEnterHolder
    {
        public ulong linkSerialId;
        public UInt16 cltIndex;
        public ulong roleId;
        public ulong accountId;
        public Iocp.NetConnection connect;
        public UInt16 returnSerialId;

        public AsyncExecuter RoleCreator;
    }

    public class PlayerEnterThread
    {
        static PlayerEnterThread smInstance = new PlayerEnterThread();
        public static PlayerEnterThread Instance
        {
            get { return smInstance; }
        }

        public void PushRoleCreator(AsyncExecuter exec)
        {
            var sqlAtom = new RoleEnterHolder();
            sqlAtom.RoleCreator = exec;
            this.Push(sqlAtom,false);
        }

        public void Push(RoleEnterHolder holder,bool checkRoleId)
        {
            int queueCount = 0;
            lock (this)
            {
                if (checkRoleId)
                {
                    foreach (var i in mLoginQueue)
                    {
                        if (i.roleId == holder.roleId)
                            return;
                    }
                }
                mLoginQueue.Add(holder);
                queueCount = mLoginQueue.Count;
                if (queueCount > 1)
                {
                    //RPC.PackageWriter pkg = new RPC.PackageWriter();
                    //H_RPCRoot.smInstance.HGet_GateServer(pkg).ClientTryLoginSuccessed(pkg, loginHolder.LinkHandle, loginHolder.Name, loginHolder.Id);
                    //这里要告诉客户端，还有多少在排队
                    //pkg.DoCommand(loginHolder.Connect, RPC.CommandTargetType.DefaultType);
                }
            }
        }
        public void Remove(ulong roldId)
        {
            lock (this)
            {
                for (int i = 0; i < mLoginQueue.Count; i++)
                {
                    if (mLoginQueue[i].roleId == roldId)
                    {
                        Log.Log.Server.Info("Remove角色,id={0}", roldId);
                        mLoginQueue.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        List<RoleEnterHolder> mLoginQueue = new List<RoleEnterHolder>();
        public int GetNumber()
        {
            return mLoginQueue.Count;
        }
        void Tick(ServerFrame.DB.DBConnect dbConnect)
        {
            DBConnect.Tick();
            if (mLoginQueue.Count > 0)
            {
                RoleEnterHolder atom = null; 
                lock (this)
                {
                    atom = mLoginQueue[0];
                    mLoginQueue.RemoveAt(0);
                }
                try
                {
                    if (atom.RoleCreator != null)
                    {
                        Log.Log.Server.Info("atom.RoleCreator != null");
                        if (atom.RoleCreator.Exec != null)
                            atom.RoleCreator.Exec();
                    }
                    else
                    {
                        IDataServer.Instance.PlayerManager.Do_RoleLogin(atom);
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Log.Login.Info("登陆失败{0}", atom.roleId);
                    Log.Log.Common.Print(ex.ToString());
                    Log.Log.Common.Print(ex.StackTrace.ToString());
                }
            }
        }
        bool mRunning;
        private ServerFrame.DB.DBConnect mDBConnect;
        public ServerFrame.DB.DBConnect DBConnect
        {
            get { return mDBConnect; }
        }
        private System.Threading.Thread mThread;
        public void ThreadLoop()
        {
            while (mRunning)
            {
                Tick(mDBConnect);
                System.Threading.Thread.Sleep(5);
            }
            mThread = null;
            Log.Log.Common.Print("PlayerEnterThread Thread exit!");
        }
        public void StartThread()
        {
            mDBConnect = new ServerFrame.DB.DBConnect();
            mDBConnect.OpenConnect();
            mRunning = true;
            mThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ThreadLoop));
            mThread.Start();
        }
        public void StopThread()
        {
            mRunning = false;
        }
    }

   
}
