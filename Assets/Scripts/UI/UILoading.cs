using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoading : UIBase
{

    protected override void Show()
    {
        base.Show();
        Invoke("JumpMainScene", 2f);
    }

    public void JumpMainScene()
    {
        ProjectManager.eventHanlder.SendEvent((uint)CEvent.JumpScene, SceneName.Main);
    }

}
