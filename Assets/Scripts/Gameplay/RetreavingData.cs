using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RetreavingData : MonoBehaviour {

    public GameplayGUI gameplayGUI;
   

    public bool isDebug;

    [DllImport("__Internal")]
    private static extern void RecheiveData(string _game_object, string _id, string _name, string _score);

    [DllImport("__Internal")]
    private static extern void SendData(string _id, string _name, int _score);

    public string ID;
    public string NAME;
    public int SCORE;

    void Start()
    {
        if (isDebug)
        {
            StartTheGame();
        }
    }

    public void GetId(string value)
    {
        ID = value;
    }

    public void GetName(string value)
    {
        NAME = value;
    }

    public void GetScore(int value)
    {
        SCORE = value;
    }

    public void GetData()
    {
#if UNITY_WEBGL
     //   RecheiveData(this.gameObject.name, "GetId", "GetName", "GetScore");
#endif
    }


    public void StartTheGame()
    {
        gameplayGUI.StartGameplay();
    }

    public void SendData()
    {
#if UNITY_WEBGL
   //     SendData(ID, NAME, SCORE);
#endif
    }

    public void SetPoint()
    {

    }
}
