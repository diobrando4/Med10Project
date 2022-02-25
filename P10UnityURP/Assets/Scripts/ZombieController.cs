using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // is needed for NavMesh

public class ZombieController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private GameObject target;
    //public Transform target; // need to auto apply this!

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
        target = FindClosestTarget();

        if (target != null)
        {
            Follow();
        }
        
        // at some point we need a switch case here for state machine behaviours
    }

    void Follow()
    {
        //Transform eatthisone;
        //if (target.tag == "Player" || target.tag == "NPC")
        //{
        //    Vector3.Distance(rb.position, target.GetComponent<Rigidbody>().position);
        //}
        //agent.SetDestination();
        agent.SetDestination(target.transform.position);
    }

/*     private bool isTriggered = false;
    public void OnTriggerEnter(Collider col){
        if (col.GetComponent<Collider>().gameObject.tag == "Player" || col.GetComponent<Collider>().gameObject.tag == "NPC"){
            if (isTriggered = false)
            {
            isTriggered = true;
            }
        }
    } */

    public GameObject FindClosestTarget()
    {
        GameObject[] candidates;
        candidates = GameObject.FindGameObjectsWithTag("GoodGuys");

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

    public int damageGiven = 1;

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "GoodGuys")
        {
            //Debug.Log("Zombie has hit Player");
            other.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damageGiven);
            //other.gameObject.GetComponent<ZombieHealthManager>().HurtZombie(damageGiven);

            // destroys the bullet when hitting the enemy
            Destroy(gameObject);
        }
    }
}
