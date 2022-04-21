using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed to change scene

// used for buttons in menu end

public class MenuEnd : MonoBehaviour
{
    // url for survey goes here
    private string url = "https://duckduckgo.com/";

    public void GameOpenLink()
    {
        Application.OpenURL(url);
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
