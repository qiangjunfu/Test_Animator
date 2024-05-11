using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonoSingleTon<T> : MonoBehaviour where T : MonoSingleTon<T>
{
    private static readonly object Lock = new object();
    private static T instance;
    public static T Instance
    {
        get
        {
            lock (Lock)
            {
                if (instance == null)
                {
                    // 尝试在场景中查找已存在的实例
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        instance = go.AddComponent<T>();
                        DontDestroyOnLoad(go);
                    }
                }
                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);

            OnAwake();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    protected virtual void OnAwake()
    {

    }
}
