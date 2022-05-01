using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed to change scene

// used for buttons in menu end

public class MenuEnd : MonoBehaviour
{
    // url for survey goes here
    private string url1 = "https://duckduckgo.com/?q=1";
    private string url2 = "https://duckduckgo.com/?q=2";

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
