using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	//--------------------------------
	// 1 - Designer variables
	//--------------------------------
	
	/// <summary>
	/// Projectile prefab for shooting
	/// </summary>
	public Transform shotPrefab;

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
	public void Attack(Shot.Colors color, int score)
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
			shotTransform.position = new Vector3( transform.position.x, transform.position.y + transform.localScale.y/2 , transform.position.z
			                                     );
			
			// The is enemy property
			Shot shot = shotTransform.gameObject.GetComponent<Shot>();
			if (shot != null)
			{
				shot.ShotColor = (int)color;
				shot.direction = this.transform.up;
				shot.Score = score;
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
