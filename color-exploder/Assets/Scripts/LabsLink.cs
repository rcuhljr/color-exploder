using UnityEngine;
using System;

public class LabsLink : MonoBehaviour {

	private static Uri url = new Uri(@"http://www.sep.com/labs/");

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown () {
		Application.ExternalCall("open2", url, 1);
	}
}
