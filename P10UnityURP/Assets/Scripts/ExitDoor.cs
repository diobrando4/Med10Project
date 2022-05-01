using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    //public GameObject[] allEnemies;
    public List<GameObject> remainingEnemies;

    public bool areAllEnemiesDead = false; // what is this used for?

    private CombatDialogueManager combatDialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        //remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        //List<GameObject>remainingEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        remainingEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        //areAllEnemiesDead = false;
        combatDialogueManager = GameObject.Find("CombatDialogueManager").GetComponent<CombatDialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingEnemies.Count == 0 && combatDialogueManager.isDialogueDone == true)
        {
            //Debug.Log("all enemies are dead");

            // we also need to make sure the door only opens when the main dialogue is finished, maybe it should be inside its own if-statement?
            
            if (FindObjectOfType<SoundManager>())
            {
                FindObjectOfType<SoundManager>().SoundPlay("DoorOpen");
            }
            areAllEnemiesDead = true; // what is this used for?
            Destroy(gameObject);
        }
        remainingEnemies.RemoveAll(item => item == null);
    }

    public void RemoveFromList(GameObject _enemy)
    {
        remainingEnemies.Remove(_enemy);
    }
}
