using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolManager : MonoSingleTon<GameObjectPoolManager>, IManager
{
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();



    public void Init() { }

    public GameObject GetObject(string prefabName, Vector3 position, Quaternion rotation, int initialPoolSize = 10)
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            CreatePool(prefabName, initialPoolSize);
        }
        return ReuseGameObject(prefabName, position, rotation);
    }

    public void RecycleObject(GameObject objectToRecycle)
    {
        string prefabName = objectToRecycle.name.Replace("(Clone)", "").Trim();
        if (poolDictionary.ContainsKey(prefabName))
        {
            objectToRecycle.SetActive(false);
            poolDictionary[prefabName].Enqueue(objectToRecycle);
        }
        else
        {
            Debug.LogWarning("No pool available for: " + prefabName);
            Destroy(objectToRecycle);
        }
    }

    private void CreatePool(string prefabName, int poolSize)
    {
        Queue<GameObject> newPool = new Queue<GameObject>();
        GameObject prefab = LoadPrefab(prefabName);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject newObject = Instantiate(prefab);
            newObject.name = prefabName;

            newObject.SetActive(false);
            newPool.Enqueue(newObject);
        }

        poolDictionary[prefabName] = newPool;
    }

    private GameObject ReuseGameObject(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (poolDictionary[prefabName].Count > 0)
        {
            GameObject objectToReuse = poolDictionary[prefabName].Dequeue();
            objectToReuse.SetActive(true);
            objectToReuse.transform.position = position;
            objectToReuse.transform.rotation = rotation;
            //poolDictionary[prefabName].Enqueue(objectToReuse);
            return objectToReuse;
        }
        else
        {
            return CreateNewGameObject(prefabName, position, rotation);
        }
    }

    private GameObject CreateNewGameObject(string prefabName, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = LoadPrefab(prefabName);
        GameObject newObject = Instantiate(prefab, position, rotation);
        newObject.name = prefabName;

        newObject.SetActive(true);
        poolDictionary[prefabName].Enqueue(newObject);
        return newObject;
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
