using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // the target in this case is the player transform
    public Vector3 offset; // the offset of the camera

    [Range(0.0f, 1.0f)]
    public float smoothSpeed = 0.125f;

    void Awake()
    {
        // finds the player automatically
        target = GameObject.FindWithTag("Player").transform;

        // in case we want to change the rotation of the camera in the script
        transform.rotation = Quaternion.Euler(55,0,0);

        // in case we want to bypass the inspector and set the values ourselves
        offset = new Vector3(0,16,-12);
    }

    void Update() // maybe use update rather than lateupdate?
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    // the old version of this script
    /*
    public Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 0.5f; // has to be between 0-1

    public bool lookAtPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - playerTransform.position;
    }

    // LateUpdate is called after Update methods, if it lags then use FixedUpdate instead
    void LateUpdate()
    {
        Vector3 newPos = playerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, smoothSpeed);

        // i don't like this effect
        if(lookAtPlayer == true)
        {
            transform.LookAt(playerTransform);
        }
    }
    */
}
