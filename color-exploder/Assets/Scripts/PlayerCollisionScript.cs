﻿using UnityEngine;
using System.Collections;

public class PlayerCollisionScript : MonoBehaviour
{

  // Use this for initialization
  void Start ()
  {
  
  }
  
  // Update is called once per frame
  void Update ()
  {
  
  }

  void OnTriggerEnter2D (Collider2D otherCollider)
  {
    // TODO: Do we want to pare down what is allowed to collide?  This makes the player explode always.
        
        
    Shot bullet = otherCollider.gameObject.GetComponent<Shot> ();
    if (bullet != null) {
      if (bullet.ShotColor != (int)ColorUtils.Colors.player) {
        Destroy (gameObject);
      }
    }
        
    EnemyCollision enemy = otherCollider.gameObject.GetComponent<EnemyCollision> ();
    if (enemy != null) {
      Destroy (gameObject);
      Destroy (enemy.gameObject);
    }
  }
}
