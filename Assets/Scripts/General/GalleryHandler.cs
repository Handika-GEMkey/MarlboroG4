using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GalleryHandler : MonoBehaviour {

	[Header("General Content")]
	public LeaderboardAndGallery LeaderboardAndGallery;

	[Header("Content Confirmation")]
	public GameObject DownloadConfirmationGameObject;
	public Text UserNameText;
	public InputField HandphoneField;
	public Button CloseButton;
	public Button SubmitButton;
	public string TempHandphoneNumber;
	public string SuccessMessage;
	public string FailedMessage1;
	public string FailedMessage2;

	[Header("Search Field Requirements")]
	public InputField SearchField;
	public Button ClearSearchField;

	[Header("Leaderboar Component List")]
	public GameObject GalleryPhotoPrefab;
	public Transform RectGalleryPhoto;
	public GameObject EmptyListViewGameObject;
	public GameObject LoadMoreGameObject;

	public int GalleryPageIndex = 1;
	public PlayerData TemporarlyPlayerData;

	[DllImport("__Internal")]
	private static extern void openURL(string _url);

	public void OnGalleryOpen()
	{
		LeaderboardAndGallery.MenuLeaderboard.SetActive(false);
		CloseButton.onClick.AddListener(CloseDownloadConfirmation);
		SubmitButton.onClick.AddListener(OnSubmitConfirmation);
		SearchField.onValueChanged.AddListener(OnSearchValueChange);
		ClearSearchField.onClick.AddListener(OnClearSearchField);
		//ShowAllData();
		HandphoneField.text = "";

		ManagerHttpRequest.Instance.GetAllTimeLeaderboard(GalleryPageIndex, LeaderboardType.ALL_TIME);
		ManagerHttpRequest.Instance.OnCallBackGetMultipleData += Instance_OnCallBackGetMultipleData;
	}

	private void Instance_OnCallBackGetMultipleData(bool arg1, List<PlayerData> arg2, int arg3, string arg4)
	{
		List<UserLeaderboard> TempGenericRanks = new List<UserLeaderboard>();
		if (arg1)
		{
			if (EmptyListViewGameObject != null && LoadMoreGameObject) { EmptyListViewGameObject.SetActive(false); LoadMoreGameObject.SetActive(false); }
			TempGenericRanks.Add(new UserLeaderboard(arg3, arg2));
			if (TempGenericRanks.Count > 0)
			{
				if (arg3 == 1) { ClearData(); }
				foreach (var data in TempGenericRanks[0].UserData)
				{
					GameObject newUser = Instantiate(GalleryPhotoPrefab, RectGalleryPhoto);
					newUser.GetComponent<GalleryPhoto>().RequestPhoto(data.PhotoEditedURL, data, OpenDownlaodableContent);
				}
			}
		}
		else
		{
			if (EmptyListViewGameObject != null && LoadMoreGameObject) { EmptyListViewGameObject.SetActive(true); LoadMoreGameObject.SetActive(true); }
		}

		if (TempGenericRanks[0].UserData.Count < 1)
		{
			if (EmptyListViewGameObject != null && LoadMoreGameObject) { EmptyListViewGameObject.SetActive(true); LoadMoreGameObject.SetActive(true); }
		}
		else
		{
			if (EmptyListViewGameObject != null && LoadMoreGameObject) { EmptyListViewGameObject.SetActive(false); LoadMoreGameObject.SetActive(false); }
		}

		ManagerApplication.Instance.LoadingScreen.SetActive(false);
	}

	void ClearData()
	{
		if (RectGalleryPhoto != null)
		{
			foreach (Transform tf in RectGalleryPhoto)
			{
				Destroy(tf.gameObject);
			}
		}
	}

	void OnSearchValueChange(string message)
	{
		UserSearching(message);

		if (message.Length > 0)
		{
			ClearSearchField.gameObject.SetActive(true);
		}
		else
		{
			ClearSearchField.gameObject.SetActive(false);
		}
	}

	void OnClearSearchField()
	{
		SearchField.text = "";
	}

	void UserSearching(string name)
	{
		ManagerApplication.Instance.LoadingScreen.SetActive(true);
		ManagerHttpRequest.Instance.GetUserByName(name, OnRequestUserByName, LeaderboardType.ALL_TIME);
	}

	void OnRequestUserByName(bool status, List<PlayerData> userDataList, string message)
	{
		ClearData();
		ManagerApplication.Instance.LoadingScreen.SetActive(false);
		if (userDataList.Count > 0)
		{
			if (EmptyListViewGameObject != null && LoadMoreGameObject) { EmptyListViewGameObject.SetActive(false); LoadMoreGameObject.SetActive(false); }
			foreach (var data in userDataList)
			{
				GameObject newUser = Instantiate(GalleryPhotoPrefab, RectGalleryPhoto);
				newUser.GetComponent<GalleryPhoto>().RequestPhoto(data.PhotoEditedURL, data, OpenDownlaodableContent);
			}
		}
		else
		{
			if (EmptyListViewGameObject != null && LoadMoreGameObject) { EmptyListViewGameObject.SetActive(true); LoadMoreGameObject.SetActive(false); }
		}
	}

	public void LoadMore()
	{
		GalleryPageIndex += 1;
		ManagerHttpRequest.Instance.GetAllTimeLeaderboard(GalleryPageIndex, LeaderboardType.ALL_TIME);
		ManagerApplication.Instance.LoadingScreen.SetActive(true);
	}

	public void CloseDownloadConfirmation()
	{
		DownloadConfirmationGameObject.SetActive(false);
	}

	private void CallbackDownloadLink(bool status, string message)
	{
		if (status)
		{
			CloseDownloadConfirmation();

#if UNITY_EDITOR
			try
			{
				Application.OpenURL(message);
			}
			catch (Exception error) { }
#endif

#if UNITY_WEBGL
			try
			{
				openURL(message);
			}
			catch (Exception error) { }
#endif
		}
		else
		{
			ManagerApplication.Instance.CustomeToast.CallToast("Check your internet connection!");
		}
		ManagerApplication.Instance.LoadingScreen.SetActive(false);
	}

	private void OnSubmitConfirmation()
	{
		string handphoneNumber = HandphoneField.text;
		if (handphoneNumber.Length < 1)
		{
			ManagerApplication.Instance.CustomeToast.CallToast(FailedMessage1);
			return;
		}

		if (handphoneNumber.Equals(TempHandphoneNumber))
		{
			ManagerApplication.Instance.CustomeToast.CallToast(SuccessMessage);
			ManagerApplication.Instance.LoadingScreen.SetActive(true);
			ManagerHttpRequest.Instance.GetDownloadLink(TemporarlyPlayerData, CallbackDownloadLink);
		}
		else
		{
			ManagerApplication.Instance.CustomeToast.CallToast(FailedMessage2);
		}
	}

	private void OpenDownlaodableContent(PlayerData data)
	{
		DownloadConfirmationGameObject.SetActive(true);
		UserNameText.text = data.Name;
		TempHandphoneNumber = data.Phone;
		TemporarlyPlayerData = data;
	}
}
