using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPlayer : MonoBehaviour {

	public static ManagerPlayer Instance;

	[SerializeField] private string userID;
	[SerializeField] private string userName;
	[SerializeField] private string email;
	[SerializeField] private string telphoneNumber;
	[SerializeField] private string carChoice;
	[SerializeField] private int genderChoosen;

	[Header("Data After")]
	[SerializeField] private float scoreData;

	[Header("Photo Section")]
	public Sprite VisualNormalPhoto;
	public byte[] NormalPhotoBytes;
	public byte[] EditedPhotoBytes;

	public string UserID
	{
		set { this.userID = value; }
		get { return this.userID; }
	}

	public string UserName
	{
		set { this.userName = value; }
		get { return this.userName; }
	}

	public string Email
	{
		set { this.email = value; }
		get { return this.email; }
	}

	public string TelphoneNumber
	{
		set { this.telphoneNumber = value; }
		get { return this.telphoneNumber; }
	}

	public string CarChoice
	{
		set { this.carChoice = value; }
		get { return this.carChoice; }
	}

	public int GenderChoosen
	{
		set { this.genderChoosen = value; }
		get { return this.genderChoosen; }
	}

	public float ScoreData
	{
		set { this.scoreData = value; }
		get { return this.scoreData; }
	}

	public void FillData(string key, string userName, string email, string telpNumber, string carChoice, int genderChoosen)
	{
		this.userID = key;
		this.userName = userName;
		this.email = email;
		this.telphoneNumber = telpNumber;
		this.carChoice = carChoice;
		this.genderChoosen = genderChoosen;
	}

	public void ResetData()
	{
		this.userID = "";
		this.userName = "";
		this.email = "";
		this.telphoneNumber = "";
		this.carChoice = "";
		this.genderChoosen = 0;
	}

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
