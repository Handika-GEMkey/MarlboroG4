using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraTesting : MonoBehaviour {

	public RawImage RawImage;

	private WebCamTexture webCam;

	void Start()
	{
		webCam = new WebCamTexture();
		RawImage.texture = webCam;
	}

	public void AccessAndroidCamera()
	{
		StartCoroutine(RequestCamera());
	}

	public void AccessIOSCamera()
	{
		if (webCam.isPlaying) webCam.Stop();
		webCam.Play();
		ManagerApplication.Instance.CustomeToast.CallToast("It Suppost to be active on IOS!");
	}

	public IEnumerator RequestCamera()
	{
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

		if (Application.HasUserAuthorization(UserAuthorization.WebCam))
		{
			ManagerApplication.Instance.CustomeToast.CallToast("Access Allowed!");
		}
		else
		{
			ManagerApplication.Instance.CustomeToast.CallToast("Access Denied on IOS!");
			yield break;
		}

		if (webCam.isPlaying) webCam.Stop();
		webCam.Play();
		ManagerApplication.Instance.CustomeToast.CallToast("It Suppost to be active on Android or PC!");
	}
}
