using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerAudio : MonoBehaviour {

	public static ManagerAudio Instance;

	public AudioSource BGM;
	public AudioSource SFX;

	[SerializeField] private AudioClip buttonClickedClip;
	[SerializeField] private AudioClip bgmClip1;

	void Awake()
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Manager");

		if (objs.Length > 1)
		{
			Destroy(this.gameObject);
		}

		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}

	public void PlaySFX(AudioClip clip)
	{
		SFX.Stop();
		SFX.clip = clip;
		SFX.Play();
	}

	public void ChangeBGM(AudioClip clip)
	{
		BGM.Stop();
		BGM.clip = clip;
		BGM.Play();
	}

	public void PlayButton()
	{
		PlaySFX(buttonClickedClip);
	}

	
}
