using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITestScript : MonoBehaviour
{
    private GameObject ally1;
    private GameObject ally2;
    private GameObject player;

    public TMP_Text ally1dialogue;
    public TMP_Text ally2dialogue;

    // Start is called before the first frame update
    void Start()
    {
        ally1 = GameObject.Find("AllyBlueBot");
        ally2 = GameObject.Find("AllyOrangeBot");
        player = GameObject.Find("ThePlayer");

        //ally1dialogue.text="Test";
        //ally1dialogue.text="2";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ClearText(float clearTime)
    {

        yield return new WaitForSeconds(clearTime);
        
    }

    IEnumerator ProgressDialogue(float clearTime)
    {

        yield return new WaitForSeconds(clearTime);
        
    }
    
}
