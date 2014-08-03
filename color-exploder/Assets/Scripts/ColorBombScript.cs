using UnityEngine;
using System.Collections;

public class ColorBombScript : MonoBehaviour {

  public int BombsLeft = 1;
  public BackgroundScript bgScript;
  public SoundScript sound;

  public void DropBomb(){
    if (BombsLeft > 0) {
      if(sound != null)
      {
        sound.Play(SoundScript.SoundList.Bombs);
      }
      BombsLeft -= 1;
      bgScript.ChangeColor(ColorUtils.Colors.player);
      GuiScript.UpdateBomb(BombsLeft);
    }
  }
}
