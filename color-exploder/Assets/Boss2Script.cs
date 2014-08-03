using UnityEngine;
using System.Collections;
using System;

public class Boss2Script : MonoBehaviour {
	
	public Transform cannon1;
	public Transform cannon2;
	public Transform cannon3;
	public Transform cannon4;
	
	public Transform sensor1;
	public Transform sensor2;
	public Transform sensor3;
	public Transform sensor4;
	
	public static int[] BlockedColumns = new int[6]{4,5,6,7,8,9};
	
	private const float shiftBounds = 2;
	private float xShift = 0;
	private bool xdirection = true;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (sensor1 == null && sensor2 == null&& sensor3 == null&& sensor4 == null) {
			Destroy (gameObject);
		}
		
		cannon1.GetComponent<WeaponScript> ().Attack (ColorUtils.Colors.boss, 2, 1);
		cannon2.GetComponent<WeaponScript> ().Attack (ColorUtils.Colors.boss, 2, 1);
		cannon3.GetComponent<WeaponScript> ().Attack (ColorUtils.Colors.boss, 2, 1);
		cannon4.GetComponent<WeaponScript> ().Attack (ColorUtils.Colors.boss, 2, 1);
		
		if(Math.Abs(xShift) >= shiftBounds)
		{
			xdirection = !xdirection;
		}
		
		xShift += (xdirection ? .02f : -.02f);
		transform.position = new Vector3(transform.position.x + (xdirection ? .02f : -.02f), transform.position.y, transform.position.z);
	}
}
