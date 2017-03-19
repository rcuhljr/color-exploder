using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Colors = ColorUtils.Colors;

public class PlayerShipMovement : MonoBehaviour {

	private const float MaxX = 6.5f;
	private const float MinX = -6.5f;
	private const float MaxY = 4.5f;
	private const float MinY = -4.5f;

    private const float MoveSpeed = 5f;

    public int PlayerNumber;

    private class PlayerInputs
    {
        public string Horizontal;
        public string Vertical;
        public string Fire1;
        public string Fire2;
        public string Jump;
    }

    private List<PlayerInputs> inputs = new List<PlayerInputs>
    {
        new PlayerInputs { Horizontal = "Horizontal", Vertical = "Vertical", Fire1 = "Fire1", Fire2 = "Fire2", Jump = "Jump" },
        new PlayerInputs { Horizontal = "P2Horizontal", Vertical = "P2Vertical", Fire1 = "P2Fire1", Fire2 = "P2Fire2", Jump = "P2Jump" },
        new PlayerInputs { Horizontal = "P3Horizontal", Vertical = "P3Vertical", Fire1 = "P3Fire1", Fire2 = "P3Fire2", Jump = "P3Jump" },
        new PlayerInputs { Horizontal = "P4Horizontal", Vertical = "P4Vertical", Fire1 = "P4Fire1", Fire2 = "P4Fire2", Jump = "P4Jump" },
    };
  
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int index = PlayerNumber - 1;
        bool shoot = Input.GetButton(inputs[index].Fire1);
        shoot |= Input.GetButton(inputs[index].Fire2);
		
		if (shoot)
		{
			WeaponScript weapon = GetComponent<WeaponScript>();
			if (weapon != null)
			{
				weapon.Attack(Colors.player, 1, 1f);
			}
		}

        bool bomb = Input.GetButtonDown(inputs[index].Jump);

    if (bomb)
    {
        ColorBombScript weapon = GetComponent<ColorBombScript>();
        if (weapon != null)
        {
            weapon.DropBomb();
        }
    };
    var rigidbody2D = GetComponent<Rigidbody2D>();
    //if (rigidbody2D.velocity.magnitude > maxSpeed)
    //{
    //    var scale = maxSpeed / rigidbody2D.velocity.magnitude;
    //    rigidbody2D.velocity.Scale(new Vector3(scale, scale, 0));
    //}

    // Cache the horizontal input.
    float h = Input.GetAxis(inputs[index].Horizontal);

    //if ( && rigidbody2D.velocity.x > 0) {
    //	rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
    //	rigidbody2D.position = new Vector2 (6.5f, rigidbody2D.position.y);
    //}
    //if (rigidbody2D.position.x <= -6.5 && rigidbody2D.velocity.x < 0) {
    //	rigidbody2D.velocity = new Vector2 (0, rigidbody2D.velocity.y);
    //	rigidbody2D.position = new Vector2 (-6.5f, rigidbody2D.position.y);
    //}

    if (h != 0f)
    {
        //if (h * rigidbody2D.velocity.x < maxSpeed)
        //{
        //    if ((rigidbody2D.position.x < MaxX && h > 0) || (rigidbody2D.position.x > MinX && h < 0))

        //        rigidbody2D.AddForce(Vector2.right * h * moveForce);
        //}

        rigidbody2D.velocity = new Vector2(MoveSpeed * h, rigidbody2D.velocity.y);

        //if (Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
        //    rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
    }
    else
    {
        rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
    }


    float v = Input.GetAxis(inputs[index].Vertical);

    if (v != 0f)
    {
        rigidbody2D.velocity = new Vector2(MoveSpeed * h, MoveSpeed * v);
        //if (v * rigidbody2D.velocity.y < maxSpeed)
        //    if ((rigidbody2D.position.y < MaxY && v > 0) || (rigidbody2D.position.y > MinY && v < 0))
        //        rigidbody2D.AddForce(Vector2.up * v * moveForce);
        //if (Mathf.Abs(rigidbody2D.velocity.y) > maxSpeed)
        //    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Sign(rigidbody2D.velocity.y) * maxSpeed);

    }
    else
    {
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
    }

    if (rigidbody2D.position.x < MinX || rigidbody2D.position.x > MaxX)
    {
        rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        if (rigidbody2D.position.x > 0)
            rigidbody2D.position = new Vector2(MaxX, rigidbody2D.position.y);
        else
            rigidbody2D.position = new Vector2(MinX, rigidbody2D.position.y);
    }

    if (rigidbody2D.position.y < MinY || rigidbody2D.position.y > MaxY)
    {
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        if (rigidbody2D.position.y > 0)
            rigidbody2D.position = new Vector2(rigidbody2D.position.x, MaxY);
        else
            rigidbody2D.position = new Vector2(rigidbody2D.position.x, MinY);
    }
	}
}
