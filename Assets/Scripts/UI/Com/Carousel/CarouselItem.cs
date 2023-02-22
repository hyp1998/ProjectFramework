using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class CarouselItem : MonoBehaviour
{

    private int objIndex;
    private Vector3 lastPos;
    private int objMax;
    private Image texture;
    private Image bg;
    private RectAutoSize rectAutoSize;
    private CanvasGroup canvasGroup;

    public int spIndex;
    private int spMaxCount;

    private Sprite spCache = null;

    public void Init(Vector3 size, Vector3 _lastPos, int _index, int _spMax, int _objMax)
    {
        texture = this.transform.Find("Texture").GetComponent<Image>();
        rectAutoSize = texture.GetComponent<RectAutoSize>();
        canvasGroup = this.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        bg = this.transform.Find("bg").GetComponent<Image>();
        lastPos = _lastPos;
        objIndex = spIndex = _index;
        spMaxCount = _spMax;
        objMax = _objMax;
    }

    public void SetPos(Vector3 pos)
    {
        this.transform.localPosition = pos;
    }

    public void SetSprite(Sprite sprite, float opacity)
    {
        if (texture)
        {
            texture.sprite = sprite;
            texture.SetNativeSize();
            canvasGroup.alpha = opacity;
            rectAutoSize.SetWidthHight();
        }
    }

    public void SetSprite(string texUrl, float opacity)
    {
        if (string.IsNullOrEmpty(texUrl))
        {
            Debug.LogError("滚动视图Url为null");
            return;
        }
        canvasGroup.alpha = 0f;
        CSNetworking.GetAsync(texUrl, (data) =>
        {
            byte[] urlContents = data[0] as byte[];
            spCache = Utility.GetSprite(urlContents);
            SetSprite(spCache, opacity);
        }, RequestType.ByteArray);
    }

    public void Move(float offset, Action<CarouselItem> restAction)
    {
        float endPos = this.transform.localPosition.x - offset;
        this.transform.DOLocalMoveX(endPos, 0.6f).onComplete += () =>
        {
            objIndex--;
            if (objIndex == -1)
            {
                SetPos(lastPos);
                objIndex = objMax - 1;

                spIndex += objMax;
                if (spIndex >= spMaxCount)
                {
                    spIndex = spIndex - spMaxCount;
                }
                canvasGroup.alpha = 0f;
                if (restAction != null)
                    restAction.Invoke(this);
            }
        };
    }


}
