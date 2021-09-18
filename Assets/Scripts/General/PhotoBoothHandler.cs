using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PhotoBoothHandler : MonoBehaviour {

	[Header("Capture Components")]
	public RectTransform CaptureRectTransform;
	public GameObject[] FlashGameObject;
	public Image PhotoPreview;

	[Header("PhotoBooth Components")]
	public bool PhotoTest;
	public RawImage Destination;
	public AspectRatioFitter fit;
	public GameObject PhotoSession;
	public GameObject BeforeTakePhoto;
	public GameObject AfterTakePhoto;

	private Texture2D captureTexture;
	private int cameraIndex = 0;

	private bool camAvailable;
	private WebCamTexture webCamTexture;
	private List<WebCamDevice> webCamDevices;

	void Awake()
	{
		this.webCamDevices = new List<WebCamDevice>();
		this.webCamTexture = new WebCamTexture();
	}

	void Start()
	{
		this.Destination.texture = this.webCamTexture;

		if (this.PhotoTest)
		{
			this.AutorizeCamera();
		}

		ManagerHttpRequest.Instance.OnCallBackUploadPhoto += Instance_OnCallBackCreateGame;
	}

	public void ReCapturePhoto()
	{
		BeforeTakePhoto.SetActive(true);
		AfterTakePhoto.SetActive(false);
		PhotoPreview.gameObject.SetActive(false);
		EnableCamera();
	}

	public void EnableCamera()
	{
		if (this.webCamTexture.isPlaying)
		{
			this.webCamTexture.Stop();
		}

		this.webCamTexture.deviceName = this.webCamDevices[cameraIndex].name;
		this.webCamTexture.Play();
		this.Destination.texture = this.webCamTexture;
	}


	public void ChangeCameraDevice()
	{
		if (this.webCamDevices.Count < 2)
		{
			ManagerApplication.Instance.CustomeToast.CallToast("Hanya ada satu kamera!");
			return;
		}

		DisableCamera();

		if (cameraIndex == 0) cameraIndex = 1;
		else cameraIndex = 0;

		this.webCamTexture.deviceName = this.webCamDevices[cameraIndex].name;
		this.webCamTexture.Play();
		this.Destination.texture = this.webCamTexture;
	}

	public void AutorizeCamera()
	{
		if(gameObject.activeInHierarchy) StartCoroutine(StartCamera());
	}

	private IEnumerator StartCamera()
	{
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

		if (Application.HasUserAuthorization(UserAuthorization.WebCam))
		{
			this.webCamDevices.Clear();

			for (var x = 0; x < WebCamTexture.devices.Length; x++)
			{
				this.webCamDevices.Add(WebCamTexture.devices[x]);
			}

			if (this.webCamDevices.Count < 1)
			{
				ManagerApplication.Instance.CustomeToast.CallToast("No camera detected!");
				yield break;
			}

			EnableCamera();

			camAvailable = true;
		}
	}

	public void DisableCamera()
	{
		if (this.webCamTexture.isPlaying)
		{
			this.webCamTexture.Stop();
		}
	}

	public void CapturePhoto()
	{
		StartCoroutine(CaptureCoroutine(CaptureRectTransform, FlashGameObject));
	}

	private IEnumerator CaptureCoroutine(RectTransform rectTransform, GameObject[] hideGameObjects)
	{
		foreach (GameObject go in hideGameObjects) { go.SetActive(false); }
		yield return new WaitForEndOfFrame(); // Important, wait till all the frame done rendering.

		var rectCorners = new Vector3[4];
		rectTransform.GetWorldCorners(rectCorners);

		var height = rectCorners[1].y - rectCorners[0].y;
		var width = rectCorners[3].x - rectCorners[0].x;

		var start = new Vector2(rectCorners[0].x, rectCorners[0].y);

		this.captureTexture = new Texture2D(Mathf.CeilToInt(width), Mathf.CeilToInt(height), TextureFormat.RGB24, false);
		var rect = new Rect(start.x, start.y, width, height);

		this.captureTexture.ReadPixels(rect, 0, 0);
		this.captureTexture.Apply();

		Sprite newImage = Sprite.Create(captureTexture, new Rect(0, 0, captureTexture.width, captureTexture.height), Vector2.one / 2);
		PhotoPreview.sprite = newImage;

		ManagerPlayer.Instance.VisualNormalPhoto = newImage;
		ManagerPlayer.Instance.NormalPhotoBytes = captureTexture.EncodeToJPG();

		foreach (GameObject go in hideGameObjects) { go.SetActive(true); }
		yield return new WaitForEndOfFrame();

		BeforeTakePhoto.SetActive(false);
		AfterTakePhoto.SetActive(true);
		PhotoPreview.gameObject.SetActive(true);
		DisableCamera();
	}

	public void SaveAndContinue()
	{
		ManagerApplication.Instance.LoadingScreen.SetActive(true);
		if (ManagerHttpRequest.Instance)
		{
			ManagerHttpRequest.Instance.UploadOriginalPhoto();
		};
	}

	private void Instance_OnCallBackCreateGame(bool status, string obj)
	{
		if (status)
		{
			ManagerApplication.Instance.CustomeToast.CallToast(obj);
			ManagerApplication.Instance.SceneChange("Game");
		}
		else
		{
			ManagerApplication.Instance.CustomeToast.CallToast(obj);
		}

		ManagerApplication.Instance.LoadingScreen.SetActive(false);
	}

	void Update()
	{
		if (!camAvailable) return;

		float ratio = (float)this.webCamTexture.width / (float)this.webCamTexture.height;
		fit.aspectRatio = ratio;

		float scaleY = this.webCamTexture.videoVerticallyMirrored ? -1 : 1;
		Destination.rectTransform.localScale = new Vector3(Destination.rectTransform.localScale.x, scaleY, 1f);

		int orient = -this.webCamTexture.videoRotationAngle;
		Destination.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
	}
}
