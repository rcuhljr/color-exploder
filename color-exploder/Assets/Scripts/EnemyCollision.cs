using UnityEngine;
using System.Collections;
using System;

public class EnemyCollision : MonoBehaviour {

	public Shot.Colors EnemyColor;
  public bool isShielded = false;

	public GameObject Enemy;

	public SoundScript sound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// TODO: Do we want to pare down what is allowed to collide?  This makes the player explode always.
		
		
		Shot bullet = otherCollider.gameObject.GetComponent<Shot>();
		if (bullet != null) {
			if (bullet.ShotColor == (int)EnemyColor || (!isShielded && bullet.ShotColor == (int)Shot.Colors.player)) {
				if(sound != null)
				{
					sound.Play(SoundScript.SoundList.Explosions);
				}
				Destroy (Enemy);
				GuiScript.AddScore ( (int) (Math.Pow(2, bullet.Magnitude-1)));
				foreach(var cannon in 
				Enemy.GetComponentsInChildren<WeaponScript>())
				{
					cannon.Attack(EnemyColor, bullet.Magnitude+1);
				}
			}

            Destroy (bullet.gameObject);
		}
		
	}
}
