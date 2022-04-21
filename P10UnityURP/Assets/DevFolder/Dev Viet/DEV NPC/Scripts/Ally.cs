using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//Child of BaseClassNPC, Ally that will help the player by shooting and later on do other tasks

public class Ally : BaseClassNPC
{
    [SerializeField]
    private GameObject player;
    public float stopDistanceFromPlayer;

    private Ally allyFriend;

    public Image healthBarFill; //Need to automate this later
    public bool isAllyDead = false;

    //Revive Related
    [HideInInspector] 
    public float reviveMax = 100;
    [HideInInspector] 
    public float reviveCurrent = 0; // has to be reset each time reviving is aborted
    private float reviveRate = 100;
    private Image reviveBarFill;
    [HideInInspector] 
    public bool isRevived = false;
    //private bool isBeingRevived;

    //Dispel related
    private bool canCureDebuff = true;
    //[HideInInspector]
    public bool isUsingDispel = false;

    void Start()
    {
        //Initial Values can be defined for the inherited variables
        maxHealth = 6;
        currHealth = maxHealth;
        distanceB4Shoot = 10; 
        projectileSpeed = 15f;
        fireRate = 0.75f;
        muzzle = gameObject.transform.Find("AllyGun/Muzzle");

        player = GameObject.FindGameObjectWithTag("Player");
        reviveBarFill = gameObject.transform.Find("ReviveBarPopUp/Canvas/ReviveBar/imgBackground/imgFill").GetComponent<Image>();
        gameObject.GetComponentInChildren<Image>().enabled = false; //Disable Image comp of Imgbackground on start
        gameObject.transform.Find("StatusIcon").GetComponent<Renderer>().enabled = false;

        //NAvMeshAgent Check & Init
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        else
        {
            Debug.Log("Missing NavMeshAgent on "+gameObject);
        }

        //allyFriend Check & Init
        if (gameObject == GameObject.Find("AllyBlueBot"))
        {
            healthBarFill = GameObject.Find("CanvasHealthBars/HolderHealthBars/HolderBlueHealthBar/imgBackground/imgFillBlue").GetComponent<Image>();;
            allyFriend = GameObject.Find("AllyOrangeBot").GetComponent<Ally>();
        }
        else if (gameObject == GameObject.Find("AllyOrangeBot"))
        {
            healthBarFill = GameObject.Find("CanvasHealthBars/HolderHealthBars/HolderOrangeHealthBar/imgBackground/imgFillOrange").GetComponent<Image>();;
            allyFriend = GameObject.Find("AllyBlueBot").GetComponent<Ally>();
        }
        //Find where the bullet spawns from
        muzzle = gameObject.transform.GetChild(0).Find("Muzzle");
        UpdateHealthBar();
    }//Start

    // Update is called once per frame
    void Update()
    {
        //DestroyOnDeath(); //Inherited function
        //Debug.Log(isRevived);
        target = FindClosestTargetWithTag("Enemy");//Inherited function
        if (isAllyDead == false) 
        {
            gameObject.transform.Find("StatusIcon").GetComponent<Renderer>().enabled = false;
            Move2Target(target);
            ShootNearestObject(target);//Inherited function
        }
        else
        {
            agent.SetDestination(gameObject.transform.position);
            gameObject.transform.Find("StatusIcon").GetComponent<Renderer>().enabled = true;
        }

        if (target == null)
        {
            inCombat = false;
        }
        else
        {
            inCombat = true;
        }

        UpdateHealthBar();

        // ally death state
        if (currHealth <= 0)
        {
            isAllyDead = true;
            isDead = isAllyDead;
            PlaySound("PlayerAllyDowned");
            gameObject.GetComponentInChildren<Image>().enabled = true;
        }
    }//Update

