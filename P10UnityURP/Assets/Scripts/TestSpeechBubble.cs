using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestSpeechBubble : MonoBehaviour
{
    public RectTransform backgroundRectTransform;
    public TextMeshProUGUI textMeshPro;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetupText("Press F to Pay Respects");
            //textMeshPro.text = "Press F to Pay Respects";
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetupText("GGGGG\nGGGGG");
            //textMeshPro.text = "Press F to Pay Respects";
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            SetupText("HHHHH HHHHH\nHHHHH\nHHHHH HHHHH");
            //textMeshPro.text = "Press F to Pay Respects";
        }
    }

    private Vector2 textSize;
    //private Vector2 paddingSize; // in case we want more space around the text

    void SetupText(string text)
    {
        textMeshPro.SetText(text);

        textMeshPro.ForceMeshUpdate();
        textSize = textMeshPro.GetRenderedValues(false);
        
        backgroundRectTransform.sizeDelta = textSize;

        //paddingSize = new Vector2(1, 1);
        //backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }
}
