using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    //Assigned in MenuStart.cs
    public int currentVerNum; 
    public int oldVerNum;

    public void LoadNextLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //LoadLevel(SceneManager.GetActiveScene().buildIndex + 1)
        if (SceneManager.GetActiveScene().buildIndex == 8) //If we are at the boss room scene
        {
            if (PlayerPrefs.GetInt("GameVersion") != oldVerNum) //and if the current GameVersion is not the same as the one that was originally assigned
            {
                StartCoroutine(LoadLevel(10)); //Load EndFinal
            }
        }
        else //Else load levels as usually
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));            
        }
    }

    public void RestartCurrentLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