    //Special made function that allows Ally to move towards a target without getting too close, 
    //but also follow the player if no valid enemy target is found
    private void Move2Target(GameObject m2target)
    {
        if (player.GetComponent<PlayerHealthManager>().isPlayerDead == false && player.GetComponent<PlayerHealthManager>().isDebuffed == false)//If the player is not dead and is not debuffed
        {
            if (inCombat == false || m2target == null) //If there is no enemies, follow player
            {
                float playerDistance = Vector3.Distance(transform.position, player.transform.position);
                float spacingDistance = 3;
                if(playerDistance <= spacingDistance) //If player is too close, back off
                {
                    Vector3 dir2Player = transform.position - player.transform.position;
                    Vector3 newPos = transform.position + dir2Player;

                    agent.stoppingDistance = stopDistanceFromPlayer;
                    agent.SetDestination(newPos);
                    //Debug.Log("Avoiding");
                }
                else if(playerDistance > spacingDistance) //If player is too far, get closer
                {
                    agent.stoppingDistance = stopDistanceFromPlayer;
                    agent.SetDestination(player.transform.position);
                }
                transform.LookAt(player.transform);
            } 
            else if (inCombat == true && m2target.tag == "Enemy") //If there is enemies
            {
                float enemyDistance = Vector3.Distance(transform.position, m2target.transform.position); //Calc Distance between self and enemy
                float runAwayDistance = 5; //Distance before backing off
                if(enemyDistance <= runAwayDistance) //If enemies are too close, backoff
                {
                    Vector3 dir2Enemy = transform.position - m2target.transform.position; //Calc direction to enemy
                    Vector3 newPos = transform.position + dir2Enemy; //Add position with enemy direction

                    agent.stoppingDistance = 3f;
                    agent.SetDestination(newPos);
                    //Debug.Log("Avoiding");
                }
                else if (enemyDistance > runAwayDistance) //If enemies are not too close, move closer
                {
                    agent.stoppingDistance = 6f;
                    agent.SetDestination(m2target.transform.position);
                    if(HasLineOfSightTo(m2target, runAwayDistance)) //If the width of their body
                    {
                        if(sphereHit.transform.gameObject.tag != "Enemy")
                        {
                            agent.stoppingDistance = 5.1f;
                            //Debug.Log("Target Obstructed");   
                        }
                    //print(gameObject + " Engaging");    
                    }  
                }
                transform.LookAt(m2target.transform);
            }
        }
        else//If player is dead or Debuffed
        {
            agent.stoppingDistance = 1.5f;
            agent.SetDestination(player.transform.position);
            if(m2target != null)
            {
               transform.LookAt(m2target.transform); 
            }
            else
            {
                transform.LookAt(player.transform); 
            }
        }
    }//Move2Target

    //Update the healthbar on the UI, if fill image has been assigned
    public void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currHealth / (float)maxHealth; //Since we are dealing with percentages, int variables are casted into floats for this calculation.
        }
    }//UpdateHealthBar

    
    private void OnTriggerStay(Collider col)
    {
        if(col.gameObject == player && player.GetComponent<PlayerHealthManager>().isPlayerDead == false)
        {
            if(isAllyDead == true)
            {
                //Start coroutine for reviving Ally if they are downed and the Player is close enough
                StartCoroutine(ReviveAlly());
            }
            if (isAllyDead == false)
            {
                //If player is debuffed and ally can use dispell on the player
                if (player.GetComponent<PlayerHealthManager>().isDebuffed == true && canCureDebuff == true)
                {
                    isUsingDispel = true;
                    StartCoroutine(Dispel());
                }
                else
                {
                    isUsingDispel = false;
                }
            }
        }
    }//OntriggerStay

    //Reset revive timer if Player leaves the radius
    private void OnTriggerExit(Collider col)
    {
        //Reset revive bar to 0 if player exits the radius
        if(col.gameObject == player)
        {
            reviveCurrent = 0;
            reviveBarFill.fillAmount = 0;
        }
    }//OnTriggerExit

    //Revive coroutine
    private IEnumerator ReviveAlly()
    {
        while(reviveCurrent < reviveMax)
        {
            reviveCurrent += reviveMax / reviveRate;
            //Add soundloop for reviving in progress here
            PlaySoundRepeat("ReviveSound");
            if (reviveCurrent >= reviveMax)
            {
                isAllyDead = false;
                isDead = isAllyDead;
                gameObject.GetComponentInChildren<Image>().enabled = false;
                currHealth = maxHealth;
                reviveCurrent = 0;
                reviveBarFill.fillAmount = 0;
                PlaySound("ReviveEnd");
                isRevived = true;
                yield return new WaitForSeconds(0.1f);
                isRevived = false;
            }
            // fill revive bar here
            
            reviveBarFill.fillAmount = reviveCurrent / reviveMax;
            yield break; // makes the coroutine stop; when x is no longer inside the trigger, so we don't have to use StopAllCoroutines
        }
    }//ReviveAlly

    //Restores Player speed and MaxHP default values, then enter a cooldown before it can be reapplied
    private IEnumerator Dispel()
    {
        if(canCureDebuff == true)
        {
            PlaySound("AllyCureDebuff");
            canCureDebuff = false;
            yield return new WaitForSeconds(1f);
            debuffMan.RestorePlayerHealth();
            debuffMan.RestorePlayerSpeed();
            debuffMan.RestorePlayerFireRate();
            player.GetComponent<PlayerHealthManager>().isDebuffable = false;
            StartCoroutine(player.GetComponent<PlayerHealthManager>().DebuffImmunity());
            //Debug.Log(gameObject+" Dispel Cooldown Start");
            yield return new WaitForSeconds(4f);
            canCureDebuff = true;
            //Debug.Log(gameObject+" Dispel Cooldown End");
            yield break;    
        }
    }//Dispel
}
