using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestRevive : MonoBehaviour
{
    [Tooltip("Is only used on the allies")]
    public Ally allySelfScript;    
    public Ally allyFriendScript;
    [Tooltip("Is only used on the player")]
    public PlayerHealthManager playerHealthScript;

    // for reviving
    public float reviveMax = 100;
    public float reviveCurrent = 0; // has to be reset each time reviving is aborted
    public float reviveRate = 100;

    public Image reviveBarFill;

    // these are just for testing, at some point should be deleted or comment out
    public bool isAllyDead = false;
    public bool isPlayerDead = false;
    public bool isDebugStopped = true;

    // used for changing color back and forth for dying
    public Color colerOriginal;

    void Awake()
    {
        // used for changing color back and forth for dying
        colerOriginal = GetComponent<Renderer>().material.color;

        allySelfScript = GetComponent<Ally>();
        if (gameObject == GameObject.Find("AllyBlueBot"))
        {
            allyFriendScript = GameObject.Find("AllyOrangeBot").GetComponent<Ally>();
        }
        else if (gameObject == GameObject.Find("AllyOrangeBot"))
        {
            allyFriendScript = GameObject.Find("AllyBlueBot").GetComponent<Ally>();
        }
        playerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthManager>();
        // i'm not sure how we should do this for the player; since there are 2 allies (it works for the player since there is only 1), not sure how to solve this, maybe a list?
        // or maybe the solution should be to have a player revive script and an ally revive srcript, since they'll be working differently anyway?
    }

    void Update()
    {
        // used for changing color back and forth for dying
        if (isAllyDead == true && isDebugStopped == true)
        {
            Debug.Log(isAllyDead);
            isDebugStopped = false; // to prevent it from keep running

            // used for changing color back and forth for dying
            //GetComponent<Renderer>().material.color = Color.black;
            GetComponent<Renderer>().material.color = new Color (0,0,0);
        }

        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("stop coroutine");
            //StopCoroutine(Revive()); // can't get this one to work
            StopAllCoroutines();
        }
        */
    }
    

    void OnTriggerStay(Collider other)
    {
        // is used for when the player enters the trigger of an ally
        if (other.gameObject.tag == "Player")
        {
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
            //if (allySelfScript.isAllyDead == true)
            if (isAllyDead == true)
            {
                StartCoroutine(ReviveAlly());
            }
        }
        
        // is used for when an ally enters the trigger of the player
        /*
        if (other.gameObject.tag == "GoodGuys")
        {
            // reviving over time by staying inside the trigger
            //if (playerHealthScript.isPlayerDead == true)
            if (isPlayerDead == true)
            {
                //StartCoroutine(Revive());
            }
        }
        */
    } // OnTriggerStay

    void OnTriggerExit(Collider other)
    { 
        // is used for when the player exits the trigger of an ally
        if (other.gameObject.tag == "Player")
        {
            // resets revive if unsuccessfully revived
            reviveCurrent = 0;
            reviveBarFill.fillAmount = 0;
        }
        
        // is used for when an ally exists the trigger of the player
        /*
        if (other.gameObject.tag == "GoodGuys")
        {
            // resets revive if unsuccessfully revived
            reviveCurrent = 0;
            reviveBarFill.fillAmount = 0;
        }
        */
    }

    IEnumerator ReviveAlly()
    {
        while(reviveCurrent < reviveMax)
        {
            //Debug.Log("coroutine is running");

            reviveCurrent += reviveMax / reviveRate;

            if (reviveCurrent >= reviveMax)
            {
                //Debug.Log("ally is alive");

                //allySelfScript.isAllyDead = false;
                isAllyDead = false;

                // used for changing color back and forth for dying
                GetComponent<Renderer>().material.color = colerOriginal;
                
                //allySelfScript.currHealth = allySelfScript.maxHealth;

                // we also need something for player, but i don't know how we should split it up!
            }

            // fill revive bar here
            reviveBarFill.fillAmount = reviveCurrent / reviveMax;
            yield break; // makes the coroutine stop; when x is no longer inside the trigger, so we don't have to use StopAllCoroutines
        }
    }
}
