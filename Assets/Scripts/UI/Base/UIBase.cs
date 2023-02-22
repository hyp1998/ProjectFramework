using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    [HideInInspector]
    public bool isResident = false;

    private void Awake() { OnAwake(); }

    private void Start() { OnStart(); }

    private void Update() { OnUpdate(); }

    private void OnDestroy() { OnDestory(); }

    protected virtual void OnAwake() { }

    protected virtual void OnStart() { }

    protected virtual void OnUpdate() { }

    protected virtual void OnDestory() { }

    protected virtual void Init() { }

    protected virtual void Show() { }

    /// <summary> 此方法只供外部调用，不需要子类调用 </summary>
    public void InitThis()
    {
        Init();
        Show();
    }

    public virtual void Close() { CancelInvoke(); }

    protected T Get<T>(string path) where T : UnityEngine.Object
    {
        if (this == null)
            return null;
        return Utility.GetObject<T>(this.transform, path);
    }

    public IEnumerator UpdateUI(RectTransform rectTransform, int num = 2)
    {
        yield return num;
        if (rectTransform)
        {
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            rectTransform.localScale = Vector3.one;
            rectTransform.gameObject.SetActive(true);
        }
    }


}
