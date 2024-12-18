using UnityEngine;
using System.Collections;

public class SimpleButton : MonoBehaviour
{
	public delegate void ClickAction ();
	public event ClickAction OnClick;

	private Camera mainCamera;
	private Animator anim;
	private SpriteRenderer sprite;
	private Rect rect;

	private float currentDeltaTime = 0;
	private float delay = 0.7f;

	// Use this for initialization
	void Start ()
	{	
		mainCamera = Camera.main;
		if (mainCamera == null) {
			Debug.Log ("Khong tim thay camera");
		}
		anim = (Animator)GetComponent (typeof(Animator));
		if (anim == null) {
			Debug.Log ("Khong tim thay animator");
		}
		sprite = (SpriteRenderer)GetComponent (typeof(SpriteRenderer));
		rect.center = sprite.bounds.center;
		rect.width = sprite.bounds.extents.x * 2;
		rect.height = sprite.bounds.extents.y * 2;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (Input.GetMouseButtonDown (0)) {
			if (HitTest ()) {
				//dosomthing
				anim.Play (0);
				currentDeltaTime = Time.deltaTime;
			}
		}
		if (currentDeltaTime > 0) {
			currentDeltaTime += Time.deltaTime;
		}
		if (currentDeltaTime >= delay) {
			currentDeltaTime = 0;
			OnClick ();
		}
	}



	bool HitTest ()
	{
		Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
		if (sprite.bounds.IntersectRay (ray)) {
			Debug.Log ("click button");
			return true;
		}
		return false;
	}


}
