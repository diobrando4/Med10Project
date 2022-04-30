using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShooter : BaseClassEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 5;
        currHealth = maxHealth;
        damageGiven = 1;
        distanceB4Shoot = 15; 
        projectileSpeed = 10f;
        fireRate = 0.75f;
        muzzle = gameObject.transform.Find("EnemyGun/Muzzle");
        muzzleSmoke = gameObject.transform.Find("EnemyGun/SmokeParticles").GetComponent<ParticleSystem>();
        
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        else
        {
            Debug.Log("Missing NavMeshAgent on " + gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DestroyOnDeath();
        target = FindClosestTargetWithTag("GoodGuys");

        if (target != null && isDead == false)
        {
            if (inCombat == true)
            {
            Move2Target(target,5,3,6);
            ShootNearestObject(target);
            }
        }
    }
}
