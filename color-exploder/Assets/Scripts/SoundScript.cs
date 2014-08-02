using UnityEngine;
using System.Collections;

public class SoundScript : MonoBehaviour {
	
	public AudioClip bombSound;
	public AudioClip explosionSound;
	public AudioClip gameOverSound;
	public AudioClip laserSound;
	public AudioClip transitionSound;

	public enum SoundList
	{
		Bombs,
		Explosions,
		GameOvers,
		Lasers,
		Transitions
	}	

	public void Play(SoundList sound)
	{
		switch(sound)
		{
			case SoundList.Bombs:
				audio.PlayOneShot(bombSound, 0.7F);
				break;
			case SoundList.Explosions:
				audio.PlayOneShot(explosionSound, 0.7F);
				break;
			case SoundList.GameOvers:
				audio.PlayOneShot(gameOverSound, 0.7F);
				break;
			case SoundList.Lasers:
				audio.PlayOneShot(laserSound, 0.7F);
				break;
			case SoundList.Transitions:
				audio.PlayOneShot(transitionSound, 0.7F);
				break;
		}
	}

	//public void Start(){}
	//public void Update(){}
}
