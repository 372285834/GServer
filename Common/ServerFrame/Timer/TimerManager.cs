using System.Collections;
using System.Collections.Generic;
using System;


namespace ServerFrame
{

    //全局计时器;
    public class TimerManager
    {
        static System.DateTime mAppStartTime = System.DateTime.Now;
        /// <summary>
        /// 从程序启动到现在的时间
        /// </summary>
        public static float PassTime
        {
            get
            {
                var span = System.DateTime.Now - mAppStartTime;
                return (float)span.TotalSeconds;
            }
        }


        //回调方法池;
        public static HashList<TimerHandler> handlers = new HashList<TimerHandler>();
        //当前帧数;
        protected static float currentFrame = 0;
        //当前时间;
        protected static float currentTime = PassTime;
        //计时器方法数量;
        protected static uint count = 0;
        //计时器方法索引;
        protected static int index = 0;
        //方法缓存池;
        protected static ArrayList pool = new ArrayList();


        //回调方法池;
        public static HashList<TodayTimerHandler> todayHandlers = new HashList<TodayTimerHandler>();


        //帧循环;
        public static void Tick()
        {
            runTimerHandle();
            runTodayTimerHandle();
        }

        /**清理定时器;
         * @param	method 创建时的cover=true时method为回调函数本身，否则method为返回的唯一ID;
         */
        public static void clearTimer(System.Object key)
        {
            if (key != null)
            {
                if (handlers.hash.ContainsKey(key))
                {
                    TimerHandler handler = handlers.getElementByKey(key);
                    handlers.removeElementByKey(key);
                    handler = null;
                }
            }
        }

        /**定时执行一次;
            * @param	delay  延迟时间(单位秒);
            * @param	method 结束时的回调方法;
            * @param	args   回调参数;
            * @param	cover  是否覆盖(true:同方法多次计时，后者覆盖前者。false:同方法多次计时，不相互覆盖);
            * @return  cover=true时返回回调函数本身，cover=false时，返回唯一ID，均用来作为clearTimer的参数;*/
        public static System.Object doOnce(float delay, TimerMethod method, object args = null, bool cover = false)
        {
            return create(false, false, delay, method, args, cover);
        }

        /**定时重复执行;
            * @param	delay  延迟时间(单位秒);
            * @param	method 结束时的回调方法;
            * @param	args   回调参数;
            * @param	cover  是否覆盖(true:同方法多次计时，后者覆盖前者。false:同方法多次计时，不相互覆盖);
            * @return  cover=true时返回回调函数本身，cover=false时，返回唯一ID，均用来作为clearTimer的参数;*/
        public static System.Object doLoop(float delay, TimerMethod method, object args = null, bool cover = false)
        {
            return create(false, true, delay, method, args, cover);
        }

        /**定时执行一次(基于帧率);
            * @param	delay  延迟时间(单位为帧);
            * @param	method 结束时的回调方法;
            * @param	args   回调参数;
            * @param	cover  是否覆盖(true:同方法多次计时，后者覆盖前者。false:同方法多次计时，不相互覆盖);
            * @return  cover=true时返回回调函数本身，cover=false时，返回唯一ID，均用来作为clearTimer的参数;*/
        public static System.Object doFrameOnce(float delay, TimerMethod method, object args = null, bool cover = false)
        {
            return create(true, false, delay, method, args, cover);
        }

        /**定时重复执行(基于帧率);
            * @param	delay  延迟时间(单位为帧);
            * @param	method 结束时的回调方法;
            * @param	args   回调参数;
            * @param	cover  是否覆盖(true:同方法多次计时，后者覆盖前者。false:同方法多次计时，不相互覆盖);
            * @return  cover=true时返回回调函数本身，否则返回唯一ID，均用来作为clearTimer的参数*/
        public static System.Object doFrameLoop(float delay, TimerMethod method, object args = null, bool cover = false)
        {
            return create(true, true, delay, method, args, cover);
        }

