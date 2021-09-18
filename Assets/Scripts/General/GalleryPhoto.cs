using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GalleryPhoto : MonoBehaviour {

    [SerializeField] private string userPhone;
    [SerializeField] private Image userProfile;
    [SerializeField] private GameObject loadingBar;
    [SerializeField] private Button reloadPhoto;

    private UnityAction<PlayerData> actionPhotoClicked;

    public void RequestPhoto(string url, PlayerData playerData, UnityAction<PlayerData> action)
    {
        reloadPhoto.gameObject.SetActive(false);
        loadingBar.SetActive(true);
        actionPhotoClicked = action;
        if (ManagerHttpRequest.Instance)
        {
            ManagerHttpRequest.Instance.GetUserPoster(url, CallBackRequestPhoto, playerData);
        }
    }

    private void CallBackRequestPhoto(bool status, Sprite sprite, string message, PlayerData playerData)
    {
        if (loadingBar != null)
        {
            loadingBar.SetActive(false);
        }
        if (status)
        {
            SetPhoto(sprite, playerData);
        }
        else
        {
            if (reloadPhoto != null) { reloadPhoto.gameObject.SetActive(true); }
            reloadPhoto.onClick.AddListener(() => ReloadImage(message, playerData));
        }
    }

    private void ReloadImage(string url, PlayerData playerData)
    {
        ManagerHttpRequest.Instance.GetUserPoster(url, CallBackRequestPhoto, playerData);
        if (loadingBar != null) { loadingBar.SetActive(true); }
        if (reloadPhoto != null) { reloadPhoto.gameObject.SetActive(false); }
    }

    public void SetPhoto(Sprite userProfile, PlayerData playerData)
    {
        if (this.userProfile != null)
        {
            this.gameObject.GetComponent<Button>().interactable = true;
            this.gameObject.GetComponent<Button>().onClick.AddListener(()=> OnPhotoClicked(playerData));
            this.userProfile.sprite = userProfile;
        }
    }

    private void OnPhotoClicked(PlayerData playerData)
    {
        if(actionPhotoClicked != null)
        {
            actionPhotoClicked.Invoke(playerData);
        }
    }
}
