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
    private float reviveMax = 100;
    private float reviveCurrent = 0; // has to be reset each time reviving is aborted
    private float reviveRate = 100;
    public Image reviveBarFill;

    void Start()
    {
        //Initial Values can be defined for the inherited variables
        maxHealth = 6;
        currHealth = maxHealth;
        distanceB4Shoot = 10; 
        projectileSpeed = 10f;
        fireRate = 0.75f;
        muzzle = gameObject.transform.Find("AllyGun/Muzzle");

        player = GameObject.FindGameObjectWithTag("Player");
        reviveBarFill = gameObject.transform.Find("ReviveBarPopUp/Canvas/ReviveBar/imgBackground/imgFill").GetComponent<Image>();
        gameObject.GetComponentInChildren<Image>().enabled = false; //Disable Image comp of Imgbackground on start

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
            allyFriend = GameObject.Find("AllyOrangeBot").GetComponent<Ally>();
        }
        else if (gameObject == GameObject.Find("AllyOrangeBot"))
        {
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
        //Debug.Log(player);
        target = FindClosestTargetWithTag("Enemy");//Inherited function
        if (isAllyDead == false) 
        {
            Move2Target(target);
            ShootNearestObject(target);//Inherited function
        }

        UpdateHealthBar();

        // ally death state
        if (currHealth <= 0)
        {
            isAllyDead = true;
            gameObject.GetComponentInChildren<Image>().enabled = true;
        }
    }//Update

    //Special made function that allows Ally to move towards a target without getting too close, 
    //but also follow the player if no valid enemy target is found
    void Move2Target(GameObject m2target)
    {
        if (player.GetComponent<PlayerHealthManager>().isPlayerDead == false)//If the player is not dead
        {
            if (inCombat == false || m2target == null) //If there is no enemies, follow player
            {
                agent.stoppingDistance = stopDistanceFromPlayer;
                agent.SetDestination(player.transform.position);
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
                    //print(gameObject + " Avoiding");
                }
                else if (enemyDistance > runAwayDistance) //If enemies are not too close, move closer
                {
                    agent.stoppingDistance = 6f;
                    agent.SetDestination(m2target.transform.position);
                    //print(gameObject + " Engaging");
                }
            transform.LookAt(m2target.transform);
            }
        }
        else//If player is dead
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
        if(col.gameObject == player)
        {
            if(isAllyDead == true)
            {
                StartCoroutine(ReviveAlly());
            }
        }
    }//OntriggerStay

    private void OnTriggerExit(Collider col)
    {
        //Reset revive bar to 0 if player exits the radius
        if(col.gameObject == player)
        {
            reviveCurrent = 0;
            reviveBarFill.fillAmount = 0;
        }
    }

    IEnumerator ReviveAlly()
    {
        while(reviveCurrent < reviveMax)
        {
            reviveCurrent += reviveMax / reviveRate;
            if (reviveCurrent >= reviveMax)
            {
                isAllyDead = false;
                gameObject.GetComponentInChildren<Image>().enabled = false;
                currHealth = maxHealth;
                reviveCurrent = 0;
                reviveBarFill.fillAmount = 0;
            }
            // fill revive bar here
            reviveBarFill.fillAmount = reviveCurrent / reviveMax;
            yield break; // makes the coroutine stop; when x is no longer inside the trigger, so we don't have to use StopAllCoroutines
        }
    }
}