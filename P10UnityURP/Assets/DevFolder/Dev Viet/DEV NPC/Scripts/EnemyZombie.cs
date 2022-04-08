using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Child of BaseClassEnemy, Zombie enemy. A simple suicide damage dealer

public class EnemyZombie : BaseClassEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 5;
        currHealth = maxHealth;
        damageGiven = 1;
        
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        else
        {
            Debug.Log("Missing NavMeshAgent on "+gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DestroyOnDeath();
        target = FindClosestTargetWithTag("GoodGuys");
        //Debug.Log(agent);
        if (target != null && isDead == false)
        {
            if (inCombat == true)
            {
            Follow(target);
            }
        }
        //Debug.Log(target);
    }

    void OnCollisionEnter(Collision other)
    {
        SuicideSingleAttack(other.gameObject); //From BaseClassEnemy
        Debug.Log(other);
    }//OnCollisionEnter
}

