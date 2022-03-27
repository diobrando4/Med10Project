using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[SerializeField]
    private Camera mainCam;
    private Plane groundPlane;
    private Ray cameraRay;
    private float rayLength;
    private Vector3 pointToLook;

    public Rigidbody rb;
    public float moveSpeed = 5f;
    
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    public GunController theGun;
    public PlayerHealthManager playerHealthScript;

    // for dash
    private bool isDashing = false;
    private float baseSpeed;
    public float dashSpeed = 10f; // multiplies the base speed of the player, the higher this is the faster the dash is
    public float dashTime = 0.1f; // the higher the number is; the longer the dash lasts
    // maybe dashCharges would be a better name than dashUses?
    public int dashUses = 1; // how many times the player can dash before it needs to be reset
    private int dashUsesMax;
    public float cooldown = 3f;
    [SerializeField]
    private float cooldownTimer;
    
    
    void Awake()
    {
        mainCam = Camera.main;
        playerHealthScript = GetComponent<PlayerHealthManager>();
        
        // for dashing
        baseSpeed = moveSpeed;
        dashUsesMax = dashUses;
        cooldownTimer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        // using raw to make it as responsive as possible
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")); // this is the equivalence of move direction?
        moveVelocity = moveInput * moveSpeed;

        // camera related
        cameraRay = mainCam.ScreenPointToRay(Input.mousePosition);
        groundPlane = new Plane(Vector3.up, Vector3.zero);

        // not sure if we should put this inside player death or not, it depends if we want the cooldown to keep running while the player is dead or not
        if (dashUses < dashUsesMax)
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                dashUses += 1;
                cooldownTimer = cooldown;
            }
        }

        if (playerHealthScript.isPlayerDead == false)
        {            
            // Left click @ mouse button
            if(Input.GetMouseButtonDown(0))
            {
                theGun.isFiring = true;
            }
            if(Input.GetMouseButtonUp(0))
            {
                theGun.isFiring = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            //if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashUses > 0)
                {
                    dashUses -= 1;

                    if (!isDashing)
                    {
                        StartCoroutine(Dash());
                    }
                }
                
            }
        }

        // not sure if the movement should happen in fixed or not, since rigidbody is already using the physics engine?
        //rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        //rb.velocity = moveVelocity;
    }

    // used for physics
    void FixedUpdate()
    {
        // i'm not sure, but i don't think you need to use fixedDeltaTime in FixedUpdate?
        //rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        
        // when the player dies; their movement is disabled
        if (playerHealthScript.isPlayerDead == false)
        {
            rb.velocity = moveVelocity;
            // the player slides slightly when movement is disable, not sure how to prevent this!
        
            if(groundPlane.Raycast(cameraRay, out rayLength))
            {
                pointToLook = cameraRay.GetPoint(rayLength);
                //Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

                // i have read that you should not use look at for rotation, but i can't find another solution!
                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
        }
    }
    
    IEnumerator Dash()
    {
        isDashing = true;
        //Debug.Log("isDashing: " + isDashing);
        playerHealthScript.isPlayerKillable = false;
        //Debug.Log("isPlayerKillable: " + playerHealthScript.isPlayerKillable);
        
        moveSpeed *= dashSpeed;

        yield return new WaitForSeconds(dashTime);

        moveSpeed = baseSpeed;

        isDashing = false;
        //Debug.Log("isDashing: " + isDashing);
        playerHealthScript.isPlayerKillable = true;
        //Debug.Log("isPlayerKillable: " + playerHealthScript.isPlayerKillable);
    }

    /*
    IEnumerator Dash()
    {
        isDashing = true;
        playerHealthScript.isPlayerKillable = false;
        
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            // moveInput * dashSpeed * Time.deltaTime

            isDashing = false;
            playerHealthScript.isPlayerKillable = true;

            yield return null;
        }
    }
    */
}
