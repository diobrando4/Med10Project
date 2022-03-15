using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : BaseClassNPC
{
    public float stopDistanceFromPlayer;
    private GameObject player;
    // Start is called before the first frame update
    //private bool isFiring;
    //[Header("Higher Fire Rate = Slower")]
    //[Tooltip("Higher Fire Rate = Slower")]

    void Start()
    {
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

    }

    // Update is called once per frame
    void Update()
    {
        DestroyOnDeath();
        target = FindClosestTargetWithTag("Enemy");
        Move2Target(target);
        ShootNearestObject(target);
    }

    void Move2Target(GameObject target)
    {
        if (target == null) //If there is no enemies, follow player
        {
            agent.stoppingDistance = stopDistanceFromPlayer;
            agent.SetDestination(player.transform.position);
            transform.LookAt(player.transform);
        } 
        else if (target.tag == "Enemy") //If there is enemies
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
    }
}
