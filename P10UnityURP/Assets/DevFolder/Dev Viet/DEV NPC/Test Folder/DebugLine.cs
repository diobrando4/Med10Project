using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLine : MonoBehaviour
{
    public Transform start;
    public Vector3 offset;

    public RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(start.position+offset, start.TransformDirection(Vector3.forward),out hit, Mathf.Infinity); //Raycast
        Debug.DrawRay(start.position+offset, start.TransformDirection(Vector3.forward)*100,Color.yellow);//Debug raycast line
        if (hit.transform != null) //As long as there is something intersecting the raycast
        {
            if(hit.transform.tag == "Player") //If it hits a gameobject with the tag "Player"
            {
                if(hit.transform.Find("Tag").tag == "GoodGuys")//Find the child called "Tag" and get its tag, then if it is "GoodGuys"
                {
                    Debug.Log(hit.transform.Find("Tag").tag); //Do something
                }
            }
            else
            {
                Debug.Log(hit.transform.tag);
            }
        }
    }
}
