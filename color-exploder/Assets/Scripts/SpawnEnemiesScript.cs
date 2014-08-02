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

	private Timer timer;

    void Start()
    {
		timer = new Timer(500);
        timer.Elapsed += timer_Elapsed;
		timer.Start();
    }

    void Update()
    {
		Spawn();
    }

	public void SetSpawnRate(int newRate){
		timer.Interval = newRate;
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
						// Create a new enemy
						var enemy = Instantiate (enemyPrefab) as Transform;

						// Assign position
						//TODO: These probably shouldn't be constants...
                        enemy.position = new Vector3( (float)((randomzier.NextDouble()-0.5)*12), 5, 1);
						var color = randomzier.Next () % 3;
						enemy.GetComponent<SpriteRenderer> ().color = ConvertToColor ((Shot.Colors)(color));
						
						var enemyCollision = enemy.GetComponent<EnemyCollision>();
						if(enemyCollision != null) {
							enemyCollision.EnemyColor = (Shot.Colors)color;
						}
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

