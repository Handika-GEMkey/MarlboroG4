using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardAndGallery : MonoBehaviour {

	public GameObject MenuLeaderboard;
	public LeaderboardHandler LeaderboardHandler;
	public GalleryHandler GalleryHandler;

	void Start()
	{
		if (ManagerApplication.Instance.ExtendedMenuKey == "Gallery")
		{
			LeaderboardHandler.gameObject.SetActive(false);
			GalleryHandler.gameObject.SetActive(true);
			GalleryHandler.OnGalleryOpen();
		}
		else
		{
			LeaderboardHandler.gameObject.SetActive(true);
			GalleryHandler.gameObject.SetActive(false);
			LeaderboardHandler.OnLeaderboardOpen();
		}
	}

	public void BackButton()
	{
		if (ManagerApplication.Instance != null)
		{
			ManagerApplication.Instance.SceneChange("MainMenu");
		}
	}
}
