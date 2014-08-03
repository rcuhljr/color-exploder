using UnityEngine;
using System.Collections;
using Colors = ColorUtils.Colors;

public class Shot : MonoBehaviour {

	
	/// <summary>
	/// Object speed
	/// </summary>
	public Vector2 speed = new Vector2(10, 10);
	
	/// <summary>
	/// Moving direction
	/// </summary>
	public Vector2 direction = new Vector2(-1, 0);

	public int ShotColor = (int)Colors.player;

	public int Magnitude = 1;
	
	private Vector2 movement;

	void Start () {
		movement = new Vector2(
			speed.x * direction.x,
			speed.y * direction.y - 0.8F);

		Destroy(gameObject, 4);
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody2D.velocity = movement;
	}
}
