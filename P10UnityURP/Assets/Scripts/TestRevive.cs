using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRevive : MonoBehaviour
{
    public bool isDead = false;

    public Ally allyScript;

    public PlayerHealthManager playerHealthScript;

    void Awake()
    {
        allyScript = GetComponent<Ally>();
        playerHealthScript = GetComponent<PlayerHealthManager>();
    }

    void OnTriggerStay(Collider other)
    {
        // is used for when the player enters the trigger of an ally
        if (other.gameObject.tag == "Player")
        {
            // reviving by key press
            if (Input.GetKeyDown(KeyCode.E))
            {
                //if (isDead == true)
                //Debug.Log("ally is still alive");
                if (allyScript.isAllyDead == true)
                {
                    //Debug.Log("ally is dead, pressing E to revive");
                    // then you can revive the player / ally
                    allyScript.currHealth = allyScript.maxHealth;
                }
            }
        }
        /*
        // is used for when an ally enters the trigger of the player
        if (other.gameObject.tag == "GoodGuys")
        {
            //
            if (playerHealthScript.isPlayerDead == true)
                {
                    //
                }
        }
        */
    }
}
