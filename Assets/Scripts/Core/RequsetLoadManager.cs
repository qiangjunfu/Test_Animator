using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class RequsetLoadManager : MonoBehaviour
{
    private static bool isInit;
    private static RequsetLoadManager instance;
    public static RequsetLoadManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject(typeof(RequsetLoadManager).ToString());
                go.AddComponent<RequsetLoadManager>();
            }
            return instance;
        }
    }
    protected void Awake()
    {
        if (instance == null && !isInit)
        {
            isInit = true;
            instance = GetComponent<RequsetLoadManager>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnDestroy()
    {
        if (instance != null) instance = null;
    }


    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();
    private Dictionary<string, string> txtDic = new Dictionary<string, string>();
    private Dictionary<string, byte[]> byteDic = new Dictionary<string, byte[]>();
    private Dictionary<string, Texture2D> textureDic = new Dictionary<string, Texture2D>();
    private Dictionary<string, AudioClip> audioClipDic = new Dictionary<string, AudioClip>();
    private Dictionary<string, GameObject> assetDic = new Dictionary<string, GameObject>();


    public void GetRequestByte(string uri, string savePath)
    {
        StartCoroutine(GetRequestTexture(uri, (byte[] bts) =>
        {
            Debug.Log("RequsetLoadManager -> RequestByte() -> 图片下载成功: " + uri);
            BytesToFile(savePath, bts);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }));
    }


    #region 
    private IEnumerator LoadAssetBundle(string uri, Action<AssetBundle> requestCallback = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                AssetBundle ab = DownloadHandlerAssetBundle.GetContent(webRequest);
                requestCallback?.Invoke(ab);
            }
            else
            {
                Debug.LogError("RequsetLoadManager -> LoadAssetBundle() -> :  " + webRequest.error);
            }
        }
    }

    private IEnumerator GetRequestTexture(string uri, Action<byte[]> requestCallback = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) //new UnityWebRequest (uri )
        {
            yield return webRequest.SendWebRequest();


            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                requestCallback?.Invoke(webRequest.downloadHandler.data);
            }
            else
            {
                Debug.LogError("RequsetLoadManager -> GetRequestBytes() -> :  " + webRequest.error);
            }
        }
    }

    public  IEnumerator GetRequestTexture(string uri, Action<Texture2D> requestCallback = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            DownloadHandlerTexture downloadHandlerTexture = new DownloadHandlerTexture(true);
            webRequest.downloadHandler = downloadHandlerTexture;
            yield return webRequest.SendWebRequest();


            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                requestCallback?.Invoke(downloadHandlerTexture.texture);
            }
            else
            {
                Debug.LogError("RequsetLoadManager -> GetRequestTexture() -> :  " + webRequest.error+ "   uri = " + uri);
            }
        }
    }

    private IEnumerator GetRequestAudioClip(string uri, AudioType audioType, Action<AudioClip> requestCallback = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            DownloadHandlerAudioClip downloadAudioClip = new DownloadHandlerAudioClip(uri, audioType);
            webRequest.downloadHandler = downloadAudioClip;
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                requestCallback?.Invoke(downloadAudioClip.audioClip);
            }
            else
            {
                Debug.LogError("RequsetLoadManager -> GetRequestAudioClip() -> :  " + webRequest.error);
            }
        }
    }
    #endregion


    public  IEnumerator GetRequestString(string uri, Action<string> requestCallback = null, Action<string> errorCallback = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) 
        {
            //Debug.LogFormat ("请求地址：" + webRequest.url); 
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                MessageManager.Broadcast<string>(GameEventType.NetworkFaild, uri);
                Debug.LogWarning("RequsetLoadManager -> GetRequestString() -> :  [error" + webRequest.error + "]     [requestCallback:" + (requestCallback==null ? "" : requestCallback.Method.Name) +"]     [URL:"+uri+"]");
                errorCallback?.Invoke(webRequest.error);
            }
            else
            {
                MessageManager.Broadcast<string>(GameEventType.NetworkSuccess, uri);
                requestCallback?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }
    public IEnumerator GetRequestStringWithDelay(string uri,float delayTime, Action<string> requestCallback = null, Action<string> errorCallback = null)
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(GetRequestString(uri, requestCallback, errorCallback));
    }
    public IEnumerator PostRequestString(string targetUri, string dataJson, Action<string> requestCallback = null, Action<string> errorCallback = null)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(targetUri, UnityWebRequest.kHttpVerbPOST))
        {
            UploadHandler uploader = new UploadHandlerRaw(System.Text.Encoding.Default.GetBytes(dataJson));
            webRequest.uploadHandler = uploader;
            //设置HTTP协议的请求头，默认的请求头HTTP服务器无法识别
            webRequest.uploadHandler.contentType = "application/json";
            //这里需要创建新的对象用于存储请求并响应后返回的消息体，否则报空引用的错误
            DownloadHandler downloadHandler = new DownloadHandlerBuffer();
            webRequest.downloadHandler = downloadHandler;

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("RequsetLoadManager -> PostRequestString() -> :  " + webRequest.error);
                MessageManager.Broadcast<string>(GameEventType.WebRequestError, targetUri);
                errorCallback?.Invoke(webRequest.error);
            }
            else
            {
                //Debug.Log("Form upload complete!  " + www.downloadHandler.text);
                requestCallback?.Invoke(webRequest.downloadHandler.text);
            }
        }
    }
    private IEnumerator PutRequest()
    {
        byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
        using (UnityWebRequest www = UnityWebRequest.Put("https://www.my-server.com/upload", myData))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Upload complete!");
            }
        }
    }


    private GameObject LoadAsset(string path)
    {
        if (assetDic.ContainsKey(path))
        {
            return assetDic[path];
        }
        else
        {
            GameObject g = Resources.Load<GameObject>(path);
            if (g == null)
            {
                Debug.LogError("LoatAsset() -> 无法加载资源，路径:" + path);
                return null;
            }
            else
            {
                assetDic.Add(path, g);
                return g;
            }
        }
    }
    private Texture2D LoadSprite(string path)
    {
        if (textureDic.ContainsKey(path))
        {
            return textureDic[path];
        }
        else
        {
            Texture2D texture = Resources.Load<Texture2D>(path);
            if (texture == null)
            {
                Debug.LogError("LoadSprite() -> 无法加载资源，路径:" + path);
                return null;
            }
            else
            {
                textureDic.Add(path, texture);
                return texture;
            }
        }
    }
    private AudioClip LoadAudioClip(string path)
    {
        if (audioClipDic.ContainsKey(path))
        {
            return audioClipDic[path];
        }
        else
        {
            AudioClip audioClip = Resources.Load(path, typeof(AudioClip)) as AudioClip;
            if (audioClip == null)
            {
                Debug.LogError("LoadAudioClip() -> 无法加载资源，路径:" + path);
                return null;
            }
            else
            {
                audioClipDic.Add(path, audioClip);
                return audioClip;
            }
        }
    }


    public static void BytesToFile(string path, byte[] bytes)
    {
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            fs.Write(bytes, 0, bytes.Length);
        }
    }
}