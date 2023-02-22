using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// ��ѡ��ť�飬����ѡ��һ�� 
/// ֧���Ҽ���ݴ��� >>>GameObject/UI/RadioGroup
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
            Debug.LogError($"�Ѿ����ڶ�Ӧ��Key{ allotIndex }");
        }
    }

    public void OnStart()
    {
        //��ʼ����Ĭ�ϵ�һ��ִ�лص�
        //�����¼���Ҫ��Awake���������֮ǰ
        SetSelectState(0);
    }

    //ˢ�µ�ѡ��״̬
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
            //�޸ľ�״̬
            _RadioButtonTemp.isSelect = false;
            _RadioButtonTemp.RefreshState(_RadioButtonTemp.isSelect);
            //�޸���״̬
            _RadioButtonTemp = onclickRadioButton;
            _RadioButtonTemp.isSelect = true;
            _RadioButtonTemp.RefreshState(_RadioButtonTemp.isSelect);
            OnClickRadio?.Invoke(obj);
        }

        
    }

    /// <summary>
    /// ������ѡ��ť
    /// �����Awake��
    /// </summary>
    /// <param name="cb"></param>
    public void AddListener(RadioOnClick cb)
    {
        OnClickRadio -= cb;
        OnClickRadio += cb;
    }

    /// <summary>
    /// ȡ��������ѡ��ť
    /// </summary>
    /// <param name="cb"></param>
    public void RemoveListener(RadioOnClick cb)
    {
        OnClickRadio -= cb;
        OnClickRadio = null;
    }

    /// <summary>
    /// �Զ�������״̬Ϊtrue
    /// </summary>
    /// <param name="index"></param>
    public void SetSelectState(int index)
    {
        if (index >= CSRadioButtons.Count || index < 0)
        {
            Debug.LogError($" {index} ����������Χ 0 ~ {CSRadioButtons.Count - 1}");
            return;
        }
        CSRadioButtons[index].SetSelectState();
    }
}
