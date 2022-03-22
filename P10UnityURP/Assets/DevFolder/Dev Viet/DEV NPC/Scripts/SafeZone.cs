using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script works as a safezone for the player. Once the player has left the safezone 
//activate combat mode for ally and enemies

public class SafeZone : MonoBehaviour
{
    private GameObject allyBlue;
    private GameObject allyOrange;
    [SerializeField]
    private List<BaseClassNPC> actors; //All obj on scene that uses the baseclass
    //private Collider ownCollider;

    private bool combatTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        allyBlue = GameObject.Find("AllyBlueBot");
        allyOrange = GameObject.Find("AllyOrangeBot");
        actors = new List<BaseClassNPC>(Object.FindObjectsOfType<BaseClassNPC>());
    }

    // Update is called once per frame
    void Update()
    {
        ResetCombatMode();
    }
    //When the player leaves exits trigger and combat hasnt been triggered yet, turn all objects that uses the BaseClassNPC or a child of it, to inCombat == true
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" && combatTriggered == false)
        {
            combatTriggered = true;

            for (int i = 0; i < actors.Count; i++) 
            {
                actors[i].inCombat = true;
            }
            transform.GetComponent<MeshRenderer>().enabled = false; //Disable the bubble mesh
            transform.GetComponent<Collider>().enabled = false; //Disable collider so it wont retrigger unintentionally
            //Debug.Log("Player Left SafeZone");
        }
    }
    //Once there is no enemies left, reset Ally back to inCombat == false
    void ResetCombatMode()
    {
        if (combatTriggered == true && actors.Count == 0)
        {
            //combatTriggered = false;
            allyBlue.GetComponent<Ally>().inCombat = false;
            allyOrange.GetComponent<Ally>().inCombat = false;
        }
    }//ResetCombatMode
}
