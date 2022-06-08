using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BulletController
{
    void Start()
    {
        lifeTime = 3.0f;
        damageGiven = 1;
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
    void OnCollisionEnter(Collision col)
    {
        //EnemyBulletFilter(col,0); 
        EnemyBulletFilter(col,0,false); //Set to true to enable projectiles old behavior
    } // OnCollisionEnter
}
