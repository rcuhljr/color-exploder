using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour {

	public enum Colors {red, blue, green, cyan, yellow, magenta, white, player };
			
	/// <summary>
	/// Object speed
	/// </summary>
	public Vector2 speed = new Vector2(10, 10);
	
	/// <summary>
	/// Moving direction
	/// </summary>
	public Vector2 direction = new Vector2(-1, 0);

	public int ShotColor = (int)Colors.player;
	
	private Vector2 movement;

	void Start () {
		movement = new Vector2(
			speed.x * direction.x,
			speed.y * direction.y);

		Destroy(gameObject, 4);
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody2D.velocity = movement;
	}
}
