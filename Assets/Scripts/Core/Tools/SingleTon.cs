using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class SingleTon<T> where T : SingleTon<T>
{
    private static T instance;
    private static readonly object lockObj = new object();  // 锁对象

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        ConstructorInfo ctor = typeof(T).GetConstructor(
                            BindingFlags.Instance | BindingFlags.NonPublic,
                            null, new Type[0], null);

                        if (ctor == null)
                        {
                            throw new InvalidOperationException("这个类必须有一个私有的无参的构造函数!!!  " + typeof(T));
                        }

                        instance = (T)ctor.Invoke(null);
                    }
                }
            }
            return instance;
        }
    }
}
