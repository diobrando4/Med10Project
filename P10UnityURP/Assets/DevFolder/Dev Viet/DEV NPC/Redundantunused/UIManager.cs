using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameObject ally1;

    private GameObject ally2;

    private GameObject player;

    public TMP_Text ally1dialogue;
    public TMP_Text ally2dialogue;
    public GameObject ally1DialogueBox;
    public GameObject ally2DialogueBox;

    public TextAsset textFileNarrative; //Need to make an interchangeable system
    private string[] textLinesNarrative;
    [SerializeField]
    private List<string> ally1NarrativeLines = new List<string>();
    [SerializeField]
    private List<string> ally2NarrativeLines = new List<string>();

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

        if(textFileNarrative != null) //If there is a text file
        {
            textLinesNarrative = (textFileNarrative.text.Split('\n')); //Seperate per line
        }
        for (int i = 0; i < textLinesNarrative.Length; i++) //Split textfile between ally1 and ally2 depending on marker [0] and [1]
        {
            if(textLinesNarrative[i].Contains("[0]"))
            {
                textLinesNarrative[i] = textLinesNarrative[i].Replace("[0]", "");                
                ally1NarrativeLines.Add(textLinesNarrative[i]);
                ally2NarrativeLines.Add(" ");
            }
            else if (textLinesNarrative[i].Contains("[1]"))
            {
                textLinesNarrative[i] = textLinesNarrative[i].Replace("[1]", "");
                
                ally2NarrativeLines.Add(textLinesNarrative[i]);  
                ally1NarrativeLines.Add(" ");
            }
        }
        if (endAtLine == 0)
        {
            endAtLine = textLinesNarrative.Length - 1;
        }

        StartCoroutine(TextScrollNarrative(ally1NarrativeLines[currentLine],ally2NarrativeLines[currentLine]));

    } // Start

    // Update is called once per frame
    void Update()
    {
        //ally1dialogue.text = ally1NarrativeLines[currentLine];
        //ally2dialogue.text = ally2NarrativeLines[currentLine];
        
        if (currentLine < endAtLine)
        {
            ally1DialogueBox.SetActive(true);
            ally2DialogueBox.SetActive(true);        
            if (Input.GetKeyDown(KeyCode.Return) && currentLine != endAtLine)
            {
                if (!isTyping)
                {

                    currentLine += 1;
                    StartCoroutine(TextScrollNarrative(ally1NarrativeLines[currentLine],ally2NarrativeLines[currentLine]));

                }
                else if (isTyping && !cancelTyping)
                {
                    cancelTyping = true;
                }
            }      
        }
        else if (Input.GetKeyDown(KeyCode.Return) && currentLine == endAtLine)
        {
            ally1DialogueBox.SetActive(false);
            ally2DialogueBox.SetActive(false);
        }    
    } //Update

    IEnumerator TextScrollNarrative(string lineOfText1, string lineOfText2) //Experimental
    {
        int letterTracker1 = 0;
        int letterTracker2 = 0;
        ally1dialogue.text = "";
        ally2dialogue.text = "";
        isTyping = true;
        cancelTyping = false;
        while (isTyping && !cancelTyping && (letterTracker1 < lineOfText1.Length - 1))
        {
            ally1dialogue.text += lineOfText1[letterTracker1];
            letterTracker1 += 1;
            yield return new WaitForSeconds(typeSpeed);
        }

        while (isTyping && !cancelTyping && (letterTracker2 < lineOfText2.Length - 1))
        {
            ally2dialogue.text += lineOfText2[letterTracker2];
            letterTracker2 += 1;
            yield return new WaitForSeconds(typeSpeed);
        }

        ally1dialogue.text = lineOfText1;
        if (currentLine == endAtLine)
        {
            ally1dialogue.text = ally1dialogue.text + "\n[End]";
            ally2dialogue.text = ally2dialogue.text + "\n[End]";
        }        
        ally2dialogue.text = lineOfText2;
        isTyping = false;
        cancelTyping = false;
    }// TextScrollAlly1
    
}
