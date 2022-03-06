using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTxtImporter : MonoBehaviour
{
    public TextAsset textFile;
    public string[] textLines;
    public List<string> ally1Lines = new List<string>();
    public List<string> ally2Lines = new List<string>();
    //public string[] ally1Lines;
    //public string[] ally2Lines;

    // Start is called before the first frame update
    void Start()
    {
        if(textFile != null)
        {
            textLines = (textFile.text.Split('\n')); //Seperate per line

        }
        for (int i = 0; i < textLines.Length; i++) 
        {
            if(textLines[i].Contains("[0]"))
            {
                ally1Lines.Add(textLines[i]);
                ally2Lines.Add(" ");
            }
            else if (textLines[i].Contains("[1]"))
            {
                ally1Lines.Add(" ");
                ally2Lines.Add(textLines[i]);
            }
        }
    }
}
