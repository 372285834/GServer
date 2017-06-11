using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFrame
{
    public static class Time
    {
        public static System.DateTime start = System.DateTime.Now;
        public static long startTick = System.DateTime.Now.Ticks;
        //public static 

        public static float delta;  //每帧间隔时间 ，单位秒
        public static float time;  //从服务器启动到现在经过的秒

        static System.DateTime lastTick;
        public static void Tick()
        {
            delta = (float)(System.DateTime.Now - lastTick).TotalSeconds;
            time = (float)(System.DateTime.Now - start).TotalSeconds;
            lastTick = System.DateTime.Now;
        }

        public static System.DateTime AddSecondNow(float second)
        {
            return System.DateTime.Now + new System.TimeSpan((long)(second * 10000000));
        }

        public static bool InitTimes(int ftime, System.DateTime lasttime)
        {
            System.DateTime nowtime = System.DateTime.Now;
            if ((nowtime - lasttime).TotalHours > 24
                || ((nowtime.Day != lasttime.Day) && lasttime.Hour < ftime)
                || ((nowtime.Day != lasttime.Day) && (lasttime.Hour >= ftime && nowtime.Hour >= ftime))
                || ((nowtime.Day == lasttime.Day) && (nowtime.Hour >= ftime) && lasttime.Hour < ftime))
            {
                return true;
            }
            return false;
        }
    }
}
