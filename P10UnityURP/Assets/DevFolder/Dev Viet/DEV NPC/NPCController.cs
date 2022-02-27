using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    private Rigidbody npc_rb;
    private NavMeshAgent npc_agent;

    private GameObject player;
    private GameObject targetEnemy;

    private bool isFiring;
    public float fireRate;
    public float projectileSpeed;
    private float shotCounter;

    public BulletController bullet;
    public Transform muzzle;

    // Start is called before the first frame update
    void Start()
    {
        npc_rb = GetComponent<Rigidbody>();
        npc_agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        //TargetPlayer;

        if (npc_rb == null)
        {
            Debug.LogError("No RigidBody attached to " + gameObject.name);
        }
        if (npc_agent == null)
        {
            Debug.LogError("No NavMesh attached to " + gameObject.name);
        }

    }
    // Update is called once per frame
    void Update()
    {
        targetEnemy = FindClosestTarget();
        AvoidEnemy(targetEnemy);
        ShootNearestEnemy(targetEnemy);
        FollowPlayer(targetEnemy);
    }

    void FollowPlayer(GameObject enemy)
    {
        if (enemy == null)
        {
            npc_agent.SetDestination(player.transform.position);            
        }
    }

    public GameObject FindClosestTarget()
    {
        GameObject[] candidates; //Array of potential candidates for closest target
        candidates = GameObject.FindGameObjectsWithTag("Enemy"); //Find and add all gameobjects with the tag

        GameObject closestTarget = null; 

        float distance = Mathf.Infinity; //Infinity!
        foreach(GameObject posCand in candidates)
        {
            Vector3 diff = posCand.transform.position - transform.position; //Find the distance in vector between possible candidate and self
            float curDist = diff.sqrMagnitude; //Calc distance
            if (curDist < distance)
            {
                closestTarget = posCand;
                distance = curDist;
            }
        }
        if (closestTarget != null)
        {
            transform.LookAt(closestTarget.transform); //Look towards nearest target
        }
        return closestTarget;
    }

    void AvoidEnemy(GameObject enemy)
    {
        if (enemy != null)
        {
            float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position); //Calc Distance between self and enemy
            float runAwayDistance = 5; //Distance before backing off
            if(enemyDistance < runAwayDistance)
            {
                Vector3 dir2Enemy = transform.position - enemy.transform.position; //Calc direction to enemy
                Vector3 newPos = transform.position + dir2Enemy; //Add position with enemy direction

                npc_agent.SetDestination(newPos);
            }
        }
    }

    void ShootNearestEnemy(GameObject enemy)
    {
        if (enemy != null)
        {
            float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
            float distanceB4Shoot = 10; //Distance before shooting at enemy
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