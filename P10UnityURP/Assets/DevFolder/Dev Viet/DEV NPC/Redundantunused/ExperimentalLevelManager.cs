using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentalLevelManager : MonoBehaviour
{
    public GameObject levelExit;
    public GameObject exitDoor;
    public List<GameObject> remainingEnemies;

    //public GameObject player;

    public GameObject interactiveObject;
    public LevelLoader levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        remainingEnemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        //remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(remainingEnemies);
        OpenDoor(exitDoor);
    }

    void OpenDoor(GameObject door)
    {
        if(remainingEnemies.Count == 0)
        {
            Destroy(door);
        }
    }
    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player" && remainingEnemies.Count == 0)
        {
            levelLoader.LoadNextLevel();
        }
    }

}
