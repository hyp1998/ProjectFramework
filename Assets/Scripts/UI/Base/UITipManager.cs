using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITipManager : Singleton<UITipManager>
{
    private List<UITip> uITips = new List<UITip>();

    private GameObject _UITipObj;
    private GameObject UITipObj
    {
        get
        {
            if (_UITipObj == null)
            {
                _UITipObj = ResourcesManager.Instance.LoadRes<GameObject>(ResType.UI, "UITip");
            }
            return _UITipObj;
        }
    }


    public void ShowTip(string content)
    {
        UITip uITip = LoadTip();
        uITip.Show(content);
        AddTip(uITip);
    }

    public void ShowTip(string content, Color color)
    {
        UITip uITip = LoadTip();
        uITip.Show(content, color);
        AddTip(uITip);
    }

    private void AddTip(UITip uITip)
    {
        if (!uITips.Contains(uITip))
        {
            TipUp();
            uITips.Add(uITip);
        }
    }

    public void RemoveTip(UITip uITip)
    {
        if (uITips.Contains(uITip))
        {
            uITips.Remove(uITip);
        }
    }

    private void TipUp()
    {
        for (int i = 0; i < uITips.Count; i++)
        {
            if (uITips[i] != null)
            {
                uITips[i].TipUpAnim();
            }
        }
    }

    private UITip LoadTip()
    {
        UITip uITip = null;
        GameObject obj = UnityEngine.GameObject.Instantiate(UITipObj);
        if (obj)
        {
            UILayerManager.Instance.SetUILayer(obj, UILayerNode.Top);
            uITip = obj.GetComponent<UITip>();
        }
        return uITip;
    }

}
