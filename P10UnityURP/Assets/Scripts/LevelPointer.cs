using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for the raw image

public class LevelPointer : MonoBehaviour
{
    public RawImage arrow;
    public ExitDoor exitDoor;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        arrow = GameObject.Find("CanvasHealthBars/HolderArrow/RawImageArrow").GetComponent<RawImage>();
        arrow.gameObject.SetActive(false);
        exitDoor = GameObject.Find("CorridorLong/Door0").GetComponent<ExitDoor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (exitDoor.areAllEnemiesDead == true && isRunning == false)
        {
            //Debug.Log("all enemies have been killed");
            isRunning = true;
            //arrow.gameObject.SetActive(true);
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        while(true)
        {
            arrow.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            arrow.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
    }
}
