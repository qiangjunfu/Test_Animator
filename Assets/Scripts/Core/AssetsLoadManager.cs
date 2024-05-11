using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.Video;



public class AssetsLoadManager : MonoSingleTon<AssetsLoadManager>, IManager
{
    private Dictionary<string, Object> objectCache = new Dictionary<string, Object>();
    private Dictionary<string, Component> componentCache = new Dictionary<string, Component>();
    private Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
    private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
    private Dictionary<string, AudioClip> audioClipCache = new Dictionary<string, AudioClip>();
    private Dictionary<string, VideoClip> videoClipCache = new Dictionary<string, VideoClip>(); // ������Ƶ��Դ



    public void Init() { }



    public T LoadObject<T>(string resourcePath, Transform parent = null) where T : Object
    {
        if (objectCache.TryGetValue(resourcePath, out Object cachedResource) && cachedResource is T)
        {
            // ����Ƿ���Ҫʵ����GameObject
            if (typeof(T) == typeof(GameObject))
            {
                GameObject instance = Instantiate(cachedResource, parent ? parent : null) as GameObject;
                return instance as T;
            }
            return cachedResource as T;
        }
        else
        {
            T resource = Resources.Load<T>(resourcePath);
            if (resource == null)
            {
                Debug.LogError("δ�ҵ���Դ, ��·��:" + resourcePath);
                return null;
            }
            objectCache[resourcePath] = resource;
            // ����Ƿ���Ҫʵ����GameObject
            if (typeof(T) == typeof(GameObject))
            {
                GameObject instance = Instantiate(resource, parent ? parent : null) as GameObject;
                return instance as T;
            }
            return resource;
        }
    }


    public T LoadComponent<T>(string resourcePath, Transform parent = null) where T : Component
    {
        GameObject instance;
        if (componentCache.TryGetValue(resourcePath, out Component cachedComponent))
        {
            instance = Instantiate(cachedComponent.gameObject, parent ? parent : null);
        }
        else
        {
            GameObject prefab = Resources.Load<GameObject>(resourcePath);
            if (prefab == null)
            {
                Debug.LogError("��·�� " + resourcePath + " ��δ�ҵ�Ԥ��");
                return null;
            }
            T component = prefab.GetComponent<T>();
            if (component == null)
            {
                Debug.LogWarning("��·�� " + resourcePath + " �µ�Ԥ����δ�ҵ�����Ϊ " + typeof(T).Name + " �����");
                return null;
            }
            componentCache[resourcePath] = component;
            instance = Instantiate(prefab, parent ? parent : null);
        }
        instance.name = resourcePath; // ����ʵ�������������Ϊ resourcePath
        return instance.GetComponent<T>();
    }







    #region UI
    public T LoadComponentUI<T>(string resourcePath, Transform parent = null) where T : Component
    {
        T t = LoadComponent<T>(resourcePath, parent);
        RectTransform rt = t.gameObject.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        //rt.sizeDelta = Vector2.zero;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.Euler(Vector3.zero);
        rt.localScale = Vector3.one;

        return t;
    }

    public T LoadUIPanel<T>(string uiPathName, Transform parent) where T : Component
    {
        T t = LoadComponent<T>(uiPathName, parent);
        RectTransform rt = t.gameObject.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        rt.sizeDelta = Vector2.zero;
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.Euler(Vector3.zero);
        rt.localScale = Vector3.one;

        return t;
    }

    public T LoadUIToggle<T>(string uiPathName, Transform parent, bool isSetToggleGroup = false) where T : Component
    {
        T t = LoadComponent<T>(uiPathName, parent);

        //����toggle������
        if (isSetToggleGroup)
        {
            UnityEngine.UI.Toggle toggle = t.gameObject.GetComponent<UnityEngine.UI.Toggle>();
            UnityEngine.UI.ToggleGroup toggleGroup = parent.GetComponent<UnityEngine.UI.ToggleGroup>();
            toggle.group = toggleGroup;
        }

        RectTransform rt = t.gameObject.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        //rt.sizeDelta = Vector2.zero;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.Euler(Vector3.zero);
        rt.localScale = Vector3.one;

        return t;
    }


    // ��������
    public Texture2D LoadTexture(string path)
    {
        if (textureCache.TryGetValue(path, out Texture2D cachedTexture))
        {
            return cachedTexture;
        }

        Texture2D texture = Resources.Load<Texture2D>(path);
        if (texture == null)
        {
            Debug.LogError("��·�� " + path + " ��δ�ҵ�����");
            return null;
        }

        textureCache[path] = texture;
        return texture;
    }

    // �������� Sprite
    public Sprite LoadSprite(string path, Rect rect, Vector2 pivot, float pixelsPerUnit = 100.0f)
    {
        if (spriteCache.TryGetValue(path, out Sprite cachedSprite))
        {
            return cachedSprite;
        }

        Texture2D texture = LoadTexture(path);
        if (texture != null)
        {
            Sprite sprite = Sprite.Create(texture, rect, pivot, pixelsPerUnit);
            spriteCache[path] = sprite;
            return sprite;
        }

        return null;
    }

    #region 
    //Texture2D myTexture = AssetsLoadManager.Instance.LoadTexture("MyTexture");
    //Sprite mySprite = AssetsLoadManager.Instance.LoadSprite("MyTexture", new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
    #endregion


    // ֱ�Ӽ��� Sprite
    public Sprite LoadSprite(string path)
    {
        if (spriteCache.TryGetValue(path, out Sprite cachedSprite))
        {
            return cachedSprite;
        }

        Sprite sprite = Resources.Load<Sprite>(path);
        if (sprite == null)
        {
            Debug.LogError("��·�� " + path + " ��δ�ҵ� Sprite");
            return null;
        }

        spriteCache[path] = sprite;
        return sprite;
    }




    #endregion



    public AudioClip LoadAudioClip(string path)
    {
        if (audioClipCache.TryGetValue(path, out AudioClip cachedClip))
        {
            return cachedClip;
        }

        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip == null)
        {
            Debug.LogError("��·�� " + path + " ��δ�ҵ� AudioClip");
            return null;
        }

        audioClipCache[path] = clip;
        return clip;
    }

    public VideoClip LoadVideoClip(string path)
    {
        if (videoClipCache.TryGetValue(path, out VideoClip cachedClip))
        {
            return cachedClip;
        }

        VideoClip clip = Resources.Load<VideoClip>(path);
        if (clip == null)
        {
            Debug.LogError("��·�� " + path + " ��δ�ҵ� VideoClip");
            return null;
        }

        videoClipCache[path] = clip;
        return clip;
    }

}
