using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    //Assigned in MenuStart.cs
    //public int currentVerNum; 
    //public int startingVerNum;

    public void LoadNextLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //LoadLevel(SceneManager.GetActiveScene().buildIndex + 1)
        if (SceneManager.GetActiveScene().name == "Level07") //If we are at the boss room scene
        {
            //Debug.Log("This is the boss level of version "+PlayerPrefs.GetInt("GameVersion"));            
            if (PlayerPrefs.GetInt("GameVersion") != PlayerPrefs.GetInt("StartingGameVersion")) //and if the current GameVersion is not the same as the one that was originally assigned
            {
                //Debug.Log("Trying to load EndFinal");
                StartCoroutine(LoadLevel(10)); //Load EndFinal
            }
            else//Else load levels as usually
            {
                StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
            }
        }
        else//Else load levels as usually
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
