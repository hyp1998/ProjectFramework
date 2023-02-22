using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 多事件系统管理 分类
/// 用于保证成对注册和注销
/// </summary>
public class EventHanlderManager
{
    /// <summary>
    /// 派遣类型
    /// 可拓展
    /// </summary>
    public enum DispatchType
    {
        Event,                  //普通事件
        Server,                 //网络事件
        OnceEventTrigger,       //一次性事件触发
    }

    /// <summary>
    /// 当前实例类代表的事件系统
    /// </summary>
    private BaseEvent mDispatcher = null;

    //用于保证成对注册和注销，通过这两个字典去（注册添加）（删除）事件
    private Dictionary<uint, BaseEvent.CallBack> mCBPairs = new Dictionary<uint, BaseEvent.CallBack>();
    private Dictionary<uint, List<BaseEvent.CallBack>> mMultiCBPairs;

    //静态是因为  仅只存在一个实例 
    static BaseEvent mEventDispatcher = new BaseEvent();
    static BaseEvent mServerDispatcher = new BaseEvent();
    static BaseEvent mOnceEventTriggerDispatcher = new BaseEvent();

    public EventHanlderManager()
    {
        //默认
        mDispatcher = mEventDispatcher;
    }

    public EventHanlderManager(DispatchType dispatchType)
    {
        SetEventType(dispatchType);
    }

    /// <summary>
    /// 设置当前事件类型
    /// </summary>
    /// <param name="dispatchType"></param>
    private void SetEventType(DispatchType dispatchType)
    {
        switch (dispatchType)
        {
            case DispatchType.Server:
                mDispatcher = mServerDispatcher;
                break;
            case DispatchType.Event:
                mDispatcher = mEventDispatcher;
                break;
            case DispatchType.OnceEventTrigger:
                mDispatcher = mOnceEventTriggerDispatcher;
                break;
        }
    }

    /// <summary>
    /// 添加注册事件
    /// </summary>
    /// <param name="msgId"></param>
    /// <param name="callBack"></param>
    public void Reg(uint msgId, BaseEvent.CallBack callBack)
    {
        List<BaseEvent.CallBack> cbList = null;
        if (mMultiCBPairs != null && mMultiCBPairs.TryGetValue(msgId, out cbList))
        {
            //这里这样写，会省去for循环去挨个判断List<BaseEvent.CallBack>中的元素 简单粗暴
            cbList.Remove(callBack);
            cbList.Add(callBack);
            mMultiCBPairs[msgId] = cbList;
        }
        else
        {
            BaseEvent.CallBack cb = null;
            if (mCBPairs.TryGetValue(msgId, out cb))
            {
                //他会自动判断所减的委托和自己是否一样，如果一样减去就为null，不一样则没有操作，不为null
                cb -= callBack;
                //此时已经没有事件委托了，重新在单个的字典里(mCBPairs)添加一下
                if (cb == null)
                {
                    cb = callBack;
                    mCBPairs[msgId] = cb;
                }
                else
                {
                    //说明还有其他的，就需要在多个的字典里(mMultiCBPairs)添加了
                    //先从单个的事件里面的记录删掉  确保ID只存在一个
                    mCBPairs.Remove(msgId);
                    //重新添加到多个的字典里面
                    cbList = new List<BaseEvent.CallBack>();
                    cbList.Add(cb);
                    cbList.Add(callBack);
                    if (mMultiCBPairs == null)
                        mMultiCBPairs = new Dictionary<uint, List<BaseEvent.CallBack>>();
                    mMultiCBPairs[msgId] = cbList;
                }
            }
            else
            {
                mCBPairs.Add(msgId, callBack);
            }
        }

        mDispatcher.RegEvent(msgId, callBack);
    }

    /// <summary>
    /// 删除消息ID中对应的某一个回调
    /// </summary>
    /// <param name="msgId"></param>
    /// <param name="callBack"></param>
    public void UnReg(uint msgId, BaseEvent.CallBack callBack)
    {
        BaseEvent.CallBack cb;
        if (mCBPairs.TryGetValue(msgId, out cb))
        {
            cb -= callBack;
            if (cb == null)
                mCBPairs.Remove(msgId);
        }
        else
        {
            if (mMultiCBPairs != null)
            {
                List<BaseEvent.CallBack> cbList;
                if (mMultiCBPairs.TryGetValue(msgId, out cbList))
                {
                    cbList.Remove(callBack);
                    if (cbList.Count == 0)
                        mMultiCBPairs.Remove(msgId);
                }
            }
        }

        mDispatcher.UnRegEvent(msgId, callBack);
    }

