using UnityEngine;
using System.Collections;

public class Bomb : Fruit
{
	public void breakMe (Vector3 direction)
	{
		//thua cuoc luon
		Debug.Log ("Loose");
	}
}
