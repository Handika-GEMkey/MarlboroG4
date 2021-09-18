using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.Analytics;
using System.Runtime.InteropServices;
using System;

public class MainMenuHandler : MonoBehaviour {

	[Header("Field Components View")]
	[SerializeField] private Text buildCode;
	[SerializeField] private InputField fieldName;
	[SerializeField] private InputField fieldEmail;
	[SerializeField] private InputField fieldTelephone;
	[SerializeField] private InputField fieldCarChoice;
	[SerializeField] private CostumizeRadioButton radioButton;

	[Header("Error Messages")]
	[SerializeField] private string successStatus;
	[SerializeField] private string fieldNameErrorStatus;
	[SerializeField] private string fieldEmailErrorStatus1;
	[SerializeField] private string fieldEmailErrorStatus2;
	[SerializeField] private string fieldTelephoneErrorStatus1;
	[SerializeField] private string fieldTelephoneErrorStatus2;
	[SerializeField] private string fieldCarChoiceErrorStatus;
	[SerializeField] private string radioButtonErrorStatus;

	[Header("Important Page")]
	[SerializeField] private GameObject menuAndFieldsGameObject;
	[SerializeField] private GameObject photoBoothGameObject;

	[Header("Submit Component")]
	[SerializeField] private PhotoBoothHandler photoBoothHandler;

	string pattern = null;
	private int inputID = 1;

	[DllImport("__Internal")]
	private static extern void inputFieldName(string title, string object_name, string function_name, string default_value);

	[DllImport("__Internal")]
	private static extern void inputFieldEmail(string title, string object_name, string function_name, string default_value);

	[DllImport("__Internal")]
	private static extern void inputFieldTelephone(string title, string object_name, string function_name, string default_value);

	[DllImport("__Internal")]
	private static extern void inputFieldCar(string title, string object_name, string function_name, string default_value);

	void Start()
	{
		pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
		ManagerHttpRequest.Instance.OnCallBackCreateGame += Instance_OnCallBackCreateGame;
		buildCode.text = ManagerApplication.Instance.BuildCode;
		InitFieldAction();
	}

	public void InitFieldAction()
	{
		fieldName.GetComponent<CustomeInputField>().onClicked += FieldName;
		fieldEmail.GetComponent<CustomeInputField>().onClicked += FieldEmail;
		fieldTelephone.GetComponent<CustomeInputField>().onClicked += FieldTelephone;
		fieldCarChoice.GetComponent<CustomeInputField>().onClicked += FieldCar;
	}

	public void FieldName() { OnKeyboardInputName("Name", this.gameObject.name, "ReceiveName", fieldName.text); }
	public void FieldEmail() { OnKeyboardInputEmail("Email", this.gameObject.name, "ReceiveEmail", fieldEmail.text); }
	public void FieldTelephone() { OnKeyboardInputTelephone("Telephone", this.gameObject.name, "ReceiveTelephone", fieldTelephone.text); }
	public void FieldCar() { OnKeyboardInputCar("Car", this.gameObject.name, "ReceiveCar", fieldCarChoice.text); }

	public void OnKeyboardInputName(string title, string object_name, string function_name, string default_value)
	{
#if UNITY_WEBGL
		try
		{
			inputFieldName(title, object_name, function_name, default_value);
		}
		catch (Exception error) { }
#endif
	}

	public void OnKeyboardInputEmail(string title, string object_name, string function_name, string default_value)
	{
#if UNITY_WEBGL
		try
		{
			inputFieldEmail(title, object_name, function_name, default_value);
		}
		catch (Exception error) { }
#endif
	}

	public void OnKeyboardInputTelephone(string title, string object_name, string function_name, string default_value)
	{
#if UNITY_WEBGL
		try
		{
			inputFieldTelephone(title, object_name, function_name, default_value);
		}
		catch (Exception error) { }
#endif
	}

	public void OnKeyboardInputCar(string title, string object_name, string function_name, string default_value)
	{
#if UNITY_WEBGL
		try
		{
			inputFieldCar(title, object_name, function_name, default_value);
		}
		catch (Exception error) { }
#endif
	}

