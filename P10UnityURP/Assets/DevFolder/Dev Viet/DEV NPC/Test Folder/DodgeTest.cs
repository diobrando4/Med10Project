using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeTest : BaseClassEnemy
{
    public float projectileDetectionRange = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
        else
        {
            Debug.Log("Missing NavMeshAgent on " + gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //target = FindClosestTargetWithTag("GoodGuys");
        target = GameObject.Find("Cubess");
        trackedProjectile = FindClosestTargetWithTag("Projectile", projectileDetectionRange);
        if (target != null)
        {
            //if (trackedProjectile == null)
            //{
                //agent.ResetPath();
                //Move2Target(target,PosisitionAwayFromProjectile(trackedProjectile),5,3,6);
                //Debug.DrawLine(transform.position, newPos);
                Move2Target(target,5,3,6);
                //Debug.Log("Moving to Player");
            //}
            //else if (trackedProjectile != null)
            //{
                //agent.ResetPath(); 
                //PosisitionAwayFromProjectile(trackedProjectile);
                //Debug.Log("Projectile");
            //}
            //transform.LookAt(target.transform.position);
        }
    }
/*     protected void Move2Target(GameObject _target,Vector3 _projectileAvoidPos, float _backoff, float _stopDistBackoff, float _stopDistApproach)
    {
        float _targetDist = Vector3.Distance(transform.position, _target.transform.position);
        float _backOffDistance = _backoff;
        if(_targetDist <= _backOffDistance) //Back Off
        {
            Vector3 dir2Target = transform.position - _target.transform.position;
            Vector3 newPos = transform.position + dir2Target;

            agent.stoppingDistance = _stopDistBackoff;
            if (trackedProjectile == null)
            {
                //agent.isStopped = false;
                //agent.ResetPath();
                agent.SetDestination(newPos);
            }
            else if (trackedProjectile != null)
            {
                //agent.ResetPath();
                agent.SetDestination(_projectileAvoidPos);
                Debug.Log("Bullet!");
            }
        }
        else if(_targetDist > _backOffDistance) //Move Closer
        {
            agent.stoppingDistance = _stopDistApproach;
            if (trackedProjectile == null)
            {
                //agent.isStopped = false;
                //agent.ResetPath();
                agent.SetDestination(_target.transform.position);
            }
            else if (trackedProjectile != null)
            {
                //agent.ResetPath();
                agent.SetDestination(_projectileAvoidPos);
                Debug.Log("Bullet!");
            }

            if(HasLineOfSightTo(_target, _backOffDistance)) //If the width of their body
            {
                if(sphereHit.transform.gameObject.tag != _target.tag)
                {
                    agent.stoppingDistance = _stopDistApproach-1f;  
                }   
            }  
        }
        transform.LookAt(_target.transform);
    }//Move2Target */
}
