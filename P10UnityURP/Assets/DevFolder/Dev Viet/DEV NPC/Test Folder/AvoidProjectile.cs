using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AvoidProjectile : MonoBehaviour
{
    public GameObject projectile;
    protected UnityEngine.AI.NavMeshAgent agent; 
    public float minDist2Dodge = 0;
    public float dodgeSpeed = 0;
    private float currMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
    //NavMeshAgent Check & Init
        if (agent == null)
        {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            currMoveSpeed = agent.speed;
        }
        else
        {
            Debug.Log("Missing NavMeshAgent on "+gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        projectile = FindClosestTargetWithTag("Projectile", minDist2Dodge);
        MoveAwayFromProjectile(projectile);
        transform.LookAt(GameObject.Find("Player").transform.position);
    }

//https://samarthdhroov.medium.com/making-the-enemy-dodge-players-attack-in-unity-8f9204da5f57
//https://answers.unity.com/questions/1258371/ai-dodge-bullets.html    
    protected void MoveAwayFromProjectile(GameObject proj2Avoid)
    {
        if (proj2Avoid != null)
        {
        Vector3 moveDir = proj2Avoid.transform.position - transform.position;
        //Direction to target
        Vector3 dir2Proj = Vector3.Normalize(moveDir);
        moveDir *= -1;
        moveDir.y = transform.position.y;
        //float sidestep = evadeSpeed * Time.deltaTime;
        
        //Dir of target Left/Right, in relation to Forward facing dir of self
        float dotOfDirLR = Vector3.Dot(transform.right, dir2Proj);
        float dotOfDirFB = Vector3.Dot(transform.forward, dir2Proj);
        //Debug.Log(dotOfDirLR);
        //Debug.Log(dotOfDirFB);
            //if (dotOfDirFB <= -0.75) //From Front
            //{
                //moveDir += transform.forward;
                //Debug.Log("WW");
            //}
            if (dotOfDirFB <= 1 && dotOfDirFB >= -0.75) //From Front and Either sides
            {
                moveDir += -transform.forward;
                //Debug.DrawLine(transform.position, moveDir, Color.white);
                if (dotOfDirLR <= 1 && dotOfDirLR >= 0) //If to the Right
                {
                    moveDir += -transform.right;
                    //Debug.DrawLine(transform.position, moveDir, Color.green);
                }
                else if(dotOfDirLR >= -1 && dotOfDirLR < 0) //If to the Left
                {
                    moveDir += transform.right;
                    //Debug.DrawLine(transform.position, moveDir, Color.red);
                }
            }
        Vector3 newPos = transform.position + moveDir.normalized;   
        //Debug.DrawLine(transform.position, newPos,Color.yellow);
        agent.SetDestination(newPos);
        //agent.speed = currMoveSpeed;
        //return newPos;
        }
    }

    //Function to find the closest target with the given tag
    protected GameObject FindClosestTargetWithTag(string tag, float minDist)
    {
        GameObject[] candidates;
        candidates = GameObject.FindGameObjectsWithTag(tag);

        GameObject closestTarget = null;

        float distance = minDist;
        Vector3 pos = transform.position;
        foreach(GameObject posCand in candidates)
        {
            Vector3 diff = posCand.transform.position - pos;
            float curDist = diff.sqrMagnitude;
             if (curDist < distance)
             {
                if(posCand.GetComponent<BulletController>())
                {
                    if(posCand.layer != gameObject.layer)
                    {
                        closestTarget = posCand;
                        distance = curDist; 
                    }
                }                 
            }
        }
        return closestTarget;

    }//FindClosestTarget
}
