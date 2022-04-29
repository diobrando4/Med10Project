using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed to change scene

// used for buttons in the start menu

public class MenuStart : MonoBehaviour
{
    public LevelLoader levelLoader;

    // make a function that auto loads the level loader?

    // just for testing
    void Update()
    {
        // to active it
        if(Input.GetKeyDown(KeyCode.T))
        {
            RandomGameVersion();
        }
        // to see the value of it
        if(Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("GetInt: " + PlayerPrefs.GetInt("GameVersion")); // default value seems to be zero
        }
        // to reset it
        if(Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    void RandomGameVersion()
    {
        // this is the only way i could thick of to lock it
        if (PlayerPrefs.GetInt("GameVersion") == 0)
        {
            int rnd = Random.Range(0, 2); // 0-1
            Debug.Log("rnd: " + rnd);

            if (rnd == 0)
            {
                PlayerPrefs.SetInt("GameVersion", 11);
                // have game version 1 here
            }
            else if (rnd == 1)
            {
                PlayerPrefs.SetInt("GameVersion", 22);
                // have game version 2 here
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
