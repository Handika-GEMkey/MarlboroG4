using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class ManagerRacing : MonoBehaviour
{

    public static ManagerRacing Instance;

    [SerializeField] private GameplayGUI gameplayGUI;

    [SerializeField]
    private WebGLBridger WebBridger;
    public bool GameStarted;
    public ObjectiveGenerator ObjectiveGenerator;

    public int CarCode;
    public bool IsPopupPointOpen;
    public bool InDebugMode;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Vector3[] linePositions;

    [Range(0.2f, 5f)]
    [SerializeField] private float racingSpeed;

    [SerializeField] private float totalScore;

    [SerializeField] private float tokenScore;

    [SerializeField] private int totalLife;

    [SerializeField] private int gameTimer;
    [SerializeField] private float maxToken;
    [SerializeField] private int maxTimer;
    [SerializeField]
    [Range(10f, 20f)]
    private int obsDistance;
    

    private event Action<float> callbackGameOverLife;
    public event Action<float> CallbackGameOverLife
    {
        add
        {
            this.callbackGameOverLife -= value;
            this.callbackGameOverLife += value;
        }
        remove
        {
            this.callbackGameOverLife -= value;
        }
    }
    private event Action<float> callbackGameOverTime;
    public event Action<float> CallbackGameOverTime
    {
        add
        {
            this.callbackGameOverTime -= value;
            this.callbackGameOverTime += value;
        }
        remove
        {
            this.callbackGameOverTime -= value;
        }
    }
    private event Action<float> callbackGameFinish;
    public event Action<float> CallbackGameFinish
    {
        add
        {
            this.callbackGameFinish -= value;
            this.callbackGameFinish += value;
        }
        remove
        {
            this.callbackGameFinish -= value;
        }
    }

    private event Action<int> callBackGameTimer;
    public event Action<int> CallBackGameTimer
    {
        add
        {
            this.callBackGameTimer -= value;
            this.callBackGameTimer += value;
        }
        remove
        {
            this.callBackGameTimer -= value;
        }
    }

    private event Action<int> callbackPlayerLife;
    public event Action<int> CallbackPlayerLife
    {
        add
        {
            this.callbackPlayerLife -= value;
            this.callbackPlayerLife += value;
        }
        remove
        {
            this.callbackPlayerLife -= value;
        }
    }

    private event Action<float> callbackPlayerScore;
    public event Action<float> CallbackPlayerScore
    {
        add
        {
            this.callbackPlayerScore -= value;
            this.callbackPlayerScore += value;
        }
        remove
        {
            this.callbackPlayerScore -= value;
        }
    }

    private event Action<float> callbackTokenScore;
    public event Action<float> CallbackTokenScore
    {
        add
        {
            this.callbackTokenScore -= value;
            this.callbackTokenScore += value;
        }
        remove
        {
            this.callbackTokenScore -= value;
        }
    }

    private void Start()
    {
        //testingpurpose
        if (InDebugMode)
        {
            PlayerPrefs.SetInt("startingguide", 0);
            PlayerPrefs.SetInt("controller_tutorial", 0);
        }
        InitGame();
    }

    public void InitGame()
    {
        StartCoroutine(delay());
    }

    void PrepareStartingGameUI()
    {
        gameplayGUI.OnGameStartGetTutorialKey();
    }
   
    IEnumerator delay()
    {
        yield return new WaitForSeconds(2);
        CarCode = WebBridger.CarCode;
        var popupPointCode = WebBridger.PointStatus;
        var tutorialKey = WebBridger.TutorialKey;

        if (popupPointCode < 1) IsPopupPointOpen = false;
        else IsPopupPointOpen = true;

        playerController.InitiateCar(CarCode);
        gameplayGUI.InitTutorial(tutorialKey);
        gameplayGUI.BlockingUI.SetActive(false);

        PrepareStartingGameUI();
    }

    public float TotalScore
    {
        get
        {
            return totalScore;
        }
        set
        {
            totalScore = value;
            if (callbackPlayerScore != null) { callbackPlayerScore.Invoke(totalScore); }
            if (totalScore % 100 == 0 && racingSpeed < 1f)
            {
                racingSpeed += 0.02f;
            }
        }
    }

    public float TokenScore
    {
        get
        {
            return tokenScore;
        }
        set
        {
            tokenScore = value;
            if (tokenScore >= maxToken)
            {
                if (callbackGameFinish != null) callbackGameFinish.Invoke(totalScore);
            }
            else if (tokenScore < maxToken)
            {
                if (callbackTokenScore != null) { callbackTokenScore.Invoke(tokenScore); }
            }

        }
    }

    public int TotalLife
    {
        get
        {
            return totalLife;
        }
        set
        {
            totalLife = value;
            if (callbackPlayerLife != null) { callbackPlayerLife.Invoke(totalLife); }
            if (totalLife < 1)
            {
                if (callbackGameOverLife != null) callbackGameOverLife.Invoke(totalScore);
            }
        }
    }

    public int ObsDistance
    {
        get { return obsDistance; }
    }

    public Vector3[] GetLinePositions() { return linePositions; }
    public float RacingSpeed
    {
        set
        {
            racingSpeed = value;
        }
        get
        {
            return racingSpeed;
        }
    }

    void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        
        GameStarted = true;
        gameTimer = 60;
        ObjectiveGenerator.BatchGenerator();
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        while (GameStarted)
        {
            yield return new WaitForSeconds(1f);
            gameTimer -= 1;
            if (gameTimer < 1)
            {

                if (callbackGameOverTime != null) callbackGameOverTime.Invoke(totalScore);

            }
            if (callBackGameTimer != null)
            {
                callBackGameTimer.Invoke(gameTimer);
            }
        }
    }
    /// <summary>
    /// Add Time from fuel
    /// </summary>
    public void AddingTime(int v)
    {
        gameTimer += v;
        if (gameTimer > maxTimer) { gameTimer = maxTimer; }
        if (callBackGameTimer != null)
        {
            callBackGameTimer.Invoke(gameTimer);
        }
    }

   /* public void ExitGame()
    {
        PlayerPrefs.SetInt("startingguide", 0);
        WebBridger.Exit();
    }*/
}