using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITip : MonoBehaviour
{
    public Text content;
    public Transform bg;
    public CanvasGroup canvasGroup;

    private float animTime = 0.3f;
    private float DestroyTime = 3f;
    private float target_y = 300;

    public void Show(string _content)
    {
        content.text = _content;
        content.color = Color.red;
        ShowAnim();
    }

    public void Show(string _content, Color _color)
    {
        content.text = _content;
        content.color = _color;
        ShowAnim();
    }

    public void Destroy()
    {
        DestroyAnim(() =>
        {
            UITipManager.Instance.RemoveTip(this);
            Destroy(gameObject);
        });
    }

    private void ShowAnim()
    {
        bg.localScale = Vector3.zero;
        bg.localPosition = new Vector3(0, 300, 0);
        canvasGroup.alpha = 0;

        bg.DOScale(Vector3.one, animTime);
        canvasGroup.DOFade(1, animTime);
        Invoke("Destroy", DestroyTime);
    }

    public void TipUpAnim()
    {
        //再快速连续点击的时候，强制修正到最新坐标
        bg.localPosition = new Vector3(0, target_y, 0);

        target_y = bg.transform.localPosition.y + 60;
        bg.DOLocalMove(new Vector3(0, target_y, 0), animTime);
    }

    public void DestroyAnim(Action cb)
    {
        canvasGroup.DOFade(0, animTime).OnComplete(() =>
        {
            cb?.Invoke();
        });
    }
}
