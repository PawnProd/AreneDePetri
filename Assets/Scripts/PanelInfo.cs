using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelInfo : MonoBehaviour {

    public TextMeshProUGUI life;
    public TextMeshProUGUI armor;
    public TextMeshProUGUI action;
    public TextMeshProUGUI power;

	public void SetPosition(Vector3 pos)
    {
        GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public void SetTextParam(int pv, int pvMax, int arm, int act, int pow)
    {
        life.text = pv + "/" + pvMax;
        armor.text = arm.ToString();
        action.text = act.ToString();
        power.text = pow.ToString();
    }
}
