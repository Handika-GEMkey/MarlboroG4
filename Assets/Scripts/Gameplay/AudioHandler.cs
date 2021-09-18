using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour {

	[SerializeField] private AudioSource bgmSource;
	[SerializeField] private AudioSource sfxSource;

	[SerializeField] private AudioSource counterSFX;
	[SerializeField] private AudioClip counter;
	[SerializeField] private AudioClip counterFinal;

	[SerializeField] private AudioClip collectingObject;
	[SerializeField] private AudioClip collisionObstacle;
	[SerializeField] private AudioClip[] audioCarSequence;

	public void PlaySFXCounter()
	{
		counterSFX.Stop();
		counterSFX.clip = counter;
		counterSFX.Play();
	}

	public void PlaySFXCounterFinal()
	{
		counterSFX.Stop();
		counterSFX.clip = counterFinal;
		counterSFX.Play();
	}

	public void PlaySFXCollision()
	{
		sfxSource.Stop();
		sfxSource.clip = collisionObstacle;
		sfxSource.Play();
	}

	public void PlaySFXCollecting()
	{
		sfxSource.Stop();
		sfxSource.clip = collectingObject;
		sfxSource.Play();
	}

	public void PlayCarSound()
	{
		StartCoroutine(CarSequentialSoundMachines(audioCarSequence));
	}

	public void StopCarSound()
	{
		bgmSource.Stop();
		bgmSource.loop = false;
	}

	IEnumerator CarSequentialSoundMachines(AudioClip[] sequenceOfCarMachine)
	{
		bgmSource.clip = sequenceOfCarMachine[0];
		bgmSource.Play();
		yield return new WaitForSeconds(12.335f);
		bgmSource.Stop();
		yield return new WaitForEndOfFrame();
		bgmSource.clip = sequenceOfCarMachine[1];
		bgmSource.Play();
		bgmSource.loop = true;
	}
}
