using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject zombiePrefab; // can make this into an array to spawn different enemy types
    private float spawnTimeInSeconds = 2.0f;
    private GameObject clone;

    //[SerializeField]
    //private GameObject[] allEnemies;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTimer());
    }

    void LateUpdate()
    {
        // need to make an array here of all the enemies, or we can't use enemy numbers for dialogue
        // allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        // you really shouldn't be doing this is update, but i don't know how else to do it
    }

    // this function is never stopped
    IEnumerator SpawnTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTimeInSeconds);
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        clone = Instantiate(zombiePrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
    }
}
