using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllyHealthManager : MonoBehaviour
{
    public float allyMaxHealth = 6;
    public float allyCurrentHealth;

    //public Image healthBarFill;

    void Start()
    {
        allyCurrentHealth = allyMaxHealth;
    }

    void Update()
    {
        if (allyCurrentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void HurtAlly(float damageTaken)
    {
        allyCurrentHealth -= damageTaken;
        //Debug.Log(allyCurrentHealth);
        //healthBarFill.fillAmount = allyCurrentHealth / allyMaxHealth;
    }
}
