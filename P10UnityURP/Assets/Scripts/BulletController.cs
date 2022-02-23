using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;

    public float lifeTime = 3.0f; // in seconds?
    
    public int damageGiven = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // destroys the bullet after having reached the end of its life time
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    // for when bullets hit enemies
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<ZombieHealthManager>().HurtZombie(damageGiven);

            // destroys the bullet when hitting the enemy
            Destroy(gameObject);
        }
    }
}
