using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTest : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    private GameObject floatText;
    public Vector3 offset = new Vector3 (0,3,0);
    private Color parentColor;
    private bool test = true;



    // Start is called before the first frame update
    void Start()
    {
        parentColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowFloatingText();
            test = !test;
        }
        
    }

    void ShowFloatingText()
    {


        if (floatText != null)
        {
            Destroy(floatText);
        }

        floatText = (GameObject)Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        floatText.GetComponent<TMP_Text>().color = parentColor;
        floatText.transform.localPosition += offset;

        if(test)
        {
            floatText.GetComponent<TMP_Text>().text = "THIS IS A LOT OF ZOMBIES";
        }
        else if (!test)
        {
            floatText.GetComponent<TMP_Text>().text = "FOCK";
        }
    }
}
