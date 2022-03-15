using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyHealthUi : MonoBehaviour
{
    public Ally allyScript;
    public Image healthBarFill;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBarFill.fillAmount = allyScript.currHealth / allyScript.maxHealth;

        /*
        if (allyScript.currHealth <= 0)
        {
            gameObject.SetActive(false);
        }
        */
    }
}
