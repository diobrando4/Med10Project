using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class EnemyBoss : BaseClassEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 25;
        currHealth = maxHealth;
        damageGiven = 1;
        distanceB4Shoot = 15; 
        projectileSpeed = 10f;
        fireRate = 0.75f;
        //muzzle = gameObject.transform.Find("EnemyGun/Muzzle");
        muzzleSmoke = gameObject.transform.Find("EnemyGun/SmokeParticles").GetComponent<ParticleSystem>();
        
        /*
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        else
        {
            Debug.Log("Missing NavMeshAgent on " + gameObject);
        }
        */
        
        StartCoroutine(BulletSpawner());
    }

    public float timeBetweenBullets;
    private float bulletCounter;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 25f, 0) * Time.deltaTime);

        DestroyOnDeath();
    }

    private float attackCooldown;
    private float cooldownTimer;
    public Transform firePoint1;
    public Transform firePoint2;

    IEnumerator BulletSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            //Debug.Log("BulletSpawner is running");
            // needs a for loop for multiple shooting points
            BulletController newBullet = Instantiate(bullet, firePoint1.position, firePoint1.rotation) as BulletController;
            PlaySound("Gunshot");
            newBullet.speed = projectileSpeed;
            muzzleSmoke.Play();
        }
    }
}
