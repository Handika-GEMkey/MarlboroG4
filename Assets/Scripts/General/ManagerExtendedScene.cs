using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerExtendedScene : MonoBehaviour {

	public static ManagerExtendedScene Instance;

	public GameObject TopRankPrefab;
	public GameObject GenericRankPrefab;

	void Awake()
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Manager");

		if (objs.Length > 1)
		{
			Destroy(this.gameObject);
		}

		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}
}
