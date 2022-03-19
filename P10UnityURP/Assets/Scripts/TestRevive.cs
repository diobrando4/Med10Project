using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestRevive : MonoBehaviour
{
    [Tooltip("Is only used on the allies")]
    public Ally allyScript;
    [Tooltip("Is only used on the player")]
    public PlayerHealthManager playerHealthScript;

    // for reviving
    public float reviveMax = 100;
    public float reviveCurrent = 0; // has to be reset each time reviving is aborted
    public float reviveRate = 100;

    public Image reviveBarFill;

    // these are just for testing, at some point should be deleted or comment out
    //public bool isAllyDead = false;
    //public bool isPlayerDead = false;

    void Awake()
    {
        //allyScript = GetComponent<Ally>();
        //playerHealthScript = GetComponent<PlayerHealthManager>();
        // this needs to be changed; since they can only find them on themselves, so for now we've to manually add them in the inspector
        // also i'm not sure how we should do this for the player; since there are 2 allies (it works for the player since there is only 1), not sure how to solve this, maybe a list?
        // or maybe the solution should be to have a player revive script and an ally revive srcript, since they'll be working differently anyway?
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
            // instant revive by key press (i made this for testing)
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Debug.Log("ally is still alive");
                //if (allyScript.isAllyDead == true)
                if (isAllyDead == true)
                {
                    //Debug.Log("ally is dead, pressing E to revive");
                    
                    // gives the ally their max health when revived
                    allyScript.currHealth = allyScript.maxHealth;
                }
            }
            */

            if (allyScript.isAllyDead == true)
            //if (isAllyDead == true)
            {
                StartCoroutine(Revive());
            }
        }
        
        // is used for when an ally enters the trigger of the player
        if (other.gameObject.tag == "GoodGuys")
        {
            // reviving over time by staying inside the trigger
            if (playerHealthScript.isPlayerDead == true)
            //if (isPlayerDead == true)
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
            //Debug.Log(other.tag + " has exit trigger");
            //transform.GetComponent<Renderer>().material.color = Color.blue;

            //StopCoroutine(Revive()); // can't get this one to work
            //StopAllCoroutines();

            // resets revive if unsuccessfully revived
            reviveCurrent = 0;
            reviveBarFill.fillAmount = 0; // for some reason we get an error when both of these are active!
        }
        
        // is used for when an ally exists the trigger of the player
        if (other.gameObject.tag == "GoodGuys")
        {
            //Debug.Log(other.tag + " has exit trigger");
            //transform.GetComponent<Renderer>().material.color = Color.red;

            // ally controller goes here!

            // resets revive if unsuccessfully revived
            reviveCurrent = 0;
            reviveBarFill.fillAmount = 0; // for some reason we get an error when both of these are active! maybe because they all use the same bar?
        }
    }

    IEnumerator Revive()
    {
        while(reviveCurrent < reviveMax)
        {
            //Debug.Log("coroutine is running");

            reviveCurrent += reviveMax / reviveRate;

            if (reviveCurrent >= reviveMax)
            {
                //Debug.Log("ally is alive");
                allyScript.isAllyDead = false;
                //isAllyDead = false;
                allyScript.currHealth = allyScript.maxHealth;

                // we also need something for player, but i don't know how we should split it up!
            }

            // fill revive bar here
            reviveBarFill.fillAmount = reviveCurrent / reviveMax;

            //yield return null;
            yield break; // makes the coroutine stop; when x is no longer inside the trigger, so we don't have to use StopAllCoroutines
        }
    }
}
