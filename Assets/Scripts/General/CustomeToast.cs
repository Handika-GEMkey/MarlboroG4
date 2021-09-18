using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomeToast : MonoBehaviour 
{
	[Header("Custome Toast")]
	[SerializeField] private bool isToastBussy;
	[SerializeField] private GameObject toastGameObject;
	[SerializeField] private Text toastMessage;
	[SerializeField] private Animator toastAnimator;

	public void CallToast(string message)
	{
		toastAnimator.Play("Calling", 0, 0);
		toastMessage.text = message;
	}
}
