using UnityEngine;
using System.Collections;

public class MusicToggleScript : MonoBehaviour {

	public SoundScript sound;
	public bool lastKnownState;

	// Use this for initialization
	void Start () {
		lastKnownState = true;
	}
	
	// Update is called once per frame
	void Update () {
		bool currentState = Options.Entries ["Music"];
		if (currentState != lastKnownState) {
			if (sound != null) {
				sound.MuteMusic(!currentState);
			}
      lastKnownState = currentState;
		}
	}
}
