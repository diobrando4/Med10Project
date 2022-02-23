using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    private Rigidbody npc_rb;
    private NavMeshAgent npc_agent;

    public Transform targetPlayer;

    // Start is called before the first frame update
    void Start()
    {
        npc_rb = GetComponent<Rigidbody>();
        npc_agent = GetComponent<NavMeshAgent>();
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
        FollowPlayer();
    }

    void FollowPlayer()
    {
            npc_agent.SetDestination(targetPlayer.position);
    }
}
