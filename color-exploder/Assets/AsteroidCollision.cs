using UnityEngine;
using System.Collections;
using System;
using Colors = ColorUtils.Colors;

public class AsteroidCollision : MonoBehaviour {
  
  public SoundScript sound;
  
  private System.Random randomizer = new System.Random ();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnTriggerEnter2D(Collider2D otherCollider)
  {
    
    Shot bullet = otherCollider.gameObject.GetComponent<Shot>();
    if (bullet != null) {

        if(sound != null)
        {
          sound.Play(SoundScript.SoundList.Explosions);
        }
        GuiScript.AddScore ( (int) (Math.Pow(2, bullet.Magnitude-1)));

      var weapon = GetComponent<ShrapnelScript>();
      if(weapon != null) {

        var randomShots = randomizer.Next (2, 8);

        for(int i=0; i<randomShots; i++)
        {
          var randomDirection = new Vector2((float)(randomizer.NextDouble() - 0.5), (float)(randomizer.NextDouble() - 0.5f));

          weapon.Attack(Colors.white, bullet.Magnitude+1, 8f, randomDirection);
        }
      }

      Destroy (gameObject);
        
      Destroy (bullet.gameObject);
    }
  }
}
