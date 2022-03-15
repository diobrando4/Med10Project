using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseClassNPC : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject target;

    public int maxHealth;
    public int currHealth;

    public int damageGiven;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        else
        {
            Debug.Log("Missing NavMeshAgent on "+gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DestroyOnDeath();
        target = FindClosestTarget("GoodGuys");
        Follow(target);
    }

    public GameObject FindClosestTarget(string tag)
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

    void Follow(GameObject target2Follow)
    {
        agent.SetDestination(target2Follow.transform.position);
    }

    void DestroyOnDeath()
    {
        if (currHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
