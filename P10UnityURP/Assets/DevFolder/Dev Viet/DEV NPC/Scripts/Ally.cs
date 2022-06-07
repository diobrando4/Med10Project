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

    //Movement related
    [Header("Movement Related")]
    [Tooltip("How close will the Ally follow the player off Combat?")]
    public float stopDistanceFromPlayer;
    [Tooltip("How close will the enemy be before the Ally begins to back off?")]
    public float distanceBeforeRunAway = 5f;
    [Tooltip("How far away will Ally move away from player during combat?")]
    public float maxDistanceFromPlayer = 10f;
    [Tooltip("While backing off, how close will Ally be to the target?")]
    public float stopDistanceOnBackoff = 3f;
    [Tooltip("While approaching, how close will Ally be to the target?")]
    public float stopDistanceOnApproach = 6f;
    [Tooltip("What is the radius for detecting hostile projectiles?")]
    public float projectileDetectionRange = 25;
    [Tooltip("How sensitive are is the character to projectiles?")]
    public float projReactivity = 50;

    private Ally allyFriend;

    private Image healthBarFill; //Need to automate this later
    [HideInInspector] 
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
    public ParticleSystem revivePart;

    //Dispel related
    private bool canCureDebuff = true;
    //[HideInInspector]
    [HideInInspector] 
    public bool isUsingDispel = false;

    void Start()
    {
        //Initial Values can be defined for the inherited variables
        //maxHealth = 7;
        currHealth = maxHealth;
        // distanceB4Shoot = 10; 
        // projectileSpeed = 15f;
        // fireRate = 0.75f;
        muzzle = gameObject.transform.Find("AllyGun/Muzzle");

        player = GameObject.FindGameObjectWithTag("Player");
        reviveBarFill = gameObject.transform.Find("ReviveBarPopUp/Canvas/ReviveBar/imgBackground/imgFill").GetComponent<Image>();
        gameObject.GetComponentInChildren<Image>().enabled = false; //Disable Image comp of Imgbackground on start
        gameObject.transform.Find("StatusIcon").GetComponent<Renderer>().enabled = false;

        //NavMeshAgent Check & Init
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
            //healthBarFill = GameObject.Find("CanvasHealthBars/HolderHealthBars/HolderBlueHealthBar/imgBackground/imgFillBlue").GetComponent<Image>();
            healthBarFill = GameObject.Find("CanvasHealthBars/HolderBlueHealthBar/imgBackground/imgFillBlue").GetComponent<Image>();
            allyFriend = GameObject.Find("AllyOrangeBot").GetComponent<Ally>();
        }
        else if (gameObject == GameObject.Find("AllyOrangeBot"))
        {
            //healthBarFill = GameObject.Find("CanvasHealthBars/HolderHealthBars/HolderOrangeHealthBar/imgBackground/imgFillOrange").GetComponent<Image>();
            healthBarFill = GameObject.Find("CanvasHealthBars/HolderOrangeHealthBar/imgBackground/imgFillOrange").GetComponent<Image>();
            allyFriend = GameObject.Find("AllyBlueBot").GetComponent<Ally>();
        }
        //Find where the bullet spawns from
        muzzle = gameObject.transform.Find("AllyGun/Muzzle");
        muzzleSmoke = gameObject.transform.Find("AllyGun/SmokeParticles").GetComponent<ParticleSystem>();
        UpdateHealthBar();
        revivePart.Stop();
    }//Start

    // Update is called once per frame
    void Update()
    {
        //DestroyOnDeath(); //Inherited function
        //Debug.Log(isRevived);
        target = FindClosestTargetWithTag("Enemy");//Inherited function
        trackedProjectile = FindClosestTargetWithTag("Projectile", projectileDetectionRange);
        if (isAllyDead == false) 
        {
            Move(target);
            ShootNearestObject(target);//Inherited function
            gameObject.transform.Find("StatusIcon").GetComponent<Renderer>().enabled = false;
        }
        else
        {
            agent.SetDestination(gameObject.transform.position);
            if(reviveCurrent <= 0)
            {
                gameObject.transform.Find("StatusIcon").GetComponent<Renderer>().enabled = true;
            }
            else
            {
                gameObject.transform.Find("StatusIcon").GetComponent<Renderer>().enabled = false;
            }
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
            //gameObject.GetComponentInChildren<Image>().enabled = true;

            // for disabling meat shield
            //gameObject.layer = 8; // 8 = dead layer
            //this.GetComponent<BoxCollider>().enabled = false;
            //this.GetComponent<NavMeshAgent>().enabled = false;
        }
    }//Update

    //Function that handles conditions on how to move
    private void Move(GameObject m2target)
    {
        if (player.GetComponent<PlayerHealthManager>().isPlayerDead == false && player.GetComponent<PlayerHealthManager>().isDebuffed == false)//If the player is not dead and is not debuffed
        {
            if (inCombat == false || m2target == null) //If there is no enemies, follow player
            {   
                //Move2Target(GameObject Target, float BackoffDistance, float StopDistOnBackoff, float StopDistanceOnApproach)
                Move2Target(player,3,stopDistanceFromPlayer,stopDistanceFromPlayer);
            } 
            else if (inCombat == true && m2target.tag == "Enemy") //If there is enemies
            {
                //Move2Target(GameObject Target, float BackoffDistance,float MaxDistanceAwayFromPlayer , float StopDistOnBackoff, float StopDistanceOnApproach)
                Move2Target(m2target,distanceBeforeRunAway,maxDistanceFromPlayer,stopDistanceOnBackoff,stopDistanceOnApproach);
            }
        }
        else//If player is dead or Debuffed
        {
            agent.stoppingDistance = 1.5f;
            agent.SetDestination(player.transform.position);
            if(m2target != null) //If there is still enemies, look at them, if not, look at player
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
                revivePart.Play();
                gameObject.GetComponentInChildren<Image>().enabled = true;
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
            revivePart.Stop();
            gameObject.GetComponentInChildren<Image>().enabled = false;
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
                revivePart.Stop();
                yield return new WaitForSeconds(0.1f);
                isRevived = false;

                // for disabling meat shield
                //gameObject.layer = 6; // 6 = good guys
                //this.GetComponent<BoxCollider>().enabled = true;
                //this.GetComponent<NavMeshAgent>().enabled = true;
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
            yield return new WaitForSeconds(2f);
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

    //Overloaded version of Move2Target from BaseClassNPC
    //Takes in the consideration of distance from player
    protected void Move2Target(GameObject _target, float _backoff,float maxDistFromPlayer, float _stopDistBackoff, float _stopDistApproach)
    {
        float _targetDist = Vector3.Distance(transform.position, _target.transform.position);
        float _playerDist = Vector3.Distance(transform.position, player.transform.position);
        float _spacing2Player = maxDistFromPlayer;
        float _backOffDistance = _backoff;

            if(_playerDist > _spacing2Player) //If far from player, Move Closer to player
            {
                agent.stoppingDistance = _spacing2Player/2;
                agent.SetDestination(player.transform.position);
            }
            else
            {
                if (trackedProjectile == null) //Disable for Old version
                {              
                    if(_targetDist <= _backOffDistance) //If too close to target, Back off from target
                    {
                        Vector3 dir2Target = transform.position - _target.transform.position;
                        Vector3 newPos = transform.position + dir2Target;
                        agent.stoppingDistance = _stopDistBackoff;
                        agent.SetDestination(newPos);
                    }
                    else if(_targetDist > _backOffDistance) //If far from target, Move Closer to target
                    {
                        agent.stoppingDistance = _stopDistApproach;
                        agent.SetDestination(_target.transform.position);
                        if(HasLineOfSightTo(_target, _backOffDistance)) //If the width of their body cant reach target, it is obstructed
                        {
                            if(sphereHit.transform.gameObject.tag != _target.tag)
                            {
                                agent.stoppingDistance = _stopDistApproach-1f;  
                            }   
                        }  
                    }
                }
                else //Disable for Old version
                {
                    agent.SetDestination(PosisitionAwayFromProjectile(trackedProjectile, projReactivity));
                }                
            }
        transform.LookAt(_target.transform);
    }
}
