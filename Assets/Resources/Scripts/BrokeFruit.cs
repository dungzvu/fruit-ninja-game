using UnityEngine;
using System.Collections;

public class BrokeFruit : MonoBehaviour
{
	public Color fruitColor;
	// Use this for initialization
	private float dieTime = 3;
	private float force = 100;
	private float torque = 30;

	//nhung vet ban va vet loang khi chem hoa qua
	private GameObject foamObj;
	private GameObject stain;
	private GameObject directedStain;
	private float stainGap = 0.6f;
	private int maxStain = 3;

	void Start ()
	{
		//load vet ban

		//foam
		foamObj = Resources.Load (GlobalVariables.FoamPath, typeof(GameObject)) as GameObject;
		GameObject foamInst = Instantiate (foamObj, transform.position, Quaternion.identity) as GameObject;
        foamInst.transform.Translate(Vector3.forward * GlobalVariables.FrontZ);
		setSpriteColor (foamInst, fruitColor);

		//random 1->3 stain
		int n = Random.Range (1, maxStain);
		stain = Resources.Load (GlobalVariables.StainPath, typeof(GameObject)) as GameObject;
		for (int i=0; i<n; i++) {
			GameObject stainInst = Instantiate (stain,
			            transform.position + new Vector3 (Random.Range (-stainGap, stainGap), Random.Range (-stainGap, stainGap), 0),
			            Quaternion.identity) as GameObject;
            stainInst.transform.Translate(Vector3.forward * GlobalVariables.BackZ);
			float size = Random.Range (0.8f, 2f);
			stainInst.transform.localScale = Vector3.one * size;
			setSpriteColor (stainInst, fruitColor);
		}

		//directed stain
		directedStain = Resources.Load (GlobalVariables.DirectedStainPath, typeof(GameObject)) as GameObject;
		GameObject dStainInst = Instantiate (directedStain, transform.position, Quaternion.identity) as GameObject;
		float angle = Vector3.Angle (Vector3.right, transform.forward);
        dStainInst.transform.Translate(Vector3.forward * GlobalVariables.BackZ);
		dStainInst.transform.Rotate (Vector3.forward, angle);
		setSpriteColor (dStainInst, fruitColor);

		//tao force cho 2 manh
		Rigidbody rb = transform.GetChild (0).GetComponent (typeof(Rigidbody)) as Rigidbody;
		rb.AddForce (transform.right * -force);
		rb.AddTorque (transform.forward * torque);


		rb = transform.GetChild (1).GetComponent (typeof(Rigidbody)) as Rigidbody;
		rb.AddForce (transform.right * force);
		rb.AddTorque (transform.forward * -torque);

		Destroy (gameObject, dieTime);
	}

	private void setSpriteColor (GameObject obj, Color color)
	{
		SpriteRenderer sr = obj.GetComponent (typeof(SpriteRenderer)) as SpriteRenderer;
		sr.color = color;
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
