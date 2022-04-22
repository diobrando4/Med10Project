using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//BaseClass for all variables and functions that a majority of multiple Actors are going to have or use in some shape or form
//Childs can then inherit the variables and functions, and call/overload them as needed

public class BaseClassNPC : MonoBehaviour
{
    protected NavMeshAgent agent; //NavMeshAgent of GameObject
    [HideInInspector]
    public GameObject target; //GameObject that it needs to target
    
    [Header("Internal variables")]
    public bool inCombat = true; //Bool if combat mode is active
    public int damageGiven = 1; //default damage given.
    protected int ignoreOwnLayer; //Should ignore the layer its in, in order to ignore targets of the same "faction"

    //Variables related to status effect
    protected DebuffManager debuffMan;

    [Header("Health related variables")]
    public int maxHealth; //Max health of GameObject
    public int currHealth; //Current health of GameObject
    protected bool isDead = false; //Death check

    
    [Header("Only Relevant if it shoots")]
    public float fireRate;
    public float projectileSpeed;
    public float distanceB4Shoot; //Distance before shooting at target2Shoot
    private float shotCounter;
    //public GoodGuysBullet goodGuysBullet; // for player and ally (GoodGuys)
    //public EnemyBullet enemyBullet; // for enemy
    public BulletController bullet = null; //Can either be GoodGuysBullet or EnemyBullet
    [SerializeField]
    protected Transform muzzle;

    // for raycast
    private RaycastHit rayHit;
    protected RaycastHit sphereHit;

    protected ParticleSystem muzzleSmoke;

    Vector3 rayOffset; // we need this, otherwise the raycast might hit unwanted objects EDIT: No longer an issue with the new layer filtering

    void Awake()
    {
        rayOffset = new Vector3(0,0,0);
        inCombat = true;
        if (debuffMan == null)
        {
            debuffMan = GameObject.Find("DebuffManager").GetComponent<DebuffManager>();
        }
        else
        {
            Debug.Log("Check if DebuffManager is missing");
        }
        //Bit shift index of own layer to create a bit mask
        ignoreOwnLayer = 1<<gameObject.layer;
        //Invert the bit mask e.g. focus only on own layer to, focus on all other layers than own
        ignoreOwnLayer = ~ignoreOwnLayer;
    }

    //Function to find the closest target with the given tag
    protected GameObject FindClosestTargetWithTag(string tag)
    {
        GameObject[] candidates;
        candidates = GameObject.FindGameObjectsWithTag(tag);

        GameObject closestTarget = null;

        float distance = Mathf.Infinity;
        Vector3 pos = transform.position;
        foreach(GameObject posCand in candidates)
        {
            Vector3 diff = posCand.transform.position - pos;
            float curDist = diff.sqrMagnitude;
             if (curDist < distance)
             {
                if(posCand.GetComponentInParent<PlayerHealthManager>())
                {
                    if(posCand.GetComponentInParent<PlayerHealthManager>().isPlayerDead == false)
                    {
                        //if (curDist < distance)
                        //{
                        //Debug.Log(posCand);   
                        closestTarget = posCand;
                        distance = curDist; 
                        //}
                    } 
                }
                else if(posCand.GetComponent<BaseClassNPC>())
                {
                    if(posCand.GetComponent<BaseClassNPC>().isDead == false)
                    {
                        //if (curDist < distance)
                        //{
                        //Debug.Log(posCand);   
                        closestTarget = posCand;
                        distance = curDist; 
                        //}
                    }
                    else{Debug.Log(posCand+" "+posCand.GetComponent<BaseClassNPC>().isDead);}
                }                 
            }
        }
        return closestTarget;

    }//FindClosestTarget

    //Simple follow to a GameObject
    public void Follow(GameObject target2Follow)
    {   
        agent.SetDestination(target2Follow.transform.position);
    }//Follow

    //Call function to reduce health
    public void DamageTaken(int damage)
    {
        currHealth -= damage;
        if(gameObject.tag == "Enemy")
        {
            PlaySound("EnemyHurt");
        }
        else if(gameObject.tag == "GoodGuys")
        {
            PlaySound("PlayerAllyHurt");
        }
    }//DamageTaken

    //Function that dictates shooting the nearest GameObject
    protected void ShootNearestObject(GameObject target2Shoot)
    {
        if (target2Shoot != null ) //If there are enemies
        {
            //if (Physics.Raycast(transform.position + rayOffset, transform.forward, out rayHit, distanceB4Shoot, ignoreOwnLayer))
            if (Physics.SphereCast(transform.position + rayOffset, 0.2f, transform.forward, out rayHit, distanceB4Shoot, ignoreOwnLayer))
            //If the raycasted sphere hits something within the distance
            {
                Debug.DrawLine(transform.position + rayOffset, rayHit.point, Color.red);

                shotCounter -= Time.deltaTime;
                if(rayHit.transform != null) //If the ray is hitting something
                {
                    if(rayHit.transform.gameObject.layer != gameObject.layer && 
                       rayHit.transform.gameObject.layer == LayerMask.NameToLayer("GoodGuys") || 
                       rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) //If the object hit by the ray doesnt share the same layer as self, but is also either on the layer GoodGuys/Enemy
                    {
                        if(shotCounter <= 0)
                        {
                            shotCounter = fireRate;
                            BulletController newBullet = Instantiate(bullet, muzzle.position, muzzle.rotation) as BulletController;
                            PlaySound("Gunshot");
                            newBullet.speed = projectileSpeed;
                            muzzleSmoke.Play();
                        }
                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position + rayOffset, transform.forward * distanceB4Shoot, Color.green);
                shotCounter = 0;
            }
        }       
    }//ShootNearest

    //Check if there is line of sight to target with the bullet width in mind
    protected bool HasLineOfSightTo(GameObject _target, float _distance)
    {
        if(Physics.SphereCast(transform.position + rayOffset, 0.2f, transform.forward, out sphereHit, _distance, ignoreOwnLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }//HasLineOfSightTo

    public void PlaySound(string _sound)
    {
        if (FindObjectOfType<SoundManager>())
        {
            FindObjectOfType<SoundManager>().SoundPlay(_sound);
        }
        else
        {
            Debug.Log("Can't find SoundManager");
        }
    }//PlaySound

    public void PlaySound(string _sound,bool _stop)
    {
        if (FindObjectOfType<SoundManager>())
        {
            if(_stop == false)
            {
                FindObjectOfType<SoundManager>().SoundPlay(_sound);
            }
            else if (_stop == true)
            {
                FindObjectOfType<SoundManager>().SoundStop(_sound);
            }
        }
        else
        {
            Debug.Log("Can't find SoundManager");
        }
    }//PlaySound

    public void PlaySoundRepeat(string _sound)
    {
        if (FindObjectOfType<SoundManager>())
        {
            FindObjectOfType<SoundManager>().SoundRepeatWOInterrupt(_sound);
        }
        else
        {
            Debug.Log("Can't find SoundManager");
        }
    }
}
