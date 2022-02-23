using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // is needed for NavMesh

public class ZombieController : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agent;

    public Transform player; // need to auto apply this!

    void Awake()
    {
        // these could also be done this in the inspector
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        // checks if the Rigidbody has been added to the zombie
        if (rb == null)
        {
            Debug.LogError("The Rigidbody isn't attached to " + gameObject.name);
        }
        // checks if the NavMesh has been added to the zombie
        if (agent == null)
        {
            Debug.LogError("The NavMesh isn't attached to " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // not sure if we should use update or fixed update for this?
        Follow();
        // at some point we need a switch case here for state machine behaviours
    }

    void Follow()
    {
        agent.SetDestination(player.position);
    }

    public int damageGiven = 1;

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Debug.Log("Zombie has hit Player");
            other.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damageGiven);
            //other.gameObject.GetComponent<ZombieHealthManager>().HurtZombie(damageGiven);

            // destroys the bullet when hitting the enemy
            Destroy(gameObject);
        }
    }
}
