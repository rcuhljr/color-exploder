using UnityEngine;
using System.Collections;
using Colors = ColorUtils.Colors;

public class WeaponScript : MonoBehaviour {

	//--------------------------------
	// 1 - Designer variables
	//--------------------------------
	
	/// <summary>
	/// Projectile prefab for shooting
	/// </summary>
	public Transform shotPrefab;

	/// <summary>
	/// Sound prefab for lasers
	/// </summary>
	public SoundScript sound;
	
	/// <summary>
	/// Cooldown in seconds between two shots
	/// </summary>
	public float shootingRate = 0.25f;
	
	//--------------------------------
	// 2 - Cooldown
	//--------------------------------
	
	private float shootCooldown;
	
	void Start()
	{
		shootCooldown = 0f;
	}
	
	void Update()
	{
		if (shootCooldown > 0)
		{
			shootCooldown -= Time.deltaTime;
		}
	}
	
	//--------------------------------
	// 3 - Shooting from another script
	//--------------------------------
	
	/// <summary>
	/// Create a new projectile if possible
	/// </summary>
	public void Attack(Colors color, int magnitude, float speedMultiplier)
	{
		if (CanAttack)
		{
			shootCooldown = shootingRate;

			// Create laser sound
			if(sound != null)
			{
				sound.Play(SoundScript.SoundList.Lasers);
			}
			
			// Create a new shot
			var shotTransform = Instantiate(shotPrefab) as Transform;
			
			// Assign position
			shotTransform.position = new Vector3( transform.position.x, transform.position.y /*+ transform.localScale.y/2*/ , transform.position.z+1
			                                     );
			
			// The is enemy property
			Shot shot = shotTransform.gameObject.GetComponent<Shot>();
			if (shot != null)
			{
				shot.ShotColor = (int)color;
				shot.direction = this.transform.up;
				shot.Magnitude = magnitude;
        shot.speed.Scale(new Vector2(speedMultiplier, speedMultiplier));
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
	}

  public Color ConvertToColor (Colors gameColor)
  {
    if (Options.Entries ["ColorBlindMode"]) 
    {
      switch (gameColor) {
      case Colors.blue:
        return new Color(0.9F,0.9F,0.9F);
      case Colors.green:
        return new Color(0.8F,0.8F,0.8F);
      case Colors.red:
        return new Color(0.6F,0.6F,0.6F);
      case Colors.player:
        return Color.black;
      case Colors.cyan:
        return new Color(0.5F,0.5F,0.5F);
      case Colors.magenta:
        return new Color(0.7F,0.7F,0.7F);
      case Colors.yellow:
        return new Color(0.4F,0.4F,0.4F);
      default:
        return Color.white;
      }
    } 
    else 
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
      case Colors.cyan:
        return Color.cyan;
      case Colors.magenta:
        return Color.magenta;
      case Colors.yellow:
        return Color.yellow;
      default:
        return Color.white;
      }
    }
  }
	
	/// <summary>
	/// Is the weapon ready to create a new projectile?
	/// </summary>
	public bool CanAttack
	{
		get
		{
			return shootCooldown <= 0f;
		}
	}

}
