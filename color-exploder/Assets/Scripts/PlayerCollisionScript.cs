using UnityEngine;
using System.Collections;

public class PlayerCollisionScript : MonoBehaviour
{

    public float invulnerabilityTime = 0;
    public GameObject explosionPrefab;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (invulnerabilityTime > 0)
        {
            invulnerabilityTime -= Time.deltaTime;
        }
        else
        {
            var renderer = gameObject.GetComponent<SpriteRenderer>();
            var color = renderer.color;
            if (color.a < 1.0)
            {
                renderer.color = new Color(color.r, color.g, color.b, 1.0f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // TODO: Do we want to pare down what is allowed to collide?  This makes the player explode always.


        Shot bullet = otherCollider.gameObject.GetComponent<Shot>();
        if (bullet != null)
        {
            if (bullet.ShotColor != (int)ColorUtils.Colors.player)
            {
                if (invulnerabilityTime <= 0)
                {
                    Explode();
                    Destroy(bullet.gameObject);
                }
            }
        }

        EnemyCollision enemy = otherCollider.gameObject.GetComponent<EnemyCollision>();
        if (enemy != null)
        {
            if (invulnerabilityTime <= 0)
            {
                Explode();
                Destroy(enemy.gameObject);
            }
        }
    }

    private void Explode()
    {
        var explosion = Instantiate(explosionPrefab);
        explosion.transform.position = gameObject.transform.position;
        Destroy(gameObject);
        Destroy(explosion, 0.75f);
    }
}
