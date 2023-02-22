using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 单选按钮
/// </summary>
public class CSRadioButton : Button
{
    public int groupIndex = -1;

    public bool isSelect;

    private Transform mBackground;
    private Transform Background { get { return mBackground ?? (mBackground = transform.Get<Transform>("Background")); } }

    private Transform mCheckmark;
    private Transform Checkmark { get { return mCheckmark ?? (mCheckmark = transform.Get<Transform>("Checkmark")); } }

    private CSRadioGroup mCSRadioGroup;

    public void OnAwake()
    {
        mCSRadioGroup = transform.parent.GetComponent<CSRadioGroup>();
        AddListener();
    }

    /// <summary>
    /// 刷新单选按钮状态
    /// </summary>
    /// <param name="_isSelectState"></param>
    public void RefreshState(bool _isSelectState)
    {
        //刷新按钮图片状态
        Checkmark.gameObject.SetActive(_isSelectState);
        Background.gameObject.SetActive(!_isSelectState);
        //刷新按钮点击图片
        Graphic _graphic = _isSelectState ? Checkmark.GetComponent<Graphic>() : Background.GetComponent<Graphic>();
        targetGraphic = _graphic;
    }

    public void AddListener()
    {
        onClick.AddListener(RadioButtonOnClick);
    }

    public void RemoveListener()
    {
        onClick.AddListener(RadioButtonOnClick);
    }

    private void RadioButtonOnClick()
    {
        isSelect = true;
        TriggerOnClickCallBack();
    }

    public void SetSelectState(bool state = true)
    {
        RadioButtonOnClick();
    }

    /// <summary>
    /// 触发点击，刷新单选组
    /// </summary>
    private void TriggerOnClickCallBack()
    {
        if (mCSRadioGroup)
            mCSRadioGroup.RefreshRadioGroup(groupIndex, gameObject);
    }

}
