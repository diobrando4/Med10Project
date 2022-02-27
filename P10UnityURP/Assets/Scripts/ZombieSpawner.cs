using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject zombiePrefab; // can make this into an array to spawn different enemy types
    private float spawnTimeInSeconds = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTimer());
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
        GameObject clone = Instantiate(zombiePrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
    }
}
