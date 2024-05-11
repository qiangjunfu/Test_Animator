using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager
{
    private static SceneLoadManager instance;
    public static SceneLoadManager Instance
    {
        get
        {
            if (instance == null)
            {
                var ctor = typeof(SceneLoadManager).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new System.Type[0], null);

                if (ctor == null)
                {
                    throw new NullReferenceException("这个类必须有一个私有的无参的构造函数!!!");
                }

                instance = (SceneLoadManager)ctor.Invoke(null);
            }

            return instance;
        }
    }
    private SceneLoadManager() { }


    private AsyncOperation ao = null;
    private string sceneName = null;
    private float virtualProgress = 0;
    private float realProgress = 0;
    private float loadSpeed = 1;
    private Action<float, float, bool> sceneLoadCallback = null;


    public void StartFunction()
    {

    }

    public void OnDestroyFunction()
    {

    }

    public void UpdateFunction()
    {
        if (ao != null)
        {
            realProgress = ao.progress;

            if (virtualProgress < realProgress)
            {
                virtualProgress += Time.deltaTime * loadSpeed;
            }

            sceneLoadCallback?.Invoke(virtualProgress, realProgress, ao.isDone);


            if (virtualProgress >= 1 & ao.isDone)
            {
                log.LogFormat(" {0} 场景加载完成 !!! ", sceneName);
                ClearData();
            }
        }
    }


    public void LoadAsync(string sceneName, Action<float, float, bool> sceneLoadCallback = null)
    {
        ClearData();

        ao = SceneManager.LoadSceneAsync(sceneName);
        this.sceneName = sceneName;
        this.sceneLoadCallback = sceneLoadCallback;

    }


    public void LoadAsync(string sceneName, float loadSpeed, Action<float, float, bool> sceneLoadCallback = null)
    {
        ClearData();

        ao = SceneManager.LoadSceneAsync(sceneName);
        this.sceneName = sceneName;
        this.sceneLoadCallback = sceneLoadCallback;
        this.loadSpeed = loadSpeed;
    }

    public string GetSceneName()
    {
        Scene scene = SceneManager.GetActiveScene();
        return scene.name;
    }


    void ClearData()
    {
        ao = null;
        sceneName = null;
        virtualProgress = 0;
        realProgress = 0;
        loadSpeed = 0.5f;
        sceneLoadCallback = null;
    }
}
