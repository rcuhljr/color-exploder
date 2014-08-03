using UnityEngine;
using System.Collections;
using System.Timers;

public class ColorBombScript : MonoBehaviour {

  public int BombsLeft = 0;
  public BackgroundScript bgScript;
  public SoundScript sound;
  private Timer delayTimer;
  private bool dropBomb = false;
  private int prevScore = 0;

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
    if (prevScore / 100 != GuiScript.GetScore ()/100) {
      prevScore = GuiScript.GetScore();
      BombsLeft += 1;
      GuiScript.UpdateBomb(BombsLeft);
    }
    if (dropBomb) {
      dropBomb = false;
      bgScript.ChangeColor (ColorUtils.Colors.player);
      delayTimer.Stop ();
    }
  }
}
