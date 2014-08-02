using UnityEngine;
using System;
using System.Timers;
using SpawnInfo = StageTimer.SpawnInfo;

/// <summary>
/// Launch projectile
/// </summary>
using System.Collections.Generic;
using System.Linq;

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
  public System.Random randomzier = new System.Random ();

  //--------------------------------
  // 2 - Cooldown
  //--------------------------------

  private float enemyCooldown;
  private Timer timer;
  private List<SpawnInfo> staticSpawns;

  void Start ()
  {
    timer = new Timer (500);
    timer.Elapsed += timer_Elapsed;
    timer.Start ();
  }

  void Update ()
  {
    Spawn ();
  }

  public void SetSpawnRate (int newRate)
  {
    staticSpawns = null;
    timer.Stop ();
    timer.Interval = newRate;
    timer.Start ();
  }

  public void SetSpawnScript (List<SpawnInfo> staticSpawns)
  {
    timer.Stop ();
    this.staticSpawns = staticSpawns;
    timer.Interval = staticSpawns [0].delay;
    timer.Start ();
  }

  private void timer_Elapsed (object sender, EventArgs a)
  {
    readyToSpawn = true;
  }

  //--------------------------------
  // 3 - Shooting from another script
  //--------------------------------

  /// <summary>
  /// Create a new enemy
  /// </summary>
  public void Spawn ()
  {
    if (!readyToSpawn) {
      return;
    }
    readyToSpawn = false;

    if (staticSpawns != null) {
      // Create a new enemy
      do {
        var spawn = staticSpawns.First ();
        staticSpawns.RemoveAt (0);

        SpawnEnemy (new Vector3 (spawn.x, spawn.y, 1), spawn.color);

      } while(staticSpawns.Count > 0 && staticSpawns.First ().delay == 0);

      if (staticSpawns.Count > 0) {
        timer.Interval = staticSpawns.First ().delay;
      }      else {
        timer.Stop();
      }

    } else {
      var position = new Vector3 ((float)((randomzier.NextDouble () - 0.5) * 12), 5, 1);
      var color = (Shot.Colors)(randomzier.Next () % 3);
      SpawnEnemy (position, color);
    }

  }

  public void SpawnEnemy (Vector3 position, Shot.Colors color)
  {
						var enemy = Instantiate (enemyPrefab);

    enemy.position = position;
                        enemy.position = new Vector3( (float)((randomzier.NextDouble()-0.5)*12), 5, 1);
			foreach(var renderer in
						(enemy as Transform).GetComponentsInChildren<SpriteRenderer> ())
			{
						enemy.GetComponent<SpriteRenderer> ().color = ConvertToColor ((Shot.Colors)(color));
			}
			foreach(var collider in
			        (enemy as Transform).GetComponentsInChildren<EnemyCollision> ())
			{
							enemyCollision.EnemyColor = (Shot.Colors)color;
				collider.Enemy = (enemy as Transform).gameObject;
						
    }
  }

  public Color ConvertToColor (Shot.Colors gameColor)
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

