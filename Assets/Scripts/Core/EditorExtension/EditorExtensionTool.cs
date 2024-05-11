//#if UNITY_EDITOR
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;


//public class EditorExtensionTool
//{
//    [MenuItem("FQJ/ShowMesh/True")]
//    public static void ShowMesh()
//    {
//        GameObject[] gos = Selection.gameObjects;
//        if (gos.Length > 0)
//        {
//            for (int i = 0; i < gos.Length; i++)
//            {
//                Debug.Log("选中游戏物体:  i = " + i + "  :  " + gos[i].name);

//                Transform[] subTrans = gos[i].GetComponentsInChildren<Transform>();
//                for (int j = 0; j < subTrans.Length; j++)
//                {
//                    MeshRenderer MeshRenderer = subTrans[j].GetComponent<MeshRenderer>();
//                    if (MeshRenderer != null)
//                    {
//                        MeshRenderer.enabled = true;
//                    }

//                    Collider sphereCollider = subTrans[j].GetComponent<Collider>();
//                    if (sphereCollider != null)
//                    {
//                        sphereCollider.enabled = true;
//                    }
//                }

//            }
//        }
//        else
//        {
//            Debug.Log("请选择至少一个游戏物体");
//        }
//    }

//    [MenuItem("FQJ/ShowMesh/False")]
//    public static void HideMesh()
//    {
//        GameObject[] gos = Selection.gameObjects;
//        if (gos.Length > 0)
//        {
//            for (int i = 0; i < gos.Length; i++)
//            {
//                Debug.Log("选中游戏物体:  i = " + i + "  :  " + gos[i].name);

//                Transform[] subTrans = gos[i].GetComponentsInChildren<Transform>();
//                for (int j = 0; j < subTrans.Length; j++)
//                {
//                    MeshRenderer MeshRenderer = subTrans[j].GetComponent<MeshRenderer>();
//                    Debug.Log("子物体:  " + subTrans[j].name);
//                    if (MeshRenderer != null)
//                    {
//                        MeshRenderer.enabled = false;

//                    }


//                    Collider sphereCollider = subTrans[j].GetComponent<Collider>();
//                    if (sphereCollider != null)
//                    {
//                        sphereCollider.enabled = false;
//                    }
//                }

//            }
//        }
//        else
//        {
//            Debug.Log("请选择至少一个游戏物体");
//        }
//    }



//    [MenuItem("FQJ/ShowCamera/True")]
//    public static void ShowCamera()
//    {
//        GameObject[] gos = Selection.gameObjects;
//        if (gos.Length > 0)
//        {
//            for (int i = 0; i < gos.Length; i++)
//            {
//                Debug.Log("选中游戏物体:  i = " + i + "  :  " + gos[i].name);

//                Transform[] subTrans = gos[i].GetComponentsInChildren<Transform>();
//                for (int j = 0; j < subTrans.Length; j++)
//                {
//                    Camera camera = subTrans[j].GetComponent<Camera>();
//                    if (camera != null)
//                    {
//                        AudioListener al = subTrans[j].GetComponent<AudioListener>();
//                        UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData hdacd =
//                             subTrans[j].GetComponent<UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData>();

//                        camera.enabled = true;
//                        al.enabled = true;
//                        hdacd.enabled = true;
//                    }
//                }
//            }
//        }
//        else
//        {
//            Debug.Log("请选择至少一个游戏物体");
//        }
//    }

//    [MenuItem("FQJ/ShowCamera/false")]
//    public static void HideCamera()
//    {
//        GameObject[] gos = Selection.gameObjects;
//        if (gos.Length > 0)
//        {
//            for (int i = 0; i < gos.Length; i++)
//            {
//                Debug.Log("选中游戏物体:  i = " + i + "  :  " + gos[i].name);

//                Transform[] subTrans = gos[i].GetComponentsInChildren<Transform>();
//                for (int j = 0; j < subTrans.Length; j++)
//                {
//                    Camera camera = subTrans[j].GetComponent<Camera>();
//                    if (camera != null)
//                    {
//                        AudioListener al = subTrans[j].GetComponent<AudioListener>();
//                        UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData hdacd =
//                             subTrans[j].GetComponent<UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData>();

//                        camera.enabled = false;
//                        al.enabled = false;
//                        hdacd.enabled = false;
//                    }
//                }
//            }
//        }
//        else
//        {
//            Debug.Log("请选择至少一个游戏物体");
//        }
//    }



//    [MenuItem("FQJ/GPU instacing/True")]
//    public static void OpenGPUInstance()
//    {
//        GameObject[] gos = Selection.gameObjects;
//        if (gos.Length > 0)
//        {
//            for (int i = 0; i < gos.Length; i++)
//            {
//                Debug.Log("选中游戏物体:  i = " + i + "  :  " + gos[i].name);

//                Transform[] subTrans = gos[i].GetComponentsInChildren<Transform>();
//                for (int j = 0; j < subTrans.Length; j++)
//                {
//                    MeshRenderer meshRenderer = subTrans[j].GetComponent<MeshRenderer>();



//                    if (meshRenderer)
//                    {
//                        Material[] mats = meshRenderer.materials;

//                        for (int k = 0; k < mats.Length; k++)
//                        {
//                            mats[k].enableInstancing = true;
//                        }
//                    }
//                }
//            }
//        }
//    }


//    [MenuItem("FQJ/GPU instacing/False")]
//    public static void HideGPUInstance()
//    {
//        GameObject[] gos = Selection.gameObjects;
//        if (gos.Length > 0)
//        {
//            for (int i = 0; i < gos.Length; i++)
//            {
//                Debug.Log("选中游戏物体:  i = " + i + "  :  " + gos[i].name);

//                Transform[] subTrans = gos[i].GetComponentsInChildren<Transform>();
//                for (int j = 0; j < subTrans.Length; j++)
//                {
//                    MeshRenderer meshRenderer = subTrans[j].GetComponent<MeshRenderer>();



//                    if (meshRenderer)
//                    {
//                        Material[] mats = meshRenderer.materials;

//                        for (int k = 0; k < mats.Length; k++)
//                        {
//                            mats[k].enableInstancing = false;
//                        }
//                    }
//                }
//            }
//        }
//    }

//}
//#endif