using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CSSceneManager : SingletonMonoBehaviour<CSSceneManager>
{
    public SceneName NowSceneName = SceneName.First;

    public void OnInit()
    {
        ProjectManager.eventHanlder.Reg((uint)CEvent.JumpScene, OnJumpScene);
    }

    public void OnDestory()
    {
        ProjectManager.eventHanlder.UnReg((uint)CEvent.JumpScene, OnJumpScene);
    }

    public void LoadScene(SceneName sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(sceneName.ToString(), mode);
        NowSceneName = sceneName;
        SceneInit();
    }

    public AsyncOperation LoadSceneAsync(SceneName sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        NowSceneName = sceneName;
        return SceneManager.LoadSceneAsync(sceneName.ToString(), mode);
    }

    public void OnCompleted(AsyncOperation asyncOperation)
    {
        if (asyncOperation != null && asyncOperation.isDone)
        {
            SceneInit();
        }
    }


    private void OnJumpScene(uint eventId, params object[] data)
    {
        if (data.Length <= 0) return;
        SceneName sceneName = (SceneName)data[0];
        LoadSceneAsync(sceneName).completed += OnCompleted;
    }

    //场景初始化
    public void SceneInit()
    {
        switch (NowSceneName)
        {
            case SceneName.First:
                UIManager.Instance.CloseAllPanel(); 
                UIManager.Instance.OpenPanel<UIFirst>();
                break;
            case SceneName.Loading:
                UIManager.Instance.ClosePanel<UIFirst>();
                UIManager.Instance.OpenPanel<UILoading>();
                break;
            case SceneName.Main:
                UIManager.Instance.ClosePanel<UILoading>();
                UIManager.Instance.OpenPanel<UIMain>();
                break;
            default:
                break;
        }
    }

}

public enum SceneName
{
    First,
    Loading,
    Main
}