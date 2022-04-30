using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("GameVersion") == 1)
        {
            //
        }
        if (PlayerPrefs.GetInt("GameVersion") == 2)
        {
            //
        }
    }
}
