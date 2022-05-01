using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        //muzzleSmoke = gameObject.transform.Find("EnemyGun/SmokeParticles").GetComponent<ParticleSystem>();
        
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
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 25f, 0) * Time.deltaTime);

        DestroyOnDeath();
        //Shooting();

        cooldownTimer += Time.deltaTime;
        Debug.Log(cooldownTimer);
        if (cooldownTimer >= attackCooldown)
        {
            //Attack();
        } 
    }

    private float attackCooldown;
    private float cooldownTimer;
    public Transform firePoint1;

    void Attack()
    {
        cooldownTimer = 0;

        BulletController newBullet = Instantiate(bullet, firePoint1.position, firePoint1.rotation) as BulletController;
        PlaySound("Gunshot");
        newBullet.speed = projectileSpeed;
        muzzleSmoke.Play();
    }

    void Shooting()
    {
        /*
            shotCounter -= Time.deltaTime;

            if(shotCounter <= 0)
            {        
                shotCounter = fireRate;
                //BulletController newBullet = Instantiate(bullet, muzzle.position, muzzle.rotation) as BulletController;

                //BulletController newBullet = Instantiate(bullet, firePoints[firePoints.Length].position, firePoints[firePoints.Length].rotation) as BulletController;
                //BulletController newBullet = Instantiate(bullet, testFirePoint.position, testFirePoint.rotation) as BulletController;
                //BulletController newBullet = Instantiate(bullet, firePoint1.position, firePoint1.rotation) as BulletController;

                //clone = Instantiate(zombiePrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

                PlaySound("Gunshot");
                bullet.speed = projectileSpeed;
                muzzleSmoke.Play();
            }
            else
            {
                shotCounter = 0;
            }
        */
    }
}
