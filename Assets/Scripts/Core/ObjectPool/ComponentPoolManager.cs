using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentPoolManager : MonoSingleTon<ComponentPoolManager> , IManager
{
    private Dictionary<string, Queue<Component>> poolDictionary = new Dictionary<string, Queue<Component>>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();


    public void Init() { }

    public T GetObject<T>(string prefabName, Vector3 position, Quaternion rotation, int initialPoolSize = 10) where T : Component
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            CreatePool<T>(prefabName, initialPoolSize);
        }
        return ReuseComponent<T>(prefabName, position, rotation);
    }

    public void RecycleObject<T>(T component) where T : Component
    {
        string prefabName = component.gameObject.name.Replace("(Clone)", "").Trim();
        if (poolDictionary.ContainsKey(prefabName))
        {
            component.gameObject.SetActive(false);
            poolDictionary[prefabName].Enqueue(component);
        }
        else
        {
            Debug.LogWarning("No pool available for: " + prefabName);
            Destroy(component.gameObject);
        }
    }


    private void CreatePool<T>(string prefabName, int poolSize) where T : Component
    {
        Queue<Component> newPool = new Queue<Component>();
        GameObject prefab = LoadPrefab(prefabName);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject newObject = Instantiate(prefab);
            newObject.name = prefabName;

            T component = newObject.GetComponent<T>() ?? newObject.AddComponent<T>();
            newObject.SetActive(false);
            newPool.Enqueue(component);
        }

        poolDictionary[prefabName] = newPool;
    }

    private T ReuseComponent<T>(string prefabName, Vector3 position, Quaternion rotation) where T : Component
    {
        //Debug.LogFormat("ReuseComponent<T>(): {0} -- {1}", prefabName, poolDictionary[prefabName].Count);

        if (poolDictionary[prefabName].Count > 0)
        {
            T componentToReuse = poolDictionary[prefabName].Dequeue() as T;  // 取出组件
            componentToReuse.gameObject.SetActive(true);
            componentToReuse.transform.position = position;
            componentToReuse.transform.rotation = rotation;
            // 不要在这里将组件放回队列
            //poolDictionary[prefabName].Enqueue(componentToReuse);
            return componentToReuse;
        }
        else
        {
            return CreateNewComponent<T>(prefabName, position, rotation);
        }
    }


    private T CreateNewComponent<T>(string prefabName, Vector3 position, Quaternion rotation) where T : Component
    {
        Debug.LogFormat("CreateNewComponent<T>()  " + prefabName); 
        GameObject prefab = LoadPrefab(prefabName);
        GameObject newObject = Instantiate(prefab, position, rotation);
        newObject.name = prefabName;

        T newComponent = newObject.GetComponent<T>() ?? newObject.AddComponent<T>();
        newComponent.gameObject.SetActive(true);
        poolDictionary[prefabName].Enqueue(newComponent);
        return newComponent;
    }

    private GameObject LoadPrefab(string prefabName)
    {
        if (!prefabDictionary.ContainsKey(prefabName))
        {
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab == null)
            {
                Debug.LogError("Prefab not found: " + prefabName);
                return null;
            }
            prefabDictionary[prefabName] = prefab;
        }
        return prefabDictionary[prefabName];
    }




}
