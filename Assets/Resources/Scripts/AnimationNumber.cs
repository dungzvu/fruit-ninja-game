using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimationNumber : MonoBehaviour
{

	// Use this for initialization
	private Animator anim;
	private Text text;
	private int value;
	private int expectedValue;

	void Start ()
	{
		anim = GetComponent<Animator> ();
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (value != expectedValue) {
			text.text = expectedValue.ToString ();
			value = expectedValue;
		}

	}

	public void setNumber (int n)
	{
		if (anim != null) {
			anim.Play ("NumberChange");
			expectedValue = n;
		}
	}
}
