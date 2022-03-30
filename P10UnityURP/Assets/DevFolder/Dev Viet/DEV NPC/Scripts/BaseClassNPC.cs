using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//BaseClass for all variables and functions that a majority of multiple Actors are going to have or use in some shape or form
//Childs can then inherit the variables and functions, and call/overload them as needed

public class BaseClassNPC : MonoBehaviour
{
    protected NavMeshAgent agent; //NavMeshAgent of GameObject
    protected GameObject target; //GameObject that it needs to target

    public bool inCombat = true; //Bool if combat mode is active

    //Variables related to status effect
    [SerializeField]
    protected DebuffManager debuffMan;

    [Header("Health related variables")]
    public int maxHealth; //Max health of GameObject
    public int currHealth; //Current health of GameObject
    protected bool isDead = false; //Death check

    [Header("Only Relevant for non-shooting enemies")]
    public int damageGiven; //Damage given. Relevant to Enemies

    [Header("Only Relevant if it shoots")]
    public float fireRate;
    public float projectileSpeed;
    public float distanceB4Shoot; //Distance before shooting at target2Shoot
    private float shotCounter;
    public BulletController bullet;
    [SerializeField]
    protected Transform muzzle;

    // for raycast
    private RaycastHit rayHit;
    private float attackDistance = 10f; // replace this with distanceB4Shoot
    Vector3 rayOffset; // we need this, otherwise the raycast might hit unwanted objects

    void Awake()
    {
        rayOffset = new Vector3(0,-0.2f,0);

        //so we don't get a bunch of errors when it's missing
        if (debuffMan != null)
        {
            debuffMan = GameObject.Find("DebuffManager").GetComponent<DebuffManager>();
        }
        //runs debug if it's missing
        if (debuffMan == null)
        {
            Debug.Log("DebuffManager is null");
        }
    }

    //Function to find the closest target with the given tag
    protected GameObject FindClosestTargetWithTag(string tag)
    {
        GameObject[] candidates;
        candidates = GameObject.FindGameObjectsWithTag(tag);

        GameObject closestTarget = null;

        float distance = Mathf.Infinity;
        Vector3 pos = transform.position;
        foreach(GameObject posCand in candidates)
        {
            Vector3 diff = posCand.transform.position - pos;
            float curDist = diff.sqrMagnitude;
            if (curDist < distance)
            {
                closestTarget = posCand;
                distance = curDist;
            }
        }
        return closestTarget;

    }//FindClosestTarget

    //Simple follow to a GameObject
    public void Follow(GameObject target2Follow)
    {   
        agent.SetDestination(target2Follow.transform.position);
    }//Follow

    //Function that destroys GameObject on 0 or less HP. Can be changed to a disable
    protected void DestroyOnDeath()
    {
        if (currHealth <= 0)
        {
            Destroy(gameObject);
            isDead = true;
        }
    }//DestroyOnDeath

    //Call function to reduce health
    public void DamageTaken(int damage)
    {
        currHealth -= damage;
    }//DamageTaken

    //Function that dictates shooting the nearest GameObject
    protected void ShootNearestObject(GameObject target2Shoot)
    {
        // replace "attackDistance" (from test script) with "distanceB4Shoot"
        
        /*
        if (Physics.Raycast(transform.position + rayOffset, transform.forward, out rayHit, distanceB4Shoot))
        {
            Debug.DrawLine(transform.position + rayOffset, rayHit.point, Color.red);

            
            if (rayHit.transform.gameObject.CompareTag("Enemy"))
            {
                // the attack happens here!
            }
            
        }
        else
        {
            Debug.DrawRay(transform.position + rayOffset, transform.forward * distanceB4Shoot, Color.green);
        }
        */

        if (target2Shoot != null ) //If there are enemies
        {
            float targetDistance = Vector3.Distance(transform.position, target2Shoot.transform.position); //Calculate distance between ally and enemy
            
            if (Physics.Raycast(transform.position + rayOffset, transform.forward, out rayHit, distanceB4Shoot) && targetDistance < distanceB4Shoot)
            //if(targetDistance < distanceB4Shoot)
            {
                Debug.DrawLine(transform.position + rayOffset, rayHit.point, Color.red);

                shotCounter -= Time.deltaTime;
                if(shotCounter <= 0)
                {
                    shotCounter = fireRate;
                    BulletController newNPCBullet = Instantiate(bullet, muzzle.position, muzzle.rotation) as BulletController;
                    newNPCBullet.speed = projectileSpeed;                
                }
            }
            else
            {
                Debug.DrawRay(transform.position + rayOffset, transform.forward * distanceB4Shoot, Color.green);
                shotCounter = 0;
            }
        }

        /*
        if (target2Shoot != null && inCombat == true) //If there are enemies
        {
            float targetDistance = Vector3.Distance(transform.position, target2Shoot.transform.position); //Calculate distance between ally and enemy
            
            if(targetDistance < distanceB4Shoot)
            {
                shotCounter -= Time.deltaTime;
                if(shotCounter <= 0)
                {
                    shotCounter = fireRate;
                    BulletController newNPCBullet = Instantiate(bullet, muzzle.position, muzzle.rotation) as BulletController;
                    newNPCBullet.speed = projectileSpeed;                
                }
            }
            else
            {
                shotCounter = 0;
            }
        }
        */
        
    }//ShootNearest
}
