using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour
{
    private float timer = 600f; // 60f = 1 minute?
    private bool isLevelSwitched = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject); // don't destory this game object when loading a new scene
    }

    // Update is called once per frame
    void Update()
    {
        if (isLevelSwitched == false) // we should probably also use || for whenever the menu pause is open
        {
            timer -= Time.deltaTime; // 1f = 1 second?
            //Debug.Log(timer);
        }
        
        if (timer <= 0 && !isLevelSwitched)
        {
            isLevelSwitched = true;
            //Debug.Log("level is switched: " + isLevelSwitched);
            SceneManager.LoadScene("MenuEnd");
        }
    }
}