    /// <summary>
    /// 删除消息ID中对应的所有回调
    /// </summary>
    /// <param name="msgId"></param>
    public void UnReg(uint msgId)
    {
        if (mCBPairs.TryGetValue(msgId, out BaseEvent.CallBack cb))
        {
            mDispatcher.UnRegEvent(msgId, cb);
            mCBPairs.Remove(msgId);
        }
        else
        {
            if (mMultiCBPairs != null)
            {
                List<BaseEvent.CallBack> cbList;
                if (mMultiCBPairs.TryGetValue(msgId, out cbList))
                {
                    //删除所有回调
                    for (int i = 0; i < cbList.Count; i++)
                    {
                        mDispatcher.UnRegEvent(msgId, cbList[i]);
                    }
                    mMultiCBPairs.Remove(msgId);
                }
            }
        }
    }

    /// <summary>
    /// 清空当前所有事件
    /// </summary>
    public void UnRegAll()
    {
        foreach (KeyValuePair<uint, BaseEvent.CallBack> item in mCBPairs)
        {
            mDispatcher.UnRegEvent(item.Key, item.Value);
        }

        if (mMultiCBPairs != null)
        {
            foreach (KeyValuePair<uint, List<BaseEvent.CallBack>> item in mMultiCBPairs)
            {
                List<BaseEvent.CallBack> cbList = item.Value;
                for (int i = 0; i < cbList.Count; i++)
                {
                    mDispatcher.UnRegEvent(item.Key, cbList[i]);
                }
            }
            mMultiCBPairs.Clear();
        }
        mCBPairs.Clear();
    }

    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="data"></param>
    public void SendEvent(uint eventId,params object[] data)
    {
        mDispatcher.SendEvent(eventId, data);
    }


}

/// <summary>
/// 基础事件类
/// </summary>
public class BaseEvent
{
    /// <summary>
    /// 事件委托
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="data"></param>
    public delegate void CallBack(uint eventId, params object[] data);

    /// <summary>
    /// 事件集合
    /// </summary>
    private Dictionary<uint, EventDelegate> mEventBaseDic = new Dictionary<uint, EventDelegate>();

    /// <summary>
    /// 添加注册事件
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cb"></param>
    public void RegEvent(uint eventId, CallBack cb)
    {
        EventDelegate _baseEvent;
        if (!mEventBaseDic.ContainsKey(eventId))
        {
            _baseEvent = new EventDelegate(eventId);
            mEventBaseDic.Add(eventId, _baseEvent);
        }
        else
        {
            _baseEvent = mEventBaseDic[eventId];
        }
        if (cb != null)
        {
            _baseEvent.AddCallBack(cb);
        }
    }

    /// <summary>
    /// 删除事件
    /// </summary>
    /// <param name="eventId"></param>
    public void UnRegEvent(uint eventId)
    {
        if (mEventBaseDic.ContainsKey(eventId))
        {
            mEventBaseDic.Remove(eventId);
        }
        else
        {
            Debug.LogError($"事件删除 >>> EventId:{eventId}不存在");
        }
    }

    /// <summary>
    /// 删除事件
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cb"></param>
    public void UnRegEvent(uint eventId, CallBack cb)
    {
        if (mEventBaseDic.TryGetValue(eventId, out EventDelegate eventDelegate))
        {
            eventDelegate.RemoveCallBack(cb);
        }
        else
        {
            Debug.LogError($"事件删除 >>> EventId:{eventId}不存在");
        }
    }

    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="data"></param>
    public void SendEvent(uint eventId, params object[] data)
    {
        if (mEventBaseDic.TryGetValue(eventId, out EventDelegate mEventBase))
        {
            mEventBase.SendEvent(data);
        }
        else
        {
            Debug.LogError($"发送事件 >>> EventId:{eventId}不存在");
        }
    }

    /// <summary>
    /// 事件委托类
    /// </summary>
    public class EventDelegate
    {

        /// <summary>
        /// 事件ID  唯一
        /// </summary>
        public uint eventId;

        /// <summary>
        /// 回调
        /// </summary>
        CallBack callback;

        public EventDelegate(uint _eventId)
        {
            this.eventId = _eventId;
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="cb"></param>
        public void AddCallBack(CallBack cb)
        {
            if (callback == null)
            {
                callback = cb;
            }
            else
            {
                callback -= cb;
                callback += cb;
            }
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="cb"></param>
        public void RemoveCallBack(CallBack cb)
        {
            if (callback != null)
            {
                callback -= cb;
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="objData"></param>
        public void SendEvent(params object[] objData)
        {
            callback?.Invoke(this.eventId, objData);
        }

    }
}

