using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    /// <summary> 打开UI </summary>
    private Dictionary<string, UIBase> openPanelDic = new Dictionary<string, UIBase>();
    /// <summary> UI对象缓存 </summary>
    private Dictionary<string, GameObject> panelObjCache = new Dictionary<string, GameObject>();

    private Canvas _uicanvas;
    public Canvas UICanvas
    {
        get { return _uicanvas; }
        private set { _uicanvas = value; }
    }

    private Camera _uicamera;
    public Camera UICamera
    {
        get { return _uicamera; }
        private set { _uicamera = value; }
    }

    public void OnInit()
    {
        InitUICanvas();
    }

    public void InitUICanvas()
    {
        GameObject obj = ResourcesManager.Instance.LoadRes<GameObject>(ResType.UI, "UICanvas");
        if (obj)
        {
            GameObject o = UnityEngine.GameObject.Instantiate(obj);
            o.name = "UICanvas";
            UICanvas = o.transform.Find("Canvas").GetComponent<Canvas>();
            UICamera = o.transform.Find("UICamera").GetComponent<Camera>();
            UILayerManager.Instance.InitUILayerNode(UICanvas.transform);
            UnityEngine.GameObject.DontDestroyOnLoad(o);
        }
    }

    /// <summary>
    /// 打开UI
    /// </summary>
    /// <typeparam name="T">UI名字</typeparam>
    /// <param name="uILayerNode">UI层级</param>
    /// <param name="isResident">是否为常驻UI</param>
    /// <returns>UI对象类</returns>
    public T OpenPanel<T>(UILayerNode uILayerNode = UILayerNode.Middle, bool isResident = false) where T : UIBase
    {
        Type type = typeof(T);
        string name = type.Name;
        if (openPanelDic.TryGetValue(name, out UIBase uiBase))
        {
            return uiBase as T;
        }
        GameObject obj = UnityEngine.GameObject.Instantiate(GetUIObj(name));
        if (obj)
        {
            UILayerManager.Instance.SetUILayer(obj, uILayerNode);
            UIBase _uibase = obj.GetComponent<UIBase>();
            _uibase.isResident = isResident;
            openPanelDic.Add(name, _uibase);
            _uibase.InitThis();
            return _uibase as T;
        }
        return null;
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    /// <typeparam name="T">UI名字</typeparam>
    public void ClosePanel<T>() where T : UIBase
    {
        Type type = typeof(T);
        string name = type.Name;
        if (openPanelDic.TryGetValue(name, out UIBase uiBase))
        {
            uiBase.Close();
            UnityEngine.GameObject.Destroy(uiBase.gameObject);
            openPanelDic.Remove(name);
        }
        else
        {
            Debug.LogWarning($"关闭UI  {type.Name}不存在");
        }
    }

    /// <summary>
    /// 关闭所有正常界面
    /// </summary>
    public void CloseAllPanel(bool isCloseResidentPanel = false)
    {
        //是否关闭常驻界面
        if (isCloseResidentPanel)
        {
            CloseAllPanel();
        }
        else
        {
            //检测是否存在常驻界面
            bool isHaveResidentPanel = false;
            foreach (var item in openPanelDic)
            {
                if (item.Value.isResident)
                {
                    isHaveResidentPanel = true;
                    break;
                }
            }

            if (isHaveResidentPanel)
            {
                Dictionary<string, UIBase> normalPanelDic = new Dictionary<string, UIBase>();
                normalPanelDic = openPanelDic.Where(P => !P.Value.isResident).ToDictionary(P => P.Key, P => P.Value);
                foreach (var item in normalPanelDic)
                {
                    if (openPanelDic.ContainsKey(item.Key))
                    {
                        if (openPanelDic.TryGetValue(item.Key, out UIBase uiBase))
                        {
                            uiBase.Close();
                            UnityEngine.GameObject.Destroy(uiBase.gameObject);
                            openPanelDic.Remove(item.Key);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"关闭UI  {item.Key}不存在");
                    }
                }
            }
            else
            {
                CloseAllPanel();
            }
        }
    }

    private void CloseAllPanel()
    {
        foreach (var item in openPanelDic)
        {
            item.Value.Close();
            UnityEngine.GameObject.Destroy(item.Value.gameObject);
        }
        openPanelDic.Clear();
    }

    /// <summary>
    /// 获取UI
    /// </summary>
    /// <typeparam name="T">UI名字</typeparam>
    /// <returns></returns>
    public T GetPanel<T>() where T : UIBase
    {
        Type type = typeof(T);
        UIBase uiBase;
        string name = type.Name;
        if (openPanelDic.TryGetValue(name, out uiBase))
        {
            return uiBase as T;
        }
        Debug.LogWarning($"{type.Name}不存在");
        return null;
    }

    /// <summary>
    /// 获取UI对象
    /// </summary>
    /// <param name="name">UI对象名字</param>
    /// <returns>UI对象</returns>
    public GameObject GetUIObj(string name)
    {
        GameObject obj;
        if (panelObjCache.TryGetValue(name, out obj))
        {
            return obj;
        }
        obj = ResourcesManager.Instance.LoadRes<GameObject>(ResType.UI, name);
        panelObjCache.Add(name, obj);
        return obj;
    }

}


