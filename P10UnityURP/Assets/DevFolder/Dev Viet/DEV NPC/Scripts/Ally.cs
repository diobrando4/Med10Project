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
    private GameObject accesories;
    //public bool isOldVersion = false;

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
    [Tooltip("What weapon should the Ally use? 0 = Default, 1 = Shotgun, 2 = Sniper")]   
    public int selectedWeapon = 0;

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
//======================================================================================================================================================
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

        //accesories = gameObject.transform.Find("Accesories");

        //Change variable to old
        if (isOldVersion == true)
        {
            selectedWeapon = 0;
            gameObject.transform.GetChild(4).gameObject.SetActive(false);
            if(gameObject.name == "AllyBlueBot")
            {
                stopDistanceFromPlayer = 3; //Follow distance to player when there is no more enemies
                distanceBeforeRunAway = 5f; //How close does the enemy have to be before the ally start running away?
                maxDistanceFromPlayer = 5f; //How far away can the companion move away from the player
                stopDistanceOnBackoff = 3f; //Stop distance when trying to run away from at target
                stopDistanceOnApproach = 6f; //Stop distance when approaching a target
            }
            else if (gameObject.name == "AllyOrangeBot")
            {
                stopDistanceFromPlayer = 6;
                distanceBeforeRunAway = 5f;
                maxDistanceFromPlayer = 12f;
                stopDistanceOnBackoff = 3f;
                stopDistanceOnApproach = 6f;
            }
        }
        else 
        {
            gameObject.transform.GetChild(4).gameObject.SetActive(true);     
            if(gameObject.name == "AllyBlueBot")
            {
                stopDistanceFromPlayer = 3;
                distanceBeforeRunAway = 6f;
                maxDistanceFromPlayer = 4f;
                stopDistanceOnBackoff = 10f;
                stopDistanceOnApproach = 8f;
                projectileDetectionRange = 30;//How far can they see the projectile that is coming in?
                projReactivity = 20;//How fast do they react and try to avoid incoming projectiles?
            }
            else if (gameObject.name == "AllyOrangeBot")
            {
                stopDistanceFromPlayer = 6;
                distanceBeforeRunAway = 5f;
                maxDistanceFromPlayer = 12f;
                stopDistanceOnBackoff = 2f;
                stopDistanceOnApproach = 5f;
                projectileDetectionRange = 15;
                projReactivity = 15;
            }                 
        }

        //Since you Ally cant change weapons, and weapons are not a seperate class, I have to define the fire rate here, depending on the selectedWeapon
        if(selectedWeapon == 0) //Default
        {
            fireRate = 0.75f;
            distanceB4Shoot = 20f;
        }
        else if (selectedWeapon == 1) //Shotgun
        {
            fireRate = 1f;
            distanceB4Shoot = 20f;
        }
        else if (selectedWeapon == 2) //Sniper
        {
            fireRate = 2f;
            distanceB4Shoot = 30f;
        }
    }//Start
//======================================================================================================================================================
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
            ShootNearestObject(target,selectedWeapon);//Inherited function 
            gameObject.transform.Find("StatusIcon").GetComponent<Renderer>().enabled = false;
        }
        else
        {
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
        if (currHealth <= 0 && isDead == false)
        {
            isAllyDead = true;
            isDead = isAllyDead;
            PlaySound("PlayerAllyDowned");
            agent.SetDestination(gameObject.transform.position);
            //gameObject.GetComponentInChildren<Image>().enabled = true;

            // for disabling meat shield
            //gameObject.layer = 8; // 8 = dead layer
            //this.GetComponent<BoxCollider>().enabled = false;
            //this.GetComponent<NavMeshAgent>().enabled = false;
            agent.enabled = false; //Related to meatshield
        }
    }//Update
//======================================================================================================================================================
    //Function that handles conditions on how to move
    private void Move(GameObject m2target)
    {
        if (player.GetComponent<PlayerHealthManager>().isPlayerDead == false && player.GetComponent<PlayerHealthManager>().isDebuffed == false)//If the player is not dead and is not debuffed
        {
            if (inCombat == false || m2target == null) //If there is no enemies, follow player
            {   
                //Move2Target(GameObject Target, float BackoffDistance, float StopDistOnBackoff, float StopDistanceOnApproach)
                Move2Target(player,3,stopDistanceFromPlayer,stopDistanceFromPlayer);
                agent.speed = 5f;
            } 
            else if (inCombat == true && m2target.tag == "Enemy") //If there is enemies
            {
                //Move2Target(GameObject Target, float BackoffDistance,float MaxDistanceAwayFromPlayer , float StopDistOnBackoff, float StopDistanceOnApproach, bool avoidProjectiles)
                if (isOldVersion == true)
                {
                    Move2Target(m2target,distanceBeforeRunAway,maxDistanceFromPlayer,stopDistanceOnBackoff,stopDistanceOnApproach,false);
                }
                else
                {
                    Move2Target(m2target,distanceBeforeRunAway,maxDistanceFromPlayer,stopDistanceOnBackoff,stopDistanceOnApproach,true);
                    if(gameObject.name == "AllyBlueBot")
                    {
                        agent.speed = 5;
                    }
                    else if (gameObject.name == "AllyOrangeBot")
                    {
                        agent.speed = 6.5f;
                    }  
                }
            }
        }
        else//If player is dead or Debuffed
        {
            agent.stoppingDistance = 2f;
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
//======================================================================================================================================================
    //Update the healthbar on the UI, if fill image has been assigned
    public void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currHealth / maxHealth; //Since we are dealing with percentages, int variables are casted into floats for this calculation.
        }
    }//UpdateHealthBar
