using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
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
        if(other.gameObject.tag == "GoodGuys" || other.gameObject.tag == "Player")
        {
            // need to use TryGetComponent for this one!
            //other.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damageGiven);

            // so enemy can deal damage to player
            if(other.transform.TryGetComponent<PlayerHealthManager>(out PlayerHealthManager _playerHealthManager))
            {
                _playerHealthManager.HurtPlayer(damageGiven);
            }

            //other.gameObject.GetComponent<Ally>().DamageTaken(damageGiven);

            // so enemy can deal damage to ally
            if(other.transform.TryGetComponent<Ally>(out Ally _ally))
            {
                _ally.DamageTaken(damageGiven);
            }

            // destroys the bullet when hitting the enemy
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Untagged")
        {
            Destroy(gameObject);
        }
        // friendly fire
        if(other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    } // OnCollisionEnter
}
