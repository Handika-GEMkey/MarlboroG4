using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomizeRankContent : MonoBehaviour
{
    [SerializeField] private Text userRankIndex;
    [SerializeField] private Image userProfile;
    [SerializeField] private Text userName;
    [SerializeField] private Text userScore;
    [SerializeField] private GameObject loadingBar;
    [SerializeField] private Button reloadPhoto;
    [SerializeField] private GameObject crown;

    public void RequestPhoto(string url)
    {
        reloadPhoto.gameObject.SetActive(false);
        loadingBar.SetActive(true);
        if (ManagerHttpRequest.Instance)
        {
            ManagerHttpRequest.Instance.GetUserPoster(url, CallBackRequestPhoto);
        }
    }

    private void CallBackRequestPhoto(bool status, Sprite sprite, string message)
    {
        if (loadingBar != null)
        {
            loadingBar.SetActive(false);
        }
        if (status)
        {
            SetPhoto(sprite);
        }
        else
        {
            if (reloadPhoto != null) { reloadPhoto.gameObject.SetActive(true); }
            reloadPhoto.onClick.AddListener(()=> ReloadImage(message));
        }
    }

    private void ReloadImage(string url)
    {
        ManagerHttpRequest.Instance.GetUserPoster(url, CallBackRequestPhoto);
        if (loadingBar != null) { loadingBar.SetActive(true); }
        if (reloadPhoto != null) { reloadPhoto.gameObject.SetActive(false); }
    }

    public void SetRank(int userRankIndex, string userName, int userScore)
    {
        if(this.userRankIndex != null) this.userRankIndex.text = userRankIndex.ToString();
        if (crown != null && userRankIndex == 1)
        {
            crown.gameObject.SetActive(true);
        }
        this.userName.text = userName;
        this.userScore.text = userScore.ToString("#,#");
    }

    public void SetPhoto(Sprite userProfile)
    {
        if (this.userProfile != null)
        {
            this.userProfile.sprite = userProfile;
        }
    }
}
