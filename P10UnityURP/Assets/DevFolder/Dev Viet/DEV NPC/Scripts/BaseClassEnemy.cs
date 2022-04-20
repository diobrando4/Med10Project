using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Child of BaseClassNPC, BaseClass for Enemies

public class BaseClassEnemy : BaseClassNPC
{
    //Bool checking to stop multi-hit on melee units
    protected bool hasDamaged = false;
    public Effect deathEffect = null;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    //Function that destroys GameObject on 0 or less HP. Can be changed to a disable
    protected void DestroyOnDeath()
    {
        if (currHealth <= 0)
        {
            if(FindObjectOfType<ExitDoor>())
            {
                FindObjectOfType<ExitDoor>().RemoveFromList(gameObject);
            }
            isDead = true;
            PlaySound("EnemyKilled");
            DeathEffect();
            Destroy(gameObject);
        }
    }//DestroyOnDeath

    //When damaging target, destroy self
    protected void SuicideSingleAttack(GameObject victim)
    {
        //Prevents the multi-hit bug. Even if the zombie is set to be destroyed on collision, there is still chance for it to do the multi-hit due to collisions
        if(hasDamaged == false)
        {
            if(victim.tag == "GoodGuys")
            {
                victim.GetComponent<Ally>().DamageTaken(damageGiven);
                hasDamaged = true;
                if(FindObjectOfType<ExitDoor>())
                {
                    FindObjectOfType<ExitDoor>().RemoveFromList(gameObject);
                }
                PlaySound("EnemyKilled");
                DeathEffect();
                Destroy(gameObject);
            }
            else if(victim.tag == "Player")
            {
                victim.GetComponent<PlayerHealthManager>().HurtPlayer(damageGiven);
                hasDamaged = true;
                if(FindObjectOfType<ExitDoor>())
                {
                    FindObjectOfType<ExitDoor>().RemoveFromList(gameObject);
                }                
                PlaySound("EnemyKilled");
                DeathEffect();
                Destroy(gameObject);
            }
        }
    }//SuicideSingleAttack

    //Overload version that also deals status effect
    protected void SuicideSingleAttack(GameObject victim, int debuffNum)
    {
        //Prevents the multi-hit bug. Even if the zombie is set to be destroyed on collision, there is still chance for it to do the multi-hit due to collisions
        if(hasDamaged == false)
        {
            if(victim.tag == "GoodGuys")
            {
                victim.GetComponent<Ally>().DamageTaken(damageGiven);
                
                hasDamaged = true;
                if(FindObjectOfType<ExitDoor>())
                {
                    FindObjectOfType<ExitDoor>().RemoveFromList(gameObject);
                }                
                PlaySound("EnemyKilled");
                DeathEffect();
                Destroy(gameObject);
            }
            else if(victim.tag == "Player")
            {
                victim.GetComponent<PlayerHealthManager>().HurtPlayer(damageGiven);
                //victim.GetComponent<PlayerHealthManager>().isDebuffed = true;
                debuffMan.DebuffSelector(debuffNum);
                hasDamaged = true;
                if(FindObjectOfType<ExitDoor>())
                {
                    FindObjectOfType<ExitDoor>().RemoveFromList(gameObject);
                }                
                PlaySound("EnemyKilled");
                DeathEffect();
                Destroy(gameObject);
            }
        }
    }//SuicideSingleAttack

    protected void DebuffAura(GameObject victim, int debuffNum)
    {
        if(victim.tag == "Player")
        {
            debuffMan.DebuffSelector(debuffNum);
        }
    }

    protected void DeathEffect()
    {
        Effect newEffect = Instantiate(deathEffect, transform.position, transform.rotation) as Effect;
        newEffect.origin = gameObject;
    }
}
