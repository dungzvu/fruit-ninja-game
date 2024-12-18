using UnityEngine;
using System.Collections;

public class MenuFruit : Fruit
{
	private Rigidbody _rigidbody;
	private float force = 100;

	// Use this for initialization
	void Start ()
	{
		init ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void init ()
	{
		Debug.Log ("init menu fruit");
		_rigidbody = GetComponent (typeof(Rigidbody)) as Rigidbody;
		Vector3 _torque = force * Vector3.one;
		_rigidbody.AddTorque (_torque);
		_rigidbody.useGravity = false;
	}

	public void breakMe (Vector3 direction)
	{
		if (BrokeApple != null) {
			var obj = (GameObject)Instantiate (BrokeApple, transform.position, Quaternion.identity);
			obj.transform.LookAt (direction, Vector3.back);
			Destroy (gameObject);
		} else {
			_rigidbody.useGravity = true;
			_rigidbody.AddForce (Vector3.up * force);
			Destroy (gameObject, 2);
		}
	}
}
