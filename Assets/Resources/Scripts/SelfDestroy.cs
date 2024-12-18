using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour
{
	public float dieTime = 0;

	void Start ()
	{
		if (dieTime > 0) {
			Destroy (gameObject, dieTime);
		}
	}
	void DestroyMe ()
	{
		Destroy (gameObject);
	}
}
