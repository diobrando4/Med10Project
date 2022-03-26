using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a manager meant to apply different conditions to the player character and/or make enemies stronger
//Should be able to have multiple functions that negatively affects the player
//Should be able to announce the effect also

public class DebuffManager : MonoBehaviour
{
    //Player related variables
    private GameObject player;
    private PlayerController playerCont;
    private PlayerHealthManager playerHPMan;
    private float playerDefaultSpeed;
    private float playerDefaultHealth;
    //Ally related variables
    private Ally ally1;
    private Ally ally2;
    //Debuff related variables
    private float slowedSpeed = 3;
    private float reverseSpeed = -5;
    private float reducedHealth = 3;
    //State related variables
    public int selectorNum = 0;
    //public bool isActive = false;
    public string debuffText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCont = player.GetComponent<PlayerController>();
        playerHPMan = player.GetComponent<PlayerHealthManager>();

        playerDefaultSpeed = playerCont.moveSpeed;
        playerDefaultHealth = playerHPMan.playerMaxHealth;
    
        ally1 = GameObject.Find("AllyBlueBot").GetComponent<Ally>();
        ally2 = GameObject.Find("AllyOrangeBot").GetComponent<Ally>();
    }

    // Update is called once per frame
    void Update()
    {
        //DebuffSelector(selectorNum);
    }

    public void DebuffSelector (int number)
    {
        switch (number)
        {
            case 0: //No Debuff
                debuffText = "All Clear!";
                break;

            case 1: //Slow player
                ChangePlayerSpeed(slowedSpeed);
                debuffText = "Slowed!";
                break; 

            case 2: //Reversed Speed 
                ChangePlayerSpeed(reverseSpeed);
                debuffText = "Reverse!";
                break;   

            case 3: //Reduced MaxHP
                ChangePlayerHealth(reducedHealth);
                debuffText = "Reduced Health!";
                break;

            default:
                Debug.Log("Not a valid number");
                debuffText = "ERROR!";
                break;
        }
    }

    //Change player speed, return to default if deactivated
    public void ChangePlayerSpeed(float newSpeed)
    {
        if (playerHPMan.isDebuffed == true)
        {
            playerCont.moveSpeed = newSpeed;
        }
    }

    public void RestorePlayerSpeed()
    {
        playerCont.moveSpeed = playerDefaultSpeed;
    }

    //Change player health to a new MaxHP value and update current health to match, return to default MaxHP values after
    //Not sure what to do if the player has already lost health before reaching this debuff
    //Not sure what to do if the player has lost health and the debuff goes away
    public void ChangePlayerHealth(float newHealth)
    {
        if (playerHPMan.isDebuffed == true)
        {
            playerHPMan.playerMaxHealth = newHealth;
            playerHPMan.playerCurrentHealth = playerHPMan.playerMaxHealth;
        }      
    }
    public void RestorePlayerHealth()
    {
        playerHPMan.playerMaxHealth = playerDefaultHealth;
        playerHPMan.playerCurrentHealth = playerHPMan.playerMaxHealth; //For now, restores curr HP to max HP
    }
}
