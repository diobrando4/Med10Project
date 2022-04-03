using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BulletController
{

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;

        // destroys the bullet after having reached the end of its life time
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    // for when bullets hit something (seems like you need rigidbody to make this work)
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Projectile")
        {
            if(other.gameObject.tag == "GoodGuys") //If it hits something with tag GoodGuys
            {
                HurtNPCType(other.gameObject,damageGiven); //Hurt the given gO that was collided with
                Destroy(gameObject);
            }
            else if(other.gameObject.tag == "Player") //If it hits something with tag Player
            {
                HurtPlayerType(other.gameObject,damageGiven); //Hurt player
                Destroy(gameObject);
            }
            else if(other.gameObject.tag == "Enemy")
            {

            }
            else //If it collides with anything else, Destroy self
            {
                Destroy(gameObject);
            }
        }
    } // OnCollisionEnter
}
