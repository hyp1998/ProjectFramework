using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 单选按钮组，至少选中一项 
/// 支持右键快捷创建 >>>GameObject/UI/RadioGroup
/// </summary>
public class CSRadioGroup : UIBehaviour
{
    public static Dictionary<int, List<CSRadioButton>> CSRadioButtonDic = new Dictionary<int, List<CSRadioButton>>();

    [SerializeField]
    private List<CSRadioButton> CSRadioButtons = new List<CSRadioButton>();

    public delegate void RadioOnClick(GameObject obj);
    private event RadioOnClick OnClickRadio;

    private CSRadioButton _RadioButtonTemp;

    protected override void Awake()
    {
        base.Awake();
        OnAwake();
    }

    protected override void Start()
    {
        base.Start();
        OnStart();
    }

    public void OnAwake()
    {
        List<Component> components = transform.GetComponentsInChildren(typeof(CSRadioButton), true).ToList();

        int allotIndex = CSRadioButtonDic.Count;

        if (!CSRadioButtonDic.ContainsKey(allotIndex))
        {
            for (int i = 0; i < components.Count; i++)
            {
                CSRadioButton crb = components[i] as CSRadioButton;
                crb.OnAwake();
                crb.isSelect = i == 0;
                crb.groupIndex = allotIndex;
                crb.RefreshState(crb.isSelect);
                CSRadioButtons.Add(crb);
            }

            CSRadioButtonDic.Add(allotIndex, CSRadioButtons);
        }
        else
        {
            Debug.LogError($"已经存在对应的Key{ allotIndex }");
        }
    }

    public void OnStart()
    {
        //初始化，默认第一个执行回调
        //监听事件需要放Awake或者在这个之前
        SetSelectState(0);
    }

    //刷新单选组状态
    public void RefreshRadioGroup(int index, GameObject obj)
    {
        CSRadioButton onclickRadioButton = obj.GetComponent<CSRadioButton>();

        if (_RadioButtonTemp == null)
        {
            for (int i = 0; i < CSRadioButtons.Count; i++)
            {
                CSRadioButton cSRadioButton = CSRadioButtons[i];

                cSRadioButton.isSelect = onclickRadioButton.GetHashCode() == cSRadioButton.GetHashCode();

                cSRadioButton.RefreshState(cSRadioButton.isSelect);

                if (cSRadioButton.isSelect)
                    OnClickRadio?.Invoke(obj);
            }
            _RadioButtonTemp = onclickRadioButton;
        }
        else
        {
            //修改旧状态
            _RadioButtonTemp.isSelect = false;
            _RadioButtonTemp.RefreshState(_RadioButtonTemp.isSelect);
            //修改新状态
            _RadioButtonTemp = onclickRadioButton;
            _RadioButtonTemp.isSelect = true;
            _RadioButtonTemp.RefreshState(_RadioButtonTemp.isSelect);
            OnClickRadio?.Invoke(obj);
        }

        
    }

    /// <summary>
    /// 监听单选框按钮
    /// 建议放Awake里
    /// </summary>
    /// <param name="cb"></param>
    public void AddListener(RadioOnClick cb)
    {
        OnClickRadio -= cb;
        OnClickRadio += cb;
    }

    /// <summary>
    /// 取消监听单选框按钮
    /// </summary>
    /// <param name="cb"></param>
    public void RemoveListener(RadioOnClick cb)
    {
        OnClickRadio -= cb;
        OnClickRadio = null;
    }

    /// <summary>
    /// 自定义设置状态为true
    /// </summary>
    /// <param name="index"></param>
    public void SetSelectState(int index)
    {
        if (index >= CSRadioButtons.Count || index < 0)
        {
            Debug.LogError($" {index} 超出索引范围 0 ~ {CSRadioButtons.Count - 1}");
            return;
        }
        CSRadioButtons[index].SetSelectState();
    }
}
