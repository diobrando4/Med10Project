using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Need this to know which scene we are in
using TMPro;

//This script takes care of all combat related dialogues that needs to pop up above the Ally's head
//It takes in a .txt file and filters out the relevant dialogue to each Ally, using a tag system of [0] and [1]

public class CombatDialogueManager : MonoBehaviour
{
    //[SerializeField]
    private GameObject ally1;
    private GameObject ally2;
    private GameObject player;

    private Ally ally1Health;
    private Ally ally2Health;
    private PlayerHealthManager playerHealth;
    //[SerializeField]
    private float ally1HPTracker;
    private float ally2HPTracker;
    private float playerHPTracker;

    public GameObject FloatingTextPrefab;
    public Vector3 offset = new Vector3 (0,3,0);    
    private GameObject ally1floatText;
    private GameObject ally2floatText;
    private Color ally1TextColor; //main material color for the text, determined by the GameObject material
    private Color ally2TextColor;

    private RectTransform ally1TextBackgroundRect;
    private RectTransform ally2TextBackgroundRect;
    private Vector2 ally1TextBackgroundSize;
    private Vector2 ally2TextBackgroundSize;

    public TextAsset textFileCombat; //For lines said during combat
    public TextAsset textFileIdle; //For lines said during Idling after End dialogue is finished
    [SerializeField]
    private List<string> ally1CombatLines = new List<string>();
    [SerializeField]
    private List<string> ally2CombatLines = new List<string>();

    public TextAsset[] textFileLevel;
    public TextAsset[] textFileStartLevel;
    [SerializeField]
    private List<string> ally1LevelLines = new List<string>();
    [SerializeField]
    private List<string> ally2LevelLines = new List<string>();
    private string ally1LevelStartLines;
    private string ally2LevelStartLines;

    private int numOfDialoguePer = 5;

    public bool toggleDialogue = true;

    private bool dialogueTrigger = true;
    private bool narrativeTrigger = false;
    private bool idleTrigger = false;
    private bool checkClear = false;
    private bool checkDebuff = false;
    private bool checkIfAlly1Downed = false;
    private bool checkIfAlly2Downed = false;
    private bool checkIfPlayerDowned = false;
    [HideInInspector]
    public bool isDialogueDone = false;

    //====TextLog Related====
    private List<string> lastDialogueSaid = new List<string>();
    public int narrativeDialogueSpeed = 3;
    public int maxNumberOfLines = 10;
    private TMP_Text textLog;
    //=======================

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
        // //Text file processing
        ally1CombatLines = ProcessTxtFile(textFileCombat,"[0]");
        ally2CombatLines = ProcessTxtFile(textFileCombat,"[1]");

        if (GameObject.Find("CanvasHealthBars/TextLogBg/Mask/TextLogText"))
        {
            textLog = GameObject.Find("CanvasHealthBars/TextLogBg/Mask/TextLogText").GetComponentInChildren<TMP_Text>();
        }        

