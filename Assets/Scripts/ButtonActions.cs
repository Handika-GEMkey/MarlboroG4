using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour {

	public Button[] Buttons;

	void Start()
	{
		InitButtonClicked();
	}

	public void InitButtonClicked()
	{
		foreach (Button btn in Buttons)
		{
			btn.onClick.AddListener(()=> ManagerAudio.Instance.PlayButton());
		}
	}
}
