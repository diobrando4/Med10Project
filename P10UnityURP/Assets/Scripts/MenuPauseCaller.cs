using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseCaller : MonoBehaviour
{
    //[HideInInspector]
    public bool isPaused = false;
    private int someInt;

    // Update is called once per frame
    void Start()
    {
        PlayerPrefs.SetInt("Pause", 0);
    }

    void Update()
    {
        // we need to find a better way to update this, so it doesn't run 24/7! but i didn't manage to pull it off (see further down)
        someInt = PlayerPrefs.GetInt("Pause");

        if (someInt == 1)
        {
            isPaused = true;

            // this just makes it so the pause var in the player controller is the same as in the menu pause caller
            if (GameObject.Find("Player")) 
            {
                GameObject.Find("Player").GetComponent<PlayerController>().isPaused = isPaused;
            }
        }
        if (someInt == 0)
        {
            isPaused = false;

            // this just makes it so the pause var in the player controller is the same as in the menu pause caller
            if (GameObject.Find("Player")) 
            {
                GameObject.Find("Player").GetComponent<PlayerController>().isPaused = isPaused;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(isPaused)
            {
                GameRunning();
            }
            else
            {
                GamePaused();
            }
        }

        /*
        // this was an attempt to make the code more efficient, but i couldn't get it to work properly

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
        //Debug.Log("Running");
        //someInt = 1; // this was used in the attempt to make the code more efficient
        isPaused = false;
        if (GameObject.Find("Player")) 
        {
            GameObject.Find("Player").GetComponent<PlayerController>().isPaused = isPaused;
        }
        PlayerPrefs.SetInt("Pause", 0);
        Time.timeScale = 1f; // is running
        SceneManager.UnloadSceneAsync("MenuPause");
    }

    public void GamePaused()
    {
        //Debug.Log("Paused");
        //someInt = 0; // this was used in the attempt to make the code more efficient
        isPaused = true;
        if (GameObject.Find("Player")) 
        {
            GameObject.Find("Player").GetComponent<PlayerController>().isPaused = isPaused;
        }
        PlayerPrefs.SetInt("Pause", 1);
        Time.timeScale = 0f; // is paused
        SceneManager.LoadScene("MenuPause", LoadSceneMode.Additive);
    }
}
