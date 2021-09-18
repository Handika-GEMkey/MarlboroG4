using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumizeRadioButton : MonoBehaviour {

	public Button RadioButton1;
	public Button RadioButton2;

	public Image RadioImage1;
	public Image RadioImage2;

	public Text RadioText1;
	public Text RadioText2;

	public Color32 RadioColorChoosen1;
	public Color32 RadioColorUnChoosen1;

	public Color32 RadioColorChoosen2;
	public Color32 RadioColorUnChoosen2;

	public int DefaultValue;

	void Awake()
	{
		RadioButton1.onClick.AddListener(()=> OnButtonClicked(1));
		RadioButton2.onClick.AddListener(() => OnButtonClicked(2));
	}

	public void OnButtonClicked(int buttonIndex)
	{
		DefaultValue = buttonIndex;

		if (buttonIndex == 1)
		{
			RadioImage1.color = RadioColorChoosen1;
			RadioImage2.color = RadioColorUnChoosen2;
			RadioText1.color = RadioColorUnChoosen1;
			RadioText2.color = RadioColorChoosen2;
		}
		else if(buttonIndex == 2)
		{
			RadioImage1.color = RadioColorUnChoosen1;
			RadioImage2.color = RadioColorChoosen2;
			RadioText1.color = RadioColorChoosen1;
			RadioText2.color = RadioColorUnChoosen2;
		}
	}
}
