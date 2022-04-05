using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLineOfSight : MonoBehaviour
{
    private RaycastHit rayHit;
    float attackDistance = 10f;
    Vector3 rayOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 pos = transform.forward + new Vector3(0,1,0);
        //Vector3 rayOffset = new Vector3(0,100,0);
        rayOffset = new Vector3(0,-0.2f,0);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(transform.position + rayOffset, rayHit.point, Color.red);
        //Debug.DrawRay(transform.position + rayOffset, transform.forward * attackDistance, Color.red);
        TestingLineOfSight();
    }

    void TestingLineOfSight()
    {
        if (Physics.Raycast(transform.position + rayOffset, transform.forward, out rayHit, attackDistance))
        {
            Debug.DrawLine(transform.position + rayOffset, rayHit.point, Color.red);

            //Debug.Log("looking at tag: " + rayHit.collider.gameObject.tag);
            //Debug.Log("looking at tag: " + rayHit.collider.gameObject.name);

            if (rayHit.transform.gameObject.CompareTag("Enemy"))
            {
                // the attack happens here!
            }
        }
        else
        {
            Debug.DrawRay(transform.position + rayOffset, transform.forward * attackDistance, Color.green);
        }
    }
}
