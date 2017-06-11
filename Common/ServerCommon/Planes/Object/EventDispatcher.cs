using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;

namespace ServerCommon.Planes
{
    public class EventDispatcher
    {
        public delegate void EVENT_FUNCTION(EventDispatcher listener, object data);

        static Dictionary<EventDispatcher, Dictionary<EventDispatcher, int>> gListenerDict = new Dictionary<EventDispatcher, Dictionary<EventDispatcher, int>>();
        struct LISTENER
	    {
		    public EventType Type;
            public WeakReference Listener;
            public EVENT_FUNCTION Function;
	    };

        List<WeakReference> mListListenTo;
        List<LISTENER> mListListener;

        public EventDispatcher()
        {
            mListListenTo = new List<WeakReference>();
            mListListener = new List<LISTENER>();
        }

        public void AddEventListener(EventType type, EventDispatcher listener, EVENT_FUNCTION callback)
        {
            lock (gListenerDict)
            {
                LISTENER listen = new LISTENER();
                listen.Type = type;
                listen.Listener = new WeakReference(listener);
                listen.Function = callback;
                RemoveEventListener(type, listener, callback);
                if (!gListenerDict.ContainsKey(this))
                    gListenerDict[this] = new Dictionary<EventDispatcher, int>();
                gListenerDict[this][listener] = 0;
                mListListener.Add(listen);
                foreach (var item in listener.mListListenTo)
                {
                    if (item.Target == this) return;
                }
                listener.mListListenTo.Add(new WeakReference(this));
            }
        }

        public void RemoveEventListener(EventType type, EventDispatcher listener, EVENT_FUNCTION callback)
        {
            lock (gListenerDict)
            {
                for (var i = 0; i < mListListener.Count; i++)
                {
                    LISTENER item = mListListener[i];
                    if (item.Type == type && item.Function == callback && item.Listener.Target == listener)
                    {
                        if (mListListener.Count == 1)
                        {
                            for (var j = 0; j < listener.mListListenTo.Count; j++)
                            {
                                if (listener.mListListenTo[j].Target == this)
                                {
                                    listener.mListListenTo.RemoveAt(j);
                                    j--;
                                    break;
                                }
                            }
                            gListenerDict.Remove(this);
                        }
                        mListListener.Remove(item);
                        i--;
                        break;
                    }
                }
            }
        }

        public void DispatchEvent(EventType type, object data)
        {
            List<LISTENER> tmp = new List<LISTENER>();
            for (var i = 0; i < mListListener.Count(); i++)
            {
                if (mListListener[i].Type == type)
                {
                    tmp.Add(mListListener[i]);
                }
            }

            //有些回调函数会删除自己的事件，甚至后面的事件，所以先选择符合条件的，再一一执行，防止空指针
            //如果不用for直接循环list就会出问题
            //要防止事件套事件，造成死循环
            for (var i = 0; i < tmp.Count; i++)
            {
                EventDispatcher listener = tmp[i].Listener.Target as EventDispatcher;
                if (null == listener) continue;

                tmp[i].Function(listener, data);
            }
        }

        public virtual void CleanUp()
        {
            ClearEventListener();
        }

        public void ClearEventListener()
        {
            lock (gListenerDict)
            {
                Dictionary<EventDispatcher, int> dict = new Dictionary<EventDispatcher, int>();
                if (gListenerDict.TryGetValue(this, out dict))
                {
                    foreach (var item in dict)
                    {
                        item.Key.mListListenTo.Clear();
                    }
                    gListenerDict.Remove(this);
                    mListListener.Clear();
                }
                foreach (var item in mListListenTo)
                {
                    EventDispatcher owner = item.Target as EventDispatcher;
                    if (null == owner) continue;

                    for (var i = 0; i < owner.mListListener.Count(); i++)
                    {
                        if (owner.mListListener[i].Listener.Target == this)
                            owner.mListListener.RemoveAt(i);
                    }
                }
            }
        }

        public static void AutoRemoveNoRefEventListener()
        {
            lock (gListenerDict)
            {
                List<EventDispatcher> owners = gListenerDict.Keys.ToList();
                for (var i = 0; i < owners.Count; i++)
                {
                    EventDispatcher owner = owners[i];
                    RemoveNoRefEventListener(ref owner);
                    if (owner.mListListenTo.Count == 0 && owner.mListListener.Count == 0)
                    {
                        gListenerDict.Remove(owner);
                        owners.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        List<EventDispatcher> listeners = gListenerDict[owner].Keys.ToList();
                        for (var j = 0; j < listeners.Count; j++)
                        {
                            EventDispatcher listener = listeners[j];
                            RemoveNoRefEventListener(ref listener);
                            if (listener.mListListenTo.Count == 0 && listener.mListListener.Count == 0)
                            {
                                gListenerDict[owner].Remove(listener);
                                listeners.RemoveAt(j);
                                j--;
                            }
                        }
                        if (listeners.Count == 0)
                        {
                            gListenerDict.Remove(owner);
                            owners.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }

        public static void RemoveNoRefEventListener(ref EventDispatcher dispatcher)
        {
            for (var i = 0; i < dispatcher.mListListenTo.Count; i++)
            {
                if (null == dispatcher.mListListenTo[i].Target)
                    dispatcher.mListListenTo.RemoveAt(i);
            }
            for (var i = 0; i < dispatcher.mListListener.Count; i++)
            {
                if (null == dispatcher.mListListener[i].Listener.Target)
                    dispatcher.mListListener.RemoveAt(i);
            }
        }


    }
}
