﻿using UnityEngine;
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
        //--------------------------------
        // 1 - Designer variables
        //--------------------------------

        /// <summary>
        /// Projectile prefab for shooting
        /// </summary>
        public Transform enemyPrefab;

		public SoundScript sound;

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

		for (int i=0; i<enemyPrefab.childCount; i++) {
			var child = enemyPrefab.GetChild(i);
			if(child.name == "Cannon")
			{
				child.GetComponent<SpriteRenderer>().enabled = false;
			}
				}
        }

        void Update ()
        {
                Spawn ();
        }

  public void Stop ()
  {
    staticSpawns = null;
    timer.Stop ();
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
    this.staticSpawns = new List<SpawnInfo>(staticSpawns);
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
          if (readyToSpawn) {
                        readyToSpawn = false;

                        if (staticSpawns != null) {
                                // Create a new enemy
                                var spawn = staticSpawns.First ();
                                staticSpawns.RemoveAt (0);

                                SpawnEnemy (new Vector3 (spawn.x, spawn.y, 1), spawn.color, spawn.shielded);
                                while (staticSpawns.Count > 0 && staticSpawns.First ().delay == 0)
                                        ;

                                if (staticSpawns.Count > 0) {
                                        timer.Interval = staticSpawns.First ().delay;
                                } else {
                                        timer.Stop ();
                                }
                        } else {
                                var position = new Vector3 ((float)((randomzier.NextDouble () - 0.5) * 12), 5, 1);
                                var color = (Shot.Colors)(randomzier.Next () % 3);
                                SpawnEnemy (position, color, (randomzier.Next() % 10) == 1);
                        }
                }
        }

	  public void SpawnEnemy (Vector3 position, Shot.Colors color, bool isShielded)
	  {
						var enemy = Instantiate (enemyPrefab) as Transform;
      enemy.position = position;
      List<Transform> disabledCannons = new List<Transform> ();

		for(int i=0; i<enemy.childCount; i++)
		{
			var child = enemy.GetChild(i);


			var renderer = child.GetComponent<SpriteRenderer> ();
			    renderer.color = ConvertToColor (color);

			if(!renderer.enabled )
			{
				if(randomzier.Next()%4 == 0)
				{
					renderer.enabled = true;
			}
				else
				{
					disabledCannons.Add(child);
				}
			}		        
		}
		if (disabledCannons.Count>3) 
		{
			var luckyCannon = randomzier.Next()%3;
			disabledCannons[luckyCannon].GetComponent<SpriteRenderer>().enabled = true;
			disabledCannons.RemoveAt(luckyCannon);
		}
    foreach (var collider in
			        (enemy as Transform).GetComponentsInChildren<EnemyCollision> ()) {
      collider.EnemyColor = color;
          collider.isShielded = isShielded;
				  collider.Enemy = (enemy as Transform).gameObject;
				  collider.DisabledCannons = disabledCannons;
collider.sound = sound;
    }

    if (isShielded) {
      var hull = (enemy as Transform).FindChild ("Hull");
      hull.GetComponent<SpriteRenderer> ().sprite =
        Resources.Load<Sprite> ("Textures/EnemyShip-Shielded");
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

