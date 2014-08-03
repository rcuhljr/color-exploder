using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Math = System.Math;
using Colors = ColorUtils.Colors;

public class EnemyCollision : MonoBehaviour {

	public List<Transform> DisabledCannons {
		get;
		set;
	}

	public Colors EnemyColor;
  public bool isShielded = false;

	public GameObject Enemy;

	public SoundScript sound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// TODO: Do we want to pare down what is allowed to collide?  This makes the player explode always.
		
		
		Shot bullet = otherCollider.gameObject.GetComponent<Shot>();
		if (bullet != null) {
      if (ColorUtils.IsAffectedByBullets((Colors)bullet.ShotColor, (Colors)EnemyColor, isShielded)) {
				if(sound != null)
				{
					sound.Play(SoundScript.SoundList.Explosions);
				}
				Destroy (Enemy);
				GuiScript.AddScore ( (int) (Math.Pow(2, bullet.Magnitude-1)));
				foreach(var cannon in 
				Enemy.GetComponentsInChildren<WeaponScript>())
				{
					if(!DisabledCannons.Any(dis=>dis.GetComponent<WeaponScript>() == cannon))
					{
            cannon.Attack(GetCompositeColor((Colors)EnemyColor), bullet.Magnitude+1, 0.4f);
					}
				}
			}

            Destroy (bullet.gameObject);
		}
		
	}

  Colors GetCompositeColor(Colors baseColor)
  {
    var bgManager = GameObject.FindGameObjectWithTag ("BG-Color");

    var bgScript = bgManager.GetComponent<BackgroundScript>();

    if(bgScript != null) {
      var bgColor = bgScript.color;

      return ColorUtils.GetCompositeColor(baseColor, bgColor);
    }

    return baseColor;
  }
}
