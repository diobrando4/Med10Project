using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodGuysBullet : BulletController
{
    public int hitCount = 0;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();        
    }

    void Start()
    {
        lifeTime = 3.0f;
        //damageGiven = 1;
        //GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // this needs to use rigidbody instead!
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);

        rb.velocity = transform.forward * speed;

        // destroys the bullet after having reached the end of its life time
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    // for when bullets hit something (seems like you need rigidbody to make this work)
    void OnCollisionEnter(Collision other)
    {
        GoodGuysBulletFilter(other, isPiercing);
    } // OnCollisionEnter

    protected void GoodGuysBulletFilter(Collision other, bool _isPierce)
    {
        if(other.gameObject.tag != "Projectile")
            {
            if(other.gameObject.tag == "Enemy")
            {
                if(_isPierce == true)
                {
                    if (hitCount <= 0)
                    {
                        //Function inherited from Parent, refers to the enemy's BaseClassNPC Component 
                        HurtNPCType(other.gameObject,damageGiven);
                        ImpactEffect();
                        Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>()); 
                        hitCount += 1;
                        //Debug.Log("HIT "+hitCount);
                    }
                    else if(hitCount > 0)
                    {
                        damageGiven /= 1.25f;
                        HurtNPCType(other.gameObject,damageGiven);
                        ImpactEffect();
                        Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>()); 
                        hitCount += 1;
                    }
                }
                else
                {
                    //Function inherited from Parent, refers to the enemy's BaseClassNPC Component 
                    HurtNPCType(other.gameObject,damageGiven);
                    // destroys the bullet when hitting the enemy
                    ImpactEffect();
                    Destroy(gameObject);
                }
            }
            else if(other.gameObject.layer == gameObject.layer) //If it shares the same layer as this bullet, ignore collision
            {
                Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
            }
            else
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
    }
}
