using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Text log that should be able to display what has been said recently by the GoodGuys, as well as color the text
public class TextLog : MonoBehaviour
{
    public List<string> lastDialogueSaid = new List<string>();
    public List<Color> lastDialogueColor = new List<Color>();
    public int maxNumberOfLines = 10;
    public TMP_Text textField;
    public Color color1;
    public Color color2;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.transform.Find("Canvas/TextLogBg/TextLogText"))
        {
            textField = gameObject.transform.Find("Canvas/TextLogBg/TextLogText").GetComponentInChildren<TMP_Text>();
        }        
    }

    // Update is called once per frame
    void Update()
    {
        //DEBUG=================================
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            //Insert a string into the first position of the list, basically pushing the oldest entry down the list.
            //Then apply a color tag to the string, with a hex color code converted from whatever Color is used, in this case "color1"
            //Currently prints out Time.deltaTime converted to string
            lastDialogueSaid.Insert(0,"<color=#"+ColorUtility.ToHtmlStringRGB(color1)+">"+Time.deltaTime.ToString()+"</color>");
            //lastDialogueColor.Insert(0, color1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            lastDialogueSaid.Insert(0,"<color=#"+ColorUtility.ToHtmlStringRGB(color2)+">"+Time.deltaTime.ToString()+"</color>");
            //lastDialogueColor.Insert(0, color2);
        }
        //======================================

        while (lastDialogueSaid.Count > maxNumberOfLines)
        {
            lastDialogueSaid.RemoveAt(lastDialogueSaid.Count-1);
            //lastDialogueColor.RemoveAt(lastDialogueColor.Count-1);
        }
        
        DisplayTextLog();
    }

    //Takes the list of strings of latest dialogues, then turn it into a single string divded by \n for new lines
    private void DisplayTextLog()
    {
        textField.text = string.Join("\n", lastDialogueSaid);
    }
}
