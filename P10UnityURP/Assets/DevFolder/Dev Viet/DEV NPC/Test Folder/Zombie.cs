using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : BaseClassNPC
{
    private bool hasDamaged = false;
    

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
            Follow(target);
        }
        //Debug.Log(target);
    }

    void OnCollisionEnter(Collision other)
    {
        // we might need a better system for how to deal damage? because of different objects having shared tags, but it works for now

        if(other.gameObject.tag == "GoodGuys")
        {
            if(hasDamaged == false)
            {
                hasDamaged = true;
                //Debug.Log("Zombie has hit Player");
                other.gameObject.GetComponent<Ally>().DamageTaken(damageGiven);
                // destroys the zombie
                Destroy(gameObject);
            }
        }
        if(other.gameObject.tag == "Player")
        {
            if(hasDamaged == false)
            {
                hasDamaged = true;
                //Debug.Log("Zombie has hit Player");
                other.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damageGiven); // not sure why but this sometimes gives an error; when being damaged the 2nd time and afterwards

                Destroy(gameObject);
            }
        }
    }
}
