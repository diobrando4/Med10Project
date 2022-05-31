using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    //public GameObject[] allEnemies;
    public List<GameObject> remainingEnemies;

    public bool areAllEnemiesDead = false; // what is this used for?

    private CombatDialogueManager combatDialogueManager;

    public Ally ally1;
    public Ally ally2;

    // Start is called before the first frame update
    void Start()
    {
        //remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        //List<GameObject>remainingEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        remainingEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        //areAllEnemiesDead = false;
        combatDialogueManager = GameObject.Find("CombatDialogueManager").GetComponent<CombatDialogueManager>();
        ally1 = GameObject.Find("AllyBlueBot").GetComponent<Ally>();
        ally2 = GameObject.Find("AllyOrangeBot").GetComponent<Ally>();
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingEnemies.Count == 0 && combatDialogueManager.isDialogueDone == true)
        {
                //Debug.Log("all enemies are dead");

                // we also need to make sure the door only opens when the main dialogue is finished, maybe it should be inside its own if-statement?
                
                if (ally1.isAllyDead == false && ally2.isAllyDead == false)
                {
                    if (FindObjectOfType<SoundManager>())
                {
                    FindObjectOfType<SoundManager>().SoundPlay("DoorOpen");
                }
                    areAllEnemiesDead = true; // what is this used for?
                    Destroy(gameObject);
                }
        }
            
        remainingEnemies.RemoveAll(item => item == null);
    }

    public void RemoveFromList(GameObject _enemy)
    {
        remainingEnemies.Remove(_enemy);
    }
}
