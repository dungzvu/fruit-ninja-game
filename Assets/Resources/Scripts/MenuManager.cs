using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{

	// Use this for initialization
	public Slice slice;

	void Start ()
	{
		slice.OnSlice += OnFruitHit;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnFruitHit (GameObject[] fruits, Vector3 direction)
	{
		if (fruits == null) {
			return;
		}
		fruits [0].GetComponent<MenuFruit> ().breakMe (direction);
        StartCoroutine(GotoNextLevel(fruits[0].tag));
		
	}

    private IEnumerator GotoNextLevel(string tag)
    {
        yield return new WaitForSeconds(1.5f);
        if (tag.Equals("FruitNewGame"))
        {
            Application.LoadLevel("MainScene");
        }
        else if (tag.Equals("FruitQuit"))
        {
            Application.Quit();
        }
    }


}
