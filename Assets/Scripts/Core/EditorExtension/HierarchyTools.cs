#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class HierarchyTools
{

    [MenuItem("GameObject/UI/Panel Custom")]
    public static void CreatePanel()
    {
        GameObject imgBtnRes = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Custom UI/Panel Custom.prefab");
        GameObject imgBtnInstance = GameObject.Instantiate(imgBtnRes);
        imgBtnInstance.name = "Panel Custom";

        Transform selection = Selection.activeTransform;
        if (selection != null)
        {
            imgBtnInstance.transform.SetParent(selection);
        }
        else
        {
            Transform canvas = GameObject.Find("Canvas").transform;
            if (canvas)
            {
                imgBtnInstance.transform.SetParent(canvas);
            }
        }
        RectTransform rt = imgBtnInstance.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        rt.sizeDelta = Vector2.zero;
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.Euler(Vector3.zero);
        rt.localScale = Vector3.one;

        Selection.activeGameObject = imgBtnInstance;
    }

    [MenuItem("GameObject/UI/Image Custom")]
    public static void CreateImage()
    {
        GameObject imgBtnRes = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Custom UI/Image Custom.prefab");
        GameObject imgBtnInstance = GameObject.Instantiate(imgBtnRes);
        imgBtnInstance.name = "Image Custom";

        Transform selection = Selection.activeTransform;
        if (selection != null)
        {
            imgBtnInstance.transform.SetParent(selection);
        }
        else
        {
            Transform canvas = GameObject.Find("Canvas").transform;
            if (canvas)
            {
                imgBtnInstance.transform.SetParent(canvas);
            }
        }
        RectTransform rt = imgBtnInstance.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        //rt.sizeDelta = Vector2.zero;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.Euler(Vector3.zero);
        rt.localScale = Vector3.one;

        Selection.activeGameObject = imgBtnInstance;
    }


    [MenuItem("GameObject/UI/Toggle Custom")]
    public static void CreateToggle()
    {
        GameObject imgBtnRes = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Custom UI/Toggle Custom.prefab");
        GameObject imgBtnInstance = GameObject.Instantiate(imgBtnRes);
        imgBtnInstance.name = "Toggle Custom";

        Transform selection = Selection.activeTransform;
        if (selection != null)
        {
            imgBtnInstance.transform.SetParent(selection);
            imgBtnInstance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        }
        else
        {
            Transform canvas = GameObject.Find("Canvas").transform;
            if (canvas)
            {
                imgBtnInstance.transform.SetParent(canvas);
            }
        }
        RectTransform rt = imgBtnInstance.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        //rt.sizeDelta = Vector2.zero;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.Euler(Vector3.zero);
        rt.localScale = Vector3.one;

        Selection.activeGameObject = imgBtnInstance;
    }


    [MenuItem("GameObject/UI/Toggle Custom Hover")]
    public static void CreateToggle_hover()
    {
        GameObject imgBtnRes = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Custom UI/Toggle Custom Hover.prefab");
        GameObject imgBtnInstance = GameObject.Instantiate(imgBtnRes);
        imgBtnInstance.name = "Toggle Custom Hover";

        Transform selection = Selection.activeTransform;
        if (selection != null)
        {
            imgBtnInstance.transform.SetParent(selection);
            imgBtnInstance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        }
        else
        {
            Transform canvas = GameObject.Find("Canvas").transform;
            if (canvas)
            {
                imgBtnInstance.transform.SetParent(canvas);
            }
        }
        RectTransform rt = imgBtnInstance.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        //rt.sizeDelta = Vector2.zero;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.Euler(Vector3.zero);
        rt.localScale = Vector3.one;

        Selection.activeGameObject = imgBtnInstance;
    }



    [MenuItem("GameObject/UI/Toggle NotStandard")]
    public static void CreateNotStandardToggle()
    {
        GameObject imgBtnRes = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Custom UI/NotStandardToggle.prefab");
        GameObject imgBtnInstance = GameObject.Instantiate(imgBtnRes);
        imgBtnInstance.name = "Toggle NotStandard";

        Transform selection = Selection.activeTransform;
        if (selection != null)
        {
            imgBtnInstance.transform.SetParent(selection);
            imgBtnInstance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        }
        else
        {
            Transform canvas = GameObject.Find("Canvas").transform;
            if (canvas)
            {
                imgBtnInstance.transform.SetParent(canvas);
            }
        }
        RectTransform rt = imgBtnInstance.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        //rt.sizeDelta = Vector2.zero;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.Euler(Vector3.zero);
        rt.localScale = Vector3.one;

        Selection.activeGameObject = imgBtnInstance;
    }

    [MenuItem("GameObject/UI/TextMeshPro Custom")]
    public static void CreateTextMeshPro()
    {
        GameObject imgBtnRes = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Custom UI/Text (TMP) Custom.prefab");
        GameObject imgBtnInstance = GameObject.Instantiate(imgBtnRes);
        imgBtnInstance.name = "Text (TMP) Custom";

        Transform selection = Selection.activeTransform;
        if (selection != null)
        {
            imgBtnInstance.transform.SetParent(selection);
        }
        else
        {
            Transform canvas = GameObject.Find("Canvas").transform;
            if (canvas)
            {
                imgBtnInstance.transform.SetParent(canvas);
            }
        }
        RectTransform rt = imgBtnInstance.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        //rt.sizeDelta = Vector2.zero;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.Euler(Vector3.zero);
        rt.localScale = Vector3.one;

        Selection.activeGameObject = imgBtnInstance;
    }


    [MenuItem("GameObject/UI/Describe Panel Custom")]
    public static void CreateDescribePanel()
    {
        GameObject imgBtnRes = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Describe Panel Custom.prefab");
        GameObject imgBtnInstance = GameObject.Instantiate(imgBtnRes);
        imgBtnInstance.name = "Describe Panel Custom";

        Transform selection = Selection.activeTransform;
        if (selection != null)
        {
            imgBtnInstance.transform.SetParent(selection);
        }
        else
        {
            Transform canvas = GameObject.Find("Canvas").transform;
            if (canvas)
            {
                imgBtnInstance.transform.SetParent(canvas);
            }
        }
        RectTransform rt = imgBtnInstance.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        //rt.sizeDelta = Vector2.zero;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.Euler(Vector3.zero);
        rt.localScale = Vector3.one;

        Selection.activeGameObject = imgBtnInstance;
    }


    [MenuItem("GameObject/UI/XiangQing Panel")]
    public static void CreateXiangQingPanel()
    {
        GameObject imgBtnRes = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/XiangQing Panel.prefab");
        GameObject imgBtnInstance = GameObject.Instantiate(imgBtnRes);
        imgBtnInstance.name = "XiangQing Panel";

        Transform selection = Selection.activeTransform;
        if (selection != null)
        {
            imgBtnInstance.transform.SetParent(selection);
        }
        else
        {
            Transform canvas = GameObject.Find("Canvas").transform;
            if (canvas)
            {
                imgBtnInstance.transform.SetParent(canvas);
            }
        }
        RectTransform rt = imgBtnInstance.GetComponent<RectTransform>();
        rt.anchoredPosition3D = Vector3.zero;
        //rt.sizeDelta = Vector2.zero;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localRotation = Quaternion.Euler(Vector3.zero);
        rt.localScale = Vector3.one;

        Selection.activeGameObject = imgBtnInstance;
    }


    [InitializeOnLoadMethod]
    private static void StartInitializeOnLoadMethod()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        if (Event.current != null && Event.current.button == 2 && Event.current.type <= EventType.MouseUp)
        {
            if (Selection.activeTransform)
            {
                Vector2 mousePosition = Event.current.mousePosition;
                EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "GameObject/", null);
            }
        }
    }
}
#endif