using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using System;

public class FormFieldWebGL : MonoBehaviour, IPointerClickHandler
{
	public string FieldTitle;
	public bool IsException;

	[DllImport("__Internal")]
	private static extern void inputFieldName(string _title, string objectName, string functionName, string _str);

	[DllImport("__Internal")]
	private static extern void inputFieldTelephone(string _title, string objectName, string functionName, string _str);

	public void ReceiveInputData(string value)
	{
		this.gameObject.GetComponent<InputField>().text = value;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
#if UNITY_WEBGL
		try
		{
			if (!IsException)
			{
				inputFieldName(FieldTitle, this.gameObject.name, "ReceiveInputData", gameObject.GetComponent<InputField>().text);
			}
			else
			{
				inputFieldTelephone(FieldTitle, this.gameObject.name, "ReceiveInputData", gameObject.GetComponent<InputField>().text);
			}
		}
		catch (Exception error) { }
#endif
	}
}
