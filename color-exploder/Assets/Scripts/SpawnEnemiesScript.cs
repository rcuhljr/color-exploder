using UnityEngine;
using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using Colors = ColorUtils.Colors;
using AssemblyCSharp;

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
  public BackgroundScript bgScript;
  public Transform asteroidPrefab;
  public SoundScript sound;
  public System.Random randomzier = new System.Random ();
  public Transform bossPrefab1;

  void Start ()
  {
  }

  void Update ()
  {
  }

  public void SpawnRandomEnemy (bool bossOut = false)
  {
    //Drop the enemy into one of thirteen "slots"
    var position = randomzier.Next (0, 13);
    while (Boss1Script.BlockedColumns.Contains(position)) {
      position = randomzier.Next (0, 13);
    };
    var color = (Colors)(randomzier.Next () % 3);
    var cannons = new List<bool>();

    for(int i=0; i<8; i++) {
      if((randomzier.Next() % 3) == 0)
        cannons.Add(true);
      else
        cannons.Add(false);
    }

    if (cannons.All(f=>!f)) {
      var luckyCannon = randomzier.Next () % 8;
      cannons [luckyCannon] = true;
    }
    SpawnEnemy (new Vector3(Constants.slots[position],5,1), color, cannons.ToArray(), (randomzier.Next () % 10) == 1, false);   
  }

  public void SpawnBoss (Boss boss)
  {
    var enemy = Instantiate (bossPrefab1) as Transform;
    enemy.position = boss.position;
	
		for (int i = 0; i<enemy.childCount; i++) 
		{
			if(enemy.GetChild(i).GetComponent<EnemyCollision>() != null)
			{
				enemy.GetChild(i).GetComponent<SpriteRenderer>().color = ColorUtils.ConvertToColor(enemy.GetChild(i).GetComponent<EnemyCollision>().EnemyColor);
			}
			if(enemy.GetChild(i).GetComponent<WeaponScript>() != null)
			{
				enemy.GetChild(i).GetComponent<SpriteRenderer>().color = ColorUtils.ConvertToColor(ColorUtils.Colors.boss);
			}
		}

    //var collider = enemy.GetComponentsInChildren<AsteroidCollision> ().First ();
    //collider.sound = sound;
  }

  public void SpawnAsteroid (Vector3 position)
  {
    var enemy = Instantiate (asteroidPrefab) as Transform;
    enemy.position = position;
    var collider = enemy.GetComponentsInChildren<AsteroidCollision> ().First ();
    collider.sound = sound;
  }
  
  public void SpawnEnemy (Vector3 position, Colors color, bool[] cannons, bool isShielded, bool rotates)
  {
    if (color == bgScript.color) {
      //Debug.Log("attempt to spawn blocked color:"+color+":"+bgScript.color);
      //#TODO Fix with better pick
      color = ColorUtils.GetRandomColorForBackground (color, randomzier);
    }
    var enemy = Instantiate (enemyPrefab) as Transform;
    enemy.tag = "enemy_ship";
    enemy.position = position;
    List<Transform> disabledCannons = new List<Transform> ();

    var enemyMovement = enemy.GetComponent<StraightEnemyScript> ();
    if (enemyMovement != null) {
      enemyMovement.rotates = rotates;
    }

    for (int i=0; i<enemy.childCount; i++) {
      var child = enemy.GetChild (i);
      
      
      var renderer = child.GetComponent<SpriteRenderer> ();
      renderer.color = ColorUtils.ConvertToColor (color);
    }

    int cannonIndex = 0; //this is terrible.

    for (int i=0; i<enemy.childCount; i++) {
      var child = enemy.GetChild (i);


      var renderer = child.GetComponent<SpriteRenderer> ();
      renderer.color = ColorUtils.ConvertToColor (color);

      if (!renderer.enabled) {
        if (cannons[cannonIndex]) {
          renderer.enabled = true;

        } else {
          disabledCannons.Add (child);
        }

        cannonIndex++;
      }           
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
}

