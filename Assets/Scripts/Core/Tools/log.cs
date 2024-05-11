using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class log
{
    static bool IsShowLog = true;

    public static void SetShowLog(bool isShowLog)
    {
        IsShowLog = isShowLog;
    }

    public static void StartFunction()
    {


    }

    #region    Log
    public static void Log(object message)
    {
        if (IsShowLog)
        {
            Debug.Log(GetCurMethodName() + message);
        }
    }
    public static void Log(object message, Object context)
    {
        if (IsShowLog)
        {
            Debug.Log(GetCurMethodName() + message, context);
        }
    }

    public static void LogWarning(object log)
    {
        if (IsShowLog)
        {
            Debug.LogWarning(GetCurMethodName() + log);
        }
    }

    public static void LogError(object log)
    {
        if (IsShowLog)
        {
            Debug.LogError(GetCurMethodName() + log);
        }
    }


    public static void LogFormat(string format, params object[] args)
    {
        if (IsShowLog)
        {
            Debug.LogFormat(GetCurMethodName() + format, args);
        }
    }
    public static void LogFormat(Object context, string format, params object[] args)
    {
        if (IsShowLog)
        {
            Debug.LogFormat(context, GetCurMethodName() + format, args);
        }
    }
    public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
    {
        if (IsShowLog)
        {
            Debug.LogFormat(logType, logOptions, context, GetCurMethodName() + format, args);
        }
    }
    public static void LogWarningFormat(string format, params object[] args)
    {
        if (IsShowLog)
        {
            Debug.LogWarningFormat(GetCurMethodName() + format, args);
        }
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
        if (IsShowLog)
        {
            Debug.LogErrorFormat(GetCurMethodName() + format, args);
        }
    }
    

    #endregion


    public static string GetCurMethodName()
    {
        System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame(2);
        System.Reflection.MethodBase methodBase = stackFrame.GetMethod();
        string str = string.Format("<color=#00E7FF>{0} -> {1}() :   </color>", methodBase.ReflectedType.FullName, methodBase.Name);
        return str;
    }


    public static  void LogArray<T>(T[] array )
    {
        string str = string.Format("{0} \n", array.ToString());
        for (int i = 0; i < array.Length; i++)
        {
            str += string.Format("{0} -> {1}\n", i, array[i]);
        }
        log.LogFormat(str);
    }

    public static void LogList<T>(List<T> list)
    {
        string str = string.Format("{0} \n", list.ToString());
        for (int i = 0; i < list.Count; i++)
        {
            //System.Type t = list[i].GetType();
            //if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
            //{
            //}
            str += string.Format("{0} -> {1} -> Type:{2} -> HashCode:{3} \n", i, list[i], list[i].GetType() , list[i].GetHashCode());
        }
        log.LogFormat(str);
    }

    public static void LogDictionary<K, T>(Dictionary<K, T> dic)
    {
        string str = string.Format("{0} \n", dic.ToString());
        foreach (var item in dic)
        {
            //str += string.Format("{0} -> {1}\n", item.Key, item.Value);
            str += string.Format("{0} -> {1} -> Type:{2} -> HashCode:{3} \n", item.Key , item.Value , item.Value.GetType(), item.Value.GetHashCode());
        }
        log.LogFormat(str);
    }
}
