using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderQueueOverlay : MonoBehaviour
{
    public int renderQueuePos = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material.renderQueue = renderQueuePos;
    }

}
