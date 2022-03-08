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
            other.gameObject.GetComponent<ZombieHealthManager>().HurtZombie(damageGiven);

            // destroys the bullet when hitting the enemy
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Wall")
        {
            //Debug.Log("a bullet hit a wall");
            // destroys the bullet when hitting a wall
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "GoodGuys")
        {
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
