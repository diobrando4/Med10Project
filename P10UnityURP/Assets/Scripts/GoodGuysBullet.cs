using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodGuysBullet : BulletController
{

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
            //Function inherited from Parent, refers to the enemy's BaseClassNPC Component 
            HurtNPCType(other.gameObject,damageGiven);
            // destroys the bullet when hitting the enemy
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);   
        }
    } // OnCollisionEnter
}
