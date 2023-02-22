using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI层级管理
/// </summary>
public class UILayerManager : Singleton<UILayerManager>
{

    private Dictionary<UILayerNode, Transform> UILayerNodeDic = new Dictionary<UILayerNode, Transform>();

    public void InitUILayerNode(Transform nodeLayerParent)
    {
        UILayerNodeDic.Clear();
        foreach (UILayerNode item in Enum.GetValues(typeof(UILayerNode)))
        {
            GameObject obj = new GameObject(item.ToString());
            obj.transform.SetParent(nodeLayerParent, false);
            //设置Transform
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            //设置全屏锚点
            RectTransform rt = obj.AddComponent<RectTransform>();
            //rt.sizeDelta = new Vector2(Screen.width, Screen.height);
            rt.sizeDelta = new Vector2(0, 0);
            rt.SetAnchor(AnchorPresets.StretchAll);

            UILayerNodeDic.Add(item, obj.transform);
        }
    }

    public void SetUILayer(GameObject uiObj, UILayerNode uILayerNode)
    {
        uiObj.transform.SetParent(GetUILayerNodeTra(uILayerNode), false);
    }

    private Transform GetUILayerNodeTra(UILayerNode uILayerNode)
    {
        Transform tra = null;
        if (UILayerNodeDic.TryGetValue(uILayerNode, out tra))
        {
            return tra;
        }
        return tra;
    }

}

/// <summary>
/// UI层级
/// </summary>
public enum UILayerNode
{
    Bottom = 0,
    Middle,
    Top
}

