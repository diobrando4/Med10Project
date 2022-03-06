using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITestScript : MonoBehaviour
{
    private GameObject ally1;
    private GameObject ally2;
    private GameObject player;

//Need to auto set this later
    public TMP_Text ally1dialogue;
    public TMP_Text ally2dialogue;
    public GameObject ally1DialogueBox;
    public GameObject ally2DialogueBox;

    public TextAsset textFile;
    public string[] textLines;
    public List<string> ally1Lines = new List<string>();
    public List<string> ally2Lines = new List<string>();

    public int currentLine;
    public int endAtLine;

    private bool isTyping = false;
    private bool cancelTyping = false;
    public float typeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ally1 = GameObject.Find("AllyBlueBot");
        ally2 = GameObject.Find("AllyOrangeBot");
        player = GameObject.Find("ThePlayer");    

        if(textFile != null) //If there is a text file
        {
            textLines = (textFile.text.Split('\n')); //Seperate per line

        }
        for (int i = 0; i < textLines.Length; i++) //Split textfile between ally1 and ally2 depending on marker [0] and [1]
        {
            if(textLines[i].Contains("[0]"))
            {
                textLines[i] = textLines[i].Replace("[0]", "");                
                ally1Lines.Add(textLines[i]);
                ally2Lines.Add(" ");
            }
            else if (textLines[i].Contains("[1]"))
            {
                textLines[i] = textLines[i].Replace("[1]", "");
                ally1Lines.Add(" ");
                ally2Lines.Add(textLines[i]);  
            }
        }
        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
    } // Start

    // Update is called once per frame
    void Update()
    {
        ally1dialogue.text = ally1Lines[currentLine];
        ally2dialogue.text = ally2Lines[currentLine];
        if (currentLine < endAtLine)
        {
            ally1DialogueBox.SetActive(true);
            ally2DialogueBox.SetActive(true);            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentLine += 1;
            }            
        }
        else if (currentLine == endAtLine)
        {
            ally1dialogue.text = ally1dialogue.text + "\n[End]";
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ally1DialogueBox.SetActive(false);
                ally2DialogueBox.SetActive(false);
                //Debug.Log(endAtLine);
            }            
        }

    } //Update

    void TypingText() // Experimental
    {
        if (!isTyping) //If text isnt being typed out
        {
            StartCoroutine(TextScroll("This is a test"));
        }
        else if (isTyping && !cancelTyping) //If text is currently being typed out, and it hasnt been canceled
        {
            cancelTyping = true;
        }
    }// TypingText

    IEnumerator TextScroll(string lineOfText) //Experimental
    {
        int letterTracker = 0;
        ally1dialogue.text = "";
        ally2dialogue.text = "";
        isTyping = true;
        cancelTyping = false;
        while (isTyping && !cancelTyping && (letterTracker < lineOfText.Length - 1))
        {
            ally1dialogue.text += lineOfText[letterTracker];
            ally2dialogue.text += lineOfText[letterTracker];
            letterTracker += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        ally1dialogue.text = lineOfText;
        ally2dialogue.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }// TextScroll

    IEnumerator ClearText(float clearTime)
    {

        yield return new WaitForSeconds(clearTime);
        
    }

    IEnumerator ProgressDialogue(float clearTime)
    {

        yield return new WaitForSeconds(clearTime);
        
    }
    
}
