using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayManager : MonoBehaviour
{

    public static PlayManager instance;

    public Slice slice;

    private int score = 0;
    private int life = 3;

    private UIManager UIManagerObject;

    private int maxFruit = 7;
    private int minFruit = 3;
    public GameObject[] fruits;
    private Vector3 camTopRight;
    private Vector3 camBottomLeft;
    private float defaultTimeInit = 5;
    private float extraTimeInit = 0.8f;
    private float timeToInitFruit = 5;
    private float deltaTime = 0;
    private GameObject ComboObj;
    private GameObject CriticalObject;

    public GameObject FreezeIcon;

    private float ExtraTime = 5;
    private float PassedExtraTime = 0;
    private bool IsExtra = false;

    private AudioSource audio;
    private ComboManager comboManager;

    public AudioClip startSound;
    public AudioClip initFruitsSound;
    public AudioClip failSound;
    public AudioClip bombSound;

    private bool isPause = false;

    // Use this for initialization
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        slice.OnSlice += OnSliceEvent;

        camBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        camTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        //load combo
        ComboObj = Resources.Load(GlobalVariables.ComboPath, typeof(GameObject)) as GameObject;
        CriticalObject = Resources.Load(GlobalVariables.CriticalPath, typeof(GameObject)) as GameObject;

        UIManagerObject = GameObject.Find("UIManager").GetComponent<UIManager>();

        comboManager = new ComboManager();

        audio = GetComponent(typeof(AudioSource)) as AudioSource;

        reset();
    }

    void DoCombo(int n, Vector3 pos)
    {
        score+=n;
        UIManagerObject.ShowScore (score);
        GameObject _combo = Instantiate(ComboObj, pos + new Vector3(0.5f, -1.6f, 0), Quaternion.identity) as GameObject;
        _combo.transform.Translate(Vector3.forward * GlobalVariables.FrontZ);
        Combo comboScript = _combo.GetComponent(typeof(Combo)) as Combo;
        comboScript.setNumber(n);
        //Debug.Log("Do combo goi combo");
    }

    void reset()
    {
        score = 0;
        life = 3;
        IsExtra = false;
        PassedExtraTime = 0;
        timeToInitFruit = defaultTimeInit;
        //sua view
        UIManagerObject.ShowScore(score);
        UIManagerObject.ShowLife(life);
        Time.timeScale = 1;

        slice.reset();

        RemoveAllFruits();

        audio.PlayOneShot(startSound);

        isPause = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //kiem tra esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        if (isCycleComplete())
        {
            if (!IsExtra)
            {
                RandomFruit();
            }
            else
            {
                RandomExtraFruit();
            }
        }
        int n = 0;
        Vector3 pos;
        if (comboManager.Update(out n, out pos))
        {
            DoCombo(n, pos);
        }
    }

    void OnSliceEvent(GameObject[] fruits, Vector3 direction)
    {
        Debug.Log("slice trung");
        foreach (var fruit in fruits)
        {
            if (fruit.tag == "Fruit")
            {
                Fruit f = fruit.GetComponent(typeof(Fruit)) as Fruit;
                f.breakMe(direction);
            }
            else if (fruit.tag == "Bomb")
            {
                Bomb b = fruit.GetComponent(typeof(Bomb)) as Bomb;
                b.breakMe(direction);
            }
            else if (fruit.tag == "ExtraFruit")
            {
                FruitExtra fe = fruit.GetComponent(typeof(FruitExtra)) as FruitExtra;
                fe.breakMe(direction);
            }

        }
        calculateFruits(fruits);
    }

    void calculateFruits(GameObject[] fruits)
	{
		//kiem tra co bomb khong
		if (checkBomb (fruits)) {
			//thua
            StartCoroutine(Loose());
		} else if (checkExtra (fruits)) {
			//hien hieu ung
			PreExtra ();
		}
		int nF = countFruit (fruits);
		score += nF;

		//random critical
		int rnd = Random.Range (0, 35);
        //tim phan tu dau tien la fruit
        Transform tf = null;
        foreach (var f in fruits)
        {
            if (f.gameObject.tag == "Fruit")
            {
                tf = f.gameObject.transform;
                break;
            }
        }
		if (rnd == 10) {
			//critical
            if (tf != null)
            {
                GameObject crit=Instantiate(CriticalObject, tf.position + new Vector3(0.2f, -1.5f, 0), Quaternion.identity) as GameObject;
                crit.transform.Translate(Vector3.forward * GlobalVariables.FrontZ);
                score += 10;
            }
		}
        //kiem tra combo
        if (nF > 0)
        {
            comboManager.addFruit(nF, tf.position);
        }

		UIManagerObject.ShowScore (score);
	}

    private IEnumerator Loose()
    {
        var anim = Camera.main.GetComponent(typeof(Animator)) as Animator;
        anim.Play("Shake");
        Debug.Log("Thua roi");
        //pause
        slice.SetDisable();
        isPause = true;
        audio.PlayOneShot(bombSound);

        yield return new WaitForSeconds(3);
        Time.timeScale = 0;
        //hien bang score
        UIManagerObject.ShowScorePanel(score);
    }

    void Pause()
    {
        Time.timeScale = 0;
        UIManagerObject.ShowPausePanel();
    }

    bool checkBomb(GameObject[] fruits)
    {
        foreach (var f in fruits)
        {
            if (f.tag == "Bomb")
            {
                return true;
            }
        }
        return false;
    }

    bool checkExtra(GameObject[] fruits)
    {
        foreach (var f in fruits)
        {
            if (f.tag == "ExtraFruit")
            {
                return true;
            }
        }
        return false;
    }

    int countFruit(GameObject[] fruits)
    {
        int i = 0;
        foreach (var f in fruits)
        {
            if (f.tag == "Fruit")
            {
                i++;
            }
        }
        return i;
    }

    bool isCycleComplete()
    {
        if (!isPause)
        {
            deltaTime += Time.deltaTime;
            if (deltaTime >= timeToInitFruit)
            {
                deltaTime = 0;
                return true;
            }
        }
        return false;
    }

    void RandomFruit()
    {
        int n = Random.Range(minFruit, maxFruit);
        int nFruit = fruits.Length - 1;//phan tu cuoi la extra

        float stepx = (camTopRight.x - camBottomLeft.x)*0.58f/ n;
        float pivotx = camBottomLeft.x * 0.58f;
        for (int i = 0; i < n; i++)
        {
            int id = Random.Range(0, nFruit);
            float x = Random.Range(pivotx, pivotx + stepx);
            pivotx += stepx;
            Instantiate(fruits[id], new Vector3(x, camBottomLeft.y, 0), Quaternion.identity);
        }

        //randomExtra
        int temp = Random.Range(0, 12);
        if (temp == 8)
        {
            //extra
            int id = nFruit;
            float x = Random.Range(pivotx, camTopRight.x)*0.58f;
            Instantiate(fruits[id], new Vector3(x, camBottomLeft.y, 0), Quaternion.identity);
        }

        audio.PlayOneShot(initFruitsSound);
    }

    void PreExtra()
    {
        timeToInitFruit = extraTimeInit;
        IsExtra = true;
        FreezeIcon.SetActive(true);
        deltaTime = timeToInitFruit;
    }

    void RandomExtraFruit()
    {
        PassedExtraTime += extraTimeInit;
        Debug.Log("pass " + PassedExtraTime);
        if (PassedExtraTime >= ExtraTime)
        {
            EndExtra();
        }
        int n = (int)Random.Range(minFruit * 0.5f, maxFruit * 0.5f);
        int nFruit = fruits.Length - 2;//phan tu cuoi la extra, va bomb

        for (int i = 0; i < n; i++)
        {
            int id = Random.Range(0, nFruit);
            float x = Random.Range(camBottomLeft.x, camTopRight.x);
            Instantiate(fruits[id], new Vector3(x, camBottomLeft.y, 0), Quaternion.identity);
        }
        audio.PlayOneShot(initFruitsSound);
    }

    void EndExtra()
    {
        timeToInitFruit = defaultTimeInit;
        IsExtra = false;
        FreezeIcon.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Fruit") && !IsExtra && !isPause)
        {
            LifeSub();
            audio.PlayOneShot(failSound);
        }
    }

    void LifeSub()
    {
        Debug.Log("life " + life);
        life--;
        UIManagerObject.ShowLife(life);
        if (life <= 0)
        {
            StartCoroutine(Loose());
        }
    }

    private void RemoveAllFruits()
    {
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
        foreach (var f in fruits)
        {
            Destroy(f);
        }
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");
        foreach (var f in bombs)
        {
            Destroy(f);
        }
        GameObject[] efruits = GameObject.FindGameObjectsWithTag("ExtraFruit");
        foreach (var f in efruits)
        {
            Destroy(f);
        }
        GameObject[] destroys = GameObject.FindGameObjectsWithTag("Destroyable");
        foreach (var f in destroys)
        {
            Destroy(f);
        }
    }

    public void Replay()
    {
        reset();
        RemoveAllFruits();
        Time.timeScale = 1;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public int getScore()
    {
        return score;
    }

}
