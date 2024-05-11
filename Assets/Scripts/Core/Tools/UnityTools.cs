using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using Newtonsoft.Json;


public static class UnityTools
{
    #region   Txt

    public static List<string> ReadTxtByStream(string path)
    {
        List<string> info = new List<string>();
        StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8);
        string s;
        while ((s = sr.ReadLine()) != null)
        {
            info.Add(s);
        }
        sr.Dispose();
        sr.Close();

        return info;
    }
    public static string ReadTxtByAllText(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogErrorFormat("路径不存在: {0}", path);
            return "";
        }
        //string str = Resources.Load<TextAsset>(path).text.ToString();
        string str = File.ReadAllText(path, System.Text.Encoding.UTF8);
        return str;
    }

    public static void WriteInTxtByStream(string path, string str)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("路径不存在 创建新路径");
            try
            {
                //File.Create(path).Dispose();
                File.Create(path);
            }
            catch (System.Exception e)
            {
                Debug.Log($"创建文件 {path} 异常: {e}");
            }
        }
        StreamWriter sw;
        FileInfo fileInfo = new FileInfo(path);
        sw = fileInfo.AppendText();  //追加
                                     //sw = fileInfo.CreateText();    //覆盖
        sw.WriteLine(str);
        sw.Flush();
        sw.Close();
        sw.Dispose();
    }
    public static void WriteInTxtByAllLines(string path, string[] s)
    {
        File.WriteAllLines(path, s);
    }


    /// <summary>
    /// 拆分列
    /// </summary>
    public static string[] SplitColumn(string path)
    {
        string[] strs = File.ReadAllLines(path);
        return strs;
    }

    /// <summary>
    /// 拆分行
    /// </summary>
    public static string[] SplitRow(string s)
    {
        string[] strs = System.Text.RegularExpressions.Regex.Split(s, "\\s+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //for (int i = 0; i < strs.Length; i++)
        //{
        //    Debug.Log(i + " " + strs[i]);
        //}
        return strs;
    }

    #endregion

    #region   Json

//    /// <summary>
//    /// 写入json文件
//    /// </summary>
//    public static void WriteJson<T>(T t, string path) where T : new()
//    {
//        string str = JsonConvert.SerializeObject(t);
//        log.LogFormat("{0} 读取信息: {1}", path, str);
//        WriteInTxtByStream(path, str);
//    }


//    /// <summary>
//    /// 读取json文件单个类
//    /// </summary>
//    public static T ReadJsonData<T>(string path) where T : new()
//    {
//        string str = ReadTxtByAllText(path);
//        log.LogFormat("{0} 读取信息: {1}", path, str);
//        T t = JsonConvert.DeserializeObject<T>(str);
//        return t;
//    }

//    /// <summary>
//    /// 读取json文件多个类 到List<T>
//    /// </summary>
//    public static List<T> ReadJsonList<T>(string path) where T : new()
//    {
//        string str = ReadTxtByAllText(path);
//#if UNITY_EDITOR
//        log.LogFormat("{0} 读取信息: {1}", path, str);
//#endif
//        List<T> t = JsonConvert.DeserializeObject<List<T>>(str);
//        return t;
//    }



//    /// <summary>
//    /// 保存List<T>数据到json文件
//    /// </summary>
//    public static void SaveListToJson<T>(List<T> list, string savePath) where T : new()
//    {
//        //找到当前路径
//        FileInfo fileInfo = new FileInfo(savePath);
//        //判断有没有文件，有则打开，没有则创建后打开
//        StreamWriter sw = fileInfo.CreateText();

//        for (int i = 0; i < list.Count; i++)
//        {
//            string json = "[";
//            string json2 = JsonConvert.SerializeObject(list[i]);

//            if (list.Count == 1)
//            {
//                json = "[" + json2 + "]";
//                json2 = json;
//            }
//            else
//            {
//                if (i == 0)
//                {
//                    json = "[" + json2 + ",";
//                    json2 = json;
//                }
//                else if (i < list.Count - 1)
//                {
//                    json2 += ",";
//                }
//                else
//                {
//                    json2 += "]";
//                }
//            }

//            //将转换好的字符串保存到文件
//            sw.WriteLine(json2);
//        }

//        Debug.LogFormat("SaveDataToJson() -> 保存Json数据: {0}", savePath);

//        //释放资源
//        sw.Close();
//        sw.Dispose();

//#if UNITY_EDITOR
//        UnityEditor.AssetDatabase.Refresh();
//#endif 
//    }

//    /// <summary>
//    /// 读取json文件到List<T>
//    /// </summary>
//    public static List<T> ReadJsonToList<T>(string path) where T : new()
//    {
//        List<T> list = null;
//        if (!File.Exists(path))
//        {
//            Debug.LogErrorFormat("路径不存在: {0}", path);
//        }
//        //string str = Resources.Load<TextAsset>(path).text.ToString();
//        string str = File.ReadAllText(path, System.Text.Encoding.UTF8);
//        Debug.LogFormat("{0} 读取信息: {1}", path, str);
//        list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(str);
//        return list;
//    }
    #endregion

    #region   array
    ///<summary>
    ///删除数组元素
    ///</summary> 
    public static T[] DeleteArray<T>(T[] array, int index, int deleteLenght = 1)
    {
        if (deleteLenght < 0) return array;

        if (index == 0 && deleteLenght >= array.Length)
        {
            deleteLenght = array.Length;
        }
        else if ((index + deleteLenght) >= array.Length)
        {
            deleteLenght = array.Length - index - 1;
        }

        T[] tempArray = new T[array.Length - deleteLenght];

        for (int i = 0; i < tempArray.Length; i++)
        {
            if (i >= index)
            {
                tempArray[i] = array[i + deleteLenght];
            }
            else
            {
                tempArray[i] = array[i];
            }
        }
        return tempArray;
    }


    /// <summary>
    /// 插入数组元素
    /// </summary>
    public static T[] InsertArray<T>(T[] array, T value, int index)
    {
        if (index > array.Length)
        {
            Debug.LogError($"index{index} > array.Length{array.Length}");
            return array;
        }
        List<T> list = new List<T>(array);
        list.Insert(index, value);
        return list.ToArray();
    }


    public static List<int> GetIntArray(int num)
    {
        List<int> listOfInts = new List<int>();
        while (num > 0)
        {
            listOfInts.Add(num % 10);
            num = num / 10;
        }
        listOfInts.Reverse();
        return listOfInts;
    }

    #endregion

    #region  UGUI

    /// <summary>
    /// 世界坐标转屏幕坐标
    /// </summary>
    public static Vector2 WorldToScreenPoint(Camera cam, Vector3 worldPoint)
    {
        return RectTransformUtility.WorldToScreenPoint(cam, worldPoint);
    }

    /// <summary>
    /// 屏幕坐标转世界坐标
    /// </summary>
    public static bool ScreenPointToWorldPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
    {
        return RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPoint, cam, out worldPoint);
    }



    /// <summary>
    /// 屏幕坐标转某个RectTransform下的localPosition坐标
    /// </summary>
    public static bool ScreenPointToLocalPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector2 localPoint)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, cam, out localPoint);
    }



    /// <summary>
    /// 获取UI组件在屏幕的坐标
    /// </summary>
    public static Vector2 GetUIInScreenPos(RectTransform rt, Camera uiCamera)
    {
        return RectTransformUtility.WorldToScreenPoint(uiCamera, rt.position);
    }

    /// <summary>
    /// 获取鼠标下T类型组件(仅最上层UI)
    /// </summary>
    public static T GetMouseUIComponent<T>(UnityEngine.UI.GraphicRaycaster canvasGraphic, PointerEventData eventData)
    {
        List<RaycastResult> list = new List<RaycastResult>();
        canvasGraphic.Raycast(eventData, list);
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T t = list[i].gameObject.GetComponent<T>();
                if (t != null)
                {
                    Debug.LogFormat("鼠标下检测到的UI: {0}  {1} ", i, list[i].gameObject);
                    return t;
                }
            }
        }
        return default(T);
    }
    /// <summary>
    /// 获取鼠标下T类型组件(仅最上层UI)
    /// </summary>
    public static T GetMouseUIComponent<T>(UnityEngine.UI.GraphicRaycaster canvasGraphic, EventSystem eventSystem)
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list = new List<RaycastResult>();
        canvasGraphic.Raycast(eventData, list);
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T t = list[i].gameObject.GetComponent<T>();
                if (t != null)
                {
                    Debug.LogFormat("鼠标下检测到的UI: {0}  {1} ", i, list[i].gameObject);
                    return t;
                }
            }
        }
        return default(T);
    }


    /// <summary>
    /// 鼠标是否在UI上
    /// </summary>
    public static GameObject IsOverUI(GraphicRaycaster canvasGraphic)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        canvasGraphic.Raycast(eventData, results);
        if (results.Count != 0)
        {
            return results[0].gameObject;
        }

        return null;
    }

    /// <summary>
    /// 鼠标是否在UI上
    /// </summary>
    public static GameObject IsOverUI(GraphicRaycaster canvasGraphic, Vector2 screenPoint)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = screenPoint;
        eventData.position = screenPoint;

        List<RaycastResult> results = new List<RaycastResult>();
        canvasGraphic.Raycast(eventData, results);
        if (results.Count != 0)
        {
            return results[0].gameObject;
        }

        return null;
    }
    //主相机与目标模型之间的夹角               最大夹角限制
    static float floatRectTransformAngle = 0f, floatRectTransformLimitedAngle = 60f;
    /// <summary>
    /// 更新世界物体的屏幕UI位置
    /// </summary>
    public static void UpdateUIWorldPos(RectTransform canvasRect, RectTransform iconRect, Transform worldTrans, Vector2 offset, bool isShowingState = true)
    {
        //检查夹角
        floatRectTransformAngle = Vector3.Angle(Camera.main.transform.forward, (worldTrans.position - Camera.main.transform.position));
        if (floatRectTransformAngle > floatRectTransformLimitedAngle)
        {
            //不在可视夹角内，隐藏UI
            if (iconRect.gameObject.activeSelf)
                iconRect.gameObject.SetActive(false);
        }
        else
        {
            //如果模型处于显示状态【PS：卡片有时需要延迟显示，所以在可视夹角内，不能直接强制显示卡片，要检查先其显示状态】
            if (isShowingState)
            {
                //在可视夹角内，显示UI，并其更新位置
                if (!iconRect.gameObject.activeSelf)
                    iconRect.gameObject.SetActive(true);
            }
            else
            {
                //如果是强制隐藏状态，则不再处理夹角逻辑，直接隐藏
                if (iconRect.gameObject.activeSelf)
                    iconRect.gameObject.SetActive(false);
                return;
            }
            Vector2 screenPos = Camera.main.WorldToScreenPoint(worldTrans.position);
            Vector2 screenPos2 = screenPos - (new Vector2(Screen.width, Screen.height) / 2);

            float ratioX = canvasRect.rect.size.x / Screen.width;
            float ratioY = canvasRect.rect.size.y / Screen.height;
            Vector2 screenPos3 = new Vector2(screenPos2.x * ratioX, screenPos2.y * ratioY);

            //RectTransform rect2 = iconRect.GetComponent<RectTransform>();
            //rect2.anchoredPosition3D = screenPos3 + offset;
            iconRect.anchoredPosition3D = screenPos3 + offset;
        }
    }

    /// <summary>
    /// 更新世界物体的屏幕UI位置
    /// </summary>
    public static void UpdateUIWorldPos(RectTransform canvasRect, RectTransform iconRect, Vector3 pos, Vector2 offset)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
        Vector2 screenPos2 = screenPos - (new Vector2(Screen.width, Screen.height) / 2);

        float ratioX = canvasRect.rect.size.x / Screen.width;
        float ratioY = canvasRect.rect.size.y / Screen.height;
        Vector2 screenPos3 = new Vector2(screenPos2.x * ratioX, screenPos2.y * ratioY);

        iconRect.anchoredPosition3D = screenPos3 + offset;
    }
    #endregion

    #region   RaycastHit 
    ///// <summary>
    ///// 获取鼠标下的3d物体T类型组件
    ///// </summary>
    //public static T GetObjComponentByRay<T>(Camera camera, int rayDis)
    //{
    //    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    bool isHit = Physics.Raycast(ray, out hit, rayDis);
    //    if (isHit)
    //    {
    //        T t = hit.collider.gameObject.GetComponent<T>();
    //        if (t != null)
    //        {
    //            return t;
    //        }
    //    }
    //    return default(T);
    //}

    ///// <summary>
    ///// 获取鼠标下的3d物体T类型组件
    ///// </summary>
    //public static T GetObjComponentByRay<T>(Camera camera, int rayDis, string layerName)
    //{
    //    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    bool isHit = Physics.Raycast(ray, out hit, rayDis, 1 << LayerMask.NameToLayer(layerName));
    //    if (isHit)
    //    {
    //        T t = hit.collider.gameObject.GetComponent<T>();
    //        Debug.LogFormat("射线检测物体: " + hit.collider.gameObject.name);
    //        if (t != null)
    //        {
    //            return t;
    //        }
    //    }
    //    return default(T);
    //}

    /// <summary>
    /// 获取鼠标下的3d物体T类型组件
    /// </summary>
    public static T GetObjComponentByRay<T>(Camera camera, Vector3 screenPoint, int rayDis, string layerName)
    {
        Ray ray = camera.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        bool isHit = Physics.Raycast(ray, out hit, rayDis, 1 << LayerMask.NameToLayer(layerName));
        if (isHit)
        {
            T t = hit.collider.gameObject.GetComponent<T>();
            //Debug.LogFormat("射线检测物体: " + hit.collider.gameObject.name);
            if (t != null)
            {
                return t;
            }
        }
        return default(T);
    }


    /// <summary>
    /// 获取射线检测到的游戏物体
    /// </summary>
    public static GameObject GetGameObjectByRay(Camera camera, Vector3 screenPoint, float rayDis = 100, string layerName = "")
    {
        GameObject go = null;
        RaycastHit raycastHit;
        Ray ray = camera.ScreenPointToRay(screenPoint);
        bool isHit = false;
        if (layerName == "")
            isHit = Physics.Raycast(ray, out raycastHit, rayDis);
        else
            isHit = Physics.Raycast(ray, out raycastHit, rayDis, 1 << LayerMask.NameToLayer(layerName));
        if (isHit)
        {
            go = raycastHit.collider.gameObject;
        }
        return go;
    }

    /// <summary>
    /// 获取射线检测到的世界坐标点 
    /// </summary>
    public static Vector3 GetWorldPointByRay(Camera camera, Vector3 screenPoint, float rayDis = 100, string layerName = "")
    {
        RaycastHit raycastHit;
        Ray ray = camera.ScreenPointToRay(screenPoint);
        bool isHit = false;
        if (layerName == "")
            isHit = Physics.Raycast(ray, out raycastHit, rayDis);
        else
            isHit = Physics.Raycast(ray, out raycastHit, rayDis, 1 << LayerMask.NameToLayer(layerName));

        if (isHit)
        {
            return raycastHit.point;
        }
        return new Vector3(0, 0, 0);
    }

    public static Vector3 GetPointByRay(Vector3 startPos, float rayDis, string layerName = "")
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(startPos, Vector3.down);
        bool isHit = false;
        if (layerName == "")
            isHit = Physics.Raycast(ray, out raycastHit, rayDis);
        else
            isHit = Physics.Raycast(ray, out raycastHit, rayDis, 1 << LayerMask.NameToLayer(layerName));

        if (isHit)
        {
            return raycastHit.point;
        }
        return new Vector3(0, 0, 0);
    }
    #endregion


    #region    Bytes

    public static void BytesToFile(string path, byte[] bytes)
    {
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            fs.Write(bytes, 0, bytes.Length);
        }
    }

    public static byte[] FileToBytes(string path)
    {
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
        {
            byte[] buffur = new byte[fs.Length];
            fs.Read(buffur, 0, buffur.Length);
            return buffur;
        }
    }

    public static void CopyFile(string path1, string path2)
    {
        if (File.Exists(path1))
        {
            File.Copy(path1, path2, true);
        }
    }

    public static void CopyDir(string srcPath, string aimPath)
    {
        try
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
            {
                aimPath += Path.DirectorySeparatorChar;
                //aimPath += "/";
            }
            // 判断目标目录是否存在如果不存在则新建
            if (!Directory.Exists(aimPath))
            {
                Directory.CreateDirectory(aimPath);
            }
            if (!Directory.Exists(srcPath))
            {
                Debug.LogError("srcPath no exists - " + srcPath);
                return;
            }
            string[] fileList = Directory.GetFileSystemEntries(srcPath);
            // 遍历所有的文件和目录
            foreach (string file in fileList)
            {
                if (Directory.Exists(file))
                {
                    CopyDir(file, aimPath + Path.GetFileName(file));
                }
                else
                {
                    File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("copydir - " + ex.Message);
        }
    }

    public static byte[] Bytes(byte[] buffer)
    {
        int len = buffer.Length;
        byte[] dst_buffer = new byte[len];
        for (int i = 0; i < len; i++)
        {
            byte t = buffer[i];
            dst_buffer[i] = (byte)(t ^ 0x99);
        }
        return dst_buffer;
    }

    public static byte[] Utf8ToByte(object utf8)
    {
        return System.Text.Encoding.UTF8.GetBytes((string)utf8);
    }

    public static string ByteToUtf8(byte[] bytes)
    {
        return System.Text.Encoding.UTF8.GetString(bytes);
    }

    public static string BytesToString(byte[] bytes)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        return encoding.GetString(bytes);
    }

    public static byte[] StringToBytes(string str)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        return encoding.GetBytes(str);
    }
    public static string Md5(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            string sb = "";
            for (int i = 0; i < retVal.Length; i++)
            {
                sb += retVal[i].ToString("x2");
            }
            return sb;
        }
        catch (System.Exception ex)
        {
            throw new System.Exception("md5 fail error=" + ex.Message);
        }
    }

    public static string Md5String(string value)
    {
        byte[] input = System.Text.Encoding.UTF8.GetBytes(value);
        byte[] hash = System.Security.Cryptography.MD5.Create().ComputeHash(input);
        string sb = "";
        int length = hash.Length;
        for (int i = 0; i < length; ++i)
        {
            sb += hash[i].ToString("x2");
        }
        return sb;
    }

    #endregion



    #region     File
    /// <summary>
    /// 获取当前路径下指定文件
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="suffix">后缀格式 (bmp txt)</param>
    /// <param name="fileNameList">文件名存放列表</param>
    /// <param name="isSubcatalog">是否编辑子目录</param>
    public static void GetFiles(string path, string suffix, ref List<string> fileNameList, bool isSubcatalog = false)
    {
        string filename;
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] file = dir.GetFiles();
        //如需遍历子文件夹时需要使用  
        DirectoryInfo[] dii = dir.GetDirectories();

        foreach (FileInfo f in file)
        {
            filename = f.FullName;//拿到了文件的完整路径
            if (filename.EndsWith(suffix))//判断文件后缀，并获取指定格式的文件全路径增添至fileList  
            {
                fileNameList.Add(filename);
                Debug.Log(filename);
            }
        }
        //获取子文件夹内的文件列表，递归遍历
        if (isSubcatalog)
        {
            foreach (DirectoryInfo d in dii)
            {
                GetFiles(d.FullName, "", ref fileNameList, false);
            }
        }

        return;
    }

    #endregion

    #region   获取子物体

    public static List<GameObject> GetChildObject( GameObject obj) 
    {
        List<GameObject> tempArrayobj = new List<GameObject>();
        foreach (Transform child in obj.transform)
        {
            tempArrayobj.Add(child.gameObject);
        }
        return tempArrayobj;
    }
    public static List<GameObject> GetAllChildrenGameObject(this GameObject go, bool includeInactive = false)
    {
        List<GameObject> listObj = new List<GameObject>();
        AddChildrenRecursively(go.transform, listObj, includeInactive);
        return listObj;
    }
    private static void AddChildrenRecursively(Transform parent, List<GameObject> listObj, bool includeInactive)
    {
        foreach (Transform child in parent)
        {
            if (includeInactive || child.gameObject.activeSelf)
            {
                listObj.Add(child.gameObject);
                AddChildrenRecursively(child, listObj, includeInactive);
            }
        }
    }

    public static List<T> GetAllChildrenComponents<T>(this GameObject go, bool includeInactive = false) where T : Component
    {
        List<T> TList = go.GetComponentsInChildren<T>(includeInactive).ToList(); 
        List<T> TListReal = new List<T>();
        for (int i = 0; i < TList.Count; i++)
        {
            TListReal.Add(TList[i]);
        }
        return TListReal;
    }
    public static List<T> GetChildrenComponents<T>(this GameObject go, bool includeInactive = false) where T : Component
    {
        List<T> TList = go.GetComponentsInChildren<T>(includeInactive).ToList();
        List<T> TListReal = new List<T>();
        for (int i = 0; i < TList.Count; i++)
        {
            if (TList[i].transform.parent == go.transform)
            {
                TListReal.Add(TList[i]);
            }
        }
        return TListReal;
    }

    public static T[] GetComponentsInRealChildren<T>(this GameObject go, bool includeInactive = false) where T : Component
    {
        List<T> TList = go.GetComponentsInChildren<T>(includeInactive).ToList();
        List<T> TListReal = new List<T>();
        for (int i = 0; i < TList.Count; i++)
        {
            if (TList[i].transform.parent == go.transform)
            {
                TListReal.Add(TList[i]);
            }
        }
        return TListReal.ToArray();
    }


    #endregion

    #region  Delay Time
    public static IEnumerator DelayCoroutine(float delayTime, System.Action delayCallback)
    {
        yield return new WaitForSeconds(delayTime);

        delayCallback?.Invoke();
    }

    public static void DelayDoTween(float delayTime, System.Action callback)
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delayTime);
        seq.AppendCallback(() =>
        {
            callback?.Invoke();
        });
        seq.SetAutoKill(true);
        //设为true时可在Time.timeScale=0的情况下正常执行
        seq.SetUpdate(true);
    }

    public static async Task TaskDelayAsync(float delayTime, System.Action callback = null)
    {
        Task t1 = new Task(() =>
        {
            int time = (int)(delayTime * 1000);
            Thread.Sleep(time);
            Debug.Log("延迟  线程id: " + Thread.CurrentThread.ManagedThreadId);
        });
        t1.Start();
        await t1;

        Debug.LogFormat("延迟:{0} 完成,  线程id: {1}", delayTime, Thread.CurrentThread.ManagedThreadId);
        if (callback != null)
        {
            callback();
        }
    }
    //public static async Task TaskDelayAsync(float delayTime, System.Action callback = null )
    //{
    //    StopwatchStart();
    //    int time = (int)(delayTime * 1000);
    //    await Task.Delay(time);
    //    //await Task.Run(() =>
    //    //{
    //    //    Thread.Sleep(time);
    //    //    //Debug.Log("延迟  线程id: " + Thread.CurrentThread.ManagedThreadId);
    //    //});
    //    //Debug.LogFormat("延迟:{0}  线程id: {1}", delayTime, Thread.CurrentThread.ManagedThreadId);
    //    StopwatchEnd(delayTime.ToString());
    //    if (callback != null)
    //    {
    //        callback();
    //    }
    //}

    #endregion


    #region   天气查询
    static void GetProvince()
    {
        //string url = "http://www.nmc.cn/rest/province";
        //StartCoroutine(RequsetLoadManager.Instance.GetRequest(url, (string str) =>
        //{
        //    Debug.Log("查询省份 : " + str);
        //}));
    }

    static void GetCity()
    {
        //string url2 = "http://www.nmc.cn/rest/province/AHE";
        //StartCoroutine(RequsetLoadManager.Instance.GetRequest(url2, (string str) =>
        //{
        //    Debug.Log("城市代码查询: " + str);
        //}));
    }

    static void GetWeather()
    {
        //string url3 = "http://www.nmc.cn/rest/weather?stationid=54602";
        //StartCoroutine(RequsetLoadManager.Instance.GetRequest(url3, (string str) =>
        //{
        //    Debug.Log("查询城市天气: " + str);
        //}));
    }

    #endregion

    #region   数组到vertor3

    public static Vector3 ArrayToVector3(string[] array)
    {
        Vector3 v3 = Vector3.zero;
        if (array.Length < 3)
        {
            Debug.LogError(" array .Length < 3   :   " + array.Length);
            return v3;
        }
        float x = float.Parse(array[0]);
        float y = float.Parse(array[1]);
        float z = float.Parse(array[2]);
        v3 = ArrayToVector3(new float[] { x, y, z });

        return v3;
    }
    public static Vector3 ArrayToVector3(float[] array)
    {
        Vector3 v3 = Vector3.zero;
        if (array.Length < 3)
        {
            Debug.LogError(" array .Length < 3   :   " + array.Length);
            return v3;
        }

        v3 = new Vector3(array[0], array[1], array[2]);
        return v3;
    }

    public static float[] Vector3ToArray(Vector3 v3)
    {
        float[] array = new float[3];
        array[0] = v3.x;
        array[1] = v3.y;
        array[2] = v3.z;
        return array;
    }
    #endregion


    #region   List

    public static List<T> CopyList<T>(List<T> originalList)
    {
        List<T> copyList = new List<T>();
        originalList.ForEach(i => copyList.Add(i));
        return copyList;
    }

    /// <summary>
    /// 随机打乱数组
    /// </summary>
    public static T[] RandomSortArray<T>(T[] arr)
    {
        System.Random r = new System.Random();
        for (int i = 0; i < arr.Length; i++)
        {
            int index = r.Next(arr.Length);
            Debug.LogFormat("index = {0}", index);
            T temp = arr[i];  //当前元素和随机元素交换位置
            arr[i] = arr[index];
            arr[index] = temp;
        }
        return arr;
    }

    /// <summary>
    /// 随机打乱List 
    /// </summary>
    public static List<T> RandomSortList<T>(List<T> list)
    {
        System.Random random = new System.Random();
        var newList = new List<T>();
        foreach (var item in list)
        {
            newList.Insert(random.Next(newList.Count), item);
        }
        return newList;
    }


    #endregion

    #region   是否在相机视角内

    public static bool IsInCameraView(Camera camera, Transform target)
    {
        //转化为视角坐标
        Vector3 viewPos = camera.WorldToViewportPoint(target.position);

        // z<0代表在相机背后
        if (viewPos.z < 0) return false;

        //太远了！看不到了！
        if (viewPos.z > camera.farClipPlane) return false;

        // x,y取值在 0~1之外时代表在视角范围外；
        if (viewPos.x < 0 || viewPos.y < 0 || viewPos.x > 1 || viewPos.y > 1) return false;

        return true;
    }

    #endregion

    #region   字典
    public static void DicRemove()
    {

    }
    #endregion


    #region  Url中文转码
    ///// <summary>
    ///// 获取设备列表url (运维总览 安防)
    ///// </summary>
    //public static string GetDeviceUrlStr(string url, string deviceModule = "1", string buildName = "6#商业裙房工程", string floorName = "3层")
    //{
    //    string ip = "";
    //    string buildId = ConstAPI.UrlEncode(buildName);
    //    string floor = ConstAPI.UrlEncode(floorName);

    //    if (string.IsNullOrEmpty(deviceModule))
    //    {
    //        ip = url + "&building=" + buildId + "&floor=" + floor;
    //    }
    //    else
    //    {
    //        ip = url + "&building=" + buildId + "&floor=" + floor + "&deviceModule=" + deviceModule;
    //    }
    //    //api = "http://ai.landingol.com:8187/princeps/device/queryPage?pageNo=1&pageSize=99999&floor=3层&building=6%23商业裙房工程&deviceModule=1";
    //    return ip;
    //}
    #endregion

    #region    
    /// <summary>
    /// 获取限制长度的字符串
    /// </summary>
    public static string GetLimitString(string content, int limitLenght)
    {
        if (string.IsNullOrEmpty(content)) return content;

        string str = "";
        int lenght = content.Length;
        if (lenght > limitLenght)
        {
            str = content.Substring(0, limitLenght);
        }
        else
        {
            str = content;
        }

        return str;
    }
    #endregion

    #region   角度
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }

        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }

    public static Vector3 GetInspectorRotationValue(Transform transform)
    {
        System.Type transformType = transform.GetType();
        System.Reflection.PropertyInfo m_propertyInfo_rotationOrder = transformType.GetProperty("rotationOrder", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        object m_OldRotationOrder = m_propertyInfo_rotationOrder.GetValue(transform, null);
        System.Reflection.MethodInfo m_methodInfo_GetLocalEulerAngles = transformType.GetMethod("GetLocalEulerAngles", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        object value = m_methodInfo_GetLocalEulerAngles.Invoke(transform, new object[] { m_OldRotationOrder });
        string temp = value.ToString();
        temp = temp.Remove(0, 1);
        temp = temp.Remove(temp.Length - 1, 1);
        string[] tempVector3;
        tempVector3 = temp.Split(',');
        Vector3 vector3 = new Vector3(float.Parse(tempVector3[0]), float.Parse(tempVector3[1]), float.Parse(tempVector3[2]));
        return vector3;

    }

    #endregion

    #region   操作耗时计算
    static System.Diagnostics.Stopwatch stopwatch = null;
    static string msg = "";
    public static void StopwatchStart(string _msg = "")
    {
        if (stopwatch == null)
        {
            stopwatch = new System.Diagnostics.Stopwatch();
        }
        msg = _msg;
        Debug.LogFormat("UnityTools -> StopwatchStart(): {0}", msg);
        stopwatch.Reset();
        stopwatch.Start();
    }

    public static long StopwatchEnd()
    {
        if (stopwatch == null)
        {
            Debug.LogErrorFormat("UnityTools -> StopwatchEnd()  stopwatch == null ");
            return 0;
        }

        stopwatch.Stop();
        long Milliseconds = stopwatch.ElapsedMilliseconds;
        Debug.LogFormat("UnityTools -> StopwatchEnd()  {0}用时: {1} ms", msg, Milliseconds);
        return Milliseconds;
    }


    public static string SecondToTime(float time)
    {
        //秒数取整
        int seconds = (int)time;
        //一小时为3600秒 秒数对3600取整即为小时
        int hour = seconds / 3600;
        //一分钟为60秒 秒数对3600取余再对60取整即为分钟
        int minute = seconds % 3600 / 60;
        //对3600取余再对60取余即为秒数
        seconds = seconds % 3600 % 60;
        //返回00:00:00时间格式
        return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, seconds);
    }
    #endregion
}
