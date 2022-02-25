using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public GameObject player;
    
    public float playerHealth;
    public float playercurrHealth;

    public GameObject otherNPC;

    public bool isTriggered;

    public int response;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("ThePlayer");
        playerHealth = player.GetComponent<PlayerHealthManager>().playerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
        playercurrHealth = player.GetComponent<PlayerHealthManager>().playerMaxHealth;

        if (playercurrHealth == playerHealth-1) 
        {
            if(isTriggered == true)
            {
            print("Are you alright?");
            isTriggered = false;
            }
        }
    }

    void Dialogue()
    {
        switch(response)
        {
            case 1:
                print("Enemies!");
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

            default:
                print("None Selected");
                break;
        }
    }

}