//======================================================================================================================================================    
    private void OnCollisionEnter(Collision other) //NEW, attempt to fix Meatshield
    {
        if (isOldVersion == false)
        {
            if(isDead == true)
            {
                if(other.gameObject.tag == "Enemy") //Projectile ignoring is done within Bullet scripts, this only handles collision between Ally and Enemy characters
                {
                    Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other.gameObject.GetComponent<Collider>(), true);
                    Debug.Log(other.gameObject.tag);
                }
            }            
        }
    }
//======================================================================================================================================================
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
//======================================================================================================================================================
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
//======================================================================================================================================================
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
                agent.enabled = true; //Related to Meatshield
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
//======================================================================================================================================================
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
//======================================================================================================================================================
    //Overloaded version of Move2Target from BaseClassNPC
    //Takes in the consideration of distance from player
    protected void Move2Target(GameObject _target, float _backoff,float maxDistFromPlayer, float _stopDistBackoff, float _stopDistApproach, bool avoidProjectile)
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
                if(avoidProjectile == false) //False is old version
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
                else //True is new version
                {
                    if (trackedProjectile == null)
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
                    else
                    {
                        agent.SetDestination(PosisitionAwayFromProjectile(trackedProjectile, projReactivity));
                    }      
                }
          
            }
        transform.LookAt(_target.transform);
    }//Overload Move2Target
//======================================================================================================================================================
    //Overload to incorporate weapon types and variable fire rate
    protected void ShootNearestObject(GameObject target2Shoot, int weaponType)
    {
        if (target2Shoot != null ) //If there are tfargets
        {
            //if (Physics.Raycast(transform.position + rayOffset, transform.forward, out rayHit, distanceB4Shoot, ignoreOwnLayer))
            if (Physics.SphereCast(transform.position + rayOffset, 0.2f, transform.forward, out rayHit, distanceB4Shoot, ignoreOwnLayer))
            //If the raycasted sphere hits something within the distance
            {
                Debug.DrawLine(transform.position + rayOffset, rayHit.point, Color.red);

                if(rayHit.transform != null) //If the ray is hitting something
                {
                    if(rayHit.transform.gameObject.layer != gameObject.layer && 
                        rayHit.transform.gameObject.layer == LayerMask.NameToLayer("GoodGuys") || 
                        rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) //If the object hit by the ray doesnt share the same layer as self, but is also either on the layer GoodGuys/Enemy
                    {
                        shotCounter -= Time.deltaTime;

                        if(shotCounter <= 0)
                        {   
                            shotCounter = fireRate;
                            PlaySound("Gunshot");
                            muzzleSmoke.Play();
                            WeaponSelect(selectedWeapon);
                            //Need to work here for Different weapon types
                            //BulletController newBullet = Instantiate(bullet, muzzle.position, muzzle.rotation) as BulletController;
                            //newBullet.speed = projectileSpeed;
                        }
                    }
                    else
                    {
                        shotCounter = 0;
                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position + rayOffset, transform.forward * distanceB4Shoot, Color.green); 
            }
        }       
    }//Overload ShootNearest
//======================================================================================================================================================
    protected void WeaponSelect (int weaponNumber)
    {
        switch(weaponNumber)
        {
            case 0: //Normal
                BulletController DefaultBullet = Instantiate(bullet, muzzle.position, muzzle.rotation);
                DefaultBullet.damageGiven = 1;
                DefaultBullet.speed = 15f;
                break;
            case 1: //Shotgun
                float spread = 15f;
                int numOfPellets = 10;
                for (int i = 0; i <= numOfPellets; i++)
                {
                    IEnumerator DelayBetweenPellets(float timeBetweenPellets)
                    {
                        yield return new WaitForSeconds(timeBetweenPellets);
                        BulletController ShotgunPellet = Instantiate(bullet, muzzle.position, Quaternion.Euler(new Vector3(0, muzzle.transform.eulerAngles.y+Random.Range(-spread, spread), 0)));
                        ShotgunPellet.speed = 20f;
                        ShotgunPellet.damageGiven = 0.15f;
                        ShotgunPellet.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
                        ShotgunPellet.rb.mass = 0.03f;
                        ShotgunPellet.GetComponent<TrailRenderer>().startWidth = 0.05f;  
                        if (i > numOfPellets)
                        {
                            yield break;
                        }                      
                    }
                    StartCoroutine(DelayBetweenPellets(0.01f*i));
                }
                break;
            case 2: //Sniper
                BulletController SniperBullet = Instantiate(bullet, muzzle.position, muzzle.rotation);
                SniperBullet.isPiercing = true;
                SniperBullet.damageGiven = 3f;
                SniperBullet.speed = 60f;
                SniperBullet.rb.mass = 0.05f;
                SniperBullet.transform.localScale = new Vector3(0.15f,0.15f,0.5f);
                break;
        }    
    }//WeaponSelect
}
