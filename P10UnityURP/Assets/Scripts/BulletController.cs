using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public float lifeTime = 3.0f; // in seconds?
    public int damageGiven = 1;
    protected Rigidbody rb;

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
}
