using UnityEngine;
using System.Collections;

public class OptionsNav : MonoBehaviour {

	public Transform overlay;
	public Transform foreGround;
	public bool isMain = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown()
	{
		overlay.gameObject.SetActive (isMain);
		foreGround.gameObject.SetActive (isMain);
		}
}
