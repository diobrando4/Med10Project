using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWorld : MonoBehaviour
{
    // this script needs to be on all of the objects that needs to follow the camera

    public Transform lookAt; // the object it should follow
    public Vector3 offset;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = mainCam.WorldToScreenPoint(lookAt.position + offset);

        if (transform.position != pos)
        {
            transform.position = pos;
        }
    }
}
