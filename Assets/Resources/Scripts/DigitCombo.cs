using UnityEngine;
using System.Collections;

public class DigitCombo : MonoBehaviour
{

	// Use this for initialization
	public Sprite[] list;

	private SpriteRenderer spr;

	void Start ()
	{
		
	}
	
	public void setNumber (int n)
	{
        spr = GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
		spr.sprite = list [n];
	}



}
