﻿using UnityEngine;
using System.Collections;
using System;

public class Boss1Script : MonoBehaviour {

	public Transform cannon1;
	public Transform cannon2;

	public Transform sensor1;
	public Transform sensor2;

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
		if (sensor1 == null && sensor2 == null) {
			Destroy (gameObject);
				}

		cannon1.GetComponent<WeaponScript> ().Attack (ColorUtils.Colors.boss, 2, 1);
		cannon2.GetComponent<WeaponScript> ().Attack (ColorUtils.Colors.boss, 2, 1);

		if(xShift >= shiftBounds * (xdirection ? .5f : -.5f))
	    {
			xdirection = !xdirection;
		}

		xShift += (xdirection ? .5f : -.5f);
		transform.position = new Vector3(transform.position.x + (xdirection ? .5f : -.5f), transform.position.y, transform.position.z);
	}
}