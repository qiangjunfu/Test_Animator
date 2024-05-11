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
            Debug.LogErrorFormat("·��������: {0}", path);
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
            Debug.LogError("·�������� ������·��");
            try
            {
                //File.Create(path).Dispose();
                File.Create(path);
            }
            catch (System.Exception e)
            {
                Debug.Log($"�����ļ� {path} �쳣: {e}");
            }
        }
        StreamWriter sw;
        FileInfo fileInfo = new FileInfo(path);
        sw = fileInfo.AppendText();  //׷��
                                     //sw = fileInfo.CreateText();    //����
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
    /// �����
    /// </summary>
    public static string[] SplitColumn(string path)
    {
        string[] strs = File.ReadAllLines(path);
        return strs;
    }

    /// <summary>
    /// �����
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
//    /// д��json�ļ�
//    /// </summary>
//    public static void WriteJson<T>(T t, string path) where T : new()
//    {
//        string str = JsonConvert.SerializeObject(t);
//        log.LogFormat("{0} ��ȡ��Ϣ: {1}", path, str);
//        WriteInTxtByStream(path, str);
//    }


//    /// <summary>
//    /// ��ȡjson�ļ�������
//    /// </summary>
//    public static T ReadJsonData<T>(string path) where T : new()
//    {
//        string str = ReadTxtByAllText(path);
//        log.LogFormat("{0} ��ȡ��Ϣ: {1}", path, str);
//        T t = JsonConvert.DeserializeObject<T>(str);
//        return t;
//    }

//    /// <summary>
//    /// ��ȡjson�ļ������ ��List<T>
//    /// </summary>
//    public static List<T> ReadJsonList<T>(string path) where T : new()
//    {
//        string str = ReadTxtByAllText(path);
//#if UNITY_EDITOR
//        log.LogFormat("{0} ��ȡ��Ϣ: {1}", path, str);
//#endif
//        List<T> t = JsonConvert.DeserializeObject<List<T>>(str);
//        return t;
//    }



//    /// <summary>
//    /// ����List<T>���ݵ�json�ļ�
//    /// </summary>
//    public static void SaveListToJson<T>(List<T> list, string savePath) where T : new()
//    {
//        //�ҵ���ǰ·��
//        FileInfo fileInfo = new FileInfo(savePath);
//        //�ж���û���ļ�������򿪣�û���򴴽����
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

//            //��ת���õ��ַ������浽�ļ�
//            sw.WriteLine(json2);
//        }

//        Debug.LogFormat("SaveDataToJson() -> ����Json����: {0}", savePath);

//        //�ͷ���Դ
//        sw.Close();
//        sw.Dispose();

//#if UNITY_EDITOR
//        UnityEditor.AssetDatabase.Refresh();
//#endif 
//    }

