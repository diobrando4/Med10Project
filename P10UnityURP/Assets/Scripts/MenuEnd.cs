using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed to change scene

// used for buttons in menu end

public class MenuEnd : MonoBehaviour
{
    public void GameReturnToMenuStart()
    {
        SceneManager.LoadScene("MenuStart");
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
