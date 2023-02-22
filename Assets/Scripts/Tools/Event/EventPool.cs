using System.Collections;
using System.Collections.Generic;
using System;

namespace FX.Runtime.Framework
{

    public class EventPool<T>
    {

        private readonly Dictionary<int, EventHandler<T>> m_EventHandlers;
        private readonly Queue<Event> m_Events;

        /// <summary>
        /// 初始化事件池的新实例。
        /// </summary>
        /// <param name="mode">事件池模式。</param>
        public EventPool()
        {
            m_EventHandlers = new Dictionary<int, EventHandler<T>>();
            m_Events = new Queue<Event>();
        }

        /// <summary>
        /// 获取事件数量。
        /// </summary>
        public int Count
        {
            get
            {
                return m_Events.Count;
            }
        }


        public void Update(float deltaTime, float realElapseSeconds)
        {
            while (m_Events.Count > 0)
            {
                Event e;
                lock (m_Events)
                {
                    e = m_Events.Dequeue();
                }

                HandleEvent(e.Id, e.Sender, e.EventArgs);
            }

        }

        /// <summary>
        /// 关闭并清理事件池。
        /// </summary>
        public void Shutdown()
        {
            Clear();
            m_EventHandlers.Clear();
        }

        /// <summary>
        /// 清理事件。
        /// </summary>
        public void Clear()
        {
            lock (m_Events)
            {
                m_Events.Clear();
            }
        }

        /// <summary>
        /// 检查订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要检查的事件处理函数。</param>
        /// <returns>是否存在事件处理函数。</returns>
        public bool Check(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            EventHandler<T> handlers = null;
            if (!m_EventHandlers.TryGetValue(id, out handlers))
            {
                return false;
            }

            if (handlers == null)
            {
                return false;
            }

            foreach (EventHandler<T> e in handlers.GetInvocationList())
            {
                if (e == handler)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要订阅的事件处理函数。</param>
        public void Subscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            EventHandler<T> eventHandler = null;
            if (!m_EventHandlers.TryGetValue(id, out eventHandler) || eventHandler == null)
            {
                m_EventHandlers[id] = handler;
            }
            else
            {
                eventHandler += handler;
                m_EventHandlers[id] = eventHandler;
            }
        }

        /// <summary>
        /// 取消订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要取消订阅的事件处理函数。</param>
        public void Unsubscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            if (m_EventHandlers.ContainsKey(id))
            {
                m_EventHandlers[id] -= handler;
            }
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void RaiseEvent(int eventId, object sender = null, T e = default(T))
        {
            Event eventNode = new Event(eventId, sender, e);
            lock (m_Events)
            {
                m_Events.Enqueue(eventNode);
            }
        }

        /// <summary>
        /// 抛出事件，立即执行。这个操作线程不安全
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RaiseEventNow(int eventId, object sender, T e)
        {
            HandleEvent(eventId, sender, e);
        }



        /// <summary>
        /// 处理事件结点。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        private void HandleEvent(int eventId, object sender, T e)
        {
            EventHandler<T> handlers = null;
            if (m_EventHandlers.TryGetValue(eventId, out handlers))
            {
                if (handlers != null)
                {
                    handlers(sender, e);
                }
            }

            if (handlers == null)
            {
                UnityEngine.Debug.LogWarning(string.Format("Event of event id {0} not allow no handler.", eventId));
                //throw new Exception(string.Format("Event '{0}' not allow no handler.", e.ToString()));
            }
        }


        private sealed class Event
        {
            private readonly int m_Id;
            private readonly object m_Sender;
            private readonly T m_EventArgs;

            public Event(int id, object sender, T e)
            {
                m_Id = id;
                m_Sender = sender;
                m_EventArgs = e;
            }

            public object Sender
            {
                get
                {
                    return m_Sender;
                }
            }

            public T EventArgs
            {
                get
                {
                    return m_EventArgs;
                }
            }

            public int Id
            {
                get
                {
                    return m_Id;
                }
            }
        }


    }

}
