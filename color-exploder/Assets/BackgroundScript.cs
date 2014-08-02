using UnityEngine;
using System.Collections;
using Colors = ColorUtils.Colors;

public class BackgroundScript : MonoBehaviour {

  public Colors color;
  private bool changeColor = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	  if (!changeColor) {
      return;
    }
    changeColor = false;
    var spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
    Debug.Log ("switching to " + color.ToString ());
    switch (color) {
    case Colors.red:
      spriteRenderer.sprite = Resources.Load<Sprite> ("Textures/red-background");
      break;
    case Colors.blue:
      spriteRenderer.sprite = Resources.Load<Sprite> ("Textures/blue-background");
      break;
    case Colors.green:
      spriteRenderer.sprite = Resources.Load<Sprite> ("Textures/green-background");
      break;
    default:
      spriteRenderer.sprite = Resources.Load<Sprite> ("Textures/grey-background");
      break;
    }
	}

  public void ChangeColor(Colors newColor){
    changeColor = true;
    color = newColor;
  }
}
