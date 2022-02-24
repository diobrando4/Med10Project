using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public int playerHealth = 3;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
            //Debug.Log("YOU DIED");
        }
    }

    public void HurtPlayer(int damageTaken)
    {
        currentHealth -= damageTaken;
        // ?.fillAmount = ?;
    }
}
