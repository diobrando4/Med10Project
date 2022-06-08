using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiMeatshieldTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other) //NEW, attempt to fix Meatshield
    {
        if(other.gameObject.tag == "Enemy") //Projectile ignoring is done within Bullet scripts, this only handles collision between Ally and Enemy characters
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(),other.gameObject.GetComponent<Collider>(),true);
            //Debug.Log(other.gameObject.tag);
        } 
    }
}
