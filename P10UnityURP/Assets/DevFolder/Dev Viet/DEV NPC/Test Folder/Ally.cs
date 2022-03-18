using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//Child of BaseClassNPC, Ally that will help the player by shooting and later on do other tasks

public class Ally : BaseClassNPC
{
    public float stopDistanceFromPlayer;
    private GameObject player;

    public Image healthBarFill;

    public bool isAllyDead = false;

    void Start()
    {
        //Initial Values can be defined for the inherited variables
        maxHealth = 6;
        currHealth = maxHealth;
        distanceB4Shoot = 10; 
        projectileSpeed = 10f;
        fireRate = 0.75f;

        player = GameObject.FindGameObjectWithTag("Player");

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        else
        {
            Debug.Log("Missing NavMeshAgent on "+gameObject);
        }

        muzzle = gameObject.transform.GetChild(0).Find("Muzzle");
        UpdateHealthBar();
    }//Start

    // Update is called once per frame
    void Update()
    {
        //DestroyOnDeath(); //Inherited function

        target = FindClosestTargetWithTag("Enemy");//Inherited function
        if (isAllyDead == false) 
        {
            Move2Target(target);
            ShootNearestObject(target);//Inherited function
        }

        UpdateHealthBar();

        // ally death state
        if (currHealth <= 0)
        {
            isAllyDead = true;
        }
    }//Update

    //Special made function that allows Ally to move towards a target without getting too close, 
    //but also follow the player if no valid enemy target is found
    void Move2Target(GameObject target)
    {
        if (inCombat == false || target == null) //If there is no enemies, follow player
        {
            agent.stoppingDistance = stopDistanceFromPlayer;
            agent.SetDestination(player.transform.position);
            transform.LookAt(player.transform);
        } 
        else if (inCombat == true && target.tag == "Enemy") //If there is enemies
        {
            float enemyDistance = Vector3.Distance(transform.position, target.transform.position); //Calc Distance between self and enemy
            float runAwayDistance = 5; //Distance before backing off
            if(enemyDistance <= runAwayDistance) //If enemies are too close, backoff
            {
                Vector3 dir2Enemy = transform.position - target.transform.position; //Calc direction to enemy
                Vector3 newPos = transform.position + dir2Enemy; //Add position with enemy direction

                agent.stoppingDistance = 3f;
                agent.SetDestination(newPos);
                //print(gameObject + " Avoiding");
            }
            else if (enemyDistance > runAwayDistance) //If enemies are not too close, move closer
            {
                agent.stoppingDistance = 6f;
                agent.SetDestination(target.transform.position);
                //print(gameObject + " Engaging");
            }
        transform.LookAt(target.transform);
        }
    }//Move2Target

    //Update the healthbar on the UI, if fill image has been assigned
    public void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currHealth / (float)maxHealth; //Since we are dealing with percentages, int variables are casted into floats for this calculation.
        }
    }//UpdateHealthBar
}
