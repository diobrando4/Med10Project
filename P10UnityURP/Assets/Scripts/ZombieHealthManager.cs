using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealthManager : MonoBehaviour
{
    public int zombieHealth = 5;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = zombieHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // if the zombie reaches 0 health then it gets destroyed
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void HurtZombie(int damageTaken)
    {
        currentHealth -= damageTaken;
    }
}
