using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ResultPageHandler : MonoBehaviour {

	public bool functionTest;

	[Header("Screenshoot Components")]
	[SerializeField] private Mask posterMasking;
	[SerializeField] private RectTransform screenShootPosition;

	[Header("Poster Components")]
	[SerializeField] private Text nameText;
	[SerializeField] private Text scoreText;
	[SerializeField] private Image originalPhotoImage;
	[SerializeField] private Image previewPhoto;
	[SerializeField] private Transform headTransform;
	[SerializeField] private Vector2[] headPositions;
	[SerializeField] private Image backgroundPhoto;
	[SerializeField] private Sprite[] backgrounds;

	Texture2D captureTexture;

	[DllImport("__Internal")]
	private static extern void openURL(string _url);

	void Start()
	{
		if (functionTest)
		{
			previewPhoto.gameObject.SetActive(true);
		}

		if (backgroundPhoto != null)
		{
			if (ManagerPlayer.Instance.GenderChoosen == 1)
			{
				backgroundPhoto.sprite = backgrounds[0];
				headTransform.localPosition = headPositions[0];
			}
			else
			{
				backgroundPhoto.sprite = backgrounds[1];
				headTransform.localPosition = headPositions[1];
			}
		}

		if (ManagerHttpRequest.Instance)
		{
			ManagerHttpRequest.Instance.OnCallBackSendScore += Instance_OnCallBackSendScore;
		}
		InitPosterComponents();

		StartCoroutine(DelayCreatePhoto());
	}

	IEnumerator DelayCreatePhoto()
	{
		yield return new WaitForSeconds(2);
		CreatePoster();
	}

	private void Instance_OnCallBackSendScore(bool status)
	{
		if (status)
		{
			if (ManagerApplication.Instance) { ManagerApplication.Instance.CustomeToast.CallToast("Upload failed, Please check your internet connection!"); }
		}
		else
		{
			if (ManagerApplication.Instance) { ManagerApplication.Instance.CustomeToast.CallToast("Score Saved!"); }
		}
		ManagerApplication.Instance.LoadingScreen.SetActive(false);
	}

	public void CreatePoster()
	{
		StartCoroutine(CreatePosterCoroutine(screenShootPosition));
	}

	private IEnumerator CreatePosterCoroutine(RectTransform rectTransform)
	{
		posterMasking.enabled = false;
		yield return new WaitForEndOfFrame();

		var rectCorners = new Vector3[4];
		rectTransform.GetWorldCorners(rectCorners);

		var width = rectCorners[3].x - rectCorners[0].x;
		var height = rectCorners[1].y - rectCorners[0].y;

		var start = new Vector2(rectCorners[0].x, rectCorners[0].y);

		this.captureTexture = new Texture2D(Mathf.CeilToInt(width), Mathf.CeilToInt(height), TextureFormat.RGB24, false);
		var rect = new Rect(start.x, start.y, width, height);
		this.captureTexture.ReadPixels(rect, 0, 0);
		this.captureTexture.Apply();

		byte[] data = captureTexture.EncodeToJPG();

		Sprite newImage = Sprite.Create(captureTexture, new Rect(0, 0, captureTexture.width, captureTexture.height), Vector2.one / 2);

		previewPhoto.sprite = newImage;

		if (ManagerPlayer.Instance)
		{
			ManagerPlayer.Instance.EditedPhotoBytes = data;
		}

		posterMasking.enabled = true;
		ManagerApplication.Instance.LoadingScreen.SetActive(true);

		var managerPlayer = ManagerPlayer.Instance;

		StartCoroutine(ManagerHttpRequest.Instance.UploadScore(managerPlayer.UserID, managerPlayer.ScoreData, managerPlayer.EditedPhotoBytes));
	}

	public void InitPosterComponents()
	{
		var playerData = ManagerPlayer.Instance;
		if (playerData)
		{
			nameText.text = playerData.UserName.ToUpper();
			scoreText.text = String.Format("{0:n0}", playerData.ScoreData);
			originalPhotoImage.sprite = playerData.VisualNormalPhoto;
		}
	}

	private void CallbackDownloadLink(bool status, string message)
	{
		if (status)
		{

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

	public void DownloadImage()
	{
		ManagerApplication.Instance.LoadingScreen.SetActive(true);
		PlayerData playerData = new PlayerData();
		playerData.UID = ManagerPlayer.Instance.UserID;
		playerData.Phone = ManagerPlayer.Instance.TelphoneNumber;
		ManagerHttpRequest.Instance.GetDownloadLink(playerData, CallbackDownloadLink);
	}

	public void ToMainLagi()
	{
		if (ManagerApplication.Instance)
		{
			ManagerApplication.Instance.SceneChange("Game");
		}
	}

	public void ToLeaderboard()
	{
		if (ManagerApplication.Instance)
		{
			ManagerApplication.Instance.SceneChange("ExtendedMenu/Leaderboard");
		}
	}

	void Update()
	{
		if (functionTest)
		{
			if (Input.GetMouseButtonDown(0))
			{
				CreatePoster();
			}
		}
	}
}
