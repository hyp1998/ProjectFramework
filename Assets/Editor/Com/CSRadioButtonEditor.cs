using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(CSRadioButton))]
public class CSRadioButtonEditor : ButtonEditor
{
    CSRadioButton mCSRadioButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        mCSRadioButton = (CSRadioButton)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var groupIndex = this.serializedObject.FindProperty("groupIndex");
        EditorGUILayout.PropertyField(groupIndex);

        var isSelect = this.serializedObject.FindProperty("isSelect");
        EditorGUILayout.PropertyField(isSelect);

        base.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();
    }

    [MenuItem("GameObject/UI/RadioGroup")]
    public static void AddCSRadioGroup(MenuCommand menuCommand)
    {
        UnityEngine.Object[] canvasObjs = FindObjectsOfType(typeof(Canvas));
        if (canvasObjs.Length <= 0)
        {
            Debug.LogError("ÇëÏÈ´´½¨Canvas");
            return;
        }
        Transform CanvasT = GameObject.Find(canvasObjs[0].name).transform;

        GameObject RadioGroup = new GameObject("RadioGroup");
        RadioGroup.transform.SetParent(CanvasT, false);
        RectTransform rt = RadioGroup.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(500, 200);
        RadioGroup.AddComponent<CSRadioGroup>();
        RadioGroup.AddComponent<HorizontalLayoutGroup>();

        for (int i = 0; i < 3; i++)
        {
            CreateRadioBtn(RadioGroup.transform, $"RadioBtn{ i}");
        }
    }

    public static void CreateRadioBtn(Transform parent, string name)
    {
        GameObject RadioBtn = new GameObject(name);
        RadioBtn.transform.SetParent(parent.transform, false);
        RectTransform rt = RadioBtn.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(150, 150);
        RadioBtn.AddComponent<CSRadioButton>();

        GameObject Background = new GameObject("Background");
        Background.transform.SetParent(RadioBtn.transform, false);
        RectTransform brt = Background.AddComponent<RectTransform>();
        brt.sizeDelta = new Vector2(150, 150);
        Background.AddComponent<Image>().color = Color.white;

        GameObject Checkmark = new GameObject("Checkmark");
        Checkmark.transform.SetParent(RadioBtn.transform, false);
        RectTransform crt = Checkmark.AddComponent<RectTransform>();
        crt.sizeDelta = new Vector2(150, 150);
        Checkmark.AddComponent<Image>().color = Color.green;
    }
}
