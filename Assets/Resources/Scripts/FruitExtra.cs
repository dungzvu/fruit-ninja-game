using UnityEngine;
using System.Collections;

public class FruitExtra : Fruit
{
	
	public void breakMe (Vector3 direction)
	{
		GameObject critical = Resources.Load (GlobalVariables.FreezePath, typeof(GameObject)) as GameObject;
		GameObject star = Resources.Load (GlobalVariables.StarPath, typeof(GameObject)) as GameObject;
        GameObject crit = Instantiate(critical, transform.position, Quaternion.identity) as GameObject;
        crit.transform.Translate(Vector3.forward * GlobalVariables.FrontZ);

		base.breakMe (direction);

		Debug.Log ("Extra");

	}

}
