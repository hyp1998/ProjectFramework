using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EraseMask : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("�Ƿ��������")]
    public bool isAllowEraser;
    [Header("�����뾶")]
    public int Radius = 60;
    [Header("������״�Ƿ�Ϊ����")]
    public bool isRect = true;
    [Header("�������ٰٷֱ�֮��ص�")]
    public int Rate = 95;
    [Header("��������ɫ")]
    public Color Col = new Color(0, 0, 0, 0);
    

    private RawImage mUIRawImage; 
    //�޸ĵ�ͼƬ
    private Texture2D MyTex;
    //��ԭ��ʱ��
    private Texture2D RawImage_Temp;

    //��ǰ��������
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

    //���õ�û����֮ǰ״̬
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
                //Բ�η�Χ
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

    //��⵱ǰ��������
    private void GetTransparentPercent()
    {
        if (!isAllowEraser || !isMouseDown) return;
        fate = changePixelNum / maxPixelNum * 100;
        fate = Math.Round(fate, 2);
        //Debug.Log("������ǰ�ٷֱ�: " + fate);
        if (fate >= Rate)
        {
            CancelInvoke(nameof(GetTransparentPercent));
            //���������¼�
            Debug.LogError("����������" + fate);
        }
    }

    private Vector2 ScreenPoint2Pixel(Vector2 mousePos)
    {
        float imageWidth = mUIRawImage.rectTransform.sizeDelta.x;
        float imageHeight = mUIRawImage.rectTransform.sizeDelta.y;
        Vector3 imagePos = mUIRawImage.rectTransform.anchoredPosition3D;
        //�������image�ϵ�λ��
        float HorizontalPercent = (mousePos.x - (Screen.width / 2 + imagePos.x - imageWidth / 2)) / imageWidth; //�����Image ˮƽ�ϵ�λ��  %
        float verticalPercent = (mousePos.y - (Screen.height / 2 + imagePos.y - imageHeight / 2)) / imageHeight;//�����Image ��ֱ�ϵ�λ��  %
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