	public void ReceiveName(string value)
	{
		fieldName.text = value;
	}

	public void ReceiveEmail(string value)
	{
		fieldEmail.text = value;
	}

	public void ReceiveTelephone(string value)
	{
		fieldTelephone.text = value;
	}

	public void ReceiveCar(string value)
	{
		fieldCarChoice.text = value;
	}

	private void Instance_OnCallBackCreateGame(bool arg1, string arg2)
	{
		if (arg1)
		{
			ManagerPlayer.Instance.UserID = arg2;
			try
			{
				menuAndFieldsGameObject.SetActive(false);
				photoBoothGameObject.SetActive(true);

				photoBoothHandler.PhotoSession.gameObject.SetActive(true);
				photoBoothHandler.PhotoPreview.gameObject.SetActive(false);
				photoBoothHandler.BeforeTakePhoto.gameObject.SetActive(true);
				photoBoothHandler.AfterTakePhoto.gameObject.SetActive(false);

				inputID++;
				ManagerApplication.Instance.CustomeToast.CallToast("Success!");
				photoBoothHandler.AutorizeCamera();
			}
			catch (Exception e)
			{
				Debug.Log(e);
			}

		}
		else
		{
			ManagerApplication.Instance.CustomeToast.CallToast("Failed : Please check your internet connection!");
		}
		ManagerApplication.Instance.LoadingScreen.SetActive(false);
	}

	public DataObjectStatusMessage CheckField()
	{
		if (fieldName.text != "")
		{
			if (fieldEmail.text != "")
			{
				if (fieldTelephone.text != "")
				{
					if (fieldCarChoice.text != "")
					{
						if (radioButton.DefaultValue != 0)
						{
							if (fieldTelephone.text.Length < 10 || fieldTelephone.text.Length > 14)
							{
								return new DataObjectStatusMessage(false, fieldTelephoneErrorStatus1);
							}
							else
							{
								if (Regex.IsMatch(fieldEmail.text, pattern))
								{
									int errorCounter = Regex.Matches(fieldTelephone.text, @"[a-zA-Z]").Count;
									if (errorCounter < 1)
									{
										return new DataObjectStatusMessage(true, successStatus);
									}
									else
									{
										return new DataObjectStatusMessage(true, "Telephon number must only number");
									}
								}
								else
								{
									return new DataObjectStatusMessage(false, fieldEmailErrorStatus2);
								}
							}
						}
						else
						{
							return new DataObjectStatusMessage(false, radioButtonErrorStatus);
						}
					}
					else
					{
						return new DataObjectStatusMessage(false, fieldCarChoiceErrorStatus);
					}
				}
				else
				{
					return new DataObjectStatusMessage(false, fieldTelephoneErrorStatus2);
				}
			}
			else
			{
				return new DataObjectStatusMessage(false, fieldEmailErrorStatus1);
			}
		}
		else
		{
			return new DataObjectStatusMessage(false, fieldNameErrorStatus);
		}
	}

	public void SubmitFields()
	{
		DataObjectStatusMessage message = CheckField();
		if (!message.Status)
		{
			ManagerApplication.Instance.CustomeToast.CallToast(message.Message);
			return;
		}

		ManagerPlayer.Instance.FillData(
			inputID.ToString(),
			fieldName.text.ToString(),
			fieldEmail.text.ToString(),
			fieldTelephone.text.ToString(),
			fieldCarChoice.text.ToString(),
			radioButton.DefaultValue
			);

		ManagerApplication.Instance.LoadingScreen.SetActive(true);
		if (ManagerHttpRequest.Instance)
		{
			ManagerHttpRequest.Instance.CreateNewGame();
		}
	}

	public void OpenLeaderboard()
	{
		ManagerApplication.Instance.SceneChange("ExtendedMenu/Leaderboard");
	}

	public void OpenGallery()
	{
		ManagerApplication.Instance.SceneChange("ExtendedMenu/Gallery");
	}
	
}

[System.Serializable]
public class DataObjectStatusMessage
{
	public bool Status;
	public string Message;

	public DataObjectStatusMessage(bool status, string message)
	{
		Status = status;
		Message = message;
	}
}
