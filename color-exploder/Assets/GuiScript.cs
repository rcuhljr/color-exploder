using UnityEngine;
using System.Collections;

public class GuiScript : MonoBehaviour {

    private int _score = 0;
    private string _currentText = "Score: 0";

	// Use this for initialization
	void Start () {
	
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

    public void AddScore(int add)
    {
        _score += add;
        _currentText = "Score: " + _score;
    }
}
