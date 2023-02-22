using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectManager : MonoBehaviour
{

    public static ProjectManager Instance;
    public static EventHanlderManager eventHanlder = new EventHanlderManager(EventHanlderManager.DispatchType.Event);

    private ScreenType _screenType = ScreenType.None;
    public ScreenType screenType
    {
        get
        {
            if (_screenType == ScreenType.None)
            {
                if (Screen.width > Screen.height)
                {
                    _screenType = ScreenType.HorizontalScreen;
                }
                else
                {
                    _screenType = ScreenType.VerticalScreen;
                }
                Debug.LogError("屏幕类型：" + _screenType.ToString());
            }
            return _screenType;
        }
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            OnAwake();
           
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        OnStart();
    }

    public void OnAwake()
    {
        Loom.Initialize();
        UIManager.Instance.OnInit();
        CSSceneManager.Instance.OnInit();
    }

    public void OnStart()
    {
        UIManager.Instance.OpenPanel<UIFirst>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UITipManager.Instance.ShowTip("测试测试");
        }
    }

    public void OnApplicationQuit()
    {

    }
}

