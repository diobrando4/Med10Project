using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // used for the UI text dash counter

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
    public PlayerGunController theGun;
    public PlayerHealthManager playerHealthScript;

    // for dash
    private bool isDashing = false;
    private float baseSpeed;
    public float dashSpeed = 10f; // multiplies the base speed of the player, the higher this is the faster the dash is
    public float dashTime = 0.1f; // the higher the number is; the longer the dash lasts
    // maybe dashCharges would be a better name than dashUses?
    public int dashUses = 3; // how many times the player can dash
    private int dashUsesMax;
    public float cooldown = 3f; // is bascially the same as max health
    [SerializeField]
    private float cooldownTimer;
    ParticleSystem dashTrail;
    public TextMeshProUGUI dashUsesText;
    public GameObject dashBar0, dashBar1, dashBar2;

    public bool isPaused; // this var is changed in the menu pause caller
    public bool isOldVersion = false;
    
    void Awake()
    {
        mainCam = Camera.main;
        playerHealthScript = GetComponent<PlayerHealthManager>();
        
        // for dashing
        baseSpeed = moveSpeed;
        dashUsesMax = dashUses;
        cooldownTimer = cooldown;
        dashTrail = transform.Find("DashTrail").GetComponent<ParticleSystem>();
    }

    void Start()
    {
        // if we put the line below inside of the if-statement then it doesn't find it!
        //dashUsesText = GameObject.Find("CanvasHealthBars/HolderHealthBars/HolderPlayerDashCounter/TextDashCounter").GetComponent<TextMeshProUGUI>();
        dashUsesText = GameObject.Find("CanvasHealthBars/HolderPlayerDashCounter/TextDashCounter").GetComponent<TextMeshProUGUI>();
        if (dashUsesText != null) 
        { 
            dashUsesText.text = "DASH: " + dashUses;
        }

        dashBar0 = GameObject.Find("CanvasHealthBars/HolderPlauerDashBar/imgBar0");
        dashBar1 = GameObject.Find("CanvasHealthBars/HolderPlauerDashBar/imgBar1");
        dashBar2 = GameObject.Find("CanvasHealthBars/HolderPlauerDashBar/imgBar2");
        //Change variable to old
        if (isOldVersion == true)
        {
            gameObject.transform.GetChild(8).gameObject.SetActive(false);
        }
        else 
        {
            gameObject.transform.GetChild(8).gameObject.SetActive(true);                      
        }
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

        //dashUsesText.text = "DASH: test" + dashUses;

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
                if (dashUsesText != null)
                {
                    dashUsesText.text = "DASH: " + dashUses;
                }
                cooldownTimer = cooldown;
            }
        }

        if (playerHealthScript.isPlayerDead == false && isPaused == false)
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

            // dash
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
            {
                if (dashUses > 0)
                {
                    dashUses -= 1;
                    if (dashUsesText != null)
                    {
                        dashUsesText.text = "DASH: " + dashUses;
                    }
                    dashTrail.Play();

                    if (!isDashing)
                    {
                        StartCoroutine(Dash());
                    }
                }
                else
                {
                    if(FindObjectOfType<SoundManager>())
                    {  
                        FindObjectOfType<SoundManager>().SoundPlay("DodgeNoCharge");
                    }
                }
                
            }
        }
        // if we didn't have this; then the player could keep shooting, if they held down mouse0 when dying
        if (playerHealthScript.isPlayerDead == true)
        {
            theGun.isFiring = false;
        }

        // not sure if the movement should happen in fixed or not, since rigidbody is already using the physics engine?
        //rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        //rb.velocity = moveVelocity;

        DashBarUpdate();
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
        else
        {
            rb.velocity *= 0;
        }
    }

    void DashBarUpdate()
    {
        switch (dashUses)
        {
            case 3:
                dashBar0.SetActive(true);
                dashBar1.SetActive(true);
                dashBar2.SetActive(true);
                break;
            case 2:
                dashBar0.SetActive(true);
                dashBar1.SetActive(true);
                dashBar2.SetActive(false);
                break;
            case 1:
                dashBar0.SetActive(true);
                dashBar1.SetActive(false);
                dashBar2.SetActive(false);
                break;
            case 0:
                dashBar0.SetActive(false);
                dashBar1.SetActive(false);
                dashBar2.SetActive(false);
                break;
        }
    }
    
    IEnumerator Dash()
    {
        isDashing = true;
        //Debug.Log("isDashing: " + isDashing);
        playerHealthScript.isPlayerKillable = false;
        //Debug.Log("isPlayerKillable: " + playerHealthScript.isPlayerKillable);
        
        moveSpeed *= dashSpeed;
        
        if(FindObjectOfType<SoundManager>())
        {
            FindObjectOfType<SoundManager>().SoundPlay("Dodge"+Random.Range(0,4));
        }

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
