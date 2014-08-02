using UnityEngine;
using System.Collections;

public class StraightEnemyScript : MonoBehaviour {
	/// <summary>
	/// Object speed
	/// </summary>
	public Vector2 speed = new Vector2(10, 10);

	private Vector2 movement;
	
	/// <summary>
	/// Moving direction
	/// </summary>
	public Vector2 direction = new Vector2(-1, 0);
	// Use this for initialization
	void Start () {
		movement = new Vector2(
			speed.x * direction.x,
			speed.y * direction.y);
		
		Destroy(gameObject, 7);
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody2D.velocity = movement;
	}
}
