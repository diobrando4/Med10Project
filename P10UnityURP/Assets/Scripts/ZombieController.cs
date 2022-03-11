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
    private bool hasDamaged = false;

    public ExitDoor exitDoor;

    void OnCollisionEnter(Collision other)
    {
        // we might need a better system for how to deal damage? because of different objects having shared tags, but it works for now

        if(other.gameObject.tag == "GoodGuys")
        {
            if(hasDamaged == false)
            {
                hasDamaged = true;
                //Debug.Log("Zombie has hit Player");
                other.gameObject.GetComponent<AllyHealthManager>().HurtAlly(damageGiven); // not sure why but this sometimes gives an error; when being damaged the 2nd time and afterwards
                //other.gameObject.GetComponent<ZombieHealthManager>().HurtZombie(damageGiven);

                // if we don't do this and there isn't an exit door; then zombies can't be destroyed and we'll get a lot of errors
                if (exitDoor != null)
                {
                    exitDoor.remainingEnemies.Remove(gameObject);
                }                
                // destroys the zombie
                Destroy(gameObject);
                
                /*
                AttackResetDelay(4f);
                print("RESET");
                */
            }
        }
        if(other.gameObject.tag == "Player")
        {
            if(hasDamaged == false)
            {
                hasDamaged = true;
                //Debug.Log("Zombie has hit Player");
                other.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damageGiven); // not sure why but this sometimes gives an error; when being damaged the 2nd time and afterwards
                //other.gameObject.GetComponent<ZombieHealthManager>().HurtZombie(damageGiven);

                // if we don't do this and there isn't an exit door; then zombies can't be destroyed and we'll get a lot of errors
                if (exitDoor != null)
                {
                    exitDoor.remainingEnemies.Remove(gameObject);
                }   
                // destroys the zombie
                Destroy(gameObject);
            }
        }
    }

    /*     
    IEnumerator AttackResetDelay(float time)
    {
        hasDamaged = true;
        yield return new WaitForSeconds(time);
        hasDamaged = false;

    }
    */
}
