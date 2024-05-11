using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum GameEventType
{
    None,

    NetworkSuccess,
    NetworkFaild,
    WebRequestError,

    /// <summary>
    /// 参数1: 状态  (1:Stay 2:Walk 3:Run 4:Sitting 5:Jump 6:Aiming 7:Attack 8:Damage 9:Death Reset)
    /// 参数2: 拥有者id
    /// </summary>
    EnemyStateChange
}

public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T1, T2>(T1 arg1, T2 arg2);
public delegate void Callback<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);


public static class MessageManager
{
    private static Dictionary<GameEventType, Delegate> allEventDic = new Dictionary<GameEventType, Delegate>();


    public static void AddListener(GameEventType eventType, Callback handler) => AddListenerInternal(eventType, handler);
    public static void AddListener<T>(GameEventType eventType, Callback<T> handler) => AddListenerInternal(eventType, handler);
    public static void AddListener<T1, T2>(GameEventType eventType, Callback<T1, T2> handler) => AddListenerInternal(eventType, handler);
    public static void AddListener<T1, T2, T3>(GameEventType eventType, Callback<T1, T2, T3> handler) => AddListenerInternal(eventType, handler);

    private static void AddListenerInternal<T>(GameEventType eventType, T handler) where T : Delegate
    {
        if (!IsCanAddListener(eventType, handler))
            return;

        if (!allEventDic.ContainsKey(eventType))
            allEventDic.Add(eventType, null);

        allEventDic[eventType] = Delegate.Combine(allEventDic[eventType], handler);
    }



    public static void RemoveListener(GameEventType eventType, Callback handler) => RemoveListenerInternal(eventType, handler);
    public static void RemoveListener<T>(GameEventType eventType, Callback<T> handler) => RemoveListenerInternal(eventType, handler);
    public static void RemoveListener<T1, T2>(GameEventType eventType, Callback<T1, T2> handler) => RemoveListenerInternal(eventType, handler);
    public static void RemoveListener<T1, T2, T3>(GameEventType eventType, Callback<T1, T2, T3> handler) => RemoveListenerInternal(eventType, handler);

    private static void RemoveListenerInternal<T>(GameEventType eventType, T handler) where T : Delegate
    {
        if (!IsCanRemoveListener(eventType, handler))
            return;

        allEventDic[eventType] = Delegate.Remove(allEventDic[eventType], handler);
        if (allEventDic[eventType] == null)
            allEventDic.Remove(eventType);
    }

    

    public static void Broadcast(GameEventType eventType) => BroadcastInternal(eventType);
    public static void Broadcast<T>(GameEventType eventType, T arg1) => BroadcastInternal(eventType, arg1);
    public static void Broadcast<T1, T2>(GameEventType eventType, T1 arg1, T2 arg2) => BroadcastInternal(eventType, arg1, arg2);
    public static void Broadcast<T1, T2, T3>(GameEventType eventType, T1 arg1, T2 arg2, T3 arg3) => BroadcastInternal(eventType, arg1, arg2, arg3);

    private static void BroadcastInternal(GameEventType eventType, params object[] args)
    {
        if (allEventDic.TryGetValue(eventType, out Delegate d))
        {
            d?.DynamicInvoke(args);
        }
        else
        {
            Debug.LogError($"Broadcast error: No delegate found for event {eventType}");
        }
    }


    private static bool IsCanAddListener(GameEventType eventType, Delegate handler)
    {
        if (allEventDic.TryGetValue(eventType, out Delegate currentDelegate))
        {
            if (currentDelegate != null && currentDelegate.GetType() != handler.GetType())
            {
                Debug.LogError($"Attempt to add incompatible delegate to existing delegates of type {currentDelegate.GetType().Name} for event type {eventType}");
                return false;
            }
        }
        return true;
    }

    private static bool IsCanRemoveListener(GameEventType eventType, Delegate handler)
    {
        if (allEventDic.TryGetValue(eventType, out Delegate currentDelegate))
        {
            if (currentDelegate == null)
            {
                Debug.LogError($"Attempt to remove delegate for event type {eventType} which has no listeners");
                return false;
            }
            if (currentDelegate.GetType() != handler.GetType())
            {
                Debug.LogError($"Attempt to remove delegate of type {handler.GetType().Name} from event type {eventType} which is currently of type {currentDelegate.GetType().Name}");
                return false;
            }
        }
        else
        {
            Debug.LogError($"MessageManager does not contain an event type {eventType}");
            return false;
        }
        return true;
    }
}
