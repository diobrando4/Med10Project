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
        //FollowPlayer();
    }

    void FollowPlayer()
    {
        npc_agent.SetDestination(player.transform.position);
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
        transform.LookAt(closestTarget.transform); //Look towards nearest target
        return closestTarget;
    }

    void AvoidEnemy(GameObject enemy)
    {
        float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position); //Calc Distance between self and enemy
        float runAwayDistance = 4; //Distance before backing off
        if(enemyDistance < runAwayDistance)
        {
            Vector3 dir2Enemy = transform.position - enemy.transform.position; //Calc direction to enemy
            Vector3 newPos = transform.position + dir2Enemy; //Add position with enemy direction

            npc_agent.SetDestination(newPos);
        }
    }

}