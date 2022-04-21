using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    // in case we need multiple doors in the same scene
    //public string nextLevel;

    public LevelLoader levelLoader;

    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("loading next level");
            //SceneManager.LoadScene(nextLevel);
            levelLoader.LoadNextLevel();
        }
    }
}
