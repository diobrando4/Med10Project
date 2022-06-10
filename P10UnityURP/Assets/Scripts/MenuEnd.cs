using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed to change scene

// used for buttons in menu end

public class MenuEnd : MonoBehaviour
{
    // url for survey goes here
    private string url1 = "https://duckduckgo.com/1";
    private string url2 = "https://duckduckgo.com/2";

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

    public void GameNextVersion() // not sure if this causes any issues if the game crashes, because then the starting version will be different?
    {
        if (PlayerPrefs.GetInt("GameVersion") == 1)
        {
            PlayerPrefs.SetInt("GameVersion", 2); // should be reversed, if we want them to play the other version afterwards?
            SceneManager.LoadScene("Level00"); // this doesn't use transition, but it probably should?
        }
        if (PlayerPrefs.GetInt("GameVersion") == 2)
        {
            PlayerPrefs.SetInt("GameVersion", 1);
            SceneManager.LoadScene("Level00"); // this doesn't use transition, but it probably should?
        }
    }

    public void GameReturnToMenuStart()
    {
        SceneManager.LoadScene("MenuStart"); // this doesn't use transition, but it probably should?
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
