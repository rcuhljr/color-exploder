using UnityEngine;
using System;
using System.Timers;

/// <summary>
/// Launch projectile
/// </summary>
public class SpawnEnemiesScript : MonoBehaviour
{
    //--------------------------------
    // 1 - Designer variables
    //--------------------------------

    /// <summary>
    /// Projectile prefab for shooting
    /// </summary>
    public Transform enemyPrefab;

    /// <summary>
    /// Cooldown in seconds between two enemies
    /// </summary>
	private bool readyToSpawn = false;

	public System.Random randomzier = new System.Random();

    //--------------------------------
    // 2 - Cooldown
    //--------------------------------

    private float enemyCooldown;

    void Start()
    {
		var timer = new Timer(500);
        timer.Elapsed += timer_Elapsed;
		timer.Start();
    }

    void Update()
    {
		Spawn();
    }

    private void timer_Elapsed(object sender, EventArgs a)
    {
		readyToSpawn = true;
    }

    //--------------------------------
    // 3 - Shooting from another script
    //--------------------------------

    /// <summary>
    /// Create a new enemy
    /// </summary>
    public void Spawn()
    {
		if (readyToSpawn) {
			readyToSpawn=false;
						// Create a new shot
						var enemy = Instantiate (enemyPrefab) as Transform;

						// Assign position
			            enemy.position = Camera.main.ViewportToWorldPoint(new Vector3 (((randomzier.Next () % 100) / 100), 1));


						enemy.GetComponent<SpriteRenderer> ().color = ConvertToColor ((Shot.Colors)(randomzier.Next () % 3));

						// Make the weapon shot always towards it
						//MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
						//if (move != null)
						//{
						//    move.direction = this.transform.right; // towards in 2D space is the right of the sprite
						//}
				}
    }

	public Color ConvertToColor(Shot.Colors gameColor)
	{
				switch (gameColor) {
				case Shot.Colors.blue:
						return Color.blue;
				case Shot.Colors.green:
						return Color.green;
				case Shot.Colors.red:
						return Color.red;
				default:
						return Color.white;
		}
	}
}

