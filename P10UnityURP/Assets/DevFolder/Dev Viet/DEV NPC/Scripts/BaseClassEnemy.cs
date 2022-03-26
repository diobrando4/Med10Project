using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Child of BaseClassNPC, BaseClass for Enemies

public class BaseClassEnemy : BaseClassNPC
{
    //Bool checking to stop multi-hit on melee units
    protected bool hasDamaged = false;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

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
                Destroy(gameObject);
            }
            else if(victim.tag == "Player")
            {
                victim.GetComponent<PlayerHealthManager>().HurtPlayer(damageGiven);
                hasDamaged = true;
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
                Destroy(gameObject);
            }
            else if(victim.tag == "Player")
            {
                victim.GetComponent<PlayerHealthManager>().HurtPlayer(damageGiven);
                victim.GetComponent<PlayerHealthManager>().isDebuffed = true;
                debuffMan.DebuffSelector(debuffNum);
                hasDamaged = true;
                Destroy(gameObject);
            }
        }
    }//SuicideSingleAttack

    //Debuff that reduces the Player's Max HP to a new value, until they are no longer debuffed,
    //In which the Player's Max HP and health is restored to what it was previously.
    // private void DebuffReducePlayerHealth(float newHealth, GameObject victim)
    // {
    //     if (victim.tag == "Player")
    //     {   
    //         if(victim.GetComponent<PlayerHealthManager>().isDebuffed == true)
    //         {
    //             defaultPlayerMaxHP = victim.GetComponent<PlayerHealthManager>().playerMaxHealth;
    //             victim.GetComponent<PlayerHealthManager>().playerMaxHealth = newHealth;
    //             victim.GetComponent<PlayerHealthManager>().playerCurrentHealth = victim.GetComponent<PlayerHealthManager>().playerMaxHealth;
    //         }
    //     }
    //     else
    //     {
    //         victim.GetComponent<PlayerHealthManager>().playerMaxHealth = defaultPlayerMaxHP;
    //         victim.GetComponent<PlayerHealthManager>().playerCurrentHealth = victim.GetComponent<PlayerHealthManager>().playerMaxHealth;
    //     }
    // }
}
