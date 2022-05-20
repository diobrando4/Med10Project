using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed to change scene
using TMPro; // needed for text

// used for buttons in the start menu

public class MenuStartForExam : MonoBehaviour
{
    public LevelLoader levelLoader;
    //public TextMeshProUGUI gameVersionText;

    void Start()
    {
        // this is basically a reset
        PlayerPrefs.SetInt("GameVersion", 0);
    }

    // dialogue
    public void GameStartV1()
    {
        PlayerPrefs.SetInt("GameVersion", 1);
        levelLoader.LoadNextLevel();
    }

    // non-dialogue
    public void GameStartV2()
    {
        PlayerPrefs.SetInt("GameVersion", 2);
        levelLoader.LoadNextLevel();
    }

    public void GameQuit()
    {
        Application.Quit();
    }    
}
