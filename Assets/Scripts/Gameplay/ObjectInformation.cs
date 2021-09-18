using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInformation : MonoBehaviour {

	[SerializeField] private ObjectType objectType;
	[SerializeField] private string objectName;

	[Header("Applicable with ObjectType.OBJECTIVE")]
	[SerializeField] private float score;

	[Header("Applicable with ObjectType.OBSTACLE")]
	[SerializeField] private int punishment;

	public ObjectType GetObjectType() { return objectType; }
	public string GetObjectName() { return objectName; }

	public float Score
	{
		get
		{
			if (objectType.Equals(ObjectType.OBJECTIVE)) return score;
			else return 0;
		}
	}

	public int Punishment
	{
		get
		{
			if (objectType.Equals(ObjectType.OBSTACLE)) return punishment;
			else return 0;
		}
	}
}

[System.Serializable]
public enum ObjectType
{
	OBJECTIVE,
	OBSTACLE
}
