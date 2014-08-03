using UnityEngine;
using System.Collections;

public class ColorBombScript : MonoBehaviour {

  public int BombsLeft = 1;
  public BackgroundScript bgScript;

  public void DropBomb(){
    if (BombsLeft > 0) {
      BombsLeft -= 1;
      bgScript.ChangeColor(ColorUtils.Colors.player);
      GuiScript.UpdateBomb(BombsLeft);
    }
  }
}
