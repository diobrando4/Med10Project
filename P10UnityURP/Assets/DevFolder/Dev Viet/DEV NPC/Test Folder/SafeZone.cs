using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script works as a safezone for the player. Once the player has left the safezone 
//activate combat mode for ally and enemies

public class SafeZone : MonoBehaviour
{
    //[SerializeField]
    //private GameObject player;
    //[SerializeField]
    private GameObject allyBlue;
    //[SerializeField]
    private GameObject allyOrange;
    private List<GameObject> enemiesOnLevel;
    
    private bool combatTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        allyBlue = GameObject.Find("AllyBlueBot");
        allyOrange = GameObject.Find("AllyOrangeBot");
        enemiesOnLevel = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        ResetCombatMode();
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" && combatTriggered == false)
        {
            combatTriggered = true;
            //player.GetComponent<PlayerController>().inCombat;
            allyBlue.GetComponent<Ally>().inCombat = true;
            allyOrange.GetComponent<Ally>().inCombat = true;

            enemiesOnLevel[0].GetComponent<Zombie>().inCombat = true;

            transform.Find("Bubble").GetComponent<MeshRenderer>().enabled = false;

            Debug.Log("Player Left SafeZone");
        }
    }
    void ResetCombatMode()
    {
        if (combatTriggered == true && enemiesOnLevel.Count == 0)
        {
            combatTriggered = false;
            allyBlue.GetComponent<Ally>().inCombat = false;
            allyOrange.GetComponent<Ally>().inCombat = false;
        }
    }
}
