using UnityEngine;
using System.Collections;

public class Fruit : MonoBehaviour
{

	public GameObject BrokeApple;
	private float forceMax = 800;
	private float forceMin = 680;
	private float torqueMax = 20;

	public void breakMe (Vector3 direction)
	{
		var obj = (GameObject)Instantiate (BrokeApple, transform.position, Quaternion.identity);
		obj.transform.LookAt (direction, Vector3.back);
		Destroy (gameObject);
	}
	// Use this for initialization
	void Start ()
	{
		init ();
	}

	protected void init ()
	{
		//tung le theo force random
		float force = Random.Range (forceMin, forceMax);
		Rigidbody rb = GetComponent (typeof(Rigidbody)) as Rigidbody;
		rb.AddForce ((new Vector3 (Random.Range (-0.1f, 0.1f), 1, 0)) * force);
		Vector3 torque = Random.Range (0, torqueMax) * Vector3.one;
        if (transform.position.x > 0)
        {
            torque.x *= -1;
        }
        torque.x *= 0.8f;
		rb.AddTorque (torque);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
