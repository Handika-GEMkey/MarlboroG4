using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ObjectiveController : MonoBehaviour {

	private Vector3 initPosition;
    public float PosX_Left;
    public float PosX_Right;
    public float PosX_Mid;
    public Material ObsMatLeft;
    public Material ObsMatRight;
    public Material ObsMatMid;

	public ObjectiveGenerator objectiveGenerator;
	public GameObject[] incomingObjects;
	public float RestartPosition;
	public float SpawnPositionAfter;
	public float LocalSpeed;

	private bool callObjectiveGenerator = false;
	private Vector3[] linePositions;

	void Start()
	{
		initPosition = this.transform.position;
	}

	public void InitiatePosition()
	{
		int lenghtObject = 0;

		linePositions = ManagerRacing.Instance.GetLinePositions();

		List<int> newPosition = new List<int>();
		for (var x = 0; x < incomingObjects.Length; x++)
		{
			lenghtObject += ManagerRacing.Instance.ObsDistance;
			newPosition.Add(lenghtObject);
		}

		var shufflePosition = Shuffle(newPosition);

		for (var x = 0; x < incomingObjects.Length; x++)
		{
			int randPos = UnityEngine.Random.Range(0, linePositions.Length);
			incomingObjects[x].transform.localPosition =
            new Vector3(linePositions[randPos].x,
                        incomingObjects[x].transform.localPosition.y,
                        shufflePosition[x]);

            if (incomingObjects[x].tag == "Obstacle")
            {
                if (incomingObjects[x].transform.localPosition.x == PosX_Left)
                {
                    incomingObjects[x].GetComponent<MeshRenderer>().material = ObsMatLeft;
                }
                else if (incomingObjects[x].transform.localPosition.x == PosX_Right)
                {
                    incomingObjects[x].GetComponent<MeshRenderer>().material = ObsMatRight;
                }
                else if (incomingObjects[x].transform.localPosition.x == PosX_Mid)
                {
                    incomingObjects[x].GetComponent<MeshRenderer>().material = ObsMatMid;
                }
            }

            if (!incomingObjects[x].activeInHierarchy)incomingObjects[x].SetActive(true);
		}
	}

	public List<int> Shuffle(List<int> aList)
	{
		System.Random _random = new System.Random ();
 
        int myGO;
 
        int n = aList.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + (int)(_random.NextDouble() * (n - i));
            myGO = aList[r];
            aList[r] = aList[i];
            aList[i] = myGO;
        }
		return aList;
	}

	// Update is called once per frame
	void Update() {
		if (gameObject.activeInHierarchy)
		{
			if (this.transform.localPosition.z > RestartPosition)
			{
				this.transform.localPosition = new Vector3(
				this.transform.localPosition.x,
				this.transform.localPosition.y,
				this.transform.localPosition.z - (Time.deltaTime * (ManagerRacing.Instance.RacingSpeed * LocalSpeed)));

				if (this.transform.localPosition.z < SpawnPositionAfter)
				{
					if (!callObjectiveGenerator)
					{
						objectiveGenerator.BatchGenerator();
						callObjectiveGenerator = true;
					}
				}
			}
			else
			{
				if (transform.childCount > 0) { foreach (Transform tr in transform) { tr.gameObject.SetActive(false); } }

				callObjectiveGenerator = false;
				this.transform.position = initPosition;
				gameObject.SetActive(false);
			}
		}
	}
}
