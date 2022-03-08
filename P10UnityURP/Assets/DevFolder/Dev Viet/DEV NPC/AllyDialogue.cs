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
    //private Coroutine currentCoroutine;
    //private Coroutine previousCoroutine;

    [SerializeField]
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
        // this needs to be changed, since this only turns the text towards the cam, not the canvas!
        backgroundRectTransform.LookAt(backgroundRectTransform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);

        // GetComponent is not recommended in update as it is quite expensive
        playerCurrHp = playerObject.GetComponent<PlayerHealthManager>().playerCurrentHealth;
        Dialogue();
        // if (playerCurrHp <= 0)
        // {
        //      if (currentCoroutine == null)
        //     {
        //     //SetupText("Commander down!");
        //     currentCoroutine = StartCoroutine(RemoveText("", 2f));
        //     } */
        //     DisplayText("Player Dead!", 2f);
        //     print("Dead");
        // }

        // if (playerCurrHp == playerMaxHp-1)
        // {
        //      if (currentCoroutine == null)
        //     {
        //     //SetupText("Are you okay?");
        //     currentCoroutine = StartCoroutine(RemoveText("", 2f));
        //     } */
        //     DisplayText("Player Damaged", 2f);

        // }

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
    private bool triggerDialogue = false;

    void Dialogue()
    {
        float playerHPTracker;
        playerHPTracker = playerMaxHp;
        //IEnumerator c = ClearText(3);

        if (playerCurrHp <= 0)
        {   
            StopAllCoroutines();     
            //Do something
            //SetupText("Doed");
            //StartCoroutine(c);
        }

        
        if (playerCurrHp == playerHPTracker-1)
        {
            if (triggerDialogue == false)
            {
                playerHPTracker = playerHPTracker-1;
                // Do Something
                StopAllCoroutines();
                //SetupText("Hovsa?");
                //Debug.Log(playerCurrHp + " " + playerHPTracker);
                StartCoroutine(ClearText("Hovsa",3));  
            }
                      
        }
    } 


    private Vector2 textSize;
    void SetupText(string text)
    {
        textMeshPro.SetText(text);

        textMeshPro.ForceMeshUpdate();
        textSize = textMeshPro.GetRenderedValues(false);
        
        backgroundRectTransform.sizeDelta = textSize;  
    } 

    IEnumerator ClearText(string text, float removeTextInSeconds)
    {
        triggerDialogue = true;
        SetupText(text);
        //Debug.Log(gameObject + "Corourtine starts");
        yield return new WaitForSeconds(removeTextInSeconds);
        //text = " ";

        SetupText("");
        //Debug.Log("END");
        triggerDialogue = false;            
    }
}
