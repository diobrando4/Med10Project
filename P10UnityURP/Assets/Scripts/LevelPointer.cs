using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for the raw image

public class LevelPointer : MonoBehaviour
{
    public RawImage arrow;

    // Start is called before the first frame update
    void Start()
    {
        arrow = GameObject.Find("CanvasHealthBars/HolderArrow/RawImageArrow").GetComponent<RawImage>();
        arrow.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
