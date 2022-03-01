using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera mainCam;
    private Plane groundPlane;
    private Ray cameraRay;
    private float rayLength;
    private Vector3 pointToLook;

    public Rigidbody rb;
    public float moveSpeed = 5f;
    private Vector3 moveInput;
    private Vector3 moveVelocity;

    public GunController theGun;
    
    void Awake()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // using raw to make it as responsive as possible
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput * moveSpeed;

        // camera related
        cameraRay = mainCam.ScreenPointToRay(Input.mousePosition);
        groundPlane = new Plane(Vector3.up, Vector3.zero);

        if(groundPlane.Raycast(cameraRay, out rayLength))
        {
            pointToLook = cameraRay.GetPoint(rayLength);
            //Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            // i have read that you should not use look at for rotation, but i cannot find another solution
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        // Left click @ mouse button
        if(Input.GetMouseButtonDown(0))
        {
            theGun.isFiring = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            theGun.isFiring = false;
        }
    }

    // used for physics
    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        rb.velocity = moveVelocity;
    }
}
