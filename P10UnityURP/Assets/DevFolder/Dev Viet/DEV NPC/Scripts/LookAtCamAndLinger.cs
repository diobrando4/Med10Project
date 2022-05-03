using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamAndLinger : MonoBehaviour
{

    public float lingerTime = 3f;
    private Camera cameraToLookAt;
    //Color parentColor;

    // Start is called before the first frame update
    void Start()
    {
        cameraToLookAt = Camera.main;
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
        //parentColor = GetComponentsInParent<Renderer>()[1].material.color;
        Destroy(gameObject, lingerTime);

    }
    void FixedUpdate()
    {
        transform.LookAt(cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
    }
}
