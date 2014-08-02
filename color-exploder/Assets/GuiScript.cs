﻿using UnityEngine;
using System.Collections;

public class GuiScript : MonoBehaviour {

    private static int _score = 0;
    private static string _currentText = "Score: 0";

	// Use this for initialization
	void Start () {
        Screen.SetResolution(640, 480, false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        // Make a background box
        GUI.Box(new Rect(250, Screen.height-50, 100, 40), string.Empty);

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        GUI.Label(new Rect(255, Screen.height-30, 100, 20), _currentText);
    }

    public static void AddScore(int add)
    {
        _score += add;
        _currentText = "Score: " + _score;
    }
}