        //创建计时器;
        protected static System.Object create(bool useFrame, bool repeat, float delay, TimerMethod method, object args, bool cover = false)
        {
            System.Object key;
            if (cover)
            {
                //先删除相同函数的计时;
                clearTimer(method);
                key = method;
            }
            else
            {
                key = index++;
            }

            TimerHandler handler;
            if (pool.Count > 0)
            {
                handler = (TimerHandler)pool[pool.Count - 1];
                pool.RemoveAt(pool.Count - 1);
            }
            else
                handler = new TimerHandler();


            TimerEvent events = new TimerEvent(args);
            events.key = key;
            handler.userFrame = useFrame;
            handler.repeat = repeat;
            handler.delay = delay;
            handler.method = method;
            handler.args = events;
            handler.exeTime = delay + (useFrame ? currentFrame : currentTime);
            handlers.addElement(key, handler);
            count++;
            return key;
        }

        /// <summary>
        /// 执行时间回调函数;
        /// </summary>
        protected static void runTimerHandle(int index = 0)
        {
            int count = handlers.count;
            if (index == 0)
            {
                currentFrame++;
            }
            currentTime = PassTime;
            if (index >= count)
                return;
            TimerHandler value = handlers.getElementByIndex(index);
            int timeIndex = index;
            float t = value.userFrame ? currentFrame : currentTime;
            timeIndex = index + 1;
            if (t >= value.exeTime)
            {
                if (value.repeat)
                {
                    while (t >= value.exeTime)
                    {
                        value.exeTime += value.delay;
                        runTimerMethod(value);
                    }
                }
                else
                {
                    TimerHandler handler = handlers.getElementByIndex(index);
                    runTimerMethod(value);
                    clearTimer(handler.args.key);
                    timeIndex -= 1;
                }
            }
            runTimerHandle(timeIndex);
        }

        /// <summary>
        /// 执行计时器回调函数;
        /// </summary>
        /// <param name="value"></param>
        protected static void runTimerMethod(TimerHandler value)
        {
            try
            {
                TimerMethod method = value.method;
                if (method != null)
                    method(value.args);
            }
            catch (System.Exception ex)
            {
                Log.Log.Server.Warning(ex.ToString());
            }
        }


        /// <summary>
        /// 今天指定时间做调用，hour(18:30填18.5)，method(调用的函数),args(函数参数)
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        public static void RegTodayTimerEvent(float hour, TimerMethod method, object args = null)
        {
            if (method == null)
            {
                return;
            }
            if (todayHandlers.hash.ContainsKey(method))
            {
                return;
            }

            TodayTimerHandler handler = new TodayTimerHandler();
            TimerEvent events = new TimerEvent(args);
            events.key = method;
            handler.method = method;
            handler.args = events;
            handler.hour = hour;
            System.DateTime now = System.DateTime.Now;
            int phour = (int)hour;
            int pminute = (int)(60 * (hour - phour));
            if (now.Hour >= phour && now.Minute >= pminute)
            {
                handler.isTodayExe = true;
            }
            else
            {
                handler.isTodayExe = false;
            }
            handler.lastTime = System.DateTime.Now;
            todayHandlers.addElement(method, handler);
        }

        /// <summary>
        /// 执行时间回调函数;
        /// </summary>
        protected static void runTodayTimerHandle(int index = 0)
        {
            int count = todayHandlers.count;
            if (index >= count)
                return;

            TodayTimerHandler value = todayHandlers.getElementByIndex(index);
            System.DateTime now = System.DateTime.Now;
            if (value.isTodayExe == false)
            {
                int hour = (int)value.hour;
                int minute = (int)(60 * (value.hour - hour));
                if (now.Hour == hour && now.Minute >= minute)
                {
                    runTodayTimerMethod(value);
                    value.isTodayExe = true;
                }
            }
            if (value.lastTime.Day != System.DateTime.Now.Day)
            {
                value.isTodayExe = false;
            }
            value.lastTime = System.DateTime.Now;

            index++;
            runTodayTimerHandle(index);
        }

        /// <summary>
        /// 执行计时器回调函数;
        /// </summary>
        /// <param name="value"></param>
        protected static void runTodayTimerMethod(TodayTimerHandler value)
        {
            TimerMethod method = value.method;
            if (method != null)
                method(value.args);
        }
    }
}
