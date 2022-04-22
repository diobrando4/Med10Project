using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // used for the UI pro mesh text level counter
using UnityEngine.SceneManagement;

public class LevelCounter : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    // Start is called before the first frame update
    void Start()
    {
        // if we put the line below inside of the if-statement then it doesn't find it!
        levelText = GameObject.Find("CanvasHealthBars/HolderLevelCounter/TextLevelCounter").GetComponent<TextMeshProUGUI>();
        if (levelText != null)
        levelText.text = "LEVEL: " + SceneManager.GetActiveScene().buildIndex.ToString();
    }
}
