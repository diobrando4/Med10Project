using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class EnemyBoss : BaseClassEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 75;
        currHealth = maxHealth;
        damageGiven = 1;
        //distanceB4Shoot = 15; 
        projectileSpeed = 10f;
        //fireRate = 0.75f;
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

    private float rotationSpeed = 40f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);

        DestroyOnDeath();
    }

    public Transform[] firePoints;

    IEnumerator BulletSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.20f); // this bascially acts as the firerate! lower = faster
            //Debug.Log("BulletSpawner is executed");
            for (int i = 0; i < firePoints.Length; i++)
            {
                BulletController newBullet = Instantiate(bullet, firePoints[i].position, firePoints[i].rotation) as BulletController;
                newBullet.speed = projectileSpeed;
                newBullet.isOldVersion = isOldVersion;
                muzzleSmoke.Play();
                PlaySound("Gunshot");
            }
        }
    }
}
