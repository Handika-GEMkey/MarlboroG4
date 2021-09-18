using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadAnimation : MonoBehaviour {

	public float scrollX = 0;
	public float scrollY = 0;

	private Renderer renderer;

	void Start()
	{
		renderer = GetComponent<Renderer>();
	}

	// Update is called once per frame
	void LateUpdate () {

		if (ManagerRacing.Instance.GameStarted)
		{
			float offsetX = Time.time * scrollX * ManagerRacing.Instance.RacingSpeed;
			float offsetY = Time.time * scrollY * ManagerRacing.Instance.RacingSpeed;

			renderer.material.mainTextureOffset = new Vector2(offsetX, offsetY);
		}
	}
}
