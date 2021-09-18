using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectHandler : MonoBehaviour {

	[SerializeField] private GameObject buttonLoadMore;
	[SerializeField] private ScrollRect scrollRect;
	[SerializeField] private float normalizedPosition;

	void Update()
	{
		normalizedPosition = scrollRect.verticalNormalizedPosition;
		if (scrollRect.verticalNormalizedPosition <= 0.05f)
		{
			buttonLoadMore.SetActive(true);
		}
		else
		{
			buttonLoadMore.SetActive(false);
		}
	}
}
