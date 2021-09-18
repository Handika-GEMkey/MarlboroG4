using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayGUI : MonoBehaviour {

	public ManagerRacing managerRacing;

	public Text ScoreText;
	public Text TimerText;
    public Text TokenText;
    public Text LivesText;

	public Image Life1Image;
	public Image Life2Image;
	public Image Life3Image;
    public Image Life1ImageBg;
    public Image Life2ImageBg;
    public Image Life3ImageBg;
    

	public GameObject GameStartedUp;
    public GameObject IntroStartedup;
	public GameObject GameplayGUIGameObject;
	public GameObject CountDownGameObject;
	public GameObject BlockingUI;
	public Text CountDownText;

	public AudioHandler audioHandler;

    [Header("Game Timer")]
    public Image TimerBarOutline;
    public GameObject TimeBarMark;
    private float TimeBarMarkPos;


    [Header("Game Finish Result")]
	public GameObject GameFinishUI1st;
    public GameObject GameFinishUIAll;
    public Text NameText;
	public Text FinalScoreText;
	public Text CoinText;

    [Header("Game Over Timer Result")]
    public GameObject GameOverUITImer;
    public Text NameGOText;
    public Text FinalScoreGOText;
    public Text CoinGOText;
    [Header("Game Over Life Result")]
    public GameObject GameOverUILife;
    [Header("Tutorial Sections")]
    public GameObject TutorialKeyboardObj;
    public GameObject TutorialSwipeObj;
    public GameObject TutorialParent;
    [SerializeField] private int isUsingKeyboard = 0; //0=swipe 1=keyboard
    [SerializeField] private bool noTutorial ;//true = tutorialon , false = tutorialoff
    public RetreavingData RetreavingData;
    public WebGLBridger WebGLBridger;
    public Animator CamAnimator;
    public Animator UIAnimator;
    public Animator UIHTPAnimator;
    public GameObject UIHTPObj;

    void Start()
	{
		managerRacing.CallbackGameOverLife += OnGameOverLife;
        managerRacing.CallbackGameOverTime += OnGameOverTime;
        managerRacing.CallbackGameFinish += OnGameFinish;
        managerRacing.CallbackTokenScore += OnPlayerToken;
        managerRacing.CallBackGameTimer += OnGameTimer;
		managerRacing.CallbackPlayerLife += OnPlayerLife;
		managerRacing.CallbackPlayerScore += OnPlayerScore;
        //StartGameplay();

    }

    public void InitTutorial(int newKey)
    {
        isUsingKeyboard = newKey;
    }

	public void InitScore(float Point)
	{
		ScoreText.text = String.Format("{0:n0}", Point);
	}

    public void InitToken(float Point)
    {
        TokenText.text = String.Format("{0:n0}", Point);
    }

    public void InitTimer(int time)
	{
        float timeBarCount=0f;
		TimeSpan ts = TimeSpan.FromSeconds(time);
		TimerText.text = ts.ToString();
        timeBarCount = (float)(((float)time/60f));
       
        TimerBarOutline.fillAmount = timeBarCount;
        TimeBarMarkPos = 455f - (891f *(1- timeBarCount));
        
        TimeBarMark.transform.localPosition = new Vector3(TimeBarMarkPos, TimeBarMark.transform.localPosition.y, TimeBarMark.transform.localPosition.z);
    }

	public void InitLife(int life)
	{
		switch (life)
		{
			case 0:
				
                Life1Image.gameObject.SetActive(false);
                Life2Image.gameObject.SetActive(false);
                Life3Image.gameObject.SetActive(false);
                Life1ImageBg.gameObject.SetActive(true);
                Life2ImageBg.gameObject.SetActive(true);
                Life3ImageBg.gameObject.SetActive(true);

                LivesText.text = "0";
                break;
			case 1:

                Life1Image.gameObject.SetActive(true);
                Life2Image.gameObject.SetActive(false);
                Life3Image.gameObject.SetActive(false);
                Life1ImageBg.gameObject.SetActive(false);
                Life2ImageBg.gameObject.SetActive(true);
                Life3ImageBg.gameObject.SetActive(true);
                LivesText.text = "1";
                break;
			case 2:
				
              
                Life1Image.gameObject.SetActive(true);
                Life2Image.gameObject.SetActive(true);
                Life3Image.gameObject.SetActive(false);
                Life1ImageBg.gameObject.SetActive(false);
                Life2ImageBg.gameObject.SetActive(false);
                Life3ImageBg.gameObject.SetActive(true);
                LivesText.text = "2";
                break;
			case 3:
                Life1Image.gameObject.SetActive(true);
                Life2Image.gameObject.SetActive(true);
                Life3Image.gameObject.SetActive(true);
                Life1ImageBg.gameObject.SetActive(false);
                Life2ImageBg.gameObject.SetActive(false);
                Life3ImageBg.gameObject.SetActive(false);
                LivesText.text = "3";

                break;
           
        }
	}

	public void StartGameplay()
	{
		StartCoroutine(CountDownCoroutine());
    }

	public IEnumerator CountDownCoroutine()
	{
        CamAnimator.Play("BeginAnimation", 0, 0);
        GameStartedUp.SetActive(false);
		yield return new WaitForSeconds(1);
		CountDownGameObject.SetActive(true);
		audioHandler.PlaySFXCounter();
		CountDownText.text = "3";
        CountDownText.fontSize = 240;
        yield return new WaitForSeconds(1);
		audioHandler.PlaySFXCounter();
		CountDownText.text = "2";
		yield return new WaitForSeconds(1);
		audioHandler.PlaySFXCounter();
		CountDownText.text = "1";
		yield return new WaitForSeconds(1);
		audioHandler.PlaySFXCounterFinal();
		CountDownText.text = "GO!";
		CountDownText.fontSize = 160;
		yield return new WaitForSeconds(1);
		CountDownGameObject.SetActive(false);
   
		
		GameplayGUIGameObject.SetActive(true);
        UIAnimator.Play("OpenUIAnimation", 0, 0);
		audioHandler.PlayCarSound();
        ManagerRacing.Instance.StartGame();
    }

	void OnGameOverLife(float totalScore)
	{
		StartCoroutine(SkipTimer_GameOverLife(2f, totalScore));		
	}

    void OnGameOverTime(float totalScore)
    {
        StartCoroutine(SkipTimer_GameOverTime(2f, totalScore));       
    }

    void OnGameFinish(float totalScore)
    {
        StartCoroutine(SkipTimer_GameFinish(2f, totalScore));
    }


    IEnumerator SkipTimer_GameFinish(float timer, float totalScore)
	{
		managerRacing.RacingSpeed = 0;
		managerRacing.GameStarted = false;
		audioHandler.StopCarSound();
        FinalScoreText.text = totalScore.ToString();
        GameplayGUIGameObject.SetActive(false);
        if (!managerRacing.IsPopupPointOpen)
        {
            GameFinishUI1st.SetActive(true);
        }
        else
        {
            GameFinishUIAll.SetActive(true);
        }

        WebGLBridger.SubmitScore(200);
        yield return new WaitForSeconds(timer);
	}


    IEnumerator SkipTimer_GameOverLife(float timer, float totalScore)
    {
        managerRacing.RacingSpeed = 0;
        managerRacing.GameStarted = false;
        audioHandler.StopCarSound();
        GameplayGUIGameObject.SetActive(false);
        GameOverUILife.SetActive(true);
        yield return new WaitForSeconds(timer);
    }
    IEnumerator SkipTimer_GameOverTime(float timer, float totalScore)
    {
        managerRacing.RacingSpeed = 0;
        managerRacing.GameStarted = false;
        audioHandler.StopCarSound();
        GameplayGUIGameObject.SetActive(false);
        GameOverUITImer.SetActive(true);
        yield return new WaitForSeconds(timer);
    }

    void OnGameTimer(int gameTimer)
	{
		InitTimer(gameTimer);
	}

    void OnPlayerToken(float tokenScore)
    {
        InitToken(tokenScore);
    }

    void OnPlayerLife(int life)
	{
		InitLife(life);
	}

	void OnPlayerScore(float score)
	{
		InitScore(score);
	}

    public void OnGameRestart()
    {
        managerRacing.InitGame();
        SceneManager.LoadScene("Game");
    }

    public void OnGamestart()
    {
        UIHTPAnimator.Play("GameStartClose", 0, 0);
    }
    IEnumerator StartAnimationDelay()
    {
        yield return new WaitForSeconds(1f);
        UIHTPObj.SetActive(false);
        int tutorial = PlayerPrefs.GetInt("controller_tutorial", 0);
        Debug.Log("TUTORIAL BEFORE: " + tutorial);
        if (tutorial == 0)
        {
            TutorialParent.SetActive(true);
            //see if game played on keyboard or swipe
            switch (isUsingKeyboard)
            {
                case 0:
                    TutorialSwipeObj.SetActive(true);
                    break;
                case 1:
                    TutorialKeyboardObj.SetActive(true);
                    break;
            }
            PlayerPrefs.SetInt("controller_tutorial", 1);
        }
        else
        {
          
            StartGameplay();
        }
    }

    public void OnGameStartGetTutorialKey()
    {
        int startingguide = PlayerPrefs.GetInt("startingguide", 0);

        if (startingguide == 1)
        {
            StartGameplay();
            IntroStartedup.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("startingguide", 1);
            IntroStartedup.SetActive(true);
        }
    }

    public void OnGameStartFromTutor()
    {
       // PlayerPrefs.SetInt("startingguide", 1);
        StartGameplay();
    }
}
