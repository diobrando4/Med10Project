using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public float lifeTime; // in seconds?
    public int damageGiven;
    protected Rigidbody rb;
    public ParticleSystem impactPart;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Damages the given NPC type (Ally or Any type of BaseClassEnemy)
    protected void HurtNPCType(GameObject _npc, int _damage)
    {
        _npc.GetComponent<BaseClassNPC>().DamageTaken(_damage);
    }//HurtEnemyType

    //Damages the given player
    protected void HurtPlayerType(GameObject _player, float _damage)
    {
        _player.GetComponent<PlayerHealthManager>().HurtPlayer(_damage);
    }//HurtPlayerType

    protected void EnemyBulletFilter(Collision other, int debuffNum)
    {
        if(other.gameObject.tag != "Projectile")
        {
            if(other.gameObject.tag == "GoodGuys") //If it hits something with tag GoodGuys
            {
                HurtNPCType(other.gameObject,damageGiven); //Hurt the given gO that was collided with
                ImpactEffect(other);
                Destroy(gameObject);
            }
            else if(other.gameObject.tag == "Player") //If it hits something with tag Player
            {
                HurtPlayerType(other.gameObject,damageGiven); //Hurt player
                FindObjectOfType<DebuffManager>().DebuffSelector(debuffNum);
                ImpactEffect(other);
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
                ImpactEffect(other);
                Destroy(gameObject);
            }
        }
        else
        {
            //If it hits something with the tag Projectile, ignore collision between the other bullet and itself
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }//EnemyBulletFilter

    protected void ImpactEffect(Collision other) 
    {
        ParticleSystem newImpactEffect = Instantiate(impactPart, transform.position, Quaternion.Inverse(transform.rotation)) as ParticleSystem;
        //newImpactEffect.GetComponent<Renderer>().material.color = other.gameObject.GetComponent<Renderer>().material.color;
        newImpactEffect.GetComponent<Renderer>().material.color = gameObject.GetComponent<Renderer>().material.color;
        newImpactEffect.Play();
        //Debug.Log(main.startColor);
    }
}
