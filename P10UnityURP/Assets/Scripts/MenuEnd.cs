using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed to change scene

// used for buttons in menu end

public class MenuEnd : MonoBehaviour
{
    // url for survey goes here
    private string url1 = "https://docs.google.com/forms/d/1sg3qfpAZs9MB9u8DRa6k6nAK0yrMoPyU7uvkPZ5xlNc/edit?usp=sharing";
    private string url2 = "https://docs.google.com/forms/d/1pUr7Y_3uczX7rx7pm9toexuj2hCZxE-6lGXXrg_tOpo/edit?usp=sharing";

    public void GameOpenLink()
    {
        if (PlayerPrefs.GetInt("GameVersion") == 1)
        {
            Application.OpenURL(url1);
        }
        if (PlayerPrefs.GetInt("GameVersion") == 2)
        {
            Application.OpenURL(url2);
        }
    }

    public void GameReturnToMenuStart()
    {
        SceneManager.LoadScene("MenuStart");
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
