using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDebuffSuicider : BaseClassEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3;
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
        SuicideSingleAttack(other.gameObject,2,false); //From BaseClassEnemy, Current Debuff is Reverse, Meatshield set to false
        //Debug.Log(other);
    }//OnCollisionEnter
}
