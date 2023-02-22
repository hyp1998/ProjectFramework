using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectAutoSize : MonoBehaviour
{
    //原始尺寸
    private Vector2 olSize;
    //缩放后的尺寸
    private Vector2 size;
    //原始尺寸宽高比
    private float al;
    private RectTransform self;
    public bool lockPos = false;
    internal float ReferHeight;
    internal float ReferWidth;
    private Vector2 parentSize;
    public float FrameThickness;


    //设置宽高
    public void SetWidthHight()
    {
        self = GetComponent<RectTransform>();
        parentSize = self.parent.GetComponent<RectTransform>().rect.size;
        ReferWidth = parentSize.x - FrameThickness;
        ReferHeight = parentSize.y - FrameThickness;
        self.GetComponent<Image>().SetNativeSize();
        olSize = self.sizeDelta / 10;
        al = olSize.x / olSize.y;

        if (olSize.x < olSize.y)
            size = new Vector2(ReferHeight * al, ReferHeight);
        else
            size = new Vector2(ReferWidth, ReferWidth / al);


        self.sizeDelta = size;
        if (lockPos)
            self.anchoredPosition = Vector2.zero;


        self.SetPivot(PivotPresets.MiddleCenter);
        self.SetAnchor(AnchorPresets.MiddleCenter);
    }
}
