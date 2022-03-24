using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseCaller : MonoBehaviour
{
    bool someBool = false;
    int someInt;

    // Update is called once per frame
    void Update()
    {
        // we need to find a better way to update this, so it doesn't run 24/7! but i didn't manage to pull it off (see further down)
        someInt = PlayerPrefs.GetInt("Pause");

        if (someInt == 1)
        {
            someBool = true;
        }
        if (someInt == 0)
        {
            someBool = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(someBool)
            {
                GameRunning();
            }
            else
            {
                GamePaused();
            }
        }

        /*
        // this was an attempt to make the code more efficient, but i couldn't get it to work

        //someInt = PlayerPrefs.GetInt("Pause");

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            //someInt = PlayerPrefs.GetInt("Pause");

            if (someInt == 0)
            {
                GameRunning();
            }
            else if (someInt == 1)
            {
                GamePaused();
            }
        }
        */
    }

    public void GameRunning()
    {
        Debug.Log("Running");
        //someInt = 1; // this was used in the attempt to make the code more efficient
        someBool = false;
        PlayerPrefs.SetInt("Pause", 0);
        Time.timeScale = 1f; // is running
        SceneManager.UnloadSceneAsync("MenuPause");
    }

    public void GamePaused()
    {
        Debug.Log("Paused");
        //someInt = 0; // this was used in the attempt to make the code more efficient
        someBool = true;
        PlayerPrefs.SetInt("Pause", 1);
        Time.timeScale = 0f; // is paused
        SceneManager.LoadScene("MenuPause", LoadSceneMode.Additive);
    }
}
