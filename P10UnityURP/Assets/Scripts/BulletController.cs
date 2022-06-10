using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public float lifeTime; // in seconds?
    public float damageGiven;
    protected Rigidbody rb;
    public ParticleSystem impactPart;
    [HideInInspector]
    public bool isPiercing = false; //Can the bullet pierce through hostiles? Only used in GoodGuysBullet so far

    //private Ally _ally;

    void Start()
    {
        //_ally = GameObject.FindWithTag("GoodGuys").GetComponent<Ally>();
    }

    //Damages the given NPC type (Ally or Any type of BaseClassEnemy)
    protected void HurtNPCType(GameObject _npc, float _damage)
    {
        _npc.GetComponent<BaseClassNPC>().DamageTaken(_damage);
    }//HurtEnemyType

    //Damages the given player
    protected void HurtPlayerType(GameObject _player, float _damage)
    {
        _player.GetComponent<PlayerHealthManager>().HurtPlayer(_damage);
    }//HurtPlayerType

/*     protected void EnemyBulletFilter(Collision other, int debuffNum)
    {
        if(other.gameObject.tag != "Projectile")
        {
            if(other.gameObject.tag == "GoodGuys") //If it hits something with tag GoodGuys
            {
                HurtNPCType(other.gameObject,damageGiven); //Hurt the given gO that was collided with
                ImpactEffect();
                Destroy(gameObject);
            }
            else if(other.gameObject.tag == "Player") //If it hits something with tag Player
            {
                HurtPlayerType(other.gameObject,damageGiven); //Hurt player
                FindObjectOfType<DebuffManager>().DebuffSelector(debuffNum);
                ImpactEffect();
                Destroy(gameObject);
            }
            else if(other.gameObject.layer == gameObject.layer) //If it shares the same layer as this bullet, ignore collision
            {
                Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
            }
            else //If it collides with anything else, Destroy self
            {
                if (FindObjectOfType<SoundManager>())
                {
                    FindObjectOfType<SoundManager>().SoundPlay("BulletImpact");
                }
                ImpactEffect();
                Destroy(gameObject);
            }
        }
        else
        {
            //If it hits something with the tag Projectile, ignore collision between the other bullet and itself
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }//EnemyBulletFilter */

    //Version for Meatshield Fix
    protected void EnemyBulletFilter(Collision other, int debuffNum, bool toggleMeatshield)
    {
        if(other.gameObject.tag != "Projectile")
        {
            if(other.gameObject.tag == "GoodGuys") //If it hits something with tag GoodGuys
            {
                if(toggleMeatshield == true)
                {
                    HurtNPCType(other.gameObject,damageGiven); //Hurt the given gO that was collided with
                    ImpactEffect();
                    Destroy(gameObject);   
                }
                else
                {
                    if(other.gameObject.GetComponent<Ally>().isAllyDead == false)
                    {
                        HurtNPCType(other.gameObject,damageGiven); //Hurt the given gO that was collided with
                        ImpactEffect();
                        Destroy(gameObject);  
                    }
                    else
                    {
                        Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
                    }                  
                }
            }
            else if(other.gameObject.tag == "Player") //If it hits something with tag Player
            {
                if(toggleMeatshield == true) //If true, use meatshield logic
                {
                    HurtPlayerType(other.gameObject,damageGiven); //Hurt player
                    FindObjectOfType<DebuffManager>().DebuffSelector(debuffNum);
                    ImpactEffect();
                    Destroy(gameObject);  
                }
                else //If false, do not use meatshield
                {
                    if(other.gameObject.GetComponent<PlayerHealthManager>().isPlayerDead == false) //If not dead, allow collision
                    {
                        HurtPlayerType(other.gameObject,damageGiven); //Hurt player
                        FindObjectOfType<DebuffManager>().DebuffSelector(debuffNum);
                        ImpactEffect();
                        Destroy(gameObject);          
                    }  
                    else //if dead, ignore collision
                    {
                        Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
                    }                            
                }
            }
            else if(other.gameObject.layer == gameObject.layer) //If it shares the same layer as this bullet, ignore collision
            {
                Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
            }
            else //If it collides with anything else, Destroy self
            {
                if (FindObjectOfType<SoundManager>())
                {
                    FindObjectOfType<SoundManager>().SoundPlay("BulletImpact");
                }
                ImpactEffect();
                Destroy(gameObject);
            }
        }
        else
        {
            //If it hits something with the tag Projectile, ignore collision between the other bullet and itself
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }//EnemyBulletFilter

    protected void ImpactEffect() 
    {
        ParticleSystem newImpactEffect = Instantiate(impactPart, transform.position, Quaternion.Inverse(transform.rotation)) as ParticleSystem;
        newImpactEffect.GetComponent<Renderer>().material.color = gameObject.GetComponent<Renderer>().material.color;
        newImpactEffect.transform.localScale = transform.localScale + new Vector3(0.8f,0.8f,0.8f);
        newImpactEffect.Play();
    }
}
