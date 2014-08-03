using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Options {

	public static Dictionary<string, bool> Entries = new Dictionary<string, bool>();

	static Options()
	{
		Entries.Add ("Music", true);
		Entries.Add ("SoundEffects", true);
		Entries.Add ("ColorBlindMode", false);
		}
	
}
