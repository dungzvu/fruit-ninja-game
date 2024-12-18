using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private AnimationNumber scoreView;
	private AnimationNumber lifeView;
    private Text highScoreView;

	public GameObject ScorePanel;
	public GameObject PausePanel;
    public GameObject HighScoreLabel;

    private string highScoreName = "nHighScore";
    private int defaultHighScore = 10;

	private PlayManager playManager;
	// Use this for initialization
	void Start ()
	{
		scoreView = GameObject.Find ("Score").GetComponent<AnimationNumber> ();
		lifeView = GameObject.Find ("Life").GetComponent<AnimationNumber> ();
		playManager = GameObject.Find ("PlayManager").GetComponent<PlayManager> ();
        highScoreView = GameObject.Find("HighScore").GetComponent<Text>();

        if (PlayerPrefs.GetInt(highScoreName, 0)==0)
        {
            PlayerPrefs.SetInt(highScoreName,defaultHighScore);
        }
        //hien thi highscore
        highScoreView.text = PlayerPrefs.GetInt(highScoreName).ToString();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

    public void ShowScorePanel(int score)
    {
        ScorePanel.SetActive(true);
        ScorePanel.transform.GetChild(1).GetComponent<Text>().text = score.ToString();
        if (score > PlayerPrefs.GetInt(highScoreName))
        {
            PlayerPrefs.SetInt(highScoreName, score);
            PlayerPrefs.Save();
            highScoreView.text = score.ToString();
            HighScoreLabel.SetActive(true);
        }
        else
        {
            HighScoreLabel.SetActive(false);
        }
        ScorePanel.GetComponent<Animator>().Play("ScorePanelAnim");
    }

    public void ShowPausePanel(){
        PausePanel.SetActive(true);
        PausePanel.GetComponent<Animator>().Play("ScorePanelAnim");
    }

    public void ExitScorePanel()
    {
        ScorePanel.SetActive(false);      
        ScorePanel.GetComponent<Animator>().Play("ScorePanelAnimExit");
        
    }

    public void ExitPausePanel()
    {
        PausePanel.SetActive(false);
        PausePanel.GetComponent<Animator>().Play("ScorePanelAnimExit");

    }
	public void ReplayScorePane ()
	{
        ExitScorePanel();
        Replay();
    }

    public void ReplayPausePane()
    {
        ExitPausePanel();
        Replay();
    }

    private void Replay(){
		playManager.Replay ();
		Debug.Log ("Replay");
	}

	public void Quit ()
	{
		Time.timeScale = 1;
		Application.LoadLevel ("MenuScene");
	}

	public void Resume ()
	{
		PausePanel.SetActive (false);
		wait (1);
		playManager.Resume ();
	}

	public void ShowScore (int score)
	{
		scoreView.setNumber (score);
	}

	public void ShowLife (int life)
	{
		lifeView.setNumber (life);
	}

	/*private GameObject getScorePanel ()
	{
		if (ScorePanel == null) {
			ScorePanel = GameObject.Find ("ScorePanel");
		}
		return ScorePanel;
	}

	private GameObject getPausePanel ()
	{
		if (PausePanel == null) {
			PausePanel = GameObject.Find ("PausePanel");
		}
		return PausePanel;
	}*/

	//utilities
	private IEnumerator wait (float t)
	{
		yield return new WaitForSeconds (10);
	}
	
	private void WaitSecond (float t)
	{
		StartCoroutine ("wait", t);
	}
}
