using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // used for the UI text level counter

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public TextMeshProUGUI levelText;

    void Start()
    {
        //Debug.Log(SceneManager.GetActiveScene().buildIndex);

        // if we put the line below inside of the if-statement then it doesn't find it!
        levelText = GameObject.Find("CanvasHealthBars/HolderLevelCounter/TextLevelCounter").GetComponent<TextMeshProUGUI>();
        if (levelText != null)
        levelText.text = "LEVEL: " + SceneManager.GetActiveScene().buildIndex.ToString();
    }

    public void LoadNextLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //LoadLevel(SceneManager.GetActiveScene().buildIndex + 1)
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void RestartCurrentLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
