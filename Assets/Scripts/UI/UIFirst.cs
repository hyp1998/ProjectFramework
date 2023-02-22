using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFirst : UIBase
{
    private Button _Btn_JumpScene;
    private Button Btn_JumpScene { get { return _Btn_JumpScene ?? (_Btn_JumpScene = Get<Button>("Btn_JumpScene")); } }

    protected override void Init()
    {
        base.Init();
        Btn_JumpScene.onClick.AddListener(BtnJumpSceneOnClick);
    }


    protected override void Show()
    {
        base.Show();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    public void BtnJumpSceneOnClick()
    {
        ProjectManager.eventHanlder.SendEvent((uint)CEvent.JumpScene,SceneName.Loading);
    }
}
