using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMovement : MonoBehaviour {

	public float LocalSpeed;
	public float restartTriggerPosition;
	public Vector3 restartPosition;

	// Update is called once per frame
	void Update () {
		if (gameObject.activeInHierarchy && ManagerRacing.Instance.GameStarted)
		{
			if (this.transform.localPosition.z > restartTriggerPosition)
			{
				this.transform.localPosition = new Vector3(
				this.transform.localPosition.x,
				this.transform.localPosition.y,
				this.transform.localPosition.z - (Time.deltaTime * (ManagerRacing.Instance.RacingSpeed * LocalSpeed)));
			}
			else
			{
				this.transform.localPosition = restartPosition;
			}
		}
	}
}
