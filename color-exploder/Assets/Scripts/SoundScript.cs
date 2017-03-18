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
		if (Options.Entries["SoundEffects"]) {
            AudioSource audio = GetComponent<AudioSource>();
			switch(sound)
			{
			case SoundList.Bombs:
				audio.PlayOneShot(bombSound, 0.7F);
				break;
			case SoundList.Explosions:
				audio.PlayOneShot(explosionSound, 0.7F);
				break;
			case SoundList.GameOvers:
				audio.PlayOneShot(gameOverSound, 1F);
				break;
			case SoundList.Lasers:
				audio.PlayOneShot(laserSound, 0.7F);
				break;
			case SoundList.Transitions:
				audio.PlayOneShot(transitionSound, 0.7F);
				break;
			}
		}
	}
	
	public AudioClip menuSound;
	public AudioClip randomSound;
	public AudioClip blueSound;
	public AudioClip greenSound;
	public AudioClip redSound;
	public AudioClip bossSound;

	public enum BackgroundSound
	{
		MenuSong,
		RandomSong,
		BlueSong,
		GreenSong,
		RedSong,
		BossSong
	}

	public void PlayMusic(BackgroundSound song)
	{
		if (Options.Entries["Music"]) {
            AudioSource audio = GetComponent<AudioSource>();
			switch(song)
			{
			case BackgroundSound.MenuSong:
                audio.clip = menuSound;
                audio.Play();
				break;
			case BackgroundSound.RandomSong:
                audio.clip = randomSound;
                audio.Play();
				break;
			case BackgroundSound.BlueSong:
                audio.clip = blueSound;
                audio.Play();
				break;
			case BackgroundSound.GreenSong:
                audio.clip = greenSound;
                audio.Play();
				break;
			case BackgroundSound.RedSong:
                audio.clip = redSound;
                audio.Play();
				break;
			case BackgroundSound.BossSong:
                audio.clip = bossSound;
                audio.Play();
				break;
			}
			audio.loop = true;
		}
	}

	public void StopMusic()
	{
		if (!Options.Entries["Music"]) {
            GetComponent<AudioSource>().Stop();
		}
	}

	public void MuteMusic(bool state) {
        GetComponent<AudioSource>().mute = state;
	}
}
