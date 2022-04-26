using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    //public GameObject[] allEnemies;
    public List<GameObject> remainingEnemies;

    public bool areAllEnemiesDead = false;

    // Start is called before the first frame update
    void Start()
    {
        //remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        //List<GameObject>remainingEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        remainingEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        //areAllEnemiesDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingEnemies.Count == 0)
        {
            //Debug.Log("all enemies are dead");
            if (FindObjectOfType<SoundManager>())
            {
                FindObjectOfType<SoundManager>().SoundPlay("DoorOpen");
            }
            areAllEnemiesDead = true;
            Destroy(gameObject);
        }
        remainingEnemies.RemoveAll(item => item == null);
    }

    public void RemoveFromList(GameObject _enemy)
    {
        remainingEnemies.Remove(_enemy);
    }
}
