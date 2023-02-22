using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EraseMask : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("是否允许擦除")]
    public bool isAllowEraser;
    [Header("擦除半径")]
    public int Radius = 60;
    [Header("擦除形状是否为矩形")]
    public bool isRect = true;
    [Header("擦除多少百分比之后回调")]
    public int Rate = 95;
    [Header("擦除后颜色")]
    public Color Col = new Color(0, 0, 0, 0);
    

    private RawImage mUIRawImage; 
    //修改的图片
    private Texture2D MyTex;
    //还原临时用
    private Texture2D RawImage_Temp;

    //当前擦除进度
    private double fate;
    private float maxPixelNum;
    private float changePixelNum;
    private bool isMouseDown = false;

    void Awake()
    {
        mUIRawImage = GetComponent<RawImage>();
        var tex = mUIRawImage.texture as Texture2D;

        MyTex = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        MyTex.SetPixels(tex.GetPixels());
        MyTex.Apply();

        RawImage_Temp = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
        RawImage_Temp.SetPixels(tex.GetPixels());
        RawImage_Temp.Apply();

        mUIRawImage.texture = MyTex;

        maxPixelNum = MyTex.GetPixels().Length;
        changePixelNum = 0;

        InvokeRepeating(nameof(GetTransparentPercent), 0f, 0.2f);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isMouseDown = true;
            if (!isAllowEraser) return;
            var posA = ScreenPoint2Pixel(Input.mousePosition);
            ChangePixelColorByCircle((int)posA.x, (int)posA.y, Radius, Col);
        }
        else
        {
            isMouseDown = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ResetRawImage();
        }
    }

    //重置到没擦除之前状态
    public void ResetRawImage()
    {
        MyTex = new Texture2D(RawImage_Temp.width, RawImage_Temp.height, TextureFormat.ARGB32, false);
        MyTex.SetPixels(RawImage_Temp.GetPixels());
        MyTex.Apply();
        mUIRawImage.texture = MyTex;
        changePixelNum = 0;
        InvokeRepeating(nameof(GetTransparentPercent), 0f, 0.2f);
    }


    void ChangePixelColorByCircle(int x, int y, int radius, Color col)
    {
        for (int i = -radius; i < radius; i++)
        {
            var py = y + i;
            if (py < 0 || py >= MyTex.height)
            {
                continue;
            }

            for (int j = -radius; j < radius; j++)
            {
                var px = x + j;
                if (px < 0 || px >= MyTex.width)
                {
                    continue;
                }
                //圆形范围
                if (new Vector2(px - x, py - y).magnitude > radius && !isRect)
                {
                    continue;
                }

                Color c = MyTex.GetPixel(px, py);
                if (c.a != 0f)
                {
                    MyTex.SetPixel(px, py, col);
                    changePixelNum++;
                }
            }
        }
        MyTex.Apply();
    }

    //检测当前擦除进度
    private void GetTransparentPercent()
    {
        if (!isAllowEraser || !isMouseDown) return;
        fate = changePixelNum / maxPixelNum * 100;
        fate = Math.Round(fate, 2);
        //Debug.Log("擦除当前百分比: " + fate);
        if (fate >= Rate)
        {
            CancelInvoke(nameof(GetTransparentPercent));
            //触发结束事件
            Debug.LogError("擦除结束了" + fate);
        }
    }

    private Vector2 ScreenPoint2Pixel(Vector2 mousePos)
    {
        float imageWidth = mUIRawImage.rectTransform.sizeDelta.x;
        float imageHeight = mUIRawImage.rectTransform.sizeDelta.y;
        Vector3 imagePos = mUIRawImage.rectTransform.anchoredPosition3D;
        //求鼠标在image上的位置
        float HorizontalPercent = (mousePos.x - (Screen.width / 2 + imagePos.x - imageWidth / 2)) / imageWidth; //鼠标在Image 水平上的位置  %
        float verticalPercent = (mousePos.y - (Screen.height / 2 + imagePos.y - imageHeight / 2)) / imageHeight;//鼠标在Image 垂直上的位置  %
        float x = HorizontalPercent * MyTex.width;
        float y = verticalPercent * MyTex.height;
        return new Vector2(x, y);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isAllowEraser = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isAllowEraser = false;
    }



}
