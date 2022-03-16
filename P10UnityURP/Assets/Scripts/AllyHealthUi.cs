using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyHealthUi : MonoBehaviour
{
    public Ally allyScript;
    public Image healthBarFill;

    // Update is called once per frame
    void Update()
    {
        //healthBarFill.fillAmount = allyScript.currHealth / allyScript.maxHealth;
        //Debug.Log(allyScript.maxHealth);
        //Debug.Log(allyScript.currHealth);
    }

    void UpdateHealthBar()
    {
        healthBarFill.fillAmount = allyScript.currHealth / allyScript.maxHealth;
    }
}
