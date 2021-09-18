using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "DataObject/DataObjectPlayer", fileName = "DataObjectPlayer.")]
public class DataObjectPlayer : ScriptableObject {

	[SerializeField] private string key;
	[SerializeField] private string userName;
	[SerializeField] private string email;
	[SerializeField] private string telphoneNumber;
	[SerializeField] private string carChoice;
	[SerializeField] private int genderChoosen;

	public string Key
	{
		set { this.key = value; }
		get { return this.key; }
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

	public int CenderChoosen
	{
		set { this.genderChoosen = value; }
		get { return this.genderChoosen; }
	}
}
