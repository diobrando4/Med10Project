using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
