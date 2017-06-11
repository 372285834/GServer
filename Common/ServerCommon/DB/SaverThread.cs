using ServerFrame.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Thread
{
    public class PlayerSaver
    {
        public CSCommon.Data.PlayerDataEx mPlayerDataEx;
        public bool mWaitRemove = false;
        Queue<ServerFrame.DB.DBOperator> mQueues = new Queue<ServerFrame.DB.DBOperator>();
        public void Push(ServerFrame.DB.DBOperator saver)
        {
            if (saver == null)
            {
                if (mPlayerDataEx != null)
                {
                    Log.Log.Common.Print("不能压入null存盘" + mPlayerDataEx.RoleDetail.RoleName);
                }
            }
            lock (this)
            {
                mIsEmpty = false;
            }
            
            mQueues.Enqueue(saver);
        }
        public void Tick(ServerFrame.DB.DBConnect dbConnect)
        {
            try
            {//这里之所以要try住，是因为存盘的过程中，如果发生了Clear，丢弃之前的存储指令，有可能触发迭代过程异常，但是这个异常不会影响到什么东西
                if (mQueues.Count == 0)
                {
                    lock (this)
                    {
                        mIsEmpty = true;
                    }
                    return;
                }
                ServerFrame.DB.DBOperator saver = mQueues.Dequeue();
                if (saver == null)
                {
                    Log.Log.Common.Print("见鬼了，居然还有null存盘" + mPlayerDataEx.RoleDetail.RoleName);
                    return;
                }
                while (saver != null)
                {
                    try
                    {
                        if (saver.Executer != null)
                        {
                            if (saver.Executer.Exec != null)
                                saver.Executer.Exec();
                        }
                        else if (saver.ExeType == SqlExeType.Update)
                        {
                            dbConnect._ExecuteUpdate(saver);
                        }
                        else if (saver.ExeType == SqlExeType.Insert)
                        {
                            dbConnect._ExecuteInsert(saver);
                        }
                        else if (saver.ExeType == SqlExeType.Delete)
                        {
                            dbConnect._ExecuteDelete(saver);
                        }
                        else if (saver.ExeType == SqlExeType.Destroy)
                        {
                            dbConnect._ExecuteDestroy(saver);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Log.Log.Common.Print(ex.ToString());
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }

                    if (mQueues.Count == 0)
                    {
                        lock(this)
                        {
                            mIsEmpty = true;
                        }
                        
                        break;//减少异常的概率做的努力，虽然这个异常没啥，但是log压力也还是要考虑的
                    }
                    saver = mQueues.Dequeue();
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.Common.Print(ex.ToString());
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        bool mIsEmpty = false;
        public bool IsEmpty()
        {
            //int count = mQueues.Count;
            return mIsEmpty;
        }
        public int GetNumber()
        {
            return mQueues.Count;
        }
        public void Clear()
        {
            mQueues.Clear();
        }
    }

    class PlayerSaverThread
    {
        Dictionary<CSCommon.Data.PlayerDataEx, PlayerSaver> mPlayers = new Dictionary<CSCommon.Data.PlayerDataEx, PlayerSaver>();
        Dictionary<CSCommon.Data.PlayerDataEx, PlayerSaver> mPushPlayers = new Dictionary<CSCommon.Data.PlayerDataEx, PlayerSaver>();
        public int GetPlayerCount()
        {
            return mPlayers.Count + mPushPlayers.Count;
        }

        int mSaverNumber;
        public int GetSaverNumber()
        {
            return mSaverNumber;
        }
        public PlayerSaver AddPlayer(CSCommon.Data.PlayerDataEx player)
        {
            PlayerSaver saver;
            lock (this)
            {
                saver = FindPlayerSaver(player);
                if (saver != null)
                    return saver;
                saver = new PlayerSaver();
                saver.mPlayerDataEx = player;
                if (player == null || player.RoleDetail == null)
                {
                    Log.Log.Common.Print("player  is null");
                }
                if (player.mSaverThread != null)
                {
                    Log.Log.Common.Print("AddPlayer mSaverThread is null");
                }
                player.mSaverThread = this;
                mPushPlayers.Add(player, saver);
            }

            return saver;
        }
        public PlayerSaver FindPlayerSaver(CSCommon.Data.PlayerDataEx roleId)
        {
            PlayerSaver saver;
            if (mPushPlayers.TryGetValue(roleId, out saver))
            {
                return saver;
            }
            else if (mPlayers.TryGetValue(roleId, out saver))
            {
                return saver;
            }
            return null;
        }
        public PlayerSaver FindPlayerSaverById(ulong roleId)
        {
            foreach(var i in mPushPlayers)
            {
                if(i.Key.RoleDetail.RoleId==roleId)
                    return i.Value;
            }
            
            foreach(var i in mPlayers)
            {
                if(i.Key.RoleDetail.RoleId==roleId)
                    return i.Value;
            }

            return null;
        }
        void Tick()
        {
            lock (this)
            {
                foreach (var i in mPushPlayers)
                {
                    mPlayers.Add(i.Key, i.Value);
                }
                mPushPlayers.Clear();
            }

            mSaverNumber = 0;
            foreach (var i in mPlayers)
            {
                mSaverNumber += i.Value.GetNumber();
                i.Value.Tick(mDBConnect);
            }
            mDBConnect.Tick();
            lock (this)
            {
                bool bFinished;
                do
                {
                    bFinished = true;
                    foreach (var i in mPlayers)
                    {
                        if (i.Value.IsEmpty())
                        {
                            if (i.Value.mWaitRemove || mRunning == false)
                            {
                                mPlayers.Remove(i.Key);
                                i.Value.mPlayerDataEx = null;
                                bFinished = false;
                                break;
                            }
                        }
                    }
                } while (bFinished == false);
            }
        }
        bool mRunning;
        ServerFrame.DB.DBConnect mDBConnect;
        public ServerFrame.DB.DBConnect DBConnect
        {
            get { return mDBConnect; }
        }
        public void ThreadLoop()
        {
            while (GetPlayerCount() > 0 || mRunning)
            {
                try
                {
                    Tick();
                }
                catch (System.Exception ex)
                {
                    Log.Log.Common.Print(ex.ToString());
                    Log.Log.Common.Print(ex.StackTrace.ToString());
                }

                System.Threading.Thread.Sleep(1);
            }
            mThread = null;

            Log.Log.Common.Print(System.String.Format("PlayerSaverThread{0} Thread exit!", mThreadIndex));
        }
        System.Threading.Thread mThread;
        int mThreadIndex;
        public void StartThread(int threadIndex)
        {
            mDBConnect = new ServerFrame.DB.DBConnect();
            mDBConnect.OpenConnect();

            mThreadIndex = threadIndex;
            mRunning = true;
            mThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ThreadLoop));
            mThread.Start();
        }
        public void StopThread()
        {
            mRunning = false;
        }
    }

    public class DBConnectManager
    {
        static DBConnectManager smInstance = new DBConnectManager();
        public static DBConnectManager Instance
        {
            get { return smInstance; }
        }

        #region 多线程的数据库读取，主要用来做登陆，进入游戏队列
        AccountLoginThread mLoginLoader = new AccountLoginThread();
        public AccountLoginThread LoginLoader
        {
            get { return mLoginLoader; }
        }
        PlayerEnterThread mRoleLoadder = new PlayerEnterThread();
        public PlayerEnterThread RoleLoadder
        {
            get { return mRoleLoadder; }
        }
        public int GetLoginPipeNumber()
        {
            return mLoginLoader.GetNumber();
        }
        public void CancelLogin(string name)
        {
            mLoginLoader.CancelLogin(name);
        }
        #endregion

        #region 多线程数据库存储
        Dictionary<Guid, PlayerSaver> mPlayerSavers = new Dictionary<Guid, PlayerSaver>();
        List<PlayerSaverThread> mSaveThreads = new List<PlayerSaverThread>();
        public void StartThreadPool(int num)
        {
            for (int i = 0; i < num; i++)
            {
                PlayerSaverThread thread = new PlayerSaverThread();
                thread.StartThread(i);
                mSaveThreads.Add(thread);
            }
        }
        public void StopThreadPool()
        {
            foreach (var i in mSaveThreads)
            {
                i.StopThread();
            }
        }
        public bool IsEmptyPool()
        {
            int count = 0;
            foreach (var i in mSaveThreads)
            {
                count += i.GetPlayerCount();
            }
            return count == 0;
        }
        public int GetPlayerSaverNumber()
        {
            int count = 0;
            foreach (var i in mSaveThreads)
            {
                count += i.GetSaverNumber();
            }
            return count;
        }
        public bool InPool(CSCommon.Data.PlayerDataEx roleId)
        {
            PlayerSaver saver = null;
            foreach (var i in mSaveThreads)
            {
                saver = i.FindPlayerSaver(roleId);
                if (saver != null)
                    return true;
            }
            return false;
        }
        public PlayerSaver FindPlayerSaverById(ulong roleId)
        {
            PlayerSaver saver = null;
            foreach (var i in mSaveThreads)
            {
                saver = i.FindPlayerSaverById(roleId);
                if (saver != null)
                    return saver;
            }
            return null;
        }
        public void _ClearSaver(CSCommon.Data.PlayerDataEx roleId)
        {
            PlayerSaver saver = null;
            foreach (var i in mSaveThreads)
            {
                saver = i.FindPlayerSaver(roleId);
                if (saver != null)
                {
                    saver.Clear();
                }
            }
        }
        public PlayerSaver AddPlayer(CSCommon.Data.PlayerDataEx roleId)
        {
            PlayerSaver saver = null;
            PlayerSaverThread thread = null;
            Int32 mincount = Int32.MaxValue;
            foreach (var i in mSaveThreads)
            {
                int count = i.GetPlayerCount();
                if (count < mincount)
                {
                    mincount = count;
                    thread = i;
                }
                saver = i.FindPlayerSaver(roleId);
                if (saver != null)
                    return saver;
            }

            if (thread==null)
            {
                Log.Log.Common.Print("AddPlayer thread is null");
            }

            saver = thread.AddPlayer(roleId);
            return saver;
        }
        public void RemovePlayer(CSCommon.Data.PlayerDataEx roleId)
        {
            PlayerSaver saver = null;
            foreach (var i in mSaveThreads)
            {
                saver = i.FindPlayerSaver(roleId);
                if (saver != null)
                    break;
            }
            if (saver != null)
                saver.mWaitRemove = true;
        }
        public void PushSave(CSCommon.Data.PlayerDataEx roleId, ServerFrame.DB.DBOperator dbOp)
        {
            if (dbOp == null)
                return;
            if (roleId == null)
            {
                //走到这里说明DataServer已经重启过了，之前的在线玩家需要存盘，放到一个特殊线程处理就好了
                switch (dbOp.ExeType)
                {
                    case SqlExeType.Update:
                        {
                            PlayerEnterThread.Instance.DBConnect._ExecuteUpdate(dbOp);
                        }
                        break;
                    case SqlExeType.Insert:
                        {
                            PlayerEnterThread.Instance.DBConnect._ExecuteInsert(dbOp);
                        }
                        break;
                    case SqlExeType.Delete:
                        {
                            PlayerEnterThread.Instance.DBConnect._ExecuteDelete(dbOp);
                        }
                        break;
                    case SqlExeType.Destroy:
                        {
                            PlayerEnterThread.Instance.DBConnect._ExecuteDestroy(dbOp);
                        }
                        break;
                }
                return;
            }
            PlayerSaver saver = AddPlayer(roleId);

            saver.Push(dbOp);
            saver.mWaitRemove = false;//只要有数据重新要存储，说明他又登陆进来了，不需要remove了
        }

        public void PushSave(CSCommon.Data.PlayerDataEx roleId, string SqlCode, SqlExeType ExeType)
        {
            PlayerSaver saver = AddPlayer(roleId);

            ServerFrame.DB.DBOperator sqlAtom = new ServerFrame.DB.DBOperator();
            sqlAtom.SqlCode = SqlCode;
            sqlAtom.ExeType = ExeType;
            saver.Push(sqlAtom);
            saver.mWaitRemove = false;//只要有数据重新要存储，说明他又登陆进来了，不需要remove了
        }

        public void PushExecuter(CSCommon.Data.PlayerDataEx roleId, ServerFrame.DB.AsyncExecuter exec)
        {
            PlayerSaver saver = null;
            PlayerSaverThread thread = null;
            Int32 mincount = Int32.MaxValue;
            foreach (var i in mSaveThreads)
            {
                int count = i.GetPlayerCount();
                if (count < mincount)
                {
                    mincount = count;
                    thread = i;
                }
                saver = i.FindPlayerSaver(roleId);
                if (saver != null)
                    break;
            }
            if (saver == null)
            {
                saver = thread.AddPlayer(roleId);
            }
            ServerFrame.DB.DBOperator sqlAtom = new ServerFrame.DB.DBOperator();
            sqlAtom.Executer = exec;
            saver.Push(sqlAtom);
            saver.mWaitRemove = false;//只要有数据重新要存储，说明他又登陆进来了，不需要remove了
        }
        #endregion
    }
}
