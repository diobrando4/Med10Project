using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AllyDialogue : MonoBehaviour
{
    public GameObject playerObject;
    public float playerMaxHp;
    public float playerCurrHp;

    //public GameObject otherNPC;

    public bool isTriggered = false;
    public bool isPlayerHurt = false;

    public int response = 0;

    // for the speech bubbles
    public RectTransform backgroundRectTransform;
    public TextMeshProUGUI textMeshPro;
    private Coroutine someCoroutine;

    private Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("ThePlayer");
        //playerHealth = playerObject.GetComponent<PlayerHealthManager>().playerMaxHealth;
        playerMaxHp = playerObject.GetComponent<PlayerHealthManager>().playerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // not sure why the text seems to follow in player controller, but it doesn't seem to do that here
        backgroundRectTransform.LookAt(backgroundRectTransform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);

        // GetComponent is not recommended in update as it is quite expensive
        playerCurrHp = playerObject.GetComponent<PlayerHealthManager>().playerCurrentHealth;

        if (playerCurrHp <= 0)
        {
            if (someCoroutine == null)
            {
            SetupText("Commander down!");
            someCoroutine = StartCoroutine(RemoveText());
            }
        }

        if (playerCurrHp == playerMaxHp-1)
        {
            if (someCoroutine == null)
            {
            SetupText("Are you okay?");
            someCoroutine = StartCoroutine(RemoveText());
            }
        }

        /*
        if (isPlayerHurt = playerCurrHp == playerMaxHp -1 ? true : false)
        {
            Debug.Log("Whatever");
        }
        */
        /*
        if (playerCurrHp == playerMaxHp-1)
        {
            isPlayerHurt = true;
            Debug.Log(isPlayerHurt);
        }
        if (isPlayerHurt == true)
        {
            Debug.Log("This should only show once!");
            //response = 1;
            isPlayerHurt = false;
            Debug.Log(isPlayerHurt);
            /*
            isTriggered = true;
            Debug.Log(isTriggered);
            if(isTriggered == true)
            {
                response = 1;
                isTriggered = false;
                Debug.Log(isTriggered);
            }
            */
        //}

        /*
        switch(response)
        {
            case 1:
                Debug.Log("Enemies!");
                break;
            
            case 2:
                print("All Clear!");
                break;

            case 3:
                print("Are you alright?");
                break;                

            case 4:
                print("Careful!");
                break;

            case 5:
                print("Ouch!");
                break;

            case 6:
                print("Im down!");
                break;
            case 7:
                print("Commander down!");
                break;
        }
        */
    } // Update

    private Vector2 textSize;

    void SetupText(string text)
    {
        textMeshPro.SetText(text);

        textMeshPro.ForceMeshUpdate();
        textSize = textMeshPro.GetRenderedValues(false);
        
        backgroundRectTransform.sizeDelta = textSize;
    }

    private float removeTextInSeconds = 2.0f;

    IEnumerator RemoveText()
    {
        //Debug.Log("Corourtine starts");
        yield return new WaitForSeconds(removeTextInSeconds);
        //Debug.Log("Corourtine ends");
        SetupText("");
        someCoroutine = null;
    }
}
