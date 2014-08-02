using UnityEngine;
using System.Collections;

public class SoundScript : MonoBehaviour {

	public AudioClip laserSound;

	public enum SoundList
	{
		Lasers
	}	

	public void Play(SoundList sound)
	{
		switch(sound)
		{
			case SoundList.Lasers:
				audio.PlayOneShot(laserSound, 0.7F);
				break;
		}
	}

	public void Start(){}
	public void Update(){}
}
