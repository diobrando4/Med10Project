using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    private CombatDialogueManager combatDialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        combatDialogueManager = GetComponent<CombatDialogueManager>();

        if (PlayerPrefs.GetInt("GameVersion") == 1)
        {
            //Debug.Log("Dialogue is enabled");
            combatDialogueManager.toggleDialogue = true; // also includes the text-log
        }
        if (PlayerPrefs.GetInt("GameVersion") == 2)
        {
            //Debug.Log("Dialogue is disabled");
            combatDialogueManager.toggleDialogue = false; // also includes the text-log
        }
    }
}
