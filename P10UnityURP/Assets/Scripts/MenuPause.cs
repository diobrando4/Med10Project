using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.GetInt("Pause");
    }

    public void GameResume()
    {
        Debug.Log("Running");
        Time.timeScale = 1f; // not sure if this is needed
        PlayerPrefs.SetInt("Pause", 0);
        SceneManager.UnloadSceneAsync("MenuPause");
    }

    public void GameReturnToMenuStart()
    {
        Time.timeScale = 1f; // not sure if this is needed
        PlayerPrefs.SetInt("Pause", 0);
        SceneManager.LoadScene("MenuStart");
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
