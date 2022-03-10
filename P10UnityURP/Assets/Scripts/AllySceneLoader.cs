using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllySceneLoader : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //transform.GetComponent<MeshRenderer>().material.color = Color.red;

            if (Input.GetMouseButtonDown(0))
            //if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Loading new level");
                //SceneManager.LoadScene("Gym01");
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
}
