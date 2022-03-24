using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed to change scene

// used for buttons in the start menu

public class MenuStart : MonoBehaviour
{
    public LevelLoader levelLoader;

    // make a function that auto loads the level loader?

    public void GameStart()
    {
        levelLoader.LoadNextLevel();
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
