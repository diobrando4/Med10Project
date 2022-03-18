using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Child of BaseClassNPC, Zombie enemy. A simple suicide damage dealer

public class Zombie : BaseClassNPC
{
    // what's the point of this variable?
    //Bool checking to stop the multi-hit
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
            if (inCombat == true)
            {
            Follow(target);
            }
        }
        //Debug.Log(target);
    }

    void OnCollisionEnter(Collision other)
    {
        // we might need a better system for how to deal damage? because of different objects having shared tags, but it works for now
        //Think this might be alright afterall, since Player and Ally needs different triggers for their function anyway
        if(other.gameObject.tag == "GoodGuys")
        {
            // what purpose does hasDamage serve? if the zombies didn't die after hitting their taget, then this would make them unable to do anything, after hitting their target once?
            //Prevents the multi-hit bug. Even if the zombie is set to be destroyed on collision, there is still chance for it to do the multi-hit due to collisions
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
    }//OnCollisionEnter
}
