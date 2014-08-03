using UnityEngine;
using System.Collections;
using System;

public class Boss1Script : MonoBehaviour {

	public Transform cannon1;
	public Transform cannon2;

	public Transform sensor1;
	public Transform sensor2;

  public static int[] BlockedColumns = new int[6]{4,5,6,7,8,9};

	private const float shiftBounds = 3;
	private float xShift = 0;
	private bool xdirection = true;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (sensor1 == null && sensor2 == null) {
			Destroy (gameObject);
				}

		cannon1.GetComponent<WeaponScript> ().Attack (ColorUtils.Colors.boss, 2, 1);
		cannon2.GetComponent<WeaponScript> ().Attack (ColorUtils.Colors.boss, 2, 1);

		if(Math.Abs(xShift) >= shiftBounds)
	    {
			xdirection = !xdirection;
		}

		xShift += (xdirection ? .03f : -.03f);
		transform.position = new Vector3(transform.position.x + (xdirection ? .03f : -.03f), transform.position.y, transform.position.z);
	}
}
