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
        Physics.Raycast(start.position+offset, start.TransformDirection(Vector3.forward),out hit, Mathf.Infinity);
        Debug.DrawRay(start.position+offset, start.TransformDirection(Vector3.forward)*100,Color.yellow);
        if (hit.transform != null)
        {
            Debug.Log(hit.transform.name);
        }
        
    }
}
