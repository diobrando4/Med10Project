using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealthManager : MonoBehaviour
{
    public int zombieHealth = 5;
    private int currentHealth;

    public ExitDoor exitDoor;

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
            // if we don't do this and there isn't an exit door; then zombies can't be destroyed and we'll get a lot of errors
            if (exitDoor != null)
            {
                // removes enemy from a list; to open the exit door when all enemies are dead
                exitDoor.remainingEnemies.Remove(gameObject);
            }
            Destroy(gameObject);
        }

        // this is just for testing
        /*
        if(Input.GetKeyDown(KeyCode.T))
        {
            if (exitDoor != null)
            {
                exitDoor.remainingEnemies.Remove(gameObject);
            }
            Destroy(gameObject);
        }
        */
    }

    public void HurtZombie(int damageTaken)
    {
        currentHealth -= damageTaken;
    }
}
