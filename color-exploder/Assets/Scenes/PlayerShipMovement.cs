using UnityEngine;
using System.Collections;

public class PlayerShipMovement : MonoBehaviour {

	public float moveForce = 365f;			
	public float maxSpeed = 5f;		
	public float dragRate = .025f;

	private const float MaxX = 6.5f;
	private const float MinX = -6.5f;
	private const float MaxY = 4.5f;
	private const float MinY = -4.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		bool shoot = Input.GetButtonDown("Fire1");
		shoot |= Input.GetButtonDown("Fire2");
		
		if (shoot)
		{
			WeaponScript weapon = GetComponent<WeaponScript>();
			if (weapon != null)
			{
				weapon.Attack(Shot.Colors.player, 1);
			}
		}
		
		if(rigidbody2D.velocity.magnitude > maxSpeed)
		{
			var scale = maxSpeed / rigidbody2D.velocity.magnitude;
			rigidbody2D.velocity.Scale( new Vector3(scale, scale, 0));
 		}

		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");

		//if ( && rigidbody2D.velocity.x > 0) {
		//	rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
		//	rigidbody2D.position = new Vector2 (6.5f, rigidbody2D.position.y);
		//}
		//if (rigidbody2D.position.x <= -6.5 && rigidbody2D.velocity.x < 0) {
		//	rigidbody2D.velocity = new Vector2 (0, rigidbody2D.velocity.y);
		//	rigidbody2D.position = new Vector2 (-6.5f, rigidbody2D.position.y);
		//}

		if (h != 0f) {
			if (h * rigidbody2D.velocity.x < maxSpeed)
			{
				if((rigidbody2D.position.x < MaxX && h > 0) || (rigidbody2D.position.x > MinX && h < 0))
					rigidbody2D.AddForce (Vector2.right * h * moveForce);
			}
								
			if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
				rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
				} else {
			rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x * dragRate, rigidbody2D.velocity.y);
		}


		float v = Input.GetAxis("Vertical");

		if (v != 0f) {
						if (v * rigidbody2D.velocity.y < maxSpeed) 	
							if((rigidbody2D.position.y < MaxY && h > 0) || (rigidbody2D.position.y > -MinY && h < 0))
								rigidbody2D.AddForce (Vector2.up * v * moveForce); 		 		
						if (Mathf.Abs (rigidbody2D.velocity.y) > maxSpeed)
								rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x, Mathf.Sign (rigidbody2D.velocity.y) * maxSpeed);

		} else {
			rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x, rigidbody2D.velocity.y * dragRate);
		}

		if (rigidbody2D.position.x < MinX || rigidbody2D.position.x > MaxX)
		{
			rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
			if(rigidbody2D.position.x > 0)
				rigidbody2D.position = new Vector2(MaxX, rigidbody2D.position.y);
			else
				rigidbody2D.position = new Vector2(MinX, rigidbody2D.position.y);
		}

		if (rigidbody2D.position.y < MinY || rigidbody2D.position.y > MaxY) {
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
			if(rigidbody2D.position.y > 0)
				rigidbody2D.position = new Vector2(rigidbody2D.position.x, MaxY);
			else
				rigidbody2D.position = new Vector2(rigidbody2D.position.x, MinY);
				}
	}
}
