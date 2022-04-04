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
    
    [Header("Internal variables")]
    public bool inCombat = true; //Bool if combat mode is active
    public int damageGiven = 1; //default damage given.
    protected int ignoreOwnLayer; //Should ignore the layer its in, in order to ignore targets of the same "faction"

    //Variables related to status effect
    protected DebuffManager debuffMan;

    [Header("Health related variables")]
    public int maxHealth; //Max health of GameObject
    public int currHealth; //Current health of GameObject
    protected bool isDead = false; //Death check

    
    [Header("Only Relevant if it shoots")]
    public float fireRate;
    public float projectileSpeed;
    public float distanceB4Shoot; //Distance before shooting at target2Shoot
    private float shotCounter;
    //public GoodGuysBullet goodGuysBullet; // for player and ally (GoodGuys)
    //public EnemyBullet enemyBullet; // for enemy
    public BulletController bullet = null; //Can either be GoodGuysBullet or EnemyBullet
    [SerializeField]
    protected Transform muzzle;

    // for raycast
    private RaycastHit rayHit;
    private RaycastHit sphereHit;

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
        //Bit shift index of own layer to create a bit mask
        ignoreOwnLayer = 1<<gameObject.layer;
        //Invert the bit mask e.g. focus only on own layer to, focus on all other layers than own
        ignoreOwnLayer = ~ignoreOwnLayer;
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
        if (target2Shoot != null ) //If there are enemies
        {
            //if (Physics.Raycast(transform.position + rayOffset, transform.forward, out rayHit, distanceB4Shoot, ignoreOwnLayer))
            if (Physics.SphereCast(transform.position + rayOffset, 0.2f, transform.forward, out rayHit, distanceB4Shoot, ignoreOwnLayer))
            //If the raycasted sphere hits something within the distance
            {
                Debug.DrawLine(transform.position + rayOffset, rayHit.point, Color.red);

                shotCounter -= Time.deltaTime;
                if(rayHit.transform != null) //If the ray is hitting something and it does not share the same tag as self
                {
                    if(rayHit.transform.gameObject.layer != gameObject.layer && 
                       rayHit.transform.gameObject.layer == LayerMask.NameToLayer("GoodGuys") || 
                       rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) //If the object hit by the ray doesnt share the same layer as self, but is also on the layer GoodGuys/Enemy
                    {
                        if(shotCounter <= 0)
                        {
                            shotCounter = fireRate;
                            BulletController newBullet = Instantiate(bullet, muzzle.position, muzzle.rotation) as BulletController;
                            newBullet.speed = projectileSpeed;
                            newBullet.damageGiven = damageGiven;
                        }
                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position + rayOffset, transform.forward * distanceB4Shoot, Color.green);
                shotCounter = 0;
            }
        }       
    }//ShootNearest
    protected bool HasLineOfSightTo(GameObject _target)
    {
        if(Physics.SphereCast(transform.position + rayOffset, 0.2f, transform.forward, out rayHit, distanceB4Shoot, ignoreOwnLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
