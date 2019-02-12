using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public Transform pointInit1;
    public Transform pointInit2;
    public float respoundTime = .1f;

    public GameObject prefab;
    public static GameObject[] skillet;
    public GameObject[] skillEditor;

    public static int catchedBibs;
    public static int carma;

    public static bool isGameGoing = false;

    public GameObject mainMenuEditor;
    public static GameObject menuPage;
    public GameObject gamePanelEditor;
    public static GameObject gamePanel;
    public GameObject startPageEditor;
    public static GameObject startPage;
    public GameObject startPageObject;
    public static GameObject adPage;
    public GameObject adPageEditor;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bonusText;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI bestScoreText;

    public static bool wasNoAd = true;

    public AudioSource audioPlayerEditor;
    public static AudioSource audioPlayer;

    void Start()
    {
        if (!PlayerPrefs.HasKey("score"))
            PlayerPrefs.SetInt("score", 0);
        menuPage = mainMenuEditor;
        gamePanel = gamePanelEditor;
        adPage = adPageEditor;
        startPage = startPageEditor;
        audioPlayer = audioPlayerEditor;

        skillet = skillEditor;
        prefab.GetComponent<Rigidbody2D>().freezeRotation = true;
        prefab.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
    }

    void Update()
    {
        scoreText.text = "" + catchedBibs;
        bonusText.text = "x" + (carma / 7 + 1);
        if (!isGameGoing)
        {
            UpdateScore();
        }
    }

    void UpdateScore()
    {
        currentScoreText.text = "" + catchedBibs;
        if (catchedBibs > PlayerPrefs.GetInt("score"))
        {
            PlayerPrefs.SetInt("score", catchedBibs);
        }
        bestScoreText.text = "BEST " + PlayerPrefs.GetInt("score");
    }

    IEnumerator GenerateBibsCorutine()
    {
        System.Random rand = new System.Random();
        while (isGameGoing)
        {
            Vector3 position = new Vector3(pointInit1.position.x + (pointInit2.position.x - pointInit1.position.x) * (float)rand.NextDouble(), pointInit2.position.y, pointInit2.position.z);
            prefab.GetComponent<Rigidbody2D>().gravityScale = Mathf.Min(prefab.GetComponent<Rigidbody2D>().gravityScale + Mathf.Sqrt(catchedBibs) / 20, 0.8f);
            Instantiate(prefab, position, prefab.transform.rotation);

            respoundTime = Mathf.Max(1 - (float)catchedBibs / 40f, 0.4f);
            yield return new WaitForSeconds(respoundTime);
        }
        
    }

    static void AdsTime()
    {
        isGameGoing = false;
        adPage.SetActive(true);
    }

    public void ContinueGame()
    {
        AdsManager.instance.ShowRewardedAd();
        wasNoAd = false;
        isGameGoing = true;
        StartCoroutine(GenerateBibsCorutine());
        adPage.SetActive(false);
    }

    public void StopGame()
    {
        adPage.SetActive(false);
        EndGame();
    }

    public static void LoseGame()
    {
        if (wasNoAd)
        {
            AdsTime();
        }
        else
        {
            EndGame();
        }
    }

    public static void EndGame()
    {
        if (catchedBibs > PlayerPrefs.GetInt("score"))
            PlayerPrefs.SetInt("score", catchedBibs);

        isGameGoing = false;
        menuPage.SetActive(true);
        gamePanel.SetActive(false);
    }


    public void StartGame()
    {
        foreach (GameObject garbage in GameObject.FindGameObjectsWithTag("bound"))
        {
            Destroy(garbage);
        }
        prefab.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
        catchedBibs = 0;
        carma = 0;
        startPage.SetActive(false);
        menuPage.SetActive(false);
        startPageObject.SetActive(false);
        gamePanel.SetActive(true);
        isGameGoing = true;
        wasNoAd = true;
        StartCoroutine(GenerateBibsCorutine());
    }
}
