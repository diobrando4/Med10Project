using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public Camera mainCamera;
    private Plane groundPlane;
    private Ray cameraRay;
    private float rayLength;
    private Vector3 pointToLook;

    public float moveSpeed = 5f;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    
    // Update is called once per frame
    void Update()
    {
        // using raw to make it as responsive as possible
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput * moveSpeed;

        cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        groundPlane = new Plane(Vector3.up, Vector3.zero);

        if(groundPlane.Raycast(cameraRay, out rayLength))
        {
            pointToLook = cameraRay.GetPoint(rayLength);
            //Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            // i have read that you should not use look at for rotation, but i cannot find another solution
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    // used for physics
    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        rb.velocity = moveVelocity;
    }
}
