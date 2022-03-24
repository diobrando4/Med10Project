using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public static bool gameIsPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // pause / unpause
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (gameIsPaused == true) 
            {
                Time.timeScale = 1f;
                gameIsPaused = false;
            }
            else
            {
                Time.timeScale = 0f;
                gameIsPaused = true;
            }
        }
        // restart current scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        // quit game
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            Application.Quit();
        }
    }
}
