using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour
{
	public float dieTime = 3;
	private float alphaStep;
	private SpriteRenderer sr;
	// Use this for initialization
	void Start ()
	{
		alphaStep = 1.0f / dieTime;
		sr = GetComponent (typeof(SpriteRenderer)) as SpriteRenderer;
		Destroy (gameObject, dieTime);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, sr.color.a - Time.fixedDeltaTime * alphaStep);
	}
}
