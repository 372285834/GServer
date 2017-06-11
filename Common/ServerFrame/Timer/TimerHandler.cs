using System.Collections;

namespace ServerFrame
{

    //计时器回调函数;
    public delegate void TimerMethod(TimerEvent timerEvent);

    //定时器函数类;
    public class TimerHandler
    {

        /**执行间隔;*/
        public float delay;
        /**是否重复执行;*/
        public bool repeat;
        /**是否用帧率;*/
        public bool userFrame;
        /**执行时间;*/
        public float exeTime;
        /**处理方法;*/
        public TimerMethod method;
        /**参数;*/
        public TimerEvent args;

        /**清理;*/
        public void clear()
        {
            method = null;
            args = null;
        }
    }

    //每日定时器函数类;
    public class TodayTimerHandler
    {
        /**几点钟执行,（7:30是7.5）*/
        public float hour;

        /**上次调用时间;*/
        public System.DateTime lastTime;

        /**今天是否调用;*/
        public bool isTodayExe;

        /**处理方法;*/
        public TimerMethod method;

        /**参数;*/
        public TimerEvent args;


        /**清理;*/
        public void clear()
        {
            method = null;
            args = null;
        }
    }
}
