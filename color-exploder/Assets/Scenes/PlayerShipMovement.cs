using UnityEngine;
using System.Collections;

public class PlayerShipMovement : MonoBehaviour {

	public float moveForce = 365f;			
	public float maxSpeed = 5f;		
	public float dragRate = .1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");

		if (h != 0f) {
						if (h * rigidbody2D.velocity.x < maxSpeed) 			
								rigidbody2D.AddForce (Vector2.right * h * moveForce); 		 		
						if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
								rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
				} else {
			rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x * dragRate, rigidbody2D.velocity.y);
		}


		float v = Input.GetAxis("Vertical");

		if (v != 0f) {
		
						if (v * rigidbody2D.velocity.y < maxSpeed) 			
								rigidbody2D.AddForce (Vector2.up * v * moveForce); 		 		
						if (Mathf.Abs (rigidbody2D.velocity.y) > maxSpeed)
								rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x, Mathf.Sign (rigidbody2D.velocity.y) * maxSpeed);

		} else {
			rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x, rigidbody2D.velocity.y * dragRate);
		}
	}
}
