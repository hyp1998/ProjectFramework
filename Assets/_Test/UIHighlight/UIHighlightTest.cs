using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIHighlightTest : MonoBehaviour
{
    public Image testM;
    public float testa = 1;
    public Color color = new Color(1,1,1,1);

    void Update()
    {
        testM.material.SetColor("_LtColor", color);
        testM.material.SetFloat("_LtOuterStrength", testa);
        testM.material.SetFloat("_LtInnerStrength", testa);
    }

}
