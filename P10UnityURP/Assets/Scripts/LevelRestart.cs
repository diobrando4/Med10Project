using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRestart : MonoBehaviour
{
    public LevelLoader levelLoader;
    public PlayerHealthManager player;
    public Ally ally1, ally2;

    // Start is called before the first frame update
    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        player = GameObject.Find("Player").GetComponent<PlayerHealthManager>();
        ally1 = GameObject.Find("AllyBlueBot").GetComponent<Ally>();
        ally2 = GameObject.Find("AllyOrangeBot").GetComponent<Ally>();
    }

    // Update is called once per frame
    void Update()
    {
        // restarts the current level loaded when player and both allies are dead
        if (player.isPlayerDead == true && ally1.isAllyDead == true && ally2.isAllyDead == true)
        {
            //Debug.Log("everyone is dead, restarting level");
            levelLoader.RestartCurrentLevel();
        }
    }
}
