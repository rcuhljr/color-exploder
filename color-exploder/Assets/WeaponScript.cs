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
	public void Attack()
	{
		if (CanAttack)
		{
			shootCooldown = shootingRate;
			
			// Create a new shot
			var shotTransform = Instantiate(shotPrefab) as Transform;
			
			// Assign position
			shotTransform.position = new Vector3( transform.position.x, transform.position.y + transform.localScale.y/2 , transform.position.z
			                                     );
			
			// The is enemy property
			Shot shot = shotTransform.gameObject.GetComponent<Shot>();
			if (shot != null)
			{
				shot.ShotColor = (int)Shot.Colors.player;
				shot.direction = this.transform.up;
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
