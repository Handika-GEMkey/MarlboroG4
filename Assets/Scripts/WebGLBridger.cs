using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebGLBridger : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OnSubmitScore(int score);

    [DllImport("__Internal")]
    private static extern void OnGameQuit1();

    [DllImport("__Internal")]
    private static extern void OnGameQuit2();

    [DllImport("__Internal")]
    private static extern void OnGameQuit3();

    [DllImport("__Internal")]
    private static extern void OnPlay();

    [DllImport("__Internal")]
    private static extern void OnGameStart();

    [DllImport("__Internal")]
    private static extern void OnPlayAgain();

    [DllImport("__Internal")]
    private static extern void OnShare();

    public bool isDebugging = false;
    public int CarCode;
    public int PointStatus;
    public int TutorialKey;

    private void OnEnable()
    {
        if (isDebugging)
        {
            ResetPlayerPref();
        }
        GameStart();
    }

    public void SubmitScore(int score)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    OnSubmitScore(score);
#endif
    }

    public void GameQuit1()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    OnGameQuit1();
#endif
    }

    public void GameQuit2()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    OnGameQuit2();
#endif
    }

    public void GameQuit3()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    OnGameQuit3();
#endif
    }

    public void Play()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    OnPlay();
#endif
    }

    public void SetPoint(int point)
    {
        PointStatus = point;
    }

    public void SetCar(int carCode)
    {
        CarCode = carCode;
    }

    public void SetTutorial(int tutorialKey)
    {
        TutorialKey = tutorialKey;
    }

    public void ResetPlayerPref()
    {
        PlayerPrefs.SetInt("controller_tutorial", 0);
        PlayerPrefs.SetInt("startingguide", 0);
    }

    public void GameStart()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    OnGameStart();
#endif
    }

    public void PlayAgain()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    OnPlayAgain();
#endif
    }

    public void Share()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    OnShare();
#endif
    }
}
