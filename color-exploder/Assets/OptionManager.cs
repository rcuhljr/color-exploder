using UnityEngine;
using System.Collections;

public class OptionManager : MonoBehaviour {

	public string optionName = string.Empty;

	public bool isOnOption = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Options.Entries[optionName] == isOnOption) {
						gameObject.GetComponent<SpriteRenderer> ().color = Color.black;
				} else {
						gameObject.GetComponent<SpriteRenderer> ().color = new Color (56, 56, 56, 117);
				}
		}

	void OnMouseDown()
		{
		if (!Options.Entries [optionName] == isOnOption) {
			Options.Entries [optionName] = isOnOption;
		}
}
}
