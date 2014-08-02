using UnityEngine;
using System.Collections;
using System;

public class GuiScript : MonoBehaviour
{

  private static int _score = 0;
  private static string _currentText = "Score: 0";

  public StageTimer stageControl;

  private bool gameOver = false;

	
	public SoundScript sound;

  // Use this for initialization
  void Start ()
  {
    Screen.SetResolution (640, 480, false);
  }
  
  // Update is called once per frame
  void Update ()
  {
    if(gameOver) {
      if(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
      {
        _score = 0;
        Application.LoadLevel(Application.loadedLevel);
      }
      return;
    }
      

    var player = GameObject.Find ("Player");

    if (player == null) {
		if(sound != null)
		{
			sound.Play(SoundScript.SoundList.GameOvers);
		}
      StopGame ();
      gameOver = true;
    }
  }

  private void StopGame ()
  {
    gameOver = true;

    stageControl.StopGame ();

  }

  void OnGUI ()
  {
    if (gameOver) {
      GUI.Box (new Rect (100, Screen.height - 300, 300, 200), "Game Over - Click to Retry");
      
      GUI.Label (new Rect (130, Screen.height - 275, 150, 40), "Final Score: " + _score);
    } else {
      GUI.Box (new Rect (250, Screen.height - 50, 100, 40), string.Empty);

      GUI.Label (new Rect (255, Screen.height - 30, 100, 20), _currentText);
    }
  }

  public static void AddScore (int add)
  {
    _score += add;
    _currentText = "Score: " + _score;
  }
}
