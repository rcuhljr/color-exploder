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
    public System.Random randomizer = new System.Random ();
    public Transform bossPrefab1;
	public Transform bossPrefab2;
	public Transform bossPrefab3;

  void Start ()
  {
  }

  void Update ()
  {
  }

  public void SpawnRandomEnemy (bool bossout)
  {
    //Drop the enemy into one of thirteen "slots"
    var position = randomizer.Next (0, 13);
    while (Boss1Script.BlockedColumns.Contains(position)) {
      position = randomizer.Next (0, 13);
    };
    var color = (Colors)(randomizer.Next () % 3);
    var cannons = new List<bool>();

    for(int i=0; i<8; i++) {
      if((randomizer.Next() % 3) == 0)
        cannons.Add(true);
      else
        cannons.Add(false);
    }

    if (cannons.All(f=>!f)) {
      var luckyCannon = randomizer.Next () % 8;
      cannons [luckyCannon] = true;
    }

    var spawn = new Spawn(new Vector3(Constants.slots[position], 5, 1), color, cannons.ToArray(), (randomizer.Next() % 10) == 1, false);
    SpawnEnemy(spawn);
  }

  public void SpawnBoss (Boss boss)
  {

		Transform enemy;

		switch(boss.bossId)
		{
		case 1:
			enemy = Instantiate (bossPrefab1) as Transform;
			break;
		case 2:
			enemy = Instantiate (bossPrefab2) as Transform;
             break;
	     case 3:
	     enemy = Instantiate (bossPrefab3) as Transform;
	     break;
		default:
	     enemy = Instantiate (bossPrefab1) as Transform;
	     break;
     	}
        enemy.position = boss.position;
	
		for (int i = 0; i<enemy.childCount; i++) 
		{
			if(enemy.GetChild(i).GetComponent<EnemyCollision>() != null)
			{
				enemy.GetChild(i).GetComponent<SpriteRenderer>().color = ColorUtils.ConvertToColor(enemy.GetChild(i).GetComponent<EnemyCollision>().EnemyColor);
				enemy.GetChild(i).GetComponent<EnemyCollision>().sound = sound;
			}
			if(enemy.GetChild(i).GetComponent<WeaponScript>() != null)
			{
				enemy.GetChild(i).GetComponent<SpriteRenderer>().color = ColorUtils.ConvertToColor(ColorUtils.Colors.boss);
				enemy.GetChild(i).GetComponent<WeaponScript>().sound = sound;
			}
		}

    //var collider = enemy.GetComponentsInChildren<AsteroidCollision> ().First ();
    //collider.sound = sound;
  }

  public void SpawnAsteroid (Vector3 position)
  {
      var enemy = Instantiate(asteroidPrefab) as Transform;
    enemy.position = position;
    var collider = enemy.GetComponentsInChildren<AsteroidCollision> ().First ();
    collider.sound = sound;
  }

  public void SpawnEnemy(Spawn spawn)
  {
      SpawnEnemy(spawn.position, spawn.color, spawn.cannons, spawn.shielded, spawn.rotated);
  }
  
  public void SpawnEnemy (Vector3 position, Colors color, bool[] cannons, bool isShielded, bool rotates)
  {
    if (color == bgScript.color || !ColorUtils.IsAllowedForBackground(color, bgScript.color)) {
      //Debug.Log("attempt to spawn blocked color:"+color+":"+bgScript.color);
      //#TODO Fix with better pick
      color = ColorUtils.GetRandomColorForBackground (bgScript.color, randomizer);
    }
    var enemy = Instantiate(enemyPrefab) as Transform;
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

