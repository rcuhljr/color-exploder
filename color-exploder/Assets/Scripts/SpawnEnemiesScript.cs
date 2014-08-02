using UnityEngine;
using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using Colors = ColorUtils.Colors;

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

    public Transform asteroidPrefab;

		public SoundScript sound;

        /// <summary>
        /// Cooldown in seconds between two enemies
        /// </summary>
        private bool readyToSpawn = false;
        public System.Random randomzier = new System.Random ();


  void Start ()
  {
  }

  void Update ()
  {
  }

  public void SpawnRandomEnemy(){
    var position = new Vector3 ((float)((randomzier.NextDouble () - 0.5) * 12), 5, 1);
    var color = (Colors)(randomzier.Next () % 3);
    SpawnEnemy (position, color, (randomzier.Next () % 10) == 1, false);   
    }

    public void SpawnAsteroid(Vector3 position) {
      var enemy = Instantiate (asteroidPrefab) as Transform;
      enemy.position = position;
		var collider = enemy.GetComponentsInChildren<AsteroidCollision>().First();
		collider.sound = sound;
	}
	
	public void SpawnEnemy (Vector3 position, Colors color, bool isShielded, bool rotates)
	  {
						var enemy = Instantiate (enemyPrefab) as Transform;
      enemy.position = position;
            List<Transform> disabledCannons = new List<Transform> ();

    var enemyMovement = enemy.GetComponent<StraightEnemyScript> ();
    if (enemyMovement != null) {
      enemyMovement.rotates = rotates;
    }

		for(int i=0; i<enemy.childCount; i++)
		{
			var child = enemy.GetChild(i);


			var renderer = child.GetComponent<SpriteRenderer> ();
			    renderer.color = ConvertToColor (color);

			if(!renderer.enabled )
			{
				if(randomzier.Next()%3 == 0)
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

  public Color ConvertToColor (Colors gameColor)
  {
    switch (gameColor) {
    case Colors.blue:
      return Color.blue;
    case Colors.green:
      return Color.green;
    case Colors.red:
      return Color.red;
    default:
      return Color.white;
    }
  }
}

