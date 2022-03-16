using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script takes care of all combat related dialogues that needs to pop up above the Ally's head
//It takes in a .txt file and filters out the relevant dialogue to each Ally, using a tag system of [0] and [1]

public class CombatDialogueManager : MonoBehaviour
{
    private GameObject ally1;
    private GameObject ally2;
    //[SerializeField]
    private GameObject player;

    private Ally ally1Health;
    private Ally ally2Health;
    private PlayerHealthManager playerHealth;

    private float ally1HPTracker;
    private float ally2HPTracker;
    private float playerHPTracker;

    public GameObject FloatingTextPrefab;
    public Vector3 offset = new Vector3 (0,3,0);    
    private GameObject ally1floatText;
    private GameObject ally2floatText;
    private Color ally1TextColor; //main material color for the text, determined by the GameObject material
    private Color ally2TextColor;

    public TextAsset textFileCombat; //For lines said during combat
    private string[] textLinesCombat;
    //[SerializeField]
    private List<string> ally1CombatLines = new List<string>();
    //[SerializeField]
    private List<string> ally2CombatLines = new List<string>();

    //public int endAtLine;

    private bool dialogueTrigger = false;
    private bool checkIfAlly1Downed = false;
    private bool checkIfAlly2Downed = false;
    private bool checkIfPlayerDowned = false;

    // Start is called before the first frame update
    void Start()
    {
        ally1 = GameObject.Find("AllyBlueBot");
        ally2 = GameObject.Find("AllyOrangeBot");
        //player = GameObject.Find("ThePlayer"); 
        player = GameObject.FindGameObjectWithTag("Player");

        if (ally1 != null)
        {
            ally1Health = ally1.GetComponent<Ally>();
            ally1HPTracker = ally1Health.maxHealth;
            ally1TextColor = ally1.GetComponent<Renderer>().material.color;
        }
        if (ally2 != null)
        {
            ally2Health = ally2.GetComponent<Ally>();
            ally2HPTracker = ally2Health.maxHealth;           
            ally2TextColor = ally2.GetComponent<Renderer>().material.color;
        }
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealthManager>();
            playerHPTracker = playerHealth.playerMaxHealth;
        }

