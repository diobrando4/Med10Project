using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionVersionToggler : MonoBehaviour
{
    public BaseClassNPC[] typesOfNPC;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        typesOfNPC = FindObjectsOfType<BaseClassNPC>();
        
        if (PlayerPrefs.GetInt("GameVersion") == 1)
        {
            //Debug.Log("Dialogue is enabled");
            player.isOldVersion = true;
            foreach (BaseClassNPC npc in typesOfNPC)
            {
                npc.isOldVersion = true;
            }
        }
        if (PlayerPrefs.GetInt("GameVersion") == 2)
        {
            //Debug.Log("Dialogue is disabled");
            player.isOldVersion = false;
            foreach (BaseClassNPC npc in typesOfNPC)
            {
                npc.isOldVersion = false;
            }            
        }
    }
}
