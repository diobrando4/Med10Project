using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllyLevelLoader : MonoBehaviour
{
    public LevelLoader levelLoader;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //transform.GetComponent<MeshRenderer>().material.color = Color.red;

            if (Input.GetMouseButtonDown(0))
            //if (Input.GetKeyDown(KeyCode.F))
            {
                //Debug.Log("Loading new level");
                //FadeToLevel("TestScene1");
                levelLoader.LoadNextLevel();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //transform.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    /*
    public Animator animator;

    void FadeToLevel(string level)
    {
        //Debug.Log("fading out");
        animator.SetTrigger("FadeOut");
    }

    void OnFadeComplete()
    {
        SceneManager.LoadScene("TestScene1");
    }
    */
}
