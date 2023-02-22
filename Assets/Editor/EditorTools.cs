using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorTools : EditorWindow
{

    [MenuItem("Tools/清除PlayerPrefs值")]
    public static void CreateWindows()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/批量生成ImageTarget")]
    public static void CreateImageTarget()
    {
        EditorWindow.GetWindow(typeof(EditorTools));
    }

    private static GameObject ImageTargetObj;
    private static Transform parentNode;
   

    public void OnGUI()
    {
        //Selection.gameObjects
        EditorGUILayout.LabelField("选择识别对象");
        ImageTargetObj = (GameObject)EditorGUILayout.ObjectField(ImageTargetObj, typeof(GameObject), true);
        EditorGUILayout.LabelField("选择识别对象父物体");
        parentNode = (Transform)EditorGUILayout.ObjectField(parentNode, typeof(Transform), true);
        EditorGUILayout.Space();
        if (GUILayout.Button("开始生成"))
        {
            
        }
    }



}
