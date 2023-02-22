using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTest : MonoBehaviour
{
    public CSRadioGroup cSRadioGroup;
    void Awake() 
    {
        cSRadioGroup.AddListener(OnClick);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            cSRadioGroup.SetSelectState(4);
        }
    }

    public void OnClick(GameObject obj)
    {
        Debug.LogError($"Ãû×Ö£º{obj.name}");
    }


}
