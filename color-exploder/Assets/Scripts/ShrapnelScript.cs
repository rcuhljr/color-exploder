using UnityEngine;
using System.Collections;
using Colors = ColorUtils.Colors;

public class ShrapnelScript : MonoBehaviour {

  /// <summary>
  /// Projectile prefab for shooting
  /// </summary>
  public Transform shotPrefab;
  
  /// <summary>
  /// Sound prefab for lasers
  /// </summary>
  public SoundScript sound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  /// <summary>
  /// Create a new projectile if possible
  /// </summary>
  public void Attack(Colors color, int magnitude, float speedMultiplier, Vector2 direction)
  {
          
    // Create laser sound
    if(sound != null)
    {
      sound.Play(SoundScript.SoundList.Lasers);
    }
    
    // Create a new shot
    var shotTransform = Instantiate(shotPrefab) as Transform;
    
    // Assign position
    shotTransform.position = new Vector3( transform.position.x, transform.position.y + transform.localScale.y/2 , transform.position.z
                                         );
    
    // The is enemy property
    Shot shot = shotTransform.gameObject.GetComponent<Shot>();
    if (shot != null)
    {
      shot.ShotColor = (int)color;
      shot.direction = direction;
      shot.Magnitude = magnitude;
      shot.speed.Normalize();
      shot.speed = shot.speed * speedMultiplier;
    }
    
    var sprite = shotTransform.gameObject.GetComponent<SpriteRenderer>();
    if(sprite != null)
    {
      sprite.color = ConvertToColor(color);
      
      if(magnitude > 1)
      {
        shotTransform.localScale =
          Vector3.Scale(shotTransform.localScale, new Vector3( 1 + (magnitude-1)*0.50f, 1 + (magnitude-1)*0.50f));
      }
    }
  }
  
  public Color ConvertToColor (Colors gameColor)
  {
    switch (gameColor) {
    case Colors.blue:
      return Color.blue;
    case Colors.green:
      return Color.green;
    case Colors.red:
      return Color.red;
    case Colors.player:
      return Color.black;
    default:
      return Color.gray;
    }
  }
}