        //What they say at the start of the level basically
        if(toggleDialogue == true)
        {
            GameObject.Find("CanvasHealthBars/TextLogBg").SetActive(true);
            for(int i = 1; i <= textFileStartLevel.Length; i++)
            {
                if(SceneManager.GetActiveScene().buildIndex == i)
                {
                    if(textFileStartLevel[i-1] != null)
                    {
                        ally1LevelStartLines = string.Join("",ProcessTxtFile(textFileStartLevel[i-1],"[0]"));
                        ally2LevelStartLines = string.Join("",ProcessTxtFile(textFileStartLevel[i-1],"[1]"));
                        ShowFloatingTextAlly1(ally1LevelStartLines);  
                        ShowFloatingTextAlly2(ally2LevelStartLines); 
                    }
                }
            }
        }
        else
        {
            GameObject.Find("CanvasHealthBars/TextLogBg").SetActive(false);
            isDialogueDone = true;
        }



    }//Start()

    // Update is called once per frame
    void Update()
    {
        if(toggleDialogue == true)
        {
            //If Ally 1 is hurt
            if(ally1HPTracker > ally1Health.currHealth && ally1Health.currHealth != 0)//initial check
            {
                dialogueTrigger = false; //reset trigger
                if (dialogueTrigger == false && checkIfAlly1Downed == false) //Trigger check to prevent spamming & talk when downed
                {
                    ShowFloatingTextAlly1(ResponseAlly1(1));
                    ally1HPTracker = ally1Health.currHealth;
                    dialogueTrigger = true; //Set trigger to true
                }
            }
            //If Ally 2 is hurt
            if(ally2HPTracker > ally2Health.currHealth && ally2Health.currHealth != 0)
            {
                dialogueTrigger = false;
                if (dialogueTrigger == false && checkIfAlly2Downed == false)
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
                    checkIfAlly1Downed = true;
                }
            }
            // else if (ally1Health.currHealth > 0)
            // {
            //     if (checkIfAlly1Downed == true)
            //     {
            //         checkIfAlly1Downed = false;
            //     }
            // }
            //If Ally 2 has 0 hp
            if(ally2Health.currHealth <= 0)
            {
                dialogueTrigger = false;
                if (dialogueTrigger == false && checkIfAlly2Downed == false)
                {
                    ShowFloatingTextAlly2(ResponseAlly2(2));  
                    ally2HPTracker = ally2Health.currHealth; 
                    dialogueTrigger = true;  
                    checkIfAlly2Downed = true;
                }           
            }
            // else if (ally2Health.currHealth > 0)
            // {
            //     if (checkIfAlly2Downed == true)
            //     {
            //         checkIfAlly2Downed = false;
            //     }
            // }

            //If Player is hurt
            if(playerHPTracker > playerHealth.playerCurrentHealth && playerHealth.playerCurrentHealth != 0)
            {
                dialogueTrigger = false;
                if (dialogueTrigger == false && checkIfPlayerDowned == false)
                {
                    if (checkIfAlly1Downed == false)
                    {
                        ShowFloatingTextAlly1(ResponseAlly1(3));
                    }
                    if (checkIfAlly2Downed == false)
                    {
                        ShowFloatingTextAlly2(ResponseAlly2(3));
                    }
                    playerHPTracker = playerHealth.playerCurrentHealth; 
                    dialogueTrigger = true;
                    //Debug.Log("Player HP: "+playerHPTracker+" | "+playerHealth.playerCurrentHealth);
                }
            }
            //If player has 0 hp
            if(playerHealth.playerCurrentHealth <= 0 && playerHealth.isBeingRevived == false)
            { 
                dialogueTrigger = false;
                if (dialogueTrigger == false && checkIfPlayerDowned == false)
                {
                    if (checkIfAlly1Downed == false)
                    {
                        ShowFloatingTextAlly1(ResponseAlly1(4));
                    }
                    if (checkIfAlly2Downed == false)
                    {
                        ShowFloatingTextAlly2(ResponseAlly2(4));
                    }
                    playerHPTracker = playerHealth.playerCurrentHealth; 
                    dialogueTrigger = true;
                    checkIfPlayerDowned = true;
                }           
            }
            // else if (playerHealth.playerCurrentHealth > 0)
            // {
            //     if (checkIfPlayerDowned == true)
            //     {
            //         checkIfPlayerDowned = false;
            //     }
            // }

            //If Ally1 is Revived by Player
            if(ally1Health.isRevived == true)
            {
                dialogueTrigger = false;
                if(dialogueTrigger == false && checkIfAlly1Downed == true)
                {
                    //Say something
                    ShowFloatingTextAlly1(ResponseAlly1(5));
                    ally1HPTracker = ally1Health.currHealth;
                    dialogueTrigger = true;
                    checkIfAlly1Downed = false;
                }
            }

            //If Ally2 is Revived by Player
            if(ally2Health.isRevived == true)
            {
                dialogueTrigger = false;
                if(dialogueTrigger == false && checkIfAlly2Downed == true)
                {
                    //Say something
                    ShowFloatingTextAlly2(ResponseAlly2(5));
                    ally2HPTracker = ally2Health.currHealth;
                    dialogueTrigger = true;
                    checkIfAlly2Downed = false;
                }
            }

            //If Ally1 is Reviving Player
            if(playerHealth.isBeingRevived == true && playerHealth.revivingAlly == ally1)
            {
                dialogueTrigger = false;
                if(dialogueTrigger == false && checkIfPlayerDowned == true)
                {
                    ShowFloatingTextAlly1(ResponseAlly1(6));
                    playerHPTracker = playerHealth.playerCurrentHealth; 
                    dialogueTrigger = true;
                    checkIfPlayerDowned = false;
                }
            }

            //If Ally2 is Reviving Player
            if(playerHealth.isBeingRevived == true && playerHealth.revivingAlly == ally2)
            {
                dialogueTrigger = false;
                if(dialogueTrigger == false && checkIfPlayerDowned == true)
                {
                    ShowFloatingTextAlly2(ResponseAlly2(6));
                    playerHPTracker = playerHealth.playerCurrentHealth; 
                    dialogueTrigger = true;
                    checkIfPlayerDowned = false;
                }
            }

            //If Ally1 is Removing Debuff on Player
            if(ally1Health.isUsingDispel == true)
            {
                dialogueTrigger = false;
                if(dialogueTrigger == false && checkIfAlly1Downed == false && checkDebuff == false)
                {
                    ShowFloatingTextAlly1(ResponseAlly1(7));
                    dialogueTrigger = true;
                    checkDebuff = true;
                }
            }
            else
            {
                checkDebuff = false;
            }

            //If Ally2 is Removing Debuff on Player
            if(ally2Health.isUsingDispel == true)
            {
                dialogueTrigger = false;
                if(dialogueTrigger == false && checkIfAlly1Downed == false && checkDebuff == false)
                {
                    ShowFloatingTextAlly2(ResponseAlly2(7));
                    dialogueTrigger = true;
                    checkDebuff = true;
                }
            }
            else
            {
                checkDebuff = false;
            }

            //If there are no more enemies on the floor, and it is not the first floor
            if(ally1Health.inCombat == false && GameObject.FindGameObjectWithTag("Enemy") == null && SceneManager.GetActiveScene().buildIndex > 1)
            {
                dialogueTrigger = false;
                if(dialogueTrigger == false && checkClear == false)
                {
                    if (checkIfAlly1Downed == false)
                    {
                        ShowFloatingTextAlly1(ResponseAlly1(8));
                    }
                    if (checkIfAlly2Downed == false)
                    {
                        ShowFloatingTextAlly2(ResponseAlly2(8));
                    }
                    dialogueTrigger = true;
                    checkClear = true;
                }
            }
            else
            {
                if(FindObjectOfType<ExitDoor>() != null)
                {
                    checkClear = false;
                }
            }

            //Post combat dialogue should trigger if not in combat, and all GoodGuys are alive
            if (ally1Health.inCombat == false && ally2Health.inCombat == false && checkIfAlly1Downed == false && checkIfAlly2Downed == false && checkIfPlayerDowned == false && GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                for (int i = 1; i <= textFileLevel.Length; i++)
                {
                    if (SceneManager.GetActiveScene().buildIndex == i && narrativeTrigger == false)
                    {
                        //Triggers only once due to narrativeTrigger
                        if (textFileLevel[i-1] != null)
                        {
                            ally1LevelLines = ProcessTxtFile(textFileLevel[i-1],"[0]","[1]");
                            ally2LevelLines = ProcessTxtFile(textFileLevel[i-1],"[1]","[0]");
                            StartCoroutine(ProgressDialogue(ally1LevelLines,ally2LevelLines,ally1LevelLines.Count,narrativeDialogueSpeed));
                            narrativeTrigger = true;
                        }
                    }
                }
                if(isDialogueDone == true && idleTrigger == false)
                {
                    ally1LevelLines = ProcessTxtFile(textFileIdle,"[0]","[1]");
                    ally2LevelLines = ProcessTxtFile(textFileIdle,"[1]","[0]");
                    StartCoroutine(IdleDialogue(ally1LevelLines,ally2LevelLines,ally1LevelLines.Count,5));
                    idleTrigger = true;
                }
            }

            DisplayTextLog();
            //Clear the oldest entry that is beyond the max number of lines    
            while (lastDialogueSaid.Count > maxNumberOfLines)
            {
                //lastDialogueSaid.RemoveAt(lastDialogueSaid.Count-1);
                lastDialogueSaid.RemoveAt(0);
            }
        }//toggleDialogue

        //FOR DEBUGGING
        // if (Input.GetKeyDown(KeyCode.Keypad0))
        // {
        //     //dialogueTrigger = false;
        //     ally1Health.DamageTaken(1);
        //     ally2Health.DamageTaken(1);
        //     ally1Health.UpdateHealthBar();
        //     ally2Health.UpdateHealthBar();
        //     //Debug.Log("Space Pressed");
        //     //playerHealth.HurtPlayer(1);
        // }
        // if (Input.GetKeyDown(KeyCode.Keypad1))
        // {
        //     playerHealth.HurtPlayer(1);
        // }
        //Debug.Log(ResponseAlly1(2));
        //Debug.Log(ResponseAlly2(1));
    }// Update

    string ResponseAlly1(int response) //Return a string with the response dialogue
    {
        switch(response)
        {
            case 1://Getting hit themselves
                //Debug.Log(ally1CombatLines[Random.Range(0,3)]); 0 1 2 not including 3
                dialogueTrigger = false;
                return ally1CombatLines[Random.Range(0,numOfDialoguePer)];

            case 2://Reaching 0 HP themselves
                //Debug.Log(ally1CombatLines[Random.Range(2,6)]);
                dialogueTrigger = false;
                return ally1CombatLines[Random.Range(numOfDialoguePer,numOfDialoguePer*2)];

            case 3://Player hit
                //Debug.Log(ally1CombatLines[Random.Range(6,9)]);
                dialogueTrigger = false;
                return ally1CombatLines[Random.Range(numOfDialoguePer*2,numOfDialoguePer*3)];

            case 4://Player downed
                //Debug.Log(ally1CombatLines[Random.Range(9,12)]);
                dialogueTrigger = false;
                return ally1CombatLines[Random.Range(numOfDialoguePer*3,numOfDialoguePer*4)];    

            case 5://Ally revived by Player
                dialogueTrigger = false;
                return ally1CombatLines[Random.Range(numOfDialoguePer*4,numOfDialoguePer*5)];

            case 6://Ally reviving player
                dialogueTrigger = false;
                return ally1CombatLines[Random.Range(numOfDialoguePer*5,numOfDialoguePer*6)];

            case 7://Ally removing debuff on player
                dialogueTrigger = false;
                return ally1CombatLines[Random.Range(numOfDialoguePer*6,numOfDialoguePer*7)];

            case 8://Ally reports no more enemies in level
                dialogueTrigger = false;
                return ally1CombatLines[Random.Range(numOfDialoguePer*7,numOfDialoguePer*8)];

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
                return ally2CombatLines[Random.Range(0,numOfDialoguePer)];

            case 2://Reaching 0 HP themselves
                //Debug.Log(ally2CombatLines[Random.Range(2,6)]);
                dialogueTrigger = false;
                return ally2CombatLines[Random.Range(numOfDialoguePer*1,numOfDialoguePer*2)];

            case 3://Player hit
                //Debug.Log(ally2CombatLines[Random.Range(6,9)]);
                dialogueTrigger = false;
                return ally2CombatLines[Random.Range(numOfDialoguePer*2,numOfDialoguePer*3)];

            case 4://Player downed
                //Debug.Log(ally2CombatLines[Random.Range(9,12)]);
                dialogueTrigger = false;
                return ally2CombatLines[Random.Range(numOfDialoguePer*3,numOfDialoguePer*4)];    
            
            case 5://Ally revived by Player
                dialogueTrigger = false;
                return ally2CombatLines[Random.Range(numOfDialoguePer*4,numOfDialoguePer*5)];

            case 6://Ally reviving player
                dialogueTrigger = false;
                return ally2CombatLines[Random.Range(numOfDialoguePer*5,numOfDialoguePer*6)];

            case 7://Ally removing debuff on player
                dialogueTrigger = false;
                return ally2CombatLines[Random.Range(numOfDialoguePer*6,numOfDialoguePer*7)];

            case 8://Ally reports no more enemies in level
                dialogueTrigger = false;
                return ally2CombatLines[Random.Range(numOfDialoguePer*7,numOfDialoguePer*8)];

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
        if (ally1 != null && text != "")
        {
            ally1floatText = (GameObject)Instantiate(FloatingTextPrefab, ally1.transform.position, Quaternion.identity, ally1.transform);
            ally1Health.PlaySound("Ally1Talk"+Random.Range(0,5));
            ally1floatText.GetComponentInChildren<TMP_Text>().color = ally1TextColor;
            ally1floatText.transform.localPosition += offset;
            ally1floatText.GetComponentInChildren<TMP_Text>().text = text;
            ally1floatText.GetComponentInChildren<TMP_Text>().ForceMeshUpdate();
            ally1TextBackgroundRect = ally1floatText.GetComponentInChildren<RectTransform>();
            ally1TextBackgroundSize = ally1floatText.GetComponentInChildren<TMP_Text>().GetRenderedValues(true);
            ally1TextBackgroundRect.sizeDelta = ally1TextBackgroundSize;
            //lastDialogueSaid.Insert(0,"<color=#"+ColorUtility.ToHtmlStringRGB(ally1TextColor)+">"+text+"</color>");
            lastDialogueSaid.Add("<color=#"+ColorUtility.ToHtmlStringRGB(ally1TextColor)+">"+text+"</color>");
        }
    }//ShowFloatingTextAlly1

        void ShowFloatingTextAlly2(string text)
    {
        if (ally2floatText != null)
        {
            Destroy(ally2floatText);
        }

        //ally2floatText = (GameObject)Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        if (ally2 != null && text != "")
        {
            ally2floatText = (GameObject)Instantiate(FloatingTextPrefab, ally2.transform.position, Quaternion.identity, ally2.transform);
            ally2Health.PlaySound("Ally2Talk"+Random.Range(0,5));
            ally2floatText.GetComponentInChildren<TMP_Text>().color = ally2TextColor;
            ally2floatText.transform.localPosition += offset;
            ally2floatText.GetComponentInChildren<TMP_Text>().text = text;
            ally2floatText.GetComponentInChildren<TMP_Text>().ForceMeshUpdate();
            ally2TextBackgroundRect = ally2floatText.GetComponentInChildren<RectTransform>();
            ally2TextBackgroundSize = ally2floatText.GetComponentInChildren<TMP_Text>().GetRenderedValues(true);
            ally2TextBackgroundRect.sizeDelta = ally2TextBackgroundSize;
            //lastDialogueSaid.Insert(0,"<color=#"+ColorUtility.ToHtmlStringRGB(ally2TextColor)+">"+text+"</color>");
            lastDialogueSaid.Add("<color=#"+ColorUtility.ToHtmlStringRGB(ally2TextColor)+">"+text+"</color>");
        }
    }//ShowFloatingTextAlly2
    
    //Takes the list of strings of latest dialogues, then turn it into a single string divded by \n for new lines
    private void DisplayTextLog()
    {
        textLog.text = string.Join("\n", lastDialogueSaid);
    }//DisplayTextLog()

    //Takes a txt file, then filter out lines with a tag, and add it to a list.
    private List<string> ProcessTxtFile(TextAsset _TxtFile, string _tag)
    {
        string[] textLines = null;
        List<string> outputLines = new List<string>();

        //Text file processing
        if(_TxtFile != null) //If there is a text file
        {
            textLines = (_TxtFile.text.Split('\n')); //Seperate per line
        }
        for (int i = 0; i < textLines.Length; i++) //Split textfile between ally1 and ally2 depending on marker [0] and [1]
        {
            if(textLines[i].Contains(_tag))
            {
                textLines[i] = textLines[i].Replace(_tag, "");                
                outputLines.Add(textLines[i]);
                    
            }
        }
        return outputLines; 
    }//ProcessTxtFile

    private List<string> ProcessTxtFile(TextAsset _TxtFile, string _tag, string _otherTag)
    {
        string[] textLines = null;
        List<string> outputLines = new List<string>();

        //Text file processing
        if(_TxtFile != null) //If there is a text file
        {
            textLines = (_TxtFile.text.Split('\n')); //Seperate per line
        }
        for (int i = 0; i < textLines.Length; i++) //Split textfile between ally1 and ally2 depending on marker [0] and [1]
        {
            if(textLines[i].Contains(_tag))
            {
                textLines[i] = textLines[i].Replace(_tag, "");                
                outputLines.Add(textLines[i]);
                    
            }
            else if (textLines[i].Contains(_otherTag))
            {
                outputLines.Add("");
            }
        }
        return outputLines;
    }

    IEnumerator ProgressDialogue(List<string> _lines1, List<string> _lines2, int _numOfLines , int _dialogueSpeed)
    {
        int lineTracker = 0;
        yield return new WaitForSeconds(2);
        //for (int i = 0; i < _numOfLines; i++)
        while(lineTracker < _numOfLines)
        {
            ShowFloatingTextAlly1(_lines1[lineTracker]);
            ShowFloatingTextAlly2(_lines2[lineTracker]);
            lineTracker += 1;
            yield return new WaitForSeconds(_dialogueSpeed);
        }
        isDialogueDone = true;
        yield break;
    }//ProgressDialogue

    IEnumerator IdleDialogue(List<string> _lines1, List<string> _lines2, int _numOfLines , int _dialogueSpeed)
    {
        int randomInt = 0;
        yield return new WaitForSeconds(8);
        while (isDialogueDone == true && idleTrigger == true)
        {
            randomInt = Random.Range(0,_lines1.Count);
            ShowFloatingTextAlly1(_lines1[randomInt]);
            ShowFloatingTextAlly2(_lines2[randomInt]);
            yield return new WaitForSeconds(_dialogueSpeed);
            idleTrigger = false;
        }
        yield break;
    }
}