using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaptureUtility : MonoBehaviour
{
	[SerializeField]
	private Image imagePreview;

	private Texture2D captureTexture;

	private void Awake()
	{
		this.captureTexture = new Texture2D(0, 0);
	}

	private IEnumerator CaptureCoroutine(Rect captureRect, GameObject[] hideGameObjects, GameObject showResult)
	{
		foreach (GameObject go in hideGameObjects) { go.SetActive(false); }
		yield return new WaitForEndOfFrame(); // Important, wait till all the frame done rendering.

		this.captureTexture = new Texture2D(Mathf.FloorToInt(captureRect.width), Mathf.FloorToInt(captureRect.width), TextureFormat.RGB24, false);

		this.captureTexture.ReadPixels(captureRect, 0, 0);
		this.captureTexture.Apply();

		Sprite newImage = Sprite.Create(captureTexture, new Rect(0, 0, captureTexture.width, captureTexture.height), Vector2.one / 2);
		imagePreview.sprite = newImage;
		foreach (GameObject go in hideGameObjects) { go.SetActive(true); }
		yield return new WaitForSeconds(15f);
		showResult.SetActive(true);
	}

	private IEnumerator CaptureCoroutine(RectTransform rectTransform, GameObject[] hideGameObjects, GameObject showResult)
	{
		foreach (GameObject go in hideGameObjects) { go.SetActive(false); }
		yield return new WaitForEndOfFrame(); // Important, wait till all the frame done rendering.

		var rectCorners = new Vector3[4];
		rectTransform.GetWorldCorners(rectCorners);

		var height = rectCorners[1].y - rectCorners[0].y;
		var width = rectCorners[3].x - rectCorners[0].x;

		var start = new Vector2(rectCorners[0].x, rectCorners[0].y);

		this.captureTexture = new Texture2D(Mathf.CeilToInt(width), Mathf.CeilToInt(height), TextureFormat.RGB24, false);
		var rect = new Rect(start.x, start.y, width, height);

		this.captureTexture.ReadPixels(rect, 0, 0);
		this.captureTexture.Apply();

		Sprite newImage = Sprite.Create(captureTexture, new Rect(0,0, captureTexture.width, captureTexture.height), Vector2.one/2);
		imagePreview.sprite = newImage;

		foreach (GameObject go in hideGameObjects) { go.SetActive(true); }
		yield return new WaitForSeconds(15f);
		showResult.SetActive(true);
	}

	public void CaptureCustomRegion(Rect captureRect, GameObject[] hideGameObjects, GameObject showResult) { this.StartCoroutine(this.CaptureCoroutine(captureRect, hideGameObjects, showResult)); }

	public void CaptureCustomRegion(RectTransform rectTransform, GameObject[] hideGameObjects, GameObject showResult) { this.StartCoroutine(this.CaptureCoroutine(rectTransform, hideGameObjects, showResult)); }

}