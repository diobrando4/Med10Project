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
    private PlayerGunController playerGunCont;
    private float playerDefaultSpeed;
    private float playerDefaultHealth;
    private float playerDefaultFireRate;
    //Ally related variables
    private Ally ally1;
    private Ally ally2;
    //Debuff related variables
    private float slowedSpeed;
    private float reverseSpeed;
    private float reducedHealth; //Redacted Debuff
    private float reducedFireRate; //Currently Unused
    //State related variables
    public int selectorNum = 0;
    //public bool isActive = false;
    public string debuffText;

    public Material debuffSlowIcon;
    public Material debuffReverseIcon;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if(player != null)
        {
            playerCont = player.GetComponent<PlayerController>();
            playerHPMan = player.GetComponent<PlayerHealthManager>();
            playerGunCont = player.transform.Find("GunCube").GetComponent<PlayerGunController>();
        
            playerDefaultSpeed = playerCont.moveSpeed;
            playerDefaultHealth = playerHPMan.playerMaxHealth;
            playerDefaultFireRate = playerGunCont.timeBetweenShots;
            slowedSpeed = playerDefaultSpeed / 2;
            reverseSpeed = playerDefaultSpeed * -1;
            reducedFireRate = playerDefaultFireRate / 2;        
        }
        else
        {
            Debug.Log("No Player Found");
        }

        if (ally1 == null)
        ally1 = GameObject.Find("AllyBlueBot").GetComponent<Ally>();
        if (ally2 == null)
        ally2 = GameObject.Find("AllyOrangeBot").GetComponent<Ally>();
    }

    // Update is called once per frame
    void Update()
    {
        //   if (Input.GetKeyDown(KeyCode.Keypad9))
        //   DebuffSelector(selectorNum);
    }

    public void DebuffSelector (int number)
    {
        switch (number)
        {
            case 0: //No Debuff
                debuffText = "";
                break;

            case 1: //Slow player
                ChangePlayerSpeed(slowedSpeed);
                playerHPMan.debuffRenderer.material = debuffSlowIcon;
                debuffText = "Slowed!";
                break; 

            case 2: //Reversed Speed 
                ChangePlayerSpeed(reverseSpeed);
                playerHPMan.debuffRenderer.material = debuffReverseIcon;
                debuffText = "Reverse!";
                break;   

            case 3: //Reduced MaxHP
                ChangePlayerHealth(reducedHealth);
                debuffText = "Reduced Health!";
                break;

            case 4: //Reduce Fire Rate
                ChangePlayerFireRate(reducedFireRate);
                debuffText = "Reduced Fire Rate!";
                break;

            default:
                Debug.Log("Not a valid int");
                debuffText = "ERROR! no seriously";
                break;
        }
    }

    //Change player speed, return to default if deactivated
    public void ChangePlayerSpeed(float newSpeed)
    {
        if(playerHPMan.isDebuffable == true)
        {
            playerHPMan.isDebuffed = true;
            playerCont.moveSpeed = newSpeed;
            
            playerHPMan.debuffRenderer.enabled = true;
            if(FindObjectOfType<SoundManager>())
            {
                FindObjectOfType<SoundManager>().SoundPlay("PlayerDebuffed");
            }
        }
    }
    //Revert player speed to default
    public void RestorePlayerSpeed()
    {
        playerHPMan.isDebuffed = false;
        playerCont.moveSpeed = playerDefaultSpeed;
        playerHPMan.DispelEffect();
        debuffText = "";
    }

    //Change player health to a new MaxHP value and update current health to match, return to default MaxHP values after
    //Not sure what to do if the player has already lost health before reaching this debuff
    //Not sure what to do if the player has lost health and the debuff goes away
    public void ChangePlayerHealth(float newHealth)
    {
        if(playerHPMan.isDebuffable == true)
        {
            playerHPMan.isDebuffed = true;
            playerHPMan.playerMaxHealth = newHealth;
            playerHPMan.playerCurrentHealth = playerHPMan.playerMaxHealth;  
            
            playerHPMan.debuffRenderer.enabled = true;
            if(FindObjectOfType<SoundManager>())
            {
                FindObjectOfType<SoundManager>().SoundPlay("PlayerDebuffed");
            } 
        }
    }

    //Revert player MaxHP to default
    public void RestorePlayerHealth()
    {
        playerHPMan.isDebuffed = false;
        playerHPMan.playerMaxHealth = playerDefaultHealth;
        playerHPMan.playerCurrentHealth = playerHPMan.playerMaxHealth; //For now, restores curr HP to max HP
        debuffText = "";
    }

    //Change Player FireRate
    public void ChangePlayerFireRate(float newFireRate)
    {
        if (playerHPMan.isDebuffable == true)
        {
            playerHPMan.isDebuffed = true;
            playerGunCont.timeBetweenShots = newFireRate;
            if(FindObjectOfType<SoundManager>())
            {
                FindObjectOfType<SoundManager>().SoundPlay("PlayerDebuffed");
            } 
        }
    }

    //Restore Player FireRate
    public void RestorePlayerFireRate()
    {
        playerHPMan.isDebuffed = false;
        playerGunCont.timeBetweenShots = playerDefaultFireRate;
        debuffText = "";
    }
}
