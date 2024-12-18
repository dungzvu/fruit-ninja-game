using UnityEngine;
using System.Collections;

public class SceneCredit : MonoBehaviour
{
	public GameObject buttonQuitObject;
	// Use this for initialization
	void Start ()
	{
		var buttonQuit = (SimpleButton)buttonQuitObject.GetComponent (typeof(SimpleButton));
		buttonQuit.OnClick += NextScene;
	}

	void NextScene ()
	{
		Debug.Log ("goto Main Menu");
		Application.LoadLevel ("sceneMainMenu");
	}


}