        if(textFileCombat != null) //If there is a text file
        {
            textLinesCombat = (textFileCombat.text.Split('\n')); //Seperate per line
        }
        for (int i = 0; i < textLinesCombat.Length; i++) //Split textfile between ally1 and ally2 depending on marker [0] and [1]
        {
            if(textLinesCombat[i].Contains("[0]"))
            {
                textLinesCombat[i] = textLinesCombat[i].Replace("[0]", "");                
                ally1CombatLines.Add(textLinesCombat[i]);
                //ally2CombatLines.Add(" ");
            }
            else if (textLinesCombat[i].Contains("[1]"))
            {
                textLinesCombat[i] = textLinesCombat[i].Replace("[1]", "");
                //ally1CombatLines.Add(" ");
                ally2CombatLines.Add(textLinesCombat[i]);  
            }
        }
        // if (endAtLine == 0)
        // {
        //     endAtLine = textLinesCombat.Length - 1;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        //If Ally 1 is hurt
        if(ally1HPTracker > ally1Health.currHealth && ally1Health.currHealth != 0)
        {
            dialogueTrigger = false;
            if (dialogueTrigger == false)
            {
                ShowFloatingTextAlly1(ResponseAlly1(1));
                ally1HPTracker = ally1Health.currHealth;
                dialogueTrigger = true;
            }
        }
        //If Ally 2 is hurt
        if(ally2HPTracker > ally2Health.currHealth && ally2Health.currHealth != 0)
        {
            dialogueTrigger = false;
            if (dialogueTrigger == false)
            {
                ShowFloatingTextAlly2(ResponseAlly2(1));  
                ally2HPTracker = ally2Health.currHealth; 
                dialogueTrigger = true;  
            }           
        }
        //If Ally 1 has 0 hp
        if(ally1Health.currHealth <= 0)
        {
            dialogueTrigger = false;
            if (dialogueTrigger == false && checkIfAlly1Downed == false)
            {
                ShowFloatingTextAlly1(ResponseAlly1(2));
                ally1HPTracker = ally1Health.currHealth;
                dialogueTrigger = true;
            }
        }
        else if (ally1Health.currHealth > 0)
        {
            if (checkIfAlly1Downed == true)
            {
                checkIfAlly1Downed = false;
            }
        }
        //If Ally 2 has 0 hp
        if(ally2Health.currHealth <= 0)
        {
            dialogueTrigger = false;
            if (dialogueTrigger == false && checkIfAlly2Downed == false)
            {
                ShowFloatingTextAlly2(ResponseAlly2(2));  
                ally2HPTracker = ally2Health.currHealth; 
                dialogueTrigger = true;  
            }           
        }
        else if (ally2Health.currHealth > 0)
        {
            if (checkIfAlly2Downed == true)
            {
                checkIfAlly2Downed = false;
            }
        }
        //If Player is hurt
        if(playerHPTracker > playerHealth.playerCurrentHealth && playerHealth.playerCurrentHealth != 0)
        {
            dialogueTrigger = false;
            if (dialogueTrigger == false)
            {
                ShowFloatingTextAlly1(ResponseAlly1(3));
                ShowFloatingTextAlly2(ResponseAlly2(3));  
                playerHPTracker = playerHealth.playerCurrentHealth; 
                dialogueTrigger = true;
                //Debug.Log("Player HP: "+playerHPTracker+" | "+playerHealth.playerCurrentHealth);
            }
        }
        //If player has 0 hp
        if(playerHealth.playerCurrentHealth <= 0)
        { 
            dialogueTrigger = false;
            if (dialogueTrigger == false && checkIfPlayerDowned == false)
            {
                ShowFloatingTextAlly1(ResponseAlly1(4));
                ShowFloatingTextAlly2(ResponseAlly2(4));  
                playerHPTracker = playerHealth.playerCurrentHealth; 
                dialogueTrigger = true;
                checkIfPlayerDowned = true;
            }           
        }
        else if (playerHealth.playerCurrentHealth > 0)
        {
            if (checkIfPlayerDowned == true)
            {
                checkIfPlayerDowned = false;
            }
        }
        //FOR DEBUGGING
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     dialogueTrigger = false;
        //     ally1Health.DamageTaken(1);
        //     ally2Health.DamageTaken(1);
        //     //ally1Health.UpdateHealthBar();
        //     //Debug.Log("Space Pressed");
        //     playerHealth.HurtPlayer(1);
        // }
    }// Update

    string ResponseAlly1(int response) //Return a string with the response dialogue
    {
        switch(response)
        {
            case 1://Getting hit themselves
                //Debug.Log(ally1CombatLines[Random.Range(0,3)]);
                dialogueTrigger = false;
                return ally1CombatLines[Random.Range(0,3)];

            case 2://Reaching 0 HP themselves
                //Debug.Log(ally1CombatLines[Random.Range(2,6)]);
                dialogueTrigger = false;
                return ally1CombatLines[Random.Range(2,6)];

            case 3://Player hit
                //Debug.Log(ally1CombatLines[Random.Range(6,9)]);
                dialogueTrigger = false;
                return ally1CombatLines[Random.Range(6,9)];

            case 4://Player downed
                //Debug.Log(ally1CombatLines[Random.Range(9,12)]);
                dialogueTrigger = false;
                return ally1CombatLines[Random.Range(9,12)];    

            default:
                return "Wrong value for Response";                             
        }
    }//ResponseAlly1

        string ResponseAlly2(int response) //Return a string with the response dialogue
    {
        switch(response)
        {
            case 1://Getting hit themselves
                //Debug.Log(ally2CombatLines[Random.Range(0,3)]);
                dialogueTrigger = false;
                return ally2CombatLines[Random.Range(0,3)];

            case 2://Reaching 0 HP themselves
                //Debug.Log(ally2CombatLines[Random.Range(2,6)]);
                dialogueTrigger = false;
                return ally2CombatLines[Random.Range(2,6)];

            case 3://Player hit
                //Debug.Log(ally2CombatLines[Random.Range(6,9)]);
                dialogueTrigger = false;
                return ally2CombatLines[Random.Range(6,9)];

            case 4://Player downed
                //Debug.Log(ally2CombatLines[Random.Range(9,12)]);
                dialogueTrigger = false;
                return ally2CombatLines[Random.Range(9,12)];    

            default:
                return "Wrong value for Response";                             
        }
    }//ResponseAlly2

    void ShowFloatingTextAlly1(string text)
    {
        if (ally1floatText != null)
        {
            Destroy(ally1floatText);
        }

        //ally1floatText = (GameObject)Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        if (ally1 != null)
        {
            ally1floatText = (GameObject)Instantiate(FloatingTextPrefab, ally1.transform.position, Quaternion.identity, ally1.transform);
            ally1floatText.GetComponent<TMP_Text>().color = ally1TextColor;
            ally1floatText.transform.localPosition += offset;
            ally1floatText.GetComponent<TMP_Text>().text = text;
        }
    }//ShowFloatingTextAlly1

        void ShowFloatingTextAlly2(string text)
    {
        if (ally2floatText != null)
        {
            Destroy(ally2floatText);
        }

        //ally1floatText = (GameObject)Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        if (ally2 != null)
        {
            ally2floatText = (GameObject)Instantiate(FloatingTextPrefab, ally2.transform.position, Quaternion.identity, ally2.transform);
            ally2floatText.GetComponent<TMP_Text>().color = ally2TextColor;
            ally2floatText.transform.localPosition += offset;
            ally2floatText.GetComponent<TMP_Text>().text = text;
        }
    }//ShowFloatingTextAlly1
}
