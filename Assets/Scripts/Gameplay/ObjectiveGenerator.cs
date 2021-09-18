using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveGenerator : MonoBehaviour {

	public List<GameObject> Batches;

	private int oldIndex = 0;

	public void BatchGenerator()
	{
		int index = UnityEngine.Random.Range(0, Batches.Count);

		while (Batches[index].activeInHierarchy)
		{
			index = UnityEngine.Random.Range(0, Batches.Count);
		}

		Batches[index].GetComponent<ObjectiveController>().InitiatePosition();
		Batches[index].SetActive(true);
	}

}
