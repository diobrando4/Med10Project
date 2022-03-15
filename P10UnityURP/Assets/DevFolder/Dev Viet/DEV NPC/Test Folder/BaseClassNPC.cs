using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseClassNPC : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected GameObject target;

    [SerializeField]
    public int maxHealth;
    public int currHealth;
    protected bool isDead = false;
    public int damageGiven;

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

    }

    // Update is called once per frame
    void Update()
    {

    }

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
    }

    public void Follow(GameObject target2Follow)
    {
        agent.SetDestination(target2Follow.transform.position);
    }

    protected void DestroyOnDeath()
    {
        if (currHealth <= 0)
        {
            Destroy(gameObject);
            isDead = true;
        }
    }

    public void DamageTaken(int damage)
    {
        currHealth -= damage;
    }

    protected void ShootNearestObject(GameObject target2Shoot)
    {
        if (target2Shoot != null) //If there is enemies
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
    }
}
