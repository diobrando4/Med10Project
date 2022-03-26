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

    public bool inCombat = false; //Bool if combat mode is active

    //Variables related to status effect
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

    // Start is called before the first frame update
    void Start()
    {
        debuffMan = GameObject.Find("DebuffManager").GetComponent<DebuffManager>();
        if (debuffMan == null)
        {
            Debug.Log("DebuffManager is null");
        }
    } //Start

    // Update is called once per frame
    void Update()
    {

    } //Update

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
        if (target2Shoot != null && inCombat == true) //If there is enemies
        {
            float enemyDistance = Vector3.Distance(transform.position, target2Shoot.transform.position); //Calculate distance between ally and enemy
            
            if(enemyDistance < distanceB4Shoot)
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
    }//ShootNearest
}
