using UnityEngine;
using System.Collections;

public class Combo : MonoBehaviour {

	// Use this for initialization
    private DigitCombo digit1;
    private DigitCombo digit2;

    void Awake()
    {
        //Debug.Log("Combo Awake");
        digit1 = transform.GetChild(0).GetChild(1).GetComponent(typeof(DigitCombo)) as DigitCombo;
        digit2 = transform.GetChild(0).GetChild(2).GetComponent(typeof(DigitCombo)) as DigitCombo;
    }
	void Start () {
	}

    public void setNumber(int n)
    {
        digit1.setNumber(n);
        digit2.setNumber(n);
    }
}
