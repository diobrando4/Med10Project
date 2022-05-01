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

    // make a function that auto loads the level loader?

    void Start()
    {
        gameVersionText = GameObject.Find("Canvas/TextGameVersion").GetComponent<TextMeshProUGUI>();
        gameVersionText.text = PlayerPrefs.GetInt("GameVersion").ToString();
    }

    // just for testing
    void Update()
    {
        // to reset it
        if(Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            gameVersionText.text = PlayerPrefs.GetInt("GameVersion").ToString();
        }
        // to test/active it
        if(Input.GetKeyDown(KeyCode.T))
        {
            RandomGameVersion();
            gameVersionText.text = PlayerPrefs.GetInt("GameVersion").ToString();
        }
        // to see/print the value of it
        if(Input.GetKeyDown(KeyCode.G))
        {
            //Debug.Log("GetInt: " + PlayerPrefs.GetInt("GameVersion")); // default value seems to be zero
            //gameVersionText.text = PlayerPrefs.GetInt("GameVersion").ToString();
        }
    }

    void RandomGameVersion()
    {
        // this is the only way i could think of to lock it
        // you can only change the version once because its default value is zero
        if (PlayerPrefs.GetInt("GameVersion") == 0)
        {
            int rnd = Random.Range(1, 3); // 1 or 2
            Debug.Log("rnd: " + rnd);
            
            if (rnd == 1)
            {
                PlayerPrefs.SetInt("GameVersion", 1);
                //gameVersionText.text = PlayerPrefs.GetInt("GameVersion").ToString();

                // have whatever variable is need to enable or disable companion dialogue
            }
            else if (rnd == 2)
            {
                PlayerPrefs.SetInt("GameVersion", 2);
                //gameVersionText.text = PlayerPrefs.GetInt("GameVersion").ToString();

                // have whatever variable is need to enable or disable companion dialogue
            }
        }
    }

    public void GameStart()
    {
        // RandomGameVersion();
        levelLoader.LoadNextLevel();
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
