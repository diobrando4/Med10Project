using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed to change scene
using TMPro; // needed for text

// used for buttons in the start menu

public class MenuStart : MonoBehaviour
{
    public LevelLoader levelLoader;
    public TextMeshProUGUI gameVersionText;
    private int versionNum;
    private int startingVersionNum;

    void Start()
    {
        if (PlayerPrefs.HasKey("GameVersion") == true) //If there already is a saved key
        {
            versionNum = PlayerPrefs.GetInt("GameVersion"); //Use that int as versionNum
            if(PlayerPrefs.HasKey("StartingGameVersion") == true)
            {
                startingVersionNum = PlayerPrefs.GetInt("StartingGameVersion");
            }
            else
            {
                startingVersionNum = versionNum;
            }
            Debug.Log("Existing save found with the number: "+versionNum+ " and "+startingVersionNum); 
        }
        else
        {
            //RandomGameVersion();
            versionNum = Random.Range(1,3);
            startingVersionNum = versionNum;
            PlayerPrefs.SetInt("GameVersion", versionNum); //Set the int versionNum to the SavedVersion Key
            PlayerPrefs.SetInt("StartingGameVersion", versionNum);
            PlayerPrefs.Save(); //Save the changes to registry
            //If used In-Editor - Registry can be found using "RegEdit" with the path being Computer/HKEY_CURRENT_USER/Software/Unity/UnityEditor/DefaultCompany/Med10P1/SavedVersion_xxxxxxxx
            Debug.Log("New version number is: "+versionNum);
        }
        
        gameVersionText = GameObject.Find("Canvas/TextGameVersion").GetComponent<TextMeshProUGUI>();
        gameVersionText.text = PlayerPrefs.GetInt("GameVersion").ToString();
        //gameVersionText.text = "";
    }

    void Update()
    {
        //DEBUG
        if(Input.GetKey(KeyCode.I))
        {
            if(Input.GetKeyDown(KeyCode.O)) // Delete Key
            {
                PlayerPrefs.DeleteAll();
                versionNum = 0;
                gameVersionText.text = PlayerPrefs.GetInt("GameVersion").ToString();
                Debug.Log("Deleting all Keys");
            }
            else if(Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)) //Reset and Change to version 1
            {   
                PlayerPrefs.SetInt("GameVersion", 1);
                PlayerPrefs.SetInt("StartingGameVersion", 1);
                versionNum = PlayerPrefs.GetInt("GameVersion");
                startingVersionNum = versionNum;
                PlayerPrefs.Save();
                Debug.Log("Making new version "+versionNum);
                gameVersionText.text = PlayerPrefs.GetInt("GameVersion").ToString();
            }
            else if(Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)) //Reset and Change to version 2
            {
                PlayerPrefs.SetInt("GameVersion", 2);
                PlayerPrefs.SetInt("StartingGameVersion", 2);
                versionNum = PlayerPrefs.GetInt("GameVersion");
                startingVersionNum = versionNum;
                PlayerPrefs.Save();
                Debug.Log("Making new version "+versionNum);
                gameVersionText.text = PlayerPrefs.GetInt("GameVersion").ToString();
            }
            else if(Input.GetKeyDown(KeyCode.L)) //TESTING print out game version number
            {
                Debug.Log("Version check: "+versionNum);
                Debug.Log("Starting Version check: "+startingVersionNum);
                gameVersionText.text = versionNum.ToString();
            }
        }
    }

    public void GameStart()
    {
        levelLoader.LoadNextLevel();
    }

    public void GameQuit()
    {
        Application.Quit();
    }    
}
