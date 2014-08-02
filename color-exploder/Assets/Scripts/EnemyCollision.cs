using UnityEngine;
using System.Collections;

public class EnemyCollision : MonoBehaviour {

	public Shot.Colors EnemyColor;

	public GameObject Enemy;

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
			if (bullet.ShotColor == (int)EnemyColor || bullet.ShotColor == (int)Shot.Colors.player) {
				Destroy (Enemy);
				GuiScript.AddScore (bullet.Score);
			}

            Destroy (bullet.gameObject);
		}
		
	}
}
