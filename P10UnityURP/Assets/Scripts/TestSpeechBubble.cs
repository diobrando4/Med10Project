using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestSpeechBubble : MonoBehaviour
{
    public RectTransform backgroundRectTransform;
    public TextMeshProUGUI textMeshPro;
    private Coroutine someCoroutine;
    
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (someCoroutine == null)
            {
            SetupText("Press F to Pay Respects");
            someCoroutine = StartCoroutine(RemoveText());
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (someCoroutine == null)
            {
            SetupText("GGGGG\nGGGGG");
            StartCoroutine(RemoveText());
            someCoroutine = StartCoroutine(RemoveText());
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (someCoroutine == null)
            {
            SetupText("HHHHH HHHHH\nHHHHH\nHHHHH HHHHH\nHHHHH");
            StartCoroutine(RemoveText());
            someCoroutine = StartCoroutine(RemoveText());
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetupText("");
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

    private float removeTextInSeconds = 2.0f;
    //private bool testing;

    // this is an issue with this; it doesn't reset/resume when it's already running!
    IEnumerator RemoveText()
    {
        //Debug.Log("Corourtine starts");
        yield return new WaitForSeconds(removeTextInSeconds);
        //Debug.Log("Corourtine ends");
        SetupText("");
        someCoroutine = null;
    }
}
