using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    private Rigidbody npc_rb;
    private NavMeshAgent npc_agent;

    private GameObject targetPlayer;
    private GameObject targetEnemy;

    // Start is called before the first frame update
    void Start()
    {
        npc_rb = GetComponent<Rigidbody>();
        npc_agent = GetComponent<NavMeshAgent>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
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
        FollowPlayer();
    }

    void FollowPlayer()
    {
        npc_agent.SetDestination(targetPlayer.transform.position);
    }

    public GameObject FindClosestTarget()
    {
        GameObject[] candidates;
        candidates = GameObject.FindGameObjectsWithTag("Enemy");

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
}