//    /// <summary>
//    /// ��ȡjson�ļ���List<T>
//    /// </summary>
//    public static List<T> ReadJsonToList<T>(string path) where T : new()
//    {
//        List<T> list = null;
//        if (!File.Exists(path))
//        {
//            Debug.LogErrorFormat("·��������: {0}", path);
//        }
//        //string str = Resources.Load<TextAsset>(path).text.ToString();
//        string str = File.ReadAllText(path, System.Text.Encoding.UTF8);
//        Debug.LogFormat("{0} ��ȡ��Ϣ: {1}", path, str);
//        list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(str);
//        return list;
//    }
    #endregion

    #region   array
    ///<summary>
    ///ɾ������Ԫ��
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
    /// ��������Ԫ��
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
    /// ��������ת��Ļ����
    /// </summary>
    public static Vector2 WorldToScreenPoint(Camera cam, Vector3 worldPoint)
    {
        return RectTransformUtility.WorldToScreenPoint(cam, worldPoint);
    }

    /// <summary>
    /// ��Ļ����ת��������
    /// </summary>
    public static bool ScreenPointToWorldPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
    {
        return RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPoint, cam, out worldPoint);
    }



    /// <summary>
    /// ��Ļ����תĳ��RectTransform�µ�localPosition����
    /// </summary>
    public static bool ScreenPointToLocalPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector2 localPoint)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, cam, out localPoint);
    }



    /// <summary>
    /// ��ȡUI�������Ļ������
    /// </summary>
    public static Vector2 GetUIInScreenPos(RectTransform rt, Camera uiCamera)
    {
        return RectTransformUtility.WorldToScreenPoint(uiCamera, rt.position);
    }

    /// <summary>
    /// ��ȡ�����T�������(�����ϲ�UI)
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
                    Debug.LogFormat("����¼�⵽��UI: {0}  {1} ", i, list[i].gameObject);
                    return t;
                }
            }
        }
        return default(T);
    }
    /// <summary>
    /// ��ȡ�����T�������(�����ϲ�UI)
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
                    Debug.LogFormat("����¼�⵽��UI: {0}  {1} ", i, list[i].gameObject);
                    return t;
                }
            }
        }
        return default(T);
    }


    /// <summary>
    /// ����Ƿ���UI��
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
    /// ����Ƿ���UI��
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
    //�������Ŀ��ģ��֮��ļн�               ���н�����
    static float floatRectTransformAngle = 0f, floatRectTransformLimitedAngle = 60f;
    /// <summary>
    /// ���������������ĻUIλ��
    /// </summary>
    public static void UpdateUIWorldPos(RectTransform canvasRect, RectTransform iconRect, Transform worldTrans, Vector2 offset, bool isShowingState = true)
    {
        //���н�
        floatRectTransformAngle = Vector3.Angle(Camera.main.transform.forward, (worldTrans.position - Camera.main.transform.position));
        if (floatRectTransformAngle > floatRectTransformLimitedAngle)
        {
            //���ڿ��Ӽн��ڣ�����UI
            if (iconRect.gameObject.activeSelf)
                iconRect.gameObject.SetActive(false);
        }
        else
        {
            //���ģ�ʹ�����ʾ״̬��PS����Ƭ��ʱ��Ҫ�ӳ���ʾ�������ڿ��Ӽн��ڣ�����ֱ��ǿ����ʾ��Ƭ��Ҫ���������ʾ״̬��
            if (isShowingState)
            {
                //�ڿ��Ӽн��ڣ���ʾUI���������λ��
                if (!iconRect.gameObject.activeSelf)
                    iconRect.gameObject.SetActive(true);
            }
            else
            {
                //�����ǿ������״̬�����ٴ���н��߼���ֱ������
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
    /// ���������������ĻUIλ��
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
    ///// ��ȡ����µ�3d����T�������
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
    ///// ��ȡ����µ�3d����T�������
    ///// </summary>
    //public static T GetObjComponentByRay<T>(Camera camera, int rayDis, string layerName)
    //{
    //    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    bool isHit = Physics.Raycast(ray, out hit, rayDis, 1 << LayerMask.NameToLayer(layerName));
    //    if (isHit)
    //    {
    //        T t = hit.collider.gameObject.GetComponent<T>();
    //        Debug.LogFormat("���߼������: " + hit.collider.gameObject.name);
    //        if (t != null)
    //        {
    //            return t;
    //        }
    //    }
    //    return default(T);
    //}

    /// <summary>
    /// ��ȡ����µ�3d����T�������
    /// </summary>
    public static T GetObjComponentByRay<T>(Camera camera, Vector3 screenPoint, int rayDis, string layerName)
    {
        Ray ray = camera.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        bool isHit = Physics.Raycast(ray, out hit, rayDis, 1 << LayerMask.NameToLayer(layerName));
        if (isHit)
        {
            T t = hit.collider.gameObject.GetComponent<T>();
            //Debug.LogFormat("���߼������: " + hit.collider.gameObject.name);
            if (t != null)
            {
                return t;
            }
        }
        return default(T);
    }


    /// <summary>
    /// ��ȡ���߼�⵽����Ϸ����
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
    /// ��ȡ���߼�⵽����������� 
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
            // ���Ŀ��Ŀ¼�Ƿ���Ŀ¼�ָ��ַ�����������������
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
            {
                aimPath += Path.DirectorySeparatorChar;
                //aimPath += "/";
            }
            // �ж�Ŀ��Ŀ¼�Ƿ����������������½�
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
            // �������е��ļ���Ŀ¼
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
    /// ��ȡ��ǰ·����ָ���ļ�
    /// </summary>
    /// <param name="path">·��</param>
    /// <param name="suffix">��׺��ʽ (bmp txt)</param>
    /// <param name="fileNameList">�ļ�������б�</param>
    /// <param name="isSubcatalog">�Ƿ�༭��Ŀ¼</param>
    public static void GetFiles(string path, string suffix, ref List<string> fileNameList, bool isSubcatalog = false)
    {
        string filename;
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] file = dir.GetFiles();
        //����������ļ���ʱ��Ҫʹ��  
        DirectoryInfo[] dii = dir.GetDirectories();

        foreach (FileInfo f in file)
        {
            filename = f.FullName;//�õ����ļ�������·��
            if (filename.EndsWith(suffix))//�ж��ļ���׺������ȡָ����ʽ���ļ�ȫ·��������fileList  
            {
                fileNameList.Add(filename);
                Debug.Log(filename);
            }
        }
        //��ȡ���ļ����ڵ��ļ��б��ݹ����
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

    #region   ��ȡ������

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
        //��Ϊtrueʱ����Time.timeScale=0�����������ִ��
        seq.SetUpdate(true);
    }

    public static async Task TaskDelayAsync(float delayTime, System.Action callback = null)
    {
        Task t1 = new Task(() =>
        {
            int time = (int)(delayTime * 1000);
            Thread.Sleep(time);
            Debug.Log("�ӳ�  �߳�id: " + Thread.CurrentThread.ManagedThreadId);
        });
        t1.Start();
        await t1;

        Debug.LogFormat("�ӳ�:{0} ���,  �߳�id: {1}", delayTime, Thread.CurrentThread.ManagedThreadId);
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
    //    //    //Debug.Log("�ӳ�  �߳�id: " + Thread.CurrentThread.ManagedThreadId);
    //    //});
    //    //Debug.LogFormat("�ӳ�:{0}  �߳�id: {1}", delayTime, Thread.CurrentThread.ManagedThreadId);
    //    StopwatchEnd(delayTime.ToString());
    //    if (callback != null)
    //    {
    //        callback();
    //    }
    //}

    #endregion


    #region   ������ѯ
    static void GetProvince()
    {
        //string url = "http://www.nmc.cn/rest/province";
        //StartCoroutine(RequsetLoadManager.Instance.GetRequest(url, (string str) =>
        //{
        //    Debug.Log("��ѯʡ�� : " + str);
        //}));
    }

    static void GetCity()
    {
        //string url2 = "http://www.nmc.cn/rest/province/AHE";
        //StartCoroutine(RequsetLoadManager.Instance.GetRequest(url2, (string str) =>
        //{
        //    Debug.Log("���д����ѯ: " + str);
        //}));
    }

    static void GetWeather()
    {
        //string url3 = "http://www.nmc.cn/rest/weather?stationid=54602";
        //StartCoroutine(RequsetLoadManager.Instance.GetRequest(url3, (string str) =>
        //{
        //    Debug.Log("��ѯ��������: " + str);
        //}));
    }

    #endregion

    #region   ���鵽vertor3

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
    /// �����������
    /// </summary>
    public static T[] RandomSortArray<T>(T[] arr)
    {
        System.Random r = new System.Random();
        for (int i = 0; i < arr.Length; i++)
        {
            int index = r.Next(arr.Length);
            Debug.LogFormat("index = {0}", index);
            T temp = arr[i];  //��ǰԪ�غ����Ԫ�ؽ���λ��
            arr[i] = arr[index];
            arr[index] = temp;
        }
        return arr;
    }

    /// <summary>
    /// �������List 
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

    #region   �Ƿ�������ӽ���

    public static bool IsInCameraView(Camera camera, Transform target)
    {
        //ת��Ϊ�ӽ�����
        Vector3 viewPos = camera.WorldToViewportPoint(target.position);

        // z<0�������������
        if (viewPos.z < 0) return false;

        //̫Զ�ˣ��������ˣ�
        if (viewPos.z > camera.farClipPlane) return false;

        // x,yȡֵ�� 0~1֮��ʱ�������ӽǷ�Χ�⣻
        if (viewPos.x < 0 || viewPos.y < 0 || viewPos.x > 1 || viewPos.y > 1) return false;

        return true;
    }

    #endregion

    #region   �ֵ�
    public static void DicRemove()
    {

    }
    #endregion


    #region  Url����ת��
    ///// <summary>
    ///// ��ȡ�豸�б�url (��ά���� ����)
    ///// </summary>
    //public static string GetDeviceUrlStr(string url, string deviceModule = "1", string buildName = "6#��ҵȹ������", string floorName = "3��")
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
    //    //api = "http://ai.landingol.com:8187/princeps/device/queryPage?pageNo=1&pageSize=99999&floor=3��&building=6%23��ҵȹ������&deviceModule=1";
    //    return ip;
    //}
    #endregion

    #region    
    /// <summary>
    /// ��ȡ���Ƴ��ȵ��ַ���
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

    #region   �Ƕ�
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

    #region   ������ʱ����
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
        Debug.LogFormat("UnityTools -> StopwatchEnd()  {0}��ʱ: {1} ms", msg, Milliseconds);
        return Milliseconds;
    }


    public static string SecondToTime(float time)
    {
        //����ȡ��
        int seconds = (int)time;
        //һСʱΪ3600�� ������3600ȡ����ΪСʱ
        int hour = seconds / 3600;
        //һ����Ϊ60�� ������3600ȡ���ٶ�60ȡ����Ϊ����
        int minute = seconds % 3600 / 60;
        //��3600ȡ���ٶ�60ȡ�༴Ϊ����
        seconds = seconds % 3600 % 60;
        //����00:00:00ʱ���ʽ
        return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, seconds);
    }
    #endregion
}
