using UnityEngine;
using System.Collections;
using Colors = ColorUtils.Colors;

public class BackgroundScript : MonoBehaviour {

  public Colors color;
  private bool changeColor = false;

	// Use this for initialization
	void Start () {
    color = Colors.player;
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
			if(!Options.Entries["ColorBlindMode"])
			{
      			spriteRenderer.sprite = Resources.Load<Sprite> ("Textures/red-background");
			}
			else
			{
				spriteRenderer.sprite = Resources.Load<Sprite> ("Textures/red-background-CB");
			}
      break;
    case Colors.blue:
			if(!Options.Entries["ColorBlindMode"])
			{
				spriteRenderer.sprite = Resources.Load<Sprite> ("Textures/blue-background");
			}
			else
			{
				spriteRenderer.sprite = Resources.Load<Sprite> ("Textures/blue-background-CB");
			}
      break;
    case Colors.green:
			if(!Options.Entries["ColorBlindMode"])
			{
				spriteRenderer.sprite = Resources.Load<Sprite> ("Textures/green-background");
			}
			else
			{
				spriteRenderer.sprite = Resources.Load<Sprite> ("Textures/green-background-CB");
			}
      break;
    default:
			if(!Options.Entries["ColorBlindMode"])
			{
				spriteRenderer.sprite = Resources.Load<Sprite> ("Textures/grey-background");
			}
			else
			{
				spriteRenderer.sprite = Resources.Load<Sprite> ("Textures/grey-background-CB");
			}
      break;
    }
    var ships = GameObject.FindGameObjectsWithTag ("enemy_ship");
    Color baseColor = ColorUtils.ConvertToColor (color);
    int convertedCount = 0;
    var newColors = ColorUtils.ColorMaps [color];
    foreach (var rawShip in ships) {

      var ship = rawShip.transform;
      var topCollider = ship.GetComponentInChildren<EnemyCollision>();
      if(topCollider != null)
      {
        if(topCollider.EnemyColor != color) continue;
      }

      var newColor = newColors[convertedCount%newColors.Count];

      for(int i=0; i<ship.childCount; i++)
      {
        var child = ship.GetChild(i);
        
        
        var renderer = child.GetComponent<SpriteRenderer> ();
        if(renderer.color != baseColor)
          break;
        renderer.color = ColorUtils.ConvertToColor (newColor);
      }

      foreach (var collider in ship.GetComponentsInChildren<EnemyCollision> ()) {
        collider.EnemyColor = newColor;
      }

      convertedCount += 1;

        
    }
	}

  public void ChangeColor(Colors newColor){
    changeColor = true;
    color = newColor;
  }
}
