using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public float playerMaxHealth = 3; // should probably rename this into max health
    public float playerCurrentHealth;
    public Image healthBarFill;

    public bool isPlayerDead = false;

    // can be used for testing and maybe for invincibility frames
    public bool isPlayerKillable = true;

    //Revive Related
    private float reviveMax = 100;
    private float reviveCurrent = 0; // has to be reset each time reviving is aborted
    private float reviveRate = 100;
    public Image reviveBarFill; //Need to automate this later
    [SerializeField]
    private GameObject revivingAlly; //Need to make sure that the revive bar does not reset just because the other ally is passing through

    // Start is called before the first frame update
    void Start()
    {
        // starting with full health
        playerCurrentHealth = playerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCurrentHealth <= 0)
        {
            //Debug.Log("YOU DIED");
            //gameObject.SetActive(false);
            
            isPlayerDead = true;
            // player movement is disabled in the player controller!
        }
    }

    [SerializeField]
    //private Transform _canvasTransform;

    void LateUpdate()
    {
        //_canvasTransform.LookAt(Camera.main.transform);

        // this is the one we're currently using!
        //_canvasTransform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void HurtPlayer(float damageTaken)
    {
        if (isPlayerKillable == true)
        {
            playerCurrentHealth -= damageTaken;
            healthBarFill.fillAmount = playerCurrentHealth / playerMaxHealth;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "GoodGuys")
        {
            if(isAllyDead == true)
            {
                revivingAlly = col.gameObject;
                StartCoroutine(RevivePlayer());
            }
        }
    }//OntriggerStay

    private void OnTriggerExit(Collider col)
    {
        //Reset revive bar to 0 if player exits the radius
        if(col.gameObject == revivingAlly)
        {
            reviveCurrent = 0;
            reviveBarFill.fillAmount = 0;
            revivingAlly = null;
        }
    }

    IEnumerator RevivePlayer()
    {
        while(reviveCurrent < reviveMax)
        {
            reviveCurrent += reviveMax / reviveRate;
            if (reviveCurrent >= reviveMax)
            {
                isPlayerDead = false;
                playerCurrentHealth = playerMaxHealth;
                revivingAlly = null;
            }
            // fill revive bar here
            reviveBarFill.fillAmount = reviveCurrent / reviveMax;
            yield break; // makes the coroutine stop; when x is no longer inside the trigger, so we don't have to use StopAllCoroutines
        }
    }
}
