using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRevive : MonoBehaviour
{
    [Tooltip("")]
    public Ally allyScript;
    [Tooltip("")]
    public PlayerHealthManager playerHealthScript;

    // for reviving
    public float reviveMax = 100;
    public float reviveCurrent = 0; // has to be reset each time reviving is aborted
    public float reviveRate = 100;

    // these are just for testing
    public bool isAllyDead = false;
    public bool isPlayerDead = false;

    void Awake()
    {
        allyScript = GetComponent<Ally>();
        playerHealthScript = GetComponent<PlayerHealthManager>();
    }

    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("stop coroutine");
            //StopCoroutine(Revive()); // can't get this one to work
            StopAllCoroutines();
        }
    }
    */

    void OnTriggerStay(Collider other)
    {
        // is used for when the player enters the trigger of an ally
        if (other.gameObject.tag == "Player")
        {
            //transform.GetComponent<Renderer>().material.color = Color.yellow;
            
            /*
            // reviving by key press
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Debug.Log("ally is still alive");
                if (allyScript.isAllyDead == true)
                {
                    //Debug.Log("ally is dead, pressing E to revive");
                    
                    // gives the ally their max health when revived
                    allyScript.currHealth = allyScript.maxHealth;
                }
            }
            */

            //if (allyScript.isAllyDead == true)
            if (isAllyDead == true)
            {
                StartCoroutine(Revive());
            }
        }
        
        // is used for when an ally enters the trigger of the player
        if (other.gameObject.tag == "GoodGuys")
        {
            // reviving over time by staying inside the trigger
            //if (playerHealthScript.isPlayerDead == true)
            if (isPlayerDead == true)
            {
                //StartCoroutine(Revive());
            }
        }
    } // OnTriggerStay

    void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.tag + " exit trigger");
        
        // is used for when the player exits the trigger of an ally
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(other.tag + " exit trigger");
            //transform.GetComponent<Renderer>().material.color = Color.blue;

            //StopCoroutine(Revive()); // can't get this one to work
            //StopAllCoroutines();

            reviveCurrent = 0;
        }
        
        // is used for when an ally exists the trigger of the player
        if (other.gameObject.tag == "GoodGuys")
        {
            //Debug.Log(other.tag + " has exit trigger");
            //transform.GetComponent<Renderer>().material.color = Color.red;

            reviveCurrent = 0;
        }
    }

    IEnumerator Revive()
    {
        while(reviveCurrent < reviveMax)
        {
            //Debug.Log("coroutine is running");

            reviveCurrent += reviveMax / reviveRate;

            // fill bar here

            //yield return null;
            yield break; // makes the coroutine stop; when x is no longer inside the trigger
        }
    }
}
