using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // i was told that in order to avoid having to put rigidbodies on every single wall; 
    // i instead have to use rigidbody and not transform (translate) to move the bullet 
    // and to use continuos collision detection (CCD) (instead of collision enter?) 
    // and i think you need to use fixedupdate; since that is using the physics engine?

    public float speed;
    public float lifeTime = 3.0f; // in seconds?
    public int damageGiven = 1;
    public Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // this needs to use rigidbody instead!
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);

        rb.velocity = transform.forward * speed;

        // destroys the bullet after having reached the end of its life time
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    // for when bullets hit something (seems like you need rigidbody to make this work)
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            // need to use TryGetComponent for this one!
            //other.gameObject.GetComponent<EnemyZombie>().DamageTaken(damageGiven);

            // so player/ally can deal damage to zombies
            if(other.transform.TryGetComponent<EnemyZombie>(out EnemyZombie _enemyZombie))
            {
                _enemyZombie.DamageTaken(damageGiven);
            }
            // so player/ally can deal damage to shooters
            if(other.transform.TryGetComponent<EnemyShooter>(out EnemyShooter _enemyShooter))
            {
                _enemyShooter.DamageTaken(damageGiven);
            }

            // destroys the bullet when hitting the enemy
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Untagged")
        {
            // destroys the bullet when hitting a wall
            Destroy(gameObject);
        }
        // friendly fire
        if(other.gameObject.tag == "GoodGuys" || other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    } // OnCollisionEnter
}
