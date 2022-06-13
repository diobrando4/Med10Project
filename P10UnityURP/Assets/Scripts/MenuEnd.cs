using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // needed to change scene

// used for buttons in menu end

public class MenuEnd : MonoBehaviour
{
    // url for survey goes here
    private string url1 = "https://forms.gle/GRXwE3utGC8ZbUU6A"; // questionnaire 1-2
    private string url2 = "https://forms.gle/Ty3SMUS5mq4rGNTH9"; // questionnaire 2-1

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

    public void GameNextVersion() // not sure if this causes any issues if the game crashes, because then the starting version will be different? Viet: Optimally, PlayerPrefs should prevent that
    {
        if (PlayerPrefs.GetInt("GameVersion") == 1)
        {
            
            PlayerPrefs.SetInt("GameVersion", 2); // should be reversed, if we want them to play the other version afterwards?
            PlayerPrefs.Save();
            Debug.Log("Changing from "+PlayerPrefs.GetInt("StartingGameVersion")+ " to "+PlayerPrefs.GetInt("GameVersion"));
            SceneManager.LoadScene("Level00"); // this doesn't use transition, but it probably should? Viet: Probably should yes
        }
        else if (PlayerPrefs.GetInt("GameVersion") == 2)
        {
            PlayerPrefs.SetInt("GameVersion", 1);
            PlayerPrefs.Save();
            Debug.Log("Changing from "+PlayerPrefs.GetInt("StartingGameVersion")+ " to "+PlayerPrefs.GetInt("GameVersion"));
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
