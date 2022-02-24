using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public GameObject player;
    public int playerHealth;
    public int playercurrHealth;
    public GameObject otherNPC;
    public bool isTriggered;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("ThePlayer");
        playerHealth = player.GetComponent<PlayerHealthManager>().playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
        playercurrHealth = player.GetComponent<PlayerHealthManager>().playerHealth;

        if (playercurrHealth == playerHealth-1) 
        {
            if(isTriggered == true)
            {
            print("Are you alright?");
            isTriggered = false;
            }
        }
    }
}
