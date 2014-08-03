using UnityEngine;
using System.Collections;
using System.Timers;

public class ColorBombScript : MonoBehaviour {

  public int BombsLeft = 1;
  public BackgroundScript bgScript;
  public SoundScript sound;
  private Timer delayTimer;
  private bool dropBomb = false;

  public void Start () {
    delayTimer = new Timer (750);
    delayTimer.Elapsed += dropReady;
  }

  void dropReady (object sender, ElapsedEventArgs e)
  {
    dropBomb = true;
  }

  public void DropBomb(){
    if (BombsLeft > 0) {
      if(sound != null)
      {
        sound.Play(SoundScript.SoundList.Bombs);
      }
      delayTimer.Start();
      BombsLeft -= 1;
      GuiScript.UpdateBomb(BombsLeft);
    }
  }

  public void Update() {
    if (dropBomb) {
      dropBomb = false;
      bgScript.ChangeColor (ColorUtils.Colors.player);
      delayTimer.Stop ();
    }
  }
}
