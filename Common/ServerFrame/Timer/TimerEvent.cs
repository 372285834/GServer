using System.Collections;

namespace ServerFrame
{
    /// <summary>
    /// 时间回调函数的参数;
    /// </summary>
    public class TimerEvent
    {
        //参数;
        public object param;
        //键值;
        public object key;

        public TimerEvent(object value)
        {
            param = value;
        }

        public TimerEvent()
        {
        }
    }
}
