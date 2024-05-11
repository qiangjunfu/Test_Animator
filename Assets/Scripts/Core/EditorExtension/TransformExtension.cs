using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
    /// <summary>
    /// 获取Transform在Hierarchy中的路径
    /// </summary>
    public static string GetHierarchyPath(this Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }

    /// <summary>
    ///  将Transform及其所有子对象的层级设置为指定层级
    /// </summary>
    public static void SetLayerRecursively(this Transform transform, int layer)
    {
        transform.gameObject.layer = layer;
        foreach (Transform child in transform)
        {
            child.SetLayerRecursively(layer);
        }
    }


    public static void ResetTransform(this Transform transform) 
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 在Transform的所有子孙对象中查找指定名称的子对象
    /// </summary>
    public static Transform FindDeepChild(this Transform transform, string name)
    {
        foreach (Transform child in transform)
        {
            if (child.name == name)
                return child;

            Transform result = child.FindDeepChild(name);
            if (result != null)
                return result;
        }
        return null;
    }


    public static void DestroyChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }


    public static List<GameObject> GetChild(this GameObject obj)
    {
        List<GameObject> tempArrayobj = new List<GameObject>();
        foreach (Transform child in obj.transform)
        {
            tempArrayobj.Add(child.gameObject);
        }
        return tempArrayobj;
    }

}
