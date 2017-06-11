using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFrame
{
    /// <summary>
    /// 观察者模式，观察者接口;
    /// </summary>
    public interface IObserver
    {
        void OnChanged(object[] param);
    }

    /// <summary>
    /// 观察者模式主题接口;
    /// </summary>
    public interface ISubject
    {
        /// <summary>
        /// 增加观察者;
        /// </summary>
        void AddObserver(IObserver observer);

        /// <summary>
        /// 移除观察者;
        /// </summary>
        void RemoveObserver(IObserver observer);

        /// <summary>
        /// 通知观察者;
        /// </summary>
        void Change(object[] param = null);
    }
}
