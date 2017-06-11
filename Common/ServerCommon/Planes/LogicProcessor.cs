using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon.Planes
{
    public class LogicProcessor
    {
        //这个逻辑处理器负责的地图列表
        List<MapInstance> mMaps = new List<MapInstance>();

        public int GetMapCount()
        {
            return mMaps.Count;
        }

        public bool InProcessor(MapInstance map)
        {
            lock (this)
            {
                foreach (var i in mMaps)
                {
                    if (i == map)
                        return true;
                }
            }
            return false;
        }

        public bool PushMap(MapInstance map)
        {
            if (map.LogicProcessor == this)
                return true;
            if (InProcessor(map))
                return true;
            if (map.LogicProcessor != null)
            {
                map.LogicProcessor.RemoveMap(map);
            }
            map.LogicProcessor = this;
            mMaps.Add(map);
            return true;
        }
        private void RemoveMap(MapInstance map)
        {
            for (int i = 0; i < mMaps.Count; i++)
            {
                if (mMaps[i] == map)
                {
                    mMaps.RemoveAt(i);
                }
            }
        }

        Int64 mTickTime = IServer.timeGetTime();
        public void Tick()
        {
            var nowTime = IServer.timeGetTime();
            Int64 elapsedMiliSeccond = nowTime - mTickTime;
            mTickTime = nowTime;

            try
            {
                for (int i = 0; i < mMaps.Count; i++)
                {
                    MapInstance map = mMaps[i];

                    if (map != null)
                    {
                        map.Tick(elapsedMiliSeccond);
                        if (map.WaitDestory)
                        {
                            lock (LogicProcessorManager.Instance)
                            {
                                mMaps.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.Common.Print(ex.ToString());
                Log.Log.Common.Print(ex.StackTrace.ToString());
            }
        }
        bool mRunning;
        System.Threading.Thread mThread;
        public void ThreadLoop()
        {
            while (mRunning)
            {
                uint begin = IServer.timeGetTime();
                Tick();
                uint end = IServer.timeGetTime();
                uint elapse = end - begin;
                if (elapse < 50)
                    System.Threading.Thread.Sleep((int)(50 - elapse));
            }
            mThread = null;
            Log.Log.Common.Print(System.String.Format("LogicProcessor{0} Thread exit!", mThreadIndex));
        }
        int mThreadIndex;
        public void StartThread(int threadIndex)
        {
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

    public class LogicProcessorManager
    {
        static LogicProcessorManager smInstance = new LogicProcessorManager();
        public static LogicProcessorManager Instance
        {
            get { return smInstance; }
        }
        LogicProcessor[] mProcessors;
        public void StartProcessors(int count)
        {
            mProcessors = new LogicProcessor[count];
            for (int i = 0; i < mProcessors.Length; i++)
            {
                mProcessors[i] = new LogicProcessor();
                mProcessors[i].StartThread(i);
            }
        }
        public void StopProcessors()
        {
            for (int i = 0; i < mProcessors.Length; i++)
            {
                mProcessors[i].StopThread();
            }
        }
        public void PushMap(MapInstance map)
        {
            lock (LogicProcessorManager.Instance)
            {
                LogicProcessor processor = null;
                Int32 minMap = Int32.MaxValue;
                for (int i = 0; i < mProcessors.Length; i++)
                {
                    if (mProcessors[i].InProcessor(map))
                        return;
                    int count = mProcessors[i].GetMapCount();
                    if (minMap > count)
                    {
                        minMap = count;
                        processor = mProcessors[i];
                    }
                }
                if (processor == null)
                    return;
                processor.PushMap(map);
            }
        }
    }
}
