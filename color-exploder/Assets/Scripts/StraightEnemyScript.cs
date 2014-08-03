using UnityEngine;
using System.Collections;
using System.Timers;
using System;

public class StraightEnemyScript : MonoBehaviour {
	/// <summary>
	/// Object speed
	/// </summary>
	public Vector2 speed = new Vector2(10, 10);

  public bool rotates = false;
  private bool shouldRotate = false;

  private Timer rotateTimer;

	private Vector2 movement;
	
	/// <summary>
	/// Moving direction
	/// </summary>
	public Vector2 direction = new Vector2(-1, 0);
	// Use this for initialization
	void Start () {
    if (rotates) {
      rotateTimer = new Timer (1500);
      rotateTimer.Elapsed += Rotate;
      rotateTimer.Start ();
    }
		movement = new Vector2(
			speed.x * direction.x,
			speed.y * direction.y);
		
		Destroy(gameObject, 12);
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Rigidbody2D body in
		gameObject.GetComponentsInChildren<Rigidbody2D> ()) {
						body.velocity = movement;
				}

    if (shouldRotate) {
      shouldRotate = false;
      var body = gameObject.transform.FindChild("Hull");
      var bodyBody = body.GetComponent<Rigidbody2D>();

      //gameObject.transform.position = bodyBody.position;
      gameObject.transform.RotateAround(bodyBody.position, Vector3.forward, 45);
//      foreach (Rigidbody2D child in
//               gameObject.GetComponentsInChildren<Rigidbody2D> ()) {
//        child.transform.RotateAround(bodyBody.position, Vector3.left, (Mathf.PI/4));
//       }
    }
	}

  void Rotate(object sender, EventArgs args)
  {
    shouldRotate = true;
  }
}
