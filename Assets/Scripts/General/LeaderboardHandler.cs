using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardHandler : MonoBehaviour
{
	[Header("General Components")]
	public LeaderboardAndGallery LeaderboardAndGallery;

	[Header("GameObject Requirements")]
	[SerializeField] private GameObject RectTopRank;
	[SerializeField] private GameObject TopRankContainer;
	[SerializeField] private List<Transform> TopRankPositions;
	[SerializeField] private Transform GenericRankPosition;

	[Header("Search Field Requirements")]
	public InputField SearchField;
	public Button ClearSearchField;
	public LeaderboardType LeaderboardType;
	public int typeCode = 1;

	[Header("Leaderboar Component List")]
	public GameObject RegularRankPrefab;
	public Transform RectLeaderboardTransform;
	public GameObject EmptyListViewGameObject;
	public bool TestLoad;
	public int LeaderboardPageIndex = 1;

	[SerializeField] private GameObject TopRankGameHandler;

	void Start()
	{

		TopRankGameHandler = RectTopRank.transform.GetChild(1).gameObject;

		int rankIndex = 1;
		foreach (Transform tr in TopRankContainer.transform)
		{
			if (tr.gameObject.CompareTag("rank_" + rankIndex))
			{
				TopRankPositions.Add(tr);
				rankIndex++;
			}
		}

		if (TestLoad)
		{
			ManagerHttpRequest.Instance.GetAllTimeLeaderboard(LeaderboardPageIndex, this.LeaderboardType);
		}

		ManagerHttpRequest.Instance.OnCallBackGetMultipleData += Instance_OnCallBackGetMultipleData;
	}

	public void OnLeaderboardOpen()
	{
		SearchField.onValueChanged.AddListener(OnSearchValueChange);
		ClearSearchField.onClick.AddListener(OnClearSearchField);

		ShowAllData();
	}

	private void SetListTopRank(List<PlayerData> data)
	{
		List<PlayerData> TempTopRank = new List<PlayerData>();
		
		if (data.Count > 0)
		{
			if(TopRankGameHandler) TopRankGameHandler.SetActive(true);
			if (data.Count > 3)
			{
				for (var c = 0; c < 3; c++)
				{
					TempTopRank.Add(data[c]);
				}
			}
			else if (data.Count == 2)
			{
				TempTopRank.Add(data[0]);
				TempTopRank.Add(data[1]);
			}
			else if (data.Count == 1)
			{
				TempTopRank.Add(data[0]);
			}
		}
		else
		{
			if (TopRankGameHandler) TopRankGameHandler.SetActive(false);
		}

		if(TempTopRank.Count > 0)
		{
			int index = 0;

			foreach (var userRank in TempTopRank)
			{
				int rank;
				int score;

				int.TryParse(userRank.Rank, out rank);
				int.TryParse(userRank.Score, out score);

				if (TopRankPositions[index])
				{
					foreach (Transform tr in TopRankPositions[index])
					{
						Destroy(tr.gameObject);
					}
				}

				GameObject newRank = Instantiate(ManagerExtendedScene.Instance.TopRankPrefab, TopRankPositions[index]);
				var rankContent = newRank.GetComponent<CustomizeRankContent>();

				rankContent.SetRank(rank, userRank.Name, score);
				rankContent.GetComponent<CustomizeRankContent>().RequestPhoto(userRank.PhotoEditedURL);

				index++;
			}
		}
	}

	private void SetListGenericRank(List<PlayerData> data, int rankStart)
	{
		for (var c = rankStart; c < data.Count; c++)
		{
			int rank;
			int score;

			int.TryParse(data[c].Rank, out rank);
			int.TryParse(data[c].Score, out score);

			GameObject newRank = Instantiate(ManagerExtendedScene.Instance.GenericRankPrefab, GenericRankPosition);
			var rankContent = newRank.GetComponent<CustomizeRankContent>();

			rankContent.SetRank(rank, data[c].Name, score);
			rankContent.GetComponent<CustomizeRankContent>().RequestPhoto(data[c].PhotoEditedURL);
		}

		if (GenericRankPosition)
		{
			if (GenericRankPosition.transform.childCount < 1)
			{
				if (EmptyListViewGameObject != null) { EmptyListViewGameObject.SetActive(true); }
			}
			else
			{
				if (EmptyListViewGameObject != null) { EmptyListViewGameObject.SetActive(false); }
			}
		}
	}

	private void Instance_OnCallBackGetMultipleData(bool arg1, List<PlayerData> arg2, int page, string message)
	{
		List<UserLeaderboard> TempGenericRanks = new List<UserLeaderboard>();
		if (arg1)
		{
			TempGenericRanks.Add(new UserLeaderboard(page, arg2));
			if (TempGenericRanks.Count > 0)
			{
				if (page == 1) { ClearData(); }
				if (page == 1) { SetListTopRank(TempGenericRanks[0].UserData); SetListGenericRank(TempGenericRanks[0].UserData, 3); }
				else
				{
					SetListGenericRank(TempGenericRanks[0].UserData, 0);
				}
			}
		}
		
		ManagerApplication.Instance.LoadingScreen.SetActive(false);
	}

	public void LoadMore()
	{
		LeaderboardPageIndex += 1;
		ManagerApplication.Instance.LoadingScreen.SetActive(true);
		ManagerHttpRequest.Instance.GetAllTimeLeaderboard(LeaderboardPageIndex, this.LeaderboardType);
	}

	void UserSearching(string name)
	{
		ManagerApplication.Instance.LoadingScreen.SetActive(true);
		ManagerHttpRequest.Instance.GetUserByName(name, OnRequestUserByName, LeaderboardType);
	}

	void ShowAllData()
	{
		EmptyListViewGameObject.SetActive(true);
	}

	void ClearData()
	{
		if (GenericRankPosition != null)
		{
			foreach (Transform tf in GenericRankPosition)
			{
				Destroy(tf.gameObject);
			}
		}
	}

	void OnSearchValueChange(string message)
	{
		if (message.Length > 0)
		{
			UserSearching(message);
			ClearSearchField.gameObject.SetActive(true);
			LeaderboardAndGallery.MenuLeaderboard.SetActive(false);
		}
		else
		{
			ManagerApplication.Instance.LoadingScreen.SetActive(true);
			ManagerHttpRequest.Instance.GetAllTimeLeaderboard(1, this.LeaderboardType);
			TopRankGameHandler.SetActive(true);
			ClearSearchField.gameObject.SetActive(false);
			LeaderboardAndGallery.MenuLeaderboard.SetActive(true);
		}
	}

	void OnClearSearchField()
	{
		SearchField.text = "";
	}

	void OnRequestUserByName(bool status, List<PlayerData> userDataList, string message)
	{
		ClearData();
		ManagerApplication.Instance.LoadingScreen.SetActive(false);
		if (userDataList.Count > 0)
		{
			if (EmptyListViewGameObject != null) { EmptyListViewGameObject.SetActive(false); }
			SetListGenericRank(userDataList, 0);
		}
		else
		{
			if (EmptyListViewGameObject != null) { EmptyListViewGameObject.SetActive(true); }
		}

		TopRankGameHandler.SetActive(false);
	}

	public void SwitchLeaderboardType(int code)
	{
		if (code == typeCode)
		{
			return;
		}

		if (code == 1)
		{
			this.LeaderboardType = LeaderboardType.ALL_TIME;
			ManagerApplication.Instance.CustomeToast.CallToast("Switched to All-time High score");
		}
		else
		{
			this.LeaderboardType = LeaderboardType.DAILY;
			ManagerApplication.Instance.CustomeToast.CallToast("Switched to Today's Daily High score");
		}

		this.typeCode = code;
		ManagerHttpRequest.Instance.GetAllTimeLeaderboard(1, this.LeaderboardType);
	}
}

[System.Serializable]
public class UserLeaderboard
{
	public int Page;
	public List<PlayerData> UserData;

	public UserLeaderboard(int page, List<PlayerData> userData)
	{
		this.Page = page;
		this.UserData = userData;
	}
}
